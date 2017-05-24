using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csnxs.DeACC
{
    class AcsFunction
    {
        public int NumberOfArguments { get; private set; }
        public int NumberOfVariables { get; private set; }
        public bool Returns { get; private set; }
        public int Pointer { get; private set; }

        public AcsInstruction[] Code;

        public AcsFunction(int argc, int varc, bool returns, int ptr)
        {
            NumberOfArguments = argc;
            NumberOfVariables = varc;
            Returns = returns;
            Pointer = ptr;
        }
    }
}
