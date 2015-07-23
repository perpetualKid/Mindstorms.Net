using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core.Sensors.Nxt
{
	public sealed class ColorChangedEventArgs
#if !WINRT
 : EventArgs
#endif
	{

		internal ColorChangedEventArgs(SensorColor color)
		{
			this.Color = color;
		}

		public SensorColor Color { get; set; }
	}
}
