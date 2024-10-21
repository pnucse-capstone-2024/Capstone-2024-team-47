using Test.Encryption.BlockCipher.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Encryption.BlockCipher.OperationMode
{
    public class GCM : OperationMode
    {
        // 128bit 전용 GCM, 64bit는 GCM64 사용

        const int IVLength = 12;
        const int TagSize = 16;
        const int blockSize = 16;
        private byte[] H;

        public GCM(IEncryptionAlgorithm encryptionAlgorithm) : base(encryptionAlgorithm)
        {
            if (encryptionAlgorithm.GetBlockSize() != blockSize)
            {
                throw new ArgumentException($"Invalid block size: {encryptionAlgorithm.GetBlockSize()}. GCM requires a block size of {blockSize} bytes. Use GCM64.");
            }

            H = new byte[16];
            H = encryptionAlgorithm.Encrypt(H);
        }

        public override byte[] Encrypt(byte[] plainText)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();

            // IV is random bytes.
            byte[] iv = new byte[blockSize];
            new Random().NextBytes(iv);

            // AAD is not used.
            byte[] aad = new byte[0];

            byte[] cipherText = CTREncrypt(plainText, iv);
            byte[] authTag = CalculateTag(iv, aad, cipherText);

            // IV will append the head of packet.
            // Tag will append next to IV.
            byte[] ret = new byte[blockSize + blockSize + cipherText.Length];
            Array.Copy(iv, 0, ret, 0, blockSize);
            Array.Copy(authTag, 0, ret, blockSize, blockSize);
            Array.Copy(cipherText, 0, ret, blockSize + blockSize, cipherText.Length);

            return ret;
        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            byte[] iv = new byte[blockSize];
            Array.Copy(cipherText, 0, iv, 0, blockSize);

            byte[] authTag = new byte[blockSize];
            Array.Copy(cipherText, blockSize, authTag, 0, blockSize);

            byte[] temp = new byte[cipherText.Length - blockSize - blockSize];
            Array.Copy(cipherText, blockSize + blockSize, temp, 0, temp.Length);

            byte[] aad = new byte[0];

            byte[] plainText = CTREncrypt(temp, iv);
            byte[] validationCheckAuthTag = CalculateTag(iv, aad, temp);

            // if tag validation check failed, throw error
            if (!validationCheckAuthTag.SequenceEqual(authTag))
                throw new Exception("Tag Check Error");

            return plainText;
        }

        public (byte[] cipherText, byte[] authTag) Encrypt(byte[] plainText, byte[] iv, byte[] aad)
        {
            byte[] cipherText = CTREncrypt(plainText, iv);
            byte[] authTag = CalculateTag(iv, aad, cipherText);

            return (cipherText, authTag);
        }

        public (bool validation, byte[] plainText) Decrypt(byte[] cipherText, byte[] iv, byte[] aad, byte[] authTag)
        {
            byte[] plainText = CTRDecrypt(cipherText, iv);
            byte[] validationCheckAuthTag = CalculateTag(iv, aad, cipherText);

            return (TagValidationCheck(authTag, validationCheckAuthTag), plainText);
        }

        private byte[] CalculateTag(byte[] iv, byte[] aad, byte[] cipherText)
        {
            byte[] Tag = encryptionAlgorithm.Encrypt(calculateY0(iv));

            byte[] X = new byte[TagSize];
            X = GHASH(X, aad);
            X = GHASH(X, cipherText);
            X = GHASHwithLen(X, aad.Length, cipherText.Length);

            for (int i = 0; i < TagSize; i++)
            {
                Tag[i] ^= X[i];
            }

            return Tag;
        }

        public byte[] calculateY0(byte[] iv)
        {
            byte[] counter = new byte[blockSize];

            if (iv.Length == 12)
            {
                Array.Copy(iv, counter, IVLength);
                IncrementCounter(counter, blockSize);
            }
            else
            {
                byte[] X = new byte[blockSize];
                X = GHASH(X, iv);

                byte[] lengthBlock = new byte[blockSize];
                ulong ivLengthBits = (ulong)iv.Length * 8;
                for (int i = 0; i < 8; i++)
                {
                    lengthBlock[15 - i] = (byte)(ivLengthBits >> (i * 8));
                }

                X = GHASH(X, lengthBlock);
                Array.Copy(X, counter, blockSize);
            }
            return counter;
        }

        private void IncrementCounter(byte[] counter, int blockSize)
        {
            for (int i = blockSize - 1; i >= IVLength; i--)
            {
                counter[i]++;
                if (counter[i] != 0)
                {
                    break;
                }
            }
        }

        private byte[] CTREncrypt(byte[] plainText, byte[] iv)
        {
            byte[] cipherText = new byte[plainText.Length];

            byte[] counter = calculateY0(iv);
            IncrementCounter(counter, blockSize);

            for (int i = 0; i < plainText.Length; i += blockSize)
            {
                byte[] encryptedCounter = encryptionAlgorithm.Encrypt(counter);
                IncrementCounter(counter, blockSize);

                int bytesToProcess = Math.Min(blockSize, plainText.Length - i);
                Array.Copy(plainText, i, cipherText, i, bytesToProcess);

                for (int j = 0; j < bytesToProcess; j++)
                {
                    cipherText[i + j] ^= encryptedCounter[j];
                }
            }

            return cipherText;
        }

        private byte[] CTRDecrypt(byte[] cipherText, byte[] iv)
        {
            byte[] plainText = new byte[cipherText.Length];

            byte[] counter = calculateY0(iv);
            IncrementCounter(counter, blockSize);

            for (int i = 0; i < cipherText.Length; i += blockSize)
            {
                byte[] encryptedCounter = encryptionAlgorithm.Encrypt(counter);
                IncrementCounter(counter, blockSize);

                int bytesToProcess = Math.Min(blockSize, cipherText.Length - i);
                Array.Copy(cipherText, i, plainText, i, bytesToProcess);

                for (int j = 0; j < bytesToProcess; j++)
                {
                    plainText[i + j] ^= encryptedCounter[j];
                }
            }

            return plainText;
        }

        private byte[] GFMultiply(byte[] x, byte[] y)
        {
            byte[] z = new byte[16];
            byte[] v = new byte[16]; Array.Copy(x, v, 16);
            byte[] R = new byte[16]; R[0] = 0xE1;

            for (int i = 0; i < 16; i++)
            {
                byte temp = y[i];
                for (int j = 0; j < 8; j++)
                {
                    if (((temp >>> (7 - j)) & 0x01) == 1)
                    {
                        for (int k = 0; k < 16; k++)
                        {
                            z[k] ^= v[k];
                        }
                    }

                    if ((v[15] & 0x01) == 0)
                    {
                        v = RightShift(v);
                    }
                    else
                    {
                        v = RightShift(v);
                        v[0] ^= R[0];
                    }
                }
            }

            return z;
        }

        private byte[] RightShift(byte[] v)
        {
            v[15] = (byte)(v[15] >>> 1);
            for (int i = 14; i >= 0; i--)
            {
                if ((v[i] & 0x01) == 1)
                {
                    v[i + 1] = (byte)(v[i + 1] + 0x80);
                }
                v[i] = (byte)(v[i] >>> 1);
            }
            return v;
        }

        private byte[] GHASH(byte[] X, byte[] A)
        {
            for (int i = 0; i < A.Length; i += 16)
            {
                byte[] block = new byte[16];
                Array.Copy(A, i, block, 0, Math.Min(16, A.Length - i));

                for (int j = 0; j < 16; j++)
                {
                    X[j] ^= block[j];
                }

                X = GFMultiply(X, H);
            }

            return X;
        }

        private byte[] GHASHwithLen(byte[] X, int len_A, int len_C)
        {
            byte[] lenA_lenC = new byte[16];
            ulong lenA = (ulong)len_A * 8;
            ulong lenC = (ulong)len_C * 8;

            for (int i = 0; i < 8; i++)
            {
                lenA_lenC[7 - i] = (byte)(lenA >> (i * 8));
                lenA_lenC[15 - i] = (byte)(lenC >> (i * 8));
            }

            for (int i = 0; i < 16; i++)
            {
                X[i] ^= lenA_lenC[i];
            }

            return GFMultiply(X, H);
        }

        private bool TagValidationCheck(byte[] Tag1, byte[] Tag2)
        {
            int minLength = Math.Min(Tag1.Length, Tag2.Length);

            for (int i = 0; i < minLength; i++)
            {
                if (Tag1[i] != Tag2[i])
                    return false;
            }

            return true;
        }

        public override string ModeName => "GCM";

        public override string AlgorithmName => encryptionAlgorithm.AlgorithmName;
    }
}
