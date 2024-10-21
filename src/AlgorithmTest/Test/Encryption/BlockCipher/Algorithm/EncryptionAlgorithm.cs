using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Encryption.BlockCipher.Algorithm
{
    public abstract class EncryptionAlgorithm : IEncryptionAlgorithm
    {
        protected EncryptionAlgorithm(byte[] key)
        {
            if (key.Length != 16)
                throw new ArgumentException("Key must be exactly 16 bytes long.");

            this.key = new byte[16];
            Array.Copy(key, this.key, 16);
        }
        public abstract byte[] Encrypt(byte[] plainText);
        public abstract byte[] Decrypt(byte[] cipherText);
        public abstract string AlgorithmName { get; }
        public abstract int GetBlockSize();

        protected readonly byte[] key;
    }
}
