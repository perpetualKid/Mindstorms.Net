using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	public sealed class PollCommandResponse: ResponseBase
	{
		byte[] data;

		public PollCommandResponse(ResponseTelegram response)
			: base(response)
		{
			data = new byte[Length];
			Buffer.BlockCopy(response.Data, 5, data, 0, Length);

		}

		public PollCommandBuffer BufferNumber { get { return (PollCommandBuffer)response.Data[3]; } }

		public byte Length { get { return response.Data[4]; } }

		public string AsString { get { return Encoding.GetEncoding("ASCII").GetString(data, 0, data.Length).TrimEnd('\0', '?', ' '); } }

		public string AsHexString { get { return BitConverter.ToString(data); } }


	}
}
