using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.Encryption.PublicKey
{
    // p-256, sha-256
    public class ECDSA : ECC
    {
        public ECDSA(BigInteger a, BigInteger b, BigInteger p, BigInteger n, ECPoint G) : base(a, b, p, n, G) { }

        public static byte[] ComputeSHA256Hash(byte[] data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(data);
            }
        }

        public byte[] Sign(byte[] message, BigInteger privKey)
        {
            byte[] hash = ComputeSHA256Hash(message);
            BigInteger z = new(hash, isUnsigned: true, isBigEndian: true);

            if (z >= n)
            {
                z >>= (z.GetByteCount() * 8 - n.GetByteCount() * 8);
            }

            BigInteger r = 0, s = 0;

            while (s == 0)
            {
                BigInteger k = GenerateRandomBigInteger(n);
                ECPoint R = MultiplyPoint(G, k);

                r = R.X % n;

                if (r == 0) continue;

                s = (ModInverse(k, n) * (z + r * privKey)) % n;

                if (s == 0) continue;
            }

            byte[] rBytes = r.ToByteArray(isUnsigned: true, isBigEndian: true);
            byte[] sBytes = s.ToByteArray(isUnsigned: true, isBigEndian: true);

            byte[] rPadded = new byte[32], sPadded = new byte[32];
            Array.Copy(rBytes, 0, rPadded, 32 - rBytes.Length, rBytes.Length);
            Array.Copy(sBytes, 0, sPadded, 32 - sBytes.Length, sBytes.Length);

            byte[] signature = new byte[64];
            Array.Copy(rPadded, 0, signature, 0, 32);
            Array.Copy(sPadded, 0, signature, 32, 32);

            return signature;
        }

        public byte[] Sign(byte[] message, BigInteger privKey, BigInteger k)
        {
            byte[] hash = ComputeSHA256Hash(message);
            BigInteger z = new(hash, isUnsigned: true, isBigEndian: true);

            if (z >= n)
            {
                z >>= (z.GetByteCount() * 8 - n.GetByteCount() * 8);
            }

            BigInteger r = 0, s = 0;

            while (s == 0)
            {
                ECPoint R = MultiplyPoint(G, k);

                r = R.X % n;

                if (r == 0) continue;

                s = (ModInverse(k, n) * (z + r * privKey)) % n;

                if (s == 0) continue;
            }

            byte[] rBytes = r.ToByteArray(isUnsigned: true, isBigEndian: true);
            byte[] sBytes = s.ToByteArray(isUnsigned: true, isBigEndian: true);

            byte[] rPadded = new byte[32], sPadded = new byte[32];
            Array.Copy(rBytes, 0, rPadded, 32 - rBytes.Length, rBytes.Length);
            Array.Copy(sBytes, 0, sPadded, 32 - sBytes.Length, sBytes.Length);

            byte[] signature = new byte[64];
            Array.Copy(rPadded, 0, signature, 0, 32);
            Array.Copy(sPadded, 0, signature, 32, 32);

            return signature;
        }

        public bool Verify(byte[] message, byte[] signature, ECPoint pubKey)
        {
            byte[] hash = ComputeSHA256Hash(message);
            BigInteger z = new(hash, isUnsigned: true, isBigEndian: true);

            if (z >= n)
            {
                z >>= (z.GetByteCount() * 8 - n.GetByteCount() * 8);
            }

            int halfLength = signature.Length / 2;
            byte[] rBytes = new byte[halfLength];
            byte[] sBytes = new byte[halfLength];
            Array.Copy(signature, 0, rBytes, 0, halfLength);
            Array.Copy(signature, halfLength, sBytes, 0, halfLength);

            BigInteger r = new(rBytes, isUnsigned: true, isBigEndian: true);
            BigInteger s = new(sBytes, isUnsigned: true, isBigEndian: true);

            if (r <= 0 || r >= n || s <= 0 || s >= n)
                return false;

            BigInteger w = ModInverse(s, n);
            BigInteger u1 = (z * w) % n;
            BigInteger u2 = (r * w) % n;

            ECPoint point1 = MultiplyPoint(G, u1);
            ECPoint point2 = MultiplyPoint(pubKey, u2);
            ECPoint R = AddPoints(point1, point2);

            return (!R.IsInfinity()) && (R.X % n == r);
        }

        public bool Verify(byte[] message, BigInteger r, BigInteger s, ECPoint pubKey)
        {
            byte[] hash = ComputeSHA256Hash(message);
            BigInteger z = new(hash, isUnsigned: true, isBigEndian: true);

            if (z >= n)
            {
                z >>= (z.GetByteCount() * 8 - n.GetByteCount() * 8);
            }

            if (r <= 0 || r >= n || s <= 0 || s >= n)
                return false;

            BigInteger w = ModInverse(s, n);
            BigInteger u1 = (z * w) % n;
            BigInteger u2 = (r * w) % n;

            ECPoint point1 = MultiplyPoint(G, u1);
            ECPoint point2 = MultiplyPoint(pubKey, u2);
            ECPoint R = AddPoints(point1, point2);

            return (!R.IsInfinity()) && (R.X % n == r);
        }

        public static string AlgorithmName => "ECDSA";
    }
}
