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
    public class TxStandardIdentifierHighRegisterTests
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0000, (byte)0)
                    .SetName("TxStandardIdentifierHighRegister - Byte Constructor - 0");
                yield return new TestCaseData((byte)0b1111_1111, (byte)255)
                    .SetName("TxStandardIdentifierHighRegister - Byte Constructor - 255");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, byte sidh)
        {
            var register = new TxStandardIdentifierHighRegister(value);
            Assert.That(register.SIDH, Is.EqualTo(sidh));
        }

        public static IEnumerable<TestCaseData> IdConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b1111_0110, (uint)4274520375)
                    .SetName("TxStandardIdentifierHighRegister - UInt Constructor - 4274520375");
            }
        }

        [TestCaseSource(nameof(IdConstructorTestSource))]
        public void UIntConstructorTest(byte value, uint id)
        {
            var register = new TxStandardIdentifierHighRegister(id);
            Assert.That(register.SIDH, Is.EqualTo(value));
        }
    }
}
