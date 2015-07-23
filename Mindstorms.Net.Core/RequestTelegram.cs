using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	public class RequestTelegram: IDisposable
	{
		private MemoryStream stream;
		private BinaryWriter writer;

		private RequestTelegram(CommandType commandType, NxtCommands command)
		{
			this.CommandType = commandType;
			this.Command = command;

			this.stream = new MemoryStream();
			this.writer = new BinaryWriter(stream);
			// add 2 bytes for Length indication, will be set correctly later
			writer.Write((ushort)0xffff);
			// Command Type byte
			writer.Write((byte)this.CommandType);

			// Command Type byte
			writer.Write((byte)this.Command);
		}

		public CommandType CommandType { get; private set; }

		public NxtCommands Command { get; private set; }

		public int Length
		{
			get
			{
				if (this.stream == null)
					throw new InvalidOperationException("Stream not valid.");
				return (int)this.stream.Length;
			}
		}

		public byte[] ToBytes()
		{

			byte[] result = stream.ToArray();
			// size of payload, not including the 2 size bytes
			ushort size = (ushort)(result.Length - 2);


			// little-endian
			result[0] = (byte)(size & 0xFF);
			result[1] = (byte)((size & 0xFF00) >> 8);

			return result;
		}

		#region  System Commands
		/// <summary>
		/// Returns the current firmware and protocol version used by the NXT.
		/// </summary>
		public static RequestTelegram GetFirmwareVersion()
		{
			return new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.GetFirmwareVersion);
		}

		/// <summary>
		/// Sends a Reboot Command to the NXT. Can be used over USB only!
		/// </summary>
		public static RequestTelegram Boot()
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.Boot);
			result.AddParameter("Let's dance SAMBA");
			return result;
		}

		/// <summary>
		/// Sets the NXT brick name to the specified value.
		/// </summary>
		/// <param name="name">The new name of the brick to set. Maximum 15 characters.</param>
		public static RequestTelegram SetBrickName(string name)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandNoResponse, NxtCommands.SetBrickName);
			result.AddParameter(name);
			return result;
		}

		/// <summary>
		/// Gets Device Information about the NXT.
		/// </summary>
		public static RequestTelegram GetDeviceInfo()
		{
			return new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.GetDeviceInfo);
		}

		/// <summary>
		/// Lists files in the NXT brick according to the file mask.
		/// </summary>
		/// <param name="fileType"></param>
		public static RequestTelegram FindFirst(string fileNamePattern)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.FindFirst);
			result.AddParameter(fileNamePattern, 20);
			return result;

		}

		/// <summary>
		/// Lists next file in the NXT current search identified by Handle-parameter.
		/// </summary>
		/// <param name="handle"></param>
		public static RequestTelegram FindNext(byte handle)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.FindNext);
			result.AddParameter(handle);
			return result;

		}

		/// <summary>
		/// Closes the given handle
		/// </summary>
		/// <param name="handle"></param>
		public static RequestTelegram Close(byte handle)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.Close);
			result.AddParameter(handle);
			return result;
		}

		/// <summary>
		/// This command opens an existing non linear file on NXT's brick 
		/// specified by file name for reading operations from the beginning of the file.
		/// </summary>
		/// <param name="fileName"></param>
		public static RequestTelegram OpenRead(string fileName)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.OpenRead);
			result.AddParameter(fileName);
			return result;
		}

		/// <summary>
		/// Reads specified bytes of data from previously opened non-linear file handle.
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="length"></param>
		public static RequestTelegram Read(byte handle, ushort length)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.Read);
			result.AddParameter(handle);
			result.AddParameter(length);
			return result;
		}

		/// <summary>
		/// Creates and opens a nonlinear file to write non textual data
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fileSize"></param>
		public static RequestTelegram OpenWrite(string fileName, uint fileSize)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.OpenWrite);
			result.AddParameter(fileName, 20);
			result.AddParameter(fileSize);
			return result;
		}

		/// <summary>
		/// Writes data from buffer to the previously opened file specified by handle
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="data"></param>
		public static RequestTelegram Write(byte handle, [System.Runtime.InteropServices.WindowsRuntime.ReadOnlyArray] byte[] data)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.Write);
			result.AddParameter(handle);
			result.AddParameter(data);
			return result;
		}


		/// <summary>
		/// Creates and opens a nonlinear file to write textual data
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fileSize"></param>
		public static RequestTelegram OpenDataWrite(string fileName, uint fileSize)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.OpenDataWrite);
			result.AddParameter(fileName, 20);
			result.AddParameter(fileSize);
			return result;
		}

		/// <summary>
		/// Opens an existing nonlinear file to append textual data
		/// </summary>
		/// <param name="fileName"></param>
		public static RequestTelegram OpenDataAppend(string fileName)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.OpenDataAppend);
			result.AddParameter(fileName, 20);
			return result;
		}

		/// <summary>
		/// Clears the user data flash and deletes all files
		/// </summary>
		public static RequestTelegram DeleteUserFlash()
		{
			return new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.DeleteUserFlash);
		}

		/// <summary>
		/// Deletes the specified file
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static RequestTelegram Delete(string fileName)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.Delete);
			result.AddParameter(fileName);
			return result;
		}

		/// <summary>
		/// Requests the length of Poll-Command data available
		/// </summary>
		/// <param name="buffer"></param>
		public static RequestTelegram PollCommandLength(PollCommandBuffer buffer)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.PollCommandLength);
			result.AddParameter(Convert.ToByte(buffer));
			return result;
		}

		/// <summary>
		/// Polls the specified buffer for given length of poll command
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="commandLength"></param>
		public static RequestTelegram PollCommand(PollCommandBuffer buffer, byte commandLength)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.PollCommand);
			result.AddParameter(Convert.ToByte(buffer));
			result.AddParameter(commandLength);
			return result;
		}

		/// <summary>
		/// Opens a linear file for read access
		/// </summary>
		/// <param name="fileName"></param>
		public static RequestTelegram OpenLinearRead(string fileName)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.OpenLinearRead);
			result.AddParameter(fileName, 20);
			return result;
		}

		/// <summary>
		/// Creates and opens a linear file for write access
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fileSize"></param>
		public static RequestTelegram OpenLinearWrite(string fileName, uint fileSize)
		{
			RequestTelegram result = new RequestTelegram(CommandType.SystemCommandResponseRequired, NxtCommands.OpenLinearWrite);
			result.AddParameter(fileName, 20);
			result.AddParameter(fileSize);
			return result;
		}
		#endregion

		#region Direct Commands
		/// <summary>
		/// Reads the battery level of the NXT.
		/// </summary>
		/// <returns>The battery level in millivolts.</returns>
		public static RequestTelegram GetBatteryLevel()
		{
			return new RequestTelegram(CommandType.DirectCommandResponseRequired, NxtCommands.GetBatteryLevel);
		}

		/// <summary>
		/// Resets the sleep timer and returns the current sleep time limit.
		/// </summary>
		/// <remarks>
		/// This syscall method resets the NXT brick's internal sleep timer and returns the current time limit, 
		/// in milliseconds, until the next automatic sleep. Use this method to keep the NXT brick from automatically
		/// turning off. Use the NXT brick's UI menu to configure the sleep time limit.
		/// </remarks>
		/// <returns>The currently set sleep time limit in milliseconds.</returns>
		public static RequestTelegram KeepAlive()
		{
			return new RequestTelegram(CommandType.DirectCommandResponseRequired, NxtCommands.KeepAlive);
		}

		/// <summary>
		/// Plays a tone on the NXT with the specified frequency for the specified duration.
		/// </summary>
		/// <param name="frequency">Frequency for the tone in Hz. Range: 200-14000 Hz</param>
		/// <param name="duration">Duration of the tone in ms.</param>
		/// 
		public static RequestTelegram PlayTone(ushort frequency, ushort duration)
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandNoResponse, NxtCommands.PlayTone);
			result.AddParameter(frequency);
			result.AddParameter(duration);
			return result;
		}

		/// <summary>
		/// Plays the specified sound file on the NXT.
		/// </summary>
		/// <param name="fileName">
		/// The name of the file to play. ASCIIZ-string with maximum size 15.3 characters, the default extension is .rso.
		/// </param>
		/// <param name="loop"><c>True</c> to loop indefinitely or <c>false</c> to play the file only once.</param>
		public static RequestTelegram PlaySoundFile(string fileName, bool loop)
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandNoResponse, NxtCommands.PlaySoundFile);
			result.AddParameter(loop);
			result.AddParameter(fileName);
			return result;
		}

		public static RequestTelegram StopSoundPlayback()
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandNoResponse, NxtCommands.StopSoundPlayback);
			return result;
		}

		/// <summary>
		/// Starts the specified program on the NXT.
		/// </summary>
		/// <param name="fileName">The name of the file to start. ASCIIZ-string with maximum size 15.3 characters, the default extension is .rxe .</param>
		public static RequestTelegram StartProgram(string fileName)
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandNoResponse, NxtCommands.StartProgram);
			result.AddParameter(fileName, 20);
			return result;
		}

		/// <summary>
		/// Stops the currently running program on the NXT.
		/// </summary>
		public static RequestTelegram StopProgram()
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandNoResponse, NxtCommands.StopProgram);
			return result;
		}

		/// <summary>
		/// Returns the name of the program currently running on the NXT.
		/// </summary>
		/// <returns>
		/// The name of the file currently executing or <c>null</c> if no program is currently running. 
		/// Format: ASCIIZ-string with maximum 15.3 characters.
		/// </returns>
		public static RequestTelegram GetCurrentProgramName()
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandResponseRequired, NxtCommands.GetCurrentProgramName);
			return result;
		}


		/// <summary>
		/// Writes the specified <paramref name="message"/> to the specified <paramref name="inboxNumber"/>.
		/// </summary>
		/// <param name="inboxNumber">The inbox number (0-9).</param>
		/// <param name="message">The message to write. Cannot be longer than 58 bytes to be legal on USB.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// If <paramref name="inboxNumber"/> is not between 0 and 9 or <paramref name="message"/> is longer than 58 bytes.
		/// </exception>
		public static RequestTelegram MessageWrite(int inboxNumber, [System.Runtime.InteropServices.WindowsRuntime.ReadOnlyArray] byte[] message)
		{
			// Validate input: inbox number should be between 0 and 9.
			if (inboxNumber < 0 || inboxNumber > 9)
			{
				throw new ArgumentOutOfRangeException("inboxNumber", "Inbox number must be between 0 and 9.");
			}

			// Validate input: message length cannot exceed 58 bytes.
			if (message.Length > 58)
			{
				throw new ArgumentOutOfRangeException("message", "Message cannot be longer than 58 bytes.");
			}

			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandNoResponse, NxtCommands.MessageWrite);
			result.AddParameter(Convert.ToByte(inboxNumber));
			result.AddParameter(Convert.ToByte(message.Length + 1));
			result.AddParameter(message);
			result.AddParameter(char.MinValue);
			return result;

		}

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
		public static RequestTelegram MessageRead(int remoteInboxNumber, int localInboxNumber, bool removeFromRemoteInbox)
		{
			// Validate input: remote inbox number should be between 0 and 19.
			if (remoteInboxNumber < 0 || remoteInboxNumber > 19)
			{
				throw new ArgumentOutOfRangeException("remoteInboxNumber", "Remote inbox number should be between 0 and 19.");
			}

			// Validate input: local inbox number should be between 0 and 9.
			if (localInboxNumber < 0 || localInboxNumber > 9)
			{
				throw new ArgumentOutOfRangeException("localInboxNumber", "Local inbox number should be between 0 and 9.");
			}

			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandResponseRequired, NxtCommands.MessageRead);
			result.AddParameter(Convert.ToByte(remoteInboxNumber));
			result.AddParameter(Convert.ToByte(localInboxNumber));
			result.AddParameter(removeFromRemoteInbox);
			return result;
		}

		/// <summary>
		/// Resets the motor position on the specified motor port.
		/// </summary>
		/// <param name="motorPort">The motor to reset.</param>
		/// <param name="relative"><c>True</c>, if position relative to last movement, <c>false</c> for absolute position.</param>
		public static RequestTelegram ResetMotorPosition(MotorPort motorPort, bool relative)
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandNoResponse, NxtCommands.ResetMotorPosition);
			result.AddParameter(Convert.ToByte(motorPort));
			result.AddParameter(relative);
			return result;

		}

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
		/// <param name="motorMode">Motor mode (bit-field).</param>
		/// <param name="motorRegulation">Regulation mode.</param>
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
		public static RequestTelegram SetOutputState(MotorPort motorPort, short power, MotorModes motorMode, MotorRegulationMode motorRegulation,
			short turnRatio, MotorRunState runState, uint tachoLimit)
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandNoResponse, NxtCommands.SetOutputState);
			result.AddParameter(Convert.ToByte(motorPort));
			result.AddParameter((byte)power);
			result.AddParameter(Convert.ToByte(motorMode));
			result.AddParameter(Convert.ToByte(motorRegulation));
			result.AddParameter((byte)turnRatio);
			result.AddParameter(Convert.ToByte(runState));
			result.AddParameter(tachoLimit);
			return result;
		}

		/// <summary>
		/// Returns the current state of the specified motor.
		/// </summary>
		/// <param name="motorPort">The port where the motor is connected.</param>
		/// <returns>The current state of the specified motor.</returns>
		public static RequestTelegram GetOutputState(MotorPort motorPort)
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandResponseRequired, NxtCommands.GetOutputState);
			result.AddParameter(Convert.ToByte(motorPort));
			return result;
		}

		/// <summary>
		/// Configures a sensor on the specified port.
		/// </summary>
		/// <param name="sensorPort">The port the sensor is connected to.</param>
		/// <param name="sensorType">The type of the sensor connected to the port.</param>
		/// <param name="sensorMode">
		/// The mode in which the sensor operates. The sensor mode affects the scaled value, 
		/// which the NXT firmware calculates depending on the sensor type and sensor mode.
		/// </param>
		public static RequestTelegram SetInputMode(SensorPort sensorPort, SensorType sensorType, SensorMode sensorMode)
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandNoResponse, NxtCommands.SetInputMode);
			result.AddParameter(Convert.ToByte(sensorPort));
			result.AddParameter(Convert.ToByte(sensorType));
			result.AddParameter(Convert.ToByte(sensorMode));
			return result;
		}

		/// <summary>
		/// Resets the value of the sensor connected to the specified port.
		/// </summary>
		/// <param name="sensorPort">The port to which the sensor is connected.</param>
		public static RequestTelegram ResetInputScaledValue(SensorPort sensorPort)
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandNoResponse, NxtCommands.ResetInputScaledValue);
			result.AddParameter(Convert.ToByte(sensorPort));
			return result;
		}

		/// <summary>
		/// Reads the current state of the sensor connected to the specified port.
		/// </summary>
		/// <param name="sensorPort">The port to which the sensor is connected.</param>
		/// <returns>A <see cref="GetInputValuesResponse"/> instance that describes the current state and value of the sensor.</returns>
		public static RequestTelegram GetInputValues(SensorPort sensorPort)
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandResponseRequired, NxtCommands.GetInputValues);
			result.AddParameter(Convert.ToByte(sensorPort));
			return result;
		}

		/// <summary>
		/// Returns the number of bytes ready in the LowSpeed port. Used with digital sensors like the Ultrasonic sensor.
		/// </summary>
		/// <param name="sensorPort">Sensor port</param>
		/// <returns>Bytes Ready (count of available bytes to read)</returns>
		/// <seealso cref="LowSpeedRead"/>
		/// <seealso cref="LowSpeedWrite"/>
		public static RequestTelegram LowSpeedGetStatus(SensorPort sensorPort)
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandResponseRequired, NxtCommands.LowSpeedGetStatus);
			result.AddParameter(Convert.ToByte(sensorPort));
			return result;
		}

		/// <summary>
		/// Writes data to the LowSpeed port. Used with digital sensors like the Ultrasonic sensor.
		/// </summary>
		/// <param name="sensorPort">Sensor port</param>
		/// <param name="data">Tx Data</param>
		/// <param name="responseLength">Rx Data Length</param>
		/// <seealso cref="LowSpeedRead"/>
		/// <seealso cref="LowSpeedGetStatus"/>
		public static RequestTelegram LowSpeedWrite(SensorPort sensorPort, [System.Runtime.InteropServices.WindowsRuntime.ReadOnlyArray] byte[] data, byte responseLength)
		{

			if (data.Length == 0)
				throw new ArgumentException("No data to send.");

			if (data.Length > 16)
				throw new ArgumentException("Transmit data may not exceed 16 bytes.");

			if (responseLength < 0 || 16 < responseLength)
				throw new ArgumentException("Response data length should be in the interval 0-16.");

			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandNoResponse, NxtCommands.LowSpeedWrite);
			result.AddParameter(Convert.ToByte(sensorPort));
			result.AddParameter(Convert.ToByte(data.Length));
			result.AddParameter(responseLength);
			result.AddParameter(data);
			return result;
		}

		/// <summary>
		/// Reads data from the LowSpeed port. Used with digital sensors like the Ultrasonic sensor.
		/// </summary>
		/// <param name="sensorPort">The sensor port</param>
		/// <returns>The data read from the port</returns>
		/// <seealso cref="LowSpeedGetStatus"/>
		/// <seealso cref="LowSpeedWrite"/>
		public static RequestTelegram LowSpeedRead(SensorPort sensorPort)
		{
			RequestTelegram result = new RequestTelegram(CommandType.DirectCommandResponseRequired, NxtCommands.LowSpeedRead);
			result.AddParameter(Convert.ToByte(sensorPort));
			return result;
		}
		#endregion

		#region private helpers
		private void AddParameter(ushort value)
		{
			this.writer.Write(BitConverter.GetBytes(value));
		}

		private void AddParameter(uint value)
		{
			this.writer.Write(BitConverter.GetBytes(value));
		}

		private void AddParameter(bool value)
		{
			this.writer.Write(Convert.ToByte(value));
		}

		private void AddParameter(byte value)
		{
			this.writer.Write(value);
		}

		private void AddParameter(byte[] value)
		{
			this.writer.Write(value);
		}

