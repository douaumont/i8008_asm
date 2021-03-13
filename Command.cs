using System;
using System.Collections.Generic;
using System.Text;

namespace i8008_asm
{
    class Command
    {
        private string mnemonic;
        private byte lengthInBytes;
        private byte opcode;
        public string Mnemonic
        {
            get
            {
                return this.mnemonic;
            }
        }
        public byte LengthInBytes
        {
            get
            {
                return this.lengthInBytes;
            }
        }
        public byte Opcode
        {
            get
            {
                return this.opcode;
            }
        }
        public Command(string mnemonic_arg, byte lengthInBytes_arg, byte opcode_arg)
        {
            this.mnemonic = mnemonic_arg;
            this.lengthInBytes = lengthInBytes_arg;
            this.opcode = opcode_arg;
        }
    }
}
