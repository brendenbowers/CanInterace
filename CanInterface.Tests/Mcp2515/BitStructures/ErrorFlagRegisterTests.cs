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
    public class ErrorFlagRegisterTests
    {

        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b1111_1111, true, true, true, true, true, true, true, true)
                    .SetName("ErrorFlagRegister - Byte Constructor - true, true, true, true, true, true, true, true");
                yield return new TestCaseData((byte)0b0000_0000, false, false, false, false, false, false, false, false)
                    .SetName("ErrorFlagRegister - Byte Constructor - false, false, false, false, false, false, false, false");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, bool rx1ovr, bool rx0ovr, bool txbo, bool txep, bool rxep, bool txwar, bool rxwar, bool ewarn)
        {
            var register = new ErrorFlagRegister(value);
            Assert.That(register.RX1OVR, Is.EqualTo(rx1ovr));
            Assert.That(register.RX0OVR, Is.EqualTo(rx0ovr));
            Assert.That(register.TXBO, Is.EqualTo(txbo));
            Assert.That(register.TXEP, Is.EqualTo(txep));
            Assert.That(register.RXEP, Is.EqualTo(rxep));
            Assert.That(register.TXWAR, Is.EqualTo(txwar));
            Assert.That(register.RXWAR, Is.EqualTo(rxwar));
            Assert.That(register.EWARN, Is.EqualTo(ewarn));
        }


        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b1100_0000, true, true)
                    .SetName("ErrorFlagRegister - ToByte - true, true");
                yield return new TestCaseData((byte)0b0000_00000, false, false)
                    .SetName("ErrorFlagRegister - ToByte - false, false");
            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, bool rx1ovr, bool rx0ovr)
        {
            var register = new ErrorFlagRegister(rx1ovr, rx0ovr);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }
    }
}
