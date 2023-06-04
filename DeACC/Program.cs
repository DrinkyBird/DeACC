using System;
using System.IO;
using System.Text;
using CommandLine;

namespace DeACC
{
    class Program
    {
        #region Command Line Verbs
        
        [Verb("disassemble", HelpText = "Disassemble an ACS file")]
        class DisassembleOptions
        {
            [Option('o', "output-file", HelpText = "File to output the disassembled code to", Required = true)]
            public string OutputFile { get; set; }

            [Value(0, MetaName = "file", HelpText = "Compiled ACS object file", Required = true)]
            public string FileName { get; set; }

            [Option("alternate-acs-95-size-method", HelpText = "Uses an alternate method to determine the size of ACS95 scripts. This calculates the size of Hexen's scripts correctly, but might not work reliably.")]
            public bool UseAlternateAcs95SizeMethod { get; set; }
        }

        [Verb("export-opcodes", HelpText = "Exports the opcodes to a file", Hidden = true)]
        class ExportOpcodesOptions
        {
            [Option('o', "output-file", HelpText = "File to output the opcodes code to", Required = true)]
            public string OutputFile { get; set; }
        }

        #endregion

        static void Main(string[] args)
        {
            new Program(args);
        }

        private Program(string[] args)
        {
            int ret = CommandLine.Parser.Default.ParseArguments<DisassembleOptions, ExportOpcodesOptions>(args)
                .MapResult(
                    (DisassembleOptions opts) => Disassemble(opts),
                    (ExportOpcodesOptions opts) => ExportOpcodes(opts),
                errs => 1);
        }

        public static void PrintError(string line)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("[ERROR] " + line);
            Console.ResetColor();
        }

        private int Disassemble(DisassembleOptions options)
        {
            string path = Path.GetFullPath(options.FileName);
            if (!File.Exists(path))
            {
                PrintError("File does not exist: " + path);
                return 2;
            }

            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            AcsFormat format = AcsFormatIdentifier.IdentifyFormat(stream);

            if (format == AcsFormat.NotAcs)
            {
                stream.Dispose();
                PrintError(path + " is not an ACS file!");
                return 3;
            }

            Console.WriteLine("Detected format: " + format);

            string outputPath = Path.GetFullPath(options.OutputFile);
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            FileStream outputStream = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write);

            global::DeACC.AcsFile file = new global::DeACC.AcsFile(stream, format, alternateAcs95ScriptSizeMethod: options.UseAlternateAcs95SizeMethod);
            file.Disassemble(outputStream);

            outputStream.Dispose();
            stream.Dispose();

            return 0;
        }

        private int ExportOpcodes(ExportOpcodesOptions options)
        {
            string path = Path.GetFullPath(options.OutputFile);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            FileStream outputStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

            string output = "";

            int longestName = 0;

            foreach (var value in AcsInstruction.Opcodes)
            {
                string n = value.Name;
                if (n.Length > longestName)
                {
                    longestName = n.Length;
                }
            }

            foreach (var value in AcsInstruction.Opcodes)
            {
                output += $"Num = " + value.Id;

                for (int i = 0; i < 3 - value.Id.ToString().Length; i++)
                {
                    output += " ";
                }

                output += $"; Id = " + Enum.GetName(value.AsEnum());

                for (int i = 0; i < longestName - value.Name.Length; i++)
                {
                    output += " ";
                }

                output += "; Name = " + value.Name;
                for (int i = 0; i < longestName - value.Name.Length; i++)
                {
                    output += " ";
                }

                output += "; Arguments = " + value.NumberOfArguments;
                if (value.NumberOfArguments > 0)
                {
                    output += " (";
                    for (int i = 0; i < value.NumberOfArguments; i++)
                    {
                        output += value.ArgumentTypes[i].Name;
                        if (i < value.NumberOfArguments - 1)
                        {
                            output += ", ";
                        }
                    }
                    output += ")";
                }

                output += "\n";
            }

            byte[] array = Encoding.UTF8.GetBytes(output);
            outputStream.Write(array, 0, array.Length);
            outputStream.Dispose();

            return 0;
        }
    }
}
