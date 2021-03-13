using System;
using System.Collections.Generic;
using System.IO;


namespace i8008_asm
{
    class Assembler
    {
        private BinaryWriter outputFile;
        private StreamReader inputFile;
        bool inputFileOpened = true, outputFileOpened = true; 

        private List<string> parsedProgram;

        private Dictionary<char, byte> letterToNumber = new Dictionary<char, byte>();
        private Dictionary<string, Int16> labels = new Dictionary<string, Int16>();
        private Dictionary<string, byte> constants = new Dictionary<string, byte>();

        private Command[] commands= new Command[] 
        {
            new Command("rlc", 1, 0x02), new Command("rfc", 1, 0x03), new Command("adi", 2, 0x04), new Command("lai", 2, 0x06), new Command("ret", 1, 0x07), new Command("inb", 1, 0x08), new Command("dcb", 1, 0x09), new Command("rrc", 1, 0x0A), new Command("rfz", 1, 0x0B), new Command("aci", 2, 0x0C), new Command("lbi", 2, 0x0E),
            new Command("inc", 1, 0x10), new Command("dcc", 1, 0x11), new Command("ral", 1, 0x12), new Command("rfs", 1, 0x13), new Command("sui", 2, 0x14), new Command("lci", 2, 0x16), new Command("ind", 1, 0x18), new Command("dcd", 1, 0x19), new Command("rar", 1, 0x1A), new Command("rfp", 1, 0x1B), new Command("sbi", 2, 0x1C), new Command("ldi", 2, 0x1E), 
            new Command("ine", 1, 0x20), new Command("dce", 1, 0x21), new Command("rtc", 1, 0x23), new Command("ndi", 2, 0x24), new Command("lei", 2, 0x26), new Command("inh", 1, 0x28), new Command("dch", 1, 0x29), new Command("rtz", 1, 0x2B), new Command("xri", 2, 0x2C), new Command("lhi", 2, 0x2E), 
            new Command("inl", 1, 0x30), new Command("dcl", 1, 0x31), new Command("rts", 1, 0x33), new Command("ori", 2, 0x34), new Command("lli", 2, 0x36), new Command("rtp", 1, 0x3B), new Command("cpi", 2, 0x3C), new Command("lmi", 2, 0x3E), 
            new Command("jfc", 3, 0x40), new Command("cfc", 3, 0x42), new Command("jmp", 3, 0x44), new Command("cal", 3, 0x46), new Command("jfz", 3, 0x48), new Command("cfz", 3, 0x4A),
            new Command("jfs", 3, 0x50), new Command("cfs", 3, 0x52), new Command("jfp", 3, 0x58), new Command("cfp", 3, 0x5A), 
            new Command("jtc", 3, 0x60), new Command("ctc", 3, 0x62), new Command("jtz", 3, 0x68), new Command("ctz", 3, 0x6A), 
            new Command("jts", 3, 0x70), new Command("cts", 3, 0x72), new Command("jtp", 3, 0x78), new Command("ctp", 3, 0x7A), 
            new Command("ada", 1, 0x80), new Command("adb", 1, 0x81), new Command("adc", 1, 0x82), new Command("add", 1, 0x83), new Command("ade", 1, 0x84), new Command("adh", 1, 0x85), new Command("adl", 1, 0x86), new Command("adm", 1, 0x87), new Command("aca", 1, 0x88), new Command("acb", 1, 0x89), new Command("acc", 1, 0x8A), new Command("acd", 1, 0x8B), new Command("ace", 1, 0x8C), new Command("ach", 1, 0x8D), new Command("acl", 1, 0x8E), new Command("acm", 1, 0x8F),
            new Command("sua", 1, 0x90), new Command("sub", 1, 0x91), new Command("suc", 1, 0x92), new Command("sud", 1, 0x93), new Command("sue", 1, 0x94), new Command("suh", 1, 0x95), new Command("sul", 1, 0x96), new Command("sum", 1, 0x97), new Command("sba", 1, 0x98), new Command("sbb", 1, 0x99), new Command("sbc", 1, 0x9A), new Command("sbd", 1, 0x9B), new Command("sbe", 1, 0x9C), new Command("sbh", 1, 0x9D), new Command("sbl", 1, 0x9E), new Command("sbm", 1, 0x9F),
            new Command("nda", 1, 0xA0), new Command("ndb", 1, 0xA1), new Command("ndc", 1, 0xA2), new Command("ndd", 1, 0xA3), new Command("nde", 1, 0xA4), new Command("ndh", 1, 0xA5), new Command("ndl", 1, 0xA6), new Command("ndm", 1, 0xA7), new Command("xra", 1, 0xA8), new Command("xrb", 1, 0xA9), new Command("xrc", 1, 0xAA), new Command("xrd", 1, 0xAB), new Command("xre", 1, 0xAC), new Command("xrh", 1, 0xAD), new Command("xrl", 1, 0xAE), new Command("xrm", 1, 0xAF),
            new Command("ora", 1, 0xB0), new Command("orb", 1, 0xB1), new Command("orc", 1, 0xB2), new Command("ord", 1, 0xB3), new Command("ore", 1, 0xB4), new Command("orh", 1, 0xB5), new Command("orl", 1, 0xB6), new Command("orm", 1, 0xB7), new Command("cpa", 1, 0xB8), new Command("cpb", 1, 0xB9), new Command("cpc", 1, 0xBA), new Command("cpd", 1, 0xBB), new Command("cpe", 1, 0xBC), new Command("cph", 1, 0xBD), new Command("cpl", 1, 0xBE), new Command("cpm", 1, 0xBF),
            new Command("nop", 1, 0xC0), new Command("lab", 1, 0xC1), new Command("lac", 1, 0xC2), new Command("lad", 1, 0xC3), new Command("lae", 1, 0xC4), new Command("lah", 1, 0xC5), new Command("lal", 1, 0xC6), new Command("lam", 1, 0xC7), new Command("lba", 1, 0xC8), new Command("lbb", 1, 0xC9), new Command("lbc", 1, 0xCA), new Command("lbd", 1, 0xCB), new Command("lbe", 1, 0xCC), new Command("lbh", 1, 0xCD), new Command("lbl", 1, 0xCE), new Command("lbm", 1, 0xCF),
            new Command("lca", 1, 0xD0), new Command("lcb", 1, 0xD1), new Command("lcc", 1, 0xD2), new Command("lcd", 1, 0xD3), new Command("lce", 1, 0xD4), new Command("lch", 1, 0xD5), new Command("lcl", 1, 0xD6), new Command("lcm", 1, 0xD7), new Command("lda", 1, 0xD8), new Command("ldb", 1, 0xD9), new Command("ldc", 1, 0xDA), new Command("ldd", 1, 0xDB), new Command("lde", 1, 0xDC), new Command("ldh", 1, 0xDD), new Command("ldl", 1, 0xDE), new Command("ldm", 1, 0xDF),
            new Command("lea", 1, 0xE0), new Command("leb", 1, 0xE1), new Command("lec", 1, 0xE2), new Command("led", 1, 0xE3), new Command("lee", 1, 0xE4), new Command("leh", 1, 0xE5), new Command("lel", 1, 0xE6), new Command("lem", 1, 0xE7), new Command("lha", 1, 0xE8), new Command("lhb", 1, 0xE9), new Command("lhc", 1, 0xEA), new Command("lhd", 1, 0xEB), new Command("lhe", 1, 0xEC), new Command("lhh", 1, 0xED), new Command("lhl", 1, 0xEE), new Command("lhm", 1, 0xEF),
            new Command("lla", 1, 0xF0), new Command("llb", 1, 0xF1), new Command("llc", 1, 0xF2), new Command("lld", 1, 0xF3), new Command("lle", 1, 0xF4), new Command("llh", 1, 0xF5), new Command("lll", 1, 0xF6), new Command("llm", 1, 0xF7), new Command("lma", 1, 0xF8), new Command("lmb", 1, 0xF9), new Command("lmc", 1, 0xFA), new Command("lmd", 1, 0xFB), new Command("lme", 1, 0xFC), new Command("lmh", 1, 0xFD), new Command("lml", 1, 0xFE), new Command("hlt", 1, 0xFF)
        };

