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
            [Value(0, MetaName = "file", HelpText = "Compiled ACS object file", Required = true)]
            public string FileName { get; set; }
        }

        #endregion

        static void Main(string[] args)
        {
            new Program(args);
        }

        private Program(string[] args)
        {
            int ret = CommandLine.Parser.Default.ParseArguments<DisassembleOptions>(args)
                .MapResult(
                    Disassemble,
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
            string path = options.FileName;
            if (!File.Exists(path))
            {
                PrintError("File does not exist: " + path);
                return 2;
            }

            FileStream stream = new FileStream(path, FileMode.Open);
            AcsFormat format = AcsFormatIdentifier.IdentifyFormat(stream);

            if (format == AcsFormat.NotAcs)
            {
                PrintError(path + " is not an ACS file!");
                return 3;
            }

            Console.WriteLine("Detected format: " + format);

            return 0;
        }
    }
}
