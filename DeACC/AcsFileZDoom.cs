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

        private Dictionary<int, int[]> MapArrays = new Dictionary<int, int[]>();
        private Dictionary<int, ImportedArray> ImportedMapArrays = new Dictionary<int, ImportedArray>();
        private Dictionary<int, int> MapVariables = new Dictionary<int, int>();

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
                else if (name == "MINI") ReadMINI(size, ref reader);
                else if (name == "SPTR") ReadSPTR(size, ref reader);
                else if (name == "SFLG") ReadSPTR(size, ref reader);
                else if (name == "STRL") ReadSTRL(size, ref reader);
                else if (name == "STRE") ReadSTRE(size, ref reader);
                else
                {
                    Program.PrintError("Not implemented.");
                    //throw new NotImplementedException(Format + " chunk " + name + " (" + size + ")");
                }

                InputStream.Seek(pos + size, SeekOrigin.Begin);
            }
        }

        private void ReadARAY(int size, ref BinaryReader reader)
        {
            int numArrays = size / 8;

            for (int i = 0; i < numArrays; i++)
            {
                int num = reader.ReadInt32();
                int arraySize = reader.ReadInt32();

                MapArrays[num] = new int[arraySize];
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
            for (int i = 0; i < num; i++)
            {
                int v = reader.ReadInt32();
                MapArrays[index][i] = v;
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

                MapVariables[index] = value;
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

                    Console.WriteLine($"Script {number} {type}, {argc} args, 0x{address:x8}");

                    AcsScript script = new AcsScript(number, type, argc);
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
                    Console.WriteLine($"Script {number} {type}, {argc} args, 0x{address:x8}");

                    AcsScript script = new AcsScript(number, type, argc);
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

        private void ReadSTRL(int size, ref BinaryReader reader)
        {
            long baseOffset = InputStream.Position;
            reader.ReadInt32();

            int numStrings = reader.ReadInt32();

            reader.ReadInt32();

            for (int i = 0; i < numStrings; i++)
            {
                int pointer = reader.ReadInt32();
                long pos = InputStream.Position;

                InputStream.Seek(baseOffset + pointer, SeekOrigin.Begin);
                StringTable.Add(ReadString());
                InputStream.Seek(pos, SeekOrigin.Begin);
            }
        }

        private void ReadSTRE(int size, ref BinaryReader reader)
        {
            long baseOffset = InputStream.Position;
            reader.ReadInt32();

            int numStrings = reader.ReadInt32();

            reader.ReadInt32();

            for (int i = 0; i < numStrings; i++)
            {
                int pointer = reader.ReadInt32();
                long pos = InputStream.Position;

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
                    array[j] = (byte) (reader.ReadByte() ^ (key + (j / 2)));
                }

                StringTable.Add(Encoding.ASCII.GetString(array));

                InputStream.Seek(pos, SeekOrigin.Begin);
            }
        }
    }
}
