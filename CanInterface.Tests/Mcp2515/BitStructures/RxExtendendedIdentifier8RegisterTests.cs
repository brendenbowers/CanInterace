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
    public class RxExtendendedIdentifier8RegisterTests
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0000, (byte)0)
                    .SetName("RxExtendendedIdentifier8Register - Byte Constructor - 0");
                yield return new TestCaseData((byte)0b1111_1111, (byte)255)
                    .SetName("RxExtendendedIdentifier8Register - Byte Constructor - 255");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, byte eid)
        {
            var register = new RxExtendendedIdentifier8Register(value);
            Assert.That(register.EID, Is.EqualTo(eid));
        }

        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0000, (byte)0)
                    .SetName("RxExtendendedIdentifier8Register - ToByte - 0");
                yield return new TestCaseData((byte)0b1111_1111, (byte)255)
                    .SetName("RxExtendendedIdentifier8Register - ToByte - 255");
            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, byte eid)
        {
            var register = new RxExtendendedIdentifier8Register(eid);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }
    }
}
