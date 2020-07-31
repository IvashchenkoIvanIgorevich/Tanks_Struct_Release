using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    [Flags]
    public enum ActionPlayer
    {
        NoAction = 0x00,
        PressRight = 0x01,
        PressLeft = 0x02,
        PressUp = 0x04,
        PressDown = 0x08,
        PressFire = 0x10,
        PressExit = 0x20,
        PressPause = 0x40,
        PressEnter = 0x80,
        MoveAction = PressRight | PressLeft | PressUp | PressDown,
        StartAction = PressEnter | PressExit
    }
}
