using System;

namespace RzHasherCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            FileManager.RunHasherOnValidFiles(FileManager.ParseCmdArguments(args));
        }
    }
}
