using System.Collections;
using System.Text;
using VirtualMachineBase.BinaryUtilities;

namespace VirtualMachineBase
{
    public abstract class Instruction
    {

        public byte[] raw;

        public void Decode(byte[] data)
        {
            raw = data;
            RegA = data[0];
            DecodeInternal(data);
        }

        public abstract void DecodeInternal(byte[] data);

        public int OpCode { get; set; }
        public int RegA { get; set; }
    }
}
