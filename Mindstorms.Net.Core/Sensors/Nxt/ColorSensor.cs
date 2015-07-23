using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINRT
using Windows.Foundation;
#endif

namespace Mindstorms.Net.Core.Sensors.Nxt
{
	public sealed class ColorSensor : SensorBase
	{
		private GetInputValuesResponse state;
		private ColorSensorMode colorSensorMode;
		private SortedSet<SensorColor> colorMatch;
		private short intensityChangeThreshold = 10;
		private short lastIntensityEvent;

		public ColorSensor() :
			base(SensorType.ColorFull, SensorMode.Raw)
		{
			this.colorSensorMode = ColorSensorMode.ColorDetector;
			colorMatch = new SortedSet<SensorColor>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="color">The floodlight color. The sensor will accept red, green and blue, or black and null for no floodlights.</param>
		/// <returns></returns>
		public
#if WINRT
		IAsyncAction
#else
		Task 
#endif
		SetSensorModeAsync(ColorSensorMode mode, SensorColor color)
		{
			return this.SetSensorModeAsyncInternal(mode, color)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async Task SetSensorModeAsyncInternal(ColorSensorMode mode, SensorColor color = SensorColor.Black)
		{
			switch (mode)
			{
				case ColorSensorMode.ColorDetector:
				case ColorSensorMode.Lamp:
					this.sensorType = SensorType.ColorFull;
					break;
				case ColorSensorMode.LightSensor:
					switch (color)
					{
						case SensorColor.Red:
							this.sensorType = SensorType.ColorRed;
							break;
						case SensorColor.Green:
							this.sensorType = SensorType.ColorGreen;
							break;
						case SensorColor.Blue:
							this.sensorType = SensorType.ColorBlue;
							break;
						default:
							this.sensorType = SensorType.ColorNone;
							break;
					}
					break;
			}
			this.colorSensorMode = mode;
			await base.Initialize();
		}

		/// <summary>
		/// Returns the color when the sensor is in color detector mode.
		/// </summary>
		public SensorColor Color
		{
			get { return (state != null && colorSensorMode == ColorSensorMode.ColorDetector) ? (SensorColor)state.ScaledValue : SensorColor.None; }
		}

		/// <summary>
		/// Returns the light intensity when the sensor is in light detector mode.
		/// </summary>
		public short Intensity
		{
			get { return (state != null && colorSensorMode == ColorSensorMode.LightSensor) ? state.ScaledValue : (short)0; }
		}

		public event EventHandler<ColorChangedEventArgs> OnColorDetected;

		private void OnColorDetectedEventHandler(SensorColor color)
		{
			if (null != OnColorDetected)
				Task.Run(() => OnColorDetected(this, new ColorChangedEventArgs(color)));
		}

		public
#if WINRT
		IAsyncAction
#else
		Task 
#endif
 SetMatchingColorsAsync(IEnumerable<SensorColor> colors)
		{
			return this.SetMatchingColorsAsyncInternal(colors)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async Task SetMatchingColorsAsyncInternal(IEnumerable<SensorColor> colors)
		{
			await Task.Run(() =>
				{
					colorMatch.Clear();
					foreach (SensorColor color in colors)
					{
						if (!colorMatch.Contains(color))
							colorMatch.Add(color);
					}
				});
		}

		public short IntensityChangeThreshold { get { return this.intensityChangeThreshold; } set { this.intensityChangeThreshold = value; } }

		internal override async Task PollAsyncInternal()
		{
			if (brick.Connected)
			{
				GetInputValuesResponse previous = state;
				this.state = await brick.DirectCommands.GetInputValuesAsyncInternal(SensorPort);
				if (null != state)
				{
					switch (colorSensorMode)
					{
						case ColorSensorMode.Lamp:
							break;
						case ColorSensorMode.ColorDetector:
							if (previous != null && state.ScaledValue != previous.ScaledValue)
							{
								OnChangedEventHandler();
							}
							if (colorMatch.Contains((SensorColor)state.ScaledValue))
								OnColorDetectedEventHandler((SensorColor)state.ScaledValue);
							break;
						case ColorSensorMode.LightSensor:
							if (previous != null && (Math.Abs(state.ScaledValue - lastIntensityEvent) > intensityChangeThreshold))
							{
								lastIntensityEvent = state.ScaledValue;
								OnChangedEventHandler();
							}
							break;
					}
				}
			}
		}
	}
}
