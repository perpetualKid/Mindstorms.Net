using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mindstorms.Net.Core;
using Mindstorms.Net.Core.Sensors;
using Mindstorms.Net.Core.Sensors.Nxt;
using Mindstorms.Net.Desktop;

namespace Mindstorms.Net.Test.Console
{
	class Program
	{
		static ManualResetEvent _mre = new ManualResetEvent(false);
		static byte lastError;
		static ResponseTelegram lastResponse;
		static NxtBrick nxtBrick;

		static void Main(string[] args)
		{
			//string portName = "COM9";

			string command = null;
			string param0 = null;
			string param1 = null;
			string param2 = null;

			//channel = new BluetoothCommunication(portName);
			//channel.ResponseReceived += channel_ResponseReceived;
			//channel.ConnectAsync().Wait();

			nxtBrick = new NxtBrick(new BluetoothCommunication(GetComPort()));
			nxtBrick.ConnectionCommands.ConnectAsync().Wait();

			while (command != "q")
			{
				switch (command)
				{
					case "q":
						break;
					case "b":
						System.Console.WriteLine(GetBatteryLevel());
						break;
					case "a":
					case "alive":
						System.Console.WriteLine(KeepAlive());
						break;
					case "v":
						System.Console.WriteLine(GetVersion());
						break;
					case "t":
						PlayTone();
						break;
					case "l":
						PlaySoundFile(true);
						break;
					case "j":
						PlaySoundFile(false);
						break;
					case "k":
						StopSoundFile();
						break;
					case "p":
						System.Console.Write("Program: ");
						param0 = System.Console.ReadLine();
						StartProgram(param0);
						break;
					case "s":
						StopProgram();
						break;
					case "c":
						System.Console.WriteLine(GetCurrentProgram());
						break;
					case "mr":
						System.Console.Write("LocalInbox: ");
						param0 = System.Console.ReadLine();
						System.Console.Write("RemoteInbox: ");
						param1 = System.Console.ReadLine();
						System.Console.WriteLine(MessageRead(int.Parse(param0), int.Parse(param1)));
						break;
					case "mw":
						System.Console.Write("Inbox: ");
						param0 = System.Console.ReadLine();
						MessageWrite(int.Parse(param0));
						break;
					case "coast":
						CoastMotors();
						break;
					case "stop":
						StopMotors();
						break;
					case "brake":
						BrakeMotors();
						break;
					case "state":
						System.Console.Write("Motor#: ");
						param0 = System.Console.ReadLine();
						System.Console.WriteLine(GetMotorState(int.Parse(param0)));
						break;
					case "reset":
						System.Console.Write("Motor#: ");
						param0 = System.Console.ReadLine();
						System.Console.Write("Relative?: ");
						param1 = System.Console.ReadLine();
						ResetMotorPort((MotorPort)int.Parse(param0), int.Parse(param1));
						break;
					case "run":
						System.Console.Write("Power %: ");
						param0 = System.Console.ReadLine();
						RunMotors(short.Parse(param0));
						break;
					case "reseti":
						System.Console.Write("Sensor#: ");
						param0 = System.Console.ReadLine();
						ResetInput(int.Parse(param0));
						break;
					case "seti":
						System.Console.Write("Sensor#: ");
						param0 = System.Console.ReadLine();
						foreach (string item in Enum.GetNames(typeof(SensorType)))
							System.Console.WriteLine(string.Format("{0}::{1}", (int)Enum.Parse(typeof(SensorType), item), item));
						System.Console.Write("SensorType: ");
						param1 = System.Console.ReadLine();
						foreach (string item in Enum.GetNames(typeof(SensorMode)))
							System.Console.WriteLine(string.Format("{0}::{1}", (int)Enum.Parse(typeof(SensorMode), item), item));
						System.Console.Write("SensorMode: ");
						param2 = System.Console.ReadLine();
						SetInput(int.Parse(param0), int.Parse(param1), int.Parse(param2));
						break;
					case "geti":
						System.Console.Write("Sensor#: ");
						param0 = System.Console.ReadLine();
						System.Console.WriteLine(GetInput(int.Parse(param0)));
						break;
					case "lsstatus":
						System.Console.Write("Sensor#: ");
						param0 = System.Console.ReadLine();
						System.Console.WriteLine(LsGetStatus(int.Parse(param0)));
						break;
					case "lsread":
						System.Console.Write("Sensor#: ");
						param0 = System.Console.ReadLine();
						System.Console.WriteLine(LsRead(int.Parse(param0)));
						break;
					case "com":
						System.Console.WriteLine(GetComPort());
						break;
					case "bt":
						System.Console.WriteLine(GetBluetoothAddress());
						break;
					case "boot":
						System.Console.WriteLine(Boot());
						break;
					case "name":
						System.Console.Write("New Name: ");
						param0 = System.Console.ReadLine();
						SetName(param0);
						break;
					case "info":
						System.Console.WriteLine(GetDeviceInfo());
						break;
					case "ff":
						System.Console.Write("FileName Pattern: ");
						param0 = System.Console.ReadLine();
						System.Console.WriteLine(FindFirst(param0));
						break;
					case "fn":
						System.Console.Write("Handle: ");
						param0 = System.Console.ReadLine();
						System.Console.WriteLine(FindNext(byte.Parse(param0)));
						break;
					case "ch":
						System.Console.Write("Handle: ");
						param0 = System.Console.ReadLine();
						System.Console.WriteLine(CloseHandle(byte.Parse(param0)));
						break;
					case "openread":
						System.Console.Write("File name: ");
						param0 = System.Console.ReadLine();
						System.Console.WriteLine(OpenRead(param0));
						break;
					case "read":
						System.Console.Write("Handle: ");
						param0 = System.Console.ReadLine();
						System.Console.Write("Length: ");
						param1 = System.Console.ReadLine();
						System.Console.WriteLine(Read(byte.Parse(param0), ushort.Parse(param1)));
						break;
					case "openwrite":
						System.Console.Write("File name: ");
						param0 = System.Console.ReadLine();
						System.Console.Write("FileSize: ");
						param1 = System.Console.ReadLine();
						System.Console.WriteLine(OpenWrite(param0, uint.Parse(param1)));
						break;
					case "write":
						System.Console.Write("Handle: ");
						param0 = System.Console.ReadLine();
						System.Console.Write("Data: ");
						param1 = System.Console.ReadLine();
						System.Console.WriteLine(Write(byte.Parse(param0), param1));
						break;
					case "opendatawrite":
						System.Console.Write("File name: ");
						param0 = System.Console.ReadLine();
						System.Console.Write("FileSize: ");
						param1 = System.Console.ReadLine();
						System.Console.WriteLine(OpenDataWrite(param0, uint.Parse(param1)));
						break;
					case "opendataappend":
						System.Console.Write("File name: ");
						param0 = System.Console.ReadLine();
						System.Console.WriteLine(OpenDataAppend(param0));
						break;
					case "openlinearread":
						System.Console.Write("File name: ");
						param0 = System.Console.ReadLine();
						System.Console.WriteLine(OpenLinearRead(param0));
						break;
					case "delete":
						System.Console.Write("File name: ");
						param0 = System.Console.ReadLine();
						System.Console.WriteLine(Delete(param0));
						break;
					case "pcl":
						System.Console.Write("Buffer (0 = Poll, 1 = High Speed: ");
						param0 = System.Console.ReadLine();
						System.Console.WriteLine(PollCommandLength((PollCommandBuffer)byte.Parse(param0)));
						break;
					case "pc":
						System.Console.Write("Buffer (0 = Poll, 1 = High Speed: ");
						param0 = System.Console.ReadLine();
						System.Console.Write("Command Length: ");
						param1 = System.Console.ReadLine();
						System.Console.WriteLine(PollCommand((PollCommandBuffer)byte.Parse(param0), byte.Parse(param1)));
						break;
					case "button":
						System.Console.Write("Sensor#: ");
						param0 = System.Console.ReadLine();
						GetButtonState(int.Parse(param0));
						break;
					case "color":
						System.Console.Write("Sensor#: ");
						param0 = System.Console.ReadLine();
						GetColor(int.Parse(param0));
						break;
					case "light":
						System.Console.Write("Sensor#: ");
						param0 = System.Console.ReadLine();
						foreach (string item in Enum.GetNames(typeof(SensorColor)))
							System.Console.WriteLine(string.Format("{0}::{1}", (int)Enum.Parse(typeof(SensorColor), item), item));
						System.Console.Write("ColorMode: ");
						param1 = System.Console.ReadLine();
						GetLightIntensity(int.Parse(param0), int.Parse(param1));
						break;
					case "ultrasonic":
						System.Console.Write("Sensor#: ");
						param0 = System.Console.ReadLine();
						GetDistance(int.Parse(param0));
						break;
					case "digitalinfo":
						System.Console.Write("Sensor#: ");
						param0 = System.Console.ReadLine();
						DigitalSensorInformation(int.Parse(param0));
						break;
					case "ultrasonicinfo":
						System.Console.Write("Sensor#: ");
						param0 = System.Console.ReadLine();
						UltrasonicInfo(int.Parse(param0));
						break;
					case "ultrasonicinterval":
						System.Console.Write("Sensor#: ");
						param0 = System.Console.ReadLine();
						System.Console.Write("Interval: ");
						param1 = System.Console.ReadLine();
						UltrasonicInterval(int.Parse(param0), int.Parse(param1));
						break;

					case "error":
						System.Console.WriteLine( null == nxtBrick.LastResponse ? "Null" : nxtBrick.LastResponse.ErrorMessage);
						break;
					case "?":
					case "h":
					case "help":
						System.Console.Clear();
						System.Console.WriteLine("q: Exits the program");
						System.Console.WriteLine("a or alive: Keep Alive");
						System.Console.WriteLine("b: Get Battery Level");
						System.Console.WriteLine("v: Get System Information");
						System.Console.WriteLine("t: Play a Tone");
						System.Console.WriteLine("l: Play Soundfile in a loop");
						System.Console.WriteLine("j: Play Soundfile once");
						System.Console.WriteLine("k: Stop Playing Soundfile");
						System.Console.WriteLine("p: Start a Program");
						System.Console.WriteLine("s: Stops current Program");
						System.Console.WriteLine("c: Get current Program Name");
						System.Console.WriteLine("mr: Reads message from inbox");
						System.Console.WriteLine("mw: Writes message to inbox");
						System.Console.WriteLine("reset: Reset Motor Port");
						System.Console.WriteLine("coast: Coast all motors");
						System.Console.WriteLine("stop: Stop all motors");
						System.Console.WriteLine("brake: Brake all motors");
						System.Console.WriteLine("state: State of a motor");
						System.Console.WriteLine("run: Runs all motors");
						System.Console.WriteLine("reseti: Resets an input port");
						System.Console.WriteLine("seti: Sets an input port");
						System.Console.WriteLine("lsstatus: Get status of a I2C Port");
						System.Console.WriteLine("lswrite: Write data to an I2C Port");
						System.Console.WriteLine("lsread: Read data of an I2C Port");
						System.Console.WriteLine("com: Get the currently paired COM port");
						System.Console.WriteLine("bt: Get the currently BT address");
						System.Console.WriteLine("boot: Reboots the NXT Brick. Not valid over Bluetooth");
						System.Console.WriteLine("name: Renames the NXT");
						System.Console.WriteLine("info: Get device information about the NXT");
						System.Console.WriteLine("ff: FindFirst file search");
						System.Console.WriteLine("fn: Continue a file search");
						System.Console.WriteLine("ch: Close a file handle");
						System.Console.WriteLine("openread: Opens a file for reading");
						System.Console.WriteLine("read: Reads from previously opened file");
						System.Console.WriteLine("delete: Deletes a file");
						System.Console.WriteLine("openwrite: Opens a nonlinear file for non textual data");
						System.Console.WriteLine("write: Writes to previously opened file");
						System.Console.WriteLine("opendatawrite: Opens a file to write textual data");
						System.Console.WriteLine("opendataappend: Opens a file to append textual data");
						System.Console.WriteLine("openlinearread: Opens a linear file for reading");
						System.Console.WriteLine("pcl: reads the poll command length");
						System.Console.WriteLine("pc: reads the poll command");
						System.Console.WriteLine("button: checks the button at a port");
						System.Console.WriteLine("color: checks the color sensor at a port");
						System.Console.WriteLine("light: checks the light sensor at a port");
						System.Console.WriteLine("ultrasonic: checks the ultrasonic sensor at a port");
						System.Console.WriteLine("digitalinfo: gets information from digital sensor at a port");
						System.Console.WriteLine("ultrasonicinfo: gets current values from ultrasonic sensor");
						System.Console.WriteLine("ultrasonicinterval: gets and set measurement interval on ultrasonic sensor");

						System.Console.WriteLine("error: Get the last respone error status");
						System.Console.WriteLine("? or h or help: This Help Screen");
						break;
				}
				System.Console.Write("Press q to exit or h for help: ");
				command = System.Console.ReadLine();

			}
			nxtBrick.ConnectionCommands.DisconnectAsync().Wait();
		}

