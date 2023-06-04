namespace DeACC.Test
{
    [TestClass]
    public class AcsInstructionTest
    {
        [TestMethod]
        public void TestOpcodesAreEqual()
        {
            Assert.IsTrue(AcsInstruction.OpcodesAreEqual(AcsInstruction.Opcodes[0], AcsInstruction.Opcodes[0]));
            Assert.IsFalse(AcsInstruction.OpcodesAreEqual(AcsInstruction.Opcodes[0], AcsInstruction.Opcodes[1]));
        }
    }
}
