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
    public class SPECKTest
    {
        public static void Run()
        {
            Console.WriteLine("\n******* SPECK TEST START *******");
            ECB();
            CBC();
            CFB();
            OFB();
            CTR();
            GCM();
            Console.WriteLine("\n******* SPECK TEST END *********");
        }

        static void ECB()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/SPECK/ECB/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<TestVector> vectors = ReadTestVector(filePath);

                foreach (TestVector vector in vectors)
                {
                    SPECK speck = new(vector.Key);
                    ECB ecb = new(speck);
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
                    Console.WriteLine($"[SPECK - ECB] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[SPECK - ECB] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void CBC()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/SPECK/CBC/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<TestVector> vectors = ReadTestVectorWithIV(filePath);

                foreach (TestVector vector in vectors)
                {
                    SPECK speck = new(vector.Key);
                    CBC cbc = new(speck);
                    byte[] cipherText = cbc.Encrypt(vector.PlainText, vector.IV);

                    if (!SequenceEqual(cipherText, vector.CipherText, 16, vector.CipherText.Length + 16)
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
                    Console.WriteLine($"[SPECK - CBC] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[SPECK - CBC] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void CFB()
        {
            Console.WriteLine();
            Console.WriteLine("[SPECK - CFB] [Fail] There is no test vectors.");
        }

        static void OFB()
        {
            Console.WriteLine();
            Console.WriteLine("[SPECK - OFB] [Fail] There is no test vectors.");
        }

        static void CTR()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/SPECK/CTR/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<TestVector> vectors = ReadTestVectorWithIV(filePath);

                foreach (TestVector vector in vectors)
                {
                    SPECK speck = new(vector.Key);
                    CTR ctr = new(speck);
                    byte[] cipherText = ctr.Encrypt(vector.PlainText, vector.IV);

                    if (!SequenceEqual(cipherText, vector.CipherText, 16, vector.CipherText.Length + 16)
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
                    Console.WriteLine($"[SPECK - CTR] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[SPECK - CTR] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void GCM()
        {
            Console.WriteLine();
            Console.WriteLine("[SPECK - GCM] [Fail] There is no test vectors.");
        }
        internal class TestVector
        {
            public byte[] Key { get; set; }
            public byte[] IV { get; set; }
            public byte[] PlainText { get; set; }
            public byte[] CipherText { get; set; }
        }

        static List<TestVector> ReadTestVector(string filePath)
        {
            Regex keyPattern = new Regex(@"Key:\s*([0-9a-fA-F\s]+)");
            Regex plaintextPattern = new Regex(@"Plaintext:\s*([0-9a-fA-F\s]+)");
            Regex ciphertextPattern = new Regex(@"Ciphertext:\s*([0-9a-fA-F\s]+)");

            List<TestVector> vectors = new List<TestVector>();
            byte[] key = null, plainText = null, cipherText = null;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (keyPattern.IsMatch(line))
                {
                    key = HexStringToByteArray(RemoveSpaces(keyPattern.Match(line).Groups[1].Value));
                }
                else if (plaintextPattern.IsMatch(line))
                {
                    plainText = HexStringToByteArray(RemoveSpaces(plaintextPattern.Match(line).Groups[1].Value));
                }
                else if (ciphertextPattern.IsMatch(line))
                {
                    cipherText = HexStringToByteArray(RemoveSpaces(ciphertextPattern.Match(line).Groups[1].Value));
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
            Regex keyPattern = new Regex(@"Key:\s*([0-9a-fA-F\s]+)");
            Regex ivPattern = new Regex(@"IV:\s*([0-9a-fA-F\s]+)");
            Regex plaintextPattern = new Regex(@"Plaintext:\s*([0-9a-fA-F\s]+)");
            Regex ciphertextPattern = new Regex(@"Ciphertext:\s*([0-9a-fA-F\s]+)");

            List<TestVector> vectors = new List<TestVector>();
            byte[] key = null, iv = null, plainText = null, cipherText = null;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (keyPattern.IsMatch(line))
                {
                    key = HexStringToByteArray(RemoveSpaces(keyPattern.Match(line).Groups[1].Value));
                }
                else if (ivPattern.IsMatch(line))
                {
                    iv = HexStringToByteArray(RemoveSpaces(ivPattern.Match(line).Groups[1].Value));
                }
                else if (plaintextPattern.IsMatch(line))
                {
                    plainText = HexStringToByteArray(RemoveSpaces(plaintextPattern.Match(line).Groups[1].Value));
                }
                else if (ciphertextPattern.IsMatch(line))
                {
                    cipherText = HexStringToByteArray(RemoveSpaces(ciphertextPattern.Match(line).Groups[1].Value));
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

        static string RemoveSpaces(string input)
        {
            return input.Replace(" ", "");
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