        private int currentAddress = 0;
        private int currentStringNumber = 1;
        public Assembler(string pathToInputFile, string pathToOutputFile)
        {
           
            try
            {
                outputFile = new BinaryWriter(File.Open(pathToOutputFile, FileMode.Create));               
            }
            catch
            {
                Console.WriteLine("Cannot open output file!");
                Environment.Exit(0);           
            }

            try
            {
                inputFile = new StreamReader(pathToInputFile);
            }
            catch
            {
                Console.WriteLine("Cannot open input file!");
                Environment.Exit(0);
            } 

            for (byte i = 0; i <= 9; i++)
            {
                letterToNumber.Add((char)('0' + i), i);
            }

            char j = 'a';
            for (byte i = 10; i <= 15; i++, j++)
            {
                letterToNumber.Add(j, i);
            }
            parsedProgram = new List<string>();
        }
        

        private void Parse()
        {
            string currentString = "";
            string[] splittedString;


            while (!inputFile.EndOfStream)
            {
                currentString = inputFile.ReadLine();
                currentStringNumber = 1;

                if (currentString.Contains(';'))
                {
                    currentString = currentString.Remove(currentString.IndexOf(';'));
                }

                splittedString = currentString.Split(new char[] { '\n', '\t', ' ' });
                
                if (splittedString.Length > 0)
                {
                    if (splittedString[0].EndsWith(':'))
                    {
                        //
                        string labelName = splittedString[0].Substring(0, splittedString[0].Length - 1);
                        if (labels.ContainsKey(labelName))
                        {
                            throw new Exception("Double definition of the label \"" + labelName + "\"!");
                        }
                        labels.Add(labelName, (Int16)currentAddress);
                        parsedProgram.Add(string.Empty);
                    }
                    else if (FindCommandByMnemonic(splittedString[0].ToLower()) != null)
                    {
                        currentAddress += FindCommandByMnemonic(splittedString[0].ToLower()).LengthInBytes;
                        parsedProgram.Add(currentString);
                    }
                    else if (splittedString[0].ToLower() == "db" || splittedString[0].ToLower() == "rst" || splittedString[0].ToLower() == "in" || splittedString[0].ToLower() == "out")
                    {
                        currentAddress++;
                        parsedProgram.Add(currentString);
                    }
                    else if (splittedString[0] == "define")
                    {
                        string constantName = splittedString[1];
                        if (constants.ContainsKey(constantName))
                        {
                            throw new Exception("Double definition of the constant \"" + constantName + "\"!");
                        }
                        constants.Add(constantName, (byte)ConvertStringToNumber(splittedString[2]));
                        parsedProgram.Add(string.Empty);
                    }         
                    else
                    {
                        parsedProgram.Add(currentString);
                    }
                }
                currentStringNumber++;
            }

            for (int i = 0; i < parsedProgram.Count; i++)
            {
                currentString = parsedProgram[i];               
                splittedString = currentString.Split(new char[] { '\n', '\t', ' ' });

                if (splittedString.Length > 1)
                {
                    if (labels.ContainsKey(splittedString[1]))
                    {
                        currentString = currentString.Replace(splittedString[1], labels[splittedString[1]].ToString());
                    }
                    else if (constants.ContainsKey(splittedString[1]))
                    {
                        currentString = currentString.Replace(splittedString[1], constants[splittedString[1]].ToString());
                    }
                    else if (splittedString[1].Contains('[') && splittedString[1].Contains(']'))
                    {
                        string[] indexedLabel = splittedString[1].Split(new char[] { '[', ']' });
                        if (labels.ContainsKey(indexedLabel[0]))
                        {
                            byte value;
                            switch(indexedLabel[1].ToLower())
                            {
                                case "h":
                                    value = (byte)((labels[indexedLabel[0]] & 0xFF00) >> 8);
                                    currentString = currentString.Replace(splittedString[1], value.ToString());
                                    break;

                                case "l":
                                    value = (byte)(labels[indexedLabel[0]] & 0xFF);
                                    currentString = currentString.Replace(splittedString[1], value.ToString());
                                    break;

                                default:
                                    throw new Exception("Wrong syntax!");
                            }
                        }
                        else
                        {
                            throw new Exception("Undefined lable!");
                        }
                    }
                }
                parsedProgram[i] = currentString;
            }
        }
        private void Assemble()
        {
            byte[] bytesToWrite = { };
            string[] splittedString;
            for (int i = 0; i < parsedProgram.Count; i++)
            {
                currentStringNumber = i + 1;
                splittedString = parsedProgram[i].Split();
                if (splittedString.Length > 0 && splittedString[0] != string.Empty)
                {
                    Command command = FindCommandByMnemonic(splittedString[0].ToLower());
                    if (command != null)
                    {
                        switch (command.LengthInBytes)
                        {
                            case 1:
                                bytesToWrite = new byte[] { command.Opcode };
                                break;
                            case 2:
                                bytesToWrite = new byte[] { command.Opcode, (byte)ConvertStringToNumber(splittedString[1]) };
                                break;
                            case 3:
                                UInt16 temp = ConvertStringToNumber(splittedString[1]);
                                bytesToWrite = new byte[] { command.Opcode, (byte)(temp & 0xFF), (byte)((temp & 0xFF00) >> 8) };
                                break;
                        }
                    }
                    else if (splittedString[0].ToLower() == "rst")
                    {
                        byte n = (byte)ConvertStringToNumber(splittedString[1]);
                        if (n >= 0 && n <= 7)
                        {
                            bytesToWrite = new byte[] { (byte)(0b00000101 | (n << 3))};
                        }
                        else
                        {
                            throw new Exception("RST instruction's operand must be less or equal to 7 and greater or equal to 0!");
                        }
                    }
                    else if (splittedString[0].ToLower() == "inp")
                    {
                        byte n = (byte)ConvertStringToNumber(splittedString[1]);
                        if (n >= 0 && n <= 7)
                        {
                            bytesToWrite = new byte[] { (byte)(0b01000001 | (n << 1)) };
                        }
                        else
                        {
                            throw new Exception("INP instruction's operand must be less or equal to 7 and greater or equal to 0!");
                        }
                    }
                    else if (splittedString[0].ToLower() == "out")
                    {
                        byte n = (byte)ConvertStringToNumber(splittedString[1]);
                        if (n >= 8 && n <= 31)
                        {
                            bytesToWrite = new byte[] { (byte)(0b01000001 | (n << 1)) };
                        }
                        else
                        {
                            throw new Exception("OUT instruction's operand must be less or equal to 31 and greater or equal to 8!");
                        }
                    }
                    else if (splittedString[0] == "db")
                    {
                        bytesToWrite = new byte[] { (byte)ConvertStringToNumber(splittedString[1]) };
                    }
                    else
                    {
                        throw new Exception("Unknown instruction!");
                    }
                    outputFile.Write(bytesToWrite);
                }               
                bytesToWrite = Array.Empty<byte>();
            }
        }

