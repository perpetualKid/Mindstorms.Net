using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	public sealed class GetFirmwareVersionResponse: ResponseBase
	{
		internal GetFirmwareVersionResponse(ResponseTelegram response): 
			base(response)
		{

		}

		public string ProtocolVersion { get { return String.Format(CultureInfo.InvariantCulture, "{0}.{1}", response.Data[4], response.Data[3]); } }

		public string FirmwareVersion { get { return String.Format(CultureInfo.InvariantCulture, "{0}.{1}", response.Data[6], response.Data[5]); } }

		/// <summary>
		/// Returns the complete firmware version in string format.
		/// </summary>
		public override string ToString()
		{
			return string.Format("Firmware Version: {0}\r\nProtocol Version: {1}\r\n", this.FirmwareVersion, this.ProtocolVersion);
		}

	}
}
