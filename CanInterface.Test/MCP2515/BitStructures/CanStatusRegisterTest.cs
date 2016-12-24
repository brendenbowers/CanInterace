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
    public class CanStatusRegisterTest
    {

        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0000, OperatingMode.Normal, InterruptFlagCode.None)
                    .SetName("CanStatusRegister - Byte Constructor - Normal, None");
                yield return new TestCaseData((byte)0b0010_0010, OperatingMode.Sleep, InterruptFlagCode.Error)
                    .SetName("CanStatusRegister - Byte Constructor - Sleep, Error");
                yield return new TestCaseData((byte)0b0100_0100, OperatingMode.Loopback, InterruptFlagCode.WakeUp)
                    .SetName("CanStatusRegister - Byte Constructor - Loopback, WakeUp");
                yield return new TestCaseData((byte)0b0110_0110, OperatingMode.ListenOnly, InterruptFlagCode.TXB0)
                    .SetName("CanStatusRegister - Byte Constructor - ListenOnly, TXB0");
                yield return new TestCaseData((byte)0b1000_1000, OperatingMode.Configuration, InterruptFlagCode.TXB1)
                    .SetName("CanStatusRegister - Byte Constructor - Configuration, TXB1");
                yield return new TestCaseData((byte)0b0000_1010, OperatingMode.Normal, InterruptFlagCode.TXB2)
                    .SetName("CanStatusRegister - Byte Constructor - Normal, TXB2");
                yield return new TestCaseData((byte)0b0000_1100, OperatingMode.Normal, InterruptFlagCode.RXB0)
                    .SetName("CanStatusRegister - Byte Constructor - Normal, RXB0");
                yield return new TestCaseData((byte)0b0000_1110, OperatingMode.Normal, InterruptFlagCode.RXB1)
                    .SetName("CanStatusRegister - Byte Constructor - Normal, RXB1");

                yield return new TestCaseData((byte)0b0001_0000, OperatingMode.Normal, InterruptFlagCode.None)
                    .SetName("CanStatusRegister - Byte Constructor - Unused Bit 4 Set");

                yield return new TestCaseData((byte)0b0000_0001, OperatingMode.Normal, InterruptFlagCode.None)
                    .SetName("CanStatusRegister - Byte Constructor - Unused Bit 0 Set");

            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, OperatingMode opMode, InterruptFlagCode flagCode)
        {
            var register = new CanStatusRegister(value);
            Assert.That(register.OperatingMode, Is.EqualTo(opMode));
            Assert.That(register.InterruptFlagCode, Is.EqualTo(flagCode));
        }
    }
}
