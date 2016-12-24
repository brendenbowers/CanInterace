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
    public class CanInterruptEnableRegisterTests
    {
        public static IEnumerable<TestCaseData> ByteConstructorTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b1111_1111, true, true, true, true, true, true, true, true)
                    .SetName("CanInterruptEnableRegister - Byte Constructor - True, True, True, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0111_1111, false, true, true, true, true, true, true, true)
                    .SetName("CanInterruptEnableRegister - Byte Constructor - false, True, True, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0011_1111, false, false, true, true, true, true, true, true)
                    .SetName("CanInterruptEnableRegister - Byte Constructor - false, false, True, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0001_1111, false, false, false, true, true, true, true, true)
                    .SetName("CanInterruptEnableRegister - Byte Constructor - false, false, false, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0000_1111, false, false, false, false, true, true, true, true)
                    .SetName("CanInterruptEnableRegister - Byte Constructor - false, false, false, false, True, True, True, True");
                yield return new TestCaseData((byte)0b0000_0111, false, false, false, false, false, true, true, true)
                    .SetName("CanInterruptEnableRegister - Byte Constructor - false, false, false, false, false, True, True, True");
                yield return new TestCaseData((byte)0b0000_0011, false, false, false, false, false, false, true, true)
                    .SetName("CanInterruptEnableRegister - Byte Constructor - false, false, false, false, false, false True, True");
                yield return new TestCaseData((byte)0b0000_0001, false, false, false, false, false, false, false, true)
                    .SetName("CanInterruptEnableRegister - Byte Constructor - false, false, false, false, false, false, false, True");
                yield return new TestCaseData((byte)0b0000_0000, false, false, false, false, false, false, false, false)
                    .SetName("CanInterruptEnableRegister - Byte Constructor - false, false, false, false, false, false, false, false");
            }
        }

        [TestCaseSource(nameof(ByteConstructorTestSource))]
        public void ByteConstructorTest(byte value, bool merre, bool wakie, bool errie, bool tx2ie, bool tx1ie, bool tx0ie, bool rx1ie, bool rx0ie)
        {
            var register = new CanInterruptEnableRegister(value);
            Assert.That(register.MERRE, Is.EqualTo(merre));
            Assert.That(register.WAKIE, Is.EqualTo(wakie));
            Assert.That(register.ERRIE, Is.EqualTo(errie));
            Assert.That(register.TX2IE, Is.EqualTo(tx2ie));
            Assert.That(register.TX1IE, Is.EqualTo(tx1ie));
            Assert.That(register.TX0IE, Is.EqualTo(tx0ie));
            Assert.That(register.RX1IE, Is.EqualTo(rx1ie));
            Assert.That(register.RX0IE, Is.EqualTo(rx0ie));
        }


        public static IEnumerable<TestCaseData> ToByteTestSource
        {
            get
            {
                yield return new TestCaseData((byte)0b1111_1111, true, true, true, true, true, true, true, true)
                    .SetName("CanInterruptEnableRegister - ToByte - True, True, True, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0111_1111, false, true, true, true, true, true, true, true)
                    .SetName("CanInterruptEnableRegister - ToByte - false, True, True, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0011_1111, false, false, true, true, true, true, true, true)
                    .SetName("CanInterruptEnableRegister - ToByte - false, false, True, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0001_1111, false, false, false, true, true, true, true, true)
                    .SetName("CanInterruptEnableRegister - ToByte - false, false, false, True, True, True, True, True");
                yield return new TestCaseData((byte)0b0000_1111, false, false, false, false, true, true, true, true)
                    .SetName("CanInterruptEnableRegister - ToByte - false, false, false, false, True, True, True, True");
                yield return new TestCaseData((byte)0b0000_0111, false, false, false, false, false, true, true, true)
                    .SetName("CanInterruptEnableRegister - ToByte - false, false, false, false, false, True, True, True");
                yield return new TestCaseData((byte)0b0000_0011, false, false, false, false, false, false, true, true)
                    .SetName("CanInterruptEnableRegister - ToByte - false, false, false, false, false, false True, True");
                yield return new TestCaseData((byte)0b0000_0001, false, false, false, false, false, false, false, true)
                    .SetName("CanInterruptEnableRegister - ToByte - false, false, false, false, false, false, false, True");
                yield return new TestCaseData((byte)0b0000_0000, false, false, false, false, false, false, false, false)
                    .SetName("CanInterruptEnableRegister - ToByte - false, false, false, false, false, false, false, false");
            }
        }

        [TestCaseSource(nameof(ToByteTestSource))]
        public void ToByteTest(byte expectedValue, bool merre, bool wakie, bool errie, bool tx2ie, bool tx1ie, bool tx0ie, bool rx1ie, bool rx0ie)
        {
            var register = new CanInterruptEnableRegister(merre, wakie, errie, tx2ie, tx1ie,tx0ie, rx1ie, rx0ie);
            Assert.That(register.ToByte(), Is.EqualTo(expectedValue));
        }
    }
}
