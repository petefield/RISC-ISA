#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using Risc16.Instructions;
using VirtualMachineBase;
using VirtualMachineBase.BinaryUtilities;

namespace Risc16 {
    public class VirtualMachine : IVirtualMachine 
    {
        private readonly ushort[] _register = new ushort[8];
        private ushort _pc;
        private readonly Action? _postDecode;
        private readonly Action? _postExecution;

        public Dictionary<int, string> OpCodes => new Dictionary<int, string>{ 
            {0,"ADD" },
            {1,"ADDI" },
            {2,"NAND" },
            {3,"LUI" },
            {4,"SW" },
            {5,"LW" },
            {6,"BEQ" },
            {7,"JALR" },
        };

        public VirtualMachine(Action? postDecode, Action? postExecution) {
            _postDecode = postDecode;
            _postExecution = postExecution;
        }

        public ushort ProgramCounter => _pc;

        public ushort[] Registers {
            get { 
                _register[0] = 0;
                return _register;
            }
        } 

        public ushort[] Memory { get; } = new ushort[255];

        public Instruction? CurrentInstruction { get; private set; }

        private void Decode(int address) { 

            BitArray instructionData = new BitArray( BitConverter.GetBytes(Memory[address]));
            var opCode = instructionData.GetBitsAs<int>(0, 3);
            switch (opCode)
            {
                case 0: case 2:
                    CurrentInstruction = new RrrInstruction(opCode,instructionData);
                    break;
                case 3:
                    CurrentInstruction = new RiInstruction(opCode,instructionData);
                    break;
                case 1: case 4: case 5: case 6: case 7:
                    CurrentInstruction = new RriInstruction(opCode,instructionData);
                    break;
                default:
                    throw new Exception();
            }
        }

        private static void Exec<T>(Instruction instruction, Action<T> action) where T : Instruction 
            => action((T)instruction);

        private void Execute()
        {
            _register[0] = 0;
            switch(CurrentInstruction?.OpCode)
            {
                case 0: //ADD
                      Exec<RrrInstruction>(CurrentInstruction, i =>{
                        _register[i.RegA] = (ushort)(_register[i.RegB] + _register[i.RegC]); 
                        _pc += 1;
                    });
                    break;
                case 1: //ADDI
                    Exec<RriInstruction>(CurrentInstruction, i => {
                        _register[i.RegA] = (ushort)(_register[i.RegB] + i.Immediate);
                        _pc += 1;
                    });
                    break;
                case 2: //NAND
                    Exec<RrrInstruction>(CurrentInstruction, i => {
                        var result = _register[i.RegB].ToBits().Nand(_register[i.RegC].ToBits());
                        _register[i.RegA] = result.GetBitsAs<ushort>(0, 16);
                        _pc += 1;
                    });
                    break;
                case 3: //LUI
                    Exec<RiInstruction>(CurrentInstruction, i => {
                        var b = i.Immediate.ToBits();
                        b.Reverse();
                        _register[i.RegA] = b.GetBitsAs<ushort>(6,16);
                        _pc++;
                    });
                    break;
                case 4: //SW
                    Exec<RriInstruction>(CurrentInstruction, i => {
                        Memory[i.RegB + i.Immediate] = _register[i.RegA]; 
                        _pc += 1;
                    });
                    break;
                case 5: //LW
                    Exec<RriInstruction>(CurrentInstruction, i => {
                        _register[i.RegA] = Memory[i.RegB + i.Immediate]; 
                        _pc++;
                    });
                    break;
                case 6: //BEQ
                    Exec<RriInstruction>(CurrentInstruction, i =>
                    {
                        _pc = _register[i.RegA] == _register[i.RegB]
                            ? (ushort) i.Immediate
                            : (ushort) (_pc + 1);
                    });
                    break;
                case 7: //JALR
                    Exec<RriInstruction>(CurrentInstruction, i=> { 
                        if (i.Immediate != 0) {
                            throw new InvalidOperationException("HALT");
                        }
                        _register[i.RegA] = (ushort)(_pc + 1);
                        _pc = _register[i.RegB];
                     });
                    break;
                default:
                    throw new InvalidOperationException($"Op code {CurrentInstruction?.OpCode} not known.)");
            }
         }

        public void Load(ushort[] data) => data.CopyTo(Memory, 0);


        public int Run()
        {
            while (true){
                try {
                    Decode(_pc);
                    _postDecode?.Invoke();
                    Execute();
                    _postExecution?.Invoke();
                }
                catch(Exception ex) { 
                    if(ex.Message == "HALT") {
                        return 0;
                    }
                    return -1;
                }
            }
        }
    }
}
