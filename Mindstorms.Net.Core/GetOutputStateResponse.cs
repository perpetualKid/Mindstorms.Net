using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	/// <summary>
	/// Structure that describes the current state of a motor.
	/// </summary>
	public sealed class GetOutputStateResponse: ResponseBase
	{

		internal GetOutputStateResponse(ResponseTelegram response)
			: base(response)
		{
		}

		public MotorPort MotorPort { get { return (MotorPort)response.Data[3]; } }

		/// <summary>
		/// Power (also referred as speed) set point. Range: -100-100.
		/// </summary>
		/// <remarks>
		/// The absolute value of <see cref="Power"/> is used as a percentage of the full power capabilities of the motor.
		/// The sign of <see cref="Power"/> specifies rotation direction. 
		/// Positive values for <see cref="Power"/> instruct the firmware to turn the motor forward, 
		/// while negative values instruct the firmware to turn the motor backward. 
		/// "Forward" and "backward" are relative to a standard orientation for a particular type of motor.
		/// Note that direction is not a meaningful concept for outputs like lamps. 
		/// Lamps are affected only by the absolute value of <see cref="Power"/>.
		/// </remarks>
		// Original type was SByte
		public short Power { get { return (short)response.Data[4]; } }

		/// <summary>
		/// Motor mode. (Bit-field.)
		/// </summary>
		public MotorModes MotorModes { get { return (MotorModes)response.Data[5]; } }

		/// <summary>
		/// Motor regulation mode.
		/// </summary>
		public MotorRegulationMode RegulationMode { get { return (MotorRegulationMode)response.Data[6]; } }

		/// <summary>
		/// This property specifies the proportional turning ratio for synchronized turning using two motors. Range: -100-100.
		/// </summary>
		/// <remarks>
		/// Negative <paramref name="turnRatio"/> values shift power towards the left motor, 
		/// whereas positive <paramref name="turnRatio"/> values shift power towards the right motor. 
		/// In both cases, the actual power applied is proportional to the <paramref name="power"/> set-point, 
		/// such that an absolute value of 50% for <paramref name="turnRatio"/> normally results in one motor stopping,
		/// and an absolute value of 100% for <paramref name="turnRatio"/> normally results in the two motors 
		/// turning in opposite directions at equal power.
		/// </remarks>
		// Originaly this type was SByte
		public short TurnRatio { get { return (short)response.Data[7]; } }

		/// <summary>
		/// Motor run state.
		/// </summary>
		public MotorRunState RunState { get { return (MotorRunState)response.Data[4]; } }

		/// <summary>
		/// Current limit on a movement in progress, if any.
		/// </summary>
		/// <remarks>
		/// This property specifies the rotational distance in 
		/// degrees that you want to turn the motor. Range: 0-4294967295, O: run forever.
		/// The sign of the <see cref="Power"/> property specifies the direction of rotation.
		/// </remarks>
		public uint TachoLimit { get { return BitConverter.ToUInt32(response.Data, 9); } }

		/// <summary>
		/// Internal count. Number of counts since last reset of the motor counter.
		/// </summary>
		/// <remarks>
		/// This property returns the internal position counter value for the specified port.
		/// The sign of <see cref="TachoCount"/> specifies rotation direction. 
		/// Positive values correspond to forward rotation while negative values correspond to backward rotation. 
		/// "Forward" and "backward" are relative to a standard orientation for a particular type of motor.
		/// </remarks>
		public int TachoCount { get { return BitConverter.ToInt32(response.Data, 13); } }

		/// <summary>
		/// Current position relative to last programmed movement. Range: -2147483648-2147483647.
		/// </summary>
		/// <remarks>
		/// This property reports the block-relative position counter value for the specified port.
		/// The sign of <see cref="BlockTachoCount" /> specifies the rotation direction. Positive values correspond to forward
		/// rotation while negative values correspond to backward rotation. "Forward" and "backward" are relative to
		/// a standard orientation for a particular type of motor.
		/// </remarks>
		public int BlockTachoCount { get { return BitConverter.ToInt32(response.Data, 17); } }

		/// <summary>
		/// Current position relative to last reset of the rotation sensor for this motor. Range: -2147483648-2147483647.
		/// </summary>
		/// <remarks>
		/// This property returns the program-relative position counter value for the specified port.
		/// The sign of <see cref="RotationCount" /> specifies rotation direction. Positive values correspond to forward rotation
		/// while negative values correspond to backward rotation. 
		/// "Forward" and "backward" are relative to a standard orientation for a particular type of motor.
		/// </remarks>
		public int RotationCount { get { return BitConverter.ToInt32(response.Data, 21); } }

		public override string ToString()
		{
			return string.Format("Motor: {0}\r\nPower: {1}%\r\nMotorModes: {2}\r\nRegulationModes: {3}\r\n" +
				"TurnRatio: {4}\r\nRunState: {5}\r\nTachoLimit: {6}\r\nTachoCount: {7}\r\nBlockTachoCount: {8}\r\nRotations: {9}\r\n", 
				this.MotorPort, this.Power, this.MotorModes, this.RegulationMode, this.TurnRatio, this.RunState,
				this.TachoLimit, this.TachoCount, this.BlockTachoCount, this.RotationCount);
		}
	}
}
