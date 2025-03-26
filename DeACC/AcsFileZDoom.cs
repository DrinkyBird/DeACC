using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DeACC
{
    partial class AcsFile
    {
        struct Chunk
        {
            public string Name;
            public long Position;
            public int Size;
        }
        
        struct ImportedArray
        {
            public int Size;
            public string Name;
        }

        struct MapVariable
        {
            public string Name;
            public int Value;
            public bool IsString;
        }

        struct MapArray
        {
            public int[] Values;
            public string Name;
            public bool IsString;
        }
        
        private List<Chunk> _chunks = new();

        private Dictionary<int, MapArray> MapArrays = new Dictionary<int, MapArray>();
        private Dictionary<int, bool> ArrayIsStringCache = new Dictionary<int, bool>();
        private Dictionary<int, ImportedArray> ImportedMapArrays = new Dictionary<int, ImportedArray>();
        private Dictionary<int, MapVariable> MapVariables = new Dictionary<int, MapVariable>();
        private List<AcsFunction> FunctionList = new List<AcsFunction>();
        private Dictionary<string, AcsFunction> FunctionMap = new Dictionary<string, AcsFunction>();
        private Dictionary<int, string> ImportedMapVariables = new Dictionary<int, string>();
        private List<string> Libraries = new List<string>();

        private void ReadZDoomAcs(ref BinaryReader reader)
        {
            byte[] nameBytes = new byte[4];

            while (InputStream.Position < InputStream.Length)
            {
                if (reader.ReadInt32() == DirOffset)
                {
                    // End of file.
                    break;
                }

                InputStream.Seek(InputStream.Position - 4, SeekOrigin.Begin);
                reader.Read(nameBytes, 0, 4);
                string name = Encoding.ASCII.GetString(nameBytes);
                int size = reader.ReadInt32();
                long pos = InputStream.Position;

                _chunks.Add(new Chunk { Name = name, Position = pos, Size = size });

                InputStream.Position += size;

                Console.WriteLine($"Chunk {name} ({size} bytes at {pos:x8})");
            }

            HandleChunks("ARAY", reader, (c, r) => ReadARAY(c, ref r));
            HandleChunks("AINI", reader, (c, r) => ReadAINI(c, ref r));
            HandleChunks("AIMP", reader, (c, r) => ReadAIMP(c, ref r));
            HandleChunks("ASTR", reader, (c, r) => ReadASTR(c, ref r));
            HandleChunks("FUNC", reader, (c, r) => ReadFUNC(c, ref r));
            HandleChunks("FNAM", reader, (c, r) => ReadFNAM(c, ref r));
            HandleChunks("MEXP", reader, (c, r) => ReadMEXP(c, ref r));
            HandleChunks("MINI", reader, (c, r) => ReadMINI(c, ref r));
            HandleChunks("MIMP", reader, (c, r) => ReadMIMP(c, ref r));
            HandleChunks("MSTR", reader, (c, r) => ReadMSTR(c, ref r));
            HandleChunks("SPTR", reader, (c, r) => ReadSPTR(c, ref r));
            HandleChunks("SNAM", reader, (c, r) => ReadSNAM(c, ref r));
            HandleChunks("SFLG", reader, (c, r) => ReadSFLG(c, ref r));
            HandleChunks("LOAD", reader, (c, r) => ReadLOAD(c, ref r));
            HandleChunks("STRL", reader, (c, r) =>
            {
                InputStream.Position = c.Position;
                StringTable.AddRange(ReadStringTable(ref r, false, true));
            });
            HandleChunks("STRE", reader, (c, r) =>
            {
                InputStream.Position = c.Position;
                StringTable.AddRange(ReadStringTable(ref r, true, true));
            });
            
            ReadCode(ref reader);
        }

        private void HandleChunks(string name, BinaryReader reader, Action<Chunk, BinaryReader> func)
        {
            foreach (var chunk in _chunks)
            {
                if (chunk.Name == name)
                {
                    func(chunk, reader);
                }
            }
        }

        private void ReadSNAM(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position;
            
            string[] names = ReadStringTable(ref reader, false, false);

            for (int i = 0; i < names.Length; i++)
            {
                AcsScript s = Scripts[-(i + 1)];
                s.Name = names[i];
            }
        }

        private void ReadLOAD(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position;
            
            long start = InputStream.Position;
            long end = start + chunk.Size;

            while (InputStream.Position < end)
            {
                Libraries.Add(ReadString());
            }
        }

        private void ReadMSTR(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position;
            
            int num = chunk.Size / 4;
            for (int i = 0; i < num; i++)
            {
                int index = reader.ReadInt32();
                MapVariable v = MapVariables[index];
                v.IsString = true;
                MapVariables[index] = v;
            }
        }

        private void ReadASTR(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position;
            
            int num = chunk.Size / 4;
            for (int i = 0; i < num; i++)
            {
                int index = reader.ReadInt32();

                if (MapArrays.ContainsKey(index))
                {
                    MapArray v = MapArrays[index];
                    v.IsString = true;
                    MapArrays[index] = v;
                }
                else
                {
                    ArrayIsStringCache[index] = true;
                }
            }
        }

        private void ReadMEXP(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position;
            
            string[] names = ReadStringTable(ref reader, false, false);

            for (int i = 0; i < MapVariables.Count; i++)
            {
                Console.WriteLine($"{i} = {names[i]}; ${MapVariables.Count}");
                if (MapVariables.ContainsKey(i))
                {
                    MapVariable v = MapVariables[i];
                    // v.Name = names[i];
                    MapVariables[i] = v;
                }
            }

            for (int i = MapVariables.Count; i < MapArrays.Count; i++)
            {
                Console.WriteLine($"{i} = {names[i]}; ${MapVariables.Count}");
                if (MapArrays.ContainsKey(i))
                {
                    MapArray v = MapArrays[i];
                    // v.Name = names[i];
                    MapArrays[i] = v;
                }
            }
        }

        private void ReadFUNC(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position;
            int numFuncs = chunk.Size / 8;

            for (int i = 0; i < numFuncs; i++)
            {
                int argc = reader.ReadByte();
                int varc = reader.ReadByte();
                bool returns = reader.ReadByte() == 0x01;
                byte importNum = reader.ReadByte();
                int address = reader.ReadInt32();
                
                AcsFunction func = new AcsFunction(GenerateArgumentNames(argc), varc, returns, importNum, address);

                FunctionList.Add(func);
            }
        }

        private void ReadCode(ref BinaryReader reader)
        {
            foreach (var script in Scripts)
            {
                int num = script.Key;
                AcsScript s = script.Value;

                s.CodeSize = FindClosestPointer(s.Pointer) - s.Pointer;

                InputStream.Seek(s.Pointer, SeekOrigin.Begin);
                s.Code = AcsInstruction.ReadCode(Format, reader, s.CodeSize);
            }

            foreach (var func in FunctionList)
            {
                func.CodeSize = FindClosestPointer(func.Pointer) - func.Pointer;

                InputStream.Seek(func.Pointer, SeekOrigin.Begin);
                func.Code = AcsInstruction.ReadCode(Format, reader, func.CodeSize);
            }
        }

        private int FindClosestPointer(int p)
        {
            const int def = Int32.MaxValue;
            int r = def;

            foreach (var pair in Scripts)
            {
                AcsScript script = pair.Value;
                int ptr = script.Pointer;

                if (p > ptr || p == ptr)
                {
                    continue;
                }

                r = Math.Min(ptr, r);
            }

            for (int i = 0; i < FunctionList.Count; i++)
            {
                int ptr = FunctionList[i].Pointer;

                if (p > ptr || p == ptr)
                {
                    continue;
                }

                r = Math.Min(ptr, r);
            }

            if (r == def && DirOffset > p)
            {
                r = DirOffset;
            }

            if (r == def)
            {
                r = (int) InputStream.Length - 1;
            }

            Console.WriteLine($"Closest to {p} is {r}");

            return r;
        }

        private void ReadFNAM(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position;
            
            string[] names = ReadStringTable(ref reader, false, false);

            for (int i = 0; i < names.Length; i++)
            {
                AcsFunction f = FunctionList[i];
                f.Name = names[i];
                FunctionMap[names[i]] = FunctionList[i];
            }
        }

        private void ReadARAY(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position;
            
            int numArrays = chunk.Size / 8;

            for (int i = 0; i < numArrays; i++)
            {
                int num = reader.ReadInt32();
                int arraySize = reader.ReadInt32();

                MapArray a = new MapArray();
                a.Values = new int[arraySize];
                a.Name = $"_a_{num:x4}_";
                a.IsString = false;

                if (ArrayIsStringCache.ContainsKey(num))
                {
                    a.IsString = true;
                }

                MapArrays[num] = a;
            }
        }

        private void ReadAINI(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position - 4;

            int numArrays = MapArrays.Count;

            if (numArrays < 1)
            {
                Program.PrintError("AINI found before ARAY!");
                return;
            }
            
            int num = (reader.ReadInt32() - 4) / 4;
            int index = reader.ReadInt32();
            MapArray b = MapArrays[index];
            for (int i = 0; i < num; i++)
            {
                int v = reader.ReadInt32();
                b.Values[i] = v;
            }
        }

        private void ReadAIMP(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position;
            int numArrays = reader.ReadInt32();

            for (int i = 0; i < numArrays; i++)
            {
                int number = reader.ReadInt32();

                ImportedArray array;
                array.Size = reader.ReadInt32();
                array.Name = ReadString();

                ImportedMapArrays[number] = array;
            }
        }

        private void ReadMINI(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position;
            
            int numVars = (chunk.Size / 4) - 1;
            int baseIndex = reader.ReadInt32();

            for (int i = 0; i < numVars; i++)
            {
                int index = baseIndex + i;
                int value = reader.ReadInt32();

                MapVariable v = new MapVariable();
                v.Name = $"_m_{index:d4}_";
                v.Value = value;
                v.IsString = false;

                MapVariables[index] = v;
            }
        }

        private void ReadMIMP(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position;
            
            long start = InputStream.Position;
            while ((InputStream.Position - start) < chunk.Size - 1)
            {
                ImportedMapVariables[reader.ReadInt32()] = ReadString();
            }
        }

        private void ReadSPTR(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position;
            
            if (HexenFaked)
            {
                int numScripts = chunk.Size / 8;

                for (int i = 0; i < numScripts; i++)
                {
                    short number = reader.ReadInt16();
                    ScriptType type = (ScriptType)reader.ReadByte();
                    int argc = reader.ReadByte();
                    int address = reader.ReadInt32();

                    AcsScript script = new AcsScript(number, type, GenerateArgumentNames(argc, type), address);
                    Scripts[number] = script;
                }
            }
            else
            {
                int numScripts = chunk.Size / 12;

                for (int i = 0; i < numScripts; i++)
                {
                    short number = reader.ReadInt16();
                    ScriptType type = (ScriptType)reader.ReadInt16();
                    int address = reader.ReadInt32();
                    int argc = reader.ReadInt32();

                    AcsScript script = new AcsScript(number, type, GenerateArgumentNames(argc, type), address);
                    Scripts[number] = script;
                }
            }
        }

        private void ReadSFLG(Chunk chunk, ref BinaryReader reader)
        {
            InputStream.Position = chunk.Position;
            
            int numScripts = chunk.Size / 8;

            for (int i = 0; i < numScripts; i++)
            {
                short number = reader.ReadInt16();
                int flags = reader.ReadInt16();

                Scripts[number].Flags = flags;
            }
        }

        private string[] ReadStringTable(ref BinaryReader reader, bool encrypted, bool wastedInts)
        {
            long baseOffset = InputStream.Position;
            if (wastedInts) reader.ReadInt32();

            int numStrings = reader.ReadInt32();

            if (wastedInts) reader.ReadInt32();

            string[] list = new string[numStrings];

            for (int i = 0; i < numStrings; i++)
            {
                int pointer = reader.ReadInt32();
                long pos = InputStream.Position;

                InputStream.Seek(baseOffset + pointer, SeekOrigin.Begin);

                string s;

                if (encrypted)
                {
                    int key = pointer * 157135;

                    InputStream.Seek(baseOffset + pointer, SeekOrigin.Begin);

                    int length = 0;
                    while ((byte)(reader.ReadByte() ^ (key + (length / 2))) != '\0')
                    {
                        length++;
                    }

                    byte[] array = new byte[length];

                    InputStream.Seek(baseOffset + pointer, SeekOrigin.Begin);
                    for (int j = 0; j < array.Length; j++)
                    {
                        array[j] = (byte)(reader.ReadByte() ^ (key + (j / 2)));
                    }

                    s = Encoding.ASCII.GetString(array);
                }
                else
                {
                    s = ReadString();
                }

                list[i] = s;

                InputStream.Seek(pos, SeekOrigin.Begin);
            }

            return list;
        }
    }
}
