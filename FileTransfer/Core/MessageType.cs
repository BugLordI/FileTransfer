using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Core
{
    internal enum MessageType
    {
        PING = 1,
        PONG = 2,
        FILE = 3,
    }
}
