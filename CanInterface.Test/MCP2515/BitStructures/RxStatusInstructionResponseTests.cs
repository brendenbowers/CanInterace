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
    public class RxStatusInstructionResponseTests
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0000_0000, false, false, MessageType.StandardData, FilterMatch.RX1RXF0)
                    .SetName("RxStatusInstructionResponseTests - Byte Constructor - false, false, StandardData, RX1RXF0");
                yield return new TestCaseData((byte)0b0000_1000, false, false, MessageType.StandardRemote, FilterMatch.RX1RXF0)
                    .SetName("RxStatusInstructionResponseTests - Byte Constructor - false, false, StandardRemote, RX1RXF0");
                yield return new TestCaseData((byte)0b0001_0000, false, false, MessageType.ExtendedData, FilterMatch.RX1RXF0)
                    .SetName("RxStatusInstructionResponseTests - Byte Constructor - false, false, ExtendedData, RX1RXF0");
                yield return new TestCaseData((byte)0b0001_1000, false, false, MessageType.ExtendedRemote, FilterMatch.RX1RXF0)
                    .SetName("RxStatusInstructionResponseTests - Byte Constructor - false, false, ExtendedRemote, RX1RXF0");
                
                yield return new TestCaseData((byte)0b0000_0001, false, false, MessageType.StandardData, FilterMatch.RX1RXF1)
                    .SetName("RxStatusInstructionResponseTests - Byte Constructor - false, false, StandardData, RX1RXF1");
                yield return new TestCaseData((byte)0b0000_0010, false, false, MessageType.StandardData, FilterMatch.RX1RXF2)
                    .SetName("RxStatusInstructionResponseTests - Byte Constructor - false, false, StandardData, RX1RXF2");
                yield return new TestCaseData((byte)0b0000_0011, false, false, MessageType.StandardData, FilterMatch.RX1RXF3)
                    .SetName("RxStatusInstructionResponseTests - Byte Constructor - false, false, StandardData, RX1RXF3");
                yield return new TestCaseData((byte)0b0000_0100, false, false, MessageType.StandardData, FilterMatch.RX1RXF4)
                    .SetName("RxStatusInstructionResponseTests - Byte Constructor - false, false, StandardData, RX1RXF4");
                yield return new TestCaseData((byte)0b0000_0101, false, false, MessageType.StandardData, FilterMatch.RX1RXF5)
                    .SetName("RxStatusInstructionResponseTests - Byte Constructor - false, false, StandardData, RX1RXF5");
                yield return new TestCaseData((byte)0b0000_0110, false, false, MessageType.StandardData, FilterMatch.RX0RXF0)
                    .SetName("RxStatusInstructionResponseTests - Byte Constructor - false, false, StandardData, RX0RXF0");
                yield return new TestCaseData((byte)0b0000_0111, false, false, MessageType.StandardData, FilterMatch.RX0RXF1)
                    .SetName("RxStatusInstructionResponseTests - Byte Constructor - false, false, StandardData, RX0RXF1");

                yield return new TestCaseData((byte)0b1100_0000, true, true, MessageType.StandardData, FilterMatch.RX1RXF0)
                    .SetName("RxStatusInstructionResponseTests - Byte Constructor - true, true, StandardData, RX1RXF0");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, bool rxb0HasMessage, bool rxb1HasMessage, MessageType messageType, FilterMatch filterMatch)
        {
            var register = new RxStatusInstructionResponse(value);
            Assert.That(register.RXB0HasMessage, Is.EqualTo(rxb0HasMessage));
            Assert.That(register.RXB1HasMessage, Is.EqualTo(rxb1HasMessage));
            Assert.That(register.MessageType, Is.EqualTo(messageType));
            Assert.That(register.FilterMatch, Is.EqualTo(filterMatch));
        }
    }
}
