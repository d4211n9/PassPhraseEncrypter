using Encryption.Models;

namespace Encryption;

public interface IEncrypter
{
    EncryptedMessageWithKey Encrypt(String message);
    string Decrypt(EncryptedMessageWithKey encryptedMessageWithKey);
}