using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Desktop
{
	public static class ComHelper
	{
		// The WMI query that makes it all happen
		private const string QueryString = "SELECT Caption,PNPDeviceID FROM Win32_PnPEntity " +
									"WHERE ConfigManagerErrorCode = 0 AND " +
									"Caption LIKE 'Standard Serial over Bluetooth link " +
									"(COM%' AND PNPDeviceID LIKE '%&001653%'";

		public static List<string> GetPairedNxtBluetoothCom()
		{
			SelectQuery WMIquery = new SelectQuery(QueryString);
			ManagementObjectSearcher WMIqueryResults = new ManagementObjectSearcher(WMIquery);
			List<string> comPorts = new List<string>();
			if (WMIqueryResults != null)
			{
				foreach (object result in WMIqueryResults.Get())
				{
					ManagementObject mo = (ManagementObject)result;
					object captionObject = mo.GetPropertyValue("Caption");
					object pnpIdObject = mo.GetPropertyValue("PNPDeviceID");

					// Get the COM port name out of the Caption, requires a little work.
					string caption = captionObject.ToString();
					string comPort = caption.Substring(caption.LastIndexOf("(COM")).
										Replace("(", string.Empty).Replace(")", string.Empty);

					comPorts.Add(comPort);
				}
			}
			return comPorts;
		}

		public static Dictionary<string, string> GetPairedNxtBluetoothBTAddress()
		{
			SelectQuery WMIquery = new SelectQuery(QueryString);
			ManagementObjectSearcher WMIqueryResults = new ManagementObjectSearcher(WMIquery);
			Dictionary<string, string> comPorts = new Dictionary<string, string>();
			if (WMIqueryResults != null)
			{
				foreach (object result in WMIqueryResults.Get())
				{
					ManagementObject mo = (ManagementObject)result;
					object captionObject = mo.GetPropertyValue("Caption");
					object pnpIdObject = mo.GetPropertyValue("PNPDeviceID");

					// Get the COM port name out of the Caption, requires a little work.
					string caption = captionObject.ToString();
					string comPort = caption.Substring(caption.LastIndexOf("(COM")).
										Replace("(", string.Empty).Replace(")", string.Empty);

					// Extract the BT address from the PNPObjectID property
					string BTaddress = pnpIdObject.ToString().Split('&')[4].Substring(0, 12);

					comPorts.Add(comPort,BTaddress);
				}
			}
			return comPorts;
		}

	}
}
