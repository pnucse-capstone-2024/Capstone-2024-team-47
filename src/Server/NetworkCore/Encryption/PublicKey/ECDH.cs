using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.Encryption.PublicKey
{
    // privKeyA = GeneratePrivKey(n);
    // privKeyB = GeneratePrivKey(n);

    // pubKeyA = GetPublicKey(privKeyA);
    // pubKeyB = GetPublicKey(privKeyB);

    // sharedSecret = ComputeSharedSecret(privKeyA, pubKeyB)
    // sharedSecret = ComputeSharedSecret(privKeyB, pubKeyA)

    public class ECDH(BigInteger a, BigInteger b, BigInteger p, BigInteger n, ECPoint G) : ECC(a, b, p, n, G)
    {
        public BigInteger GeneratePrivKey()
        {
            return GenerateRandomBigInteger(n);
        }

        public BigInteger ComputeSharedSecret(BigInteger privKey, ECPoint pubKey)
        {
            ECPoint sharedSecret = MultiplyPoint(pubKey, privKey);

            return sharedSecret.X;
        }

        public static string AlgorithmName => "ECDH";
    }
}
