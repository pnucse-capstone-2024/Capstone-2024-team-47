using System.Data.SqlTypes;
using System.Numerics;
using System.Security.Cryptography;
using Test.BlockCipherTest;
using Test.Encryption.BlockCipher.Algorithm;
using Test.Encryption.BlockCipher.OperationMode;
using Test.PublicKeyTest;

namespace Test
{
    public class Program
    {
        static void BlockCipherTest()
        {
            AESTest.Run();
            ARIATest.Run();
            HIGHTTest.Run();
            SPECKTest.Run();
            TWINETest.Run();
        }

        static void PublicKeyTest()
        {
            ECDSATest.Run();
            ECDHTest.Run();
        }

        public static void Main(string[] args)
        {
            BlockCipherTest();
            PublicKeyTest();
            // HKDFTest();


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

        static void printHex(byte[] data)
        {
            Console.WriteLine(BitConverter.ToString(data).Replace("-", " "));
        }

        static void HKDFTest()
        {
            byte[] IKM = HexStringToByteArray("0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b");
            Console.WriteLine();
            Console.WriteLine("IKM: ");
            printHex(IKM);

            byte[] salt = HexStringToByteArray("000102030405060708090a0b0c");
            Console.WriteLine();
            Console.WriteLine("salt: ");
            printHex(salt);

            byte[] info = HexStringToByteArray("f0f1f2f3f4f5f6f7f8f9");
            Console.WriteLine();
            Console.WriteLine("info: ");
            printHex(info);

            int L = 42;
            Console.WriteLine();
            Console.WriteLine("L : ");
            Console.WriteLine(L);

            byte[] PRK = HKDF.KeyExtract(salt, IKM);
            Console.WriteLine();
            Console.WriteLine("PRK : ");
            printHex(PRK);

            byte[] TestVectorPRK = HexStringToByteArray("077709362c2e32df0ddc3f0dc47bba6390b6c73bb50f9c3122ec844ad7c2b3e5");
            Console.WriteLine();
            Console.WriteLine("TestVectorPRK : ");
            printHex(TestVectorPRK);

            byte[] OKM = HKDF.KeyExpand(PRK, info, L);
            Console.WriteLine();
            Console.WriteLine("OKM");
            printHex(OKM);

            byte[] TestVectorOKM = HexStringToByteArray("3cb25f25faacd57a90434f64d0362f2a2d2d0a90cf1a5a4c5db02d56ecc4c5bf34007208d5b887185865");
            Console.WriteLine();
            Console.WriteLine("TestVectorOKM : ");
            printHex(TestVectorOKM);
        }
    }
}