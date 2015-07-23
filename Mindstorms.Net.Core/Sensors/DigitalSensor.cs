using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINRT
using Windows.Foundation;
#endif

namespace Mindstorms.Net.Core.Sensors
{
	public enum DigitalSensorCommand
	{
		ReadVersion = 0x00,
		ReadProductID = 0x08,
		ReadSensorType = 0x10,
		ReadFactoryZero = 0x11,
		ReadFactoryScaleFactor = 0x12,
		ReadFactoryScaleDivisor = 0x13,
		ReadMeasurementUnits = 0x14,
	}

	public class DigitalSensor: SensorBase
	{
		protected readonly byte deviceAddress;

		internal const int CommandDelay = 10;

		internal DigitalSensor(byte deviceAddress) :
			base(SensorType.LowSpeed9V, SensorMode.Raw)
		{
			this.deviceAddress = deviceAddress;
		}

		#region ReadVersion
#if WINRT
		public IAsyncOperation<string>
#else
		public Task<string>
#endif
		ReadVersionAsync()
		{
			return ReadVersionAsyncInternal()
#if WINRT
			.AsAsyncOperation<string>()
#endif
			;
		}

		internal async Task<string> ReadVersionAsyncInternal()
	{
			LowSpeedReadResponse result = await SensorReadCommand((byte)DigitalSensorCommand.ReadVersion, 0x08);
			return result != null ? result.AsString : string.Empty;
	}
		#endregion

		#region ReadSensorType
#if WINRT
		public IAsyncOperation<string>
#else
		public Task<string>
#endif
		ReadSensorTypeAsync()
		{
			return ReadSensorTypeAsyncInternal()
#if WINRT
			.AsAsyncOperation<string>()
#endif
			;
		}

		internal async Task<string> ReadSensorTypeAsyncInternal()
		{
			LowSpeedReadResponse result = await SensorReadCommand((byte)DigitalSensorCommand.ReadSensorType, 0x08);
			return result != null ? result.AsString : string.Empty;
		}
		#endregion

		#region ReadProductID
#if WINRT
		public IAsyncOperation<string>
#else
		public Task<string>
#endif
		ReadProductIDAsync()
		{
			return ReadProductIDAsyncInternal()
#if WINRT
			.AsAsyncOperation<string>()
#endif
			;
		}

		internal async Task<string> ReadProductIDAsyncInternal()
		{
			LowSpeedReadResponse result = await SensorReadCommand((byte)DigitalSensorCommand.ReadProductID, 0x08);
			return result != null ? result.AsString : string.Empty;
		}
		#endregion

		#region ReadFactoryZero
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		ReadFactoryZeroAsync()
		{
			return ReadFactoryZeroAsyncInternal()
#if WINRT
			.AsAsyncOperation<byte>()
#endif
			;
		}

		internal async Task<byte> ReadFactoryZeroAsyncInternal()
		{
			LowSpeedReadResponse result = await SensorReadCommand((byte)DigitalSensorCommand.ReadFactoryZero, 0x01);
			return result != null ? result.AsBytes[0] : byte.MinValue;
		}
		#endregion

		#region ReadFactoryScaleFactor
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		ReadFactoryScaleFactorAsync()
		{
			return ReadFactoryScaleFactorAsyncInternal()
#if WINRT
			.AsAsyncOperation<byte>()
#endif
			;
		}

		internal async Task<byte> ReadFactoryScaleFactorAsyncInternal()
		{
			LowSpeedReadResponse result = await SensorReadCommand((byte)DigitalSensorCommand.ReadFactoryScaleFactor, 0x01);
			return result != null ? result.AsBytes[0] : byte.MinValue;
		}
		#endregion

		#region ReadFactoryScaleDivisor
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		ReadFactoryScaleDivisorAsync()
		{
			return ReadFactoryScaleDivisorAsyncInternal()
#if WINRT
			.AsAsyncOperation<byte>()
#endif
			;
		}

		internal async Task<byte> ReadFactoryScaleDivisorAsyncInternal()
		{
			LowSpeedReadResponse result = await SensorReadCommand((byte)DigitalSensorCommand.ReadFactoryScaleDivisor, 0x01);
			return result != null ? result.AsBytes[0] : byte.MinValue;
		}
		#endregion

		#region ReadMeasurementUnits
#if WINRT
		public IAsyncOperation<string>
#else
		public Task<string>
#endif
		ReadMeasurementUnitsAsync()
		{
			return ReadMeasurementUnitsAsyncInternal()
#if WINRT
			.AsAsyncOperation<string>()
#endif
			;
		}

		internal async Task<string> ReadMeasurementUnitsAsyncInternal()
		{
			LowSpeedReadResponse result = await SensorReadCommand((byte)DigitalSensorCommand.ReadMeasurementUnits, 0x07);
			return result != null ? result.AsString : string.Empty;
		}
		#endregion

		internal override async Task InitializeAsyncInternal()
		{
			if (brick.Connected)
			{
				await base.InitializeAsyncInternal();
				//cleanout potentially leftover garbaga from read buffer
				await brick.DirectCommands.LowSpeedGetStatusAsync(SensorPort);
				byte leftover = await brick.DirectCommands.LowSpeedGetStatusAsync(SensorPort);
				if (leftover > 0)
					await brick.DirectCommands.LowSpeedReadInternalAsync(this.SensorPort);
			}
		}

		protected async Task<LowSpeedReadResponse> SensorReadCommand(byte command, byte responseLength)
		{
			int i = 0;
			if (brick.Connected)
			{
				byte[] buffer = new byte[] { deviceAddress, command };
				await brick.DirectCommands.LowSpeedWriteAsyncInternal(SensorPort, buffer, responseLength);
				if (responseLength != 0)
				{
					await Task.Delay(CommandDelay);
					while (await brick.DirectCommands.LowSpeedGetStatusAsync(SensorPort) != responseLength && i++ < 3)
					{
						await Task.Delay(CommandDelay);
					}
					return await brick.DirectCommands.LowSpeedReadInternalAsync(this.SensorPort);
				}
			}
			return null;
		}

		protected async Task SensorWriteCommand(byte command, byte value)
		{
			if (brick.Connected)
			{
				byte[] buffer = new byte[] { deviceAddress, command, value};
				await brick.DirectCommands.LowSpeedWriteAsyncInternal(SensorPort, buffer, 0x00);
			}
		}

	}
}
