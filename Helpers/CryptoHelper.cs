using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;

namespace StudentManagementSystem.Helpers
{
    public static class CryptoHelper
    {
        // ==== PBKDF2 PASSWORD HASHING ====
        public static string GenerateSalt(int size = 16)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[size];
                rng.GetBytes(buffer);
                return Convert.ToBase64String(buffer);
            }
        }

        public static string HashPassword(string password, string salt, int iterations = 10000)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), iterations, HashAlgorithmName.SHA256);
            return Convert.ToBase64String(pbkdf2.GetBytes(32)); // 256-bit hash
        }

        public static bool VerifyPassword(string password, string salt, string expectedHash)
        {
            string hash = HashPassword(password, salt);
            return hash == expectedHash;
        }

        // ==== SYMMETRIC ENCRYPTION (AES) ====
        // Key should be 32 chars (256 bits); IV is auto-generated here (all zeros for demo, random for production!)
        public static string AESEncrypt(string plaintext, string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16]; // For demo only! Use a random IV for production.
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] input = Encoding.UTF8.GetBytes(plaintext);
                    byte[] encrypted = encryptor.TransformFinalBlock(input, 0, input.Length);
                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        public static string AESDecrypt(string ciphertext, string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16]; // Same as above
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] encryptedBytes = Convert.FromBase64String(ciphertext);
                    byte[] decrypted = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decrypted);
                }
            }
        }

        // ==== ASYMMETRIC ENCRYPTION (RSA) ====
        // For hybrid: Encrypt AES key with RSA public key, decrypt with private key

        public static string RSAEncrypt(string plaintext, string publicKeyXml)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(publicKeyXml);
                byte[] encrypted = rsa.Encrypt(Encoding.UTF8.GetBytes(plaintext), false);
                return Convert.ToBase64String(encrypted);
            }
        }

        public static string RSADecrypt(string ciphertext, string privateKeyXml)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(privateKeyXml);
                byte[] bytes = Convert.FromBase64String(ciphertext);
                byte[] decrypted = rsa.Decrypt(bytes, false);
                return Encoding.UTF8.GetString(decrypted);
            }
        }

        // ==== HYBRID ENCRYPTION ====
        // Encrypt data with AES, then encrypt AES key with RSA
        public static (string EncryptedData, string EncryptedAESKey) HybridEncrypt(string plaintext, string aesKey, string rsaPublicKeyXml)
        {
            string encryptedData = AESEncrypt(plaintext, aesKey);
            string encryptedKey = RSAEncrypt(aesKey, rsaPublicKeyXml);
            return (encryptedData, encryptedKey);
        }

        // Decrypt AES key with RSA, then decrypt data with AES
        public static string HybridDecrypt(string encryptedData, string encryptedAESKey, string rsaPrivateKeyXml)
        {
            string aesKey = RSADecrypt(encryptedAESKey, rsaPrivateKeyXml);
            return AESDecrypt(encryptedData, aesKey);
        }

        // ==== RSA KEY GENERATION UTILITY ====
        public static (string PublicKey, string PrivateKey) GenerateRSAKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                return (rsa.ToXmlString(false), rsa.ToXmlString(true));
            }
        }
    }
}
