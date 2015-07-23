using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	public sealed class ReadResponse: ResponseBase
	{
		byte[] data;

		public ReadResponse(ResponseTelegram response)
			: base(response)
		{
			data = new byte[Size];
			Buffer.BlockCopy(response.Data, 6, data, 0, Size);
		}

		public byte Handle { get { return response.Data[3]; } }

		public ushort Size { get { return BitConverter.ToUInt16(response.Data, 4); } }

		public string AsString { get { return Encoding.GetEncoding("ASCII").GetString(data, 0, data.Length).TrimEnd('\0', '?', ' '); } }

		public string AsHexString { get { return BitConverter.ToString(data); } }

		public byte[] AsBytes { get { return data; } }

	}
}
