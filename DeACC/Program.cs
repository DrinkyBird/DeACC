using System;
using System.Collections.Generic;
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
            public long FileName { get; set; }
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

        private int Disassemble(DisassembleOptions options)
        {


            return 0;
        }
    }
}
