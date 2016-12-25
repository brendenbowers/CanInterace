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
    public class Configuration2RegisterTests
    {

        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b1100_0000, true, true, (byte)0, (byte)0)
                    .SetName("Configuration2Register - Byte Constructor - true, true, 0, 0");
                yield return new TestCaseData((byte)0b0011_1111, false, false, (byte)7, (byte)7)
                    .SetName("Configuration2Register - Byte Constructor - false, false, 7, 7");
                yield return new TestCaseData((byte)0b1111_1000, true, true, (byte)7, (byte)0)
                    .SetName("Configuration2Register - Byte Constructor - true, true, 7, 0");
                yield return new TestCaseData((byte)0b1100_0111, true, true, (byte)0, (byte)7)
                    .SetName("Configuration2Register - Byte Constructor - true, true, 0, 7");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, bool btlmode, bool sam, byte phseg1, byte prseg)
        {
            var register = new Configuration2Register(value);
            Assert.That(register.BTLMODE, Is.EqualTo(btlmode));
            Assert.That(register.SAM, Is.EqualTo(sam));
            Assert.That(register.PHSEG1, Is.EqualTo(phseg1));
            Assert.That(register.PRSEG, Is.EqualTo(prseg));
        }

        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b1100_0000, true, true, (byte)0, (byte)0)
                    .SetName("Configuration2Register - ToByte - true, true, 0, 0");
                yield return new TestCaseData((byte)0b0011_1111, false, false, (byte)7, (byte)7)
                    .SetName("Configuration2Register - ToByte - false, false, 7, 7");
                yield return new TestCaseData((byte)0b1111_1000, true, true, (byte)7, (byte)0)
                    .SetName("Configuration2Register - ToByte - true, true, 7, 0");
                yield return new TestCaseData((byte)0b1100_0111, true, true, (byte)0, (byte)7)
                    .SetName("Configuration2Register - ToByte - true, true, 0, 7");
            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, bool btlmode, bool sam, byte phseg1, byte prseg)
        {
            var register = new Configuration2Register(btlmode, sam, phseg1, prseg);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }
    }
}
