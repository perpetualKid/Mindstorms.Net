using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINRT
using Windows.Foundation;
#endif

namespace Mindstorms.Net.Core.Commands
{
    public sealed partial class ConnectionCommands : CommandsBase
	{
		#region Connection
		/// <summary>
		/// Connect to the NXT brick.
		/// </summary>
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		ConnectAsync()
		{
			return ConnectAsyncInternal()
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		/// <summary>
		/// Disconnect from the NXT brick
		/// </summary>
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		DisconnectAsync()
		{
			return DisconnectAsyncInternal()
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

	}
}
