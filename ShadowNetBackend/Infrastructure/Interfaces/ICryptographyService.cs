using ShadowNetBackend.Common;

namespace ShadowNetBackend.Infrastructure.Interfaces;

public interface ICryptographyService
{
    string Encrypt(string plainText, EncryptionType encryptionType, string? key = null);
    string Decrypt(string cipherText, EncryptionType encryptionType, string? key = null);
}

