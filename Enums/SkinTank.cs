using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    [Flags]
    public enum SkinTank
    {
        NoSkin = 0x00,
        Light = 0x01,
        Heavy = 0x02,
        Destroy = 0x04
    }
}
