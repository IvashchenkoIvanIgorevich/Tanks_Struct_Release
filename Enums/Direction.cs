using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    [Flags]
    public enum Direction
    {
        NoDirection = 0x00,
        Right = 0x01,
        Left = 0x02,
        Up = 0x04,
        Down = 0x08
    }
}
