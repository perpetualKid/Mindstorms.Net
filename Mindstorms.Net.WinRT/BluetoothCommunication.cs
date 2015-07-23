using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mindstorms.Net.Core;
using Mindstorms.Net.Interfaces;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.System.Threading;

namespace Mindstorms.Net.WinRT
{
	public sealed class BluetoothCommunication: ICommunication
	{
		public event EventHandler<ResponseReceivedEventArgs> ResponseReceived;

		private StreamSocket socket;
		private DataReader dataReader;
		private CancellationTokenSource tokenSource;

		private readonly string deviceName;

		/// <summary>
		/// Create a new NetworkCommunication object
		/// </summary>
		/// <param name="device">Devicename of the NXT brick</param>
		public BluetoothCommunication(string deviceName)
		{
			this.deviceName = deviceName;
		}

		public bool Connected
		{
			get { return !tokenSource.IsCancellationRequested && socket != null; }
		}

		public IAsyncAction ConnectAsync()
		{
			return ConnectAsyncInternal().AsAsyncAction();
		}

		/// <summary>
		/// Disconnect from the NEXT brick.
		/// </summary>
		public IAsyncAction DisconnectAsync()
		{
			return DisconnectAsyncInternal().AsAsyncAction();
		}


		/// <summary>
		/// Write data to the NEXT brick.
		/// </summary>
		/// <param name="data">Byte array to write to the NXT brick.</param>
		/// <returns></returns>
		public IAsyncAction WriteAsync([ReadOnlyArray]byte[] data)
		{
			return WriteAsyncInternal(data).AsAsyncAction();
		}


		private async Task ConnectAsyncInternal()
		{
			this.tokenSource = new CancellationTokenSource();

			string selector = RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort);
			DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);
			DeviceInformation device = (from d in devices where d.Name == deviceName select d).LastOrDefault();
			if (device == null)
				throw new Exception("NXT brick not found.");

			RfcommDeviceService service = await RfcommDeviceService.FromIdAsync(device.Id);
			if (service == null)
				throw new Exception("Unable to connect to Mindstorms NXT brick...make sure the manifest is set properly.");

			socket = new StreamSocket();
			await socket.ConnectAsync(service.ConnectionHostName, service.ConnectionServiceName,
				 SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);

			dataReader = new DataReader(socket.InputStream);
			dataReader.ByteOrder = ByteOrder.LittleEndian;

			await ThreadPool.RunAsync(PollInput);
		}

		private async Task DisconnectAsyncInternal()
		{
			await Task.Run(() =>
				{
					tokenSource.Cancel();
					if (dataReader != null)
					{
						dataReader.DetachStream();
						dataReader = null;
					}

					if (socket != null)
					{
						socket.Dispose();
						socket = null;
					}
				});
		}

		private async void PollInput(IAsyncAction operation)
		{
			while (this.socket != null)
			{
				try
				{
					DataReaderLoadOperation loadOperation = dataReader.LoadAsync(2);
					await loadOperation.AsTask(tokenSource.Token);
					short size = dataReader.ReadInt16();

					byte[] buffer = new byte[size];

					loadOperation = dataReader.LoadAsync((uint)size);
					await loadOperation.AsTask(tokenSource.Token);
					dataReader.ReadBytes(buffer);

					if (ResponseReceived != null)
						ResponseReceived(this, new ResponseReceivedEventArgs()
						{
							ResponseTelegram = new ResponseTelegram()
							{
								Data = buffer,
								Size = size
							}
						});
				}
				catch (TaskCanceledException)
				{
					return;
				}
			}
		}

		private async Task WriteAsyncInternal(byte[] data)
		{
			if (socket != null)
				await socket.OutputStream.WriteAsync(data.AsBuffer());
		}

	}
}
