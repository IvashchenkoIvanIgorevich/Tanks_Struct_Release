using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public struct Tank
    {
        public bool IsBot;
        public bool IsBonus;
        public CharacterTank TankCharacter;
        public Direction TankDirection;
        public int PositionX;
        public int PositionY;
    }
}
