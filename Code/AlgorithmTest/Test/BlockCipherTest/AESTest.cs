﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Test.Encryption.BlockCipher.Algorithm;
using Test.Encryption.BlockCipher.OperationMode;

namespace Test.BlockCipherTest
{
    public class AESTest
    {
        public static void Run()
        {
            Console.WriteLine("\n******* AES TEST START *******");
            ECB();
            CBC();
            CFB();
            OFB();
            CTR();
            GCM();
            Console.WriteLine("\n******* AES TEST END *********");
        }

        internal class AESTestVector
        {
            public byte[] Key { get; set; }
            public byte[] IV { get; set; }
            public byte[] PlainText { get; set; }
            public byte[] CipherText { get; set; }
            public byte[] AAD { get; set; }
            public byte[] Tag { get; set; }
            public bool IsFail { get; set; }
        }

        static void ECB()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/AES/ECB/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<AESTestVector> vectors = ReadTestVector(filePath);

                foreach (AESTestVector vector in vectors)
                {
                    AES aes = new(vector.Key);
                    ECB ecb = new(aes);
                    byte[] cipherText = ecb.Encrypt(vector.PlainText);

                    if (!SequenceEqual(cipherText, vector.CipherText, 0, 16)
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

                        Console.Write("DecryptText: ");
                        printHex(ecb.Decrypt(cipherText));
                        Console.Write("\n");

                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    Console.WriteLine($"[AES - ECB] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[AES - ECB] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void CBC()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/AES/CBC/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<AESTestVector> vectors = ReadTestVectorWithIV(filePath);

                foreach (AESTestVector vector in vectors)
                {
                    AES aes = new(vector.Key);
                    CBC cbc = new(aes);
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
                    Console.WriteLine($"[AES - CBC] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[AES - CBC] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void CFB()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/AES/CFB/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<AESTestVector> vectors = ReadTestVectorWithIV(filePath);

                foreach (AESTestVector vector in vectors)
                {
                    AES aes = new(vector.Key);
                    CFB cfb = new(aes);
                    byte[] cipherText = cfb.Encrypt(vector.PlainText, vector.IV);

                    if (!SequenceEqual(cipherText, vector.CipherText, 16, vector.CipherText.Length + 16)
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
                    Console.WriteLine($"[AES - CFB] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[AES - CFB] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void OFB()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/AES/OFB/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<AESTestVector> vectors = ReadTestVectorWithIV(filePath);

                foreach (AESTestVector vector in vectors)
                {
                    AES aes = new(vector.Key);
                    OFB ofb = new(aes);
                    byte[] cipherText = ofb.Encrypt(vector.PlainText, vector.IV);

                    if (!SequenceEqual(cipherText, vector.CipherText, 16, vector.CipherText.Length + 16)
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
                    Console.WriteLine($"[AES - OFB] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[AES - OFB] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void CTR()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/AES/CTR/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<AESTestVector> vectors = ReadTestVectorWithIV(filePath);

                foreach (AESTestVector vector in vectors)
                {
                    AES aes = new(vector.Key);
                    CTR ctr = new(aes);
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
                    Console.WriteLine($"[AES - CTR] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[AES - CTR] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static void GCM()
        {
            Console.WriteLine();
            string folderPath = "../../../test-vector/AES/GCM/";
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                bool flag = true;
                List<AESTestVector> vectors = ReadTestVectorWithTag(filePath);

                foreach (AESTestVector vector in vectors)
                {
                    AES aes = new(vector.Key);
                    GCM gcm = new(aes);
                    (byte[] cipherText, byte[] authTag) = gcm.Encrypt(vector.PlainText, vector.IV, vector.AAD);
                    (bool validation, byte[] plainText) = gcm.Decrypt(cipherText, vector.IV, vector.AAD, vector.Tag);


                    if (!ValidationCheck(vector, validation, plainText, cipherText))
                    {
                        Console.Write("Key: ");
                        printHex(vector.Key);
                        Console.Write("\n");

                        Console.Write("IV: ");
                        printHex(vector.IV);
                        Console.Write("\n");

                        Console.Write("vector.PlainText: ");
                        printHex(vector.PlainText);
                        Console.Write("\n");

                        Console.Write("vector.AAD: ");
                        printHex(vector.AAD);
                        Console.Write("\n");

                        Console.Write("vector.CipherText: ");
                        printHex(vector.CipherText);
                        Console.Write("\n");

                        Console.Write("vector.Tag: ");
                        printHex(vector.Tag);
                        Console.Write("\n");

                        Console.Write("CipherText: ");
                        printHex(cipherText);
                        Console.Write("\n");

                        Console.Write("DecryptText: ");
                        printHex(plainText);
                        Console.Write("\n");

                        Console.Write("authTag: ");
                        printHex(authTag);
                        Console.Write("\n");

                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    Console.WriteLine($"[AES - GCM] [Success]\t {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"[AES - GCM] [Fail]\t {Path.GetFileName(filePath)}");
                }
            }
        }

        static List<AESTestVector> ReadTestVector(string filePath)
        {
            Regex keyPattern = new Regex(@"KEY\s*=\s*([0-9a-fA-F]+)");
            Regex plaintextPattern = new Regex(@"PLAINTEXT\s*=\s*([0-9a-fA-F]+)");
            Regex ciphertextPattern = new Regex(@"CIPHERTEXT\s*=\s*([0-9a-fA-F]+)");

            List<AESTestVector> vectors = new List<AESTestVector>();
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
                    vectors.Add(new AESTestVector
                    {
                        Key = key, PlainText = plainText, CipherText = cipherText
                    });
                    key = plainText = cipherText = null;
                }
            }

            return vectors;
        }

        static List<AESTestVector> ReadTestVectorWithIV(string filePath)
        {
            Regex keyPattern = new Regex(@"KEY\s*=\s*([0-9a-fA-F]+)");
            Regex ivPattern = new Regex(@"IV\s*=\s*([0-9a-fA-F]+)");
            Regex plaintextPattern = new Regex(@"PLAINTEXT\s*=\s*([0-9a-fA-F]+)");
            Regex ciphertextPattern = new Regex(@"CIPHERTEXT\s*=\s*([0-9a-fA-F]+)");

            List<AESTestVector> vectors = new List<AESTestVector>();
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
                    vectors.Add(new AESTestVector
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

        static List<AESTestVector> ReadTestVectorWithTag(string filePath)
        {
            Regex keyPattern        = new Regex(@"Key\s*=\s*([0-9a-fA-F]+)");
            Regex ivPattern         = new Regex(@"IV\s*=\s*([0-9a-fA-F]+)");
            Regex plaintextPattern  = new Regex(@"PT\s*=\s*([0-9a-fA-F]*)");
            Regex aadPattern        = new Regex(@"AAD\s*=\s*([0-9a-fA-F]*)");
            Regex ciphertextPattern = new Regex(@"CT\s*=\s*([0-9a-fA-F]*)");
            Regex tagPattern        = new Regex(@"Tag\s*=\s*([0-9a-fA-F]+)");
            Regex countPattern      = new Regex(@"Count\s*=\s*\d+");
            Regex failPattern       = new Regex(@"FAIL");

            List<AESTestVector> vectors = new List<AESTestVector>();
            byte[] key = null, iv = null, plainText = null, aad = null, cipherText = null, tag = null;
            bool isFail = false;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (countPattern.IsMatch(line))
                {
                    if (key != null && iv != null && tag != null)
                    {
                        vectors.Add(new AESTestVector
                        {
                            Key = key,
                            IV = iv,
                            PlainText = plainText ?? [],
                            AAD = aad ?? [],
                            CipherText = cipherText ?? [],
                            Tag = tag,
                            IsFail = isFail
                        });
                        key = iv = plainText = aad = cipherText = tag = null;
                        isFail = false;
                    }
                }
                else if (keyPattern.IsMatch(line))
                {
                    key = HexStringToByteArray(keyPattern.Match(line).Groups[1].Value);
                }
                else if (ivPattern.IsMatch(line))
                {
                    iv = HexStringToByteArray(ivPattern.Match(line).Groups[1].Value);
                }
                else if (plaintextPattern.IsMatch(line))
                {
                    string temp = plaintextPattern.Match(line).Groups[1].Value;
                    plainText = string.IsNullOrEmpty(temp) ? [] : HexStringToByteArray(temp);
                }
                else if (aadPattern.IsMatch(line))
                {
                    string temp = aadPattern.Match(line).Groups[1].Value;
                    aad = string.IsNullOrEmpty(temp) ? [] : HexStringToByteArray(temp);
                }
                else if (ciphertextPattern.IsMatch(line))
                {
                    string temp = ciphertextPattern.Match(line).Groups[1].Value;
                    cipherText = string.IsNullOrEmpty(temp) ? [] : HexStringToByteArray(temp);
                }
                else if (tagPattern.IsMatch(line))
                {
                    tag = HexStringToByteArray(tagPattern.Match(line).Groups[1].Value);
                }
                else if (failPattern.IsMatch(line))
                {
                    isFail = true;
                }
            }

            if (key != null && iv != null && tag != null)
            {
                vectors.Add(new AESTestVector
                {
                    Key = key,
                    IV = iv,
                    PlainText = plainText ?? [],
                    AAD = aad ?? [],
                    CipherText = cipherText ?? [],
                    Tag = tag,
                    IsFail = isFail
                });
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

        static bool ValidationCheck(AESTestVector vector, bool validation, byte[] plainText, byte[] cipherText)
        {
            // 정상 태그로 판정하고 테스트 벡터의 결과가 비정상 태그인 경우
            if (validation && vector.IsFail) 
                return false;

            // 비정상 태그로 판정하고 테스트 벡터의 결과가 정상 태그인 경우
            if (!validation && !vector.IsFail) 
                return false;

            // 정상 태그로 판정한 상태에서 암호문과 테스트 벡터의 암호문이 다른 경우
            if (validation && !cipherText.SequenceEqual(vector.CipherText)) 
                return false;

            // 정상 태그로 판정한 상태에서 복호화한 암호문과 테스트 벡터의 평문이 다른 경우
            if (validation && !plainText.SequenceEqual(vector.PlainText)) 
                return false;

            return true;
        }
    }
}