namespace Encryption.Models;

public class EncryptedMessageWithKey : EncryptedMessage
{
    public byte[] key { get; set; }
}