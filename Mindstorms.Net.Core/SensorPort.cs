using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	/// <summary>
	/// Available sensor ports.
	/// </summary>
	public enum SensorPort
	{
		/// <summary>
		/// First sensor port, default port for the touch sensor.
		/// </summary>
		Port1 = 0x00,

		/// <summary>
		/// Second sensor port, default port for the sound sensor.
		/// </summary>
		Port2 = 0x01,

		/// <summary>
		/// Third sensor port, default port for the light sensor.
		/// </summary>
		Port3 = 0x02,

		/// <summary>
		/// Fourth sensor port, default port for the ultrasonic sensor.
		/// </summary>
		Port4 = 0x03,

		/// <summary>
		/// None of the available sensor ports.
		/// </summary>
		None = 0xFE
	}
}
