using System;

namespace Risc_16 {
    
    class VMRenderer {

        private readonly IVirtualMachine _vm;

        public VMRenderer(IVirtualMachine vm) {
            Console.CursorVisible = false;
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
                Write(col, row +r, $"R[{r}]: {_vm.Registers[r]}");
            }
        }

        private void Output_memory(int col, int row)
        {
            Console.SetCursorPosition(col, row);
            var rowt = row;
            col = col - 16;
            for(int address = 0; address < _vm.Memory.Length; address++) {

                if ((address % 32) == 0) {
                    col = col + 16;
                    rowt = row;
                }
                rowt += 1;
                Write(col, rowt, $"[{address:000}] {_vm.Memory[address]:000000}");
            }
        }
        
        public void Render() {
            Write(0, 0, $"_pc   : {_vm.ProgramCounter}");
            Write(0, 1, $"INS  : {_vm.CurrentInstruction}");
            Output_register(0, 3);
            Output_memory(25, 0);
         }
    }
}
