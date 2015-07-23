using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Mindstorms.Net.Core;
#if WINRT
using Windows.Foundation;
#endif

namespace Mindstorms.Net.Interfaces
{
	/// <summary>
	/// Interface for communicating with the NXT brick
	/// </summary>
	public interface ICommunication
	{
		/// <summary>
		/// Fired when a full report is ready to parse and process.
		/// </summary>
		event EventHandler<ResponseReceivedEventArgs> ResponseReceived;

		bool Connected { get; }

		/// <summary>
		/// Connect to the NXT brick.
		/// </summary>
#if WINRT
		IAsyncAction ConnectAsync();
#else
		Task ConnectAsync();
#endif
 
		/// <summary>
		/// Disconnect from the NXT brick.
		/// </summary>
#if WINRT
		IAsyncAction DisconnectAsync();
#else
		Task DisconnectAsync();
#endif

		/// <summary>
		/// Write a request to the NXT brick.
		/// </summary>
#if WINRT
		IAsyncAction WriteAsync([ReadOnlyArray]byte[] data);
#else
		Task WriteAsync([ReadOnlyArray]byte[] data);
#endif
		
	}
}
