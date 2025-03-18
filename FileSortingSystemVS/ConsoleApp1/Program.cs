using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    // Class level variables to be accessed by all methods
    static string specificFolderName;
    static string specificFile = "*"; // Default to all files unless specified
    static string fileToFind;
    static string directoryPath;
    static string moveFileSpecific;
    static string renameFolder;

    static void Main()
    {
        // Get input from user
        Console.WriteLine("Enter file name to search for:");
        fileToFind = Console.ReadLine();

        Console.WriteLine("Enter directory path to search in:");
        directoryPath = Console.ReadLine();

        Console.WriteLine("Do you want a specific file moved? Y/N");
        moveFileSpecific = Console.ReadLine();

        Console.WriteLine("Do you Wish to have a costom Folder name?");
        renameFolder = Console.ReadLine();

        if (FileSpecificationCheck(moveFileSpecific))
        {
            Console.WriteLine("What file type do you want moved? (e.g., .txt, .exe)");
            specificFile = Console.ReadLine();
        }

        if (FolderNameSpecificationCheck(renameFolder))
        {
            Console.WriteLine("What do you want the folder to be named?");
            specificFolderName = Console.ReadLine();
        }
        else
        {
            specificFolderName = fileToFind; // Set default folder name if not specified
        }

        FileFetcher(fileToFind, directoryPath, specificFile);
    }

    static void FileFetcher(string fileToFind, string directoryPath, string specificFile)  // Gather files
    {
        List<string> sortedFiles = new List<string>();

        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine($"Directory not found: {directoryPath}");
            return;
        }

        string[] fileNames = Directory.GetFiles(directoryPath, "*" + specificFile, SearchOption.AllDirectories);

        FindFileNameMatches(fileNames, fileToFind, sortedFiles, directoryPath);

        // Print all matching files
        if (sortedFiles.Count > 0)
        {
            Console.WriteLine("\nMatching files found:");
            foreach (string file in sortedFiles)
                Console.WriteLine(file);
        }
    }

    static void FindFileNameMatches(string[] fileNames, string fileToFind, List<string> sortedFiles, string directoryPath)
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

        if (sortedFiles.Count == 0)
        {
            Console.WriteLine($"No files found containing: {fileToFind}");
        }
        else
        {
            FileManager(sortedFiles, directoryPath, fileToFind);
        }
    }

    static void FileManager(List<string> files, string directoryPath, string folderName) // Moves files to a new directory (The files, The places, The name)
    {
        // Create a subdirectory for the matching files
        string matchingFilesDir = Path.Combine(directoryPath, specificFolderName);

        if (!Directory.Exists(matchingFilesDir))
            Directory.CreateDirectory(matchingFilesDir);

        foreach (string file in files) // Move files to the new directory
        {
            FileInfo fileInfo = new FileInfo(file);
            string destinationPath = Path.Combine(matchingFilesDir, fileInfo.Name);

            if (!File.Exists(destinationPath))
            {
                fileInfo.MoveTo(destinationPath);
                Console.WriteLine($"Moved file: {fileInfo.Name}");
            }
            else
            {
                Console.WriteLine($"File already exists in destination: {fileInfo.Name}");
            }
        }
    }

    static bool FileSpecificationCheck(string input) // checks if user wants to move a specific file type
    {
        return input.Trim().Equals("y", StringComparison.OrdinalIgnoreCase);
    }

    static bool FolderNameSpecificationCheck(string input) // checks if user wants to rename the folder
    {
        return input.Trim().Equals("y", StringComparison.OrdinalIgnoreCase);
    }
}