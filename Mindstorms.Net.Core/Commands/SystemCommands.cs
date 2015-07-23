using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINRT
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Foundation.Metadata;
#endif

namespace Mindstorms.Net.Core.Commands
{
	public sealed partial class SystemCommands: CommandsBase
	{
		#region GetFirmwareVersion
		/// <summary>
		/// Returns the current version of the firmware and the protocol used by the NXT.
		/// </summary>
#if WINRT
		public IAsyncOperation<GetFirmwareVersionResponse>
#else
		public Task<GetFirmwareVersionResponse>
#endif
		GetFirmwareVersionAsync()
		{
			return GetFirmwareVersionAsyncInternal()
#if WINRT
			.AsAsyncOperation<GetFirmwareVersionResponse>()
#endif
;
		}
		#endregion

		#region Boot
		/// <summary>
		/// Boot Command for NXT
		/// This will work over USB only!
		/// </summary>
#if WINRT
		public IAsyncOperation<string>
#else
		public Task<string>
#endif
 BootAsync()
		{
			return BootAsyncInternal()
#if WINRT
			.AsAsyncOperation<string>()
#endif
				;
		}
		#endregion

		#region SetBrickName
		/// <summary>
		/// Sets the NXT brick name to the specified value.
		/// </summary>
		/// <param name="name">The new name of the brick to set. Maximum 14 characters.</param>
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
 SetBrickNameAsync(string name)
		{
			return SetBrickNameAsyncInternal(name)
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region GetDeviceInfo
		/// <summary>
		/// Returns general info about the NXT.
		/// </summary>
#if WINRT
		public IAsyncOperation<GetDeviceInfoResponse>
#else
		public Task<GetDeviceInfoResponse>
#endif
		GetDeviceInfoAsync()
		{
			return GetDeviceInfoAsyncInternal()
#if WINRT
			.AsAsyncOperation<GetDeviceInfoResponse>()
#endif
			;
		}
		#endregion

		#region FindFirst
		/// <summary>
		/// Lists files in the NXT brick according to the file mask.
		/// </summary>
#if WINRT
		[DefaultOverload()]
		public IAsyncOperation<FindFileResponse>
#else
		public Task<FindFileResponse>
#endif
		FindFirstAsync(string fileNamePattern)
		{
			return FindFirstAsyncInternal(fileNamePattern)
#if WINRT
			.AsAsyncOperation<FindFileResponse>()
#endif
			;
		}

		/// <summary>
		/// Lists files in the NXT brick according to the file extension.
		/// </summary>
#if WINRT
		public IAsyncOperation<FindFileResponse>
#else
		public Task<FindFileResponse>
#endif
 FindFirstAsync(FileTypeExtension fileExtension)
		{
			return FindFirstAsyncInternal(fileExtension)
#if WINRT
			.AsAsyncOperation<FindFileResponse>()
#endif
			;
		}
		#endregion

		#region FindNext
		/// <summary>
		/// Lists next file in the NXT current search identified by Handle-parameter.
		/// </summary>
		/// <param name="handle"></param>
#if WINRT
		public IAsyncOperation<FindFileResponse>
#else
		public Task<FindFileResponse>
#endif
		FindNextAsync(byte handle)
		{
			return FindNextAsyncInternal(handle)
#if WINRT
			.AsAsyncOperation<FindFileResponse>()
#endif
;
		}
		#endregion

		#region Close
		/// <summary>
		/// Closes a given handle.
		/// </summary>
		/// <param name="handle"></param>
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		CloseAsync(byte handle)
		{
			return CloseAsyncInternal(handle)
#if WINRT
			.AsAsyncOperation<byte>()
#endif
			;
		}
		#endregion

		#region OpenRead
		/// <summary>
		/// This command opens an existing non linear file on NXT's brick 
		/// specified by file name for reading operations from the beginning of the file.
		/// </summary>
		/// <param name="fileName"></param>
#if WINRT
		public IAsyncOperation<OpenReadResponse>
#else
		public Task<OpenReadResponse>
#endif
		OpenReadAsync(string fileName)
		{
			return OpenReadAsyncInternal(fileName)
#if WINRT
			.AsAsyncOperation<OpenReadResponse>()
#endif
			;
		}
		#endregion

		#region Read
		/// <summary>
		/// This command reads n bytes from a previously openend non linear file
		/// specified by handle 
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="length"></param>
#if WINRT
		public IAsyncOperation<ReadResponse>
#else
		public Task<ReadResponse>
#endif
		ReadAsync(byte handle, ushort length)
		{
			return ReadAsyncInternal(handle, length)
#if WINRT
			.AsAsyncOperation<ReadResponse>()
#endif
			;
		}
		#endregion

		#region Delete
		/// <summary>
		/// This command deletes the file specified by filename parameter
		/// </summary>
		/// <param name="fileName"></param>
		/// <summary>
#if WINRT
		public IAsyncOperation<string>
#else
		public Task<string>
#endif
		DeleteAsync(string fileName)
		{
			return DeleteAsyncInternal(fileName)
#if WINRT
			.AsAsyncOperation<string>()
#endif
			;
		}
		#endregion

		#region OpenWrite
		/// <summary>
		/// Creates and opens a nonlinear file to write non textual data
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fileSize"></param>
		/// <returns></returns>
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		OpenWriteAsync(string fileName, uint fileSize)
		{
			return OpenWriteAsyncInternal(fileName, fileSize)
#if WINRT
			.AsAsyncOperation<byte>()
#endif
			;
		}
		#endregion

		#region Write
		/// <summary>
		/// Writes data from buffer to the previously opened file specified by handle
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="data"></param>
		/// <returns></returns>
#if WINRT
		[DefaultOverload()]
		public IAsyncOperation<WriteResponse>
#else
		public Task<WriteResponse>
#endif
		WriteAsync(byte handle, string data)
		{
			return WriteAsyncInternal(handle, data)
#if WINRT
			.AsAsyncOperation<WriteResponse>()
#endif
			;
		}

#if WINRT
		public IAsyncOperation<WriteResponse> WriteAsync(byte handle, IBuffer data)
#else
		public Task<WriteResponse> WriteAsync(byte handle, byte[] data)
#endif
		{
			return WriteAsyncInternal(handle, data)
#if WINRT
		.AsAsyncOperation<WriteResponse>()
#endif
			;
		}
		#endregion

		#region OpenDataWrite
		/// <summary>
		/// Creates and opens a nonlinear file to write textual data
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fileSize"></param>
		/// <returns></returns>
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		OpenDataWriteAsync(string fileName, uint fileSize)
		{
			return OpenDataWriteAsyncInternal(fileName, fileSize)
#if WINRT
			.AsAsyncOperation<byte>()
#endif
;
		}
		#endregion

		#region OpenDataAppend
		/// <summary>
		/// Opens an existing data file to append data
		/// </summary>
		/// <param name="fileName"></param>
#if WINRT
		public IAsyncOperation<OpenDataAppendResponse>
#else
		public Task<OpenDataAppendResponse>
#endif
		OpenDataAppendAsync(string fileName)
		{
			return OpenDataAppendAsyncInternal(fileName)
#if WINRT
			.AsAsyncOperation<OpenDataAppendResponse>()
#endif
			;
		}
		#endregion

		#region OpenLinearRead
		/// <summary>
		/// Opens a linear file for read access
		/// </summary>
		/// <param name="fileName"></param>
#if WINRT
		public IAsyncOperation<uint>
#else
		public Task<uint>
#endif
		OpenLinearReadAsync(string fileName)
		{
			return OpenLinearReadAsyncInternal(fileName)
#if WINRT
			.AsAsyncOperation<uint>()
#endif
			;
		}
		#endregion

		#region OpenLinearWrite
		/// <summary>
		/// Creates and opens a linear file for write access
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fileSize"></param>
#if WINRT
		public IAsyncOperation<byte>
#else
		public Task<byte>
#endif
		OpenLinearWriteAsync(string fileName, uint fileSize)
		{
			return OpenLinearWriteAsyncInternal(fileName, fileSize)
#if WINRT
			.AsAsyncOperation<byte>()
#endif
;
		}
		#endregion

		#region DeleteUserFlash
		/// <summary>
		/// Erases/Formats the Brick's User Flash.
		/// </summary>
#if WINRT
		public IAsyncAction
#else
		public Task
#endif
		DeleteUserFlashAsync()
		{
			return DeleteUserFlashAsyncInternal()
#if WINRT
			.AsAsyncAction()
#endif
			;
		}
		#endregion

		#region PollCommandLength
		/// <summary>
		/// Requests the length of Poll-Command data available
		/// </summary>
		/// <param name="buffer"></param>
#if WINRT
		public IAsyncOperation<PollCommandLengthResponse>
#else
		public Task<PollCommandLengthResponse>
#endif
		PollCommandLengthAsync(PollCommandBuffer buffer)
		{
			return PollCommandLengthAsyncInternal(buffer)
#if WINRT
			.AsAsyncOperation<PollCommandLengthResponse>()
#endif
			;
		}
		#endregion

		#region PollCommand
		/// <summary>
		/// Polls the specified buffer for given length of poll command
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="commandLength"></param>
#if WINRT
		public IAsyncOperation<PollCommandResponse>
#else
		public Task<PollCommandResponse>
#endif
		PollCommandAsync(PollCommandBuffer buffer, byte commandLength)
		{
			return PollCommandAsyncInternal(buffer, commandLength)
#if WINRT
			.AsAsyncOperation<PollCommandResponse>()
#endif
			;
		}
		#endregion
	}
}
