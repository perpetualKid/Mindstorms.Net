using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINRT
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using System.Runtime.InteropServices.WindowsRuntime;
#endif

namespace Mindstorms.Net.Core.Commands
{
	public sealed partial class SystemCommands: CommandsBase
	{
		internal SystemCommands(NxtBrick brick) :
			base(brick)
		{

		}

		internal async Task<GetFirmwareVersionResponse> GetFirmwareVersionAsyncInternal()
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.GetFirmwareVersion());
			return ResponseTelegram.GetFirmwareVersion(brick.LastResponse);
		}

		internal async Task<string> BootAsyncInternal()
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.Boot());
			return ResponseTelegram.Boot(brick.LastResponse);
		}

		internal async Task SetBrickNameAsyncInternal(string name)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.SetBrickName(name));
		}

		internal async Task<GetDeviceInfoResponse> GetDeviceInfoAsyncInternal()
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.GetDeviceInfo());
			return ResponseTelegram.GetDeviceInfo(brick.LastResponse);
		}

		internal async Task<FindFileResponse> FindFirstAsyncInternal(string fileNamePattern)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.FindFirst(fileNamePattern));
			return ResponseTelegram.FindFile(brick.LastResponse);
		}

		internal async Task<FindFileResponse> FindFirstAsyncInternal(FileTypeExtension fileType)
		{
			return await FindFirstAsyncInternal("*." + fileType.ToString());
		}

		internal async Task<FindFileResponse> FindNextAsyncInternal(byte handle)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.FindNext(handle));
			return ResponseTelegram.FindFile(brick.LastResponse);
		}

		internal async Task<byte> CloseAsyncInternal(byte handle)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.Close(handle));
			return ResponseTelegram.Close(brick.LastResponse);
		}

		internal async Task<OpenReadResponse> OpenReadAsyncInternal(string fileName)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.OpenRead(fileName));
			return ResponseTelegram.OpenRead(brick.LastResponse);
		}

		internal async Task<ReadResponse> ReadAsyncInternal(byte handle, ushort length)
		{ 
			await brick.SendCommandAsyncInternal(RequestTelegram.Read(handle, length));
			return ResponseTelegram.Read(brick.LastResponse);
		}

		internal async Task<byte> OpenWriteAsyncInternal(string fileName, uint fileSize)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.OpenWrite(fileName, fileSize));
			return ResponseTelegram.OpenWrite(brick.LastResponse);
		}

#if WINRT
		internal async Task<WriteResponse> WriteAsyncInternal(byte handle, IBuffer data)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.Write(handle, data.ToArray()));
			return ResponseTelegram.Write(brick.LastResponse);
		}
#endif

		internal async Task<WriteResponse> WriteAsyncInternal(byte handle, byte[] data)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.Write(handle, data));
			return ResponseTelegram.Write(brick.LastResponse);
		}

		internal async Task<WriteResponse> WriteAsyncInternal(byte handle, string data)
		{
			return await WriteAsyncInternal(handle, Encoding.GetEncoding("ASCII").GetBytes(data));
		}

		internal async Task<byte> OpenDataWriteAsyncInternal(string fileName, uint fileSize)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.OpenDataWrite(fileName, fileSize));
			return ResponseTelegram.OpenDataWrite(brick.LastResponse);
		}

		internal async Task<OpenDataAppendResponse> OpenDataAppendAsyncInternal(string fileName)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.OpenDataAppend(fileName));
			return ResponseTelegram.OpenDataAppend(brick.LastResponse);
		}

		internal async Task<uint> OpenLinearReadAsyncInternal(string fileName)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.OpenLinearRead(fileName));
			return ResponseTelegram.OpenLinearRead(brick.LastResponse);
		}

		internal async Task<byte> OpenLinearWriteAsyncInternal(string fileName, uint fileSize)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.OpenLinearWrite(fileName, fileSize));
			return ResponseTelegram.OpenLinearWrite(brick.LastResponse);
		}

		internal async Task<string> DeleteAsyncInternal(string fileName)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.Delete(fileName));
			return ResponseTelegram.Delete(brick.LastResponse);
		}

		internal async Task DeleteUserFlashAsyncInternal()
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.DeleteUserFlash(), 3000);	//this might take up to 3 seconds to cleanup
		}

		internal async Task<PollCommandLengthResponse> PollCommandLengthAsyncInternal(PollCommandBuffer buffer)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.PollCommandLength(buffer));
			return ResponseTelegram.PollCommandLength(brick.LastResponse);
		}

		internal async Task<PollCommandResponse> PollCommandAsyncInternal(PollCommandBuffer buffer, byte commandLength)
		{
			await brick.SendCommandAsyncInternal(RequestTelegram.PollCommand(buffer, commandLength));
			return ResponseTelegram.PollCommand(brick.LastResponse);
		}
	}
}
