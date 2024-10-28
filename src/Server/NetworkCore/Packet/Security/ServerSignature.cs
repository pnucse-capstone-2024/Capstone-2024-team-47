using NetworkCore.Encryption.PublicKey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetworkCore.Packet.Security
{
    public class ServerSignature
    {
        static ServerSignature _instance = new();
        public static ServerSignature Instance { get { return _instance; } }

        byte[] _message;
        byte[] _signature;

        public byte[] Message
        {
            get { return _message; }
        }

        public byte[] Signature
        {
            get { return _signature; }
        }

        // SigGenTest.txt의 line 97 ~ 103 테스트 값을 그대로 사용함.
        ServerSignature()
        {
            _message = HexStringToByteArray("c5204b81ec0a4df5b7e9fda3dc245f98082ae7f4efe81998dcaa286bd4507ca840a53d21b01e904f55e38f78c3757d5a5a4a44b1d5d4e480be3afb5b394a5d2840af42b1b4083d40afbfe22d702f370d32dbfd392e128ea4724d66a3701da41ae2f03bb4d91bb946c7969404cb544f71eb7a49eb4c4ec55799bda1eb545143a7");
            _signature = HexStringToByteArray("e67a9717ccf96841489d6541f4f6adb12d17b59a6bef847b6183b8fcf16a32eb9ae6ba6d637706849a6a9fc388cf0232d85c26ea0d1fe7437adb48de58364333");
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

        static BigInteger HexStringToBigInteger(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
        }


    }
}
