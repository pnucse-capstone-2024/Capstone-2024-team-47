using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Test.Encryption.PublicKey;

namespace Test.PublicKeyTest
{
    public class ECDHTest
    {
        public static void Run()
        {
            BigInteger a = HexStringToBigInteger("ffffffff00000001000000000000000000000000fffffffffffffffffffffffc"),
                       b = HexStringToBigInteger("5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b"),
                       p = HexStringToBigInteger("ffffffff00000001000000000000000000000000ffffffffffffffffffffffff"),
                       n = HexStringToBigInteger("ffffffff00000000ffffffffffffffffbce6faada7179e84f3b9cac2fc632551"),
                       gx = HexStringToBigInteger("6b17d1f2e12c4247f8bce6e563a440f277037d812deb33a0f4a13945d898c296"),
                       gy = HexStringToBigInteger("4fe342e2fe1a7f9b8ee7eb4a7c0f9e162bce33576b315ececbb6406837bf51f5");
            ECDH ecdh = new(a, b, p, n, new ECPoint(gx, gy));

            string filePath;

            Console.WriteLine("\n******* ECDH(P-256) TEST START *******\n");

            filePath = "../../../test-vector/ECDH/ECDHTestVectorP256.txt";

            if (IUTTest(ecdh, filePath))
            {
                Console.WriteLine($"[ECDH - IUTTest]\t[Success]\t{Path.GetFileName(filePath)}");
            }
            else
            {
                Console.WriteLine($"[ECDH - IUTTest]\t[Fail]   \t{Path.GetFileName(filePath)}");
            }

            Console.WriteLine("\n******* ECDH(P-256) TEST END *******\n");
        }

        internal class ECDHTestVector
        {
            public BigInteger QCAVSx;
            public BigInteger QCAVSy;
            public BigInteger dIUT;
            public BigInteger QIUTx;
            public BigInteger QIUTy;
            public BigInteger ZIUT;
        }

        static bool IUTTest(ECDH ecdh, string filePath)
        {
            Regex QCAVSxPattern     = new Regex(@"QCAVSx\s*=\s*([0-9a-fA-F]+)");
            Regex QCAVSyPattern     = new Regex(@"QCAVSy\s*=\s*([0-9a-fA-F]+)");
            Regex dIUTPattern       = new Regex(@"dIUT\s*=\s*([0-9a-fA-F]+)");
            Regex QIUTxPattern      = new Regex(@"QIUTx\s*=\s*([0-9a-fA-F]+)");
            Regex QIUTyPattern      = new Regex(@"QIUTy\s*=\s*([0-9a-fA-F]+)");
            Regex ZIUTPattern       = new Regex(@"ZIUT\s*=\s*([0-9a-fA-F]+)");

            List<ECDHTestVector> vectors = new List<ECDHTestVector>();
            bool qcavsx_flag = false, qcavsy_flag = false, diut_flag = false, qiutx_flag = false, qiuty_flag = false, ziut_flag = false;
            BigInteger QCAVSx = 0, QCAVSy = 0, dIUT = 0, QIUTx = 0, QIUTy = 0, ZIUT = 0;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (QCAVSxPattern.IsMatch(line))
                {
                    QCAVSx = HexStringToBigInteger(QCAVSxPattern.Match(line).Groups[1].Value);
                    qcavsx_flag = true;
                }
                else if (QCAVSyPattern.IsMatch(line))
                {
                    QCAVSy = HexStringToBigInteger(QCAVSyPattern.Match(line).Groups[1].Value);
                    qcavsy_flag = true;
                }
                else if (dIUTPattern.IsMatch(line))
                {
                    dIUT = HexStringToBigInteger(dIUTPattern.Match(line).Groups[1].Value);
                    diut_flag = true;
                }
                else if (QIUTxPattern.IsMatch(line))
                {
                    QIUTx = HexStringToBigInteger(QIUTxPattern.Match(line).Groups[1].Value);
                    qiutx_flag = true;
                }
                else if (QIUTyPattern.IsMatch(line))
                {
                    QIUTy = HexStringToBigInteger(QIUTyPattern.Match(line).Groups[1].Value);
                    qiuty_flag = true;
                }
                else if (ZIUTPattern.IsMatch(line))
                {
                    ZIUT = HexStringToBigInteger(ZIUTPattern.Match(line).Groups[1].Value);
                    ziut_flag = true;
                }

                if (qcavsx_flag && qcavsy_flag && diut_flag && qiutx_flag && qiuty_flag && ziut_flag)
                {
                    vectors.Add(new ECDHTestVector
                    {
                        QCAVSx = QCAVSx, 
                        QCAVSy = QCAVSy,
                        dIUT = dIUT,
                        QIUTx = QIUTx,
                        QIUTy = QIUTy,
                        ZIUT = ZIUT,
                    });
                    qcavsx_flag = qcavsy_flag = diut_flag = qiutx_flag = qiuty_flag = ziut_flag = false;
                }
            }

            foreach (ECDHTestVector vector in vectors)
            {
                /*
                ECPoint pubKey = ecdsa.GetPublicKey(vector.Key);
                if (pubKey.X != vector.Qx || pubKey.Y != vector.Qy)
                {
                    Console.Write($"Key: {vector.Key:X}\n");

                    Console.Write($"vector.Qx: {vector.Qx:X}\n");
                    Console.Write($"Qx: {pubKey.X:X}\n");

                    Console.Write($"vector.Qy: {vector.Qy:X}\n");
                    Console.Write($"Qy: {pubKey.Y:X}\n");

                    return false;
                }
                */
                ECPoint pubKeyA = new ECPoint(QCAVSx, QCAVSy);

                BigInteger privKeyB = dIUT;
                ECPoint pubKeyB = new ECPoint(QIUTx, QIUTy);
                ECPoint calculatedPubKeyB = ecdh.GetPublicKey(privKeyB);

                BigInteger sharedSecret = ecdh.ComputeSharedSecret(privKeyB, pubKeyA);

                // 공개키 생성 검증
                if (pubKeyB.X != calculatedPubKeyB.X || pubKeyB.Y != calculatedPubKeyB.Y)
                {
                    // 검증 실패
                    Console.WriteLine($"pubKeyB.X: {pubKeyB.X:x}");
                    Console.WriteLine($"calculatedPubKeyB.X: {calculatedPubKeyB.X:x}");

                    Console.WriteLine($"pubKeyB.Y: {pubKeyB.Y:x}");
                    Console.WriteLine($"calculatedPubKeyB.Y: {calculatedPubKeyB.Y:x}");

                    return false;
                }

                // sharedSecret 생성 검증
                if (!ZIUT.Equals(sharedSecret))
                {
                    Console.WriteLine($"dIUT:\t\t{dIUT:x}");
                    Console.WriteLine($"sharedSecret:\t{sharedSecret:x}");

                    return false;
                }
            }

            return true;
        }

        static BigInteger HexStringToBigInteger(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
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
    }
}
