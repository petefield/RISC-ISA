using NUnit.Framework;

namespace TestProject1 {
    public class AssemblyTests {
        
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void RRI_Parses_PositiveImmediates() {
            var data = new string[] { "ADDI 8 8 4"};
            var machineCode = Risc16.Assembler.Assembler.Assemble(data);
            Assert.AreEqual(new byte[]{1,8,8,4 }, machineCode);
        }

        [Test]
        public void RRI_Parses_NegativeImmediates() {
            var data = new string[] { "ADDI 8 8 -4"};
            var machineCode = Risc16.Assembler.Assembler.Assemble(data);
            Assert.AreEqual(new byte[]{1,8,8,252 }, machineCode);
        }

       [TestCase("LUI 8 1" ,    new byte[]{ 3, 8, 0, 1 })]
       [TestCase("LUI 8 257" ,  new byte[]{ 3, 8, 1, 1})]
       [TestCase("LUI 8 257" ,  new byte[]{ 3, 8, 1, 1})]
       [TestCase("LUI 8 2047" , new byte[]{ 3, 8, 7, 255})]

        public void RI_Parses_Immediates(string instruction, byte[] expectedMachineCode) {
            var data = new string[] { instruction };
            var machineCode = Risc16.Assembler.Assembler.Assemble(data);
            Assert.AreEqual(expectedMachineCode, machineCode);
        }

           [Test]
        public void RRI_Parses_HALT() {
            var data = new string[] { "HALT"};
            var machineCode = Risc16.Assembler.Assembler.Assemble(data);
            Assert.AreEqual(new byte[]{7,0,0,1 }, machineCode);
        }

    }
}