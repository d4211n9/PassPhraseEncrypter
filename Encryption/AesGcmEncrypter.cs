using System.Security.Cryptography;
using System.Text;
using Encryption.Models;

namespace Encryption;

public class AesGcmEncrypter : IEncrypter
{
    private static readonly int KeySize = 32;
    
    public EncryptedMessageWithKey Encrypt(string message)
    {
        byte[] key = new byte[KeySize];
        RandomNumberGenerator.Fill(key);
        byte[] nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(nonce);
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(message);
        byte[] cipherText = new byte[plainTextBytes.Length];
        byte[] tag = new byte[AesGcm.TagByteSizes.MaxSize];

        using AesGcm aesgcm = new AesGcm(key, tag.Length);

        try
        {
            aesgcm.Encrypt(nonce, plainTextBytes, cipherText, tag);

            EncryptedMessageWithKey encryptedMessageWithKey = new EncryptedMessageWithKey();
            encryptedMessageWithKey.key = key;
            encryptedMessageWithKey.Nonce = nonce;
            encryptedMessageWithKey.CipherText = cipherText;
            encryptedMessageWithKey.Tag = tag;

            return encryptedMessageWithKey;
        }
        catch (Exception e)
        {
            throw new Exception("Failed to encrypt message");
        }
    }

    public string Decrypt(EncryptedMessageWithKey encryptedMessageWithKey)
    {
        using AesGcm aesgcm = new AesGcm(encryptedMessageWithKey.key, encryptedMessageWithKey.Tag.Length);

        byte[] plaintextBytes = new byte[encryptedMessageWithKey.CipherText.Length];

        try
        {
            aesgcm.Decrypt(
                encryptedMessageWithKey.Nonce,
                encryptedMessageWithKey.CipherText,
                encryptedMessageWithKey.Tag,
                plaintextBytes);

            return Encoding.UTF8.GetString(plaintextBytes);
        }
        catch (Exception e)
        {
            throw new Exception("Failed to decrypt message");
        }
    }

    public static string KeyToPassPhrase(byte[] key)
    {
        return Convert.ToHexString(key);
    }

    public static byte[] PassPhraseToKey(string passPhrase)
    {
        return Convert.FromHexString(passPhrase);
    }
}