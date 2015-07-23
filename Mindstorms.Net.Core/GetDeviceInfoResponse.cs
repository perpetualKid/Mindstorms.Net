using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	public sealed class GetDeviceInfoResponse: ResponseBase
	{
		public GetDeviceInfoResponse(ResponseTelegram response)
			: base(response)
		{
		}

		public string Name { get { return Encoding.GetEncoding("ASCII").GetString(response.Data, 3, 14).TrimEnd('\0', '?', ' '); } }

		public string BluetoothAddress { get { return BitConverter.ToString(response.Data, 18, 6); } }

		public uint SignalStrength { get { return BitConverter.ToUInt32(response.Data, 25); } }

		public uint FreeUserFlash { get { return BitConverter.ToUInt32(response.Data, 29); } }


		/// <summary>
		/// Returns all device info in string format.
		/// </summary>
		public override string ToString()
		{
			return string.Format("NXT Device Name: {0}\r\nBluetooth Address: {1}\r\nBT Signal Strenght: {2}\r\nFree User Flash: {3} Bytes\r\n",
				this.Name, this.BluetoothAddress, this.SignalStrength, this.FreeUserFlash);
		}

	}
}
