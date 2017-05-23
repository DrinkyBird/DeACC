﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Csnxs.DeACC
{
    partial class AcsFile
    {
        private Stream InputStream;
        private Stream OutputStream;
        public AcsFormat Format { get; private set; }

        private int DirOffset;
        private bool HexenFaked = false;

        public Dictionary<int, AcsScript> Scripts = new Dictionary<int, AcsScript>();
        public List<string> StringTable = new List<string>();

        public AcsFile(Stream stream) : this(stream, AcsFormatIdentifier.IdentifyFormat(stream)) { }

        private int ParameterCounter = 0;

        public AcsFile(Stream stream, AcsFormat format)
        {
            InputStream = stream;
            Format = format;

            stream.Seek(4, SeekOrigin.Begin);
                            
            BinaryReader reader = new BinaryReader(InputStream);
            int dirOffset = reader.ReadInt32();

            if (format == AcsFormat.ZDoomLower || format == AcsFormat.ZDoomUpper)
            {
                byte[] sig = new byte[4];
                stream.Seek(dirOffset - 4, SeekOrigin.Begin);
                reader.Read(sig, 0, 4);

                if (sig[0] == 'A' && sig[1] == 'C' && sig[2] == 'S' && (sig[3] == 'E' || sig[3] == 'e'))
                {
                    HexenFaked = true;
                    stream.Seek(dirOffset - 8, SeekOrigin.Begin);
                    dirOffset = reader.ReadInt32();
                }
            }

            stream.Seek(dirOffset, SeekOrigin.Begin);
            DirOffset = dirOffset;

            if (format == AcsFormat.Acs95)
            {
                ReadAcs95(ref reader);
            }
            else
            {
                ReadZDoomAcs(ref reader);
            }

            reader.Dispose();
        }

        private void ReadAcs95(ref BinaryReader reader)
        {
            int numPointers = reader.ReadInt32();
            for (int i = 0; i < numPointers; i++)
            {
                int number = reader.ReadInt32();
                int pointer = reader.ReadInt32();
                int argc = reader.ReadInt32();

                int id = number % 1000;
                int typeNum = number / 1000;
                ScriptType type = (ScriptType) typeNum;

                Console.WriteLine("Script " + id + " is of type " + type);

                long pos = InputStream.Position;

                InputStream.Seek(pointer, SeekOrigin.Begin);
                
                InputStream.Seek(pos, SeekOrigin.Begin);
            }

            int stringCount = reader.ReadInt32();
            for (int i = 0; i < stringCount; i++)
            {
                int pointer = reader.ReadInt32();
                long pos = InputStream.Position;

                InputStream.Seek(pointer, SeekOrigin.Begin);
                StringTable.Add(ReadString());

                InputStream.Seek(pos, SeekOrigin.Begin);
            }
        }

        public void Disassemble(Stream outputStream)
        {
            OutputStream = outputStream;

            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();

            WriteLine("// Disassembled by " + assemblyName.Name + " version " + assemblyName.Version);
            WriteLine("//");
            WriteLine("// String Table:");
            for (int i = 0; i < StringTable.Count; i++)
            {
                WriteLine("//   "+ i + " " + StringTable[i]);
            }

            WriteLine();

            if (Format == AcsFormat.Acs95)
            {
                WriteLine("#include \"common.acs\"");
            }
            else
            {
                WriteLine("#include \"zcommon.acs\"");
            }

            WriteLine("// ================================================== MAP ARRAYS");
            WriteLine();

            foreach (KeyValuePair<int, int[]> pair in MapArrays)
            {
                int index = pair.Key;
                int[] array = pair.Value;

                Write($"int _a_{index:x4}_[{array.Length}] = " + "{");
                for (int i = 0; i < array.Length; i++)
                {
                    Write(array[i].ToString());

                    if (i < array.Length - 1)
                    {
                        Write(", ");
                    }
                }
                WriteLine("};");
            }

            WriteLine();
            WriteLine("// ================================================== MAP VARIABLES");
            WriteLine();

            foreach (KeyValuePair<int, int> pair in MapVariables)
            {
                int index = pair.Key;
                int value = pair.Value;

                WriteLine($"int _m_{index:x4}_ = {value};");
            }

            WriteLine();
            WriteLine("// ================================================== FUNCTIONS");
            WriteLine();

            foreach (KeyValuePair<string, AcsFunction> pair in FunctionMap)
            {
                string name = pair.Key;
                AcsFunction function = pair.Value;

                string returnType = (function.Returns ? "int" : "void");
                string args;

                if (function.NumberOfArguments > 1)
                {
                    args = "";
                    for (int i = 0; i < function.NumberOfArguments; i++)
                    {
                        args += $"int _p_{ParameterCounter:x4}_";
                        ParameterCounter++;

                        if (i < function.NumberOfArguments - 1)
                        {
                            args += ", ";
                        }
                    }
                }
                else
                {
                    args = "void";
                }

                WriteLine($"function {returnType} {name} ({args})");
                WriteLine("{");
                WriteLine();
                WriteLine("}");
                WriteLine();
            }

            WriteLine();
            WriteLine("// ================================================== SCRIPTS");
            WriteLine();

            foreach (KeyValuePair<int, AcsScript> pair in Scripts)
            {
                int number = pair.Key;
                AcsScript script = pair.Value;

                string args = "void";
                string type = "";
                string flags = "";

                if (script.NumberOfArguments > 0)
                {
                    args = "";
                    for (int i = 0; i < script.NumberOfArguments; i++)
                    {
                        args += $"int _p_{ParameterCounter:x4}_";
                        ParameterCounter++;

                        if (i < script.NumberOfArguments - 1)
                        {
                            args += ", ";
                        }
                    }
                }

                if (script.Type != ScriptType.Closed)
                {
                    type = script.Type.ToString().ToUpper();
                }

                if ((script.Flags & (int)ScriptFlags.Net) != 0)
                {
                    flags += "NET ";
                }

                if ((script.Flags & (int)ScriptFlags.Clientside) != 0)
                {
                    flags += "CLIENTSIDE ";
                }

                string typeSpace = (script.Type != ScriptType.Closed ? " " : "");

                WriteLine($"Script {number} ({args}){typeSpace}{type} {flags}");

                WriteLine();
                WriteLine("{");
                WriteLine("");
                WriteLine("}");
                WriteLine();
            }
        }

        private void Write(string line = "")
        {
            byte[] bytes = Encoding.UTF8.GetBytes(line);
            OutputStream.Write(bytes, 0, bytes.Length);
        }

        private void WriteLine(string line = "")
        {
            Write(line + "\n");
        }

        private string ReadString()
        {
            long pos = InputStream.Position;
            int strLen = 0;
            while (InputStream.ReadByte() != 0)
            {
                strLen++;
            }

            InputStream.Seek(pos, SeekOrigin.Begin);

            byte[] array = new byte[strLen];
            for (int c = 0; c < strLen; c++)
            {
                array[c] = (byte) InputStream.ReadByte();
            }

            string s = Encoding.ASCII.GetString(array);
            return s;
        }
    }
}
