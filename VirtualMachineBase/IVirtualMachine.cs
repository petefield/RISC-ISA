#nullable enable

namespace VirtualMachineBase {
    public interface IVirtualMachine {
        public int ProgramCounter { get; }
        public uint[] Registers { get; }
        public byte[] Memory { get; }
        public Instruction? CurrentInstruction { get; }
        public int Run();
        public void Load(byte[] data);
        public string OperationCodeDescription(int opcode);
    }
}
