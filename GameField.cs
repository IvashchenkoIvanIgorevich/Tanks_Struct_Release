using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public struct GameField
    {
        public Tank PlayerTank;
        public Tank[] EnemyTanks;
        public Bullet[] Bullets;
        public int Width;
        public int Height;
        public int sumDeadEnemy;
        public Block[] Blocks;
        public Base FieldBase;
        public Bonus Bonus;
    }
}
