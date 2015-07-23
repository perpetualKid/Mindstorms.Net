using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	public enum FileTypeExtension
	{
		/// <summary>
		/// NXT Firmware file
		/// </summary>
		rfw,
		/// <summary>
		/// NXT Executable file
		/// </summary>
		rxe,
		/// <summary>
		/// NXT OnBrick-Programming file
		/// </summary>
		rpg,
		/// <summary>
		/// NXT Preloaded Try.Me Program file
		/// </summary>
		rtm,
		/// <summary>
		/// NXT Sound file
		/// </summary>
		rso,
		/// <summary>
		/// NXT Image file
		/// </summary>
		ric,
		/// <summary>
		/// NXT Datalog file
		/// </summary>
		rdt,
		/// <summary>
		/// NXT Internal Firmware file
		/// </summary>
		sys,
		/// <summary>
		/// NXT Sensor Calibration file
		/// </summary>
		cal,
		/// <summary>
		/// ASCII Text file (CR/LF Windows EOL Convention)
		/// </summary>
		txt,
		/// <summary>
		/// ASCII-based Log file
		/// </summary>
		log,
	}
}
