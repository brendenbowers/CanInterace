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
    public class CanControlRegisterTests
    {

        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0001_1100, OperatingMode.Normal, true, true, true, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - Byte Constructor - Normal, true, true, true, One");
                yield return new TestCaseData((byte)0b0101_1100, OperatingMode.Loopback, true, true, true, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - Byte Constructor - Loopback, true, true, true, One");
                yield return new TestCaseData((byte)0b0011_1100, OperatingMode.Sleep, true, true, true, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - Byte Constructor - Sleep, true, true, true, One");
                yield return new TestCaseData((byte)0b0111_1100, OperatingMode.ListenOnly, true, true, true, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - Byte Constructor - ListenOnly, true, true, true, One");
                yield return new TestCaseData((byte)0b0111_1101, OperatingMode.ListenOnly, true, true, true, ClockOutPrescaler.Two)
                    .SetName("CanControlRegister - Byte Constructor - ListenOnly, true, true, true, Two");
                yield return new TestCaseData((byte)0b0111_1110, OperatingMode.ListenOnly, true, true, true, ClockOutPrescaler.Three)
                    .SetName("CanControlRegister - Byte Constructor - ListenOnly, true, true, true, Three");
                yield return new TestCaseData((byte)0b0111_1111, OperatingMode.ListenOnly, true, true, true, ClockOutPrescaler.Four)
                    .SetName("CanControlRegister - Byte Constructor - ListenOnly, true, true, true, Four");
                yield return new TestCaseData((byte)0b0100_1100, OperatingMode.Loopback, false, true, true, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - Byte Constructor - Loopback, false, true, true, One");
                yield return new TestCaseData((byte)0b0100_0100, OperatingMode.Loopback, false, false, true, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - Byte Constructor - Loopback, false, false, true, One");
                yield return new TestCaseData((byte)0b0100_0000, OperatingMode.Loopback, false, false, false, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - Byte Constructor - Loopback, false, false, false, One");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, OperatingMode opmode, bool abat, bool osm, bool clken, ClockOutPrescaler clkpre)
        {
            var register = new CanControlRegister(value);
            Assert.That(register.REQOP, Is.EqualTo(opmode));
            Assert.That(register.ABAT, Is.EqualTo(abat));
            Assert.That(register.OSM, Is.EqualTo(osm));
            Assert.That(register.CLKEN, Is.EqualTo(clken));
            Assert.That(register.CLKPRE, Is.EqualTo(clkpre));
        }


        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0001_1100, OperatingMode.Normal, true, true, true, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - ToByte - Normal, true, true, true, One");
                yield return new TestCaseData((byte)0b0101_1100, OperatingMode.Loopback, true, true, true, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - ToByte - Loopback, true, true, true, One");
                yield return new TestCaseData((byte)0b0011_1100, OperatingMode.Sleep, true, true, true, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - ToByte - Sleep, true, true, true, One");
                yield return new TestCaseData((byte)0b0111_1100, OperatingMode.ListenOnly, true, true, true, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - ToByte - ListenOnly, true, true, true, One");
                yield return new TestCaseData((byte)0b0111_1101, OperatingMode.ListenOnly, true, true, true, ClockOutPrescaler.Two)
                    .SetName("CanControlRegister - ToByte - ListenOnly, true, true, true, Two");
                yield return new TestCaseData((byte)0b0111_1110, OperatingMode.ListenOnly, true, true, true, ClockOutPrescaler.Three)
                    .SetName("CanControlRegister - ToByte - ListenOnly, true, true, true, Three");
                yield return new TestCaseData((byte)0b0111_1111, OperatingMode.ListenOnly, true, true, true, ClockOutPrescaler.Four)
                    .SetName("CanControlRegister - ToByte - ListenOnly, true, true, true, Four");
                yield return new TestCaseData((byte)0b0100_1100, OperatingMode.Loopback, false, true, true, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - ToByte - Loopback, false, true, true, One");
                yield return new TestCaseData((byte)0b0100_0100, OperatingMode.Loopback, false, false, true, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - ToByte - Loopback, false, false, true, One");
                yield return new TestCaseData((byte)0b0100_0000, OperatingMode.Loopback, false, false, false, ClockOutPrescaler.One)
                    .SetName("CanControlRegister - ToByte - Loopback, false, false, false, One");
            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, OperatingMode opmode, bool abat, bool osm, bool clken, ClockOutPrescaler clkpre)
        {
            var register = new CanControlRegister(opmode,abat, osm, clken, clkpre);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }


    }
}
