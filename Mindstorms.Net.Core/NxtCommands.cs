using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	/// <summary>
	/// The Command code being sent to the brick    /// </summary>
	public enum NxtCommands
	{
		#region Direct Commands
		GetBatteryLevel = 0x0B,
		GetCurrentProgramName = 0x11,
		GetInputValues = 0x07,
		GetOutputState = 0x06,
		KeepAlive = 0x0D,
		LowSpeedGetStatus = 0x0E,
		LowSpeedRead = 0x10,
		LowSpeedWrite = 0x0F,
		MessageRead = 0x13,
		MessageWrite = 0x09,
		PlayTone = 0x03,
		PlaySoundFile = 0x02,
		ResetInputScaledValue = 0x08,
		ResetMotorPosition = 0x0A,
		SetInputMode = 0x05,
		SetOutputState = 0x04,
		StartProgram = 0x00,
		StopProgram = 0x01,
		StopSoundPlayback = 0x0C,
		#endregion

		#region System Commands
		GetDeviceInfo = 0x9B,
		GetFirmwareVersion = 0x88,
		SetBrickName = 0x98,
		Boot = 0x97,
		OpenRead = 0x80,
		OpenWrite = 0x81,
		Read = 0x82,
		Write = 0x83,
		Close = 0x84,
		Delete = 0x85,
		FindFirst = 0x86,
		FindNext = 0x87,
		OpenLinearWrite = 0x89,
		OpenLinearRead = 0x8A,
		OpenDataWrite = 0x8B,
		OpenDataAppend = 0x8C,
		DeleteUserFlash = 0xA0,
		PollCommandLength = 0xA1,
		PollCommand = 0xA2,
		#endregion
	}
}
