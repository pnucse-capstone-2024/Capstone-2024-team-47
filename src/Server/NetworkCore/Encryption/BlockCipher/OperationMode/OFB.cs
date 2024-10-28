using NetworkCore.Encryption.BlockCipher.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.Encryption.BlockCipher.OperationMode
{
    public class OFB(IEncryptionAlgorithm encryptionAlgorithm) : OperationMode(encryptionAlgorithm)
    {
        public override byte[] Encrypt(byte[] plainText)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] cipherText = new byte[plainText.Length + blockSize];

            // IV is random bytes. It will append on the head of cipherText.
            byte[] iv = new byte[blockSize];
            new Random().NextBytes(iv);
            Array.Copy(iv, 0, cipherText, 0, blockSize);

            byte[] feedback = iv;
            for (int i = 0; i < plainText.Length; i += blockSize)
            {
                byte[] encryptedFeedback = encryptionAlgorithm.Encrypt(feedback);
                Array.Copy(encryptedFeedback, feedback, blockSize);

                int bytesToProcess = Math.Min(blockSize, plainText.Length - i);
                Array.Copy(plainText, i, cipherText, i + blockSize, bytesToProcess);

                for (int j = 0; j < bytesToProcess; j++)
                {
                    cipherText[i + blockSize + j] ^= encryptedFeedback[j];
                }
            }

            return cipherText;
        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] plainText = new byte[cipherText.Length - blockSize];

            // IV is at the head of cipherText.
            byte[] iv = new byte[blockSize];
            Array.Copy(cipherText, 0, iv, 0, blockSize);

            byte[] feedback = iv;
            for (int i = blockSize; i < cipherText.Length; i += blockSize)
            {
                byte[] encryptedFeedback = encryptionAlgorithm.Encrypt(feedback);
                Array.Copy(encryptedFeedback, feedback, blockSize);

                int bytesToProcess = Math.Min(blockSize, cipherText.Length - i);

                for (int j = 0; j < bytesToProcess; j++)
                {
                    plainText[i - blockSize + j] = (byte)(encryptedFeedback[j] ^ cipherText[i + j]);
                }
            }

            return plainText;
        }

        public byte[] Encrypt(byte[] plainText, byte[] iv)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] cipherText = new byte[plainText.Length + blockSize];

            Array.Copy(iv, 0, cipherText, 0, blockSize);

            byte[] feedback = iv;
            for (int i = 0; i < plainText.Length; i += blockSize)
            {
                byte[] encryptedFeedback = encryptionAlgorithm.Encrypt(feedback);
                Array.Copy(encryptedFeedback, feedback, blockSize);

                int bytesToProcess = Math.Min(blockSize, plainText.Length - i);
                Array.Copy(plainText, i, cipherText, i + blockSize, bytesToProcess);

                for (int j = 0; j < bytesToProcess; j++)
                {
                    cipherText[i + blockSize + j] ^= encryptedFeedback[j];
                }
            }

            return cipherText;
        }

        public override string ModeName => "OFB";
        public override string AlgorithmName => encryptionAlgorithm.AlgorithmName;
    }
}
