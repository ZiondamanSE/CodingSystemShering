using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        // Get input from user
        Console.WriteLine("Enter file name to search for:");
        string fileToFind = Console.ReadLine();

        Console.WriteLine("Enter directory path to search in:");
        string directoryPath = Console.ReadLine();

        Console.WriteLine("Do you want a specific file moved? Y/N");
        string moveFileSpesific = Console.ReadLine();

        if(FileSpesificanonCheck(moveFileSpesific))
        {
            Debug.WriteLine("What File do you want moved? (Write the executable ext: .txt .exe)");
        }
        else
        {
            List<string> sortedFiles = new List<string>();

            if (Directory.Exists(directoryPath)) // Locates all files within locashon (aka ingnors folders)
            {
                string[] fileNames = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
                FindFileNameMatches(fileNames, fileToFind, sortedFiles, directoryPath);

                // Print all matching files
                if (sortedFiles.Count > 0)
                {
                    Console.WriteLine("\nMatching files found:");
                    foreach (string file in sortedFiles)
                        Console.WriteLine(file);
                }
            }
            else
                Console.WriteLine($"Directory not found: {directoryPath}");
        }
    }

    static void FindFileNameMatches(string[] fileNames, string fileToFind, List<string> sortedFiles,string directoryPath)
    {
        foreach (string filePath in fileNames)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            Console.WriteLine($"Checking file: {fileName}");

            if (fileName.IndexOf(fileToFind, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                sortedFiles.Add(filePath);
                Console.WriteLine($"Match found: {fileName}");
            }
        }


        if (sortedFiles.Count == 0) // If Files were not found
            Console.WriteLine($"No files found containing: {fileToFind}");
        else
            FileMainger(sortedFiles, directoryPath, fileToFind);
    }

    static void FileMainger(List<string> Files, string directoryPath, string folderName)
    {
        // Create a subdirectory for the matching files
        string matchingFilesDir = Path.Combine(directoryPath, folderName);

        if (!Directory.Exists(matchingFilesDir))
            Directory.CreateDirectory(matchingFilesDir);

        foreach (string file in Files) // moves files to the new folder
        {
            FileInfo fileInfo = new FileInfo(file);
            fileInfo.MoveTo(Path.Combine(matchingFilesDir, fileInfo.Name));
            Debug.WriteLine($"Moved file: {fileInfo.Name}");
        }   

    }

    static bool FileSpesificanonCheck(string input) // Checks if the user wants to move a spesific file
    {
        if (input.ToLower() == "y") return true;  else  return false;
    }

}
