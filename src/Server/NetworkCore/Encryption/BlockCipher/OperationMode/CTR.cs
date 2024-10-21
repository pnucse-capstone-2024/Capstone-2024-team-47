using NetworkCore.Encryption.BlockCipher.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.Encryption.BlockCipher.OperationMode
{
    public class CTR(IEncryptionAlgorithm encryptionAlgorithm) : OperationMode(encryptionAlgorithm)
    {
        public override byte[] Encrypt(byte[] plainText)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] cipherText = new byte[plainText.Length + blockSize];

            byte[] nonce = new byte[blockSize];
            new Random().NextBytes(nonce);
            Array.Copy(nonce, 0, cipherText, 0, blockSize);

            byte[] counter = new byte[blockSize];
            Array.Copy(nonce, counter, blockSize);

            for (int i = 0; i < plainText.Length; i += blockSize)
            {
                byte[] encryptedCounter = encryptionAlgorithm.Encrypt(counter);
                IncrementCounter(counter, blockSize);

                int bytesToProcess = Math.Min(blockSize, plainText.Length - i);
                Array.Copy(plainText, i, cipherText, i + blockSize, bytesToProcess);

                for (int j = 0; j < bytesToProcess; j++)
                {
                    cipherText[i + blockSize + j] ^= encryptedCounter[j];
                }
            }

            return cipherText;
        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] plainText = new byte[cipherText.Length - blockSize];

            byte[] counter = new byte[blockSize];
            Array.Copy(cipherText, 0, counter, 0, blockSize);

            for (int i = blockSize; i < cipherText.Length; i += blockSize)
            {
                byte[] encryptedCounter = encryptionAlgorithm.Encrypt(counter);
                IncrementCounter(counter, blockSize);

                int bytesToProcess = Math.Min(blockSize, cipherText.Length - i);

                for (int j = 0; j < bytesToProcess; j++)
                {
                    plainText[i - blockSize + j] = (byte)(encryptedCounter[j] ^ cipherText[i + j]);
                }
            }

            return plainText;
        }

        public byte[] Encrypt(byte[] plainText, byte[] nonce)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] cipherText = new byte[plainText.Length + blockSize];

            Array.Copy(nonce, 0, cipherText, 0, blockSize);

            byte[] counter = new byte[blockSize];
            Array.Copy(nonce, counter, blockSize);

            for (int i = 0; i < plainText.Length; i += blockSize)
            {
                byte[] encryptedCounter = encryptionAlgorithm.Encrypt(counter);
                IncrementCounter(counter, blockSize);

                int bytesToProcess = Math.Min(blockSize, plainText.Length - i);
                Array.Copy(plainText, i, cipherText, i + blockSize, bytesToProcess);

                for (int j = 0; j < bytesToProcess; j++)
                {
                    cipherText[i + blockSize + j] ^= encryptedCounter[j];
                }
            }

            return cipherText;
        }

        private void IncrementCounter(byte[] counter, int blockSize)
        {
            for (int i = blockSize - 1; i >= 0; i--)
            {
                counter[i]++;
                if (counter[i] != 0)
                {
                    break;
                }
            }
        }

        public override string ModeName => "CTR";
        public override string AlgorithmName => encryptionAlgorithm.AlgorithmName;
    }
}
