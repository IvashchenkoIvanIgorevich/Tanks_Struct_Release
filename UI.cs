using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    class UI
    {
        public const int QUANTITY_ENEMY = 20;
        const int SHIFT_START_WINDOW = 5;
        const int CUR_LEFT_PAUSE = 45;
        const int CUR_TOP_PAUSE = 36;
        const int CUR_LEFT_STAT = 101;
        const int CUR_TOP_STAT = 1;
        const int CUR_TOP_HP = 22;
        const int WIDTH_RIGHT_BAR = 9;
        const int BUFFER_HEIGHT = 110;
        const int BUFFER_WIDTH = 78;
        const int CUR_TOP_GAMEPAD = 14;
        const int TOP_RIGHT_ARROW = 2;
        const int TOP_LEFT_ARROW = 4;
        const int TOP_UP_ARROW = 6;
        const int TOP_DOWN_ARROW = 8;
        const int TOP_FIRE = 10;

        public static void PrintGameField(ref GameField gameField)
        {
            Console.SetCursorPosition(0, 0);

            for (int posY = 0; posY < gameField.Height; posY++)
            {
                for (int posX = 0; posX < gameField.Width; posX++)
                {
                    Console.SetCursorPosition(posX, posY);

                    if ((posY > 0) && (posY < (gameField.Height - 1)) 
                        && (posX > 0) && (posX < (gameField.Width - 1)))
                    {                        
                        Console.Write(' ');
                    }
                    else
                    {
                        Console.Write('\x2588');
                    }
                }
            }
        }

        public static ActionPlayer GetActionPlayer(ConsoleKey pressKey)
        {
            ActionPlayer pressPlayer = ActionPlayer.NoAction;

            switch (pressKey)
            {
                case ConsoleKey.LeftArrow:
                    pressPlayer = ActionPlayer.PressLeft;
                    break;
                case ConsoleKey.UpArrow:
                    pressPlayer = ActionPlayer.PressUp;
                    break;
                case ConsoleKey.RightArrow:
                    pressPlayer = ActionPlayer.PressRight;
                    break;
                case ConsoleKey.DownArrow:
                    pressPlayer = ActionPlayer.PressDown;
                    break;
                case ConsoleKey.Spacebar:
                    pressPlayer = ActionPlayer.PressFire;
                    break;
                case ConsoleKey.Escape:
                    pressPlayer = ActionPlayer.PressExit;
                    break;
                case ConsoleKey.P:
                    pressPlayer = ActionPlayer.PressPause;
                    break;
                case ConsoleKey.Enter:
                    pressPlayer = ActionPlayer.PressEnter;
                    break;
            }

            return pressPlayer;
        }

        #region ===---   GetViewAccordingToDirection   ---===

        public static char[,] GetViewLeftRightTank(Direction dirTank, char[,] leftAndRightView, 
            char leftHarp, char RightHarp, char tankHatch, char tankBarrel, char befforBarrel, 
            char tankLining, char elseLining)
        {
            for (int row = 0; row < leftAndRightView.GetLength(0); row++)
            {
                for (int col = 0; col < leftAndRightView.GetLength(1); col++)
                {
                    if (row == 0)
                    {
                        leftAndRightView[row, col] = leftHarp;
                    }
                    if (row == leftAndRightView.GetLength(0) - 1)
                    {
                        leftAndRightView[row, col] = RightHarp;
                    }
                    if ((row == 1) || (row == leftAndRightView.GetLength(0) - 2))
                    {
                        leftAndRightView[row, col] = elseLining;
                    }
                    if ((row == 2) && (dirTank.HasFlag(Direction.Right)))
                    {
                        if ((col == 0) || (col == 1))
                        {
                            leftAndRightView[row, col] = tankLining;
                        }
                        if (row == col)
                        {
                            leftAndRightView[row, col] = tankHatch;
                        }
                        if (col == (leftAndRightView.GetLength(1) - 1))
                        {
                            leftAndRightView[row, col] = tankBarrel;
                        }
                        if ((col == leftAndRightView.GetLength(1) - 2))
                        {
                            leftAndRightView[row, col] = befforBarrel;
                        }
                    }
                    else
                    {
                        if (row == 2)
                        {
                            if (col == (leftAndRightView.GetLength(1) - 1) || (col == leftAndRightView.GetLength(1) - 2))
                            {
                                leftAndRightView[row, col] = tankLining;
                            }
                            if (row == col)
                            {
                                leftAndRightView[row, col] = tankHatch;
                            }
                            if (col == 0)
                            {
                                leftAndRightView[row, col] = tankBarrel;
                            }
                            if (col == 1)
                            {
                                leftAndRightView[row, col] = befforBarrel;
                            }
                        }
                    }
                }
            }

            return leftAndRightView;
        }

        public static char[,] GetViewUpDownTank(Direction dirTank, char[,] upAndDownView, 
            char leftHarp, char RightHarp, char tankHatch, char tankBarrel, char befforBarrel, 
            char tankLining, char elseLining)
        {
            for (int row = 0; row < upAndDownView.GetLength(0); row++)
            {
                for (int col = 0; col < upAndDownView.GetLength(1); col++)
                {
                    if (col == 0)
                    {
                        upAndDownView[row, col] = leftHarp;
                    }
                    if (col == upAndDownView.GetLength(1) - 1)
                    {
                        upAndDownView[row, col] = RightHarp;
                    }
                    if ((col == 1) || (col == upAndDownView.GetLength(0) - 2))
                    {
                        upAndDownView[row, col] = elseLining;
                    }
                    if ((col == 2) && (dirTank.HasFlag(Direction.Down)))
                    {
                        if ((row == 0) || (row == 1))
                        {
                            upAndDownView[row, col] = tankLining;
                        }
                        if (row == col)
                        {
                            upAndDownView[row, col] = tankHatch;
                        }
                        if (row == (upAndDownView.GetLength(1) - 1))
                        {
                            upAndDownView[row, col] = tankBarrel;
                        }
                        if ((row == upAndDownView.GetLength(1) - 2))
                        {
                            upAndDownView[row, col] = befforBarrel;
                        }
                    }
                    else
                    {
                        if (col == 2)
                        {
                            if (row == (upAndDownView.GetLength(1) - 1) || (row == upAndDownView.GetLength(1) - 2))
                            {
                                upAndDownView[row, col] = tankLining;
                            }
                            if (row == col)
                            {
                                upAndDownView[row, col] = tankHatch;
                            }
                            if (row == 0)
                            {
                                upAndDownView[row, col] = tankBarrel;
                            }
                            if (row == 1)
                            {
                                upAndDownView[row, col] = befforBarrel;
                            }
                        }
                    }
                }
            }

            return upAndDownView;
        }

        #endregion

        #region ===---   ViewTank   ---===

        public static char[,] GetViewByTankForm(ref Tank tank)
        {
            char[,] viewTank = new char[tank.TankCharacter.FormTank.Height, tank.TankCharacter.FormTank.Width];

            switch (tank.TankDirection)
            {
                case Direction.Right:
                    viewTank = GetLeftRightSkin(ref tank);
                    break;
                case Direction.Up:
                    viewTank = GetUpDownSkin(ref tank);
                    break;
                case Direction.Left:
                    viewTank = GetLeftRightSkin(ref tank);
                    break;
                case Direction.Down:
                    viewTank = GetUpDownSkin(ref tank);
                    break;
                case Direction.NoDirection:
                    break;
            }

            return viewTank;
        }

        public static char[,] GetLeftRightSkin(ref Tank tank)
        {
            char[,] viewTankSkin = new char[tank.TankCharacter.FormTank.Height, tank.TankCharacter.FormTank.Width];

            switch (tank.TankCharacter.FormTank.TankSkin)
            {
                case SkinTank.Light:
                    GetViewLeftRightTank(tank.TankDirection, viewTankSkin, ' ', ' ', '\x25A0', '\x2500', '\x2500', '\x2588', '*');
                    break;
                case SkinTank.Heavy:
                    GetViewLeftRightTank(tank.TankDirection, viewTankSkin, '\x2559', '\x2556', '\x25A0', '\x2500', '\x2588', '\x2588', '\x2588');
                    break;
                case SkinTank.Destroy:
                    GetViewLeftRightTank(tank.TankDirection, viewTankSkin, '#', '#', '\x25A0', '\x2500', '\x2500', '\x2588', '\x2588');
                    break;
                case SkinTank.NoSkin:
                    break;
            }

            return viewTankSkin;
        }

        public static char[,] GetUpDownSkin(ref Tank tank)
        {
            char[,] viewTankSkin = new char[tank.TankCharacter.FormTank.Height, tank.TankCharacter.FormTank.Width];

            switch (tank.TankCharacter.FormTank.TankSkin)
            {
                case SkinTank.Light:
                    GetViewUpDownTank(tank.TankDirection, viewTankSkin, ' ', ' ', '\x25A0', '\x2502', '\x2502', '\x2588', '*');
                    break;
                case SkinTank.Heavy:
                    GetViewUpDownTank(tank.TankDirection, viewTankSkin, '\x255A', '\x255D', '\x25A0', '\x2502', '\x2588', '\x2588', '\x2588');
                    break;
                case SkinTank.Destroy:
                    GetViewUpDownTank(tank.TankDirection, viewTankSkin, '#', '#', '\x25A0', '\x2502', '\x2502', '\x2588', '\x2588');
                    break;
                case SkinTank.NoSkin:
                    break;
            }

            return viewTankSkin;
        }

        public static void PrintTank(ref Tank tank)
        {
            Console.CursorVisible = false;

            char[,] printNewTank = GetViewByTankForm(ref tank);

            Console.SetCursorPosition(tank.PositionX, tank.PositionY);

            int currLeft = Console.CursorLeft;

            if (tank.IsBonus)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            else
            {
                Console.ForegroundColor = tank.TankCharacter.FormTank.TankColor;
            }

            if ((tank.TankCharacter.HealthPoint < 400) && (!tank.IsBonus))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            for (int posTankY = 0; posTankY < printNewTank.GetLength(0); posTankY++)
            {
                for (int posTankX = 0; posTankX < printNewTank.GetLength(1); posTankX++)
                {
                    Console.Write(printNewTank[posTankY, posTankX]);
                }

                Console.WriteLine();

                int currTop = Console.CursorTop;
                Console.SetCursorPosition(currLeft, currTop);
            }

            Console.ResetColor();
        }

        public static void HideTank(ref Tank tank)
        {
            Console.SetCursorPosition(tank.PositionX, tank.PositionY);

            int currLeft = Console.CursorLeft;

            for (int posY = 0; posY < tank.TankCharacter.FormTank.Height; posY++)
            {
                for (int posX = 0; posX < tank.TankCharacter.FormTank.Width; posX++)
                {
                    Console.Write(' ');
                }
                Console.WriteLine();

                int currTop = Console.CursorTop;
                Console.SetCursorPosition(currLeft, currTop);
            }
        }

        #endregion   

        #region ===---   PrintBullet   ---===

        public static void PrintBullet(ref Bullet[] bullets, int numBullet)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(bullets[numBullet].PosX, bullets[numBullet].PosY);
            Console.Write(bullets[numBullet].Skin);
            bullets[numBullet].Range -= bullets[numBullet].AtackSpeed;
            Console.ResetColor();
        }

        public static void HideBullet(Bullet[] bullet, int numHideBullet)
        {
            for (int numBullet = 0; numBullet < bullet.Length; numBullet++)
            {
                if (numBullet == numHideBullet)
                {
                    Console.SetCursorPosition(bullet[numHideBullet].PosX,
                        bullet[numHideBullet].PosY);
                    Console.Write(' ');
                }
            }
        }

        #endregion

        #region ===---   PrintMessage   ---===

        public static void GetViewStartWindow(GameField gameField)
        {
            try
            {
                Console.SetWindowSize(BUFFER_HEIGHT, BUFFER_WIDTH);
                Console.SetBufferSize(BUFFER_HEIGHT, BUFFER_WIDTH);
            }
            catch (Exception)
            {
                Console.WriteLine();
                Console.WriteLine("В свойствах даного окна консоли, измените <Шрифт>" +
                    " на <Точечный> <Размер> 8x9.\n И перезапустите проект.");
                Console.ReadKey();
            }

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, gameField.Height / 3);

            int curLeft = Console.CursorLeft;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"         ____       _______ _______ _      ______        _____ _____ _________     __ ");
            Console.WriteLine(@"        |  _ \   /\|__   __|__   __| |    |  ____|      / ____|_   _|__   __\ \   / / ");
            Console.WriteLine(@"        | |_) | /  \  | |     | |  | |    | |__        | |      | |    | |   \ \_/ /  ");
            Console.WriteLine(@"        |  _ < / /\ \ | |     | |  | |    |  __|       | |      | |    | |    \   /   ");
            Console.WriteLine(@"        | |_) / ____ \| |     | |  | |____| |____      | |____ _| |_   | |     | |    ");
            Console.WriteLine(@"        |____/_/    \_\_|     |_|  |______|______|      \_____|_____|  |_|     |_|    ");
            Console.WriteLine(@"                           _____  ______ ____   ____  _____  _   _                    ");
            Console.WriteLine(@"                          |  __ \|  ____|  _ \ / __ \|  __ \| \ | |                   ");
            Console.WriteLine(@"                          | |__) | |__  | |_) | |  | | |__) |  \| |                   ");
            Console.WriteLine(@"                          |  _  /|  __| |  _ <| |  | |  _  /|     |                   ");
            Console.WriteLine(@"                          | | \ \| |____| |_) | |__| | | \ \| |\  |                   ");
            Console.WriteLine(@"                          |_|  \_\______|____/ \____/|_|  \_\_| \_|                   ");
            Console.WriteLine();

            int topStart = gameField.Height / 2 + SHIFT_START_WINDOW;
            int topExit = gameField.Height / 2 + SHIFT_START_WINDOW + 2;
            int topPause = gameField.Height / 2 + SHIFT_START_WINDOW + 4;

            Console.SetCursorPosition(gameField.Height / 2 - 1, topStart);
            Console.WriteLine("PRESS \"ENTER\" TO START");
            Console.WriteLine();
            Console.SetCursorPosition(gameField.Height / 2, topExit);
            Console.WriteLine("PRESS \"ESC\" TO EXIT");
            Console.SetCursorPosition(gameField.Height / 2 + 1, topPause);
            Console.WriteLine("PRESS \"P\" TO PAUSE");
            Console.SetCursorPosition(gameField.Height / 2 - 3, topPause + TOP_RIGHT_ARROW);
            Console.WriteLine("PRESS: \"\u2192\" TO MOVE RIGHT ");
            Console.SetCursorPosition(gameField.Height / 2 - 3, topPause + TOP_LEFT_ARROW);
            Console.WriteLine("       \"\u2190\" TO MOVE LEFT ");
            Console.SetCursorPosition(gameField.Height / 2 - 3, topPause + TOP_UP_ARROW);
            Console.WriteLine("       \"\u2191\" TO MOVE UP ");
            Console.SetCursorPosition(gameField.Height / 2 - 3, topPause + TOP_DOWN_ARROW);
            Console.WriteLine("       \"\u2193\" TO MOVE DOWN ");
            Console.SetCursorPosition(gameField.Height / 2 - 3, topPause + TOP_FIRE);
            Console.WriteLine("       \"SPACE\" TO FIRE ");
            Console.ResetColor();
        }

        public static void GetViewMessageTheGame(string firstStr, string secondStr, string other = "")
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(CUR_LEFT_PAUSE, CUR_TOP_PAUSE);
            Console.Write(firstStr);
            Console.SetCursorPosition(CUR_TOP_PAUSE + 2, CUR_TOP_PAUSE + SHIFT_START_WINDOW);
            Console.Write(secondStr);
            Console.SetCursorPosition(CUR_TOP_PAUSE + 2, CUR_TOP_PAUSE + SHIFT_START_WINDOW + 5);
            Console.Write(other);
            Console.ReadKey(true);

            Console.ForegroundColor = Console.BackgroundColor;
            Console.SetCursorPosition(CUR_LEFT_PAUSE, CUR_TOP_PAUSE);
            Console.Write(firstStr);
            Console.SetCursorPosition(CUR_TOP_PAUSE + 2, CUR_TOP_PAUSE + SHIFT_START_WINDOW);
            Console.Write(secondStr);
            Console.SetCursorPosition(CUR_TOP_PAUSE + 2, CUR_TOP_PAUSE + SHIFT_START_WINDOW + 5);
            Console.Write(other);
            Console.ResetColor();
        }

        public static void HideMessageTheGame(string firstStr, string secondStr, string other = "")
        {
            Console.ForegroundColor = Console.BackgroundColor;
            Console.SetCursorPosition(CUR_LEFT_PAUSE, CUR_TOP_PAUSE);
            Console.Write(firstStr);
            Console.SetCursorPosition(CUR_TOP_PAUSE + 2, CUR_TOP_PAUSE + SHIFT_START_WINDOW);
            Console.Write(secondStr);
            Console.SetCursorPosition(CUR_TOP_PAUSE + 2, CUR_TOP_PAUSE + SHIFT_START_WINDOW + 5);
            Console.Write(other);
            Console.ResetColor();
        }

        public static void GetViewRigthBar(GameField gameField)
        {
            for (int posY = 0; posY < gameField.Height - 1; posY++)
            {
                for (int posX = gameField.Width; posX < gameField.Width + WIDTH_RIGHT_BAR; posX++)
                {
                    Console.SetCursorPosition(posX, posY);
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(' ');
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        public static void GetViewStatistic(GameField gameField)
        {
            int deathEnemy = QUANTITY_ENEMY - gameField.sumDeadEnemy;

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(CUR_LEFT_STAT, 0);

            Console.Write("ENEMIES");

            Console.SetCursorPosition(CUR_LEFT_STAT, CUR_TOP_STAT);

            for (int i = 1; i <= QUANTITY_ENEMY; i++)
            {
                if (i <= deathEnemy)
                {
                    Console.WriteLine('X');
                }
                else
                {
                    Console.WriteLine(' ');
                }
                Console.SetCursorPosition(CUR_LEFT_STAT, i + 1);
            }

            Console.SetCursorPosition(CUR_LEFT_STAT, CUR_TOP_HP);
            Console.WriteLine("PLAYER");
            Console.SetCursorPosition(CUR_LEFT_STAT, CUR_TOP_HP + 1);
            Console.WriteLine("HP {0,4}", gameField.PlayerTank.TankCharacter.HealthPoint);
            Console.SetCursorPosition(CUR_LEFT_STAT, CUR_TOP_HP + 3);
            Console.WriteLine("AD {0,4}", gameField.PlayerTank.TankCharacter.AtackDamage);
            Console.SetCursorPosition(CUR_LEFT_STAT, CUR_TOP_HP + 5);
            Console.WriteLine("{0,4}", gameField.PlayerTank.TankCharacter.FormTank.TankSkin.ToString());

            Console.ResetColor();
        }

        public static string GetViewScore(int minutes, int seconds)
        {
            return "YOUR SCORE " + minutes + "min" + seconds + "sec";
        }

        #endregion

        #region ===---   PrintBlocks   ---===

        public static void PrintBlocks(ref GameField gameField)
        {
            for (int numBlock = 0; numBlock < gameField.Blocks.Length; numBlock++)
            {
                Console.SetCursorPosition(gameField.Blocks[numBlock].PosX, gameField.Blocks[numBlock].PosY);

                switch (gameField.Blocks[numBlock].SkinBlock)
                {
                    case SkinBlock.Brick:
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.Write('\u25A0');
                        break;
                    case SkinBlock.Metal:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write('\u25A0');
                        break;
                    case SkinBlock.NoSkin:
                        Console.ForegroundColor = Console.BackgroundColor;
                        Console.Write(' ');
                        break;
                }
            }

            Console.ResetColor();
        }

        #endregion

        #region ===---   PrintBase   ---===

        public static void PrintBase(Base gameBase)
        {
            for (int numBaseR = 0; numBaseR < gameBase.fieldBase.GetLength(0); numBaseR++)
            {
                for (int numBaseL = 0; numBaseL < gameBase.fieldBase.GetLength(1); numBaseL++)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(gameBase.PosX + numBaseL, gameBase.PosY + numBaseR);
                    Console.Write(gameBase.fieldBase[numBaseR, numBaseL]);
                    Console.ResetColor();
                }
            }
        }

        public static void GetViewGameBase(ref Base gameBase)
        {
            for (int numBaseR = 0; numBaseR < gameBase.fieldBase.GetLength(0); numBaseR++)
            {
                for (int numBaseL = 0; numBaseL < gameBase.fieldBase.GetLength(1); numBaseL++)
                {
                    if (((numBaseR == 0) || (numBaseR == 1) || (numBaseR == 4))
                        && ((numBaseL == 0) || (numBaseL == 2) || (numBaseL == 4))
                        || (numBaseR == 2))
                    {
                        gameBase.fieldBase[numBaseR, numBaseL] = '*';
                    }

                    if ((numBaseR == 3) && ((numBaseL == 1)
                        || (numBaseL == 2) || (numBaseL == 3)))
                    {
                        gameBase.fieldBase[numBaseR, numBaseL] = '*';
                    }
                }
            }
        }

        #endregion

        #region ===---   PrintBonus   ---===

        public static void PrintBonus(Bonus bonus)
        {
            if (bonus.PosX != 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(bonus.PosX, bonus.PosY);
                Console.Write("B");
                Console.ResetColor();
            }
            else
            {
                Console.Write('\x2588');
            }
        }

        public static void HideBonus(GameField gameField)
        {
            Console.SetCursorPosition(gameField.Bonus.PosX, gameField.Bonus.PosY);

            bool isBlock = false;

            for (int numBlock = 0; numBlock < gameField.Blocks.Length; numBlock++)
            {
                if ((gameField.Blocks[numBlock].PosX == gameField.Bonus.PosX)
                    && (gameField.Blocks[numBlock].PosY == gameField.Bonus.PosY))
                {
                    switch (gameField.Blocks[numBlock].SkinBlock)
                    {
                        case SkinBlock.Brick:
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Write('\u25A0');
                            break;
                        case SkinBlock.Metal:
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.Write('\u25A0');
                            break;
                        case SkinBlock.NoSkin:
                            Console.ForegroundColor = Console.BackgroundColor;
                            Console.Write(' ');
                            break;
                    }

                    isBlock = true;
                }
            }

            for (int numBaseR = 0; numBaseR < gameField.FieldBase.fieldBase.GetLength(0); numBaseR++)
            {
                for (int numBaseL = 0; numBaseL < gameField.FieldBase.fieldBase.GetLength(1); numBaseL++)
                {
                    if ((numBaseL + gameField.FieldBase.PosX == gameField.Bonus.PosX) 
                        && (numBaseR + gameField.FieldBase.PosY == gameField.Bonus.PosY))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(gameField.FieldBase.fieldBase[numBaseR, numBaseL]);

                        isBlock = true;
                    }
                }
            }

            Console.ResetColor();

            if (gameField.Bonus.PosX == 0)
            {
                Console.Write('\x2588');
            }
            else
            {
                if (!isBlock)
                {
                    Console.Write(" ");
                }
            }
        }

        #endregion
    }
}
