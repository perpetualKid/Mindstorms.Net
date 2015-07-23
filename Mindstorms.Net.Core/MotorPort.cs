using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
    /// <summary>
    /// Available motor ports.
    /// </summary>
    public enum MotorPort
    {
        /// <summary>
        /// Motor connected to port A.
        /// </summary>
        PortA = 0x00,

        /// <summary>
        /// Motor connected to port B.
        /// </summary>
        PortB = 0x01,

        /// <summary>
        /// Motor connected to port C.
        /// </summary>
        PortC = 0x2,

        /// <summary>
        /// All motors connected to A, B and C ports.
        /// </summary>
        All = 0xFF
    }
}
