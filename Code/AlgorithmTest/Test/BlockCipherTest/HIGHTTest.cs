using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Test.Encryption.BlockCipher.Algorithm;
using Test.Encryption.BlockCipher.OperationMode;

namespace Test.BlockCipherTest
{
    public class HIGHTTest
    {
        public static void Run()
        {
            Console.WriteLine("\n******* HIGHT TEST START *******");
            ECB();
            CBC();
            CFB();
            OFB();
            CTR();
            GCM();
            Console.WriteLine("\n******* HIGHT TEST END *********");
        }

        internal class TestVector
        {
            public byte[] Key { get; set; }
            public byte[] IV { get; set; }
            public byte[] PlainText { get; set; }
            public byte[] CipherText { get; set; }
        }

        static void ECB()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/HIGHT/ECB/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<TestVector> vectors = ReadTestVector(filePath);

                foreach (TestVector vector in vectors)
                {
                    HIGHT hight = new(vector.Key);
                    ECB ecb = new(hight);
                    byte[] cipherText = ecb.Encrypt(vector.PlainText);
                    if (!SequenceEqual(cipherText, vector.CipherText, 0, vector.CipherText.Length)
                        || !SequenceEqual(ecb.Decrypt(cipherText), vector.PlainText, 0, vector.PlainText.Length))
                    {
                        Console.Write("Key: ");
                        printHex(vector.Key);
                        Console.Write("\n");

                        Console.Write("PlainText: ");
                        printHex(vector.PlainText);
                        Console.Write("\n");

                        Console.Write("CipherText: ");
                        printHex(cipherText);
                        Console.Write("\n");

                        Console.Write("Vector.CipherText: ");
                        printHex(vector.CipherText);
                        Console.Write("\n");

                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    Console.WriteLine($"[HIGHT - ECB] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[HIGHT - ECB] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void CBC()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/HIGHT/CBC/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<TestVector> vectors = ReadTestVectorWithIV(filePath);

                foreach (TestVector vector in vectors)
                {
                    HIGHT hight = new(vector.Key);
                    CBC cbc = new(hight);
                    byte[] cipherText = cbc.Encrypt(vector.PlainText, vector.IV);

                    if (!SequenceEqual(cipherText, vector.CipherText, 8, vector.CipherText.Length + 8)
                        || !SequenceEqual(cbc.Decrypt(cipherText), vector.PlainText, 0, vector.PlainText.Length))
                    {
                        Console.Write("Key: ");
                        printHex(vector.Key);
                        Console.Write("\n");

                        Console.Write("PlainText: ");
                        printHex(vector.PlainText);
                        Console.Write("\n");

                        Console.Write("CipherText: ");
                        printHex(cipherText);
                        Console.Write("\n");

                        Console.Write("Vector.CipherText: ");
                        printHex(vector.CipherText);
                        Console.Write("\n");

                        Console.Write("DecryptText: ");
                        printHex(cbc.Decrypt(cipherText));
                        Console.Write("\n");

                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    Console.WriteLine($"[HIGHT - CBC] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[HIGHT - CBC] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void CFB()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/HIGHT/CFB/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<TestVector> vectors = ReadTestVectorWithIV(filePath);

                foreach (TestVector vector in vectors)
                {
                    HIGHT hight = new(vector.Key);
                    CFB cfb = new(hight);
                    byte[] cipherText = cfb.Encrypt(vector.PlainText, vector.IV);

                    if (!SequenceEqual(cipherText, vector.CipherText, 8, vector.CipherText.Length + 8)
                        || !SequenceEqual(cfb.Decrypt(cipherText), vector.PlainText, 0, vector.PlainText.Length))
                    {
                        Console.Write("Key: ");
                        printHex(vector.Key);
                        Console.Write("\n");

                        Console.Write("PlainText: ");
                        printHex(vector.PlainText);
                        Console.Write("\n");

                        Console.Write("CipherText: ");
                        printHex(cipherText);
                        Console.Write("\n");

                        Console.Write("Vector.CipherText: ");
                        printHex(vector.CipherText);
                        Console.Write("\n");

                        Console.Write("DecryptText: ");
                        printHex(cfb.Decrypt(cipherText));
                        Console.Write("\n");

                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    Console.WriteLine($"[HIGHT - CFB] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[HIGHT - CFB] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void OFB()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/HIGHT/OFB/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<TestVector> vectors = ReadTestVectorWithIV(filePath);

                foreach (TestVector vector in vectors)
                {
                    HIGHT hight = new(vector.Key);
                    OFB ofb = new(hight);
                    byte[] cipherText = ofb.Encrypt(vector.PlainText, vector.IV);

                    if (!SequenceEqual(cipherText, vector.CipherText, 8, vector.CipherText.Length + 8)
                        || !SequenceEqual(ofb.Decrypt(cipherText), vector.PlainText, 0, vector.PlainText.Length))
                    {
                        Console.Write("Key: ");
                        printHex(vector.Key);
                        Console.Write("\n");

                        Console.Write("PlainText: ");
                        printHex(vector.PlainText);
                        Console.Write("\n");

                        Console.Write("CipherText: ");
                        printHex(cipherText);
                        Console.Write("\n");

                        Console.Write("Vector.CipherText: ");
                        printHex(vector.CipherText);
                        Console.Write("\n");

                        Console.Write("DecryptText: ");
                        printHex(ofb.Decrypt(cipherText));
                        Console.Write("\n");

                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    Console.WriteLine($"[HIGHT - OFB] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[HIGHT - OFB] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void CTR()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/HIGHT/CTR/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<TestVector> vectors = ReadTestVectorWithIV(filePath);

                foreach (TestVector vector in vectors)
                {
                    HIGHT hight = new(vector.Key);
                    CTR ctr = new(hight);
                    byte[] cipherText = ctr.Encrypt(vector.PlainText, vector.IV);

                    if (!SequenceEqual(cipherText, vector.CipherText, 8, vector.CipherText.Length + 8)
                        || !SequenceEqual(ctr.Decrypt(cipherText), vector.PlainText, 0, vector.PlainText.Length))
                    {
                        Console.Write("Key: ");
                        printHex(vector.Key);
                        Console.Write("\n");

                        Console.Write("PlainText: ");
                        printHex(vector.PlainText);
                        Console.Write("\n");

                        Console.Write("CipherText: ");
                        printHex(cipherText);
                        Console.Write("\n");

                        Console.Write("Vector.CipherText: ");
                        printHex(vector.CipherText);
                        Console.Write("\n");

                        Console.Write("DecryptText: ");
                        printHex(ctr.Decrypt(cipherText));
                        Console.Write("\n");

                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    Console.WriteLine($"[HIGHT - CTR] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[HIGHT - CTR] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void GCM()
        {
            Console.WriteLine();
            Console.WriteLine("[HIGHT - GCM] [Fail] There is no test vectors.");
        }

        static List<TestVector> ReadTestVector(string filePath)
        {
            Regex keyPattern = new Regex(@"KEY\s*=\s*([0-9a-fA-F]+)");
            Regex plaintextPattern = new Regex(@"PT\s*=\s*([0-9a-fA-F]+)");
            Regex ciphertextPattern = new Regex(@"CT\s*=\s*([0-9a-fA-F]+)");

            List<TestVector> vectors = new List<TestVector>();
            byte[] key = null, plainText = null, cipherText = null;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (keyPattern.IsMatch(line))
                {
                    key = HexStringToByteArray(keyPattern.Match(line).Groups[1].Value);
                }
                else if (plaintextPattern.IsMatch(line))
                {
                    plainText = HexStringToByteArray(plaintextPattern.Match(line).Groups[1].Value);
                }
                else if (ciphertextPattern.IsMatch(line))
                {
                    cipherText = HexStringToByteArray(ciphertextPattern.Match(line).Groups[1].Value);
                }

                if (key != null && plainText != null && cipherText != null)
                {
                    vectors.Add(new TestVector
                    {
                        Key = key,
                        PlainText = plainText,
                        CipherText = cipherText
                    });
                    key = plainText = cipherText = null;
                }
            }

            return vectors;
        }

        static List<TestVector> ReadTestVectorWithIV(string filePath)
        {
            Regex keyPattern = new Regex(@"KEY\s*=\s*([0-9a-fA-F]+)");
            Regex ivPattern = new Regex(@"IV\s*=\s*([0-9a-fA-F]+)");
            Regex plaintextPattern = new Regex(@"PT\s*=\s*([0-9a-fA-F]+)");
            Regex ciphertextPattern = new Regex(@"CT\s*=\s*([0-9a-fA-F]+)");

            List<TestVector> vectors = new List<TestVector>();
            byte[] key = null, iv = null, plainText = null, cipherText = null;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (keyPattern.IsMatch(line))
                {
                    key = HexStringToByteArray(keyPattern.Match(line).Groups[1].Value);
                }
                else if (ivPattern.IsMatch(line))
                {
                    iv = HexStringToByteArray(ivPattern.Match(line).Groups[1].Value);
                }
                else if (plaintextPattern.IsMatch(line))
                {
                    plainText = HexStringToByteArray(plaintextPattern.Match(line).Groups[1].Value);
                }
                else if (ciphertextPattern.IsMatch(line))
                {
                    cipherText = HexStringToByteArray(ciphertextPattern.Match(line).Groups[1].Value);
                }

                if (key != null && iv != null && plainText != null && cipherText != null)
                {
                    vectors.Add(new TestVector
                    {
                        Key = key,
                        IV = iv,
                        PlainText = plainText,
                        CipherText = cipherText
                    });
                    key = iv = plainText = cipherText = null;
                }
            }

            return vectors;
        }

        static void printHex(byte[] data)
        {
            Console.Write(BitConverter.ToString(data).Replace("-", " "));
        }

        static byte[] HexStringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        static bool SequenceEqual(byte[] cipherText, byte[] vectorCipherText, int s, int e)
        {
            for (int i = s; i < e; i++)
            {
                if (cipherText[i] != vectorCipherText[i - s])
                    return false;
            }
            return true;
        }
    }
}
