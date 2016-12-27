using CanInterface.MCP2515.BitStructures;
using CanInterface.MCP2515.Enum;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.Test.MCP2515.BitStructures
{
    [TestFixture(Category = "Unit Test")]
    public class TransmitBufferControlRegisterTests
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0000, false, false, false, false, TransmitBufferPriority.Low)
                    .SetName("TransmitBufferControlRegister - Byte Constructor - false, false, false, false, Low");
                yield return new TestCaseData((byte)0b0111_1000, true, true, true, true, TransmitBufferPriority.Low)
                    .SetName("TransmitBufferControlRegister - Byte Constructor - true, true, true, true, Low");

                yield return new TestCaseData((byte)0b0000_0001, false, false, false, false, TransmitBufferPriority.LowIntermidate)
                    .SetName("TransmitBufferControlRegister - Byte Constructor - false, false, false, false, LowIntermidate");
                yield return new TestCaseData((byte)0b0000_0010, false, false, false, false, TransmitBufferPriority.HighIntermidate)
                    .SetName("TransmitBufferControlRegister - Byte Constructor - false, false, false, false, HighIntermidate");
                yield return new TestCaseData((byte)0b0000_0011, false, false, false, false, TransmitBufferPriority.High)
                    .SetName("TransmitBufferControlRegister - Byte Constructor - false, false, false, false, High");

                yield return new TestCaseData((byte)0b1000_0000, false, false, false, false, TransmitBufferPriority.Low)
                    .SetName("TransmitBufferControlRegister - Byte Constructor - Bit 7 set");
                yield return new TestCaseData((byte)0b0000_0100, false, false, false, false, TransmitBufferPriority.Low)
                    .SetName("TransmitBufferControlRegister - Byte Constructor - Bit 2 set");

            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, bool abtf, bool mloa, bool txerr, bool txreq, TransmitBufferPriority txp)
        {
            var register = new TransmitBufferControlRegister(value);
            Assert.That(register.ABTF, Is.EqualTo(abtf));
            Assert.That(register.MLOA, Is.EqualTo(mloa));
            Assert.That(register.TXERR, Is.EqualTo(txerr));
            Assert.That(register.TXREQ, Is.EqualTo(txreq));
            Assert.That(register.TXP, Is.EqualTo(txp));
        }


        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0000, TransmitBufferPriority.Low)
                    .SetName("TransmitBufferControlRegister - ToByte - Low");
                yield return new TestCaseData((byte)0b0000_0001, TransmitBufferPriority.LowIntermidate)
                    .SetName("TransmitBufferControlRegister - ToByte - LowIntermidate");
                yield return new TestCaseData((byte)0b0000_0010, TransmitBufferPriority.HighIntermidate)
                    .SetName("TransmitBufferControlRegister - ToByte - HighIntermidate");
                yield return new TestCaseData((byte)0b0000_0011, TransmitBufferPriority.High)
                    .SetName("TransmitBufferControlRegister - ToByte - High");
            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, TransmitBufferPriority txp)
        {
            var register = new TransmitBufferControlRegister(txp);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }
    }
}
