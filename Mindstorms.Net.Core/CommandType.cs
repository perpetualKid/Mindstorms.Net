using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
    /// <summary>
    /// The type of command being sent to the brick
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// Direct command with a reply expected
        /// </summary>
        DirectCommandResponseRequired = 0x00,

        /// <summary>
        ///  System command with a reply expected
        /// </summary>
        SystemCommandResponseRequired = 0x01,

        /// <summary>
        /// Response only
        /// </summary>
        ReplyCommand = 0x02,

        /// <summary>
        /// Direct command with no reply
        /// </summary>
        DirectCommandNoResponse = 0x80,

        /// <summary>
        /// System command with no reply
        /// </summary>
        SystemCommandNoResponse = 0x81

    }
}
