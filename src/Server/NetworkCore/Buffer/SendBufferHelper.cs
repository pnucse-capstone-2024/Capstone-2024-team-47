using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.Buffer
{
    public class SendBufferHelper
    {
        public static ThreadLocal<SendBuffer> CurrentBuffer = new(() => { return null; });

        public static int ChunkSize { get; set; } = 65535;

        public static ArraySegment<byte> Open(int reserveSize)
        {
            if (CurrentBuffer.Value == null || CurrentBuffer.Value.FreeSize < reserveSize)
            {
                CurrentBuffer.Value = new SendBuffer(ChunkSize);
            }

            return CurrentBuffer.Value.Open(reserveSize);
        }

        public static ArraySegment<byte> Close(int usedSize)
        {
            return CurrentBuffer.Value.Close(usedSize);
        }
    }
}
