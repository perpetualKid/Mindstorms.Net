using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core.Sensors
{
	public sealed class SensorEventArgs
#if !WINRT
	: EventArgs
#endif
	{
	}

}
