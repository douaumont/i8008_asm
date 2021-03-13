using System;

namespace i8008_asm
{
    class MainClass
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage of the assembler: the first argument is an input file name, the second argument is an output file name. The second argument is not required, therefore if it is not presented, the output file name shall be \"a.bin\"");
            }
            else
            {
                Assembler assembler;
                switch(args.Length)
                {
                    case 1:
                        assembler = new Assembler(args[0], "a.bin");
                        assembler.Translate();
                        break;
                    case 2:
                        assembler = new Assembler(args[0], args[1]);
                        assembler.Translate();
                        break;
                    default:
                        Console.WriteLine("Too many arguments!");
                        break;
                }
            }
            
        }
    }
}
