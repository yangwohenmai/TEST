using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            //对文件加密
            dESCSPSampleMemory();
            //对内存中的数据加密解密
            dESCSPSample();
            //使用密钥加密解密
            encrypt();
        }

        /// <summary>
        /// 如何创建和使用DESCryptoServiceProvider对象在内存中的数据进行加密和解密
        /// </summary>
        static public void dESCSPSampleMemory()
        {
            // Create a new DESCryptoServiceProvider object
            // to generate a key and initialization vector (IV).
            DESCryptoServiceProvider DESalg = new DESCryptoServiceProvider();

            // Create a string to encrypt.
            string sData = "Here is some data to encrypt.";

            // Encrypt the string to an in-memory buffer.
            byte[] Data = DESCSPSampleMemory.EncryptTextToMemory(sData, DESalg.Key, DESalg.IV);

            // Decrypt the buffer back to a string.
            string Final = DESCSPSampleMemory.DecryptTextFromMemory(Data, DESalg.Key, DESalg.IV);

            // Display the decrypted string to the console.
            Console.WriteLine(Final);
        }

        /// <summary>
        /// 如何创建和使用DESCryptoServiceProvider对象进行加密和解密文件中的数据。
        /// </summary>
        static public void dESCSPSample()
        {
            // Create a new DESCryptoServiceProvider object
            // to generate a key and initialization vector (IV).
            DESCryptoServiceProvider DESalg = new DESCryptoServiceProvider();

            // Create a string to encrypt.
            string sData = "Here is some data to encrypt.";
            string FileName = "CText.txt";

            // Encrypt text to a file using the file name, key, and IV.
            DESCSPSample.EncryptTextToFile(sData, FileName, DESalg.Key, DESalg.IV);
            // Decrypt the text from a file using the file name, key, and IV.
            string Final = DESCSPSample.DecryptTextFromFile(FileName, DESalg.Key, DESalg.IV);

            // Display the decrypted string to the console.
            Console.WriteLine(Final);
        }

        /// <summary>
        /// 使用密钥加密解密
        /// </summary>
        static public void encrypt()
        {
            
            string strDesEncrypt = Encrypt.DesEncrypt("abc", "finchina", "finchina");
            string strDesDecrypt = Encrypt.DesDecrypt(strDesEncrypt, "finchina", "finchina");
        }


    }
}
