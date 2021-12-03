using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;

namespace ComBase
{
    public class clsCyper
    {
        public static string EncryptionKey = "mentorCyper";
        public static void Encrypt(string inputFilePath, string outputfilePath)
        {
            using (Aes encryptor = Aes.Create())
            {
                using (Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
                {
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                    {
                        using (CryptoStream cs = new CryptoStream(fsOutput, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                int data;
                                while ((data = fsInput.ReadByte()) != -1)
                                {
                                    cs.WriteByte((byte)data);
                                }
                                fsInput.Close();
                            }
                            cs.Close();
                        }
                        fsOutput.Close();
                    }
                }

            }
        }

        public static void EncryptImage(Image Img, string outputfilePath)
        {
            using (Aes encryptor = Aes.Create())
            {
                using (Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
                {
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                    {
                        using (CryptoStream cs = new CryptoStream(fsOutput, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            using (MemoryStream memory = new MemoryStream())
                            {
                                Img.Save(memory, ImageFormat.Jpeg);
                                memory.Position = 0;

                                byte[] memArray = memory.ToArray();
                                cs.Write(memArray, 0, memArray.Length);
                            }
                            cs.Close();
                        }
                        fsOutput.Close();
                    }
                }

            }
        }

        public static void Decrypt(string inputFilePath, string outputfilePath)
        {
            using (Aes encryptor = Aes.Create())
            {
                using (Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
                {
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                    {
                        using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                            {
                                int data;
                                while ((data = cs.ReadByte()) != -1)
                                {
                                    fsOutput.WriteByte((byte)data);
                                }
                                fsOutput.Close();
                            }
                            cs.Close();
                        }
                        fsInput.Close();
                    }
                }
            }
        }

        public static Image DecryptImage(string inputFilePath)
        {
            if (File.Exists(inputFilePath) == false)
                return null;

            Image rtnImg = null;
            if (inputFilePath.IndexOf(".env") != -1)
            {
                using (Aes encryptor = Aes.Create())
                {
                    using (Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
                    {
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                            {
                                rtnImg = Image.FromStream(cs);
                            }
                            fsInput.Close();
                        }
                    }
                }
            }
            else
            {
                byte[] buff = File.ReadAllBytes(inputFilePath);
                using (MemoryStream ms = new MemoryStream(buff))
                {
                    rtnImg = Image.FromStream(ms);
                }
            }
            
            return rtnImg;
        }
    }
}
