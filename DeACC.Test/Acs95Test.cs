namespace DeACC.Test
{
    [TestClass]
    public class Acs95Test
    {
        private static AcsInstruction[] DecodeInstructions(int[] data)
        {
            byte[] bytes = new byte[data.Length * 4];
            Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length);
            
            using var stream = new MemoryStream(bytes);
            using var reader = new BinaryReader(stream);

            AcsInstruction[] instructions = AcsInstruction.ReadCode(AcsFormat.Acs95, reader, bytes.Length);

            return instructions;
        }

        private static AcsInstruction DecodeInstruction(int[] data)
        {
            var instructions = DecodeInstructions(data);
            return instructions[0];
        }

        [TestMethod]
        public void ReadInstructionNoArguments()
        {
            int[] data =
            {
                (int)OpcodeEnum.Terminate
            };

            AcsInstruction instruction = DecodeInstruction(data);

            Assert.IsTrue(
                AcsInstruction.OpcodesAreEqual(instruction.Opcode, OpcodeEnum.Terminate)
                && instruction.Arguments.Length == 0
            );
        }

        [TestMethod]
        public void ReadInstructionFiveArguments()
        {
            int[] data =
            {
                (int)OpcodeEnum.LSpec4Direct,
                94, 14, 8, 0, 25
            };

            AcsInstruction instruction = DecodeInstruction(data);

            Assert.IsTrue(
                AcsInstruction.OpcodesAreEqual(instruction.Opcode, OpcodeEnum.LSpec4Direct)
                && instruction.Arguments.Length == 5
                && instruction.Arguments[0] == 94
                && instruction.Arguments[1] == 14
                && instruction.Arguments[2] == 8
                && instruction.Arguments[3] == 0
                && instruction.Arguments[4] == 25
            );
        }
    }
}