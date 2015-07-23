using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	public sealed class FindFileResponse: ResponseBase
	{
		public FindFileResponse(ResponseTelegram response)
			: base(response)
		{
		}

		public byte Handle { get { return response.Data[3]; } }

		public string FileName { get { return Encoding.GetEncoding("ASCII").GetString(response.Data, 4, 20).TrimEnd('\0', '?', ' '); } }

		public uint FileSize { get { return BitConverter.ToUInt32(response.Data, 24); } }

		public bool NoMoreFile { get { return response.Status == Error.FileNotFound; } }
	}
}
