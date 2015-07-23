using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mindstorms.Net.Core.Commands;
using Mindstorms.Net.Core.Sensors;
using Mindstorms.Net.Interfaces;
#if WINRT
using Windows.Foundation;
using Windows.Storage.Streams;
#endif

namespace Mindstorms.Net.Core
{
	public sealed class NxtBrick: IDisposable
	{

		private ICommunication channel;
		private readonly SynchronizationContext context;
		private CancellationTokenSource tokenSource;
		private ManualResetEvent dataReceivedEvent;
		private SemaphoreSlim channelMonitor;

		private readonly ConnectionCommands connectionCommands;
		private readonly DirectCommands directCommands;
		private readonly SystemCommands systemCommands;

		private ResponseTelegram currentResponse;
		private RequestTelegram lastRequest;

		private Queue<ResponseTelegram> responses;

		private Dictionary<SensorPort, SensorBase> sensors;

		public NxtBrick(ICommunication channel)
		{
			this.channel = channel;
			this.channel.ResponseReceived += Channel_ResponseReceived;
			this.context = SynchronizationContext.Current;
			this.tokenSource = new CancellationTokenSource();
			this.dataReceivedEvent = new ManualResetEvent(false);
			this.responses = new Queue<ResponseTelegram>();
			this.channelMonitor = new SemaphoreSlim(1);

			this.connectionCommands = new ConnectionCommands(this);
			this.directCommands = new DirectCommands(this);
			this.systemCommands = new SystemCommands(this);

			this.sensors = new Dictionary<SensorPort, SensorBase>();

		}

		public ConnectionCommands ConnectionCommands { get { return this.connectionCommands; } }

		public DirectCommands DirectCommands { get { return this.directCommands; } }

		public SystemCommands SystemCommands { get { return this.systemCommands; } }

		public bool Connected { get { return channel.Connected; } }

#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		AttachSensorAsync(SensorPort port, SensorBase sensor)
		{
			return AttachSensorAsyncInternal(port, sensor)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async Task AttachSensorAsyncInternal(SensorPort port, SensorBase sensor)
		{
			await sensor.Attach(this, port);
			sensors[port] = sensor;
			await InitializeSensorAsyncInternal(port);
		}

#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		InitializeSensorAsync(SensorPort port)
		{
			return InitializeSensorAsyncInternal(port)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async Task InitializeSensorAsyncInternal(SensorPort port)
		{
			await sensors[port].Initialize();
		}

#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		DetachSensorAsync(SensorPort port)
		{
			return DetachSensorAsyncInternal(port)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}

		internal async Task DetachSensorAsyncInternal(SensorPort port)
		{
			//Detach current sensor
			await sensors[port].DetachAsyncInteral();
			//
			await AttachSensorAsyncInternal(port, new NoSensor());
		}

		public bool ThrowErrorsAsException { get; set; }

		internal ICommunication Channel { get { return this.channel; } }

		internal CancellationTokenSource TokenSource { get { return this.tokenSource; } }

		public ResponseTelegram LastResponse { get { return this.currentResponse; } }

		public RequestTelegram LastRequest { get { return this.lastRequest; } }

		private void Channel_ResponseReceived(object sender, ResponseReceivedEventArgs e)
		{
			if (e.ResponseTelegram == null || e.ResponseTelegram.Size < 3 )
				return;

			this.currentResponse = e.ResponseTelegram;

			dataReceivedEvent.Set();
		}

		internal async Task SendCommandAsyncInternal(RequestTelegram command, int timeout = 1000)
		{
			if (this.channel == null || !this.channel.Connected)
			{
				throw new InvalidOperationException("Data channel is not open!");
			}


			await channelMonitor.WaitAsync();

			this.lastRequest = command;
			dataReceivedEvent.Reset();

			await channel.WriteAsync(command.ToBytes());

			if (command.CommandType == CommandType.DirectCommandResponseRequired ||
				command.CommandType == CommandType.SystemCommandResponseRequired)
			{
				await Task.Run(() =>
				{
					if (dataReceivedEvent.WaitOne(timeout))
					{
						// Parse the rest of the message if the size is correct.
						if (currentResponse.Size == currentResponse.Data.Length)
						{
							// If the first byte in the reply is not 0x02 (reply command) - throw exception.
							if (currentResponse.CommandType != CommandType.ReplyCommand)
							{
								throw new InvalidDataException("Unexpected return message type: " + Enum.GetName(typeof(CommandType), (CommandType)currentResponse.Data[0]) ?? "Null");
							}
							// If the second byte in the reply does not match the command sent - throw exception.
							else if (currentResponse.RequestorCommand != LastRequest.Command)
							{
								throw new InvalidDataException(string.Format("Unexpected return command. Expected {0}. Received {1}",
									Enum.GetName(typeof(NxtCommands), lastRequest.Command) ?? "Null",
									Enum.GetName(typeof(NxtCommands), (NxtCommands)currentResponse.Data[1]) ?? "Null"));
							}
							if (currentResponse.Status != Error.Success && this.ThrowErrorsAsException)
								throw new InvalidOperationException(string.Format("Failed response. Error Code {0} : {1}",
									BitConverter.ToString(new byte[] { currentResponse.Data[2] }), ErrorMessages.Messages[currentResponse.Status]));
						}
						else
						{
							throw new InvalidDataException("Invalid message size: " + currentResponse.Size.ToString(CultureInfo.InvariantCulture));
						}
					}
					else
					{
						throw new TimeoutException("Timeout by waiting for the response!");
					}
				});
			}
			channelMonitor.Release();
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
					// Dispose managed resources.
					if (null != this.channel)
					{
						Task.Run(() => this.channel.DisconnectAsync());
						this.channel = null;
					}
					if(null != this.tokenSource)
					{
						this.tokenSource.Cancel();
						this.tokenSource.Dispose();
					}
					if (null != this.channelMonitor)
						this.channelMonitor.Dispose();

					if(null != this.dataReceivedEvent)
					{
						this.dataReceivedEvent.Dispose();
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
			this.Dispose( true );

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
		~NxtBrick()
		{
			this.Dispose(false);
		}


		#endregion
	}
}
