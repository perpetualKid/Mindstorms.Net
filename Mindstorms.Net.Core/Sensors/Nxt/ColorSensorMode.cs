using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core.Sensors.Nxt
{
	/// <summary>
	/// Sensor-modes for the NXT 2.0 color sensor.
	/// </summary>
	public enum ColorSensorMode
	{
		/// <summary>
		/// Color detector mode with no light.
		/// </summary>
		ColorDetector,

		/// <summary>
		/// Light sensor mode with or without light.
		/// </summary>
		LightSensor,

		/// <summary>
		/// Lamp only, no values polled or events fired
		/// </summary>
		Lamp,
	}

}
