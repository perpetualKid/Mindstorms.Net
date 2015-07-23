using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	public sealed class MessageReadResponse: ResponseBase
	{
		byte[] data;

		internal MessageReadResponse(ResponseTelegram response)
			: base(response)
		{
			data = new byte[response.Data[4]];
			Buffer.BlockCopy(response.Data, 5, data, 0, response.Data[4]);
		}

		public int LocalInboxNumber { get { return Convert.ToInt32(response.Data[3]); } }

		public string AsString { get { return Encoding.GetEncoding("ASCII").GetString(data, 0, data.Length).TrimEnd('\0', '?', ' '); } }

		public string AsHexString { get { return BitConverter.ToString(data); } }

		public byte[] AsBytes { get { return data; }}
	}
}
