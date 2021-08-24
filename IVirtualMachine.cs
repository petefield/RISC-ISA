using System;
using System.Collections.Generic;

namespace Risc_16 {
    public interface IVirtualMachine {

        public ushort ProgramCounter { get; }
        public ushort[] Registers { get; }
        public ushort[] Memory { get; }
        public Instruction CurrentInstruction { get; }
        Dictionary<int, string> OpCodes { get; }
        public int Run(ushort[] data);
    }
}
