using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.Encryption.PublicKey
{
    public class ECPoint(BigInteger x, BigInteger y)
    {
        public BigInteger X { get; } = x;
        public BigInteger Y { get; } = y;

        public bool IsInfinity() { return X == 0 && Y == 0; }

        public static readonly ECPoint Infinity = new(0, 0);
    }
}
