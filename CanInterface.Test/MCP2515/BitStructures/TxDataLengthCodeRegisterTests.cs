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
    public class TxDataLengthCodeRegisterTests
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0100_0000, true, (byte)0)
                    .SetName("TxDataLengthCodeRegister - Byte Constructor - true, 0");
                yield return new TestCaseData((byte)0b0000_1111, false, (byte)15)
                    .SetName("TxDataLengthCodeRegister - Byte Constructor - false, 255");
                yield return new TestCaseData((byte)0b1000_0000, false, (byte)0)
                    .SetName("TxDataLengthCodeRegister - Byte Constructor - Bit 7 set");
                yield return new TestCaseData((byte)0b0010_0000, false, (byte)0)
                    .SetName("TxDataLengthCodeRegister - Byte Constructor - Reserve bit 1 set");
                yield return new TestCaseData((byte)0b0001_0000, false, (byte)0)
                    .SetName("TxDataLengthCodeRegister - Byte Constructor - Reserve bit 0 set");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, bool rtr, byte dlc)
        {
            var register = new TxDataLengthCodeRegister(value);
            Assert.That(register.RTR, Is.EqualTo(rtr));
            Assert.That(register.DLC, Is.EqualTo(dlc));
        }


        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0100_0000, true, (byte)0)
                    .SetName("TxDataLengthCodeRegister - Byte Constructor - true, 0");
                yield return new TestCaseData((byte)0b0000_1111, false, (byte)15)
                    .SetName("TxDataLengthCodeRegister - Byte Constructor - false, 255");
            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, bool rtr, byte dlc)
        {
            var register = new TxDataLengthCodeRegister(rtr, dlc);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }
    }
}
