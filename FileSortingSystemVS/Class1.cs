using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

// Serializable class to hold search configuration and results
[Serializable]
public class SearchConfiguration
{
    public string FileToFind { get; set; }
    public string DirectoryPath { get; set; }
    public string SpecificFile { get; set; }
    public List<string> MatchedFiles { get; set; }
    public DateTime SearchDate { get; set; }

    // Default constructor
    public SearchConfiguration()
    {
        MatchedFiles = new List<string>();
        SearchDate = DateTime.Now;
    }
}

class Program
{
    // Existing class-level variables...
    static SearchConfiguration currentSearchConfig = new SearchConfiguration();
    static string XML_FILE_PATH = "search_results.xml";

    // XML Serialization Method
    static void SaveSearchResultsToXml(SearchConfiguration config)
    {
        try
        {
            // Create an XML serializer
            XmlSerializer serializer = new XmlSerializer(typeof(SearchConfiguration));

            // Write to file
            using (FileStream fs = new FileStream(XML_FILE_PATH, FileMode.Create))
            {
                serializer.Serialize(fs, config);
            }

            Console.WriteLine($"Search results saved to {XML_FILE_PATH}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving XML: {ex.Message}");
        }
    }

    // XML Deserialization Method
    static SearchConfiguration LoadSearchResultsFromXml()
    {
        try
        {
            // Check if file exists
            if (!File.Exists(XML_FILE_PATH))
            {
                Console.WriteLine($"No saved search results found at {XML_FILE_PATH}");
                return new SearchConfiguration();
            }

            // Create an XML serializer
            XmlSerializer serializer = new XmlSerializer(typeof(SearchConfiguration));

            // Read from file
            using (FileStream fs = new FileStream(XML_FILE_PATH, FileMode.Open))
            {
                SearchConfiguration loadedConfig = (SearchConfiguration)serializer.Deserialize(fs);

                Console.WriteLine($"Loaded search results from {XML_FILE_PATH}");
                return loadedConfig;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading XML: {ex.Message}");
            return new SearchConfiguration();
        }
    }

    // Modify the FindFileNameMatches method to save results
    static void FindFileNameMatches(string[] fileNames, string fileToFind, List<string> sortedFiles, string directoryPath)
    {
        // Clear previous results
        currentSearchConfig = new SearchConfiguration
        {
            FileToFind = fileToFind,
            DirectoryPath = directoryPath
        };

        foreach (string filePath in fileNames)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            Console.WriteLine($"Checking file: {fileName}");

            if (fileName.IndexOf(fileToFind, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                sortedFiles.Add(filePath);
                currentSearchConfig.MatchedFiles.Add(filePath); // Save to configuration
                Console.WriteLine($"Match found: {fileName}");
            }
        }

        if (sortedFiles.Count == 0)
        {
            Console.WriteLine($"No files found containing: {fileToFind}");
        }
        else
        {
            // Save results before moving files
            SaveSearchResultsToXml(currentSearchConfig);

            FileManager(sortedFiles, directoryPath, fileToFind);
        }
    }

    // Optional: Add a method to show previous search results
    static void ShowPreviousSearchResults()
    {
        SearchConfiguration previousSearch = LoadSearchResultsFromXml();

        Console.WriteLine("\nPrevious Search Details:");
        Console.WriteLine($"File Searched: {previousSearch.FileToFind}");
        Console.WriteLine($"Search Directory: {previousSearch.DirectoryPath}");
        Console.WriteLine($"Search Date: {previousSearch.SearchDate}");
        Console.WriteLine("Matched Files:");
        foreach (string file in previousSearch.MatchedFiles)
        {
            Console.WriteLine(file);
        }
    }

    // Modify Main method to include option for viewing previous results
    static void Main()
    {
        Console.WriteLine("1. New Search");
        Console.WriteLine("2. View Previous Search Results");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        if (choice == "2")
        {
            ShowPreviousSearchResults();
            return;
        }

        // Rest of your existing Main method code...
        // Get input from user
        Console.WriteLine("Enter file name to search for:");
        fileToFind = Console.ReadLine();

        Console.WriteLine("Enter directory path to search in:");
        directoryPath = Console.ReadLine();

        // ... (rest of the original Main method remains the same)
    }

    // ... (rest of the original methods remain unchanged)
}