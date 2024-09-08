namespace Battleships

{
    internal class Program
    {
        static void Main(string[] args)
        {
            //
            //Start
            //

            GameEngine engine = new GameEngine();
            GameGraphics playerGraphics = new GameGraphics();
            GameAI ai = new GameAI();

            ConsoleKeyInfo consoleKey;
            Console.SetWindowSize(120, 40);
            Console.SetBufferSize(120, 40);

            Console.Clear();
            Console.CursorVisible = false;

            //Initial playerGraphics state
            playerGraphics.mapBorders = playerGraphics.GenerateMap();
            playerGraphics.DrawMap(playerGraphics.mapBorders, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);

            playerGraphics.shipMap = playerGraphics.GenerateShipMap();
            playerGraphics.DrawShipMap(playerGraphics.shipMap, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);

            playerGraphics.shipSelection = playerGraphics.GenerateShipSelection(engine.carrierMax, engine.battleshipMax, engine.submarineMax, engine.destroyerMax);
            playerGraphics.DrawShipSelection(playerGraphics.shipSelection);

            Console.SetCursorPosition(engine.XCursor, engine.YCursor);
            Helpers.MoveCursor(engine.XCursor, engine.YCursor, GameGraphics.cursor);

            //
            //Ship placement phase
            //

            do
            {
                consoleKey = Console.ReadKey(true);

                //Clear old cursor                
                Helpers.MoveCursor(engine.XCursor, engine.YCursor, "  ");

                //Draw Ships
                playerGraphics.DrawShipMap(playerGraphics.shipMap, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);

                //Draw Ship Selection playerGraphics
                playerGraphics.DrawShipSelection(playerGraphics.shipSelection);

                //Save cursor position in case of edge of map
                engine.XCursorStored = engine.XCursor;
                engine.YCursorStored = engine.YCursor;

                //Read key press
                switch (consoleKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        engine.YCursor--;
                        Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                        break;
                    case ConsoleKey.DownArrow:
                        engine.YCursor++;
                        Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                        break;
                    case ConsoleKey.LeftArrow:
                        engine.XCursor--;
                        Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                        break;
                    case ConsoleKey.RightArrow:
                        engine.XCursor++;
                        Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                        break;
                    case ConsoleKey.D1:
                        if (engine.carrierMax > 0)
                        {
                            engine.selectedShipLength = engine.carrierLength;
                            engine.shipPlacementSelected = true;
                        }
                        break;
                    case ConsoleKey.D2:
                        if (engine.battleshipMax > 0)
                        {
                            engine.selectedShipLength = engine.battleshipLength;
                            engine.shipPlacementSelected = true;
                        }
                        break;
                    case ConsoleKey.D3:
                        if (engine.submarineMax > 0)
                        {
                            engine.selectedShipLength = engine.submarineLength;
                            engine.shipPlacementSelected = true;
                        }
                        break;
                    case ConsoleKey.D4:
                        if (engine.destroyerMax > 0)
                        {
                            engine.selectedShipLength = engine.destroyerLength;
                            engine.shipPlacementSelected = true;
                        }
                        break;
                    case ConsoleKey.Spacebar:

                        //Placement of ships
                        if (engine.shipPlacementSelected)
                        {
                            if (!engine.shipFrontPlaced)
                            {
                                engine.XPositionShipFront = engine.XCursor;
                                engine.YPositionShipFront = engine.YCursor;
                                engine.shipFrontPlaced = true;
                            }
                            else
                            {
                                engine.XPositionShipBack = engine.XCursor;
                                engine.YPositionShipBack = engine.YCursor;
                                if (engine.GetValidShipPlacement(playerGraphics.shipMap, engine.XPositionShipFront, engine.YPositionShipFront, engine.XPositionShipBack, engine.YPositionShipBack, engine.selectedShipLength))
                                {
                                    playerGraphics.shipMap = playerGraphics.PlaceShip(playerGraphics.shipMap, engine.XPositionShipFront, engine.YPositionShipFront, engine.XPositionShipBack, engine.YPositionShipBack, engine.selectedShipLength);
                                    switch (engine.selectedShipLength)
                                    {
                                        case 2:
                                            engine.destroyerMax--;
                                            break;
                                        case 3:
                                            engine.submarineMax--;
                                            break;
                                        case 4:
                                            engine.battleshipMax--;
                                            break;
                                        case 5:
                                            engine.carrierMax--;
                                            break;
                                    }
                                    playerGraphics.shipSelection = playerGraphics.GenerateShipSelection(engine.carrierMax, engine.battleshipMax, engine.submarineMax, engine.destroyerMax);
                                    playerGraphics.DrawShipSelection(playerGraphics.shipSelection);
                                }
                                playerGraphics.DrawShipMap(playerGraphics.shipMap, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);
                                engine.shipFrontPlaced = false;
                                engine.shipPlacementSelected = false;
                            }
                        }
                        break;
                }

                //Placement of ships feedback                
                playerGraphics.DrawShipPlacementFeedback(engine.shipPlacementSelected, engine.shipFrontPlaced);

                //Edge detection                
                if (playerGraphics.mapBorders[engine.XCursor, engine.YCursor] == GameGraphics.wall)
                {
                    engine.XCursor = engine.XCursorStored;
                    engine.YCursor = engine.YCursorStored;
                }

                //Move Cursor                
                Helpers.MoveCursor(engine.XCursor, engine.YCursor, GameGraphics.cursor);

            } while (!engine.GetAllShipsPlaced(engine.carrierMax, engine.battleshipMax, engine.submarineMax, engine.destroyerMax));

            //
            //AI Generates map
            //

            GameGraphics aiGraphic = new GameGraphics();

            aiGraphic.shipMap = aiGraphic.GenerateShipMap();
            do
            {
                do
                {
                    do
                    {
                        engine.selectedShipLength = ai.GetShipSelection();

                        switch (engine.selectedShipLength)
                        {
                            case 2:
                                if (ai.AiDestroyerMax == 0)
                                {
                                    ai.ValidShipSelection = false;
                                }
                                else
                                {
                                    ai.ValidShipSelection = true;
                                }
                                break;
                            case 3:
                                if (ai.AiSubmarineMax == 0)
                                {
                                    ai.ValidShipSelection = false;
                                }
                                else
                                {
                                    ai.ValidShipSelection = true;
                                }
                                break;
                            case 4:
                                if (ai.AiBattleshipMax == 0)
                                {
                                    ai.ValidShipSelection = false;
                                }
                                else
                                {
                                    ai.ValidShipSelection = true;
                                }
                                break;
                            case 5:
                                if (ai.AiCarrierMax == 0)
                                {
                                    ai.ValidShipSelection = false;
                                }
                                else
                                {
                                    ai.ValidShipSelection = true;
                                }
                                break;
                        }
                    } while (!ai.ValidShipSelection);


                    engine.XPositionShipFront = ai.GetShotOrShipPlacement();
                    engine.YPositionShipFront = ai.GetShotOrShipPlacement();

                    ai.ShipDirection = ai.GetShipDirection();

                    engine.XPositionShipBack = ai.GetShipBackPlacementX(ai.ShipDirection, engine.selectedShipLength, engine.XPositionShipFront, engine.YPositionShipFront);
                    engine.YPositionShipBack = ai.GetShipBackPlacementY(ai.ShipDirection, engine.selectedShipLength, engine.XPositionShipFront, engine.YPositionShipFront);

                } while (!engine.GetValidShipPlacement(aiGraphic.shipMap, engine.XPositionShipFront + 1, engine.YPositionShipFront + 1, engine.XPositionShipBack + 1, engine.YPositionShipBack + 1, engine.selectedShipLength));

                aiGraphic.shipMap = aiGraphic.PlaceShip(aiGraphic.shipMap, engine.XPositionShipFront + 1, engine.YPositionShipFront + 1, engine.XPositionShipBack + 1, engine.YPositionShipBack + 1, engine.selectedShipLength);

                switch (engine.selectedShipLength)
                {
                    case 2:
                        if (ai.AiDestroyerMax > 0)
                        {
                            ai.AiDestroyerMax--;
                        }
                        break;
                    case 3:
                        if (ai.AiSubmarineMax > 0)
                        {
                            ai.AiSubmarineMax--;
                        }
                        break;
                    case 4:
                        if (ai.AiBattleshipMax > 0)
                        {
                            ai.AiBattleshipMax--;
                        }
                        break;
                    case 5:
                        if (ai.AiCarrierMax > 0)
                        {
                            ai.AiCarrierMax--;
                        }
                        break;
                }

            } while (!engine.GetAllShipsPlaced(ai.AiCarrierMax, ai.AiBattleshipMax, ai.AiSubmarineMax, ai.AiDestroyerMax));

            //
            //Main game ui initialization
            //

            Console.Clear();

            aiGraphic.mapBorders = aiGraphic.GenerateMap();
            aiGraphic.concealedShipMap = aiGraphic.GenerateShipMap();

            playerGraphics.DrawMap(playerGraphics.mapBorders, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);
            aiGraphic.DrawMap(aiGraphic.mapBorders, GameGraphics.AiMapPositionX, GameGraphics.AiMapPositionY);
            playerGraphics.DrawShipMap(playerGraphics.shipMap, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);
            aiGraphic.DrawShipMap(aiGraphic.concealedShipMap, GameGraphics.AiMapPositionX, GameGraphics.AiMapPositionY);

            engine.XCursor = GameGraphics.AiMapPositionX + 2;
            engine.YCursor = GameGraphics.AiMapPositionY + 2;
            Console.SetCursorPosition(GameGraphics.AiMapPositionX + 2, GameGraphics.AiMapPositionY + 2);
            Helpers.MoveCursor(engine.XCursor, engine.YCursor, GameGraphics.cursor);

            //
            //Main game phase
            //

            do
            {
                //
                //Player Turn
                //

                consoleKey = Console.ReadKey(true);

                //Clear old cursor                
                Helpers.MoveCursor(engine.XCursor, engine.YCursor, "  ");

                //Draw Ships
                playerGraphics.DrawShipMap(playerGraphics.shipMap, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);
                aiGraphic.DrawShipMap(aiGraphic.concealedShipMap, GameGraphics.AiMapPositionX, GameGraphics.AiMapPositionY);

                //Draw remaining ships UI


                //Save cursor position in case of edge of map
                engine.XCursorStored = engine.XCursor;
                engine.YCursorStored = engine.YCursor;

                switch (consoleKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        engine.YCursor--;
                        Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                        break;
                    case ConsoleKey.DownArrow:
                        engine.YCursor++;
                        Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                        break;
                    case ConsoleKey.LeftArrow:
                        engine.XCursor--;
                        Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                        break;
                    case ConsoleKey.RightArrow:
                        engine.XCursor++;
                        Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                        break;
                    case ConsoleKey.Spacebar:

                        //Shooting enemy ships logic
                        engine.ValidShot = engine.GetValidShot(aiGraphic.shipMap, engine.XCursor - GameGraphics.AiMapPositionX, engine.YCursor);
                        if (engine.ValidShot)
                        {
                            aiGraphic.concealedShipMap = aiGraphic.ShootShip(aiGraphic.shipMap, aiGraphic.concealedShipMap, engine.XCursor - GameGraphics.AiMapPositionX, engine.YCursor);
                            engine.ValidShot = false;
                        }
                        break;
                }

                //Edge detection                
                if (playerGraphics.mapBorders[engine.XCursor - GameGraphics.AiMapPositionX, engine.YCursor] == GameGraphics.wall)
                {
                    engine.XCursor = engine.XCursorStored;
                    engine.YCursor = engine.YCursorStored;
                }

                //Move Cursor                
                Helpers.MoveCursor(engine.XCursor, engine.YCursor, GameGraphics.cursor);

                //
                //Ai Turn
                //

                ai.ShotX = ai.GetShotOrShipPlacement();
                ai.ShotY = ai.GetShotOrShipPlacement();

                //Wind detection
                engine.Win = engine.GetWin(aiGraphic.concealedShipMap);

            } while (!engine.Win);
            
            Console.WriteLine("You won!");
            Console.ReadKey();
        }
    }
    public class GameEngine
    {
        public int XCursor = 5;
        public int YCursor = 5;
        public int XCursorStored = 5;
        public int YCursorStored = 5;
        public bool shipFrontPlaced = false;
        public bool shipPlacementSelected = false;
        public bool allShipsPlaced = false;
        public bool ValidShot = false;
        public bool Win = false;
        public int XPositionShipFront = 0;
        public int YPositionShipFront = 0;
        public int XPositionShipBack = 0;
        public int YPositionShipBack = 0;
        public int carrierMax = 1;
        public int battleshipMax = 2;
        public int submarineMax = 3;
        public int destroyerMax = 4;
        public int carrierLength = 5;
        public int battleshipLength = 4;
        public int submarineLength = 3;
        public int destroyerLength = 2;
        public int selectedShipLength = 0;
        public bool GetValidShipPlacement(string[,] shipMap, int XPositionShipFront, int YPositionShipFront, int XPositionShipBack, int YPositionShipBack, int shipSize)
        {
            if (YPositionShipFront > YPositionShipBack && YPositionShipFront - shipSize < 0)
            {
                return false;
            }
            if (YPositionShipFront < YPositionShipBack && YPositionShipFront + shipSize > 11)
            {
                return false;
            }
            if (XPositionShipFront > XPositionShipBack && XPositionShipFront - shipSize < 0)
            {
                return false;
            }
            if (XPositionShipFront < XPositionShipBack && XPositionShipFront + shipSize > 11)
            {
                return false;
            }
            if (XPositionShipFront == XPositionShipBack && YPositionShipFront == YPositionShipBack)
            {
                return false;
            }
            if (YPositionShipFront < YPositionShipBack)
            {
                for (int i = YPositionShipFront; i < (YPositionShipFront + shipSize); i++)
                {
                    if (shipMap[XPositionShipFront - 1, i - 1] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else if (YPositionShipFront > YPositionShipBack)
            {
                for (int i = YPositionShipFront; i > (YPositionShipFront - shipSize); i--)
                {
                    if (shipMap[XPositionShipFront - 1, i - 1] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else if (XPositionShipFront < XPositionShipBack)
            {
                for (int i = XPositionShipFront; i < (XPositionShipFront + (shipSize)); i++)
                {
                    if (shipMap[i - 1, YPositionShipFront - 1] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = XPositionShipFront; i > (XPositionShipFront - (shipSize)); i--)
                {
                    if (shipMap[i - 1, YPositionShipFront - 1] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool GetAllShipsPlaced(int carrierMax, int battleshipMax, int submarineMax, int destroyerMax)
        {
            if (carrierMax == 0 && battleshipMax == 0 && submarineMax == 0 && destroyerMax == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GetValidShot(string[,] shipMap, int x, int y)
        {
            for (int i = 0; i < shipMap.GetLength(0); i++)
            {
                if (i == x - 1)
                {
                    for (int j = 0; j < shipMap.GetLength(1); j++)
                    {
                        if (j == y - 1)
                        {
                            if (shipMap[i, j] == GameGraphics.MissMarker || shipMap[i, j] == GameGraphics.hitCarrier || shipMap[i, j] == GameGraphics.hitBattleship || shipMap[i, j] == GameGraphics.hitSubmarine || shipMap[i, j] == GameGraphics.hitDestroyer)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public bool GetWin(string[,] concealedShipMap)
        {
            int hitCounter = 0;
            for (int i = 0; i < concealedShipMap.GetLength(0); i++)
            {
                for (int j = 0; j < concealedShipMap.GetLength(1); j++)
                {
                    if (concealedShipMap[i, j] == GameGraphics.hitCarrier || concealedShipMap[i, j] == GameGraphics.hitBattleship || concealedShipMap[i, j] == GameGraphics.hitSubmarine || concealedShipMap[i, j] == GameGraphics.hitDestroyer)
                    {
                        hitCounter++;
                    }
                }
            }
            return (hitCounter >= 30) ? true : false;
        }
    }
    public class GameAI
    {
        public Random Random = new Random();
        public int AiBattleshipMax = 2;
        public int AiSubmarineMax = 3;
        public int AiDestroyerMax = 4;
        public int AiCarrierMax = 1;
        public int ShipDirection;
        public bool ValidShipSelection = false;
        public bool ValidPlacement = false;
        public int ShotX;
        public int ShotY;
        public int GetShipSelection()
        {
            return Random.Next(2, 6);
        }
        public int GetShotOrShipPlacement()
        {
            return Random.Next(0, 10);
        }
        public int GetShipDirection()
        {
            return Random.Next(0, 4);
        }
        public int GetShipBackPlacementX(int shipDirection, int shipLength, int x, int y)
        {
            switch (shipDirection)
            {
                case 0:
                    return x;
                case 1:
                    return x;
                case 2:
                    return x - (shipLength - 1);
                case 3:
                    return x + (shipLength - 1);
            }
            return x;
        }
        public int GetShipBackPlacementY(int shipDirection, int shipLength, int x, int y)
        {
            switch (shipDirection)
            {
                case 0:
                    return y - (shipLength - 1);
                case 1:
                    return y + (shipLength - 1);
                case 2:
                    return y;
                case 3:
                    return y;
            }
            return y;
        }
        public bool GetValidCoordinates(int XPositionShipFront, int YPositionShipFront, int XPositionShipBack, int YPositionShipBack, int shipSize)
        {
            if (YPositionShipFront > YPositionShipBack && YPositionShipBack - shipSize < 0)
            {
                return false;
            }
            if (YPositionShipFront < YPositionShipBack && YPositionShipBack + shipSize > 9)
            {
                return false;
            }
            if (XPositionShipFront > XPositionShipBack && XPositionShipBack - shipSize < 0)
            {
                return false;
            }
            if (XPositionShipFront < XPositionShipBack && XPositionShipBack + shipSize > 9)
            {
                return false;
            }
            return true;
        }
    }
    public class GameGraphics
    {
        public static string cursor = "  ";
        public static char wall = '█';
        public static string sea = "  ";
        public static string carrier = "██";
        public static string battleship = "▓▓";
        public static string submarine = "░░";
        public static string destroyer = "▒▒";
        public static string hitCarrier = "╬█";
        public static string hitBattleship = "╫▓";
        public static string hitSubmarine = "╪░";
        public static string hitDestroyer = "┼▒";
        public static string MissMarker = "xx";
        public static int mapWidth = 12;
        public static int mapHeight = 12;
        public static int PlayerMapPositionY = 0;
        public static int PlayerMapPositionX = 0;
        public static int AiMapPositionY = 0;
        public static int AiMapPositionX = 15;
        public char[,] mapBorders;
        public string[,] shipMap;
        public string[,] concealedShipMap;
        public string[,] shipSelection;
        public void DrawShipPlacementFeedback(bool shipPlacementSelected, bool shipFrontPlaced)
        {
            if (shipPlacementSelected && !shipFrontPlaced)
            {
                Helpers.Write(0, 25, "Place the front by pressing Spacebar");
            }
            else if (shipPlacementSelected)
            {
                Helpers.Write(0, 25, "Place the back by pressing Spacebar ");
            }
            else
            {
                Helpers.Write(0, 25, "                                    ");
            }
        }
        public string[,] GenerateShipMap()
        {
            string[,] map = new string[mapWidth - 2, mapHeight - 2];

            for (int i = 0; i < mapWidth - 2; i++)
            {
                for (int j = 0; j < mapHeight - 2; j++)
                {
                    map[i, j] = sea;
                }
            }
            return map;
        }
        public char[,] GenerateMap()
        {
            char[,] map = new char[mapWidth, mapHeight];

            for (int i = 0; i < mapWidth; i++)
            {
                map[i, 0] = wall;
            }
            for (int i = 0; i < mapWidth; i++)
            {
                map[i, mapHeight - 1] = wall;
            }
            for (int i = 0; i < mapHeight; i++)
            {
                map[0, i] = wall;
            }
            for (int i = 0; i < mapHeight; i++)
            {
                map[mapWidth - 1, i] = wall;
            }
            return map;
        }
        public void DrawShipMap(string[,] map, int positionX, int positionY)
        {
            //Console.BackgroundColor = ConsoleColor.Cyan;
            for (int x = 0; x < mapWidth - 2; x++)
            {
                for (int y = 0; y < mapHeight - 2; y++)
                {
                    Helpers.Write(x + 1 + positionX, y + 1 + positionY, map[x, y]);
                }
            }
            //Console.BackgroundColor = ConsoleColor.Black;
        }
        public void DrawMap(char[,] map, int positionX, int positionY)
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    Helpers.Write(x + positionX, y + positionY, map[x, y]);
                }
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public string[,] PlaceShip(string[,] shipMap, int XPositionShipFront, int YPositionShipFront, int XPositionShipBack, int YPositionShipBack, int shipSize)
        {
            if (YPositionShipFront < YPositionShipBack)
            {
                for (int i = YPositionShipFront; i < (YPositionShipFront + shipSize); i++)
                {
                    switch (shipSize)
                    {
                        case 2:
                            shipMap[XPositionShipFront - 1, i - 1] = destroyer;
                            break;
                        case 3:
                            shipMap[XPositionShipFront - 1, i - 1] = submarine;
                            break;
                        case 4:
                            shipMap[XPositionShipFront - 1, i - 1] = battleship;
                            break;
                        case 5:
                            shipMap[XPositionShipFront - 1, i - 1] = carrier;
                            break;
                    }
                }
            }
            else if (YPositionShipFront > YPositionShipBack)
            {
                for (int i = YPositionShipFront; i > (YPositionShipFront - shipSize); i--)
                {
                    switch (shipSize)
                    {
                        case 2:
                            shipMap[XPositionShipFront - 1, i - 1] = destroyer;
                            break;
                        case 3:
                            shipMap[XPositionShipFront - 1, i - 1] = submarine;
                            break;
                        case 4:
                            shipMap[XPositionShipFront - 1, i - 1] = battleship;
                            break;
                        case 5:
                            shipMap[XPositionShipFront - 1, i - 1] = carrier;
                            break;
                    }
                }
            }
            else if (XPositionShipFront < XPositionShipBack)
            {
                for (int i = XPositionShipFront; i < (XPositionShipFront + (shipSize)); i++)
                {
                    switch (shipSize)
                    {
                        case 2:
                            shipMap[i - 1, YPositionShipFront - 1] = destroyer;
                            break;
                        case 3:
                            shipMap[i - 1, YPositionShipFront - 1] = submarine;
                            break;
                        case 4:
                            shipMap[i - 1, YPositionShipFront - 1] = battleship;
                            break;
                        case 5:
                            shipMap[i - 1, YPositionShipFront - 1] = carrier;
                            break;
                    }
                }
            }
            else
            {
                for (int i = XPositionShipFront; i > (XPositionShipFront - (shipSize)); i--)
                {
                    switch (shipSize)
                    {
                        case 2:
                            shipMap[i - 1, YPositionShipFront - 1] = destroyer;
                            break;
                        case 3:
                            shipMap[i - 1, YPositionShipFront - 1] = submarine;
                            break;
                        case 4:
                            shipMap[i - 1, YPositionShipFront - 1] = battleship;
                            break;
                        case 5:
                            shipMap[i - 1, YPositionShipFront - 1] = carrier;
                            break;
                    }
                }
            }
            return shipMap;

        }
        public string[,] GenerateShipSelection(int carrierMax, int battleshipMax, int submarineMax, int destroyerMax)
        {
            int shipSelectionWidth = 25;
            int shipSelectionHeight = 6;
            int carrierLength = 5;
            int battleshipLength = 4;
            int submarineLength = 3;
            int destroyerLength = 2;
            string[,] shipSelection = new string[shipSelectionWidth, shipSelectionHeight];

            for (int i = 0; i < shipSelectionWidth; i++)
            {
                for (int j = 0; j < shipSelectionHeight; j++)
                {
                    if (i == 0 && j >= shipSelectionHeight - carrierLength - 1)
                    {
                        shipSelection[i, j] = carrier;
                        if (j == shipSelectionHeight - 1)
                        {
                            shipSelection[i, j] = $"{carrierMax}:";
                        }
                    }
                    else if (i == 7 && j >= shipSelectionHeight - battleshipLength - 1)
                    {
                        shipSelection[i, j] = battleship;
                        if (j == shipSelectionHeight - 1)
                        {
                            shipSelection[i, j] = $"{battleshipMax}:";
                        }
                    }
                    else if (i == 14 && j >= shipSelectionHeight - submarineLength - 1)
                    {
                        shipSelection[i, j] = submarine;
                        if (j == shipSelectionHeight - 1)
                        {
                            shipSelection[i, j] = $"{submarineMax}:";
                        }
                    }
                    else if (i == 21 && j >= shipSelectionHeight - destroyerLength - 1)
                    {
                        shipSelection[i, j] = destroyer;
                        if (j == shipSelectionHeight - 1)
                        {
                            shipSelection[i, j] = $"{destroyerMax}:";
                        }
                    }
                    else if (j == shipSelectionHeight - 1 && i == 1 || j == shipSelectionHeight - 1 && i == 8 || j == shipSelectionHeight - 1 && i == 15 || j == shipSelectionHeight - 1 && i == 22)
                    {
                        shipSelection[i, j] = " L";
                    }
                    else if (j == shipSelectionHeight - 1 && i == 2 || j == shipSelectionHeight - 1 && i == 9 || j == shipSelectionHeight - 1 && i == 16 || j == shipSelectionHeight - 1 && i == 23)
                    {
                        shipSelection[i, j] = "ef";
                    }
                    else if (j == shipSelectionHeight - 1 && i == 3 || j == shipSelectionHeight - 1 && i == 10 || j == shipSelectionHeight - 1 && i == 17 || j == shipSelectionHeight - 1 && i == 24)
                    {
                        shipSelection[i, j] = "t ";
                    }
                    else
                    {
                        shipSelection[i, j] = "  ";
                    }
                }
            }



            return shipSelection;

        }
        public void DrawShipSelection(string[,] shipSelecion)
        {
            for (int x = 0; x < 25; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    Helpers.Write(x, y + 15, shipSelecion[x, y]);
                }
            }

            Helpers.Write(0, 21, "[1]Carrier    [2]Battleship [3]Submarine  [4]Destroyer");
            Helpers.Write(0, 23, "Place your ships, select by using numbers in brackets []");
        }
        public string[,] ShootShip(string[,] shipMap, string[,] concealedShipMap, int x, int y)
        {
            for (int i = 0; i < shipMap.GetLength(0); i++)
            {
                if (i == x - 1)
                {
                    for (int j = 0; j < shipMap.GetLength(1); j++)
                    {
                        if (j == y - 1)
                        {
                            if (shipMap[i, j] == sea)
                            {
                                concealedShipMap[i, j] = MissMarker;
                            }
                            else if (shipMap[i, j] == destroyer)
                            {
                                concealedShipMap[i, j] = hitDestroyer;
                            }
                            else if (shipMap[i, j] == submarine)
                            {
                                concealedShipMap[i, j] = hitSubmarine;
                            }
                            else if (shipMap[i, j] == battleship)
                            {
                                concealedShipMap[i, j] = hitBattleship;
                            }
                            else if (shipMap[i, j] == carrier)
                            {
                                concealedShipMap[i, j] = hitCarrier;
                            }
                            return concealedShipMap;
                        }
                    }
                }
            }
            return concealedShipMap;
        }
    }
    public static class Helpers
    {
        public static void Write(int x, int y, string charachter)
        {
            Console.SetCursorPosition(2 * x + 1, y + 1);
            Console.Write(charachter);

        }
        public static void Write(int x, int y, char wall)
        {
            Console.SetCursorPosition(2 * x + 1, y + 1);
            Console.Write(wall);
            Console.Write(wall);

        }
        public static void MoveCursor(int XCursor, int YCursor, string cursor)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Helpers.Write(XCursor, YCursor, cursor);
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
