using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Csnxs.DeACC
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

            AcsFile file = new AcsFile(stream, format);
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

            output = "AcsOpcode[] opcodes = new AcsOpcode[] {\n";

            int longestName = 0;

            foreach (var value in Enum.GetValues(typeof(OpcodeEnum)))
            {
                string n = value.ToString();
                if (n.Length > longestName)
                {
                    longestName = n.Length;
                }
            }

            foreach (var value in Enum.GetValues(typeof (OpcodeEnum)))
            {
                output += $"    new AcsOpcode {{Name = \"{value.ToString()}\",";

                for (int i = 0; i < longestName - value.ToString().Length; i++)
                {
                    output += " ";
                }

                output += $" NumberOfArguments = 0, FirstArgumentIsByte = false}},\n";
            }

            output += "};";

            byte[] array = Encoding.UTF8.GetBytes(output);
            outputStream.Write(array, 0, array.Length);
            outputStream.Dispose();

            return 0;
        }
    }
}