        public void Translate()
        {
            try
            {
                Parse();
                Assemble();
            }
            catch (Exception excep)
            {
                Console.WriteLine(string.Format("A error has occured in line {0}: {1}", currentStringNumber, excep.Message));
            }
            inputFile.Close();
            outputFile.Close();
        }

        private bool isNumber(string number)
        {
            bool result = true;
            number = number.ToLower();
            char[] numberAsCharArray = number.ToCharArray();
            byte basis;

            if (number.Length > 0)
            {
                if (number.StartsWith("0x"))
                {
                    basis = 16;
                    numberAsCharArray[1] = '0';
                }
                else if (number.StartsWith("0c"))
                {
                    basis = 8;
                    numberAsCharArray[1] = '0';
                }
                else if (number.StartsWith("0b"))
                {
                    basis = 2;
                    numberAsCharArray[1] = '0';
                }
                else if (number.StartsWith("0d"))
                {
                    basis = 10;
                    numberAsCharArray[1] = '0';
                }
                else
                {
                    basis = 10;
                }

                number = new string(numberAsCharArray);

                foreach (char digit in number)
                {
                    if (letterToNumber.ContainsKey(digit))
                    {
                        if (letterToNumber[digit] >= basis)
                        {
                            result = false;
                        }
                    }
                    if (!letterToNumber.ContainsKey(digit))
                    {
                        result = false;
                    }
                }
            }
            else
            {
                result = false;
            }


            return result;
        }

        public UInt16 ConvertStringToNumber(string number)
        {
            string temp = number.Substring(1);
            int basis = 10;
            switch(number[0])
            {
                case '#':
                    basis = 10;
                    break;
                case '$':
                    basis = 16;
                    break;
                case '%':
                    basis = 2;
                    break;
                default:
                    temp = number;
                    basis = 10;
                    break;
            }
            return (UInt16)Convert.ToInt16(temp, basis);
        }

        private Command FindCommandByMnemonic(string mnemonic)
        {
            Command searchedCommand = null;
            for (int i = 0; i < commands.Length && searchedCommand == null; i++)
            {
                if (commands[i].Mnemonic == mnemonic)
                {
                    searchedCommand = commands[i];
                }
            }
            return searchedCommand;
        }
    }
}
