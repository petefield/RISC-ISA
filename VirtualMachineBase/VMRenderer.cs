using System;
using System.Collections;
using System.Linq;
using VirtualMachineBase.BinaryUtilities;

namespace VirtualMachineBase {
    public class VmRenderer {

        private readonly IVirtualMachine _vm;
        private byte[] _lastMemory;

        public VmRenderer(IVirtualMachine vm) {
            Console.CursorVisible = false;
            Console.SetWindowPosition(0,0);
            Console.SetWindowSize(Console.LargestWindowWidth,Console.LargestWindowHeight);
            _vm = vm;
            _lastMemory = new Byte[_vm.Memory.GetUpperBound(0)];

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

        private void Output_memory_words(int col, int row)
        {
            Console.SetCursorPosition(col, row);
            var rowt = row;
            var bck = Console.BackgroundColor;
            var fore = Console.ForegroundColor;

            var byteCount = _vm.Memory.GetUpperBound(0);

            for(int address = 0; address <= byteCount; address+=4) {

                if (address == _vm.ProgramCounter) {
                    Console.BackgroundColor = ConsoleColor.Green;
                }

                var word = _vm.Memory[address..(address + 4)];

                Write(col, rowt++, $"[{address:000}] {word.Render()} {ValueConvertor.ToUInt(word):0000}");

                if (address == 128)
                {
                    rowt = 0;
                    col += 64;
                }

                Console.ForegroundColor = fore;
                Console.BackgroundColor = bck;
            }

            _lastMemory = _vm.Memory.ToArray();
        }


        public void Render() {
            Write(0, 0, $"PC   : {_vm.ProgramCounter}");
            Write(0, 1, $"{_vm.CurrentInstruction}");
            if (_vm.CurrentInstruction != null)
            {
                Write(0, 3, $"{_vm.OperationCodeDescription(_vm.CurrentInstruction.OpCode)}    ");
            }

            Output_register(0, 4);
            Output_memory_words(64, 0);
         }
    }
}