		static void channel_ResponseReceived(object sender, ResponseReceivedEventArgs e)
		{
			// Clear the last error.
			lastError = 0;
			// Parse the header that contains the message size.
			byte[] buffer = e.ResponseTelegram.Data;

			// Parse the rest of the message if the size is correct.
			if (e.ResponseTelegram.Size == e.ResponseTelegram.Data.Length)
			{
				// Read the response from the input buffer.
				lastResponse = e.ResponseTelegram;

				// If the first byte in the reply is not 0x02 (reply command) - throw exception.
				if (lastResponse.Data[0] != (byte)CommandType.ReplyCommand)
				{
					throw new InvalidDataException("Unexpected return message type: " + lastResponse.Data[0].ToString(CultureInfo.InvariantCulture));
				}
				// If the second byte in the reply does not match the command sent - throw exception.
				//else if( LastResponse[ 1 ] != lastCommandSent )
				//{
				//    throw new InvalidDataException( "Unexpected return command: " + LastResponse[ 1 ].ToString( CultureInfo.InvariantCulture ) );
				//}

				// Save the status byte as the last error value.
				lastError = lastResponse.Data[2];
			}
			else
			{
				throw new InvalidDataException("Invalid message size: " + e.ResponseTelegram.Size.ToString(CultureInfo.InvariantCulture));
			}
			// Set the flag to indicate that the caller can parse the response.
			_mre.Set();

		}

