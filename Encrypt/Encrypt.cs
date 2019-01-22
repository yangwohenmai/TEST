using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encrypt
{
    class Encrypt
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strInput">输入字符串</param>
        /// <param name="KEY_64">用于对称算法的密钥。密钥必须为8位</param>
        /// <param name="IV_64">用于对称算法的初始化向量。密钥必须为8位</param>
        /// <returns></returns>
        public static string DesEncrypt(string strInput, string KEY_64, string IV_64)
        {
            DESCryptoServiceProvider provider = null;
            MemoryStream stream = null;
            CryptoStream stream2 = null;
            StreamWriter writer = null;
            string str2;
            if (string.IsNullOrEmpty(strInput))
            {
                return "";
            }
            if (string.IsNullOrEmpty(KEY_64) || string.IsNullOrEmpty(IV_64))
            {
                return strInput;
            }
            if ((KEY_64.Length != 8) || (IV_64.Length != 8))
            {
                return strInput;
            }
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(KEY_64);
                byte[] rgbIV = Encoding.ASCII.GetBytes(IV_64);
                provider = new DESCryptoServiceProvider();
                int keySize = provider.KeySize;
                stream = new MemoryStream();
                stream2 = new CryptoStream(stream, provider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
                writer = new StreamWriter(stream2);
                writer.Write(strInput);
                writer.Flush();
                stream2.FlushFinalBlock();
                writer.Flush();
                str2 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
                if (stream2 != null)
                {
                    stream2.Dispose();
                }
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                if (provider != null)
                {
                    provider.Dispose();
                }
            }
            return str2;
        }


        public static string ToDBC(string strInput)
        {
            string str = "";
            if (string.IsNullOrEmpty(strInput))
            {
                return str;
            }
            char[] chArray = strInput.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if (chArray[i] == '　')
                {
                    chArray[i] = ' ';
                }
                else if ((chArray[i] > 0xff00) && (chArray[i] < 0xff5f))
                {
                    chArray[i] = (char)(chArray[i] - 0xfee0);
                }
            }
            return new string(chArray);
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="strInput">输入字符串</param>
        /// <param name="KEY_64">用于对称算法的密钥。密钥必须为8位</param>
        /// <param name="IV_64">用于对称算法的初始化向量。密钥必须为8位</param>
        /// <returns></returns>
        public static string DesDecrypt(string strInput, string KEY_64, string IV_64)
        {
            DESCryptoServiceProvider provider = null;
            MemoryStream stream = null;
            CryptoStream stream2 = null;
            StreamReader reader = null;
            string str2;
            if (string.IsNullOrEmpty(strInput))
            {
                return "";
            }
            if (string.IsNullOrEmpty(KEY_64) || string.IsNullOrEmpty(IV_64))
            {
                return strInput;
            }
            if ((KEY_64.Length != 8) || (IV_64.Length != 8))
            {
                return strInput;
            }
            try
            {
                byte[] buffer3;
                byte[] bytes = Encoding.ASCII.GetBytes(KEY_64);
                byte[] rgbIV = Encoding.ASCII.GetBytes(IV_64);
                try
                {
                    buffer3 = Convert.FromBase64String(strInput);
                }
                catch
                {
                    return "";
                }
                provider = new DESCryptoServiceProvider();
                stream = new MemoryStream(buffer3);
                stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Read);
                reader = new StreamReader(stream2);
                str2 = reader.ReadToEnd();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (stream2 != null)
                {
                    stream2.Dispose();
                }
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                if (provider != null)
                {
                    provider.Dispose();
                }
            }
            return str2;
        }
    }
}
