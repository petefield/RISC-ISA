using System;
using System.Collections;

namespace Risc_16 {
    public class Risc16Vm : IVirtualMachine 
    {
        private int[] _register = new int[8];
        private ushort[] _memory = new ushort[128];
        private int _pc = 0;
        private Instruction _instruction;
        private Action _onTick;

        public Risc16Vm(Action onTick) {
            _onTick = onTick;
        }

        public int ProgramCounter => _pc;

        public int[] Registers => _register;

        public ushort[] Memory => _memory;

        public Instruction CurrentInstruction => _instruction;

        public Action OnTick { 
            get => _onTick;
            set => _onTick = value;
        }

        private void Decode(int address) { 

            BitArray instructionData = new BitArray( BitConverter.GetBytes(_memory[address]));
            var opcode = instructionData.GetBitsAs<int>(0, 3);

            switch (opcode)
            {
                case 0:
                case 2:
                    _instruction = new RRRInstruction(opcode,instructionData);
                    break;
                case 3:
                    _instruction =  new RIInstruction(opcode,instructionData);
                    break;
                case 1:
                case 4:
                case 5:
                case 6:
                case 7:
                    _instruction = new RRIInstruction(opcode,instructionData);
                    break;
                default:
                    throw new Exception();
            }

        }

        private void Exec<T>(T Instruction, Action<T> action) where T : Instruction => action(Instruction);

        private void Execute()
        {
            switch(_instruction.opCode)
            {
                case 0: //ADD
                    Exec((RRRInstruction)_instruction, i => _register[i.regA] = _register[i.regB] + _register[i.regC]);
                    break;
                case 1: //ADDI
                    Exec((RRIInstruction)_instruction, i => _register[i.regA] = _register[i.regB] + i.Immediate);
                    break;
                case 2: //NAND
                    Exec((RRRInstruction)_instruction, i => {
                        var b = new BitArray(_register[i.regB]);
                        var c = new BitArray(_register[i.regC]);

                        var result = b.Nand(c);

                        _register[i.regA] = result.GetBitsAs<int>(0, result.Length);
                    });
                    break;
                case 3: //LUI
                    Exec((RIInstruction)_instruction, i => {
                        var b = new BitArray(i.Immediate);
                        _register[i.regA] = b.GetBitsAs<int>(0,10);
                    });
                    break;
                case 4: //SW
                    Exec((RRIInstruction)_instruction, i => _memory[i.regB + i.Immediate] = (ushort)_register[i.regA]);
                    break;
                case 5: //LW
                    Exec((RRIInstruction)_instruction, i => _register[i.regA] = _memory[i.regB + i.Immediate]);
                    break;
                case 6: //BEQ
                    Exec((RRIInstruction)_instruction, i =>{
                        if(_register[i.regA] == _register[i.regB]) {
                            _pc = _pc + 1 + i.Immediate;
                        }
                    });
                    break;
                case 7: //JALR
                    Exec((RRIInstruction)_instruction, i=> {
                        _register[i.regA] = _pc + 1;
                        _pc = _register[i.regB];
                     });
                    break;
                default:
                    throw new InvalidOperationException($"Op code {_instruction.opCode} not known.)");
            }
         }

        public void Run()
        {
            while (true){
                try {
                    Decode(_pc);
                    Execute();
                    _onTick?.Invoke();
                    _pc += 1;
                }
                catch(Exception) { 
                    _pc = 0;
                }
            }
        }
    }
}
