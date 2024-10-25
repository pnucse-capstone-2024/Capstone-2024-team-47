using Google.Protobuf.Security;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using NetworkCore.Encryption.PublicKey;
using NetworkCore.Encryption.BlockCipher.Algorithm;
using NetworkCore.Encryption.BlockCipher.OperationMode;
using NetworkCore.Buffer;
using System.Diagnostics;
using System.Net.Sockets;

namespace NetworkCore.Packet
{
    public abstract class PacketSession : Session
    {
        protected bool _isSecure = false;

        public bool IsSecure 
        {
            get => _isSecure;
            set => _isSecure = value;
        }

        BigInteger _privKey;
        ECPoint _pubKey;
        CipherSuite _cipherSuite;

        byte[] _sharedSecret    = new byte[32];
        byte[] _salt            = new byte[16];
        byte[] _sessionKey      = new byte[16];
        
        IOperationMode _operationMode;

        public BigInteger PrivKey 
        { 
            get => _privKey; 
            set => _privKey = value; 
        }

        public ECPoint PubKey
        {
            get => _pubKey;
            set => _pubKey = value;
        }

        public CipherSuite CipherSuite
        {
            get => _cipherSuite;
            set => _cipherSuite = value;
        }

        public byte[] Salt
        {
            get => _salt;
            set => _salt = value;
        }

        public byte[] SharedSecret
        {
            get => _sharedSecret;
            set => _sharedSecret = value;
        }

        public byte[] SessionKey
        {
            get => _sessionKey;
            set => _sessionKey = value;
        }

        public IOperationMode OperationMode
        {
            get => _operationMode;
            set => _operationMode = value;
        }

        public static readonly int HeaderSize = 2;

        // NIST P-256 // SHA-256
        protected static BigInteger 
                    a  = HexStringToBigInteger("ffffffff00000001000000000000000000000000fffffffffffffffffffffffc"),
                    b  = HexStringToBigInteger("5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b"),
                    p  = HexStringToBigInteger("ffffffff00000001000000000000000000000000ffffffffffffffffffffffff"),
                    n  = HexStringToBigInteger("ffffffff00000000ffffffffffffffffbce6faada7179e84f3b9cac2fc632551"),
                    gx = HexStringToBigInteger("6b17d1f2e12c4247f8bce6e563a440f277037d812deb33a0f4a13945d898c296"),
                    gy = HexStringToBigInteger("4fe342e2fe1a7f9b8ee7eb4a7c0f9e162bce33576b315ececbb6406837bf51f5");
        
        protected ECDH   _ecdh = new(a, b, p, n, new ECPoint(gx, gy));
        protected ECDSA _ecdsa = new(a, b, p, n, new ECPoint(gx, gy));

        public ECDH ECDH
        {
            get => _ecdh;
        }

        public ECDSA ECDSA
        {
            get => _ecdsa;
        }

        public sealed override int OnRecv(ArraySegment<byte> buffer)
        {
            int processLen = 0;

            if (!_isSecure)
            {
                while (true)
                {
                    if (buffer.Count < HeaderSize)
                        break;

                    ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
                    if (buffer.Count < dataSize)
                        break;

                    OnRecvPacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));

                    processLen += dataSize;
                    buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
                }
            }
            else
            {
                while (true)
                {
                    if (buffer.Count < HeaderSize)
                        break;

                    ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
                    if (buffer.Count < dataSize)
                        break;

                    // Copy EncryptedPacket
                    byte[] encryptedPacket = new byte[dataSize - 4];
                    Array.Copy(buffer.Array, buffer.Offset + 4, encryptedPacket, 0, dataSize - 4);

                    // Packet Decrypt 
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    byte[] decryptedPacket = OperationMode.Decrypt(encryptedPacket);
                    stopwatch.Stop();
                    FileLogger.Instance.Log(new CipherLogObject
                    {
                        IsEncrypt = false,
                        AlgorithmName = OperationMode.AlgorithmName,
                        OperationModeName = OperationMode.ModeName,
                        ElapsedMilliseconds = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000,
                    });

                    // Create ArraySegment (Need Optimization)
                    byte[] tempBuffer = new byte[decryptedPacket.Length + 4];
                    Array.Copy(buffer.Array, buffer.Offset, tempBuffer, 0, 4);
                    Array.Copy(decryptedPacket, 0, tempBuffer, 4, decryptedPacket.Length);
                    OnRecvPacket(new ArraySegment<byte>(tempBuffer, 0, decryptedPacket.Length + 4));

                    processLen += dataSize;
                    buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
                }
            }
            return processLen;
        }

        public abstract void OnRecvPacket(ArraySegment<byte> buffer);

        public void Send(IMessage packet)
        {
            // Get MsgId
            string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
            MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);

            byte[] sendBuffer;
            ushort size;

            if (!_isSecure)
            {
                // Get Packet Size
                size = (ushort)packet.CalculateSize();

                // Set sendBuffer 
                sendBuffer = new byte[size + 4];

                // [Packet Contents]
                Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);                
            }
            else
            {
                // Packet Encrypt
                Stopwatch stopwatch = Stopwatch.StartNew();
                byte[] encryptionPacket = OperationMode.Encrypt(packet.ToByteArray());
                stopwatch.Stop();
                FileLogger.Instance.Log(new CipherLogObject
                {
                    IsEncrypt = true,
                    AlgorithmName = OperationMode.AlgorithmName,
                    OperationModeName = OperationMode.ModeName,
                    ElapsedMilliseconds = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000,
                });
                
                // Set size
                size = (ushort)encryptionPacket.Length;

                // Set sendBuffer
                sendBuffer = new byte[encryptionPacket.Length + 4];

                // [Packet Contents]
                Array.Copy(encryptionPacket, 0, sendBuffer, 4, size);

            }
            // [Size 2 bytes]
            Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));

            // [MsgId 2 Bytes]
            Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
            
            Send(new ArraySegment<byte>(sendBuffer));
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

        protected void StartSessionKeyExchange()
        {
            Logger.DebugLog("SessionKeyExchange");

            C_Hello c_Hello = new C_Hello();

            // Random Choose CipherSuite 
            Array values = Enum.GetValues(typeof(CipherSuite));
            CipherSuite cipherSuite = (CipherSuite)values.GetValue(new Random().Next(0, values.Length));

            // Create PrivKey
            BigInteger privKey = _ecdh.GeneratePrivKey();

            // Calculate PubKey
            ECPoint pubKey = _ecdh.GetPublicKey(privKey);
            byte[] pubKeyX = pubKey.X.ToByteArray(isUnsigned: true, isBigEndian: true);
            byte[] pubKeyY = pubKey.Y.ToByteArray(isUnsigned: true, isBigEndian: true);

            // Set CipherSuite, PrivKey, PubKey
            CipherSuite = cipherSuite;
            PrivKey = privKey;
            PubKey = pubKey;

            // Send Packet
            c_Hello.CipherSuite = cipherSuite;
            c_Hello.PubKeyX = ByteString.CopyFrom(pubKeyX);
            c_Hello.PubKeyY = ByteString.CopyFrom(pubKeyY);
            Send(c_Hello);
        }
    }
}
