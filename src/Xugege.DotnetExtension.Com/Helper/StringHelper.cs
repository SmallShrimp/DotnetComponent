using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Xugege.DotnetExtension.Com.Helper
{
    public class StringHelper
    {

        private const string EncryptKey = "qweasdzx";
        private const string EncryptIV = "rtyuiogj";

        public static string CreateIdByTime()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

        }

        public static string CreateIdByGuidAndTime(int count, char split = '-')
        {
            string result = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (i == count - 1) result += CreateIdByGuidAndTime();
                else result += CreateIdByGuidAndTime() + split;
            }
            return result;
        }

        public static string CreateIdByGuidAndTime()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        public static long CreateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        #region 加密解密

        public static string Encrypt(string sourceString, string key = EncryptKey, string iv = EncryptIV)
        {
            try
            {
                byte[] btKey = Encoding.UTF8.GetBytes(key);

                byte[] btIV = Encoding.UTF8.GetBytes(iv);

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] inData = Encoding.UTF8.GetBytes(sourceString);
                    try
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV),
                            CryptoStreamMode.Write))
                        {
                            cs.Write(inData, 0, inData.Length);

                            cs.FlushFinalBlock();
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                    catch
                    {
                        return sourceString;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string Decrypt(string encryptedString, string key = EncryptKey, string iv = EncryptIV)
        {
            byte[] btKey = Encoding.UTF8.GetBytes(key);

            byte[] btIV = Encoding.UTF8.GetBytes(iv);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Convert.FromBase64String(encryptedString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);

                        cs.FlushFinalBlock();
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        #endregion
    }
}
