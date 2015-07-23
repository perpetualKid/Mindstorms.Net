using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINRT
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Foundation.Metadata;
#endif

namespace Mindstorms.Net.Core.Commands
{
	public sealed partial class DirectCommands : CommandsBase
	{
		#region PlayTone
		/// <summary>
		/// Plays a tone of the specified frequency for the specified time.
		/// </summary>
		/// <param name="frequency">Frequency of tone, in hertz.</param>
		/// <param name="duration">Duration to play tone, in milliseconds.</param>
		/// <returns></returns>
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		PlayToneAsync(ushort frequency, ushort duration)
		{
			return PlayToneAsyncInternal(frequency, duration)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region StopSoundPlayback
		/// <summary>
		/// Stops playing the current sound on the NXT.
		/// </summary>
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		StopSoundPlaybackAsync()
		{
			return StopSoundPlaybackAsyncInternal()
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region PlaySoundFile
		/// <summary>
		/// Plays the specified sound file on the NXT.
		/// </summary>
		/// <param name="fileName">
		/// The name of the file to play. ASCIIZ-string with maximum size 15.3 characters, the default extension is .rso.
		/// </param>
		/// <param name="loop"><c>True</c> to loop indefinitely or <c>false</c> to play the file only once.</param>
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		PlaySoundFileAsync(string fileName, bool loop)
		{
			return PlaySoundFileAsyncInternal(fileName, loop)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region GetBatteryLevel
		/// <summary>
		/// Reads the battery level of the NXT.
		/// </summary>
		/// <returns>The battery level in millivolts.</returns>
#if WINRT
		public IAsyncOperation<ushort>
#else
		public Task<ushort>
#endif
		GetBatteryLevelAsync()
		{
			return GetBatteryLevelAsyncInternal()
#if WINRT
			.AsAsyncOperation<ushort>()
#endif
;
		}
		#endregion

		#region KeepAlive
		/// <summary>
		/// Resets the sleep timer and returns the current sleep time limit.
		/// </summary>
		/// <remarks>
		/// This syscall method resets the NXT brick's internal sleep timer and returns the current time limit, 
		/// in milliseconds, until the next automatic sleep. Use this method to keep the NXT brick from automatically
		/// turning off. Use the NXT brick's UI menu to configure the sleep time limit.
		/// </remarks>
		/// <returns>The currently set sleep time limit in milliseconds.</returns>
#if WINRT
		public IAsyncOperation<uint>
#else
		public Task<uint>
#endif
		KeepAliveAsync()
		{
			return KeepAliveAsyncInternal()
#if WINRT
			.AsAsyncOperation<uint>()
#endif
			;
		}
		#endregion

		#region StartProgram
		/// <summary>
		/// Starts the specified program on the NXT.
		/// </summary>
		/// <param name="fileName">The name of the file to start. ASCIIZ-string with maximum size 15.3 characters, the default extension is .rxe .</param>
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		StartProgramAsync(string fileName)
		{
			return StartProgramAsyncInternal(fileName)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region StopProgram
		/// <summary>
		/// Stops the currently running program on the NXT.
		/// </summary>
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		 StopProgramAsync()
		{
			return StopProgramAsyncInternal()
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region GetCurrentProgramName
		/// <summary>
		/// Returns the name of the program currently running on the NXT.
		/// </summary>
		/// <returns>
		/// The name of the file currently executing or <c>null</c> if no program is currently running. 
		/// Format: ASCIIZ-string with maximum 15.3 characters.
		/// </returns>
#if WINRT
		public IAsyncOperation<string>
#else
		public Task<string>
#endif
		GetCurrentProgramNameAsync()
		{
			return GetCurrentProgramNameAsyncInternal()
#if WINRT
			.AsAsyncOperation<string>()
#endif
;
		}
		#endregion

		#region MessageWrite
		/// <summary>
		/// Writes the specified <paramref name="message"/> to the specified <paramref name="inboxNumber"/>.
		/// </summary>
		/// <param name="inboxNumber">The inbox number (0-9).</param>
		/// <param name="message">The message to write. Cannot be longer than 58 bytes to be legal on USB.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// If <paramref name="inboxNumber"/> is not between 0 and 9 or <paramref name="message"/> is longer than 58 bytes.
		/// </exception>
#if WINRT
		[DefaultOverload()]
		public IAsyncAction
#else
		public Task
#endif
		MessageWriteAsync(int inboxNumber, string message)
		{
			return MessageWriteAsyncInternal(inboxNumber, message)
#if WINRT
			.AsAsyncAction()
#endif
;
		}

#if WINRT
		public IAsyncAction MessageWriteAsync(int inboxNumber, IBuffer message)
#else
		public Task MessageWriteAsync(int inboxNumber, byte[] message)
#endif
		
		{
			return MessageWriteAsyncInternal(inboxNumber, message)
#if WINRT
		.AsAsyncAction()
#endif
;
		}
		#endregion

		#region MessageRead
		/// <summary>
		/// Reads a message from the specified local inbox.
		/// </summary>
		/// <param name="remoteInboxNumber">Remote inbox number (0-19). </param>
		/// <param name="localInboxNumber">Local inbox number (0-9).</param>
		/// <param name="removeFromRemoteInbox"><c>True</c> to clear the message from the remote inbox, <c>false</c> otherwise.</param>
		/// <returns>The incoming message including null termination byte.</returns>
		/// <remarks>
		/// Remote inbox number may specify a value of 0-19, while all other mailbox numbers should remain below 9.
		/// This is due to the master-slave relationship between the connected NXT bricks.
		/// Slave devices may not initiate communication transactions with their masters,
		/// so they store outgoing messages in the upper 10 mailboxes (indices 10-19).
		/// Use the MessageRead command from the master device to retrieve these messages.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">
		/// If <paramref name="remoteInboxNumber"/> not between 0 and 19 or 
		/// <paramref name="localInboxNumber"/> not between 0 and 9.
		/// </exception>
#if WINRT
		public IAsyncOperation<MessageReadResponse>
#else
		public Task<MessageReadResponse>
#endif
		MessageReadAsync(int remoteInboxNumber, int localInboxNumber, bool removeFromRemoteInbox)
		{
			return MessageReadAsyncInternal(remoteInboxNumber, localInboxNumber, removeFromRemoteInbox)
#if WINRT
			.AsAsyncOperation<MessageReadResponse>()
#endif
			;
		}
		#endregion

		#region ResetMotorPosition
		/// <summary>
		/// Resets the motor position on the specified motor port.
		/// </summary>
		/// <param name="motorPort">The motor to reset.</param>
		/// <param name="relative"><c>True</c>, if position relative to last movement, <c>false</c> for absolute position.</param>
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		ResetMotorPositionAsync(MotorPort motorPort, bool relative)
		{
			return ResetMotorPositionAsyncInternal(motorPort, relative)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region SetOutputState
		/// <summary>
		/// Sends a command to the motor connected to the specified port.
		/// </summary>
		/// <param name="motorPort">The port where the motor is connected to.</param>
		/// <param name="power">
		/// Power (also referred as speed) set point. Range: -100-100.
		/// The absolute value of <paramref name="power"/> is used as a percentage of the full power capabilities of the motor.
		/// The sign of <paramref name="power"/> specifies rotation direction. 
		/// Positive values for <paramref name="power"/> instruct the firmware to turn the motor forward, 
		/// while negative values instruct the firmware to turn the motor backward. 
		/// "Forward" and "backward" are relative to a standard orientation for a particular type of motor.
		/// Note that direction is not a meaningful concept for outputs like lamps. 
		/// Lamps are affected only by the absolute value of <paramref name="power"/>.
		/// </param>
		/// <param name="motorModes">Motor mode (bit-field).</param>
		/// <param name="regulationregulationMode">Regulation mode.</param>
		/// <param name="turnRatio">
		/// This property specifies the proportional turning ratio for synchronized turning using two motors. Range: -100-100.
		/// <remarks>
		/// Negative <paramref name="turnRatio"/> values shift power towards the left motor, 
		/// whereas positive <paramref name="turnRatio"/> values shift power towards the right motor. 
		/// In both cases, the actual power applied is proportional to the <paramref name="power"/> set-point, 
		/// such that an absolute value of 50% for <paramref name="turnRatio"/> normally results in one motor stopping,
		/// and an absolute value of 100% for <paramref name="turnRatio"/> normally results in the two motors 
		/// turning in opposite directions at equal power.
		/// </remarks>
		/// </param>
		/// <param name="runState">Motor run state.</param>
		/// <param name="tachoLimit">
		/// Tacho limit. This property specifies the rotational distance in 
		/// degrees that you want to turn the motor. Range: 0-4294967295, O: run forever.
		/// The sign of the <paramref name="power"/> property specifies the direction of rotation.
		/// </param>
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		SetOutputStateAsync(MotorPort motorPort, short power, MotorModes motorModes, MotorRegulationMode regulationMode,
			short turnRatio, MotorRunState runState, uint tachoLimit)
		{
			return SetOutputStateAsyncInternal(motorPort, power, motorModes, regulationMode, turnRatio, runState, tachoLimit)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region GetOutputState
		/// <summary>
		/// Returns the current state of the specified motor.
		/// </summary>
		/// <param name="motorPort">The port where the motor is connected.</param>
		/// <returns>The current state of the specified motor.</returns>
#if WINRT
		public IAsyncOperation<GetOutputStateResponse>
#else
		public Task<GetOutputStateResponse>
#endif
		GetOutputStateAsync(MotorPort motorPort)
		{
			return GetOutputStateAsyncInternal(motorPort)
#if WINRT
			.AsAsyncOperation<GetOutputStateResponse>()
#endif
			;
		}
		#endregion

		#region SetInputMode
		/// <summary>
		/// Configures a sensor on the specified port.
		/// </summary>
		/// <param name="sensorPort">The port the sensor is connected to.</param>
		/// <param name="sensorType">The type of the sensor connected to the port.</param>
		/// <param name="sensorMode">
		/// The mode in which the sensor operates. The sensor mode affects the scaled value, 
		/// which the NXT firmware calculates depending on the sensor type and sensor mode.
		/// </param>
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		SetInputModeAsync(SensorPort sensorPort, SensorType sensorType, SensorMode sensorMode)
		{
			return SetInputModeAsyncInternal(sensorPort, sensorType, sensorMode)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region ResetInputScaledValue
		/// <summary>
		/// Resets the value of the sensor connected to the specified port.
		/// </summary>
		/// <param name="sensorPort">The port to which the sensor is connected.</param>
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		ResetInputScaledValueAsync(SensorPort sensorPort)
		{
			return ResetInputScaledValueAsyncInternal(sensorPort)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region GetInputValues
		/// <summary>
		/// Reads the current state of the sensor connected to the specified port.
		/// </summary>
		/// <param name="sensorPort">The port to which the sensor is connected.</param>
		/// <returns>A <see cref="GetInputValuesResponse"/> instance that describes the current state and value of the sensor.</returns>
#if WINRT
		public IAsyncOperation<GetInputValuesResponse>
#else
		public Task<GetInputValuesResponse>
#endif
		GetInputValuesAsync(SensorPort sensorPort)
		{
			return GetInputValuesAsyncInternal(sensorPort)
#if WINRT
			.AsAsyncOperation<GetInputValuesResponse>()
#endif
;
		}
		#endregion

		#region LowSpeedGetStatus
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		LowSpeedGetStatusAsync(SensorPort sensorPort)
		{
			return LowSpeedGetStatusAsyncInternal(sensorPort)
#if WINRT
			.AsAsyncOperation<byte>()
#endif
			;
		}
		#endregion

		#region LowSpeedWrite
		/// <summary>
		/// Writes data to the LowSpeed port. Used with digital sensors like the Ultrasonic sensor.
		/// </summary>
		/// <param name="sensorPort">Sensor port</param>
		/// <param name="data">Tx Data</param>
		/// <param name="responseLength">Rx Data Length</param>
		/// <seealso cref="LowSpeedRead"/>
		/// <seealso cref="LowSpeedGetStatus"/>
#if WINRT
		[DefaultOverload()]
		public IAsyncAction
#else
		public Task
#endif
		LowSpeedWriteAsync(SensorPort sensorPort, string message, int expectedResultLength)
		{
			return LowSpeedWriteAsyncInternal(sensorPort, message, (byte)expectedResultLength)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

#if WINRT
		public IAsyncAction LowSpeedWriteAsync(SensorPort sensorPort, IBuffer data, int expectedResultLength)
#else
		public Task LowSpeedWriteAsync(SensorPort sensorPort, byte[] data, int expectedResultLength)
#endif

		{
			return LowSpeedWriteAsyncInternal(sensorPort, data, (byte)expectedResultLength)
#if WINRT
			.AsAsyncAction()
#endif
		;
		}
		#endregion

		#region MessageRead
		/// <summary>
		/// Reads data from the LowSpeed port. Used with digital sensors like the Ultrasonic sensor.
		/// </summary>
		/// <param name="sensorPort">The sensor port</param>
		/// <returns>The data read from the port</returns>
		/// <seealso cref="LowSpeedGetStatus"/>
		/// <seealso cref="LowSpeedWrite"/>
#if WINRT
		public IAsyncOperation<LowSpeedReadResponse>
#else
		public Task<LowSpeedReadResponse>
#endif
		LowSpeedReadAsync(SensorPort sensorPort)
		{
			return LowSpeedReadInternalAsync(sensorPort)
#if WINRT
			.AsAsyncOperation<LowSpeedReadResponse>()
#endif
;
		}
		#endregion

	}
}
