using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Tanks
{
    class BL
    {
        public static Random rndActionEnemy = new Random();
        public static Random rndBonusPosX = new Random();
        public static Random rndBonusPosY = new Random();
        const int NUM_RND_ACTION = 10;
        const int SHIFT_NEW_ENEMY = 8;
        const int NUM_BONUS_TANK = 3;
        const int NUM_RND_BONUS = 15;
        const int BONUS_HP = 100;
        const int BONUS_SPEED = 1;
        const int BONUS_ATACK_SP = 1;
        const int BONUS_ATACK_DM = 100;
        const int NUM_RND_CHARAC = 5;
        const int SHIFT_POSX_NEW_ENEMY = 46; 

        public static void AddEnemyTank(ref GameField gameField, Tank newEnemyTank, ConsoleColor bonus)
        {
            if (gameField.EnemyTanks == null)
            {
                gameField.EnemyTanks = new Tank[] { newEnemyTank };
                return;
            }

            Array.Resize(ref gameField.EnemyTanks, gameField.EnemyTanks.Length + 1);
            gameField.EnemyTanks[gameField.EnemyTanks.Length - 1] = newEnemyTank;

            if (gameField.sumDeadEnemy % NUM_BONUS_TANK == 0)
            {
                gameField.EnemyTanks[gameField.EnemyTanks.Length - 1].IsBonus = true;
            }
        }

        public static void AddBullet(ref GameField gameField, Bullet newBullet)
        {
            if (gameField.Bullets == null)
            {
                gameField.Bullets = new Bullet[] { newBullet };
                return;
            }

            Array.Resize(ref gameField.Bullets, gameField.Bullets.Length + 1);

            gameField.Bullets[gameField.Bullets.Length - 1] = newBullet;
        }

        #region ===---   MoveTanks   ---===

        public static bool IsPosXCorrect(GameField gameField, Tank tank, string condition)
        {
            return (String.Equals(condition, "<=")
                && ((tank.PositionX + tank.TankCharacter.MoveSpeed)
                <= (gameField.Width - tank.TankCharacter.FormTank.Width)))
                || (String.Equals(condition, ">")
                && ((tank.PositionX + tank.TankCharacter.MoveSpeed)
                > (gameField.Width - tank.TankCharacter.FormTank.Width)));
        }

        public static bool IsPosYCorrect(GameField gameField, Tank tank, string condition)
        {
            return (String.Equals(condition, "<=")
                && ((tank.PositionY + tank.TankCharacter.MoveSpeed)
                <= (gameField.Height - tank.TankCharacter.FormTank.Height)))
                || (String.Equals(condition, ">")
                && ((tank.PositionY + tank.TankCharacter.MoveSpeed)
                > (gameField.Height - tank.TankCharacter.FormTank.Height)));
        }

        public static int GetDeltaX(GameField gameField, Tank tank)
        {
            return gameField.Width - tank.TankCharacter.FormTank.Width
                - tank.PositionX - 1;
        }

        public static int GetDeltaY(GameField gameField, Tank tank)
        {
            return gameField.Height - tank.TankCharacter.FormTank.Height
                - tank.PositionY - 1;
        }

        public static void SetPosXY(ref Tank tank, ref int posXY, string operation)
        {
            switch (operation)
            {
                case "+=":
                    if (tank.TankCharacter.MoveSpeed == 1)
                    {
                        posXY += tank.TankCharacter.MoveSpeed;
                    }
                    else
                    {
                        posXY += tank.TankCharacter.MoveSpeed - 1;
                    }
                    break;
                case "-=":
                    if (tank.TankCharacter.MoveSpeed == 1)
                    {
                        posXY -= tank.TankCharacter.MoveSpeed;
                    }
                    else
                    {
                        posXY -= tank.TankCharacter.MoveSpeed - 1;
                    }
                    break;
            }
        }

        public static void MoveTanks(ref GameField gameField, ref Tank tank, ActionPlayer actionPlayer)
        {
            switch (actionPlayer)
            {
                case ActionPlayer.PressRight:
                    if (!tank.TankDirection.HasFlag(Direction.Right))
                    {
                        tank.TankDirection = Direction.Right;
                    }
                    else
                    {
                        if (IsPosXCorrect(gameField, tank, "<="))
                        {
                            SetPosXY(ref tank, ref tank.PositionX, "+=");
                        }
                        if (IsPosXCorrect(gameField, tank, ">"))
                        {
                            tank.PositionX += GetDeltaX(gameField, tank);
                        }
                    }
                    break;
                case ActionPlayer.PressLeft:
                    if (!tank.TankDirection.HasFlag(Direction.Left))
                    {
                        tank.TankDirection = Direction.Left;
                    }
                    else
                    {
                        if ((tank.PositionX - tank.TankCharacter.MoveSpeed) >= 0)
                        {
                            SetPosXY(ref tank, ref tank.PositionX, "-=");
                        }
                        if ((tank.PositionX - tank.TankCharacter.MoveSpeed) < 0)
                        {
                            tank.PositionX -= tank.PositionX - 1;
                        }
                    }
                    break;
                case ActionPlayer.PressUp:
                    if (!tank.TankDirection.HasFlag(Direction.Up))
                    {
                        tank.TankDirection = Direction.Up;
                    }
                    else
                    {
                        if ((tank.PositionY - tank.TankCharacter.MoveSpeed) >= 0)
                        {
                            SetPosXY(ref tank, ref tank.PositionY, "-=");
                        }
                        if ((tank.PositionY - tank.TankCharacter.MoveSpeed) < 0)
                        {
                            tank.PositionY -= tank.PositionY - 1;
                        }
                    }
                    break;
                case ActionPlayer.PressDown:
                    if (!tank.TankDirection.HasFlag(Direction.Down))
                    {
                        tank.TankDirection = Direction.Down;
                    }
                    else
                    {
                        if (IsPosYCorrect(gameField, tank, "<="))
                        {
                            SetPosXY(ref tank, ref tank.PositionY, "+=");
                        }
                        if (IsPosYCorrect(gameField, tank, ">"))
                        {
                            tank.PositionY += GetDeltaY(gameField, tank);
                        }
                    }
                    break;
            }
        }

        #endregion

        #region ===---   MoveBullets   ---===

        public static bool IsBulletCorrect(GameField gameField, Tank tank)
        {
            bool isCorrect = true;

            switch (tank.TankDirection)
            {
                case Direction.Right:
                    if (tank.PositionX + tank.TankCharacter.FormTank.Width
                        >= gameField.Width - 1)
                    {
                        isCorrect = false;
                    }
                    break;
                case Direction.Left:
                    if (tank.PositionX <= 1)
                    {
                        isCorrect = false;
                    }
                    break;
                case Direction.Up:
                    if (tank.PositionY <= 1)
                    {
                        isCorrect = false;
                    }
                    break;
                case Direction.Down:
                    if (tank.PositionY + tank.TankCharacter.FormTank.Height
                        >= gameField.Height - 1)
                    {
                        isCorrect = false;
                    }
                    break;
                case Direction.NoDirection:
                    break;
            }

            return isCorrect;
        }

        public static void AddBulletToGameField(ref GameField newGameField, ref Tank tank)
        {
            int posX = 0;
            int posY = 0;

            if (!IsBulletCorrect(newGameField, tank))
            {
                return;
            }

            switch (tank.TankDirection)
            {
                case Direction.Right:
                    posX = tank.PositionX + tank.TankCharacter.FormTank.Width;
                    posY = tank.PositionY + (tank.TankCharacter.FormTank.Height / 2);
                    break;
                case Direction.Left:
                    posX = tank.PositionX - 1;
                    posY = tank.PositionY + (tank.TankCharacter.FormTank.Height / 2);
                    break;
                case Direction.Up:
                    posX = tank.PositionX + (tank.TankCharacter.FormTank.Width / 2);
                    posY = tank.PositionY - 1;
                    break;
                case Direction.Down:
                    posX = tank.PositionX + (tank.TankCharacter.FormTank.Width / 2);
                    posY = tank.PositionY + tank.TankCharacter.FormTank.Height;
                    break;
                case Direction.NoDirection:
                    break;
            }

            Bullet newBullet = new Bullet()
            {
                BulletDirection = tank.TankDirection,
                Range = tank.TankCharacter.AtackRange,
                AtackSpeed = tank.TankCharacter.AtackSpeed,
                AtackDamage = tank.TankCharacter.AtackDamage,
                Skin = '*',
                PosX = posX,
                PosY = posY,
                IsBotBullet = tank.IsBot
            };

            AddBullet(ref newGameField, newBullet);
        }

        public static void DeleteBullet(ref GameField gameField, int numDeleteBullet)
        {
            bool deleteBullet = false;

            for (int numBullet = 0; numBullet < gameField.Bullets.Length - 1; numBullet++)
            {
                if (numDeleteBullet == numBullet)
                {
                    deleteBullet = true;
                }

                if (deleteBullet)
                {
                    gameField.Bullets[numBullet] = gameField.Bullets[numBullet + 1];
                }
            }

            Array.Resize(ref gameField.Bullets, gameField.Bullets.Length - 1);
        }

        public static bool IsBulletInField(Bullet bullet, int gameFieldWidth, int gameFieldHeight)
        {
            return ((bullet.Range > 0) && (bullet.PosX + bullet.AtackSpeed < gameFieldWidth - 1)
                && (bullet.PosY + bullet.AtackSpeed < gameFieldHeight - 1)
                && (bullet.PosX - bullet.AtackSpeed > 0) && (bullet.PosY - bullet.AtackSpeed > 0));
        }

        public static bool IsBulletSameBlock(Bullet bullet, Block[] block)
        {
            bool isSame = false;

            for (int numBlock = 0; numBlock < block.Length; numBlock++)
            {
                if ((bullet.PosX == block[numBlock].PosX)
                    && (bullet.PosY == block[numBlock].PosY))
                {
                    isSame = true;
                    break;
                }
            }

            return isSame;
        }

        public static bool IsBlockMetal(Bullet bullet, Block[] block)
        {
            bool isMetal = false;

            for (int numBlock = 0; numBlock < block.Length; numBlock++)
            {
                if ((bullet.PosX == block[numBlock].PosX)
                    && (bullet.PosY == block[numBlock].PosY)
                    && (block[numBlock].SkinBlock == SkinBlock.Metal))
                {
                    isMetal = true;
                    break;
                }
            }

            return isMetal;
        }

        public static bool IsBulletSameBase(Bullet bullet, Base gameBase)
        {
            bool isSame = false;

            for (int numBaseR = 0; numBaseR < gameBase.fieldBase.GetLength(0); numBaseR++)
            {
                for (int numBaseL = 0; numBaseL  < gameBase.fieldBase.GetLength(1); numBaseL++)
                {
                    if ((bullet.PosX == gameBase.PosX + numBaseL) 
                        && (bullet.PosY == gameBase.PosY + numBaseR))
                    {
                        isSame = true;
                        break;
                    }
                }
            }

            return isSame;
        }

        public static void MoveBullet(ref GameField gameField)
        {
            for (int numBullet = 0; numBullet < gameField.Bullets.Length; numBullet++)
            {
                if (IsBulletInField(gameField.Bullets[numBullet], gameField.Width, gameField.Height)
                && (!IsEnemyTakeAndSetDamage(ref gameField, numBullet))
                && (!IsPlayerTakeAndSetDamage(ref gameField, numBullet))
                && (!IsBulletSameBlock(gameField.Bullets[numBullet], gameField.Blocks))
                && (!IsBulletSameBase(gameField.Bullets[numBullet], gameField.FieldBase)))
                {
                    switch (gameField.Bullets[numBullet].BulletDirection)
                    {
                        case Direction.Right:
                            GetViewBullet(ref gameField.Bullets[numBullet], ref gameField.Bullets[numBullet].PosX);
                            break;
                        case Direction.Left:
                            GetViewBullet(ref gameField.Bullets[numBullet], ref gameField.Bullets[numBullet].PosX);
                            break;
                        case Direction.Up:
                            GetViewBullet(ref gameField.Bullets[numBullet], ref gameField.Bullets[numBullet].PosY);
                            break;
                        case Direction.Down:
                            GetViewBullet(ref gameField.Bullets[numBullet], ref gameField.Bullets[numBullet].PosY);
                            break;
                        case Direction.NoDirection:
                            break;
                    }
                }
                else
                {
                    if (IsBulletSameBlock(gameField.Bullets[numBullet], gameField.Blocks))
                    {
                        int numDeleteBlock = GetNumDeleteBlock(gameField.Bullets[numBullet], gameField.Blocks);

                        if (gameField.Blocks[numDeleteBlock].SkinBlock == SkinBlock.Brick)
                        {
                            DeleteBlock(ref gameField, numDeleteBlock);
                        }                        
                    }

                    DeleteBullet(ref gameField, numBullet);
                }
            }
        }
      
        public static int GetNumDeleteBlock(Bullet bullet, Block[] block)
        {
            int numDelBlock = -1;

            for (int numBlock = 0; numBlock < block.Length; numBlock++)
            {
                if ((bullet.PosX == block[numBlock].PosX)
                    && (bullet.PosY == block[numBlock].PosY))
                {
                    numDelBlock = numBlock;
                    break;
                }
            }

            return numDelBlock;
        }

        public static void DeleteBlock(ref GameField gameField, int numDeleteBlock)
        {
            for (int numBlock = numDeleteBlock; numBlock < gameField.Blocks.Length - 1; numBlock++)
            {
                gameField.Blocks[numBlock] = gameField.Blocks[numBlock + 1];
            }

            Array.Resize(ref gameField.Blocks, gameField.Blocks.Length - 1);
        }

        public static void GetViewBullet(ref Bullet bullet, ref int posXY)
        {
            if (bullet.BulletDirection.HasFlag(Direction.Right)
                || bullet.BulletDirection.HasFlag(Direction.Down))
            {
                posXY += bullet.AtackSpeed;
            }
            if (bullet.BulletDirection.HasFlag(Direction.Left)
                || bullet.BulletDirection.HasFlag(Direction.Up))
            {
                posXY -= bullet.AtackSpeed;
            }
        }

        #endregion        

        #region ===---   TakeDamage   ---===

        public static bool IsEnemyDamageEnemy(GameField gameField)
        {
            bool isEnemyDamageEnemy = false;

            for (int numBullet = 0; numBullet < gameField.Bullets.Length; numBullet++)
            {
                for (int numEnemyTank = 0; numEnemyTank < gameField.EnemyTanks.Length; numEnemyTank++)
                {
                    if (IsTakeDamage(gameField.EnemyTanks[numEnemyTank], gameField.Bullets[numBullet])
                        && gameField.Bullets[numBullet].IsBotBullet)
                    {
                        isEnemyDamageEnemy = true;
                        break;
                    }
                }

                if (isEnemyDamageEnemy)
                {
                    break;
                }
            }

            return isEnemyDamageEnemy;
        }

        public static bool IsEnemyTakeAndSetDamage(ref GameField gameField, int numBullet)
        {
            bool isEnemyDamage = false;

            for (int numEnemyTank = 0; numEnemyTank < gameField.EnemyTanks.Length; numEnemyTank++)
            {
                if (IsTakeDamage(gameField.EnemyTanks[numEnemyTank], gameField.Bullets[numBullet])
                    && !gameField.Bullets[numBullet].IsBotBullet)
                {
                    gameField.EnemyTanks[numEnemyTank].TankCharacter.HealthPoint
                        -= gameField.Bullets[numBullet].AtackDamage;
                    isEnemyDamage = true;
                    break;
                }

                if (isEnemyDamage)
                {
                    break;
                }
            }

            return isEnemyDamage;
        }

        public static bool IsTakeDamage(Tank playerTank, Bullet bullet)
        {
            bool isTakeDamage = false;

            for (int posY = 0; posY < playerTank.TankCharacter.FormTank.Height; posY++)
            {
                for (int posX = 0; posX < playerTank.TankCharacter.FormTank.Width; posX++)
                {
                    if ((bullet.PosX == playerTank.PositionX + posX)
                        && (bullet.PosY == playerTank.PositionY + posY))
                    {
                        isTakeDamage = true;
                        break;
                    }
                }

                if (isTakeDamage)
                {
                    break;
                }
            }

            return isTakeDamage;
        }

        public static bool IsPlayerTakeAndSetDamage(ref GameField gameField, int numBullet)
        {
            bool isPlayerTakeDamage = false;

            if (IsTakeDamage(gameField.PlayerTank, gameField.Bullets[numBullet]))
            {
                gameField.PlayerTank.TankCharacter.HealthPoint -= gameField.Bullets[numBullet].AtackDamage;
                isPlayerTakeDamage = true;
            }

            return isPlayerTakeDamage;
        }

        #endregion

        public static void TryMoveForwardTank(ref Tank tank, ActionPlayer actionPlayer)
        {
            switch (actionPlayer)
            {                
                case ActionPlayer.PressRight:
                    if (tank.TankDirection.HasFlag(Direction.Right))
                    {
                        tank.PositionX += tank.TankCharacter.MoveSpeed;
                    }
                    break;
                case ActionPlayer.PressLeft:
                    if (tank.TankDirection.HasFlag(Direction.Left))
                    {
                        tank.PositionX -= tank.TankCharacter.MoveSpeed;
                    }
                    break;
                case ActionPlayer.PressUp:
                    if (tank.TankDirection.HasFlag(Direction.Up))
                    {
                        tank.PositionY -= tank.TankCharacter.MoveSpeed;
                    }
                    break;
                case ActionPlayer.PressDown:
                    if (tank.TankDirection.HasFlag(Direction.Down))
                    {
                        tank.PositionY += tank.TankCharacter.MoveSpeed;
                    }
                    break;
                case ActionPlayer.NoAction:
                    break;
            }
        }

        public static void TryMoveBackwardTank(ref Tank tank, ActionPlayer actionPlayer)
        {
            switch (actionPlayer)
            {
                case ActionPlayer.PressRight:
                    if (tank.TankDirection.HasFlag(Direction.Right))
                    {
                        tank.PositionX -= tank.TankCharacter.MoveSpeed;
                    }
                    break;
                case ActionPlayer.PressLeft:
                    if (tank.TankDirection.HasFlag(Direction.Left))
                    {
                        tank.PositionX += tank.TankCharacter.MoveSpeed;
                    }
                    break;
                case ActionPlayer.PressUp:
                    if (tank.TankDirection.HasFlag(Direction.Up))
                    {
                        tank.PositionY += tank.TankCharacter.MoveSpeed;
                    }
                    break;
                case ActionPlayer.PressDown:
                    if (tank.TankDirection.HasFlag(Direction.Down))
                    {
                        tank.PositionY -= tank.TankCharacter.MoveSpeed;
                    }
                    break;
                case ActionPlayer.NoAction:
                    break;
            }
        }

        public static bool IsPosPlayerSameEnemy(GameField gameField, ActionPlayer actionPlayer)
        {
            bool isSame = false;

            if (gameField.EnemyTanks == null)
            {
                return isSame;
            }

            TryMoveForwardTank(ref gameField.PlayerTank, actionPlayer);

            for (int numEnemy = 0; numEnemy < gameField.EnemyTanks.Length; numEnemy++)
            {
                for (int i = 0; i < gameField.PlayerTank.TankCharacter.FormTank.Height; i++)
                {
                    for (int j = 0; j < gameField.PlayerTank.TankCharacter.FormTank.Width; j++)
                    {
                        if (IsSamePlayerEnemyPosXY(gameField.EnemyTanks[numEnemy], 
                            gameField.PlayerTank.PositionX + j, 
                            gameField.PlayerTank.PositionY + i))
                        {
                            isSame = true;
                            break;
                        }
                    }

                    if (isSame)
                    {
                        break;
                    }
                }

                if (isSame)
                {
                    break;
                }
            }

            TryMoveBackwardTank(ref gameField.PlayerTank, actionPlayer);

            return isSame;
        }    

        public static bool IsPosEnemySamePlayer(GameField gameField, ActionPlayer actionEnemy, int numEnemy)
        {
            bool isSame = false;

            if (gameField.EnemyTanks == null)
            {
                return isSame;
            }

            TryMoveForwardTank(ref gameField.EnemyTanks[numEnemy], actionEnemy);

            for (int posY = 0; posY < gameField.PlayerTank.TankCharacter.FormTank.Height; posY++)
            {
                for (int posX = 0; posX < gameField.PlayerTank.TankCharacter.FormTank.Width; posX++)
                {
                    if (IsSamePlayerEnemyPosXY(gameField.EnemyTanks[numEnemy], 
                        gameField.PlayerTank.PositionX + posX, gameField.PlayerTank.PositionY + posY))
                    {
                        isSame = true;
                        break;
                    }
                }

                if (isSame)
                {
                    break;
                }
            }

            TryMoveBackwardTank(ref gameField.EnemyTanks[numEnemy], actionEnemy);

            return isSame;
        }

        public static bool IsPosEnemySameEnemy(GameField gameField, ActionPlayer actionEnemy, int numSameEnemy)
        {
            bool isSame = false;

            TryMoveForwardTank(ref gameField.EnemyTanks[numSameEnemy], actionEnemy);

            for (int numEnemy = 0; numEnemy < gameField.EnemyTanks.Length; numEnemy++)
            {
                if (numEnemy == numSameEnemy)
                {
                    continue;
                }

                if (numEnemy >= gameField.EnemyTanks.Length)
                {
                    break;
                }

                for (int posY = 0; posY < gameField.EnemyTanks[numSameEnemy].TankCharacter.FormTank.Height; posY++)
                {
                    for (int posX = 0; posX < gameField.EnemyTanks[numSameEnemy].TankCharacter.FormTank.Width; posX++)
                    {
                        if (IsSamePlayerEnemyPosXY(gameField.EnemyTanks[numEnemy], gameField.EnemyTanks[numSameEnemy].PositionX + posX, 
                            gameField.EnemyTanks[numSameEnemy].PositionY + posY))
                        {
                            isSame = true;
                            break;
                        }
                    }

                    if (isSame)
                    {
                        break;
                    }
                }

                if (isSame)
                {
                    break;
                }
            }

            TryMoveBackwardTank(ref gameField.EnemyTanks[numSameEnemy], actionEnemy);

            return isSame;
        }

        public static bool IsSamePlayerEnemyPosXY(Tank tank, int playerPosX, int playerPosY)
        {
            bool isSame = false;

            for (int posY = 0; posY < tank.TankCharacter.FormTank.Height; posY++)
            {
                for (int posX = 0; posX < tank.TankCharacter.FormTank.Width; posX++)
                {
                    if ((playerPosX == tank.PositionX + posX) && (playerPosY == tank.PositionY + posY))
                    {
                        isSame = true;
                        break;
                    }
                }

                if (isSame)
                {
                    break;
                }
            }

            return isSame;
        }

        public static bool IsPosEnemyRegardingField(GameField gameField, ActionPlayer actionEnemy, int numEnemy)
        {
            bool isSame = false;

            TryMoveForwardTank(ref gameField.EnemyTanks[numEnemy], actionEnemy);

            for (int posY = 0; posY < gameField.EnemyTanks[numEnemy].TankCharacter.FormTank.Height; posY++)
            {
                for (int posX = 0; posX < gameField.EnemyTanks[numEnemy].TankCharacter.FormTank.Width; posX++)
                {
                    if ((gameField.EnemyTanks[numEnemy].PositionX + posX <= 0) || 
                        (gameField.EnemyTanks[numEnemy].PositionY + posY <= 0) || 
                        (gameField.EnemyTanks[numEnemy].PositionX + posX >= gameField.Width - 1) || 
                        (gameField.EnemyTanks[numEnemy].PositionY + posY >= gameField.Height - 1))
                    {
                        isSame = true;
                        break;
                    }
                }

                if (isSame)
                {
                    break;
                }
            }

            TryMoveBackwardTank(ref gameField.EnemyTanks[numEnemy], actionEnemy);

            return isSame;
        }

        public static bool IsPosPlayerSameBlock(GameField gameField, ActionPlayer actionPlayer)
        {
            bool isSame = false;

            TryMoveForwardTank(ref gameField.PlayerTank, actionPlayer);

            for (int numBlock = 0; numBlock < gameField.Blocks.Length; numBlock++)
            {
                if (IsSamePlayerEnemyPosXY(gameField.PlayerTank, gameField.Blocks[numBlock].PosX, gameField.Blocks[numBlock].PosY))
                {
                    isSame = true;
                    break;
                }
            }

            TryMoveBackwardTank(ref gameField.PlayerTank, actionPlayer);

            return isSame;
        }

        public static bool IsPosPlayerSameBase(GameField gameField, ActionPlayer actionPlayer)
        {
            bool isSame = false;

            TryMoveForwardTank(ref gameField.PlayerTank, actionPlayer);

            for (int numBaseR = 0; numBaseR < gameField.FieldBase.fieldBase.GetLength(0); numBaseR++)
            {
                for (int numBasesL = 0; numBasesL < gameField.FieldBase.fieldBase.GetLength(1); numBasesL++)
                {
                    if (IsSamePlayerEnemyPosXY(gameField.PlayerTank, 
                        gameField.FieldBase.PosX + numBasesL, gameField.FieldBase.PosY + numBaseR))
                    {
                        isSame = true;
                        break;
                    }
                }

                if (isSame)
                {
                    break;
                }
            }

            TryMoveBackwardTank(ref gameField.PlayerTank, actionPlayer);

            return isSame;
        }

        public static bool IsPosEnemySameBlock(GameField gameField, ActionPlayer actionPlayer, int numEnemy)
        {
            bool isSame = false;

            if (gameField.EnemyTanks == null)
            {
                return isSame;
            }

            TryMoveForwardTank(ref gameField.EnemyTanks[numEnemy], actionPlayer);

            for (int numBlock = 0; numBlock < gameField.Blocks.Length; numBlock++)
            {
                if (IsSamePlayerEnemyPosXY(gameField.EnemyTanks[numEnemy],
                    gameField.Blocks[numBlock].PosX, gameField.Blocks[numBlock].PosY))
                {
                    isSame = true;
                    break;
                }
            }

            TryMoveBackwardTank(ref gameField.EnemyTanks[numEnemy], actionPlayer);

            return isSame;
        }

        public static bool IsPosEnemySameBases(GameField gameField, ActionPlayer actionPlayer, int numEnemy)
        {
            bool isSame = false;

            if (gameField.EnemyTanks == null)
            {
                return isSame;
            }

            TryMoveForwardTank(ref gameField.EnemyTanks[numEnemy], actionPlayer);

            for (int numBaseR = 0; numBaseR < gameField.FieldBase.fieldBase.GetLength(0); numBaseR++)
            {
                for (int numBaseL = 0; numBaseL < gameField.FieldBase.fieldBase.GetLength(1); numBaseL++)
                {
                    if (IsSamePlayerEnemyPosXY(gameField.EnemyTanks[numEnemy], 
                        gameField.FieldBase.PosX + numBaseL, gameField.FieldBase.PosY + numBaseR))
                    {
                        isSame = true;
                        break;
                    }
                }

                if (isSame)
                {
                    break;
                }
            }

            TryMoveBackwardTank(ref gameField.EnemyTanks[numEnemy], actionPlayer);

            return isSame;
        }

        public static ActionPlayer GetRandomAction(ref ActionPlayer actionEnemy)
        {
            int tempAction = rndActionEnemy.Next(0, NUM_RND_ACTION);

            switch (tempAction)
            {
                case 0:
                    actionEnemy = ActionPlayer.PressRight;
                    break;
                case 1:
                    actionEnemy = ActionPlayer.PressLeft;
                    break;
                case 2:
                    actionEnemy = ActionPlayer.PressUp;
                    break;
                case 3:
                    actionEnemy = ActionPlayer.PressDown;
                    break;
                case 4:
                    actionEnemy = ActionPlayer.PressUp;
                    break;
                case 5:
                    actionEnemy = ActionPlayer.PressDown;
                    break;
                case 6:
                    actionEnemy = ActionPlayer.PressRight;
                    break;
                case 7:
                    actionEnemy = ActionPlayer.PressRight;
                    break;
                case 8:
                    actionEnemy = ActionPlayer.PressLeft;
                    break;
                case 9:
                    actionEnemy = ActionPlayer.PressLeft;
                    break;
                case 10:
                    actionEnemy = ActionPlayer.PressRight;
                    break;
            }

            return actionEnemy;
        }

        public static bool IsEnemyDeath(Tank[] tank, out int indexDeath)
        {
            bool deathTank = false;

            for (indexDeath = 0; indexDeath < tank.Length; indexDeath++)
            {
                if (tank[indexDeath].TankCharacter.HealthPoint <= 0) 
                {
                    deathTank = true;
                    break;
                }
            }

            return deathTank;
        }

        public static void SetEnemyDeath(ref Tank[] tank, int indexDeath)
        {
            bool isDeath = false;

            for (int numEnemy = 0; numEnemy < tank.Length; numEnemy++)
            {
                if ((numEnemy == indexDeath) || isDeath)
                {
                    if (numEnemy != tank.Length - 1)
                    {
                        tank[numEnemy] = tank[numEnemy + 1];                        
                        isDeath = true;
                    }
                    else
                    {
                        Array.Resize(ref tank, tank.Length - 1);
                    }
                }
            }
        }

        public static bool IsPlayerDeath(Tank tank)
        {
            return (tank.TankCharacter.HealthPoint <= 0);
        }

        public static bool IsEnemySameNewEnemy(Tank[] tank, int posX, int posY)
        {
            bool isSame = false;

            for (int numEnemy = 0; numEnemy < tank.Length; numEnemy++)
            {
                for (int posH = 0; posH < tank[numEnemy].TankCharacter.FormTank.Height; posH++)
                {
                    for (int posW = 0; posW < tank[numEnemy].TankCharacter.FormTank.Width; posW++)
                    {
                        if ((posX == tank[numEnemy].PositionX + posW) && (posY == tank[numEnemy].PositionY + posH))
                        {
                            isSame = true;
                            break;
                        }
                    }

                    if (isSame)
                    {
                        break;
                    }
                }

                if (isSame)
                {
                    break;
                }
            }

            return isSame;
        }

        public static void GetNewEnemyTankPosXY(ref GameField gameField, out int startX, out int startY)
        {
            int shiftX = gameField.PlayerTank.TankCharacter.FormTank.Width;
            int shiftY = gameField.PlayerTank.TankCharacter.FormTank.Height;
            startX = 1;
            startY = 1;

            for (int posY = startY; posY < shiftY; posY++)
            {
                for (int posX = startX; posX < shiftX; posX++)
                {
                    if (IsSamePlayerEnemyPosXY(gameField.PlayerTank, posX, posY)
                        || IsEnemySameNewEnemy(gameField.EnemyTanks, posX, posY))
                    {
                        shiftX += gameField.PlayerTank.TankCharacter.FormTank.Width * SHIFT_NEW_ENEMY;
                        startX += gameField.PlayerTank.TankCharacter.FormTank.Width * SHIFT_NEW_ENEMY;
                        posY = startY - 1;
                        break;
                    }                    
                }

                if (shiftX >= gameField.Width)
                {
                    shiftX = gameField.PlayerTank.TankCharacter.FormTank.Width;
                    shiftY += gameField.PlayerTank.TankCharacter.FormTank.Height;
                    startY += gameField.PlayerTank.TankCharacter.FormTank.Height;
                }
            }
        }

        #region ===---   Blocks   ---===

        public static void CreatBlock(ref GameField gameField, ref int numBlock, int posX, int posY, 
            int numW, int numH, SkinBlock skinBlock)
        {
            int width = gameField.PlayerTank.TankCharacter.FormTank.Width * numW;
            int height = gameField.PlayerTank.TankCharacter.FormTank.Height * numH;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    gameField.Blocks[numBlock].PosX = posX + j;
                    gameField.Blocks[numBlock].PosY = posY + i;

                    switch (skinBlock)
                    {
                        case SkinBlock.Brick:
                            gameField.Blocks[numBlock].SkinBlock = SkinBlock.Brick;
                            break;
                        case SkinBlock.Metal:
                            gameField.Blocks[numBlock].SkinBlock = SkinBlock.Metal;
                            break;
                        case SkinBlock.NoSkin:
                            break;
                    }
                    ++numBlock;
                }
            }
        }

        public static void CreatAllBlocks(ref GameField gameField)
        {
            int numBlock = 0;

            CreatBlock(ref gameField, ref numBlock, 6, 6, 1, 6, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 16, 6, 1, 6, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 26, 6, 1, 6, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 36, 6, 1, 5, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 41, 22, 3, 1, SkinBlock.Metal);
            CreatBlock(ref gameField, ref numBlock, 56, 6, 1, 5, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 66, 6, 1, 6, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 76, 6, 1, 6, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 86, 6, 1, 6, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 36, 36, 1, 1, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 56, 36, 1, 1, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 1, 41, 1, 1, SkinBlock.Metal);
            CreatBlock(ref gameField, ref numBlock, 11, 41, 4, 1, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 66, 41, 4, 1, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 94, 41, 1, 1, SkinBlock.Metal);
            CreatBlock(ref gameField, ref numBlock, 6, 51, 1, 4, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 16, 51, 1, 4, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 26, 51, 1, 4, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 66, 51, 1, 4, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 76, 51, 1, 4, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 86, 51, 1, 4, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 36, 46, 1, 3, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 56, 46, 1, 3, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 41, 51, 3, 1, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 41, 66, 1, 2, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 51, 66, 1, 2, SkinBlock.Brick);
            CreatBlock(ref gameField, ref numBlock, 46, 66, 1, 1, SkinBlock.Brick);
        }

        #endregion

        #region ===---   Bonus   ---===

        public static void GetBonusPos(ref GameField gameField, int indexDeath)
        {
            if (gameField.EnemyTanks[indexDeath].IsBonus)
            {
                gameField.Bonus.PosX = rndBonusPosX.Next(1, gameField.Width - 1);
                gameField.Bonus.PosY = rndBonusPosX.Next(1, gameField.Height - 1);
            }
        }

        public static bool IsPosPlayerSameBonus(GameField gameField, ActionPlayer actionPlayer)
        {
            bool isSame = false;

            if (IsSamePlayerEnemyPosXY(gameField.PlayerTank,
                gameField.Bonus.PosX, gameField.Bonus.PosY))
            {
                isSame = true;
            }

            return isSame;
        }

        public static void SetPosBonus(ref GameField gameField, ActionPlayer actionPlayer)
        {
            if (IsPosPlayerSameBonus(gameField, actionPlayer))
            {
                gameField.Bonus.PosX = 0;
                gameField.Bonus.PosY = 0;
            }
        }

        public static SkinBonus GetRandomBonus()
        {
            int randomNum = rndActionEnemy.Next(0, NUM_RND_BONUS);
            SkinBonus skin = SkinBonus.NoBonus;

            switch (randomNum)
            {
                case 0:
                    skin = SkinBonus.AtackDamage;
                    break;
                case 1:
                    skin = SkinBonus.HP;
                    break;
                case 2:
                    skin = SkinBonus.AtackDamage;
                    break;
                case 3:
                    skin = SkinBonus.HP;
                    break;
                case 4:
                    skin = SkinBonus.HP;
                    break;
                case 5:
                    skin = SkinBonus.HP;
                    break;
                case 6:
                    skin = SkinBonus.LevelUp;
                    break;
                case 7:
                    skin = SkinBonus.AtackDamage;
                    break;
                case 8:
                    skin = SkinBonus.AtackDamage;
                    break;
                case 9:
                    skin = SkinBonus.LevelUp;
                    break;
                case 10:
                    skin = SkinBonus.LevelUp;
                    break;
                case 11:
                    skin = SkinBonus.LevelUp;
                    break;
                case 12:
                    skin = SkinBonus.AtackDamage;
                    break;
                case 13:
                    skin = SkinBonus.LevelUp;
                    break;
                case 14:
                    skin = SkinBonus.LevelUp;
                    break;
                case 15:
                    skin = SkinBonus.AtackDamage;
                    break;
            }

            return skin;
        }

        public static void SetPlayerBonus(ref GameField gameField, ActionPlayer actionPlayer, 
            CharacterTank lightTank, CharacterTank heavyTank, CharacterTank destroyerTank)
        {
            if (IsPosPlayerSameBonus(gameField, actionPlayer))
            {
                SkinBonus newSkin = GetRandomBonus();

                switch (newSkin)
                {
                    case SkinBonus.HP:
                        gameField.PlayerTank.TankCharacter.HealthPoint += BONUS_HP;
                        break;
                    case SkinBonus.AtackDamage:
                        gameField.PlayerTank.TankCharacter.AtackDamage += BONUS_ATACK_DM;
                        break;
                    case SkinBonus.LevelUp:
                        if (gameField.PlayerTank.TankCharacter.FormTank.TankSkin == SkinTank.Heavy)
                        {
                            gameField.PlayerTank.TankCharacter = destroyerTank;
                        }
                        if (gameField.PlayerTank.TankCharacter.FormTank.TankSkin == SkinTank.Light)
                        {
                            gameField.PlayerTank.TankCharacter = heavyTank;
                        }
                        break;
                    case SkinBonus.NoBonus:
                        break;
                }
            }
        }

        #endregion        

        public static CharacterTank GetRandomCharacter(CharacterTank lightTank, CharacterTank heavyTank, CharacterTank destroyerTank)
        {
            int randomNum = rndActionEnemy.Next(0, NUM_RND_CHARAC);
            CharacterTank characterTank = lightTank;

            switch (randomNum)
            {
                case 1:
                    characterTank = lightTank;
                    break;
                case 2:
                    characterTank = destroyerTank;
                    break;
                case 3:
                    characterTank = heavyTank;
                    break;
                case 4:
                    characterTank = destroyerTank;
                    break;
                case 5:
                    characterTank = lightTank;
                    break;
            }

            return characterTank;
        }

        public static void CreateStartEnemy(ref GameField gameField, int startNumEnemy, 
            CharacterTank lightTank, CharacterTank heavyTank, CharacterTank destroyerTank)
        {
            for (int numEnemy = 0; numEnemy < startNumEnemy; numEnemy++)
            {
                gameField.EnemyTanks[numEnemy] = new Tank
                {
                    PositionX = numEnemy * SHIFT_POSX_NEW_ENEMY + 1,
                    PositionY = 1,
                    TankCharacter = GetRandomCharacter(lightTank, heavyTank, destroyerTank),
                    TankDirection = Direction.Down,
                    IsBot = true
                };
            }
        }
    }
}