		public static string LSRead(SensorPort port)
		{
			byte[] buffer = new byte[] { 0x02, 0x00};
			int expected = 8;
			List<byte> bytesRead = new List<byte>();
			nxtBrick.DirectCommands.LowSpeedWriteAsync(port, buffer, expected).Wait();
			nxtBrick.DirectCommands.LowSpeedGetStatusAsync(port).Wait();
			return nxtBrick.DirectCommands.LowSpeedReadAsync(port).Result.AsString;
		}

		public static string GetVersion()
		{
			GetFirmwareVersionResponse firmwareVersion = nxtBrick.SystemCommands.GetFirmwareVersionAsync().Result;
			return string.Format(CultureInfo.InvariantCulture, "Protocol Version> {0}\n\rFirmware Version> {1}", firmwareVersion.ProtocolVersion, firmwareVersion.FirmwareVersion);
}

		public static string GetBatteryLevel()
		{

			//TransmitAndWait(RequestTelegram.GetBatteryLevel(), 5).Wait(); // Return package: 0:0x02, 1:Command, 2:StatusByte, 3-4: Voltage in millivolts (UWORD)

			//return String.Format(CultureInfo.InvariantCulture, "Battery> {0}", (UInt16)(lastResponse.Data[3] + 0x100 * lastResponse.Data[4]));

			return String.Format(CultureInfo.InvariantCulture, "Battery> {0}", nxtBrick.DirectCommands.GetBatteryLevelAsync().Result);

		}

