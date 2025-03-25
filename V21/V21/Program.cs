using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

public class XmlData
{
    public string fileName { get; set; }
    public string fileType{ get; set; }
    public string folderName{ get; set; }
    public string folderPath { get; set; }
}

class program
{
    static void Main()
    {
        string SaveFilename = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_Karnitin"; // Creating a filename with the current date and time

        XmlData xmlData = new XmlData // Giving Variables to the class
        {
            fileName = Console.ReadLine(),
            fileType = Console.ReadLine(),
            folderName = Console.ReadLine(),
            folderPath = Console.ReadLine()
        };

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlData)); // Creating an object of the XmlSerializer class

            using (FileStream stream = new FileStream(SaveFilename+".xml", FileMode.Create)) // Creating a file
            {
                serializer.Serialize(stream, xmlData); // Writing the data to the file
            }
            Console.WriteLine("XML file saved successfully using XmlSerializer.");
        }
        catch (Exception ex) // Catching the exception
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }
}