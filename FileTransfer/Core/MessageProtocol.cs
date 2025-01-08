using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Core
{
    internal class MessageProtocol
    {
        public static int GetMessageType(byte[] buffer)
        {
            byte[] btype = { buffer[0], 0, 0, 0 };
            int type = BitConverter.ToInt32(btype);
            return type;
        }

        public static int GetMessageLength(byte[] buffer)
        {
            byte[] bLength = { buffer[1], buffer[2], buffer[3], buffer[4] };
            int length = BitConverter.ToInt32(bLength);
            return length;
        }

        public static String GetFileName(byte[] buffer)
        {
            byte[] bName = { buffer[5], buffer[6], buffer[7], buffer[8], buffer[9], buffer[10], buffer[11], buffer[12] };
            String fileName = Encoding.UTF8.GetString(bName);
            return fileName;
        }
    }
}
