namespace DeACC.Test
{
    [TestClass]
    public class AcsInstructionTest
    {
        [TestMethod]
        public void TestOpcodesAreEqualSame()
        {
            Assert.IsTrue(AcsInstruction.OpcodesAreEqual(AcsInstruction.Opcodes[0], AcsInstruction.Opcodes[0]));
        }

        [TestMethod]
        public void TestOpcodesAreEqualDifferent()
        {
            Assert.IsFalse(AcsInstruction.OpcodesAreEqual(AcsInstruction.Opcodes[0], AcsInstruction.Opcodes[1]));
        }
        [TestMethod]
        public void TestOpcodesAreEqualEnumSame()
        {
            Assert.IsTrue(AcsInstruction.OpcodesAreEqual(AcsInstruction.Opcodes[0], OpcodeEnum.Nop));
        }

        [TestMethod]
        public void TestOpcodesAreEqualEnumDifferent()
        {
            Assert.IsFalse(AcsInstruction.OpcodesAreEqual(AcsInstruction.Opcodes[0], OpcodeEnum.Add));
        }
    }
}
