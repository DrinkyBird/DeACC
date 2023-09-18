namespace DeACC
{
    class AcsFunction
    {
        public string Name;
        public string[] Arguments { get; private set; }
        public int NumberOfVariables { get; private set; }
        public int ImportNum { get; private set; }
        public bool Returns { get; private set; }
        public int Pointer { get; private set; }
        public int CodeSize;

        public AcsInstruction[] Code;

        public bool IsImported => Pointer == 0;

        public AcsFunction(string[] args, int varc, bool returns, int importNum, int ptr)
        {
            Arguments = args;
            NumberOfVariables = varc;
            ImportNum = importNum;
            Returns = returns;
            Pointer = ptr;
        }
    }
}
