namespace Encryption.Models;

public class EncryptedMessage
{
    public byte[] Nonce { get; set; }
    public byte[] CipherText { get; set; }
    public byte[] Tag { get; set; }
}