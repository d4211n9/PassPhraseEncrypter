namespace FileManager.Interfaces;

public interface IFileManager
{
    string WriteToFile(string text, string filePath);
    string ReadFromFile(string filePath);
}