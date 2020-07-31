using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Tanks
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TIME_MOVE_ENEMY = 30;
            const int TIME_DIRECT_ENEMY = 2000;
            const int TIME_CREATE_BULLET = 500;
            const int TIME_MOVE_BULLET = 3;
            const int TIME_SLEEP = 3;

            ulong gameTime = 0;
            int indexDeath = -1;
            bool exit = false;

            ActionPlayer action = ActionPlayer.NoAction;
            ActionPlayer[] actionEnemy = new ActionPlayer[InitGame.START_NUM_ENEMYTANK];

            UI.GetViewStartWindow(InitGame.myGameField);

            do
            {
                action = UI.GetActionPlayer(Console.ReadKey(true).Key);
            }
            while ((action & ActionPlayer.StartAction) == 0);

            if (ActionPlayer.PressEnter.HasFlag(action))
            {
                BL.CreatAllBlocks(ref InitGame.myGameField);
                UI.GetViewGameBase(ref InitGame.myGameField.FieldBase);
                UI.GetViewRigthBar(InitGame.myGameField);
                UI.PrintGameField(ref InitGame.myGameField);
                UI.PrintBlocks(ref InitGame.myGameField);
                UI.PrintBase(InitGame.myGameField.FieldBase);
                UI.PrintTank(ref InitGame.myGameField.PlayerTank);
                BL.CreateStartEnemy(ref InitGame.myGameField, InitGame.START_NUM_ENEMYTANK,
                    InitGame.lightCharacterEnemy, InitGame.heavyCharacterEnemy, 
                    InitGame.destroyerCharacterEnemy);
            }

            Stopwatch watch = new Stopwatch();

            watch.Start();

            do
            {
                if (Console.KeyAvailable)
                {
                    action = UI.GetActionPlayer(Console.ReadKey(true).Key);

                    if (action == ActionPlayer.PressPause)
                    {
                        UI.GetViewMessageTheGame( "PAUSE!!!", "PRESS ANY KEY TO CONTINUE");
                    }

                    if (((action & ActionPlayer.MoveAction) > 0) 
                        && (!BL.IsPosPlayerSameEnemy(InitGame.myGameField, action)) 
                        && (!BL.IsPosPlayerSameBlock(InitGame.myGameField, action))
                        && (!BL.IsPosPlayerSameBase(InitGame.myGameField, action)))
                    {
                        UI.HideTank(ref InitGame.myGameField.PlayerTank);
                        BL.MoveTanks(ref InitGame.myGameField, ref InitGame.myGameField.PlayerTank, action);
                        UI.PrintTank(ref InitGame.myGameField.PlayerTank);
                    }

                    if (action == ActionPlayer.PressFire)
                    {
                        BL.AddBulletToGameField(ref InitGame.myGameField, ref InitGame.myGameField.PlayerTank);
                    }
                }

                if (gameTime % TIME_MOVE_ENEMY == 0)
                {
                    for (int numEnemy = 0; numEnemy < InitGame.myGameField.EnemyTanks.Length; numEnemy++)
                    {
                        if (!BL.IsPosEnemySamePlayer(InitGame.myGameField, actionEnemy[numEnemy], numEnemy) 
                            && (!BL.IsPosEnemyRegardingField(InitGame.myGameField, actionEnemy[numEnemy], numEnemy)) 
                            && (!BL.IsPosEnemySameEnemy(InitGame.myGameField, actionEnemy[numEnemy], numEnemy)) 
                            && (!BL.IsPosEnemySameBlock(InitGame.myGameField, actionEnemy[numEnemy], numEnemy))
                            && (!BL.IsPosEnemySameBases(InitGame.myGameField, actionEnemy[numEnemy], numEnemy)))
                        {
                            if (gameTime % TIME_DIRECT_ENEMY == 0)
                            {
                                BL.GetRandomAction(ref actionEnemy[numEnemy]);
                            }
                            UI.HideTank(ref InitGame.myGameField.EnemyTanks[numEnemy]);
                            BL.MoveTanks(ref InitGame.myGameField, ref InitGame.myGameField.EnemyTanks[numEnemy], 
                                actionEnemy[numEnemy]);
                            UI.PrintTank(ref InitGame.myGameField.EnemyTanks[numEnemy]);
                        }
                        else
                        {
                            BL.GetRandomAction(ref actionEnemy[numEnemy]);
                        }
                    }
                }

                if (gameTime % TIME_CREATE_BULLET == 0)
                {
                    for (int numEnemy = 0; numEnemy < InitGame.myGameField.EnemyTanks.Length; numEnemy++)
                    {
                        if (gameTime % (ulong)(500 * (numEnemy + 1)) == 0)
                        {
                            BL.AddBulletToGameField(ref InitGame.myGameField, 
                                ref InitGame.myGameField.EnemyTanks[numEnemy]);
                        }
                    }
                }

                if ((gameTime % TIME_MOVE_BULLET == 0) && (InitGame.myGameField.Bullets != null))
                {
                    if (InitGame.myGameField.Bullets.Length != 0)
                    {
                        for (int numBullet = 0; numBullet < InitGame.myGameField.Bullets.Length; numBullet++)
                        {
                            if ((!BL.IsEnemyDamageEnemy(InitGame.myGameField))
                                && (!BL.IsBlockMetal(InitGame.myGameField.Bullets[numBullet], 
                                InitGame.myGameField.Blocks)))
                            {
                                UI.HideBullet(InitGame.myGameField.Bullets, numBullet);
                            }
                        }

                        BL.MoveBullet(ref InitGame.myGameField);

                        for (int numBullet = 0; numBullet < InitGame.myGameField.Bullets.Length; numBullet++)
                        {
                            if ((!BL.IsEnemyDamageEnemy(InitGame.myGameField))
                                && (!BL.IsBulletSameBlock(InitGame.myGameField.Bullets[numBullet], 
                                InitGame.myGameField.Blocks)))
                            {
                                UI.PrintBullet(ref InitGame.myGameField.Bullets, numBullet);
                            }
                        }
                    }
                }

                if (BL.IsEnemyDeath(InitGame.myGameField.EnemyTanks, out indexDeath))
                {
                    UI.HideBonus(InitGame.myGameField);
                    BL.GetBonusPos(ref InitGame.myGameField, indexDeath);
                    UI.HideTank(ref InitGame.myGameField.EnemyTanks[indexDeath]);
                    BL.SetEnemyDeath(ref InitGame.myGameField.EnemyTanks, indexDeath);
                    ++InitGame.myGameField.sumDeadEnemy;

                    if (InitGame.myGameField.sumDeadEnemy <= UI.QUANTITY_ENEMY - InitGame.START_NUM_ENEMYTANK)
                    {
                        int newEnemyPosX = 0;
                        int newEnemyPosY = 0;

                        BL.GetNewEnemyTankPosXY(ref InitGame.myGameField, out newEnemyPosX, out newEnemyPosY);

                        InitGame.enemyTank[InitGame.myGameField.EnemyTanks.Length] = new Tank
                        {
                            PositionX = newEnemyPosX,
                            PositionY = newEnemyPosY,
                            TankCharacter = InitGame.destroyerCharacterEnemy,
                            TankDirection = Direction.Down,
                            IsBot = true
                        };

                        BL.AddEnemyTank(ref InitGame.myGameField, InitGame.enemyTank[InitGame.myGameField.EnemyTanks.Length],
                            ConsoleColor.Magenta);
                    }
                }

                UI.GetViewStatistic(InitGame.myGameField);
                UI.PrintBonus(InitGame.myGameField.Bonus);
                BL.SetPlayerBonus(ref InitGame.myGameField, action, InitGame.lightCharacterPlay, 
                    InitGame.heavyCharacterPlay, InitGame.destroyerCharacterPlay);
                BL.SetPosBonus(ref InitGame.myGameField, action);

                if (InitGame.myGameField.sumDeadEnemy == UI.QUANTITY_ENEMY)
                {
                    watch.Stop();

                    string score = UI.GetViewScore(watch.Elapsed.Minutes, watch.Elapsed.Seconds);

                    UI.GetViewMessageTheGame("YOU WON", score, "PRESS \"ESC\" TO EXIT");

                    UI.HideMessageTheGame("YOU WON", score, "PRESS \"ESC\" TO EXIT");

                    exit = true;
                }

                if (BL.IsPlayerDeath(InitGame.myGameField.PlayerTank))
                {
                    UI.GetViewMessageTheGame
                        ("YOU LOSE", "\t    GAME OVER", "PRESS \"ESC\" TO EXIT");

                    UI.HideMessageTheGame("YOU LOSE", "\t    GAME OVER", "PRESS \"ESC\" TO EXIT");

                    exit = true;
                }

                if (InitGame.myGameField.Bullets != null)
                {
                    for (int numBullet = 0; numBullet < InitGame.myGameField.Bullets.Length; numBullet++)
                    {
                        if (BL.IsBulletSameBase(InitGame.myGameField.Bullets[numBullet], InitGame.myGameField.FieldBase))
                        {
                            UI.GetViewMessageTheGame("YOU LOSE", "\t    GAME OVER", "PRESS \"ESC\" TO EXIT");;

                            UI.HideMessageTheGame("YOU LOSE", "\t    GAME OVER", "PRESS \"ESC\" TO EXIT");

                            exit = true;
                            break;
                        }
                    }
                }
                ++gameTime;
                System.Threading.Thread.Sleep(TIME_SLEEP);
            }
            while ((action != ActionPlayer.PressExit) && !exit);            
        }
    }
}
    
