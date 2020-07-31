using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    [Flags]
    public enum SkinBlock
    {
        NoSkin = 0x00,
        Brick = 0x01,
        Metal = 0x02
    }
}
