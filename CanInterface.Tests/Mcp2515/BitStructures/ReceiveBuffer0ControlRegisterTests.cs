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
    public class ReceiveBuffer0ControlRegisterTests
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0110_1111, ReceiveBufferOperatingMode.AcceptAll, true, true, true, true)
                    .SetName("ReceiveBuffer0ControlRegister - Byte Constructor - AcceptAll, true, true, true, true");
                yield return new TestCaseData((byte)0b0100_1111, ReceiveBufferOperatingMode.ExtendedIdentifierFilterMatchesOnly, true, true, true, true)
                    .SetName("ReceiveBuffer0ControlRegister - Byte Constructor - ExtendedIdentifierFilterMatchesOnly, true, true, true, true");
                yield return new TestCaseData((byte)0b0010_1111, ReceiveBufferOperatingMode.StandardIdentifierFilterMatchesOnly, true, true, true, true)
                    .SetName("ReceiveBuffer0ControlRegister - Byte Constructor - StandardIdentifierFilterMatchesOnly, true, true, true, true");
                yield return new TestCaseData((byte)0b0000_1111, ReceiveBufferOperatingMode.AllFilterMatches, true, true, true, true)
                    .SetName("ReceiveBuffer0ControlRegister - Byte Constructor - AllFilterMatches, true, true, true, true");
                yield return new TestCaseData((byte)0b0110_0000, ReceiveBufferOperatingMode.AcceptAll, false, false, false, false)
                    .SetName("ReceiveBuffer0ControlRegister - Byte Constructor - AcceptAll, false, false, false, false");

                yield return new TestCaseData((byte)0b1110_1111, ReceiveBufferOperatingMode.AcceptAll, true, true, true, true)
                    .SetName("ReceiveBuffer0ControlRegister - Byte Constructor - Bit 7 Set");
                yield return new TestCaseData((byte)0b0111_1111, ReceiveBufferOperatingMode.AcceptAll, true, true, true, true)
                    .SetName("ReceiveBuffer0ControlRegister - Byte Constructor - Bit 4 Set");

            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, ReceiveBufferOperatingMode rxm, bool rxrtr, bool bukt, bool bukt1, bool filhit)
        {
            var register = new ReceiveBuffer0ControlRegister(value);
            Assert.That(register.RXM, Is.EqualTo(rxm));
            Assert.That(register.RXRTR, Is.EqualTo(rxrtr));
            Assert.That(register.BUKT, Is.EqualTo(bukt));
            Assert.That(register.BUKT1, Is.EqualTo(bukt1));
            Assert.That(register.FILHIT, Is.EqualTo(filhit));
        }


        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0110_0100, ReceiveBufferOperatingMode.AcceptAll, true)
                    .SetName("ReceiveBuffer0ControlRegister - Byte Constructor - AcceptAll, true");
                yield return new TestCaseData((byte)0b0100_0100, ReceiveBufferOperatingMode.ExtendedIdentifierFilterMatchesOnly, true)
                    .SetName("ReceiveBuffer0ControlRegister - Byte Constructor - ExtendedIdentifierFilterMatchesOnly, true");
                yield return new TestCaseData((byte)0b0010_0100, ReceiveBufferOperatingMode.StandardIdentifierFilterMatchesOnly, true)
                    .SetName("ReceiveBuffer0ControlRegister - Byte Constructor - StandardIdentifierFilterMatchesOnly, true");
                yield return new TestCaseData((byte)0b0000_0100, ReceiveBufferOperatingMode.AllFilterMatches, true)
                    .SetName("ReceiveBuffer0ControlRegister - Byte Constructor - AllFilterMatches, true");
                yield return new TestCaseData((byte)0b0110_0000, ReceiveBufferOperatingMode.AcceptAll, false)
                    .SetName("ReceiveBuffer0ControlRegister - Byte Constructor - AcceptAll, false");

            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, ReceiveBufferOperatingMode rxm, bool bukt)
        {
            var register = new ReceiveBuffer0ControlRegister(rxm, bukt);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }
    }
}