		public static string KeepAlive()
		{
			TimeSpan interval = TimeSpan.FromMilliseconds(nxtBrick.DirectCommands.KeepAliveAsync().Result);
			return String.Format(CultureInfo.InvariantCulture, "Keep Alive> {0}", interval.ToString());

		}

		public static string ReadDigitalValue(SensorPort port)
		{
			string result = string.Empty;
			return result;
		}

		public static void PlayTone()
		{
			nxtBrick.DirectCommands.PlayToneAsync(1000, 3000).Wait();
			//channel.WriteAsync(RequestTelegram.PlayTone(1000, 5000).ToBytes());
			//Transmit(RequestTelegram.PlayTone(1000, 5000), 5).Wait(); // Return package: 0:0x02, 1:Command, 2:StatusByte, 3-4: Voltage in millivolts (UWORD)
		}

		public static void PlaySoundFile(bool loop)
		{
			nxtBrick.DirectCommands.PlaySoundFileAsync("Object", loop).Wait();
		}

		public static void StopSoundFile()
		{
			nxtBrick.DirectCommands.StopSoundPlaybackAsync().Wait();
		}

		public static void StartProgram(string program)
		{
			if (string.IsNullOrEmpty(program))
				program = "Objekte Finden";
			nxtBrick.DirectCommands.StartProgramAsync(program).Wait();
			//channel.WriteAsync(RequestTelegram.PlayTone(1000, 5000).ToBytes());
			//Transmit(RequestTelegram.PlayTone(1000, 5000), 5).Wait(); // Return package: 0:0x02, 1:Command, 2:StatusByte, 3-4: Voltage in millivolts (UWORD)
		}

