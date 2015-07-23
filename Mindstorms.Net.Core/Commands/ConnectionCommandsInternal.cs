using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core.Commands
{
    public sealed partial class ConnectionCommands : CommandsBase
    {
        internal ConnectionCommands(NxtBrick brick) :
            base(brick)
        {
        }

        private async Task ConnectAsyncInternal()
        {
            await brick.Channel.ConnectAsync();

            //if(pollingTime != TimeSpan.Zero)
            //{
            //    Task t = Task.Factory.StartNew(async () =>
            //    {
            //        while(!tokenSource.IsCancellationRequested)
            //        {
            //            await PollSensorsAsync();
            //            await Task.Delay(pollingTime, tokenSource.Token);
            //        }

            //    }, tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            //}
        }

        private async Task DisconnectAsyncInternal()
        {
            brick.TokenSource.Cancel();
            await brick.Channel.DisconnectAsync();
        }

    }
}
