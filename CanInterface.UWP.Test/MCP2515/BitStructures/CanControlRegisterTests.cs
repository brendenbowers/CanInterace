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
    [TestFixture]
    public class CanControlRegisterTests
    {

        public IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData(0b0001_1100, (OperatingMode.Normal, true, true, true, ClockOutPrescaler.One))
                    .SetCategory("Unit Test")
                    .SetName("CanControlRegister - Byte Constructor - Normal, true, true, true, One");
                yield return new TestCaseData(0b0101_1100, (OperatingMode.Loopback, true, true, true, ClockOutPrescaler.One))
                    .SetCategory("Unit Test")
                    .SetName("CanControlRegister - Byte Constructor - Loopback, true, true, true, One");
                yield return new TestCaseData(0b0011_1100, (OperatingMode.Sleep, true, true, true, ClockOutPrescaler.One))
                    .SetCategory("Unit Test")
                    .SetName("CanControlRegister - Byte Constructor - Sleep, true, true, true, One");
                yield return new TestCaseData(0b0111_1100, (OperatingMode.ListenOnly, true, true, true, ClockOutPrescaler.One))
                    .SetCategory("Unit Test")
                    .SetName("CanControlRegister - Byte Constructor - ListenOnly, true, true, true, One");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, (OperatingMode opmode, bool abat, bool osm, bool clken, ClockOutPrescaler clkpre) expected)
        {
            var register = new CanControlRegister(value);
            Assert.That(register, Has.Property(nameof(register.REQOP)).EqualTo(expected.opmode));
            Assert.That(register, Has.Property(nameof(register.ABAT)).EqualTo(expected.abat));
            Assert.That(register, Has.Property(nameof(register.OSM)).EqualTo(expected.osm));
            Assert.That(register, Has.Property(nameof(register.CLKEN)).EqualTo(expected.clken));
            Assert.That(register, Has.Property(nameof(register.CLKPRE)).EqualTo(expected.clkpre));
        }
    }
}
