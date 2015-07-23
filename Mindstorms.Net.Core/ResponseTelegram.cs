using System;
using System.Text;

namespace Mindstorms.Net.Core
{
	/// <summary>
	/// Event arguments for the ReportReceived event.
	/// </summary>
	public sealed class ResponseTelegram
#if !WINRT
		: EventArgs
#endif
	{

		private byte[] payload;

		public int Size {get; set; }

		public byte[] Data { get; set; }

		public byte[] Payload
		{
			get
			{
				if (null == payload)
				{
					payload = new byte[Data.Length - 3];
					Buffer.BlockCopy(Data, 3, payload, 0, payload.Length);
				}
				return payload;
			}
		}

		public CommandType CommandType { get { return (CommandType)Data[0]; } }

		public NxtCommands RequestorCommand { get { return (NxtCommands)Data[1]; } }
		
		public Error Status { get { return (Error)Data[2]; } }

		public String ErrorMessage { get { return ErrorMessages.Messages[Status]; } }

		#region Direct Command responses
		public static ushort GetBatteryLevel(ResponseTelegram response)
		{
			return BitConverter.ToUInt16(response.Data, 3);
		}

		public static uint KeepAlive(ResponseTelegram response)
		{
			return BitConverter.ToUInt32(response.Data, 3);
		}

		public static string GetCurrentProgramName(ResponseTelegram response)
		{
			return Encoding.GetEncoding("ASCII").GetString(response.Payload, 0, response.Payload.Length).TrimEnd('\0', '?', ' ');
		}

		public static MessageReadResponse MessageRead(ResponseTelegram response)
		{
			return new MessageReadResponse(response);
		}

		public static GetOutputStateResponse GetOutputState(ResponseTelegram response)
		{
			return new GetOutputStateResponse(response);
		}

		public static GetInputValuesResponse GetInputValues(ResponseTelegram response)
		{ 
			return new GetInputValuesResponse(response); 
		}

		public static LowSpeedReadResponse LowSpeedRead(ResponseTelegram response)
		{
			return new LowSpeedReadResponse(response);
		}
		#endregion

		#region System Command responses
		public static GetFirmwareVersionResponse GetFirmwareVersion(ResponseTelegram response)
		{
			return new GetFirmwareVersionResponse(response);
		}

		public static string Boot(ResponseTelegram response)
		{
			return Encoding.GetEncoding("ASCII").GetString(response.Payload, 0, response.Payload.Length).TrimEnd('\0', '?', ' ');
		}

		public static GetDeviceInfoResponse GetDeviceInfo(ResponseTelegram response)
		{
			return new GetDeviceInfoResponse(response);
		}

		public static FindFileResponse FindFile(ResponseTelegram response)
		{
			return new FindFileResponse(response);
		}

		public static byte Close(ResponseTelegram response)
		{
			return response.Data[3];
		}

		public static OpenReadResponse OpenRead(ResponseTelegram response)
		{
			return new OpenReadResponse(response);
		}

		public static ReadResponse Read(ResponseTelegram response)
		{
			return new ReadResponse(response);
		}

		public static byte OpenWrite(ResponseTelegram response)
		{
			return response.Data[3];
		}

		public static WriteResponse Write(ResponseTelegram response)
		{
			return new WriteResponse(response);
		}

		public static OpenDataAppendResponse OpenDataAppend(ResponseTelegram response)
		{
			return new OpenDataAppendResponse(response);
		}

		public static byte OpenDataWrite(ResponseTelegram response)
		{
			return response.Data[3];
		}

		public static uint OpenLinearRead(ResponseTelegram response)
		{
			return BitConverter.ToUInt32(response.Data, 3);
		}

		public static byte OpenLinearWrite(ResponseTelegram response)
		{
			return response.Data[3];
		}

		public static string Delete(ResponseTelegram response)
		{
			return Encoding.GetEncoding("ASCII").GetString(response.Payload, 0, response.Payload.Length).TrimEnd('\0', '?', ' ');
		}

		public static PollCommandLengthResponse PollCommandLength(ResponseTelegram response)
		{
			return new PollCommandLengthResponse(response);
		}

		public static PollCommandResponse PollCommand(ResponseTelegram response)
		{
			return new PollCommandResponse(response);
		}

		#endregion
	}
}
