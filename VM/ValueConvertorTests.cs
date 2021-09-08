using NUnit.Framework;
using VirtualMachineBase.BinaryUtilities;

namespace VM {
    public class ValueConvertorTests {

 
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(252, -4)]
        [TestCase(4, 4)]
        [TestCase(127, 127)]
        [TestCase(128, -128)]
        [TestCase(255, -1)]
        public void ToSByte_WhenCalled_ReturnsCorrectValue(byte value, sbyte expected) {
            var result = ValueConvertor.ToSByte(value);
            Assert.AreEqual(expected, result);
        }

        [TestCase(new byte []{ 0,0 } , (ushort)0)]
        [TestCase(new byte []{ 0,1 } , (ushort)1)]
        [TestCase(new byte []{ 0,2 } , (ushort)2)]
        [TestCase(new byte []{ 1,0 } , (ushort)256)]
        [TestCase(new byte []{ 2,0 } , (ushort)512)]
        [TestCase(new byte []{ 2,1 } , (ushort)513)]
        [TestCase(new byte []{ 255,1 } , (ushort)65281)]
        public void ToUshort_WhenCalled_ReturnsCorrectValue(byte[] value, ushort expected) {
            var result = ValueConvertor.ToUshort(value);
            Assert.AreEqual(expected, result);
        }

        [TestCase(new byte []{ 0,0,0,0 }            , (uint)0)]
        [TestCase(new byte []{ 0,0,0,1 }            , (uint)1)]
        [TestCase(new byte []{ 0,0,1,0 }            , (uint)256)]
        [TestCase(new byte []{ 255,255,255,255 }    , uint.MaxValue)]
        public void ToUInt_WhenCalled_ReturnsCorrectValue(byte[] value, uint expected) {
            var result = ValueConvertor.ToUInt(value);
            Assert.AreEqual(expected, result);
        }

        [TestCase(new byte []{ 0,0,0,0 }            , (uint)0)]
        [TestCase(new byte []{ 0,0,0,1 }            , (uint)1)]
        [TestCase(new byte []{ 2,2,2,2 }            , (uint)33686018)]
        [TestCase(new byte []{ 0,0,1,0 }            , (uint)256)]
        [TestCase(new byte []{ 255,255,255,255 }    , uint.MaxValue)]
        public void ToBytes_WhenCalledWithUint_ReturnsCorrectValue(  byte[] expected, uint value) {
            var result = ValueConvertor.ToBytes(value);
            Assert.AreEqual(expected, result);
        }

        [TestCase((uint)1,1)]
        [TestCase((uint)0,0)]
        [TestCase((uint)2147483649 ,-2147483647) ]
        [TestCase(uint.MaxValue ,-1) ]
        [TestCase((uint)4294967264 ,-32) ]

        public void ToI_WhenCalledWithUint_ReturnsCorrectValue(  uint value, int expected) {
            var result = ValueConvertor.ToInt(value);
            Assert.AreEqual(expected, result);
        }
    }
}