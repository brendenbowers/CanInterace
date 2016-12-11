using CanInterface.MCP2515.BitStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515
{
    public class CanMessage
    {
        private const int CANID_11BITS = 0x7FF;

        public uint CanId { get; set; }
        public bool IsExtended => CanId > CANID_11BITS;
        public bool IsRemote;
        public byte[] Data { get; set; } = new byte[8];

        public CanMessage(RxStandardIdentifierHighRegister sidh, RxStandardIdentifierLowRegister sidl, RxExtendendedIdentifier8Register? eid8, 
            RxExtendendedIdentifier0Register? eid0, RxDataLengthCodeRegister dlc, byte[] data)
        {
            if(sidl.IDE)
            {
                CanId = ((uint)sidh.SIDH << 21) | ((uint)sidl.SIDL << 18) | ((uint)sidl.EID << 16) | ((uint)eid8.Value.EID << 8) | ((uint)eid0.Value.EID);
            }
            else
            {
                CanId = ((uint)sidh.SIDH << 3) | ((uint)sidl.SIDL);
            }

            IsRemote = dlc.RTR;

            if(dlc.DLC != data.Length)
            {
                throw new ArgumentOutOfRangeException("DLC", $"DLC({dlc.DLC}) does not match the length of the read data ({data.Length})");
            }

            Data = data;

        }

        public (TxStandardIdentifierHighRegister sidh, TxStandardIdentifierLowRegister sidl, TxExtendendedIdentifier8Register eid8, 
            TxExtendendedIdentifier0Register eid0, TxDataLengthCodeRegister dlc, byte[] data) ToTransmitRegisters()
        {
            return (new TxStandardIdentifierHighRegister(CanId), new TxStandardIdentifierLowRegister(CanId, IsExtended), new TxExtendendedIdentifier8Register(CanId), 
                new TxExtendendedIdentifier0Register(CanId), new TxDataLengthCodeRegister(IsRemote, Convert.ToByte(Data.Length)), Data);
        }
        
    }
}
