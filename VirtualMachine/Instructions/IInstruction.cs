using System.Collections;
using VirtualMachineBase;

namespace Risc16.Instructions {
    public interface IInstruction
    {
        Instruction Operation { get; }
        public void Execute();
        public void Decode(byte[] operands);
    }
}
