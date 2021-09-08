using System;

using Risc16.Instructions;
using VirtualMachineBase;

namespace Risc16
{
    public class Instruction<T> : IInstruction where T : Instruction, new()
    {
        private readonly Action<T> _execute;
        private readonly T _operation;

        public Instruction(Risc16.VirtualMachine.OpCode opCode, Action<T> execute)
        {
            _execute = execute;
            _operation = new T {OpCode = (int) opCode};
        }

        public void Decode(byte[] data)
        {
            _operation.Decode(data);
        }

        public Instruction Operation => _operation;

        public void Execute()
        {
            _execute(_operation);
        }
    }
}