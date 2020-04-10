# EU4 to Json
Simple command line application that parses some data from an Europa Universalis 4 save game (.eu4) and outputs it to a .json. Available for Windows & Linux.

## Getting started
### Prerequisites
You need [7-Zip](https://www.7-zip.org/) to unpack the program and the [.NET Core 3.1 Runtime](https://dotnet.microsoft.com/download) to run it.

### Installation & Run
1. Unpack the .7zip to the desired location.
2. Execute the EU4 to Json with the path to a .eu4 save game. 

   On Linux: `./EU4ToJson savegame.eu4`
   
   on Windows: `EU4ToJson.exe savegame.eu4`
   
3. EU4 to Json will create the .json at the same location of the save game and with the same name (expect the file ending). 

**Caution: If the .json already exists it will be overwritten!** 

## Build with 
* [Pdoxcl2Sharp](https://github.com/nickbabcock/Pdoxcl2Sharp)
* [Json.NET](https://www.newtonsoft.com/json)


## Build
```
dotnet publish -c Release --no-self-contained -r linux-x64
dotnet publish -c Release --no-self-contained -r win10-x64
```

