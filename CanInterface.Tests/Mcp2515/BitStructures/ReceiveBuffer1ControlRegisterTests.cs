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
    public class ReceiveBuffer1ControlRegisterTests
    {

        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0110_1000, ReceiveBufferOperatingMode.AcceptAll, true, FilterMatch.RX1RXF0)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - AcceptAll, true, RX1RXF0");
                yield return new TestCaseData((byte)0b0100_1000, ReceiveBufferOperatingMode.ExtendedIdentifierFilterMatchesOnly, true, FilterMatch.RX1RXF0)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - ExtendedIdentifierFilterMatchesOnly, true, RX1RXF0");
                yield return new TestCaseData((byte)0b0010_1000, ReceiveBufferOperatingMode.StandardIdentifierFilterMatchesOnly, true, FilterMatch.RX1RXF0)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - StandardIdentifierFilterMatchesOnly, true, RX1RXF0");
                yield return new TestCaseData((byte)0b0000_1000, ReceiveBufferOperatingMode.AllFilterMatches, true, FilterMatch.RX1RXF0)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - AllFilterMatches, true, RX1RXF0");
                yield return new TestCaseData((byte)0b0110_0000, ReceiveBufferOperatingMode.AcceptAll, false, FilterMatch.RX1RXF0)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - AcceptAll, false, RX1RXF0");
                yield return new TestCaseData((byte)0b0000_0001, ReceiveBufferOperatingMode.AllFilterMatches, false, FilterMatch.RX1RXF1)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - AllFilterMatches, true, RX1RXF1");
                yield return new TestCaseData((byte)0b0000_0010, ReceiveBufferOperatingMode.AllFilterMatches, false, FilterMatch.RX1RXF2)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - AllFilterMatches, true, RX1RXF2");
                yield return new TestCaseData((byte)0b0000_0011, ReceiveBufferOperatingMode.AllFilterMatches, false, FilterMatch.RX1RXF3)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - AllFilterMatches, true, RX1RXF3");
                yield return new TestCaseData((byte)0b0000_0100, ReceiveBufferOperatingMode.AllFilterMatches, false, FilterMatch.RX1RXF4)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - AllFilterMatches, true, RX1RXF4");
                yield return new TestCaseData((byte)0b0000_0101, ReceiveBufferOperatingMode.AllFilterMatches, false, FilterMatch.RX1RXF5)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - AllFilterMatches, true, RX1RXF5");
                yield return new TestCaseData((byte)0b0000_0110, ReceiveBufferOperatingMode.AllFilterMatches, false, FilterMatch.RX0RXF0)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - AllFilterMatches, true, RX0RXF0");
                yield return new TestCaseData((byte)0b0000_0111, ReceiveBufferOperatingMode.AllFilterMatches, false, FilterMatch.RX0RXF1)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - AllFilterMatches, true, RX0RXF1");


                yield return new TestCaseData((byte)0b1110_0000, ReceiveBufferOperatingMode.AcceptAll, false, FilterMatch.RX1RXF0)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - Bit 7 Set");
                yield return new TestCaseData((byte)0b0111_0000, ReceiveBufferOperatingMode.AcceptAll, false, FilterMatch.RX1RXF0)
                    .SetName("ReceiveBuffer1ControlRegister - Byte Constructor - Bit 4 Set");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, ReceiveBufferOperatingMode rxm, bool rxrtr, FilterMatch filhit)
        {
            var register = new ReceiveBuffer1ControlRegister(value);
            Assert.That(register.RXM, Is.EqualTo(rxm));
            Assert.That(register.RXRTR, Is.EqualTo(rxrtr));
            Assert.That(register.FILHIT, Is.EqualTo(filhit));
        }


        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0110_0000, ReceiveBufferOperatingMode.AcceptAll)
                    .SetName("ReceiveBuffer1ControlRegister - ToByte - AcceptAll");
                yield return new TestCaseData((byte)0b0100_0000, ReceiveBufferOperatingMode.ExtendedIdentifierFilterMatchesOnly)
                    .SetName("ReceiveBuffer1ControlRegister - ToByte - ExtendedIdentifierFilterMatchesOnly");
                yield return new TestCaseData((byte)0b0010_0000, ReceiveBufferOperatingMode.StandardIdentifierFilterMatchesOnly)
                    .SetName("ReceiveBuffer1ControlRegister - ToByte - StandardIdentifierFilterMatchesOnly");
                yield return new TestCaseData((byte)0b0000_0000, ReceiveBufferOperatingMode.AllFilterMatches)
                    .SetName("ReceiveBuffer1ControlRegister - ToByte - AllFilterMatches");
            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, ReceiveBufferOperatingMode rxm)
        {
            var register = new ReceiveBuffer1ControlRegister(rxm);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }
    }
}