		public static void StopProgram()
		{
			nxtBrick.DirectCommands.StopProgramAsync().Wait();
			//channel.WriteAsync(RequestTelegram.PlayTone(1000, 5000).ToBytes());
			//Transmit(RequestTelegram.PlayTone(1000, 5000), 5).Wait(); // Return package: 0:0x02, 1:Command, 2:StatusByte, 3-4: Voltage in millivolts (UWORD)
		}

		public static string GetCurrentProgram()
		{
			string result = nxtBrick.DirectCommands.GetCurrentProgramNameAsync().Result;
			if (String.IsNullOrEmpty(result) || nxtBrick.LastResponse.Status != Error.Success)
				result = nxtBrick.LastResponse.ErrorMessage;
			return String.Format(CultureInfo.InvariantCulture, "Program Name> {0}", result);
		}

		public static string MessageRead(int localInbox, int remoteInbox)
		{
			string result = nxtBrick.DirectCommands.MessageReadAsync(remoteInbox, localInbox, true).Result.AsString;
			if (String.IsNullOrEmpty(result) || nxtBrick.LastResponse.Status != Error.Success)
				result = nxtBrick.LastResponse.ErrorMessage;
			return String.Format(CultureInfo.InvariantCulture, "Message[{0}]::[{1}]> {2}", localInbox, remoteInbox, result);
		}

		public static void MessageWrite(int inbox)
		{
			nxtBrick.DirectCommands.MessageWriteAsync(inbox, System.DateTime.Now.TimeOfDay.ToString());
		}

		public static void ResetMotorPort(MotorPort motorPort, int relative)
		{
			nxtBrick.DirectCommands.ResetMotorPositionAsync(motorPort, (relative == 0)).Wait();
		}

		public static void CoastMotors()
		{
			nxtBrick.DirectCommands.SetOutputStateAsync(MotorPort.All, 50, MotorModes.Coast, MotorRegulationMode.Idle, 0, MotorRunState.Running, 0);
		}

		public static void StopMotors()
		{
			nxtBrick.DirectCommands.SetOutputStateAsync(MotorPort.All, 0, MotorModes.Coast, MotorRegulationMode.Idle, 0, MotorRunState.Idle, 0);
		}

		public static void BrakeMotors()
		{
			nxtBrick.DirectCommands.SetOutputStateAsync(MotorPort.All, 0, MotorModes.On, MotorRegulationMode.Idle, 0, MotorRunState.Running, 0);
		}

		public static void RunMotors(short power)
		{
			nxtBrick.DirectCommands.SetOutputStateAsync(MotorPort.All, power, MotorModes.On, MotorRegulationMode.Idle, 0, MotorRunState.Running, 0);
		}

		public static string GetMotorState(int motor)
		{
			GetOutputStateResponse motorstate = nxtBrick.DirectCommands.GetOutputStateAsync((MotorPort)motor).Result;
			return motorstate.ToString();
		}

		public static void ResetInput(int sensor)
		{
			nxtBrick.DirectCommands.ResetInputScaledValueAsync((SensorPort)sensor).Wait();
		}

		public static void SetInput(int sensor, int sensorType, int sensorMode)
		{
			nxtBrick.DirectCommands.SetInputModeAsync((SensorPort)sensor, (SensorType)sensorType, (SensorMode)sensorMode);
		}

		public static string GetInput(int sensor)
		{
			GetInputValuesResponse result = nxtBrick.DirectCommands.GetInputValuesAsync((SensorPort)sensor).Result;
			return result.ToString();
		}

		public static string LsGetStatus(int sensor)
		{
			return nxtBrick.DirectCommands.LowSpeedGetStatusAsync((SensorPort)sensor).Result.ToString();
		}

		public static string LsRead(int sensor)
		{
			return LSRead((SensorPort)sensor);
		}

		public static string GetComPort()
		{
			return ComHelper.GetPairedNxtBluetoothCom().FirstOrDefault();
		}

		public static string GetBluetoothAddress()
		{
			return ComHelper.GetPairedNxtBluetoothBTAddress().FirstOrDefault().Value;
		}

