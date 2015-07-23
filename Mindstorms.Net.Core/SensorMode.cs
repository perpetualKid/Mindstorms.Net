using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	/// <summary>
	/// Available sensor modes. The sensor mode affects the scaled value, 
	/// which the NXT firmware calculates depending on the sensor type and sensor mode.
	/// </summary>
	public enum SensorMode
	{
		/// <summary>
		/// Reports scaled value equal to raw value.
		/// </summary>
		Raw = 0x0,

		/// <summary>
		/// Report scaled value as 1 (true) or 0 (false). 
		/// Readings are FALSE if raw value exceeds 55% of total range; 
		/// readings are TRUE if raw value is less than 45% of total range.
		/// </summary>
		/// <remarks>
		/// The firmware uses inverse Boolean logic to match the physical characteristics of NXT sensors.
		/// </remarks>
		Boolean = 0x20,

		/// <summary>
		/// Report scaled value as number of transitions between true and false.
		/// </summary>
		TransitionCount = 0x40,

		/// <summary>
		/// Report scaled value as number of transitions from false to true, then back to false.
		/// </summary>
		PeriodCounter = 0x60,

		/// <summary>
		/// Report scaled value as % of full scale reading for configured sensor type.
		/// </summary>
		PercentFullScale = 0x80,

		/// <summary>
		/// Scale TEMPERATURE reading to degrees Celsius.
		/// </summary>
		Celsius = 0xA0,

		/// <summary>
		/// Scale TEMPERATURE reading to degrees Fahrenheit.
		/// </summary>
		Fahrenheit = 0xC0,

		/// <summary>
		/// Report scaled value as count of ticks on RCX-style rotation sensor.
		/// </summary>
		AngleStep = 0xE0,

		/// <summary>
		/// TODO
		/// </summary>
		SlopeMask = 0x1F,

		/// <summary>
		/// TODO
		/// </summary>
		ModeMask = 0xE0
	}
}
