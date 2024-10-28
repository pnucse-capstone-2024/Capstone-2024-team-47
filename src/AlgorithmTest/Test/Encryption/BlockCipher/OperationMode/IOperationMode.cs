using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Encryption.BlockCipher.OperationMode
{
    public interface IOperationMode
    {
        byte[] Encrypt(byte[] plainText);
        byte[] Decrypt(byte[] cipherText);
        string ModeName { get; }
        string AlgorithmName { get; }
    }
}
