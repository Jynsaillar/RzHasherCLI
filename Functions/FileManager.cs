/* 
    This class will handle file IO, i.e. parsing the command line arguments,
    determining if an argument is a file or a folder by parsing flags like:
    -f for (next argument is a ) file,
    -d for (next argument is a ) directory,
    -h for action: hash all specified files and files in specified directories,
    -u for action: unhash all specified files and files in specified directories
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace RzHasherCLI
{
    internal static class FileManager
    {
        public static void DisplayCmdHelp()
        {
            Console.WriteLine($@"
            This program expects arguments containing paths to files or folders that are to be hashed or unhashed.\n
            Folders will not be parsed recursively, i.e. if you specify a target folder only files directly in that folder will be processed.\n
            Valid flags are:\n
            -f: next argument is a file to be (un)hashed\n
            -d: next argument is a folder whose files are to be (un)hashed\n
            -h: Hash all specified files\n
            -u: Unhash all specified files\n
            While you can specify -h and -u multiple times respectively, only the last one will be used (e.g. -h -u -h would hash all files,\n
            -u -h -h -u would unhash all files).
            ");
            // todo: Add proper help description.
        }
        public static Tuple<HashSet<string>, HashSet<string>, bool> ParseCmdArguments(string[] args)
        {
            DisplayCmdHelp();

            if (args.Length < 1)
            {
                Console.WriteLine("Not enough arguments specified, expected [1]: path to file or folder containing files to be hashed.");
            }

            HashSet<string> files = new HashSet<string>();
            HashSet<string> directories = new HashSet<string>();
            HashSet<string> validPathFlags = new HashSet<string>() { "-f", "-d" };
            string lastArgument = string.Empty;
            bool hash = true;
            foreach (var argument in args)
            {
                // Skips all arguments that are flags to process them in the next iteration.
                if (validPathFlags.Contains(argument))
                {
                    lastArgument = argument;
                    continue;
                }

                switch (argument)
                {
                    case "-h":
                        hash = true;
                        break;
                    case "-u":
                        hash = false;
                        break;
                }

                if (lastArgument != string.Empty)
                {
                    switch (lastArgument)
                    {
                        case "-f":
                            files.Add(argument);
                            break;
                        case "-d":
                            directories.Add(argument);
                            break;
                    }
                }

                lastArgument = argument;
            }

            return Tuple.Create(files, directories, hash);
        }

        public static void RunHasherOnValidFiles(Tuple<HashSet<string>, HashSet<string>, bool> optionContainer)
        {
            (HashSet<string> files, HashSet<string> directories, bool hash) = optionContainer;

            foreach (var directory in directories)
            {
                if (!Directory.Exists(directory))
                {
                    Console.WriteLine($"{directory} is not a valid directory, skipping.");
                    continue;
                }
                var unsortedFiles = new HashSet<string>(Directory.GetFiles(directory));

                if (unsortedFiles.Count < 1)
                {
                    Console.WriteLine($"{directory} is empty, skipping.");
                    continue;
                }

                RunHasherOnValidFiles(unsortedFiles, hash);
            }

            RunHasherOnValidFiles(files, hash);

            Console.WriteLine(hash ? "Finished hashing files." : "Finished unhashing files.");
        }

        private static void RunHasherOnValidFiles(HashSet<string> files, bool hash)
        {
            string newFileName = string.Empty;
            string newFilePath = string.Empty;
            FileInfo oldFileInfo;

            foreach (var file in files)
            {
                if (!File.Exists(file))
                {
                    Console.WriteLine($"{file} is not a valid file, skipping.");
                    continue;
                }

                oldFileInfo = new FileInfo(file);
                newFileName = hash ? RappelzHasher.Encrypt(oldFileInfo.Name) : RappelzHasher.Decrypt(oldFileInfo.Name);

                if (string.IsNullOrEmpty(newFileName))
                {
                    continue;
                }

                newFilePath = Path.Join(oldFileInfo.Directory.FullName, newFileName);
                File.Move(file, newFilePath);
            }
        }
    }
}