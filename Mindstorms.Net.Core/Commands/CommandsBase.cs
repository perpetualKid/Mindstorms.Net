using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms.Net.Core.Commands
{
    public class CommandsBase
    {
        protected NxtBrick brick;

        internal CommandsBase(NxtBrick brick)
        {
            this.brick = brick;
        }

    }
}
