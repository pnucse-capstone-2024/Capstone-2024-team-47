using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.Encryption.PublicKey
{
    public class ECC
    {
        public ECC(BigInteger a, BigInteger b, BigInteger p, BigInteger n, ECPoint G)
        {
            this.a = a;
            this.b = b;
            this.p = p;
            this.n = n;
            this.G = G;
        }

        protected readonly BigInteger a, b, p, n;
        protected readonly ECPoint G;

        public virtual int IsValidPoint(ECPoint point)
        {
            if (point.X < 0 || point.X >= n || point.Y < 0 || point.Y >= n) return 1;
            if (!IsOnCurve(point)) return 2;
            return 0;
        }

        public virtual ECPoint GetPublicKey(BigInteger privKey)
        {
            if (privKey < 0 || privKey >= n)
                throw new ArgumentException("Private key must be in the range [1, n-1]");

            return MultiplyPoint(G, privKey);
        }

        public virtual BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            if (a == 0) throw new DivideByZeroException();

            a %= m;
            if (a < 0) a += m;

            BigInteger m0 = m, t, q;
            BigInteger x0 = 0, x1 = 1;

            while (a > 1)
            {
                q = a / m;
                t = m;
                m = a % m;
                a = t;
                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0) x1 += m0;

            return x1;
        }

        public virtual ECPoint AddPoints(ECPoint p1, ECPoint p2)
        {
            if (p1.IsInfinity()) return p2;
            if (p2.IsInfinity()) return p1;

            CheckPoint(p1);
            CheckPoint(p2);

            BigInteger m;
            if (p1.X == p2.X)
            {
                if (p1.Y != p2.Y)
                    return ECPoint.Infinity;

                // m = (3 * x1^2 + a) / (2 * y1) mod p
                m = (3 * BigInteger.ModPow(p1.X, 2, p) + a) * ModInverse(2 * p1.Y, p);
            }
            else
            {
                // m = (y2 - y1) / (x2 - x1) mod p
                m = (p2.Y - p1.Y) * ModInverse(p2.X - p1.X, p);
            }

            m %= p;
            if (m < 0) m += p;

            // x3 = m^2 - x1 - x2 mod p
            BigInteger x3 = (BigInteger.ModPow(m, 2, p) - p1.X - p2.X) % p;
            if (x3 < 0) x3 += p;

            // y3 = m * (x1 - x3) - y1 mod p
            BigInteger y3 = (m * (p1.X - x3) - p1.Y) % p;
            if (y3 < 0) y3 += p;

            return new ECPoint(x3, y3);
        }

        public virtual ECPoint MultiplyPoint(ECPoint point, BigInteger k)
        {
            if (point.IsInfinity() || k == 0)
                return ECPoint.Infinity;

            CheckPoint(point);

            ECPoint result = ECPoint.Infinity;
            ECPoint addend = point;

            while (k != 0)
            {
                if ((k & 1) == 1)
                    result = AddPoints(result, addend);

                addend = AddPoints(addend, addend);

                k >>= 1;
            }

            return result;
        }

        public virtual BigInteger GenerateRandomBigInteger(BigInteger max)
        {
            BigInteger result;
            byte[] bytes = new byte[max.ToByteArray().Length];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                do
                {
                    rng.GetBytes(bytes);
                    result = new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
                } while (result >= max || result == 0);
            }

            return result;
        }

        public virtual void CheckPoint(ECPoint point)
        {
            if (!IsOnCurve(point))
                throw new ArgumentException("Point is not on curve.");
        }

        public virtual bool IsOnCurve(ECPoint point)
        {
            if (point.IsInfinity()) return true;

            BigInteger lhs = BigInteger.ModPow(point.Y, 2, p);
            BigInteger rhs = (BigInteger.ModPow(point.X, 3, p) + a * point.X + b) % p;

            return lhs == rhs;
        }
    }
}
