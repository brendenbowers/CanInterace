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
    public class RxStandardIdentifierLowRegisterTests
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0000, (byte)0, false, false, (byte)0)
                    .SetName("RxStandardIdentifierLowRegister - Byte Constructor - 0, false, false, 0");
                yield return new TestCaseData((byte)0b1111_1011, (byte)7, true, true, (byte)3)
                    .SetName("RxStandardIdentifierLowRegister - Byte Constructor - 7, true, true, 3");
                yield return new TestCaseData((byte)0b0000_0100, (byte)0, false, false, (byte)0)
                    .SetName("RxStandardIdentifierLowRegister - Byte Constructor - Bit 2 set");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value,byte sidl, bool srr, bool ide, byte eid)
        {
            var register = new RxStandardIdentifierLowRegister(value);
            Assert.That(register.SIDL, Is.EqualTo(sidl));
            Assert.That(register.SRR, Is.EqualTo(srr));
            Assert.That(register.IDE, Is.EqualTo(ide));
            Assert.That(register.EID, Is.EqualTo(eid));
        }

    }
}
