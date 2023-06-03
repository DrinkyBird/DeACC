namespace DeACC
{
    class AcsFunction
    {
        public string Name;
        public int NumberOfArguments { get; private set; }
        public int NumberOfVariables { get; private set; }
        public int ImportNum { get; private set; }
        public bool Returns { get; private set; }
        public int Pointer { get; private set; }
        public int CodeSize;

        public AcsInstruction[] Code;

        public bool IsImported => Pointer == 0;

        public AcsFunction(int argc, int varc, bool returns, int importNum, int ptr)
        {
            NumberOfArguments = argc;
            NumberOfVariables = varc;
            ImportNum = importNum;
            Returns = returns;
            Pointer = ptr;
        }
    }
}
