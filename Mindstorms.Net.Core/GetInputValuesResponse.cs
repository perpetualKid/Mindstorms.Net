using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	/// <summary>
	/// Structure that describes the current state and value of a sensor.
	/// </summary>
	public sealed class GetInputValuesResponse: ResponseBase
	{

		internal GetInputValuesResponse(ResponseTelegram response)
			: base(response)
		{
		}

		public SensorPort SensorPort { get { return (SensorPort)response.Data[3]; } }

		/// <summary>
		/// <c>True</c>, if new data value can be seen as valid data.
		/// </summary>
		public bool IsValid { get { return BitConverter.ToBoolean(response.Data, 4); } }

		/// <summary>
		/// <c>True</c>, if calibration file found and used for <see cref="CalibratedValue"/> field.
		/// </summary>
		public bool IsCalibrated { get { return BitConverter.ToBoolean(response.Data, 5); } }

		/// <summary>
		/// The type of the sensor.
		/// </summary>
		public SensorType SensorType { get { return (SensorType)response.Data[6]; } }

		/// <summary>
		/// The mode in which the sensor currently operates.
		/// </summary>
		public SensorMode SensorMode { get { return (SensorMode)response.Data[7]; } }

		/// <summary>
		/// Raw A/D value of the sensor. (UWORD, device dependent)
		/// </summary>
		public ushort RawValue { get { return BitConverter.ToUInt16(response.Data, 8); } }

		/// <summary>
		/// Normalized A/D value of the sensor. (UWORD, type dependent, Range: 0-1023)
		/// </summary>
		public ushort NormalizedValue { get { return BitConverter.ToUInt16(response.Data, 10); } }

		/// <summary>
		/// Scaled value. (SWORD, mode dependent)
		/// The sensor mode affects the scaled value, which the NXT firmware calculates 
		/// depending on the sensor type and sensor mode.
		/// </summary>
		/// <remarks>
		/// The legal value range depends on <see cref="SensorMode"/>, as listed below:
		/// Raw: [0, 1023]
		/// Boolean: [0, 1]
		/// TransitionCount: [0, 65535]
		/// PeriodCounter: [0, 65535]
		/// FullScale: [0, 100]
		/// Celsius: [-200, 700] (readings in 10th of a degree Celsius)
		/// Fahrenheit: [-400, 1580] (readings in 10th of a degree Fahrenheit)
		/// AngleStep: [0, 65535]
		/// </remarks>
		public short ScaledValue { get { return BitConverter.ToInt16(response.Data, 12); } }

		/// <summary>
		/// Value scaled according to calibration (SWORD, currently unused by the NXT)
		/// </summary>
		public short CalibratedValue { get { return BitConverter.ToInt16(response.Data, 14); } }

		public bool AsBoolean { get { return (SensorMode == Core.SensorMode.Boolean ? ScaledValue != 0 : false); } }


		/// <summary>
		/// Returns the complete sensor state in string format.
		/// </summary>
		/// <returns>All details of the current state of the sensor in string format.</returns>
		public override string ToString()
		{
			return string.Format("Sensor: {0}\r\nIsValid: {1}%\r\nIsCalibrated: {2}\r\nSensorType: {3}\r\n" +
				"SensorMode: {4}\r\nRawValue: {5}\r\nNormalizedValue: {6}\r\nScaledValue: {7}\r\nCalibratedValue: {8}\r\n",
				this.SensorPort, this.IsValid, this.IsCalibrated, this.SensorType, this.SensorMode, this.RawValue,
				this.NormalizedValue, this.ScaledValue, this.CalibratedValue);
		}

	}
}
