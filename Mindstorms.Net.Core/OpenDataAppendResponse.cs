using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	public sealed class OpenDataAppendResponse: ResponseBase
	{
		public OpenDataAppendResponse(ResponseTelegram response)
			: base(response)
		{
		}

		public byte Handle { get { return response.Data[3]; } }

		public ushort Size { get { return BitConverter.ToUInt16(response.Data, 4); } }

	}
}
