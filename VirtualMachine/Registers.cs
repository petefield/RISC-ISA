using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risc16 {
    class Registers
    {

        private readonly uint[] _registers;

        public Registers(int registers)
        {
            _registers = new uint[registers];
        }

        public uint this[int i]
        {
            get => _registers[i];
            set => _registers[i] = i == 0 ? (int)0 : value;
        }

        public uint[] AsArray() => _registers;
    }
}
