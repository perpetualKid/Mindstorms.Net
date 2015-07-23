using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	/// <summary>
	/// Available motor modes. This property is a bitfield that can include any combination of the flag bits.
	/// </summary>
	/// <remarks>
	/// 0x00- Coast mode. Motor that is connected to the specified port will rotate freely.
	/// Use the coast mode to release the motor locking for letting the motor to rotate from external source alone (manually).
	/// The Motor power set point value will be ignored.
	/// 0x01- Motor ON mode. Motor that is connected to the specified port will rotate according to the value specified in the motor power set point value.
	/// Note- the actual speed can be effected by external physical load on the motor and can be different from the motor power set point value.
	/// 0x02- Break mode alone. Nothing will happen. Don’t use this value.
	/// 0x03- Motor ON with Break mode. Use this mode to improve the accuracy of motor output.
	/// Note- this mode uses more battery power, and the actual speed can be affected by external physical load on the motor and can be different from the motor power set point value.
	/// 0x04- Regulated mode alone. Nothing will happen. Don’t use this value.
	/// 0x05- Motor ON with Regulated mode. Turn on the regulation mode that will be specified in the Regulation mode byte.
	/// This mode gives NXT firmware the ability to adjust the speed (add more power to the motors) according to the external physical load on the motors.
	/// This mode is used to synchronize two motors when it is important the vehicle to drive straight no matter the surface it is driving on, or to synchronize the specified speed (motor power set point) to be constant no matter the physical overload on the motor.
	/// The regulation synchronize mode will be configured in Regulation Mode byte.
	/// In case of using the regulation mode to synchronize two motors with separate SetOutputState commands, make sure to use either the 0x05 or 0x07 mode on the both commands.
	/// 0x07- Motor ON with Break and Regulated mode- This mode is used for regulated mode with accuracy of motor output.
	/// The actual speed will not be affected by external physical load on the motor.
	/// The regulation synchronize mode will be configured in Regulation Mode byte.
	/// In case of using the regulation mode to synchronize two motors with separate SetOutputState commands, make sure to use either the 0x05 or 0x07 mode on the both commands.
	/// Note- this mode uses more battery power.
	/// </remarks>
	[Flags]
	public enum MotorModes
	{
		/// <summary>
		/// Motors connected to the specified port(s) will rotate freely.
		/// </summary>
		Coast = 0x00,

		/// <summary>
		/// Turns on the specified motor. Enables pulse-width modulation (PWM) power to port(s) 
		/// according to the specified power value.
		/// </summary>
		On = 0x01,

		/// <summary>
		/// Break the motor after the action is completed. Applies electronic braking to port(s).
		/// </summary>
		Brake = 0x02,

		/// <summary>
		/// Turns on regulation.
		/// </summary>
		Regulated = 0x04

	}
}
