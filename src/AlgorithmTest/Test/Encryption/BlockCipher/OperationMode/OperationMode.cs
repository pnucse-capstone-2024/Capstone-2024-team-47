using Test.Encryption.BlockCipher.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Encryption.BlockCipher.OperationMode
{
    public abstract class OperationMode(IEncryptionAlgorithm encryptionAlgorithm) : IOperationMode
    {
        public abstract byte[] Encrypt(byte[] plainText);
        public abstract byte[] Decrypt(byte[] cipherText);
        protected byte[] Padding(byte[] input)
        {
            int blockSize = encryptionAlgorithm.GetBlockSize();
            int paddingSize = blockSize - (input.Length % blockSize);
            byte[] output = new byte[input.Length + paddingSize];
            Array.Copy(input, output, input.Length);
            for (int i = input.Length; i < output.Length; i++)
            {
                output[i] = (byte)paddingSize;
            }

            return output;
        }
        protected byte[] UnPadding(byte[] input)
        {
            int paddingSize = input[input.Length - 1];
            for (int i = input.Length - paddingSize; i < input.Length; i++)
            {
                if (input[i] != paddingSize)
                    throw new ArgumentException("Invalid padding.");
            }
            byte[] output = new byte[input.Length - paddingSize];
            Array.Copy(input, output, output.Length);

            return output;
        }
        public abstract string ModeName { get; }
        public abstract string AlgorithmName { get; }

        protected readonly IEncryptionAlgorithm encryptionAlgorithm = encryptionAlgorithm;
    }
}
