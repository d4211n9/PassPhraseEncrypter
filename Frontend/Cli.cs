using System.Text;
using Encryption;
using Encryption.Models;
using FileManager;
using FileManager.Interfaces;
using ObjectJsonConverter;
using ObjectJsonConverter.Interfaces;

namespace Frontend;

public class Cli
{
    public void Start()
    {
        string[] options = { "0", "1", "2" };
        string userChoice = "";
        bool failed = false;
        
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("Encrypt to file (0)");
        Console.WriteLine("Decrypt from file (1)");
        Console.WriteLine("Exit (2)");
        
        while (!options.Contains(userChoice))
        {
            if (failed) Console.WriteLine("You must input either 0, 1, or 2");
            if (!failed) failed = true;
            
            userChoice = Console.ReadLine();
        }

        switch (userChoice)
        {
            case "0":
                EncryptPassPhraseToFile();
                break;
            case "1":
                ReadAndDecrypt();
                break;
            default:
                Environment.Exit(1);
                break;
        }
    }

    private void ReadAndDecrypt()
    {
        string filePath = GetFilePath();

        string json = ReadMessageFromFile(filePath);

        string key = GetKeyFromUser();

        IObjectJsonConverter<EncryptedMessageWithKey> jsonConverter = new JsonConverter<EncryptedMessageWithKey>();

        EncryptedMessage? encryptedMessage = jsonConverter.JsonToObject(json);

        if (encryptedMessage == null)
        {
            Console.WriteLine("Something went wrong");
            return;
        }

        EncryptedMessageWithKey encryptedMessageWithKey = new EncryptedMessageWithKey();
        encryptedMessageWithKey.key = AesGcmEncrypter.PassPhraseToKey(key);
        encryptedMessageWithKey.CipherText = encryptedMessage.CipherText;
        encryptedMessageWithKey.Tag = encryptedMessage.Tag;
        encryptedMessageWithKey.Nonce = encryptedMessage.Nonce;

        string message = DecryptPassPhrase(encryptedMessageWithKey);
        
        Console.WriteLine(message);
    }
    
    private void EncryptPassPhraseToFile()
    {
        string message = GetMessage();

        EncryptedMessageWithKey encryptedMessageWithKey = EncryptPassPhrase(message);
        EncryptedMessage encryptedMessage = encryptedMessageWithKey;

        IObjectJsonConverter<EncryptedMessage> jsonConverter = new JsonConverter<EncryptedMessage>();
        string json = jsonConverter.ObjectToJson(encryptedMessage);

        string filePath = GetFilePath();
        
        WriteMessageToFile(json, filePath);
        
        Console.WriteLine("Successfully wrote to the file");
        Console.WriteLine("Your key is: " + AesGcmEncrypter.KeyToPassPhrase(encryptedMessageWithKey.key));
    }
    
    private string DecryptPassPhrase(EncryptedMessageWithKey encryptedMessageWithKey)
    {
        try
        {
            IEncrypter encrypter = new AesGcmEncrypter();

            return encrypter.Decrypt(encryptedMessageWithKey);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    private string ReadMessageFromFile(string filePath)
    {
        try
        {
            IFileManager fileManager = new BasicFileManager();

            return fileManager.ReadFromFile(filePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    private void WriteMessageToFile(string message, string filePath)
    {
        try
        {
            IFileManager fileManager = new BasicFileManager();

            fileManager.WriteToFile(message, filePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    private EncryptedMessageWithKey EncryptPassPhrase(string message)
    {
        try
        {
            IEncrypter encrypter = new AesGcmEncrypter();

            return encrypter.Encrypt(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    private string GetMessage()
    {
        Console.WriteLine("Please enter a message");

        string? message = null;
        bool failed = false;
        
        while (message == null || message.Equals(""))
        {
            if (failed) Console.WriteLine("The message must not be empty");
            if (!failed) failed = true;
            message = Console.ReadLine();
        }

        return message;
    }

    private string GetFilePath()
    {
        Console.WriteLine("Please enter a file path");

        string? filePath = null;
        bool failed = false;
        
        while (filePath == null || filePath.Equals(""))
        {
            if (failed) Console.WriteLine("File path must not be empty");
            if (!failed) failed = true;
            filePath = Console.ReadLine();
        }

        return filePath;
    }

    private string GetKeyFromUser()
    {
        Console.WriteLine("Please enter the encryption key");

        string? key = null;
        bool failed = false;
        
        while (key == null || key.Equals(""))
        {
            if (failed) Console.WriteLine("Key must not be empty");
            if (!failed) failed = true;
            key = Console.ReadLine();
        }

        return key;
    }
}