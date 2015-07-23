using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	/// <summary>
	/// Available motor run states.
	/// </summary>
	/// <remarks>
	/// <see cref="Running"/> is the most common setting. Use <see cref="Running"/> to enable power to any
	/// output device connected to the specified port(s).
	/// 
	/// <see cref="RampUp"/> enables automatic ramping to a new speed set-point that is greater 
	/// than the current speed set-point. When you use <see cref="RampUp"/> in conjunction with appropriate
	/// <c>tachoLimit</c> and <c>power</c> values, the NXT firmware attempts to increase the actual power 
	/// smoothly to the speed set-point over the number of degrees specified by <c>tachoLimit</c>.
	/// 
	/// <see cref="RampDown"/> enables automatic ramping to a new speed set-point that is less than the
	/// current speed set-point. When you use <see cref="RampDown"/> in conjunction with appropriate
	/// <c>tachoLimit</c> and <c>power</c> values, the NXT firmware attempts to smoothly decrease the actual power 
	/// to the speed set-point over the number of degrees specified by <c>tachoLimit</c>.
	/// </remarks>
	public enum MotorRunState
	{
		/// <summary>
		/// Disables power to the specified port, output will be idle.
		/// </summary>
		Idle = 0x00,

		/// <summary>
		/// Output will ramp-up to the speed set-point.
		/// </summary>
		RampUp = 0x10,

		/// <summary>
		/// Output will be running.
		/// </summary>
		Running = 0x20,

		/// <summary>
		/// Output will ramp-down to the speed set-point.
		/// </summary>
		RampDown = 0x40
	}
}
