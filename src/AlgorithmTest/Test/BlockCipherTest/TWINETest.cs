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
    public class TWINETest
    {
        public static void Run()
        {
            Console.WriteLine("\n******* TWINE TEST START *******");
            ECB();
            CBC();
            CFB();
            OFB();
            CTR();
            GCM();
            Console.WriteLine("\n******* TWINE TEST END *********");
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
            string folderPath = "../../../test-vector/TWINE/ECB/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<TestVector> vectors = ReadTestVector(filePath);

                foreach (TestVector vector in vectors)
                {
                    TWINE twine = new(vector.Key);
                    ECB ecb = new(twine);
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
                    Console.WriteLine($"[TWINE - ECB] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[TWINE - ECB] [Fail]\t {Path.GetFileName(filePath)}");
                }
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

        static void CBC()
        {
            Console.WriteLine();
            Console.WriteLine("[TWINE - CBC] [Fail] There is no test vectors.");
        }

        static void CFB()
        {
            Console.WriteLine();
            Console.WriteLine("[TWINE - CFB] [Fail] There is no test vectors.");
        }

        static void OFB()
        {
            Console.WriteLine();
            Console.WriteLine("[TWINE - OFB] [Fail] There is no test vectors.");
        }

        static void CTR()
        {
            Console.WriteLine();
            Console.WriteLine("[TWINE - CTR] [Fail] There is no test vectors.");
        }

        static void GCM()
        {
            Console.WriteLine();
            Console.WriteLine("[TWINE - GCM] [Fail] There is no test vectors.");
        }
    }
}
