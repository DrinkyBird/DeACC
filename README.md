# DeACC

**DeACC** is a bytecode disassembler for Hexen and ZDoom [ACS](https://doomwiki.org/wiki/ACS).
Currently it mostly works, but may produce incorrect results in some cases due to incorrect data on opcodes and functions.
File an [issue](https://dev.firestick.games/sean/DeACC/-/issues) if something's awry.

## Usage

Automatic builds are available for Windows, Linux, and macOS - go to the [CI jobs page](https://dev.firestick.games/sean/DeACC/-/jobs) and download the artefacts from the publish job for your platform.
Alternatively, you can build DeACC yourself; the .NET 8.0 SDK is required.

To disassemble an ACS object file, open a terminal/command prompt, change to the directory you downloaded the executable to, and run the command below.
Replace `path_to_input.o` with your the object file you want to disassemble, and `path_to_output.bcs` with the file you want the disassembly to be written to.

```
DeACC disassemble -o path_to_output.bcs path_to_input.o
```

If building DeACC yourself from source, you can also do:
```
dotnet run --project DeACC\DeACC.csproj disassemble -o path_to_output.bcs path_to_input.o
```