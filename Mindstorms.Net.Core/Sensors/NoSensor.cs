using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core.Sensors
{
	internal sealed class NoSensor : SensorBase
	{
		public NoSensor()
			: base(SensorType.NoSensor, SensorMode.Raw)
		{
		}

		internal override async Task InitializeAsyncInternal()
		{
			await Task.Yield();
		}
	}
}
