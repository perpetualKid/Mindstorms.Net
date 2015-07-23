using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mindstorms.Net.Core;
using Mindstorms.Net.Interfaces;

namespace Mindstorms.Net.Desktop
{
	public class BluetoothCommunication: ICommunication
	{
		/// <summary>
		/// Event fired when a complete report is received from the NXT brick.
		/// </summary>
		public event EventHandler<ResponseReceivedEventArgs> ResponseReceived;

		private SerialPort serialPort;
		private BinaryReader reader;
		private SemaphoreSlim writeLock;

		/// <summary>
		/// Initialize a BluetoothCommunication object.
		/// </summary>
		/// <param name="port">The COM port on which to connect.</param>
		public BluetoothCommunication(string port)
		{
			this.serialPort = new SerialPort(port, 115200);
			this.writeLock = new SemaphoreSlim(1);
		}

		public bool Connected
		{
			get { return serialPort.IsOpen; }
		}


		/// <summary>
		/// Connect to the NXT brick.
		/// </summary>
		/// <returns></returns>
		public async Task ConnectAsync()
		{
			await Task.Run(() =>
			{
				this.serialPort.DataReceived += SerialPortDataReceived;
				this.serialPort.Open();

				this.reader = new BinaryReader(serialPort.BaseStream);

			});
		}

		/// <summary>
		/// Disconnect from the NXT brick.
		/// </summary>
		public async Task DisconnectAsync()
		{
			if (this.serialPort != null)
			{
				this.serialPort.DataReceived -= SerialPortDataReceived;
				this.serialPort.Close();
				this.serialPort = null;
			}
			await Task.FromResult(true);
		}

		/// <summary>
		/// Write data to the NXT brick.
		/// </summary>
		/// <param name="data">Byte array of data to send to the NXT brick.</param>
		/// <returns></returns>
		public async Task WriteAsync(byte[] data)
		{
#if DEBUG
			System.Diagnostics.Debug.WriteLine(">> " + BitConverter.ToString(data));
#endif
			await Task.Run(async () =>
			{
				if (!this.serialPort.IsOpen)
					return;

				await writeLock.WaitAsync();
				this.serialPort.Write(data, 0, data.Length);
				writeLock.Release();
			});
		}

		private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			if (e.EventType == SerialData.Chars)
			{
				byte[] buffer = reader.ReadBytes(2);
				int size = (buffer[0] | (buffer[1] << 8));
				buffer = this.reader.ReadBytes(size);
#if DEBUG
				System.Diagnostics.Debug.WriteLine("<< " + BitConverter.ToString(buffer));
#endif

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
		}


	}
}
