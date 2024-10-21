using NetworkCore.Encryption.PublicKey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.Packet.Security
{
    public class CertificateAuthority
    {
        static CertificateAuthority _instance = new();
        public static CertificateAuthority Instance { get { return _instance; } }

        ECPoint _serverSignaturePubKey;

        public ECPoint ServerSignaturePubKey
        {
            get { return _serverSignaturePubKey; }
        }
         
        CertificateAuthority()
        {
            _serverSignaturePubKey = new ECPoint(
                    HexStringToBigInteger("68229b48c2fe19d3db034e4c15077eb7471a66031f28a980821873915298ba76"), 
                    HexStringToBigInteger("303e8ee3742a893f78b810991da697083dd8f11128c47651c27a56740a80c24c")
                );
        }

        static BigInteger HexStringToBigInteger(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
        }

        static byte[] HexStringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}
