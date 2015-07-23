using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	/// <summary>
	/// Available motor regulation modes.
	/// </summary>
	/// <remarks>
	/// 0x00- No regulation will be enabled. Use this value if the Motor mode value is 0x00 (Coast mode), 0x01 (Motor ON mode) or 0x03 (Motor ON with Break mode);
	/// 0x01- Speed regulation. Use this regulation mode if the Motor mode value is 0x05 (Motor ON and Regulated mode) or 0x07 (Motor ON with Break and Regulated mode), and you would like the NXT firmware automatically adjust the speed to the specified motor power set point value no matter the physical overload on the motor.
	/// 0x02- Two motors synchronization regulation. Use this regulation mode if the Motor mode value is 0x05 (Motor ON and Regulated mode) or 0x07 (Motor ON with Break and Regulated mode), and you would like the NXT firmware automatically keep the two motors in sync regardless of varying physical loads.
	/// Synchronization of the two motors is highly used when the NXT vehicle should drive straight.
	/// In case of using separate SetOutputState commands for each motor, make sure to configure both motors to 0x02 as a regulation mode.
	/// The synchronization regulation (0x02) of the third motor will be ignored.
	/// The minimum Motor power set point for motor synchronization regulation is 75% or higher or -75% or lower.
	/// </remarks>
	public enum MotorRegulationMode
	{
		/// <summary>
		/// No regulation will be enabled.
		/// </summary>
		Idle = 0x00,

		/// <summary>
		/// Power control will be enabled on the specified output.
		/// </summary>
		/// <remarks>
		/// Speed regulation means that the NXT firmware attempts to 
		/// maintain a certain speed according to the <c>power</c> set-point
		/// To accomplish this, the NXT firmware automatically adjusts the actual 
		/// PWM duty cycle if the motor is affected by a physical load.
		/// </remarks>
		Speed = 0x01,

		/// <summary>
		/// Synchronization will be enabled (needs enabled on two output).
		/// </summary>
		/// <remarks>
		/// Synchronization means that the firmware attempts keep any two motors in synch 
		/// regardless of varying physical loads. You typically use this mode is to maintain 
		/// a straight path for a vehicle robot automatically. You also can use this mode with 
		/// the <c>turnRatio</c> parameter property to provide proportional turning. 
		/// You must set <c>Sync</c> on at least two motor ports to have the desired affect. 
		/// If <c>Sync</c> is set on all three motor ports, only the first two (A and B) are synchronized.
		/// </remarks>
		Sync = 0x02
	}
}
