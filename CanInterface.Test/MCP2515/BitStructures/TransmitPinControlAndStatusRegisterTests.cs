using CanInterface.MCP2515.BitStructures;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.Test.MCP2515.BitStructures
{
    [TestFixture(Category = "Unit Test")]
    public class TransmitPinControlAndStatusRegisterTests
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0011_1111, true, true, true, true, true, true)
                    .SetName("TransmitPinControlAndStatusRegister - Byte Constructor - true, true, true, true, true, true");
                yield return new TestCaseData((byte)0b0000_0000, false, false, false, false, false, false)
                    .SetName("TransmitPinControlAndStatusRegister - Byte Constructor - false, false, false, false, false, false");
                yield return new TestCaseData((byte)0b1000_0000, false, false, false, false, false, false)
                    .SetName("TransmitPinControlAndStatusRegister - Byte Constructor - Bit 7 set");
                yield return new TestCaseData((byte)0b0100_0000, false, false, false, false, false, false)
                    .SetName("TransmitPinControlAndStatusRegister - Byte Constructor - Bite 6 set");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, bool b2rts, bool b1rts, bool b0rts, bool b2rtsm, bool b1rtsm, bool b0rtsm)
        {
            var register = new TransmitPinControlAndStatusRegister(value);
            Assert.That(register.B2RTS, Is.EqualTo(b2rts));
            Assert.That(register.B1RTS, Is.EqualTo(b1rts));
            Assert.That(register.B0RTS, Is.EqualTo(b0rts));
            Assert.That(register.B2RTSM, Is.EqualTo(b2rtsm));
            Assert.That(register.B1RTSM, Is.EqualTo(b1rtsm));
            Assert.That(register.B0RTSM, Is.EqualTo(b0rtsm));
        }

        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0111, true, true, true)
                    .SetName("TransmitPinControlAndStatusRegister - ToByte - true, true, true");
                yield return new TestCaseData((byte)0b0000_0000, false, false, false)
                    .SetName("TransmitPinControlAndStatusRegister - ToByte - false, false, false");
            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, bool b2rtsm, bool b1rtsm, bool b0rtsm)
        {
            var register = new TransmitPinControlAndStatusRegister(b2rtsm, b1rtsm, b0rtsm);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }
    }
}
