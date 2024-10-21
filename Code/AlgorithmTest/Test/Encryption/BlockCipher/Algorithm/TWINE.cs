using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Encryption.BlockCipher.Algorithm
{
    public class TWINE : EncryptionAlgorithm
    {
        public TWINE(byte[] key) : base(key)
        {
            rk = new byte[36][];
            for (int i = 0; i < 36; i++)
            {
                rk[i] = new byte[8];
            }
            expandKeys();
        }

        public override byte[] Encrypt(byte[] plainText)
        {
            if (plainText.Length != 8)
                throw new ArgumentException("plainText must be exactly 8 bytes long.");

            byte[] x = new byte[16];

            for (int i = 0; i < 8; i++)
            {
                x[2 * i] = (byte)(plainText[i] >> 4);
                x[2 * i + 1] = (byte)(plainText[i] & 0x0f);
            }

            for (int i = 0; i < 35; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    x[2 * j + 1] ^= sbox[x[2 * j] ^ rk[i][j]];
                }

                byte[] xnext = new byte[16];
                for (int k = 0; k < 16; k++)
                {
                    xnext[shuffle[k]] = x[k];
                }

                Array.Copy(xnext, x, 16);
            }

            for (int j = 0; j < 8; j++)
            {
                x[2 * j + 1] ^= sbox[x[2 * j] ^ rk[35][j]];
            }

            byte[] cipherText = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                cipherText[i] = (byte)(x[2 * i] << 4 | x[2 * i + 1]);
            }

            return cipherText;
        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            if (cipherText.Length != 8)
                throw new ArgumentException("cipherText must be exactly 8 bytes long.");

            byte[] x = new byte[16];

            for (int i = 0; i < 8; i++)
            {
                x[2 * i] = (byte)(cipherText[i] >> 4);
                x[2 * i + 1] = (byte)(cipherText[i] & 0x0f);
            }

            for (int i = 35; i >= 1; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    x[2 * j + 1] ^= sbox[x[2 * j] ^ rk[i][j]];
                }

                byte[] xnext = new byte[16];
                for (int k = 0; k < 16; k++)
                {
                    xnext[shuffle_inv[k]] = x[k];
                }

                Array.Copy(xnext, x, 16);
            }

            for (int j = 0; j < 8; j++)
            {
                x[2 * j + 1] ^= sbox[x[2 * j] ^ rk[0][j]];
            }

            byte[] plainText = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                plainText[i] = (byte)(x[2 * i] << 4 | x[2 * i + 1]);
            }

            return plainText;
        }

        public override string AlgorithmName => "TWINE";

        public override int GetBlockSize()
        {
            return 8;
        }

        private void expandKeys()
        {
            byte[] wk = new byte[32];

            for (int i = 0; i < 16; i++)
            {
                wk[2 * i] = (byte)(key[i] >> 4);
                wk[2 * i + 1] = (byte)(key[i] & 0x0f);
            }

            for (int i = 0; i < 35; i++)
            {
                rk[i][0] = wk[2];
                rk[i][1] = wk[3];
                rk[i][2] = wk[12];
                rk[i][3] = wk[15];
                rk[i][4] = wk[17];
                rk[i][5] = wk[18];
                rk[i][6] = wk[28];
                rk[i][7] = wk[31];

                wk[1] ^= sbox[wk[0]];
                wk[4] ^= sbox[wk[16]];
                wk[7] ^= (byte)(rcon[i] >> 3);
                wk[19] ^= (byte)(rcon[i] & 7);
                wk[23] ^= sbox[wk[30]];

                int tmp0 = wk[0];
                int tmp1 = wk[1];
                int tmp2 = wk[2];
                int tmp3 = wk[3];

                for (int j = 0; j < 7; j++)
                {
                    wk[j * 4] = wk[j * 4 + 4];
                    wk[j * 4 + 1] = wk[j * 4 + 5];
                    wk[j * 4 + 2] = wk[j * 4 + 6];
                    wk[j * 4 + 3] = wk[j * 4 + 7];
                }
                wk[28] = (byte)tmp1;
                wk[29] = (byte)tmp2;
                wk[30] = (byte)tmp3;
                wk[31] = (byte)tmp0;
            }

            rk[35][0] = wk[2];
            rk[35][1] = wk[3];
            rk[35][2] = wk[12];
            rk[35][3] = wk[15];
            rk[35][4] = wk[17];
            rk[35][5] = wk[18];
            rk[35][6] = wk[28];
            rk[35][7] = wk[31];
        }

        private readonly byte[][] rk;

        private static readonly byte[] sbox = new byte[16]
        {
            0x0C, 0x00, 0x0F, 0x0A, 0x02, 0x0B, 0x09, 0x05, 0x08, 0x03, 0x0D, 0x07, 0x01, 0x0E, 0x06, 0x04
        };

        private static readonly byte[] shuffle = new byte[16]
        {
            5, 0, 1, 4, 7, 12, 3, 8, 13, 6, 9, 2, 15, 10, 11, 14
        };

        private static readonly byte[] shuffle_inv = new byte[16]
        {
            1, 2, 11, 6, 3, 0, 9, 4, 7, 10, 13, 14, 5, 8, 15, 12
        };

        private static readonly byte[] rcon = new byte[36]
        {
            0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x03, 0x06, 0x0c, 0x18, 0x30, 0x23, 0x05, 0x0a, 0x14, 0x28, 0x13, 0x26,
            0x0f, 0x1e, 0x3c, 0x3b, 0x35, 0x29, 0x11, 0x22, 0x07, 0x0e, 0x1c, 0x38, 0x33, 0x25, 0x09, 0x12, 0x24, 0x0b
        };
    }
}