#if WinRT
		private void AddParameter(IBuffer value)
		{
			byte[] buffer = new byte[value.Length];
			buffer = Buffer.
			this.AddParameter(buffer);
		}
#endif

		private void AddParameter(string value, int fixedLength = -1)
		{
			byte[] buffer = Encoding.GetEncoding("ASCII").GetBytes(value);
			this.writer.Write(buffer);
			this.writer.Write(byte.MinValue);

			if (buffer.Length + 1 < fixedLength)
			{
				this.writer.Write(new byte[fixedLength - buffer.Length -1]);
			}
		}
		#endregion

		#region IDisposable
		private bool disposed;
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					// Dispose managed resources.
					if (null != this.writer)
					{
						this.writer.Dispose();
						this.writer = null;
					}
					if (null != this.stream)
					{
						this.stream.Dispose();
						this.stream = null;
					}
				}

				// Free native resources here if necessary.

				// Disposing has been done.
				this.disposed = true;
			}
		}


		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <remarks>
		/// Closes the serial port and disposes the channel used for communication with the NXT.
		/// </remarks>
		public void Dispose()
		{
			this.Dispose(true);

			// This object will be cleaned up by the Dispose method.
			// Take this object off the finalization queue and prevent finalization code 
			// for this object from executing a second time.
			GC.SuppressFinalize(this);
		}
		#endregion

	}
}
