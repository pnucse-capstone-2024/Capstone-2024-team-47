using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.Encryption.PublicKey
{
    public class ECDH : ECC
    {
        public ECDH(BigInteger a, BigInteger b, BigInteger p, BigInteger n, ECPoint G) : base(a, b, p, n, G) { }

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
