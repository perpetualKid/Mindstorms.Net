using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core
{
	/// <summary>
	/// Possible error codes sent by the NXT.
	/// </summary>

	public enum Error
	{
		/// <summary>
		/// Successful communication, no error.
		/// </summary>
		Success = 0x0,

		/// <summary>
		/// Pending communication transaction in progress.
		/// </summary>
		PendingTransactionInProgress = 0x20,

		/// <summary>
		/// No More Handles available.
		/// </summary>
		NoMoreHandles = 0x81,

		/// <summary>
		/// No space available.
		/// </summary>
		NoSpace = 0x82,

		/// <summary>
		/// No more files found.
		/// </summary>
		NoMoreFiles = 0x83,

		/// <summary>
		/// End Of File (EOF) expected.
		/// </summary>
		EndOfFileExpected = 0x84,

		/// <summary>
		/// End Of File (EOF) found.
		/// </summary>
		EndOfFile = 0x85,

		/// <summary>
		/// File is not a linear File.
		/// </summary>
		NotALinearFile = 0x86,

		/// <summary>
		/// File not found.
		/// </summary>
		FileNotFound = 0x87,

		/// <summary>
		/// The Handle is already closed.
		/// </summary>
		HandleAlreadyClosed = 0x88,

		/// <summary>
		/// No Linear Space available.
		/// </summary>
		NoLinearSpace = 0x89,

		/// <summary>
		/// Undefined Error. No further information available.
		/// </summary>
		UndefinedError = 0x8A,

		/// <summary>
		/// File busy already.
		/// </summary>
		FileIsBusy = 0x8B,

		/// <summary>
		/// No Write-Buffers available.
		/// </summary>
		NoWriteBuffers = 0x8C,

		/// <summary>
		/// Not possible to append to this file.
		/// </summary>
		AppendNotPossible = 0x8D,

		/// <summary>
		/// File already full.
		/// </summary>
		FileIsFull = 0x8E,

		/// <summary>
		/// File exists already.
		/// </summary>
		FileExists = 0x8F,

		/// <summary>
		/// Module not found.
		/// </summary>
		ModuleNotFound = 0x90,

		/// <summary>
		/// Out of boundary.
		/// </summary>
		OutOfBoundary = 0x91,

		/// <summary>
		/// Illegal file name.
		/// </summary>
		IllegalFileName = 0x92,

		/// <summary>
		/// Illegal Handle.
		/// </summary>
		IllegalHandle = 0x93,

		/// <summary>
		/// Specified mailbox queue is empty.
		/// </summary>
		SpecifiedMailboxQueueEmpty = 0x40,

		/// <summary>
		/// Request failed (i.e. specified file not found).
		/// </summary>
		RequestFailed = 0xBD,

		/// <summary>
		/// Unknown command opcode.
		/// </summary>
		UnknownCommandOpcode = 0xBE,

		/// <summary>
		/// Insane packet.
		/// </summary>
		InsanePacket = 0xBF,

		/// <summary>
		/// Data contains out-of-range values.
		/// </summary>
		OutOfRangeValues = 0xC0,

		/// <summary>
		/// Communication bus error.
		/// </summary>
		CommunicationBusError = 0xDD,

		/// <summary>
		/// No free memory in communication buffer.
		/// </summary>
		NoFreeMemoryInComBuffer = 0xDE,

		/// <summary>
		/// Specified channel/connection is not valid.
		/// </summary>
		SpecifiedChannelIsNotValid = 0xDF,

		/// <summary>
		/// Specified channel/connection not configured or busy.
		/// </summary>
		SpecifiedChannelNotConfiguredOrBusy = 0xE0,

		/// <summary>
		/// No active program.
		/// </summary>
		NoActiveProgram = 0xEC,

		/// <summary>
		/// Illegal size specified.
		/// </summary>
		IllegalSizeSpecified = 0xED,

		/// <summary>
		/// Illegal mailbox queue ID specified.
		/// </summary>
		IllegalMailboxQueueId = 0xEE,

		/// <summary>
		/// Attempted to access invalid field of a structure.
		/// </summary>
		InvalidFieldAccess = 0xEF,

		/// <summary>
		/// Bad input or output specified.
		/// </summary>
		BadInputOrOutput = 0xF0,

		/// <summary>
		/// Insufficient memory available.
		/// </summary>
		InsufficientMemoryAvailable = 0xFB,

		/// <summary>
		/// Bad arguments.
		/// </summary>
		BadArguments = 0xFF
	}

	public static class ErrorMessages
	{
		static ErrorMessages()
		{
			Messages = new Dictionary<Error, string>();
			#region Direct Command Errors
			Messages.Add(Error.BadArguments, "Bad arguments");
			Messages.Add(Error.InsufficientMemoryAvailable, "Insufficient memory available");
			Messages.Add(Error.BadInputOrOutput, "Bad input or output specified");
			Messages.Add(Error.InvalidFieldAccess, "Attempted to access invalid field of a structure");
			Messages.Add(Error.IllegalMailboxQueueId, "Illegal mailbox queue ID specified");
			Messages.Add(Error.IllegalSizeSpecified, "Illegal size specified");
			Messages.Add(Error.NoActiveProgram, "No active program");
			Messages.Add(Error.SpecifiedChannelNotConfiguredOrBusy, "Specified channel/connection not configured or busy");
			Messages.Add(Error.SpecifiedChannelIsNotValid, "Specified channel/connection is not valid");
			Messages.Add(Error.NoFreeMemoryInComBuffer, "No free memory in communication buffer");
			Messages.Add(Error.CommunicationBusError, "Communication bus error");
			Messages.Add(Error.OutOfRangeValues, "Data contains out-of-range values");
			Messages.Add(Error.InsanePacket, "Insane packet");
			Messages.Add(Error.UnknownCommandOpcode, "Unknown command opcode");
			Messages.Add(Error.RequestFailed, "Request failed ");
			Messages.Add(Error.SpecifiedMailboxQueueEmpty, "Specified mailbox queue is empty");
			Messages.Add(Error.PendingTransactionInProgress, "Pending communication transaction in progress");
			Messages.Add(Error.Success, "Success");
			#endregion

			#region System Command Errors
			Messages.Add(Error.NoMoreHandles, "No more handles available");
			Messages.Add(Error.NoSpace, "No space available");
			Messages.Add(Error.NoMoreFiles, "No more files found");
			Messages.Add(Error.EndOfFileExpected, "End Of File (EOF) expected");
			Messages.Add(Error.EndOfFile, "End Of File (EOF) found");
			Messages.Add(Error.NotALinearFile, "File is not a linear File");
			Messages.Add(Error.FileNotFound, "File not found");
			Messages.Add(Error.HandleAlreadyClosed, "The handle is already closed");
			Messages.Add(Error.NoLinearSpace, "No linear space available");
			Messages.Add(Error.UndefinedError, "Undefined error. No further information available");
			Messages.Add(Error.FileIsBusy, "File busy already");
			Messages.Add(Error.NoWriteBuffers, "No write buffers available");
			Messages.Add(Error.AppendNotPossible, "Not possible to append to this file");
			Messages.Add(Error.FileIsFull, "File already full");
			Messages.Add(Error.FileExists, "File exists already");
			Messages.Add(Error.ModuleNotFound, "Module not found");
			Messages.Add(Error.OutOfBoundary, "Out of boundary");
			Messages.Add(Error.IllegalFileName, "Illegal file name");
			Messages.Add(Error.IllegalHandle, "Illegal handle");
			#endregion
		}

		public static IDictionary<Error, string> Messages { get; private set; }

	}
}
