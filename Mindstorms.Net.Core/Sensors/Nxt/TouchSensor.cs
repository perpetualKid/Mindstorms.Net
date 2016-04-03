using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core.Sensors.Nxt
{
	public sealed class TouchSensor: SensorBase
	{
		private GetInputValuesResponse state;

		public TouchSensor():
			base(SensorType.Switch, SensorMode.Boolean)
		{
		}

		public bool Pressed
		{
			get { return this.state != null ? this.state.AsBoolean : false; }
		}

		public event EventHandler<SensorEventArgs> OnPressed;

		public event EventHandler<SensorEventArgs> OnReleased;

		private void OnPressedEventHandler()
		{
			if (null != OnPressed)
				Task.Run(() => OnPressed(this, new SensorEventArgs()));
		}

		private void OnReleasedEventHandler()
		{
			if (null != OnReleased)
				Task.Run(() => OnReleased(this, new SensorEventArgs()));
		}

		internal override async Task PollAsyncInternal()
		{
			if (brick.Connected)
			{
				GetInputValuesResponse previous = state;
				this.state = await brick.DirectCommands.GetInputValuesAsyncInternal(SensorPort);
				if (state != null)
				{
					if (previous != null && previous.AsBoolean != state.AsBoolean)
					{
						OnChangedEventHandler();
					}
					if (state.AsBoolean)
						OnPressedEventHandler();
					else
						OnReleasedEventHandler();
				}
			}

		}
	}
}
