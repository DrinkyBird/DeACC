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
            Console.WriteLine("\t" + numArrays + " arrays:");

            for (int i = 0; i < numArrays; i++)
            {
                int num = reader.ReadInt32();
                int arraySize = reader.ReadInt32();

                MapArrays[num] = new int[arraySize];

                Console.WriteLine("\t\t" + num + ": " + arraySize);
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
            Console.WriteLine("\t#" + index);
            for (int i = 0; i < num; i++)
            {
                int v = reader.ReadInt32();
                Console.WriteLine("\t\t" + i + ": " + v);
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
    }
}
