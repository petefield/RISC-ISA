
using System.Diagnostics;

namespace Risc_16 {

    class Program {

        public static void Main() {
        
            VMRenderer renderer = null;
            int instructions = 0;
            var s = new Stopwatch();

            var vm = new Risc16Vm(null,() => {
                instructions ++;
                // s.Stop();
                // renderer.Render();
                // s.Start();

                //System.Console.ReadKey();
            });

            renderer = new VMRenderer(vm);

            var data = new ushort[] {

                //0b101_001_000_0001000,  //0 LW   1 0 8  - load target from mem [8] into reg [1]
                //0b011_010_1000000000,  //1 LUI   2 1    - Set reg [2] to 1 (increment)
                //0b011_101_0000000011,  //2 LUI   5 011  - set reg [5] t0 3 (loop)
                //0b000_011_011_0000_010, //3 ADD  3 3 0 2 -Add reg [2] (increment) to reg [3] result
                //0b100_011_000_0010000,  //4 SW   3 0 16 - Store reg[3] into mem[16]
                //0b110_001_011_0000001,  //5 BEQ  3 1 1  - If reg[3] (result) == reg[1] (target) goto 7
                //0b111_111_101_0000000,  //6 JALR 7 2 0  - jump to reg[5] (loop)
                //0b111_000_000_0000001,  //7 JALR 0 0 1  - halt
                //0b1000000000000000,     //8 111
            };

            var program = new ushort[255];
            program[0] = 0b_011_001_0000000001;     //0 LUI  1  1         Set reg [1] to 1
            program[1] = 0b_011_011_1100000000;     //1 LUI  3  8         Set reg [3] t0 8
            program[2] = 0b_011_111_0000000101;     //2 LUI  7  4         Set reg [7] t0 5
            program[3] = 0b_010_100_001_0000_001;   //3 NAND 4  1  0  1  Nand reg[1] and reg[1] store result in reg[4]
            program[4] = 0b_001_100_100_0000001;    //4 ADDI 4  4  1      Add 1 to reg[4] and store in reg[4]
            program[5] = 0b_000_011_011_0000_100;   //5 ADD  3  3  0 4    Add reg[4] to reg[3] and store in reg[3]
            program[6] = 0b_100_011_000_0010000;    //6 SW   3  0  16     Store reg[3] into mem[16]
            program[7] = 0b_110_000_011_0000001;    //7 BEQ  3 1 1  -     If reg[3] (result) == reg[1] (target) goto 7
            program[8] = 0b_111_000_111_0000000;    //8 JALR 0 7 0  -     jump to reg[7] (loop)
            program[9] = 0b_111_000_000_0000001;    //9 JALR 0 0 1  -     jump to reg[7] (loop)


            s.Start();
            var returnCode = vm.Run(program);
            s.Stop();

            var duration = s.ElapsedMilliseconds / 1000d;
            
            System.Console.WriteLine($"Duration : {duration} s");
            System.Console.WriteLine($"Instructions : {instructions} ");
            System.Console.WriteLine($"Speed : {(instructions / duration)/1000d:#,##0.00} mhz");
            System.Console.WriteLine(returnCode);
        }
    }
}
