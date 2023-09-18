using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DeACC
{
    struct Acs95ScriptDef
    {
        public int Number, Pointer, Args;
    }

    public partial class AcsFile
    {
        private Stream InputStream;
        private Stream OutputStream;
        public AcsFormat Format { get; private set; }

        private int DirOffset;
        private bool HexenFaked = false;

        public Dictionary<int, AcsScript> Scripts = new();
        public List<string> StringTable = new();

        public AcsFile(Stream stream) : this(stream, AcsFormatIdentifier.IdentifyFormat(stream)) { }

        private int ParameterCounter = 0;

        public AcsFile(Stream stream, AcsFormat format, bool alternateAcs95ScriptSizeMethod = false)
        {
            InputStream = stream;
            Format = format;

            stream.Seek(4, SeekOrigin.Begin);
                            
            BinaryReader reader = new(InputStream);
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
                ReadAcs95(ref reader, alternateAcs95ScriptSizeMethod);
            }
            else
            {
                ReadZDoomAcs(ref reader);
            }

            reader.Dispose();
        }

        private void ReadAcs95(ref BinaryReader reader, bool alternateScriptSizeMethod)
        {
            List<Acs95ScriptDef> scripts = new();
            int firstStringAddress = (int)InputStream.Position;

            int numPointers = reader.ReadInt32();
            for (int i = 0; i < numPointers; i++)
            {
                int number = reader.ReadInt32();
                int pointer = reader.ReadInt32();
                int argc = reader.ReadInt32();

                scripts.Add(new()
                {
                    Number = number,
                    Pointer = pointer,
                    Args = argc
                });
            }

            int stringCount = reader.ReadInt32();
            for (int i = 0; i < stringCount; i++)
            {
                int pointer = reader.ReadInt32();
                long pos = InputStream.Position;

                InputStream.Seek(pointer, SeekOrigin.Begin);
                StringTable.Add(ReadString());

                InputStream.Seek(pos, SeekOrigin.Begin);

                if (i == 0)
                {
                    firstStringAddress = pointer;
                }
            }

            for (int i = 0; i < scripts.Count; i++)
            {
                var def = scripts[i];

                int id = def.Number % 1000;
                int typeNum = def.Number / 1000;
                ScriptType type = (ScriptType)typeNum;

                Console.WriteLine("Script " + id + " is of type " + type);
                long pos = InputStream.Position;

                AcsScript script = new(def.Number, type, GenerateArgumentNames(def.Args, type), def.Pointer);

                int len;

                if (alternateScriptSizeMethod)
                {
                    if (i < scripts.Count - 1)
                    {
                        len = scripts[i + 1].Pointer - def.Pointer;
                    }
                    else
                    {
                        len = firstStringAddress - def.Pointer;
                    }
                }
                else
                {
                    InputStream.Seek(def.Pointer, SeekOrigin.Begin);

                    do
                    {
                        AcsOpcode opcode = AcsInstruction.ReadOpcode(ref reader, true);
                        InputStream.Position += 4 * opcode.NumberOfArguments;
                        if (opcode.AsEnum() == OpcodeEnum.Terminate)
                        {
                            break;
                        }
                    } while (true);

                    len = (int)InputStream.Position - def.Pointer;
                }

                InputStream.Seek(def.Pointer, SeekOrigin.Begin);
                script.Code = AcsInstruction.ReadCode(Format, reader, len);

                InputStream.Seek(pos, SeekOrigin.Begin);

                Scripts[def.Number] = script;
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
                WriteLine($"//  {i,4} " + StringTable[i]);
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

            WriteLine();
            foreach (var library in Libraries)
            {
                WriteLine($"#import \"{library}\"");
            }
            WriteLine();

            WriteLine("// ================================================== MAP ARRAYS");
            WriteLine();

            if (ImportedMapArrays.Count > 0)
            {
                WriteLine($"/* Imported map arrays ({ImportedMapArrays.Count}:");
                foreach (KeyValuePair<int, AcsFile.ImportedArray> pair in ImportedMapArrays)
                {
                    int index = pair.Key;
                    AcsFile.ImportedArray array = pair.Value;

                    WriteLine($"/* imported - index {index:x4} */ int {array.Name}[{array.Size}];");
                }
                WriteLine("//*/");
            }

            foreach (var pair in MapArrays)
            {
                int index = pair.Key;
                AcsFile.MapArray mapArray = pair.Value;
                int[] array = mapArray.Values;
                
                string type = (mapArray.IsString ? "str" : "int");

                Write($"{type} {mapArray.Name}[{array.Length}] = " + "{");
                for (int i = 0; i < array.Length; i++)
                {
                    if (mapArray.IsString)
                    {
                        string s;
                        int sI = array[i];

                        if (sI < StringTable.Count)
                        {
                            s = StringTable[array[i]];
                        }
                        else
                        {
                            s = "(DeACC: Invalid string index " + sI + ")";
                        }
                        Write($"\"{s}\"");
                    }
                    else
                    {
                        Write(array[i].ToString());
                    }

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

            if (ImportedMapVariables.Count > 0)
            {
                WriteLine($"/* Imported map variables ({ImportedMapVariables.Count}:");
                foreach (KeyValuePair<int, string> pair in ImportedMapVariables)
                {
                    int index = pair.Key;
                    string name = pair.Value;

                    WriteLine($"int {name}; // imported - index {index:x4}");
                }
                WriteLine("//*/");
            }

            foreach (var pair in MapVariables)
            {
                int index = pair.Key;
                AcsFile.MapVariable var = pair.Value;
                
                string type = (var.IsString ? "str" : "int");

                WriteLine($"{type} {var.Name} = {var.Value};");
            }

            WriteLine();
            WriteLine("// ================================================== FUNCTIONS");
            WriteLine();

            foreach (KeyValuePair<string, AcsFunction> pair in FunctionMap)
            {
                string name = pair.Key;
                AcsFunction function = pair.Value;

                if (function.IsImported)
                {
                    continue;
                }

                string returnType = (function.Returns ? "int" : "void");
                string args;

                if (function.Arguments.Length > 0)
                {
                    args = "";
                    for (int i = 0; i < function.Arguments.Length; i++)
                    {
                        args += $"int {function.Arguments[i]}";

                        if (i < function.Arguments.Length - 1)
                        {
                            args += ", ";
                        }
                    }
                }
                else
                {
                    args = "void";
                }

                WriteLine($"// Pointer: {function.Pointer}; Size = {function.CodeSize}; ImportNum = {function.ImportNum}");
                WriteLine($"function {returnType} {name} ({args})");
                WriteLine("{");
                WriteCode(function.Code, null, function);
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

                string args = "(void)";
                string type = "";
                string flags = "";

                if (script.Arguments.Length > 0)
                {
                    args = "(";
                    for (int i = 0; i < script.Arguments.Length; i++)
                    {
                        args += $"int {script.Arguments[i]}";

                        if (i < script.Arguments.Length - 1)
                        {
                            args += ", ";
                        }
                    }
                    args += ")";
                } else if (script.Arguments.Length == 0 && script.Type != ScriptType.Closed)
                {
                    args = "";
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

                string argsSpace = (String.IsNullOrEmpty(args) ? "" : " ");
                string typeSpace = (script.Type != ScriptType.Closed ? " " : "");

                string name = (String.IsNullOrEmpty(script.Name) ? number.ToString() : $"\"{script.Name}\"");

                WriteLine($"// Pointer: {script.Pointer}; Size = {script.CodeSize}");
                WriteLine($"Script {name}{argsSpace}{args}{typeSpace}{type} {flags}");
                
                WriteLine("{");
                WriteCode(script.Code, script, null);
                WriteLine("}");
                WriteLine();
            }
        }

        private void WriteCode(AcsInstruction[] code, AcsScript script, AcsFunction function)
        {
            StringBuilder builder = new();
            string[] arguments = script != null ? script.Arguments : function.Arguments;
            
            for (int j = 0; j < code.Length; j++)
            {
                AcsInstruction instruction = code[j];
                AcsInstruction next = (j == code.Length - 1 ? null : code[j + 1]);

                builder.Append($"    /* {instruction.Offset,8} */ > {instruction.Opcode.Name} ");

                for (int i = 0; i < instruction.Arguments.Length; i++)
                {
                    if (i == 0)
                    {
                        if (MapVariables.ContainsKey(instruction.Arguments[0])
                            && AcsInstruction.OpcodesAreEqual(instruction.Opcode, OpcodeEnum.PushMapVar))
                        {
                            builder.Append(MapVariables[instruction.Arguments[0]].Name);
                        }
                        else if (AcsInstruction.OpcodesAreEqual(instruction.Opcode, OpcodeEnum.PushScriptVar)
                            && instruction.Arguments[i] < arguments.Length)
                        {
                            builder.Append(arguments[instruction.Arguments[i]]);
                        }
                        else if (AcsInstruction.OpcodesAreEqual(instruction.Opcode, OpcodeEnum.Call)
                                 || AcsInstruction.OpcodesAreEqual(instruction.Opcode, OpcodeEnum.CallDiscard))
                        {
                            if (instruction.Arguments[0] >= 0 && instruction.Arguments[0] < FunctionList.Count)
                            {
                                builder.Append(FunctionList[instruction.Arguments[0]].Name);
                            }
                            else
                            {
                                builder.Append(instruction.Arguments[0]).Append(" // unknown function!");
                            }
                        }
                        else
                        {
                            builder.Append(instruction.Arguments[i]);
                        }
                    }
                    else if (i == 1 && CheckOpcode(code, j, OpcodeEnum.CallFunc))
                    {
                        AcsBuiltIn builtIn = (AcsBuiltIn)instruction.Arguments[i];

                        builder.Append(Enum.GetName(builtIn));
                    }
                    else
                    {
                        builder.Append(instruction.Arguments[i]);
                    }

                    if (i < instruction.Arguments.Length - 1)
                    {
                        builder.Append(", ");
                    }
                }

                if (instruction.JumpTable != null)
                {
                    builder.Append("{ ");
                    
                    int n = 0;
                    foreach (var entry in instruction.JumpTable)
                    {
                        builder.Append($"{entry.Key} -> {entry.Value}");
                        if (n < instruction.JumpTable.Count - 1)
                        {
                            builder.Append(", ");
                        }
                        n++;
                    }

                    builder.Append(" }");
                }

                if (CheckOpcode(code, j, OpcodeEnum.PushByte) || CheckOpcode(code, j, OpcodeEnum.PushNumber))
                {
                    if (CheckOpcode(code, j + 1, OpcodeEnum.TagString) || CheckOpcode(code, j + 1, OpcodeEnum.PrintString)
                        || CheckOpcode(code, j + 2, OpcodeEnum.ThingSound))
                    {
                        int si = instruction.Arguments[0];
                        builder.Append($" // (String table index {si}): {StringTable[si]}");
                    }
                }

                WriteLine(builder.ToString());
                builder.Clear();
            }
        }

        private bool CheckOpcode(AcsInstruction[] code, int i, OpcodeEnum wanted)
        {
            if (i < 0 || i >= code.Length)
            {
                return false;
            }

            return AcsInstruction.OpcodesAreEqual(code[i].Opcode, wanted);
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

            InputStream.Seek(1, SeekOrigin.Current);

            string s = Encoding.ASCII.GetString(array);
            return s;
        }

        private string[] GenerateArgumentNames(int argc)
        {
            List<string> list = new();
            
            for (int i = 0; i < argc; i++)
            {
                list.Add($"_p_{ParameterCounter:x4}_");
                ParameterCounter++;
            }

            return list.ToArray();
        }

        private string[] GenerateArgumentNames(int argc, ScriptType scriptType)
        {
            if (scriptType == ScriptType.Event)
            {
                List<string> list = new();
                if (argc >= 1)
                {
                    list.Add("eventType");
                }
                for (int i = 1; i < argc; i++)
                {
                    list.Add($"arg{i}");
                }
                
                return list.ToArray();
            }

            return GenerateArgumentNames(argc);
        }
    }
}
