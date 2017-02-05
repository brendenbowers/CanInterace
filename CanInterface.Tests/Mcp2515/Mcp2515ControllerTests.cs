using CanInterface.MCP2515;
using CanInterface.MCP2515.BitStructures;
using CanInterface.MCP2515.Enum;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.Tests.Mcp2515
{
    [TestFixture(Category = "Unit Test")]
    public class Mcp2515ControllerTests
    {
        protected Mock<IDevice> Device;
        protected Mcp2515Controller Controller;


        [SetUp]
        public void Setup()
        {
            Device = new Mock<IDevice>();
            Controller = new Mcp2515Controller(Device.Object);
        }


        [TestCase(Registers.BFPCTRL,  (byte)0b0001_0001, TestName = "Mcp2515ControllerReadRegisterData - BFPCTRL")]
        [TestCase(Registers.EFLG,     (byte)0b0001_0001, TestName = "Mcp2515ControllerReadRegisterData - EFLG")]
        [TestCase(Registers.CANSTAT,  (byte)0b0001_0001, TestName = "Mcp2515ControllerReadRegisterData - CANSTAT")]
        [TestCase(Registers.CANCTRL,  (byte)0b0001_0001, TestName = "Mcp2515ControllerReadRegisterData - CANCTRL")]
        [TestCase(Registers.CANINTE,  (byte)0b0001_0001, TestName = "Mcp2515ControllerReadRegisterData - CANINTE")]
        [TestCase(Registers.CANINTF,  (byte)0b0001_0001, TestName = "Mcp2515ControllerReadRegisterData - CANINTF")]
        [TestCase(Registers.TXB0CTRL, (byte)0b0001_0001, TestName = "Mcp2515ControllerReadRegisterData - TXB0CTRL")]
        [TestCase(Registers.TXB1CTRL, (byte)0b0001_0001, TestName = "Mcp2515ControllerReadRegisterData - TXB1CTRL")]
        [TestCase(Registers.TXB2CTRL, (byte)0b0001_0001, TestName = "Mcp2515ControllerReadRegisterData - TXB2CTRL")]
        [TestCase(Registers.RXB0CTRL, (byte)0b0001_0001, TestName = "Mcp2515ControllerReadRegisterData - RXB0CTRL")]
        [TestCase(Registers.RXB1CTRL, (byte)0b0001_0001, TestName = "Mcp2515ControllerReadRegisterData - RXB1CTRL")]
        public void Mcp2515ControllerReadRegister(byte register, byte expectedOutput)
        {
            byte[] inputData = null;
            Device.Setup(m => m.Write(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Callback<byte[], byte[]>(
                (input, output) => {
                    inputData = input;
                    output[0] = expectedOutput;
                });

            Assert.That(() => Controller.ReadRegister(register), Is.EqualTo(expectedOutput));
            Assert.That(inputData, Is.EqualTo(new[] { Commands.READ, register }));
        }

        [TestCase(Registers.BFPCTRL, new byte[] {1, 0, 1 }, TestName = "Mcp2515ControllerWriteRegister - BFPCTRL")]
        [TestCase(Registers.CANINTF, new byte[] { 1, 1, 1 }, TestName = "Mcp2515ControllerWriteRegister - CANINTF")]
        [TestCase(Registers.CANCTRL, new byte[] { 1, 1, 0 }, TestName = "Mcp2515ControllerWriteRegister - CANCTRL")]
        public void Mcp2515ControllerWriteRegister(byte register, byte[] values)
        {
            byte[] expected = new byte[values.Length + 2];
            expected[0] = Commands.WRITE;
            expected[1] = register;
            Array.ConstrainedCopy(values, 0, expected, 2, values.Length);

            byte[] dataWritten = null;
            Device.Setup(m => m.Write(It.IsAny<byte[]>())).Callback<byte[]>((toWrite) => dataWritten = toWrite);

            Assert.That(() => Controller.WriteRegister(register, values), Throws.Nothing);
            Assert.That(dataWritten, Is.EqualTo(expected));
        }

        [TestCase]
        public void  Mcp2515ControllerReadStatus()
        {
            var expectedOuput = new ReadStatusInstructionResponse(0b0101_1010);
            byte[] inputData = null;
            Device.Setup(m => m.Write(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Callback<byte[], byte[]>(
                    (input, output) => {
                        inputData = input;
                        output[0] = 0b0101_1010;
                    });

            var status = Controller.ReadStatus();

            Assert.That(status.CANINTFL_RX1IF, Is.EqualTo(expectedOuput.CANINTFL_RX1IF));
            Assert.That(status.CANINTF_RX0IF, Is.EqualTo(expectedOuput.CANINTF_RX0IF));
            Assert.That(status.CANINTF_TX0IF, Is.EqualTo(expectedOuput.CANINTF_TX0IF));
            Assert.That(status.CANINTF_TX1IF, Is.EqualTo(expectedOuput.CANINTF_TX1IF));
            Assert.That(status.CANINTF_TX2IF, Is.EqualTo(expectedOuput.CANINTF_TX2IF));
            Assert.That(status.TXB0CNTRL_TXREQ, Is.EqualTo(expectedOuput.TXB0CNTRL_TXREQ));
            Assert.That(status.TXB1CNTRL_TXREQ, Is.EqualTo(expectedOuput.TXB1CNTRL_TXREQ));
            Assert.That(status.TXB1CNTRL_TXREQ, Is.EqualTo(expectedOuput.TXB1CNTRL_TXREQ));
        }

        [TestCase()]
        public void Mcp2515ControllerReadRxBuffer()
        {

        }
    }
}
