using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	public sealed class LowSpeedReadResponse: ResponseBase
	{
		byte[] data;

		internal LowSpeedReadResponse(ResponseTelegram response)
			: base(response)
		{
			data = new byte[16];
			Buffer.BlockCopy(response.Data, 4, data, 0, response.Data.Length == 20 ? 16 : response.Data.Length - 4);
		}

		public string AsString { get { return Encoding.GetEncoding("ASCII").GetString(data, 0, data.Length).TrimEnd('\0', '?', ' '); } }

		public string AsHexString { get { return BitConverter.ToString(data); } }

		public byte[] AsBytes { get { return data; }}

		public byte Length { get { return response.Data[3]; } }
	}
}
