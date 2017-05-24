using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csnxs.DeACC
{
    partial class AcsFile
    {
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

                Console.WriteLine("Chunk " + name + " (" + size + " bytes)");

                if (name == "ARAY") ReadARAY(size, ref reader);
                else if (name == "AINI") ReadAINI(size, ref reader);
                else if (name == "AIMP") ReadAIMP(size, ref reader);
                else if (name == "ASTR") ReadASTR(size, ref reader);
                else if (name == "FUNC") ReadFUNC(size, ref reader);
                else if (name == "FNAM") ReadFNAM(size, ref reader);
                else if (name == "MEXP") ReadMEXP(size, ref reader);
                else if (name == "MINI") ReadMINI(size, ref reader);
                else if (name == "MIMP") ReadMIMP(size, ref reader);
                else if (name == "MSTR") ReadMSTR(size, ref reader);
                else if (name == "SPTR") ReadSPTR(size, ref reader);
                else if (name == "SFLG") ReadSFLG(size, ref reader);
                else if (name == "STRL") StringTable.AddRange(ReadStringTable(ref reader, false, true));
                else if (name == "STRE") StringTable.AddRange(ReadStringTable(ref reader, true, true));
                else if (name == "LOAD") ReadLOAD(size, ref reader);
                else
                {
                    Program.PrintError("Not implemented.");
                    //throw new NotImplementedException(Format + " chunk " + name + " (" + size + ")");
                }

                InputStream.Seek(pos + size, SeekOrigin.Begin);
            }
        }

        private void ReadLOAD(int size, ref BinaryReader reader)
        {
            long start = InputStream.Position;

            while (InputStream.Position - start < size - 1)
            {
                Libraries.Add(ReadString());
            }
        }

        private void ReadMSTR(int size, ref BinaryReader reader)
        {
            int num = size / 4;
            for (int i = 0; i < num; i++)
            {
                int index = reader.ReadInt32();
                MapVariable v = MapVariables[index];
                v.IsString = true;
                MapVariables[index] = v;
            }
        }

        private void ReadASTR(int size, ref BinaryReader reader)
        {
            int num = size / 4;
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

        private void ReadMEXP(int size, ref BinaryReader reader)
        {
            string[] names = ReadStringTable(ref reader, false, false);

            for (int i = 0; i < MapVariables.Count; i++)
            {
                Console.WriteLine($"{i} = {names[i]}; ${MapVariables.Count}");
                if (MapVariables.ContainsKey(i))
                {
                    MapVariable v = MapVariables[i];
                    v.Name = names[i];
                    MapVariables[i] = v;
                }
            }

            for (int i = MapVariables.Count; i < MapArrays.Count; i++)
            {
                Console.WriteLine($"{i} = {names[i]}; ${MapVariables.Count}");
                if (MapArrays.ContainsKey(i))
                {
                    MapArray v = MapArrays[i];
                    v.Name = names[i];
                    MapArrays[i] = v;
                }
            }
        }

        private void ReadFUNC(int size, ref BinaryReader reader)
        {
            int numFuncs = size / 8;

            for (int i = 0; i < numFuncs; i++)
            {
                int argc = reader.ReadByte();
                int varc = reader.ReadByte();
                bool returns = reader.ReadByte() == 0x01;
                reader.ReadByte();
                int address = reader.ReadInt32();

                AcsFunction func = new AcsFunction(argc, varc, returns);
                FunctionList.Add(func);
            }
        }

        private void ReadFNAM(int size, ref BinaryReader reader)
        {
            string[] names = ReadStringTable(ref reader, false, false);
            Console.WriteLine("Read names");

            for (int i = 0; i < names.Length; i++)
            {
                FunctionMap[names[i]] = FunctionList[i];
            }
        }

        private void ReadARAY(int size, ref BinaryReader reader)
        {
            int numArrays = size / 8;

            for (int i = 0; i < numArrays; i++)
            {
                int num = reader.ReadInt32();
                int arraySize = reader.ReadInt32();

                MapArray a = new MapArray();
                a.Values = new int[arraySize];
                a.IsString = false;

                if (ArrayIsStringCache.ContainsKey(num))
                {
                    a.IsString = true;
                }

                MapArrays[num] = a;
            }
        }

        private void ReadAINI(int size, ref BinaryReader reader)
        {
            InputStream.Seek(InputStream.Position - 4, SeekOrigin.Begin);

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

        private void ReadAIMP(int size, ref BinaryReader reader)
        {
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

        private void ReadMINI(int size, ref BinaryReader reader)
        {
            int numVars = (size / 4) - 1;

            for (int i = 0; i < numVars; i++)
            {
                int index = reader.ReadInt32();
                int value = reader.ReadInt32();

                MapVariable v = new MapVariable();
                v.Name = null;
                v.Value = value;
                v.IsString = false;

                MapVariables[index] = v;
            }
        }

        private void ReadMIMP(int size, ref BinaryReader reader)
        {
            long start = InputStream.Position;
            while ((InputStream.Position - start) < size - 1)
            {
                ImportedMapVariables[reader.ReadInt32()] = ReadString();
            }
        }

        private void ReadSPTR(int size, ref BinaryReader reader)
        {
            if (HexenFaked)
            {
                int numScripts = size / 8;

                for (int i = 0; i < numScripts; i++)
                {
                    short number = reader.ReadInt16();
                    ScriptType type = (ScriptType)reader.ReadByte();
                    int argc = reader.ReadByte();
                    int address = reader.ReadInt32();

                    AcsScript script = new AcsScript(number, type, argc);

                    long pos = InputStream.Position;
                    InputStream.Seek(address, SeekOrigin.Begin);

                    script.Code = AcsInstruction.ReadCode(this, ref reader);

                    InputStream.Seek(pos, SeekOrigin.Begin);
                    Scripts[number] = script;
                }
            }
            else
            {
                int numScripts = size / 12;

                for (int i = 0; i < numScripts; i++)
                {
                    short number = reader.ReadInt16();
                    ScriptType type = (ScriptType)reader.ReadInt16();
                    int address = reader.ReadInt32();
                    int argc = reader.ReadInt32();

                    AcsScript script = new AcsScript(number, type, argc);

                    long pos = InputStream.Position;
                    InputStream.Seek(address, SeekOrigin.Begin);

                    script.Code = AcsInstruction.ReadCode(this, ref reader);

                    InputStream.Seek(pos, SeekOrigin.Begin);
                    Scripts[number] = script;
                }
            }
        }

        private void ReadSFLG(int size, ref BinaryReader reader)
        {
            int numScripts = size / 8;

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
