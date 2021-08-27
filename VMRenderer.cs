using System;
using System.Linq;

namespace Risc_16 {
    
    class VMRenderer {

        private readonly IVirtualMachine _vm;

        public VMRenderer(IVirtualMachine vm) {
            Console.CursorVisible = false;
            Console.SetWindowPosition(0,0);
            Console.SetWindowSize(Console.LargestWindowWidth,Console.LargestWindowHeight);
            _vm = vm;
        }

        private void Write(int col, int row, object obj) {
            Console.SetCursorPosition(col, row);
            Console.WriteLine(obj.ToString());
        }

        private void Output_register(int col, int row)
        {
            Console.SetCursorPosition(col, row);

            for(int r = 0; r < _vm.Registers.Length; r++) {
                Write(col, row +r, $"R[{r}]: {_vm.Registers[r].ToBits().Render()} {_vm.Registers[r]} ");
            }
        }
        
        private ushort[] lastMemory = new ushort[255];

        private void Output_memory(int col, int row)
        {
            Console.SetCursorPosition(col, row);
            var rowt = row;
            col = col - 32;

            var bck = Console.BackgroundColor;
            var fore = Console.ForegroundColor;

            for(int address = 0; address < _vm.Memory.Length; address++) {

                if ((address % 48) == 0) {
                    col = col + 32;
                    rowt = row;
                }
                rowt += 1;

                if(address == _vm.ProgramCounter) {
                    Console.BackgroundColor = ConsoleColor.Red;
                }

                if((lastMemory != null) && (lastMemory[address] != _vm.Memory[address])) {
                    Console.BackgroundColor = ConsoleColor.Blue;
                }

                Write(col, rowt, $"[{address:000}] {_vm.Memory[address].ToBits().Render()} {_vm.Memory[address]:000000}");

                Console.ForegroundColor = fore;
                Console.BackgroundColor = bck;

            }


            lastMemory = _vm.Memory.ToArray();

        }
        
        public void Render() {
            Write(0, 0, $"PC   : {_vm.ProgramCounter}");
            Write(0, 1, $"{_vm.CurrentInstruction}");
            if (_vm.CurrentInstruction != null)
            {
                Write(0, 3, $"{_vm.OpCodes[_vm.CurrentInstruction.opCode]}    ");
            }

            Output_register(0, 4);
            Output_memory(48, 0);
         }
    }
}
