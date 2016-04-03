using Mindstorms.Net.Core.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINRT
using Windows.Foundation;
#endif

namespace Mindstorms.Net.Core.Interfaces
{
	public interface IPollable
	{
#if WINRT
		IAsyncAction Poll();
#else
		Task Poll();
#endif

		void AutoPoll(int interval);

		event EventHandler<SensorEventArgs> OnChanged;

	}
}
