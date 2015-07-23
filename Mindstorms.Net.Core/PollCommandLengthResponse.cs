using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	public sealed class PollCommandLengthResponse: ResponseBase
	{
		public PollCommandLengthResponse(ResponseTelegram response)
			: base(response)
		{
		}

		public PollCommandBuffer BufferNumber { get { return (PollCommandBuffer)response.Data[3]; } }

		public byte PollCommandLength { get { return response.Data[4]; } }
	}
}
