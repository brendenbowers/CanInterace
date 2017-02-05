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
    public class TxStandardIdentifierLowRegisterTest
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0000, (byte)0, false, (byte)0)
                    .SetName("TxStandardIdentifierLowRegister - Byte Constructor - 0, false, 0");
                yield return new TestCaseData((byte)0b1110_1011, (byte)7, true, (byte)3)
                    .SetName("TxStandardIdentifierLowRegister - Byte Constructor - 255");

                yield return new TestCaseData((byte)0b0001_0000, (byte)0, false, (byte)0)
                    .SetName("TxStandardIdentifierLowRegister - Byte Constructor - Bit 4 set");
                yield return new TestCaseData((byte)0b0000_0100, (byte)0, false, (byte)0)
                    .SetName("TxStandardIdentifierLowRegister - Byte Constructor - Bit 2 set");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, byte sidl, bool exide, byte eid)
        {
            var register = new TxStandardIdentifierLowRegister(value);
            Assert.That(register.SIDL, Is.EqualTo(sidl));
            Assert.That(register.EXIDE, Is.EqualTo(exide));
            Assert.That(register.EID, Is.EqualTo(eid));
        }

        public static IEnumerable<TestCaseData> IdConstructorTestCaseSource
        {
            get
            {
                yield return new TestCaseData((uint)4274520375, true, (byte)0b0100_0000, (byte)0b0000_0000)
                    .SetName("TxStandardIdentifierLowRegister - UInt Constructor - 4274520375");
            }
        }

        [TestCaseSource(nameof(IdConstructorTestCaseSource))]
        public void UIntConstructorTest(uint id, bool extended, byte sidl, byte eid)
        {
            var register = new TxStandardIdentifierLowRegister(id, extended);
            Assert.That(register.SIDL, Is.EqualTo(sidl));
            Assert.That(register.EXIDE, Is.EqualTo(extended));
            Assert.That(register.EID, Is.EqualTo(eid));
        }
    }
}
