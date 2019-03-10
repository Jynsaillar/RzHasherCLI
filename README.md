# RzHasherCLI
Command line hasher which hashes file names according to a hash algorithm.

## TL;DR:
* Install [.NET Core SDK 2.2](https://dotnet.microsoft.com/download)
* Clone repository: `git clone https://github.com/Jynsaillar/RzHasherCLI.git`
* From the command line, change to the project directory, e.g. `cd C:\projects\RzHasherCLI`
* Run the application with valid parameters using .NET Core: `dotnet run -- -h -d "C:\unhashedfiles"`

### Valid flags
Argument          |Default           |Purpose
------------------|:----------------:|:------------------:|
`-h`              |`true`            |Changes the action to `hash all source files`
`-u`              |`false`           |Changes the action to `unhash all source files`
`-f`              |` `               |Adds a target file if the next argument is a valid **file**
`-d`              |` `               |Adds a target directory if the next argument is a valid **directory**

*Note:  If -f or -d are not specified, the default is **not** the current working directory!*

## Examples:
* Hashing a single file:  
`dotnet run -- -h -f "C:\my hashing folder\model.obj"`
* Unhashing a single file:  
`dotnet run -- -u -f "C:\my unhashing folder\x%$JFaw)"`
* Hashing all files in a folder (non-recursive!):  
`dotnet run -- -h -d "C:\unhashed\"`
* Unhashing all files in a folder (non-recursive!):  
`dotnet run -- -u -d "C:\hashed\"`
