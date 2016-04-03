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
	public enum NxtUltraSonicSensorCommands
	{
		//Variables
		ReadContinuousMeasurementInterval = 0x40,
		ReadCommandState = 0x41,
		ReadMeasurementByte0 = 0x42,
		ReadMeasurementByte1 = 0x43,
		ReadMeasurementByte2 = 0x44,
		ReadMeasurementByte3 = 0x45,
		ReadMeasurementByte4 = 0x46,
		ReadMeasurementByte5 = 0x47,
		ReadMeasurementByte6 = 0x48,
		ReadMeasurementByte7 = 0x49,
		ReadActualZero = 0x50,
		ReadActualScaleFactor = 0x51,
		ReadActualScaleDivisor = 0x52,
		//Commands
		OffCommand = 0x41,
		SingleShotCommand = 0x41,
		ContinuousMeasurementCommand = 0x41,
		EventCaptureCommand = 0x41,
		RequestWarmReset = 0x41,
		SetContinuousMeasurementInterval = 0x40,
		SetActualZero = 0x50,
		SetActualScaleFactor = 0x51,
		SetActualScaleDivisor = 0x52,
	}

	public sealed class UltraSonicSensor: DigitalSensor
	{
		private byte[] distances;
		private byte distance;

		public UltraSonicSensor() :
			base(0x02)
		{
		}

		#region ReadContinuousMeasurementInterval
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		ReadContinuousMeasurementIntervalAync()
		{
			return ReadContinuousMeasurementIntervalAsyncInternal()
#if WINRT
			.AsAsyncOperation<byte>()
#endif
			;
		}

		internal async Task<byte> ReadContinuousMeasurementIntervalAsyncInternal()
		{
			LowSpeedReadResponse result = await SensorReadCommand((byte)NxtUltraSonicSensorCommands.ReadContinuousMeasurementInterval, 0x01);
			return result != null ? result.AsBytes[0] : byte.MinValue;
		}
		#endregion

		#region ReadCommandState
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		ReadCommandStateAsync()
		{
			return ReadCommandStateAsyncInternal()
#if WINRT
			.AsAsyncOperation<byte>()
#endif
			;
		}

		internal async Task<byte> ReadCommandStateAsyncInternal()
		{
			LowSpeedReadResponse result = await SensorReadCommand((byte)NxtUltraSonicSensorCommands.ReadCommandState, 0x01);
			return result != null ? result.AsBytes[0] : byte.MinValue;
		}
		#endregion

		#region ReadMeasurementByte
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		ReadMeasurementByteAsync(byte position)
		{
			return ReadMeasurementByteAsyncInternal(position)
#if WINRT
			.AsAsyncOperation<byte>()
#endif
			;
		}

		internal async Task<byte> ReadMeasurementByteAsyncInternal(byte position)
		{
			if (position < 0 || 7 < position)
				throw new ArgumentException("The measurement byte position must be in the interval 0-7.");

			LowSpeedReadResponse result = await SensorReadCommand((byte)(NxtUltraSonicSensorCommands.ReadMeasurementByte0 + position), 0x01);
			return result != null ? result.AsBytes[0] : byte.MinValue;
		}
		#endregion

		#region ReadMeasurementBytes
#if WINRT
		public IAsyncOperation<IEnumerable<byte>>
#else
		public Task<IEnumerable<byte>>
#endif
		ReadMeasurementBytesAsync()
		{
			return ReadMeasurementBytesAsyncInternal()
#if WINRT
			.AsAsyncOperation<IEnumerable<byte>>()
#endif
			;
		}

		internal async Task<IEnumerable<byte>> ReadMeasurementBytesAsyncInternal()
		{
			LowSpeedReadResponse result = await SensorReadCommand((byte)(NxtUltraSonicSensorCommands.ReadMeasurementByte0), 0x08);
			return result != null ? result.AsBytes : new byte[0];
		}
		#endregion

		#region ReadActualZero
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		ReadActualZeroAsync()
		{
			return ReadActualZeroAsyncInternal()
#if WINRT
			.AsAsyncOperation<byte>()
#endif
			;
		}

		internal async Task<byte> ReadActualZeroAsyncInternal()
		{
			LowSpeedReadResponse result = await SensorReadCommand((byte)NxtUltraSonicSensorCommands.ReadActualZero, 0x01);
			return result != null ? result.AsBytes[0] : byte.MinValue;
		}
		#endregion

		#region ReadActualScaleFactor
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		ReadActualScaleFactorAsync()
		{
			return ReadActualScaleFactorAsyncInternal()
#if WINRT
			.AsAsyncOperation<byte>()
#endif
			;
		}

		internal async Task<byte> ReadActualScaleFactorAsyncInternal()
		{
			LowSpeedReadResponse result = await SensorReadCommand((byte)NxtUltraSonicSensorCommands.ReadActualScaleFactor, 0x01);
			return result != null ? result.AsBytes[0] : byte.MinValue;
		}
		#endregion

		#region ReadActualScaleDivisor
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		ReadActualScaleDivisorAsync()
		{
			return ReadActualScaleDivisorAsyncInternal()
#if WINRT
			.AsAsyncOperation<byte>()
#endif
			;
		}

		internal async Task<byte> ReadActualScaleDivisorAsyncInternal()
		{
			LowSpeedReadResponse result = await SensorReadCommand((byte)NxtUltraSonicSensorCommands.ReadActualScaleDivisor, 0x01);
			return result != null ? result.AsBytes[0] : byte.MinValue;
		}
		#endregion

		#region OffCommand
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		OffCommandAsync()
		{
			return OffCommandAsyncInternal()
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async Task OffCommandAsyncInternal()
		{
			await SensorWriteCommand((byte)NxtUltraSonicSensorCommands.OffCommand, 0x00);
		}
		#endregion

		#region SingleShotCommand
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		SingleShotCommandAsync()
		{
			return SetCommandStateAsyncInternal(NxtUltraSonicSensorCommands.SingleShotCommand, 0x01)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region ContinuousMeasurementCommand
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		ContiniousMeasurementCommandAsync()
		{
			return SetCommandStateAsyncInternal(NxtUltraSonicSensorCommands.ContinuousMeasurementCommand, 0x02)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region EventCaptureCommand
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
 EventCaptureCommandAsync()
		{
			return SetCommandStateAsyncInternal(NxtUltraSonicSensorCommands.EventCaptureCommand, 0x03)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region RequestWarmReset
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		RequestWarmResetAsync()
		{
			return SetCommandStateAsyncInternal(NxtUltraSonicSensorCommands.RequestWarmReset, 0x04)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		internal async Task SetCommandStateAsyncInternal(NxtUltraSonicSensorCommands command, byte commandValue)
		{
			await SensorWriteCommand((byte)command, commandValue);
		}

		#region SetContinuousMeasurementInterval
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		SetContiniousMeasurementIntervalAsync(byte interval)
		{
			return SetContinuousMeasurementIntervalAsyncInternal(interval)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async Task SetContinuousMeasurementIntervalAsyncInternal(byte interval)
		{
			await SensorWriteCommand((byte)NxtUltraSonicSensorCommands.SetContinuousMeasurementInterval, interval);
		}
		#endregion

		#region SetActualZero
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		SetActualZeroAsync(byte offset)
		{
			return SetActualZeroAsyncInternal(offset)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async Task SetActualZeroAsyncInternal(byte offset)
		{
			await SensorWriteCommand((byte)NxtUltraSonicSensorCommands.SetActualZero, offset);
			await Task.Delay(20);
			byte result = await brick.DirectCommands.LowSpeedGetStatusAsync(SensorPort);
			System.Diagnostics.Debug.WriteLine("Result = {0}", result);

		}
		#endregion

		#region SetActualScaleFactor
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		SetActualScaleFactorAsync(byte factor)
		{
			return SetActualScaleFactorAsyncInternal(factor)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async Task SetActualScaleFactorAsyncInternal(byte factor)
		{
			await SensorWriteCommand((byte)NxtUltraSonicSensorCommands.SetActualScaleFactor, factor);
		}
		#endregion

		#region SetActualScaleDivisor
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		SetActualScaleDivisorAsync(byte divisor)
		{
			return SetActualScaleDivisorAsyncInternal(divisor)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async Task SetActualScaleDivisorAsyncInternal(byte divisor)
		{
			await SensorWriteCommand((byte)NxtUltraSonicSensorCommands.SetActualScaleDivisor, divisor);
		}
		#endregion

		public byte Distance { get { return distance; } }

		public byte TriggerDistance { get; set; }

		public event EventHandler<SensorEventArgs> OnObjectDetected;

		public event EventHandler<SensorEventArgs> OnObjectLost;

		private void OnObjectDetectedEventHandler()
		{
			if (null != OnObjectDetected)
				Task.Run(() => OnObjectDetected(this, new SensorEventArgs()));
		}

		private void OnObjectLostEventHandler()
		{
			if (null != OnObjectLost)
				Task.Run(() => OnObjectLost(this, new SensorEventArgs()));
		}

		internal override async Task PollAsyncInternal()
		{
			byte previous = this.distance;
			IEnumerable<byte> result = await this.ReadMeasurementBytesAsync();
			this.distances = result.ToArray();
			if (distances.Length > 0)
			{
				if (distances[0] != distance)
				{
					distance = distances[0];
					if (previous != distance)
						OnChangedEventHandler();
					if ((previous == byte.MinValue || previous > TriggerDistance) && distance <= TriggerDistance)
						OnObjectDetectedEventHandler();
					else if (previous <= TriggerDistance && distance > TriggerDistance)
						OnObjectLostEventHandler();
				}
			}
		}
	}
}
