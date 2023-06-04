namespace DeACC.Test
{
    [TestClass]
    public class AcsFormatIdentifierTest
    {
        [TestMethod]
        [DeploymentItem("Data/hexen_map03.o")]
        public void IdentifyAcs95()
        {
            using var stream = new FileStream("hexen_map03.o", FileMode.Open, FileAccess.Read);
            var format = AcsFormatIdentifier.IdentifyFormat(stream);
            Assert.AreEqual(format, AcsFormat.Acs95);
        }

        [TestMethod]
        [DeploymentItem("Data/lib1.o")]
        public void IdentifyAcsZDoomLower()
        {
            using var stream = new FileStream("lib1.o", FileMode.Open, FileAccess.Read);
            var format = AcsFormatIdentifier.IdentifyFormat(stream);
            Assert.AreEqual(format, AcsFormat.ZDoomLower);
        }
    }
}
