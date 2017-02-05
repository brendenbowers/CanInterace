using CanInterface.MCP2515.BitStructures;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.Test.MCP2515.BitStructures
{
    [TestFixture(Category ="Unit Test")]
    public class CanInterruptFlagRegisterTests
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b1111_1111, true, true, true, true, true, true, true, true)
                    .SetName("CanInterruptFlagRegister - Byte Constructor - True, True, True, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0111_1111, false, true, true, true, true, true, true, true)
                    .SetName("CanInterruptFlagRegister - Byte Constructor - false, True, True, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0011_1111, false, false, true, true, true, true, true, true)
                    .SetName("CanInterruptFlagRegister - Byte Constructor - false, false, True, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0001_1111, false, false, false, true, true, true, true, true)
                    .SetName("CanInterruptFlagRegister - Byte Constructor - false, false, false, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0000_1111, false, false, false, false, true, true, true, true)
                    .SetName("CanInterruptFlagRegister - Byte Constructor - false, false, false, false, True, True, True, True");
                yield return new TestCaseData((byte)0b0000_0111, false, false, false, false, false, true, true, true)
                    .SetName("CanInterruptFlagRegister - Byte Constructor - false, false, false, false, false, True, True, True");
                yield return new TestCaseData((byte)0b0000_0011, false, false, false, false, false, false, true, true)
                    .SetName("CanInterruptFlagRegister - Byte Constructor - false, false, false, false, false, false True, True");
                yield return new TestCaseData((byte)0b0000_0001, false, false, false, false, false, false, false, true)
                    .SetName("CanInterruptFlagRegister - Byte Constructor - false, false, false, false, false, false, false, True");
                yield return new TestCaseData((byte)0b0000_0000, false, false, false, false, false, false, false, false)
                    .SetName("CanInterruptFlagRegister - Byte Constructor - false, false, false, false, false, false, false, false");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, bool merrf, bool wakif, bool errif, bool tx2if, bool tx1if, bool tx0if, bool rx1if, bool rx0if)
        {
            var register = new CanInterruptFlagRegister(value);
            Assert.That(register.MERRF, Is.EqualTo(merrf));
            Assert.That(register.WAKIF, Is.EqualTo(wakif));
            Assert.That(register.ERRIF, Is.EqualTo(errif));
            Assert.That(register.TX2IF, Is.EqualTo(tx2if));
            Assert.That(register.TX1IF, Is.EqualTo(tx1if));
            Assert.That(register.TX0IF, Is.EqualTo(tx0if));
            Assert.That(register.RX1IF, Is.EqualTo(rx1if));
            Assert.That(register.RX0IF, Is.EqualTo(rx0if));
        }


        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b1111_1111, true, true, true, true, true, true, true, true)
                    .SetName("CanInterruptFlagRegister - ToByte - True, True, True, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0111_1111, false, true, true, true, true, true, true, true)
                    .SetName("CanInterruptFlagRegister - ToByte - false, True, True, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0011_1111, false, false, true, true, true, true, true, true)
                    .SetName("CanInterruptFlagRegister - ToByte - false, false, True, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0001_1111, false, false, false, true, true, true, true, true)
                    .SetName("CanInterruptFlagRegister - ToByte - false, false, false, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0000_1111, false, false, false, false, true, true, true, true)
                    .SetName("CanInterruptFlagRegister - ToByte - false, false, false, false, True, True, True, True");
                yield return new TestCaseData((byte)0b0000_0111, false, false, false, false, false, true, true, true)
                    .SetName("CanInterruptFlagRegister - ToByte - false, false, false, false, false, True, True, True");
                yield return new TestCaseData((byte)0b0000_0011, false, false, false, false, false, false, true, true)
                    .SetName("CanInterruptFlagRegister - ToByte - false, false, false, false, false, false True, True");
                yield return new TestCaseData((byte)0b0000_0001, false, false, false, false, false, false, false, true)
                    .SetName("CanInterruptFlagRegister - ToByte - false, false, false, false, false, false, false, True");
                yield return new TestCaseData((byte)0b0000_0000, false, false, false, false, false, false, false, false)
                    .SetName("CanInterruptFlagRegister - ToByte - false, false, false, false, false, false, false, false");
            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, bool merrf, bool wakif, bool errif, bool tx2if, bool tx1if, bool tx0if, bool rx1if, bool rx0if)
        {
            var register = new CanInterruptFlagRegister(merrf, wakif, errif, tx2if, tx1if, tx0if, rx1if, rx0if);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }
    }
}

