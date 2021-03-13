# A simple assembler for the Intel 8008 CPU #

The running of this program requires at least one argument - an input file nime with the program's text itself; the second argument is an output file name and is optinal. If the second argument is not presented, the name of the output file shall be "a.bin".

The assembler supports labels and constants. A label ends with colon, i.e. "label_name:". There should be only a label itself in a line. Otherwise all contents of this line will be ignored, excluding the label. 

If you want to declare a constant, you must write a keyword "define", then the constant's name and finally its value, which should fit in 8-bit.

Numbers in this assembler can have a prefix, which determines the number's base. The list of the prefixes is: $ for hex values, % for binary and # for decimal. If the number does not have a prefix, the number shall be determined as decimal.

In addition, you are able to "index" label to get access to its low or high part of address. Here is how to use it: labe_name[index], where index can be 'h'  or 'l' for high and low parts respectively. A space symbol between label's name and brackets is strictly prohibited!

Note: the assembler accepts only the old mnemonics of the instructions!
