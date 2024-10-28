using NetworkCore.Encryption.BlockCipher.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.Encryption.BlockCipher.OperationMode
{
    public class CBC(IEncryptionAlgorithm encryptionAlgorithm) : OperationMode(encryptionAlgorithm)
    {
        public override byte[] Encrypt(byte[] plainText)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] paddedPlainText = Padding(plainText);
            byte[] cipherText = new byte[paddedPlainText.Length + blockSize];

            // IV is random bytes. It will append on the head of cipherText.
            byte[] iv = new byte[blockSize];
            new Random().NextBytes(iv);
            Array.Copy(iv, 0, cipherText, 0, blockSize);

            byte[] prevBlock = iv;
            for (int i = 0; i < paddedPlainText.Length; i += blockSize)
            {
                byte[] block = new byte[blockSize];
                Array.Copy(paddedPlainText, i, block, 0, blockSize);

                for (int j = 0; j < blockSize; j++)
                {
                    block[j] ^= prevBlock[j];
                }
                byte[] encryptedBlock = encryptionAlgorithm.Encrypt(block);
                Array.Copy(encryptedBlock, 0, cipherText, i + blockSize, blockSize);

                prevBlock = encryptedBlock;
            }

            return cipherText;
        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] paddedPlainText = new byte[cipherText.Length - blockSize];

            // IV is at the head of cipherText.
            byte[] iv = new byte[blockSize];
            Array.Copy(cipherText, 0, iv, 0, blockSize);

            byte[] prevBlock = iv;
            for (int i = blockSize; i < cipherText.Length; i += blockSize)
            {
                byte[] block = new byte[blockSize];
                Array.Copy(cipherText, i, block, 0, blockSize);

                byte[] decryptedBlock = encryptionAlgorithm.Decrypt(block);
                for (int j = 0; j < blockSize; j++)
                {
                    decryptedBlock[j] ^= prevBlock[j];
                }
                Array.Copy(decryptedBlock, 0, paddedPlainText, i - blockSize, blockSize);

                prevBlock = block;
            }

            return UnPadding(paddedPlainText);
        }

        public byte[] Encrypt(byte[] plainText, byte[] iv)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] paddedPlainText = Padding(plainText);
            byte[] cipherText = new byte[paddedPlainText.Length + blockSize];

            Array.Copy(iv, 0, cipherText, 0, blockSize);

            byte[] prevBlock = iv;
            for (int i = 0; i < paddedPlainText.Length; i += blockSize)
            {
                byte[] block = new byte[blockSize];
                Array.Copy(paddedPlainText, i, block, 0, blockSize);

                for (int j = 0; j < blockSize; j++)
                {
                    block[j] ^= prevBlock[j];
                }
                byte[] encryptedBlock = encryptionAlgorithm.Encrypt(block);
                Array.Copy(encryptedBlock, 0, cipherText, i + blockSize, blockSize);

                prevBlock = encryptedBlock;
            }

            return cipherText;
        }

        public override string ModeName => "CBC";
        public override string AlgorithmName => encryptionAlgorithm.AlgorithmName;
    }
}
