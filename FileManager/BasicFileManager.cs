using System.Security.Cryptography;
using System.Text;
using FileManager.Interfaces;

namespace FileManager;

public class BasicFileManager : IFileManager
{
    private readonly string _savedFilesPath = "SavedFiles";
    
    public string WriteToFile(string text, string filePath)
    {
        try
        {
            filePath = filePath.Replace("\\", "\\\\");
            
            using StreamWriter outputFile = File.AppendText(filePath);
        
            outputFile.WriteLine(text);

            return filePath;
        }
        catch (Exception e)
        {
            throw new Exception("Failed to write to file");
        }
    }

    public string ReadFromFile(string filePath)
    {
        try
        {
            using StreamReader streamReader = new StreamReader(filePath);

            return streamReader.ReadToEnd();
        }
        catch (Exception e)
        {
            throw new Exception("Failed to read from file");
        }
    }
}