		public static string Boot()
		{
			string result = nxtBrick.SystemCommands.BootAsync().Result;
			if (string.IsNullOrEmpty(result) && nxtBrick.LastResponse.Status != Error.Success)
				result = nxtBrick.LastResponse.ErrorMessage;
			return result;
		}

		public static void SetName(string name)
		{
			nxtBrick.SystemCommands.SetBrickNameAsync(name).Wait();
		}

		public static string GetDeviceInfo()
		{
			return nxtBrick.SystemCommands.GetDeviceInfoAsync().Result.ToString();
		}

		public static string FindFirst(string pattern)
		{
			FindFileResponse result = nxtBrick.SystemCommands.FindFirstAsync(pattern).Result;
			if (result.NoMoreFile)
				return ErrorMessages.Messages[Error.FileNotFound];
			return result.Handle + "::" + result.FileName + "::" + result.FileSize.ToString() + " byte";
		}

		public static string FindNext(byte handle)
		{
			FindFileResponse result = nxtBrick.SystemCommands.FindNextAsync(handle).Result;
			if (result.NoMoreFile)
				return ErrorMessages.Messages[Error.FileNotFound];
			return result.Handle + "::" + result.FileName + "::" + result.FileSize.ToString() + " byte";
		}

		public static string CloseHandle(byte handle)
		{
			byte result = nxtBrick.SystemCommands.CloseAsync(handle).Result;
			return string.Format("Handle {0} closed", result);
		}

		public static string OpenRead(string fileName)
		{
			OpenReadResponse result = nxtBrick.SystemCommands.OpenReadAsync(fileName).Result;
			return string.Format("File {0} opened with Handle {1}, size {2}", fileName, result.Handle, result.FileSize);
		}
		public static string Read(byte handle, ushort length)
		{
			ReadResponse result = nxtBrick.SystemCommands.ReadAsync(handle, length).Result;
			return string.Format("File at handle {0} read {1} bytes\n\rText:: {2}\n\rHex::{3}", result.Handle, result.Size, result.AsString, result.AsHexString);
		}

		public static string Delete(string fileName)
		{
			return nxtBrick.SystemCommands.DeleteAsync(fileName).Result;
		}

		public static string OpenWrite(string fileName, uint fileSize)
		{
			return nxtBrick.SystemCommands.OpenWriteAsync(fileName, fileSize).Result.ToString();
		}

		public static string Write(byte handle, string data)
		{
			WriteResponse result = nxtBrick.SystemCommands.WriteAsync(handle, data).Result;
			return string.Format("Written {0} bytes to file at handle {1}", result.Size, result.Handle);
		}

		public static string OpenDataWrite(string fileName, uint fileSize)
		{
			return nxtBrick.SystemCommands.OpenDataWriteAsync(fileName, fileSize).Result.ToString();
		}

		public static string OpenDataAppend(string fileName)
		{
			OpenDataAppendResponse result = nxtBrick.SystemCommands.OpenDataAppendAsync(fileName).Result;
			return string.Format("File at handle {0} opened, remaining {1} bytes to append", result.Handle, result.Size);
		}

		public static string PollCommandLength(PollCommandBuffer buffer)
		{
			PollCommandLengthResponse result = nxtBrick.SystemCommands.PollCommandLengthAsync(buffer).Result;
			return string.Format("Buffer {0} has {1} bytes ready", result.BufferNumber.ToString(), result.PollCommandLength);
		}

		public static string PollCommand(PollCommandBuffer buffer, byte length)
		{
			PollCommandResponse result = nxtBrick.SystemCommands.PollCommandAsync(buffer, length).Result;
			return string.Format("Buffer {0} has {1} bytes command\n\rText:: {2}\n\rHex::{3}", result.BufferNumber.ToString(), result.Length, result.AsString, result.AsHexString);
		}

		public static string OpenLinearRead(string fileName)
		{
			uint pointer = nxtBrick.SystemCommands.OpenLinearReadAsync(fileName).Result;
			return string.Format("Segment Pointer: {0}", pointer);
		}

		public static void GetButtonState(int port)
		{
			TouchSensor button = new TouchSensor();

			button.OnPressed += button_OnPressed1;
			button.OnPressed += button_OnPressed2;
			button.OnChanged += button_OnChanged;
			button.OnReleased += button_OnReleased;
			nxtBrick.AttachSensorAsync((SensorPort)port, button).Wait();

			button.AutoPoll(50);
			while (!System.Console.KeyAvailable || System.Console.ReadKey().KeyChar != 'c' )
			{
//				button.Poll().Wait();
//				System.Console.WriteLine("IsPressed: {0}", button.Pressed);
				Thread.Sleep(10);
			}
			button.AutoPoll(0);
			Thread.Sleep(1000);
			button.Poll().Wait();
			nxtBrick.DetachSensorAsync((SensorPort)port);
		}

