using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINRT
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using System.Runtime.InteropServices.WindowsRuntime;
#endif

namespace Mindstorms.Net.Core.Commands
{
	public sealed partial class DirectCommands : CommandsBase
	{

		internal DirectCommands(NxtBrick brick) :
			base(brick)
		{
		}

		internal async Task PlayToneAsyncInternal(ushort frequency, ushort duration)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.PlayTone(frequency, duration));
		}

		internal async Task PlaySoundFileAsyncInternal(string fileName, bool loop)
		{
			if (string.IsNullOrEmpty(System.IO.Path.GetExtension(fileName)))
				fileName += ".rso";
			await brick.SendCommandAsyncInternal(RequestTelegram.PlaySoundFile(fileName, loop));
		}

		internal async Task StopSoundPlaybackAsyncInternal()
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.StopSoundPlayback());
		}

		internal async Task<ushort> GetBatteryLevelAsyncInternal()
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.GetBatteryLevel());
			return ResponseTelegram.GetBatteryLevel(brick.LastResponse);
		}

		internal async Task<uint> KeepAliveAsyncInternal()
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.KeepAlive());
			return ResponseTelegram.KeepAlive(brick.LastResponse);
		}

		internal async Task StartProgramAsyncInternal(string fileName)
		{
			if (!System.IO.Path.HasExtension(fileName))
				fileName = System.IO.Path.ChangeExtension(fileName, FileTypeExtension.rxe.ToString());

			await brick.SendCommandAsyncInternal(RequestTelegram.StartProgram(fileName));
		}

		internal async Task StopProgramAsyncInternal()
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.StopProgram());
		}

		internal async Task<string> GetCurrentProgramNameAsyncInternal()
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.GetCurrentProgramName());
			return ResponseTelegram.GetCurrentProgramName(brick.LastResponse);
		}

#if WINRT
		internal async Task MessageWriteAsyncInternal(int inboxNumber, IBuffer message)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.MessageWrite(inboxNumber, message.ToArray()));
		}
#endif

		internal async Task MessageWriteAsyncInternal(int inboxNumber, byte[] message)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.MessageWrite(inboxNumber, message));
		}

		internal async Task MessageWriteAsyncInternal(int inboxNumber, string message)
		{
			byte[] buffer = Encoding.GetEncoding("ASCII").GetBytes(message);
			await MessageWriteAsyncInternal(inboxNumber, buffer);
		}

		internal async Task<MessageReadResponse> MessageReadAsyncInternal(int remoteInboxNumber, int localInboxNumber, bool removeFromRemoteInbox)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.MessageRead(remoteInboxNumber, localInboxNumber, removeFromRemoteInbox));
			return ResponseTelegram.MessageRead(brick.LastResponse);
		}

		internal async Task ResetMotorPositionAsyncInternal(MotorPort motorPort, bool relative)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.ResetMotorPosition(motorPort, relative));
		}

		internal async Task SetOutputStateAsyncInternal(MotorPort motorPort, short power, MotorModes motorMode, MotorRegulationMode motorRegulation,
			short turnRatio, MotorRunState runState, uint tachoLimit)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.SetOutputState(motorPort, power, motorMode, motorRegulation, 
				turnRatio, runState, tachoLimit));
		}

		internal async Task<GetOutputStateResponse> GetOutputStateAsyncInternal(MotorPort motorPort)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.GetOutputState(motorPort));
			return ResponseTelegram.GetOutputState(brick.LastResponse);
		}

		internal async Task ResetInputScaledValueAsyncInternal(SensorPort sensorPort)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.ResetInputScaledValue(sensorPort));
		}

		internal async Task SetInputModeAsyncInternal(SensorPort sensorPort, SensorType sensorType, SensorMode sensorMode)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.SetInputMode(sensorPort, sensorType, sensorMode));
		}

		internal async Task<GetInputValuesResponse> GetInputValuesAsyncInternal(SensorPort sensorPort)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.GetInputValues(sensorPort));
			return ResponseTelegram.GetInputValues(brick.LastResponse);
		}

		internal async Task<byte> LowSpeedGetStatusAsyncInternal(SensorPort sensorPort)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.LowSpeedGetStatus(sensorPort));
			return brick.LastResponse.Data[3];
		}


#if WINRT
		internal async Task LowSpeedWriteAsyncInternal(SensorPort sensorPort, IBuffer data, byte responseLength)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.LowSpeedWrite(sensorPort, data.ToArray(), responseLength));
		}
#endif
		internal async Task LowSpeedWriteAsyncInternal(SensorPort sensorPort, byte[] data, byte responseLength)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.LowSpeedWrite(sensorPort, data, responseLength));
		}

		internal async Task LowSpeedWriteAsyncInternal(SensorPort sensorPort, string data, byte responseLength, bool isHexString = false)
		{
			byte[] buffer;
			if (isHexString)
			{
				List<Byte> bytes = new List<byte>();
				foreach (string value in data.Split(' ', '-', ';'))
				{
					bytes.Add(Convert.ToByte(value, 16));
				}
				buffer = bytes.ToArray();
			}
			else
				buffer = Encoding.GetEncoding("ASCII").GetBytes(data);

			await brick.SendCommandAsyncInternal(RequestTelegram.LowSpeedWrite(sensorPort, buffer, responseLength));
		}

		internal async Task<LowSpeedReadResponse> LowSpeedReadInternalAsync(SensorPort sensorPort)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.LowSpeedRead(sensorPort));
			return ResponseTelegram.LowSpeedRead(brick.LastResponse);
		}
	}
}
