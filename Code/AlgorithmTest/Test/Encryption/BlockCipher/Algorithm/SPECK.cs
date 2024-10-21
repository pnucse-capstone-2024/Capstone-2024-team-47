using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Encryption.BlockCipher.Algorithm
{
    public class SPECK : EncryptionAlgorithm
    {
        public SPECK(byte[] key) : base(key)
        {
            uint64_key = new ulong[2];
            rk = new ulong[32];
            uint64_key[0] = BitConverter.ToUInt64(key, 0);
            uint64_key[1] = BitConverter.ToUInt64(key, 8);

            keySchedule();
        }

        public override byte[] Encrypt(byte[] plainText)
        {
            if (plainText.Length != 16)
                throw new ArgumentException("plainText must be exactly 16 bytes long.");

            ulong[] Pt = new ulong[2];
            ulong[] Ct = new ulong[2];

            Pt[0] = BitConverter.ToUInt64(plainText, 0);
            Pt[1] = BitConverter.ToUInt64(plainText, 8);

            Ct[0] = Pt[0]; Ct[1] = Pt[1];
            for (int i = 0; i < 32; i++)
            {
                Ct[1] = ROTR64(Ct[1], 8);
                Ct[1] += Ct[0];
                Ct[1] ^= rk[i];
                Ct[0] = ROTL64(Ct[0], 3);
                Ct[0] ^= Ct[1];
            }

            byte[] cipherText = new byte[16];
            System.Buffer.BlockCopy(BitConverter.GetBytes(Ct[0]), 0, cipherText, 0, 8);
            System.Buffer.BlockCopy(BitConverter.GetBytes(Ct[1]), 0, cipherText, 8, 8);

            return cipherText;
        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            if (cipherText.Length != 16)
                throw new ArgumentException("cipherText must be exactly 16 bytes long.");

            ulong[] Ct = new ulong[2];
            ulong[] Pt = new ulong[2];

            Ct[0] = BitConverter.ToUInt64(cipherText, 0);
            Ct[1] = BitConverter.ToUInt64(cipherText, 8);

            Pt[0] = Ct[0]; Pt[1] = Ct[1];
            for (int i = 31; i >= 0; i--)
            {
                Pt[0] ^= Pt[1];
                Pt[0] = ROTR64(Pt[0], 3);
                Pt[1] ^= rk[i];
                Pt[1] -= Pt[0];
                Pt[1] = ROTL64(Pt[1], 8);
            }

            byte[] plainText = new byte[16];
            System.Buffer.BlockCopy(BitConverter.GetBytes(Pt[0]), 0, plainText, 0, 8);
            System.Buffer.BlockCopy(BitConverter.GetBytes(Pt[1]), 0, plainText, 8, 8);

            return plainText;
        }

        public override string AlgorithmName => "SPECK";

        public override int GetBlockSize()
        {
            return 16;
        }

        private void keySchedule()
        {
            ulong B = uint64_key[1], A = uint64_key[0];

            for (int i = 0; i < 31; i++)
            {
                rk[i] = A;
                B = ROTR64(B, 8);
                B += A;
                B ^= (ulong)i;
                A = ROTL64(A, 3);
                A ^= B;
            }
            rk[31] = A;
        }

        private static ulong ROTL64(ulong x, int r)
        {
            return ((x) << (r)) | (x >> (64 - (r)));
        }

        private static ulong ROTR64(ulong x, int r)
        {
            return ((x) >> (r)) | ((x) << (64 - (r)));
        }

        private readonly ulong[] uint64_key;
        private readonly ulong[] rk;
    }
}
