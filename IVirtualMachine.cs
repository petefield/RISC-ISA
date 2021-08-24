using System;

namespace Risc_16 {
    public interface IVirtualMachine {

        public int ProgramCounter { get; }
        public int[] Registers { get; }
        public ushort[] Memory { get; }
        public Instruction CurrentInstruction { get; }

        public Action OnTick { get; set;}
    }
}
