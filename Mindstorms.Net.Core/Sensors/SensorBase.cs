
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Mindstorms.Net.Core.Interfaces;
using System.Threading;
#if WINRT
using Windows.Foundation;
#endif

namespace Mindstorms.Net.Core.Sensors
{
	public class SensorBase : IPollable, IDisposable
	{
		protected NxtBrick brick;

		protected SensorMode sensorMode;

		protected SensorType sensorType;
		private CancellationTokenSource tokenSource;

		internal SensorBase(SensorType sensorType, SensorMode sensorMode)
		{
			this.sensorMode = sensorMode;
			this.sensorType = sensorType;
		}

		public SensorPort SensorPort { get; set; }

		internal async Task Attach(NxtBrick brick, SensorPort port)
		{
			await Task.Run(() =>
			{
				this.SensorPort = port;
				this.brick = brick;
			});
		}

		internal async Task DetachAsyncInteral()
		{
			AutoPollInternal(0);
			if (brick.Connected)
				await brick.DirectCommands.SetInputModeAsync(SensorPort, SensorType.NoSensor, SensorMode.Raw);
		}

#if WINRT
		public IAsyncAction Initialize()
#else
		public Task Initialize()
#endif
		{
			return this.InitializeAsyncInternal()
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async virtual Task InitializeAsyncInternal()
		{
			if (brick.Connected)
				await brick.DirectCommands.SetInputModeAsync(SensorPort, sensorType, sensorMode);
		}

#if WINRT
		public IAsyncAction Reset()
#else
		public Task Reset()
#endif
		{
			return this.ResetAsyncInternal()
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async virtual Task ResetAsyncInternal()
		{
			if (brick.Connected)
				await brick.DirectCommands.ResetInputScaledValueAsync(SensorPort);
		}

#if WINRT
		public IAsyncAction Poll()
#else
		public Task Poll()
#endif
		{
			return this.PollAsyncInternal()
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal virtual async Task PollAsyncInternal()
		{
			await Task.Yield();
		}

		public event EventHandler<SensorEventArgs> OnChanged;

		protected virtual void OnChangedEventHandler()
		{
			if (null != OnChanged)
				Task.Run(() => OnChanged(this, new SensorEventArgs()));
		}

		public void AutoPoll(int interval)
		{
			AutoPollInternal(interval);
		}

		protected virtual void AutoPollInternal(int interval)
		{
			if (null != tokenSource)
				tokenSource.Cancel();
			if (interval > 0)
			{
				tokenSource = new CancellationTokenSource();
				Task.Run(async () =>
				{
					while (!tokenSource.IsCancellationRequested)
					{
						await PollAsyncInternal();
						await Task.Delay(interval, tokenSource.Token);
					}
				}, tokenSource.Token).ConfigureAwait(false);
			}
		}

		#region IDisposable Members

		/// <summary>
		/// <c>True</c>, if the class is already disposed and should not be used any more.
		/// </summary>
		private bool disposed;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">
		/// <c>True</c>, if the method is called directly from user code or
		/// <c>false</c>, if the method is called from inside the finalizer.
		/// </param>
		/// <remarks>
		/// This method executes in two distinct scenarios. 
		/// If <paramref name="disposing"/> equals <c>true</c>, the method has been called 
		/// directly or indirectly by a user's code. 
		/// Managed and unmanaged resources can be disposed.
		/// If <paramref name="disposing"/> equals <c>false</c>, the method has been called 
		/// by the runtime from inside the finalizer and you should not reference other objects. 
		/// Only unmanaged resources can be disposed.
		/// </remarks>
		private void Dispose(bool disposing)
		{
			if( !this.disposed )
			{
				if( disposing )
				{
					if(null != this.tokenSource)
					{
						this.tokenSource.Cancel();
						this.tokenSource.Dispose();
					}
				}

				// Free native resources here if necessary.

				// Disposing has been done.
				this.disposed = true;
			}
		}


		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <remarks>
		/// Closes the serial port and disposes the channel used for communication with the NXT.
		/// </remarks>
		public void Dispose()
		{
			this.Dispose(true);

			// This object will be cleaned up by the Dispose method.
			// Take this object off the finalization queue and prevent finalization code 
			// for this object from executing a second time.
			GC.SuppressFinalize(this);
		}


		/// <summary>
		/// Finalizer.
		/// </summary>
		/// <remarks>
		/// Forwards the call to the <see cref="Dispose(bool)"/> method the clean up resources.
		/// </remarks>
		~SensorBase()
		{
			this.Dispose( false );
		}


		#endregion
	}
}
