using Test.Encryption.BlockCipher.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Encryption.BlockCipher.OperationMode
{
    public class ECB(IEncryptionAlgorithm encryptionAlgorithm) : OperationMode(encryptionAlgorithm)
    {
        public override byte[] Encrypt(byte[] plainText)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] paddedPlainText = Padding(plainText);
            byte[] cipherText = new byte[paddedPlainText.Length];

            for (int i = 0; i < paddedPlainText.Length; i += blockSize)
            {
                byte[] block = new byte[blockSize];
                Array.Copy(paddedPlainText, i, block, 0, blockSize);
                byte[] encryptedBlock = encryptionAlgorithm.Encrypt(block);
                Array.Copy(encryptedBlock, 0, cipherText, i, blockSize);
            }

            return cipherText;
        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            byte[] paddedPlainText = new byte[cipherText.Length];

            for (int i = 0; i < cipherText.Length; i += blockSize)
            {
                byte[] block = new byte[blockSize];
                Array.Copy(cipherText, i, block, 0, blockSize);
                byte[] decryptedBlock = encryptionAlgorithm.Decrypt(block);
                Array.Copy(decryptedBlock, 0, paddedPlainText, i, blockSize);
            }

            return UnPadding(paddedPlainText);
        }

        public override string ModeName => "ECB";
        public override string AlgorithmName => encryptionAlgorithm.AlgorithmName;
    }
}