		public static void GetColor(int port)
		{
			ColorSensor colorSensor = new ColorSensor();
			nxtBrick.AttachSensorAsync((SensorPort)port, colorSensor).Wait();

			colorSensor.SetMatchingColorsAsync(new List<SensorColor>() { SensorColor.Green, SensorColor.Red });
			colorSensor.OnColorDetected += colorSensor_OnColorDetected;

			colorSensor.AutoPoll(100);
			while (!System.Console.KeyAvailable || System.Console.ReadKey().KeyChar != 'c')
			{
				//colorSensor.Poll().Wait();
				//System.Console.WriteLine("Color: {0}", colorSensor.Color);
				Thread.Sleep(100);
			}
			nxtBrick.DetachSensorAsync((SensorPort)port);
		}

		static void colorSensor_OnColorDetected(object sender, ColorChangedEventArgs e)
		{
			System.Console.WriteLine("Sensor {0}:: has detected color {1}", ((SensorBase)sender).SensorPort, e.Color);
		}

		public static void GetLightIntensity(int port, int color )
		{
			ColorSensor colorSensor = new ColorSensor();
			nxtBrick.AttachSensorAsync((SensorPort)port, colorSensor).Wait();
			colorSensor.OnChanged += colorSensor_OnChanged;
			colorSensor.SetSensorModeAsync(ColorSensorMode.LightSensor, (SensorColor)color).Wait();
			colorSensor.AutoPoll(20);
			while (!System.Console.KeyAvailable || System.Console.ReadKey().KeyChar != 'c')
			{
				//colorSensor.Poll().Wait();
				//System.Console.WriteLine("Intensity: {0} ", colorSensor.Intensity);
				Thread.Sleep(100);
			}
			nxtBrick.DetachSensorAsync((SensorPort)port).Wait();
		}

		public static void UltrasonicInfo(int port)
		{
			UltraSonicSensor ultraSonic = new UltraSonicSensor();
			nxtBrick.AttachSensorAsync((SensorPort)port, ultraSonic).Wait();
			System.Console.WriteLine("Continuous Measurement Interval: {0} ", ultraSonic.ReadContinuousMeasurementIntervalAync().Result);
			System.Console.WriteLine("Command State : {0} ", ultraSonic.ReadCommandStateAsync().Result);
			System.Console.WriteLine("Actual Zero: {0} ", ultraSonic.ReadActualZeroAsync().Result);
			System.Console.WriteLine("Actual Scale Factor: {0} ", ultraSonic.ReadActualScaleFactorAsync().Result);
			System.Console.WriteLine("Actual Scale Divisor: {0} ", ultraSonic.ReadActualScaleDivisorAsync().Result);
			for (int i = 0; i < 8; i++)
			{
				System.Console.WriteLine("Measurement Byte[{1}]: {0} ", ultraSonic.ReadMeasurementByteAsync((byte)i).Result, i);
			}
			Thread.Sleep(50);
			ultraSonic.SingleShotCommandAsync().Wait();
			System.Console.WriteLine("Command State : {0} ", ultraSonic.ReadCommandStateAsync().Result);
			System.Console.WriteLine("Single Shot Measurement Bytes : {0} ", ultraSonic.ReadMeasurementBytesAsync().Result);
			Thread.Sleep(50);
			ultraSonic.SetActualZeroAsync(150).Wait();
			Thread.Sleep(50);
			ultraSonic.SingleShotCommandAsync().Wait();
			Thread.Sleep(50);
			System.Console.WriteLine("Single Shot Measurement Bytes : {0} ", ultraSonic.ReadMeasurementBytesAsync().Result);
			System.Console.WriteLine("Measurement Byte[{1}]: {0} ", ultraSonic.ReadMeasurementByteAsync((byte)0).Result, 0);
			System.Console.WriteLine("Command State : {0} ", ultraSonic.ReadCommandStateAsync().Result);
			System.Console.WriteLine("Actual Zero: {0} ", ultraSonic.ReadActualZeroAsync().Result);
			ultraSonic.RequestWarmResetAsync().Wait();
			nxtBrick.DetachSensorAsync((SensorPort)port).Wait();
		}

