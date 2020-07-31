using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public struct Bullet
    {
        public Direction BulletDirection;
        public int Range;
        public int AtackSpeed;
        public int AtackDamage;
        public int PosX;
        public int PosY;
        public char Skin;
        public bool IsBotBullet;
    }
}
