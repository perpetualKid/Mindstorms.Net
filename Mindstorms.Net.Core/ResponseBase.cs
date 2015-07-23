using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	public class ResponseBase
	{
		protected ResponseTelegram response;

		internal ResponseBase(ResponseTelegram response)
		{
			this.response = response;
		}
	}
}