		public static void UltrasonicInterval(int port, int interval)
		{
			UltraSonicSensor ultraSonic = new UltraSonicSensor();
			nxtBrick.AttachSensorAsync((SensorPort)port, ultraSonic).Wait();
			System.Console.WriteLine("Continious Measurement Interval: {0} ", ultraSonic.ReadContinuousMeasurementIntervalAync().Result);
			Thread.Sleep(10);
			ultraSonic.SetContiniousMeasurementIntervalAsync((byte)interval);
			Thread.Sleep(10);
			System.Console.WriteLine("Continious Measurement Interval: {0} ", ultraSonic.ReadContinuousMeasurementIntervalAync().Result);
			nxtBrick.DetachSensorAsync((SensorPort)port).Wait();
		}

		public static void DigitalSensorInformation(int port)
		{
			UltraSonicSensor ultraSonic = new UltraSonicSensor();
			nxtBrick.AttachSensorAsync((SensorPort)port, ultraSonic).Wait();
			System.Console.WriteLine("Version: {0} ", ultraSonic.ReadVersionAsync().Result);
			System.Console.WriteLine("Product ID: {0} ", ultraSonic.ReadProductIDAsync().Result);
			System.Console.WriteLine("Sensor Type: {0} ", ultraSonic.ReadSensorTypeAsync().Result);
			System.Console.WriteLine("Factory Zero: {0} ", ultraSonic.ReadFactoryZeroAsync().Result);
			System.Console.WriteLine("Factory Scale Factor: {0} ", ultraSonic.ReadFactoryScaleFactorAsync().Result);
			System.Console.WriteLine("Factory Scale Divisor: {0} ", ultraSonic.ReadFactoryScaleDivisorAsync().Result);
			System.Console.WriteLine("Measurement Units: {0} ", ultraSonic.ReadMeasurementUnitsAsync().Result);
			nxtBrick.DetachSensorAsync((SensorPort)port).Wait();
		}

		public static void GetDistance(int port)
		{
			UltraSonicSensor ultraSonic = new UltraSonicSensor();
			nxtBrick.AttachSensorAsync((SensorPort)port, ultraSonic).Wait();
			ultraSonic.AutoPoll(50);
			ultraSonic.TriggerDistance = 50;
			ultraSonic.OnObjectDetected += ultraSonic_OnObjectDetected;
			ultraSonic.OnObjectLost += ultraSonic_OnObjectLost;
			while (!System.Console.KeyAvailable || System.Console.ReadKey().KeyChar != 'c')
			{
//				ultraSonic.Poll().Wait();
//				System.Console.WriteLine("Distance: {0} ", ultraSonic.Distance);
				Thread.Sleep(100);
			}
			nxtBrick.DetachSensorAsync((SensorPort)port).Wait();

		}

		static void ultraSonic_OnObjectLost(object sender, EventArgs e)
		{
			System.Console.WriteLine("Sensor {0}:: Lost at distance {1}", ((SensorBase)sender).SensorPort, ((UltraSonicSensor)sender).Distance);
		}

		static void ultraSonic_OnObjectDetected(object sender, EventArgs e)
		{
			System.Console.WriteLine("Sensor {0}:: Detected at distance {1}", ((SensorBase)sender).SensorPort, ((UltraSonicSensor)sender).Distance);
		}

		static void colorSensor_OnChanged(object sender, EventArgs e)
		{
			System.Console.WriteLine("Sensor {0}:: intensity {1}", ((SensorBase)sender).SensorPort, ((ColorSensor)sender).Intensity);
		}

		static void button_OnReleased(object sender, EventArgs e)
		{
			System.Console.WriteLine("Sensor {0}::1 is released", ((SensorBase)sender).SensorPort);
		}

		static void button_OnChanged(object sender, EventArgs e)
		{
			System.Console.WriteLine("Sensor {0}::1 has changed", ((SensorBase)sender).SensorPort);
		}

		static void button_OnPressed1(object sender, EventArgs e)
		{
			System.Console.WriteLine("Sensor {0}::1 is pressed", ((SensorBase)sender).SensorPort);
		}
		static void button_OnPressed2(object sender, EventArgs e)
		{
			System.Console.WriteLine("Sensor {0}::2 is pressed", ((SensorBase)sender).SensorPort);
		}
	}
}
