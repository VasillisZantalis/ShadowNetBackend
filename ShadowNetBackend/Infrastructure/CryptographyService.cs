using ShadowNetBackend.Common;
using ShadowNetBackend.Infrastructure.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ShadowNetBackend.Infrastructure;

public class CryptographyService : ICryptographyService
{
    private readonly IConfiguration _configuration;

    public CryptographyService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Encrypt(string plainText, EncryptionType encryptionType, string? key = null)
    {
        return encryptionType switch
        {
            EncryptionType.AES => EncryptAES(plainText, key ?? _configuration["EncryptionKey:Key"]!),
            EncryptionType.RSA => EncryptRSA(plainText),
            _ => throw new ArgumentException("Invalid encryption type")
        };
    }

    public string Decrypt(string cipherText, EncryptionType encryptionType, string? key = null)
    {
        return encryptionType switch
        {
            EncryptionType.AES => DecryptAES(cipherText, key ?? _configuration["EncryptionKey:Key"]!),
            EncryptionType.RSA => DecryptRSA(cipherText),
            _ => throw new ArgumentException("Invalid decryption type")
        };
    }

    private string EncryptAES(string plainText, string key)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
        aes.IV = Encoding.UTF8.GetBytes(key.PadRight(16).Substring(0, 16));
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

        return Convert.ToBase64String(encryptedBytes);
    }

    private string DecryptAES(string cipherText, string key)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
        aes.IV = Encoding.UTF8.GetBytes(key.PadRight(16).Substring(0, 16));
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        byte[] encryptedBytes = Convert.FromBase64String(cipherText);
        byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

        return Encoding.UTF8.GetString(decryptedBytes);
    }


    private string EncryptRSA(string plainText)
    {
        using var rsa = new RSACryptoServiceProvider(2048);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encryptedBytes = rsa.Encrypt(plainBytes, false);
        return Convert.ToBase64String(encryptedBytes);
    }

    private string DecryptRSA(string cipherText)
    {
        using var rsa = new RSACryptoServiceProvider(2048);
        byte[] encryptedBytes = Convert.FromBase64String(cipherText);
        byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, false);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
