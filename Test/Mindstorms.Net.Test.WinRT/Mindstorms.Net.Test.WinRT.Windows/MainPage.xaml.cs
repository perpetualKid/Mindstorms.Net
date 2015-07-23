using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Mindstorms.Net.Core;
using Mindstorms.Net.WinRT;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Mindstorms.Net.Test.WinRT
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		byte lastError;

		NxtBrick brick;

		public MainPage()
		{
			this.InitializeComponent();
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				brick = new NxtBrick(new BluetoothCommunication("Mindy"));
				await brick.ConnectionCommands.ConnectAsync();
				Output.Text = "Connected";
			}
			catch (Exception ex)
			{
				new MessageDialog("Failed to connect: " + ex).ShowAsync();
			}

		}

		private async void Button_Click_1(object sender, RoutedEventArgs e)
		{
			await brick.ConnectionCommands.DisconnectAsync();
			Output.Text = "Disconnected";

		}

		private async void Button_Click_2(object sender, RoutedEventArgs e)
		{
			await brick.DirectCommands.PlayToneAsync(1000, 5000);
		}

		private async void Button_Click_3(object sender, RoutedEventArgs e)
		{
			Output.Text = String.Format(CultureInfo.InvariantCulture, "Battery> {0}",
				await brick.DirectCommands.GetBatteryLevelAsync());
		}

		private async void Button_Click_4(object sender, RoutedEventArgs e)
		{
			await brick.DirectCommands.PlaySoundFileAsync("! Startup", false);
		}

		private async void Button_Click_5(object sender, RoutedEventArgs e)
		{
			await brick.DirectCommands.StopSoundPlaybackAsync();
		}

		private async void Button_Click_6(object sender, RoutedEventArgs e)
		{
			await brick.DirectCommands.PlaySoundFileAsync("! Attention", true);
		}
	}
}
