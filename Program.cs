
namespace Risc_16 {

    class Program {

        public static void Main() {
        
            VMRenderer renderer = null;

            var vm = new Risc16Vm(() => {
                renderer.Render();
                System.Console.ReadKey();
            });

            renderer = new VMRenderer(vm);

            vm.Memory[0] = 0b101_001_000_0001000;
            vm.Memory[1] = 0b101_010_000_0001001;
            vm.Memory[2] = 0b000_011_001_0000_010;
            vm.Memory[3] = 0b100_011_000_0000111;
            vm.Memory[4] = 0b111_111_000_0000000;
            vm.Memory[8] = 0b1000;
            vm.Memory[9] = 0b1000;
            vm.Run();
        }
    }
}
