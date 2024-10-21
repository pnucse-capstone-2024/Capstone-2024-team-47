using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class HKDF
    {
        static byte[] HMAC_Hash(byte[] key, byte[] message)
        {
            SHA256 sha256 = SHA256.Create();
            // Hash함수에서 사용하는 블럭 사이즈 (SHA256 기준 512비트)
            int blockSize = 64;
            // Hash함수 결과 길이 (SHA256기준 256비트)
            int outputLength = 32;

            byte[] tempKey = new byte[blockSize];
            if (key.Length > blockSize)
            {
                // Key길이가 블럭 사이즈보다 길 경우 해시를 한번 함으로써 짧게함
                key = sha256.ComputeHash(key);
                Array.Copy(key, 0, tempKey, 0, outputLength);
                key = tempKey;
            }
            else if (key.Length < blockSize)
            {
                // Key 길이가 블럭 사이즈보다 길 경우 블럭사이즈까지 0으로 패딩함
                Array.Copy(key, 0, tempKey, 0, key.Length);
                key = tempKey;
            }

            byte[] o_key_pad = new byte[blockSize];
            byte[] i_key_pad = new byte[blockSize];

            for (int i = 0; i < blockSize; i++)
            {
                // o_key_pad = key xor [0x5c * blockSize]
                // i_key_pad = key xor [0x36 * blockSize]
                o_key_pad[i] = (byte)(key[i] ^ 0x5c);
                i_key_pad[i] = (byte)(key[i] ^ 0x36);
            }

            // ret = hash(o_key_pad || hash(i_key_pad || message))
            // concat1 = hash(i_key_pad || message)
            byte[] concat1 = new byte[blockSize + message.Length];
            Array.Copy(i_key_pad, 0, concat1, 0, blockSize);
            Array.Copy(message, 0, concat1, blockSize, message.Length);
            concat1 = sha256.ComputeHash(concat1);

            // concat2 = hash(o_key_pad || concat1)
            byte[] concat2 = new byte[blockSize + outputLength];
            Array.Copy(o_key_pad, 0, concat2, 0, blockSize);
            Array.Copy(concat1, 0, concat2, blockSize, outputLength);
            concat2 = sha256.ComputeHash(concat2);

            // ret = hash(concat2 || concat1)
            return concat2;
        }
        public static byte[] KeyExtract(byte[]? salt, byte[] IKM)
        {
            // PRK = HMAC-Hash(salt, IKM)
            if (salt == null) salt = new byte[32];
            return HMAC_Hash(salt, IKM);
        }
        public static byte[] KeyExpand(byte[] PRK, byte[]? info, int L)
        {
            // Hash함수 결과 길이 (SHA256기준 256비트)
            int outputLength = 32;
            // N = ceil(L/HashLen)
            int N = L / outputLength;

            // T = T(1) || T(2) || T(3) || ... || T(N)
            byte[] concat = new byte[(N + 1) * outputLength];

            if (info == null)
            {
                byte[] prev_T = new byte[outputLength];
                for (int i = 0; i <= N; i++)
                {
                    if (i == 0)
                    {
                        // T(0) = empty string (zero length)
                        // T(1) = HMAC-Hash(PRK, 0x01)
                        byte[] message = [0x01];
                        prev_T = HMAC_Hash(PRK, message);
                        Array.Copy(prev_T, 0, concat, 0, outputLength);
                    }
                    else
                    {
                        // T(2) = HMAC-Hash(PRK, T(1) || 0x02)
                        // T(3) = HMAC-Hash(PRK, T(2) || 0x03)
                        // ...
                        byte[] message = new byte[outputLength + 1];
                        Array.Copy(prev_T, 0, message, 0, outputLength);
                        message[outputLength] = (byte)(i + 1);
                        prev_T = HMAC_Hash(PRK, message);
                        Array.Copy(prev_T, 0, concat, N * i, outputLength);
                    }
                }
            }
            else
            {
                byte[] prev_T = new byte[outputLength];
                for (int i = 0; i <= N; i++)
                {
                    if (i == 0)
                    {
                        // T(0) = empty string (zero length)
                        // T(1) = HMAC-Hash(PRK, info || 0x01)
                        byte[] message = new byte[info.Length + 1];
                        Array.Copy(info, 0, message, 0, info.Length);
                        message[info.Length] = 0x01;
                        prev_T = HMAC_Hash(PRK, message);
                        Array.Copy(prev_T, 0, concat, 0, outputLength);
                    }
                    else
                    {
                        // T(2) = HMAC-Hash(PRK, T(1) || info || 0x02)
                        // T(3) = HMAC-Hash(PRK, T(2) || info || 0x03)
                        byte[] message = new byte[outputLength + info.Length + 1];
                        Array.Copy(prev_T, 0, message, 0, outputLength);
                        Array.Copy(info, 0, message, outputLength, info.Length);
                        message[outputLength + info.Length] = (byte)(i + 1);
                        prev_T = HMAC_Hash(PRK, message);
                        Array.Copy(prev_T, 0, concat, outputLength * i, outputLength);
                    }
                }
            }

            // OKM = first L octets of T
            byte[] OKM = new byte[L];
            Array.Copy(concat, 0, OKM, 0, L);
            return OKM;
        }
        public static byte[] KeyDerivation(byte[] sharedSecret, byte[] salt, byte[]? info, int L)
        {
            byte[] temp = KeyExtract(salt, sharedSecret);
            byte[] ret = KeyExpand(temp, info, L);
            return ret;
        }
    }
}
