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

        public AcsFunction(int argc, int varc, bool returns)
        {
            NumberOfArguments = argc;
            NumberOfVariables = varc;
            Returns = returns;
        }
    }
}
