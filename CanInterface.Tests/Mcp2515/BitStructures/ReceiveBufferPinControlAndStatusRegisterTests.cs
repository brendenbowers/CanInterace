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
    public class ReceiveBufferPinControlAndStatusRegisterTests
    {

        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0011_1111, true, true, true, true, true, true)
                    .SetName("ReceiveBufferPinControlAndStatusRegister - Byte Constructor - true, true, true, true, true, true");
                yield return new TestCaseData((byte)0b0000_0000, false, false, false, false, false, false)
                    .SetName("ReceiveBufferPinControlAndStatusRegister - Byte Constructor - false, false, false, false, false, false");
                yield return new TestCaseData((byte)0b1000_0000, false, false, false, false, false, false)
                    .SetName("ReceiveBufferPinControlAndStatusRegister - Byte Constructor - Bit 7 set");
                yield return new TestCaseData((byte)0b0100_0000, false, false, false, false, false, false)
                    .SetName("ReceiveBufferPinControlAndStatusRegister - Byte Constructor - Bite 6 set");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, bool b1bfs, bool b0bfs, bool b1bfe, bool b0bfe, bool b1bfm, bool b0bfm)
        {
            var register = new ReceiveBufferPinControlAndStatusRegister(value);
            Assert.That(register.B1BFS, Is.EqualTo(b1bfs));
            Assert.That(register.B0BFS, Is.EqualTo(b0bfs));
            Assert.That(register.B1BFE, Is.EqualTo(b1bfe));
            Assert.That(register.B0BFE, Is.EqualTo(b0bfe));
            Assert.That(register.B1BFM, Is.EqualTo(b1bfm));
            Assert.That(register.B0BFM, Is.EqualTo(b0bfm));
        }

        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b0011_1111, true, true, true, true, true, true)
                    .SetName("ReceiveBufferPinControlAndStatusRegister - ToByte - true, true, true, true, true, true");
                yield return new TestCaseData((byte)0b0000_0000, false, false, false, false, false, false)
                    .SetName("ReceiveBufferPinControlAndStatusRegister - ToByte - false, false, false, false, false, false");
            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, bool b1bfs, bool b0bfs, bool b1bfe, bool b0bfe, bool b1bfm, bool b0bfm)
        {
            var register = new ReceiveBufferPinControlAndStatusRegister(b1bfs, b0bfs, b1bfe, b0bfe, b1bfm, b0bfm);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }
    }
}
