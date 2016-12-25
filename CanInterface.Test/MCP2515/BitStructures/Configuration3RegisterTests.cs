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
    public class Configuration3RegisterTests
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b1100_0000, true, true, (byte)0)
                    .SetName("Configuration2Register - Byte Constructor - true, true, 0");
                yield return new TestCaseData((byte)0b0000_0111, false, false, (byte)7)
                    .SetName("Configuration2Register - Byte Constructor - false, false, 7,");
                yield return new TestCaseData((byte)0b1110_0111, true, true, (byte)7)
                    .SetName("Configuration2Register - Byte Constructor - Bit 5 Set");
                yield return new TestCaseData((byte)0b1101_0111, true, true, (byte)7)
                    .SetName("Configuration2Register - Byte Constructor - Bit 4 Set");
                yield return new TestCaseData((byte)0b1100_1111, true, true, (byte)7)
                    .SetName("Configuration2Register - Byte Constructor - Bit 3 Set");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, bool sof, bool wakfil, byte phseg2)
        {
            var register = new Configuration3Register(value);
            Assert.That(register.SOF, Is.EqualTo(sof));
            Assert.That(register.WAKFIL, Is.EqualTo(wakfil));
            Assert.That(register.PHSEG2, Is.EqualTo(phseg2));
        }

        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b1100_0000, true, true, (byte)0)
                    .SetName("Configuration2Register - ToByte - true, true, 0");
                yield return new TestCaseData((byte)0b0000_0111, false, false, (byte)7)
                    .SetName("Configuration2Register - ToByte - false, false, 7");
            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, bool sof, bool wakfil, byte phseg2)
        {
            var register = new Configuration3Register(sof, wakfil, phseg2);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }
    }
}
