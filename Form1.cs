using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncryptDecryptString
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            txtCyptherText.Text = Encrypt(txtPlainText.Text, txtPassphrase.Text);
        }


        private static string Decrypt(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = System.Text.Encoding.ASCII.GetBytes("tu89geji340t89u2");
            const int keysize = 256;
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            using (System.Security.Cryptography.PasswordDeriveBytes password = new System.Security.Cryptography.PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (System.Security.Cryptography.RijndaelManaged symmetricKey = new System.Security.Cryptography.RijndaelManaged())
                {
                    symmetricKey.Mode = System.Security.Cryptography.CipherMode.CBC;
                    using (System.Security.Cryptography.ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                            {
                                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return System.Text.Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        public static string Encrypt(string plainText, string passPhrase)
        {

            byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");
            const int keysize = 256;

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (System.Security.Cryptography.PasswordDeriveBytes password = new System.Security.Cryptography.PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (System.Security.Cryptography.RijndaelManaged symmetricKey = new System.Security.Cryptography.RijndaelManaged())
                {
                    symmetricKey.Mode = System.Security.Cryptography.CipherMode.CBC;
                    using (System.Security.Cryptography.ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            txtPlainText.Text = Decrypt(txtCyptherText.Text, txtPassphrase.Text);
        }
    }
}
