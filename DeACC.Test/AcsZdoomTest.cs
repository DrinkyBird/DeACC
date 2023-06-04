namespace DeACC.Test
{
    [TestClass]
    public class AcsZdoomTest
    {
        private static AcsInstruction[] DecodeInstructions(byte[] data)
        {
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            AcsInstruction[] instructions = AcsInstruction.ReadCode(AcsFormat.ZDoomLower, reader, data.Length);

            return instructions;
        }

        private static AcsInstruction DecodeInstruction(byte[] data)
        {
            var instructions = DecodeInstructions(data);
            return instructions[0];
        }

        [TestMethod]
        public void TestDecodeSingleByteOpcode()
        {
            byte[] data = { 14 };

            AcsInstruction instruction = DecodeInstruction(data);
            Assert.IsTrue(AcsInstruction.OpcodesAreEqual(instruction.Opcode, OpcodeEnum.Add));
        }   

        [TestMethod]
        public void TestDecodeMultiByteOpcode()
        {
            byte[] data = { 0xF0, 0x1E };

            AcsInstruction instruction = DecodeInstruction(data);
            Assert.IsTrue(AcsInstruction.OpcodesAreEqual(instruction.Opcode, OpcodeEnum.EndLog));
        }

        [TestMethod]
        public void TestDecodeSingleByteOpcodeWithSingleByteArgument()
        {
            byte[] data = { 4, 123 };

            AcsInstruction instruction = DecodeInstruction(data);
            Assert.IsTrue(
                AcsInstruction.OpcodesAreEqual(instruction.Opcode, OpcodeEnum.LSpec1)
                && instruction.Arguments.Length == 1
                && instruction.Arguments[0] == 123
            );
        }

        [TestMethod]
        public void TestDecodeMultiByteOpcodeWithSingleByteArgument()
        {
            byte[] data = { 240, 114, 123 };

            AcsInstruction instruction = DecodeInstruction(data);
            Assert.IsTrue(
                AcsInstruction.OpcodesAreEqual(instruction.Opcode, OpcodeEnum.PushFunction)
                && instruction.Arguments.Length == 1
                && instruction.Arguments[0] == 123
            );
        }
    }
}
