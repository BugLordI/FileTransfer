using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Core
{
    internal class Message
    {
        public const String PONG = "PONG";
        public const String PING = "PING";

        /// <summary>
        /// 0 byte
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 1-4 byte
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 5- tail byte
        /// </summary>
        public String? Content { get; set; }
    }
}
