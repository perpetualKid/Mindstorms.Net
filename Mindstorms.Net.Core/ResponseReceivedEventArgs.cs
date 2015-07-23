using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
    public sealed class ResponseReceivedEventArgs
#if !WINRT
    : EventArgs
#endif
    {
        /// <summary>
        /// Byte array of the data received from the NXT brick.
        /// </summary>
        public ResponseTelegram ResponseTelegram { get; set; }
    }
}
