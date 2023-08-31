# DeACC

**DeACC** is a bytecode disassembler for Hexen and ZDoom [ACS](https://doomwiki.org/wiki/ACS).
Currently it mostly works, but may produce incorrect results in some cases due to incorrect data on opcodes and functions.
File an [issue](https://foundry.creationsdeath.net/sean/DeACC/-/issues) if something's awry.

## Usage

This project requires .NET 7.0. Install that and clone the repository.

To disassemble an ACS object file, run the following command from the directory you cloned this repo to.
Replace `path_to_input.o` with your the object file you want to disassemble, and `path_to_output.bcs` with the file you want the disassembly to be written to.

```
dotnet run --project DeACC\DeACC.csproj disassemble -o path_to_output.bcs path_to_input.o
```
