using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	public sealed class OpenReadResponse: ResponseBase
	{
		public OpenReadResponse(ResponseTelegram response)
			: base(response)
		{ }

		public byte Handle { get { return response.Data[3]; } }

		public uint FileSize { get { return BitConverter.ToUInt32(response.Data, 4); } }

	}
}
