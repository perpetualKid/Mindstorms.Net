using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	/// <summary>
	/// Available sensor types.
	/// </summary>
	public enum SensorType
	{
		/// <summary>
		/// No sensor connected to the port.
		/// </summary>
		NoSensor = 0x00,

		/// <summary>
		/// NXT or RCX touch sensor.
		/// </summary>
		Switch = 0x01,

		/// <summary>
		/// RCX temperature sensor.
		/// </summary>
		Temperature = 0x02,

		/// <summary>
		/// RCX light sensor.
		/// </summary>
		Reflection = 0x03,

		/// <summary>
		/// RCX rotation sensor.
		/// </summary>
		Angle = 0x04,

		/// <summary>
		/// NXT light sensor with floodlight enabled.
		/// </summary>
		LightActive = 0x05,

		/// <summary>
		/// NXT light sensor detecting ambient light (floodlight disabled).
		/// </summary>
		LightInactive = 0x06,

		/// <summary>
		/// NXT sound sensor, decibel scaling.
		/// </summary>
		SoundDb = 0x07,

		/// <summary>
		/// NXT sound sensor, dBA scaling (decibel adjusted for the human ear).
		/// </summary>
		SoundDba = 0x08,

		/// <summary>
		/// Custom sensor, unused in NXT programs.
		/// </summary>
		Custom = 0x09,

		/// <summary>
		/// Low speed (I2C digital) sensor.
		/// </summary>
		LowSpeed = 0x0A,

		/// <summary>
		/// Low speed (I2C digital) sensor, 9V power (like the ultrasonic sensor).
		/// </summary>
		LowSpeed9V = 0x0B,

		/// <summary>
		/// None of the specified sensor types. High speed. Unused in NXT programs.
		/// </summary>
		NoneOfSensorTypes = 0x0C,

		#region NXT 2.0 sensor types
		/// <summary>
        /// NXT color sensor in color detector mode.
        /// </summary>
        /// <remarks>
        /// NXT 2.0 only.
        /// </remarks>
        ColorFull = 0x0D,

        /// <summary>
        /// NXT color sensor in light sensor mode with red light.
        /// </summary>
        /// <remarks>
        /// NXT 2.0 only.
        /// </remarks>
        ColorRed = 0x0E,

        /// <summary>
        /// NXT color sensor in light sensor mode with green light.
        /// </summary>
        /// <remarks>
        /// NXT 2.0 only.
        /// </remarks>
        ColorGreen = 0x0F,

        /// <summary>
        /// NXT color sensor in light sensor mode with blue light.
        /// </summary>
        /// <remarks>
        /// NXT 2.0 only.
        /// </remarks>
        ColorBlue = 0x10,

        /// <summary>
        /// NXT color sensor in light sensor mode with no light.
        /// </summary>
        /// <remarks>
        /// NXT 2.0 only.
        /// </remarks>
        ColorNone = 0x11
		#endregion
	}
}
