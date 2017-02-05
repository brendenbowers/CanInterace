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
    public class Configuration1RegisterTests
    {

        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0000, SyncronizationJumpWidth.OneXTQ, (byte)0)
                    .SetName("Configuration1Register - Byte Constructor - OneXTQ, 0");
                yield return new TestCaseData((byte)0b0100_0000, SyncronizationJumpWidth.TwoXTQ, (byte)0)
                    .SetName("Configuration1Register - Byte Constructor - TwoXTQ, 0");
                yield return new TestCaseData((byte)0b1000_0000, SyncronizationJumpWidth.ThreeXTQ, (byte)0)
                    .SetName("Configuration1Register - Byte Constructor - ThreeXTQ, 0");
                yield return new TestCaseData((byte)0b1100_0000, SyncronizationJumpWidth.FourXTQ, (byte)0)
                    .SetName("Configuration1Register - Byte Constructor - FourXTQ, 0");
                yield return new TestCaseData((byte)0b0011_1111, SyncronizationJumpWidth.OneXTQ, (byte)63)
                    .SetName("Configuration1Register - Byte Constructor - OneXTQ, 63");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, SyncronizationJumpWidth sjw, byte brp)
        {
            var register = new Configuration1Register(value);
            Assert.That(register.SJW, Is.EqualTo(sjw));
            Assert.That(register.BRP, Is.EqualTo(brp));
        }

        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0000, SyncronizationJumpWidth.OneXTQ, (byte)0)
                    .SetName("Configuration1Register - ToByte - OneXTQ, 0");
                yield return new TestCaseData((byte)0b0100_0000, SyncronizationJumpWidth.TwoXTQ, (byte)0)
                    .SetName("Configuration1Register - ToByte - TwoXTQ, 0");
                yield return new TestCaseData((byte)0b1000_0000, SyncronizationJumpWidth.ThreeXTQ, (byte)0)
                    .SetName("Configuration1Register - ToByte - ThreeXTQ, 0");
                yield return new TestCaseData((byte)0b1100_0000, SyncronizationJumpWidth.FourXTQ, (byte)0)
                    .SetName("Configuration1Register - ToByte - FourXTQ, 0");
                yield return new TestCaseData((byte)0b0011_1111, SyncronizationJumpWidth.OneXTQ, (byte)63)
                    .SetName("Configuration1Register - ToByte - OneXTQ, 63");
            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, SyncronizationJumpWidth sjw, byte brp)
        {
            var register = new Configuration1Register(sjw, brp);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }

    }
}
