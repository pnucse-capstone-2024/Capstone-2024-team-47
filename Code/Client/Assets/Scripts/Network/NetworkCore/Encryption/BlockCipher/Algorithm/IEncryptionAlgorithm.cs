﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.Encryption.BlockCipher.Algorithm
{
    public interface IEncryptionAlgorithm
    {
        byte[] Encrypt(byte[] plainText);
        byte[] Decrypt(byte[] cipherText);
        string AlgorithmName { get; }
        int GetBlockSize();
    }
}