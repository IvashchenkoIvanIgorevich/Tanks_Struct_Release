using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public static class InitGame
    {
        public const int START_NUM_ENEMYTANK = 3;
        const int WIDTH_GAMEFIELD = 100;
        const int HEIGHT_GAMEFIELD = 77;
        const int CAPACITY_GAMEBLOCKS = 2475;
        const int WIDTH_TANK = 5;
        const int HEIGHT_TANK = 5;
        const int POSX_BASE = 46;
        const int POSY_BASE = 71;
        const int WIDTH_BASE = 5;
        const int HEIGHT_BASE = 5;
        const int PLAYER_POSX = 31;
        const int PLAYER_POSY = 71;
        const int HP_LIGHT = 400;
        const int HP_HEAVY = 1000;
        const int HP_DESTROY = 700;
        const int ATACK_RANGE_LIGHT = 30;
        const int ATACK_RANGE_HEAVY = 40;
        const int ATACK_RANGE_DESTROY = 30;
        const int ATACK_DAMAGE_LIGHT = 50;
        const int ATACK_DAMAGE_HEAVY = 100;
        const int ATACK_DAMAGE_DESTROY = 200;

        static FormTank lightEnemyForm = new FormTank()
        {
            TankSkin = SkinTank.Light,
            Height = HEIGHT_TANK,
            Width = WIDTH_TANK,
            TankColor = ConsoleColor.Gray
        };

        static FormTank heavyEnemyForm = new FormTank()
        {
            TankSkin = SkinTank.Heavy,
            Height = HEIGHT_TANK,
            Width = WIDTH_TANK,
            TankColor = ConsoleColor.Gray
        };

        static FormTank destroyerEnemyForm = new FormTank()
        {
            TankSkin = SkinTank.Destroy,
            Height = HEIGHT_TANK,
            Width = WIDTH_TANK,
            TankColor = ConsoleColor.Gray
        };

        static FormTank lightPlayForm = new FormTank()
        {
            TankSkin = SkinTank.Light,
            Height = HEIGHT_TANK,
            Width = WIDTH_TANK,
            TankColor = ConsoleColor.Yellow
        };

        static FormTank heavyPlayForm = new FormTank()
        {
            TankSkin = SkinTank.Heavy,
            Height = HEIGHT_TANK,
            Width = WIDTH_TANK,
            TankColor = ConsoleColor.Blue
        };

        static FormTank destroyerPlayForm = new FormTank()
        {
            TankSkin = SkinTank.Destroy,
            Height = HEIGHT_TANK,
            Width = WIDTH_TANK,
            TankColor = ConsoleColor.Green
        };

        public static CharacterTank lightCharacterEnemy = new CharacterTank()
        {
            FormTank = lightEnemyForm,
            HealthPoint = HP_LIGHT,
            MoveSpeed = 1,
            AtackSpeed = 1,
            AtackRange = ATACK_RANGE_LIGHT,
            AtackDamage = ATACK_DAMAGE_LIGHT
        };

        public static CharacterTank heavyCharacterEnemy = new CharacterTank()
        {
            FormTank = heavyEnemyForm,
            HealthPoint = HP_HEAVY,
            MoveSpeed = 1,
            AtackSpeed = 1,
            AtackRange = ATACK_RANGE_HEAVY,
            AtackDamage = ATACK_DAMAGE_HEAVY
        };

        public static CharacterTank destroyerCharacterEnemy = new CharacterTank()
        {
            FormTank = destroyerEnemyForm,
            HealthPoint = HP_DESTROY,
            MoveSpeed = 1,
            AtackSpeed = 1,
            AtackRange = ATACK_RANGE_DESTROY,
            AtackDamage = ATACK_DAMAGE_DESTROY
        };

        public static CharacterTank lightCharacterPlay = new CharacterTank()
        {
            FormTank = lightPlayForm,
            HealthPoint = HP_LIGHT,
            MoveSpeed = 1,
            AtackSpeed = 1,
            AtackRange = ATACK_RANGE_LIGHT,
            AtackDamage = ATACK_DAMAGE_LIGHT
        };

        public static CharacterTank heavyCharacterPlay = new CharacterTank()
        {
            FormTank = heavyPlayForm,
            HealthPoint = HP_HEAVY,
            MoveSpeed = 1,
            AtackSpeed = 1,
            AtackRange = ATACK_RANGE_HEAVY,
            AtackDamage = ATACK_DAMAGE_HEAVY
        };

        public static CharacterTank destroyerCharacterPlay = new CharacterTank()
        {
            FormTank = destroyerPlayForm,
            HealthPoint = HP_DESTROY,
            MoveSpeed = 1,
            AtackSpeed = 1,
            AtackRange = ATACK_RANGE_DESTROY,
            AtackDamage = ATACK_DAMAGE_DESTROY
        };

        static Tank myTank = new Tank()
        {
            PositionX = PLAYER_POSX,
            PositionY = PLAYER_POSY,
            TankCharacter = lightCharacterPlay,
            TankDirection = Direction.Up,
            IsBot = false
        };

        public static Tank[] enemyTank = new Tank[START_NUM_ENEMYTANK];

        static Block[] gameBlock = new Block[CAPACITY_GAMEBLOCKS];

        static Base myGameBase = new Base
        {
            PosX = POSX_BASE,
            PosY = POSY_BASE,
            fieldBase = new char[HEIGHT_BASE, WIDTH_BASE]
        };

        public static GameField myGameField = new GameField()
        {
            PlayerTank = myTank,
            EnemyTanks = new Tank[enemyTank.Length],
            Blocks = gameBlock,
            Width = WIDTH_GAMEFIELD,
            Height = HEIGHT_GAMEFIELD,
            sumDeadEnemy = 0,
            FieldBase = myGameBase
        };
    }
}
