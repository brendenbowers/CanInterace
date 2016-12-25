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
    public class ReadStatusInstructionResponseTests
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b1111_1111, true, true, true, true, true, true, true, true)
                    .SetName("ReadStatusInstructionResponse - Byte Constructor - true, true, true, true, true, true, true, true");
                yield return new TestCaseData((byte)0b0000_0000, false, false, false, false, false, false, false, false)
                    .SetName("ReadStatusInstructionResponse - Byte Constructor - false, false, false, false, false, false, false, false");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, bool canintf_rx0if, bool canintf_rx1if, bool txb0cntrl_txreq, bool canintf_tx0if, bool txb1cntrl_txreq, bool canintf_tx1if, bool txb2cntrl_txreq, bool canintf_tx2if)
        {
            var register = new ReadStatusInstructionResponse(value);
            Assert.That(register.CANINTF_RX0IF, Is.EqualTo(canintf_rx0if));
            Assert.That(register.CANINTFL_RX1IF, Is.EqualTo(canintf_rx1if));
            Assert.That(register.TXB0CNTRL_TXREQ, Is.EqualTo(txb0cntrl_txreq));
            Assert.That(register.CANINTF_TX0IF, Is.EqualTo(canintf_tx0if));
            Assert.That(register.TXB1CNTRL_TXREQ, Is.EqualTo(txb1cntrl_txreq));
            Assert.That(register.CANINTF_TX1IF, Is.EqualTo(canintf_tx1if));
            Assert.That(register.TXB2CNTRL_TXREQ, Is.EqualTo(txb2cntrl_txreq));
            Assert.That(register.CANINTF_TX2IF, Is.EqualTo(canintf_tx2if));
        }

    }
}
