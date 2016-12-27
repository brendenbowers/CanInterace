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
    public class TxExtendendedIdentifier8RegisterTests
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0000, (byte)0)
                    .SetName("TxExtendendedIdentifier0Register - Byte Constructor - 0");
                yield return new TestCaseData((byte)0b1111_1111, (byte)255)
                    .SetName("TxExtendendedIdentifier0Register - Byte Constructor - 255");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, byte eid)
        {
            var register = new TxExtendendedIdentifier8Register(value);
            Assert.That(register.EID, Is.EqualTo(eid));
        }

        public static IEnumerable<TestCaseData> IdConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0001, (uint)4274520375)
                    .SetName("TxExtendendedIdentifier0Register - UInt Constructor - 4274520375");
            }
        }

        [TestCaseSource(nameof(IdConstructorTestSource))]
        public void UIntConstructorTest(byte value, uint id)
        {
            var register = new TxExtendendedIdentifier8Register(id);
            Assert.That(register.EID, Is.EqualTo(value));
        }
    }
}
