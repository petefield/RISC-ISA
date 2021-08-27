#nullable enable
using System.Collections.Generic;

namespace VirtualMachineBase {
    public interface IVirtualMachine {
        public ushort ProgramCounter { get; }
        public ushort[] Registers { get; }
        public ushort[] Memory { get; }
        public Instruction? CurrentInstruction { get; }
        Dictionary<int, string> OpCodes { get; }
        public int Run();
        public void Load(ushort[] data);
    }
}
