namespace Battleships

{
    internal class Program
    {
        static void Main(string[] args)
        {
            //
            //Start
            //

            Engine engine = new Engine();
            GameUI ui = new GameUI();
            GameAI ai = new GameAI();

            ConsoleKeyInfo consoleKey;
            Console.SetWindowSize(120, 40);
            Console.SetBufferSize(120, 40);

            Console.Clear();
            Console.CursorVisible = false;


            //Initial UI state
            ui.map = ui.GenerateMap();
            ui.DrawMap(ui.map);

            ui.shipMap = ui.GenerateShipMap();
            ui.DrawShipMap(ui.shipMap);

            ui.shipSelection = ui.GenerateShipSelection(engine.carrierMax, engine.battleshipMax, engine.submarineMax, engine.destroyerMax);
            ui.DrawShipSelection(ui.shipSelection);

            Console.SetCursorPosition(engine.XCursor, engine.YCursor);
            Helpers.MoveCursor(engine.XCursor, engine.YCursor, ui.cursor);

            //
            //Ship placement phase
            //
            do
            {
                consoleKey = Console.ReadKey(true);

                //Clear old cursor                
                Helpers.MoveCursor(engine.XCursor, engine.YCursor, "  ");

                //Draw Ships
                ui.DrawShipMap(ui.shipMap);

                //Draw Ship Selection UI
                ui.DrawShipSelection(ui.shipSelection);

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
                                if (engine.GetValidShipPlacement(ui.shipMap, engine.XPositionShipFront, engine.YPositionShipFront, engine.XPositionShipBack, engine.YPositionShipBack, engine.selectedShipLength, ui.sea))
                                {
                                    ui.shipMap = ui.PlaceShip(ui.shipMap, engine.XPositionShipFront, engine.YPositionShipFront, engine.XPositionShipBack, engine.YPositionShipBack, engine.selectedShipLength);
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
                                    ui.shipSelection = ui.GenerateShipSelection(engine.carrierMax, engine.battleshipMax, engine.submarineMax, engine.destroyerMax);
                                    ui.DrawShipSelection(ui.shipSelection);
                                }
                                ui.DrawShipMap(ui.shipMap);
                                engine.shipFrontPlaced = false;
                                engine.shipPlacementSelected = false;
                            }
                        }
                        break;
                }

                //Placement of ships feedback                
                ui.DrawShipPlacementFeedback(engine.shipPlacementSelected, engine.shipFrontPlaced);

                //Edge detection                
                if (ui.map[engine.XCursor, engine.YCursor] == ui.wall)
                {
                    engine.XCursor = engine.XCursorStored;
                    engine.YCursor = engine.YCursorStored;
                }

                //Move Cursor                
                Helpers.MoveCursor(engine.XCursor, engine.YCursor, ui.cursor);

            } while (!engine.GetAllShipsPlaced(engine.carrierMax, engine.battleshipMax, engine.submarineMax, engine.destroyerMax));

            //
            //AI Generates map
            //

            ui.aiShipMap = ui.GenerateShipMap();
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


                    engine.XPositionShipFront = ai.GetShipFrontPlacement();
                    engine.YPositionShipFront = ai.GetShipFrontPlacement();

                    ai.ShipDirection = ai.GetShipDirection();

                    engine.XPositionShipBack = ai.GetShipBackPlacementX(ai.ShipDirection, engine.selectedShipLength, engine.XPositionShipFront, engine.YPositionShipFront);
                    engine.YPositionShipBack = ai.GetShipBackPlacementY(ai.ShipDirection, engine.selectedShipLength, engine.XPositionShipFront, engine.YPositionShipFront);                                       

                } while (!engine.GetValidShipPlacement(ui.aiShipMap, engine.XPositionShipFront + 1, engine.YPositionShipFront + 1, engine.XPositionShipBack + 1, engine.YPositionShipBack + 1, engine.selectedShipLength, ui.sea));

                ui.aiShipMap = ui.PlaceShip(ui.aiShipMap, engine.XPositionShipFront + 1, engine.YPositionShipFront + 1, engine.XPositionShipBack + 1, engine.YPositionShipBack + 1, engine.selectedShipLength);

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

            ui.DrawShipMap(ui.aiShipMap);

            //
            //Main game ui initialization
            //            

            //
            //Main game phase
            //

        }

    }

    public class Engine
    {
        public int XCursor = 5;
        public int YCursor = 5;
        public int XCursorStored = 5;
        public int YCursorStored = 5;
        public bool shipFrontPlaced = false;
        public bool shipPlacementSelected = false;
        public bool allShipsPlaced = false;
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
        public bool GetValidShipPlacement(string[,] shipMap, int XPositionShipFront, int YPositionShipFront, int XPositionShipBack, int YPositionShipBack, int shipSize, string sea)
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
            if(XPositionShipFront == XPositionShipBack && YPositionShipFront == YPositionShipBack)
            {
                return false;
            } 
            if (YPositionShipFront < YPositionShipBack)
            {
                for (int i = YPositionShipFront; i < (YPositionShipFront + shipSize); i++)
                {
                    if (shipMap[XPositionShipFront - 1, i - 1] != sea)
                    {
                        return false;
                    }
                }
            }
            else if (YPositionShipFront > YPositionShipBack)
            {
                for (int i = YPositionShipFront; i > (YPositionShipFront - shipSize); i--)
                {
                    if (shipMap[XPositionShipFront - 1, i - 1] != sea)
                    {
                        return false;
                    }
                }
            }
            else if (XPositionShipFront < XPositionShipBack)
            {
                for (int i = XPositionShipFront; i < (XPositionShipFront + (shipSize)); i++)
                {
                    if (shipMap[i - 1, YPositionShipFront - 1] != sea)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = XPositionShipFront; i > (XPositionShipFront - (shipSize)); i--)
                {
                    if (shipMap[i - 1, YPositionShipFront - 1] != sea)
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
    }
    public class GameAI
    {
        public Random Random = new Random();
        public int AiBattleshipMax = 2;
        public int AiSubmarineMax = 3;
        public int AiDestroyerMax = 4;
        public int AiCarrierMax = 1;
        public string? ShipDirection;
        public bool ValidShipSelection = false;
        public bool ValidPlacement = false;
        public int GetShipSelection()
        {
            return Random.Next(2, 6);
        }
        public int GetShipFrontPlacement()
        {
            return Random.Next(0, 10);
        }
        public string GetShipDirection()
        {
            return $"{Random.Next(0, 2)}{Random.Next(0, 2)}";
        }
        public int GetShipBackPlacementX(string shipDirection, int shipLength, int x, int y)
        {
            switch (shipDirection)
            {
                case "00":
                    return x;
                case "01":
                    return x;
                case "10":
                    return x - (shipLength - 1);
                case "11":
                    return x + (shipLength - 1);
            }
            return x;
        }
        public int GetShipBackPlacementY(string shipDirection, int shipLength, int x, int y)
        {
            switch (shipDirection)
            {
                case "00":
                    return y - (shipLength - 1);
                case "01":
                    return y + (shipLength - 1);
                case "10":
                    return y;
                case "11":
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


    public class GameUI
    {
        public string cursor = "  ";
        public char wall = '█';
        public string sea = "  ";
        public string carrier = "██";
        public string battleship = "▓▓";
        public string submarine = "░░";
        public string destroyer = "▒▒";
        public int mapWidth = 12;
        public int mapHeight = 12;
        public char[,] map;
        public string[,] shipMap;
        public string[,] aiShipMap;
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
        public void DrawShipMap(string[,] map)
        {
            //Console.BackgroundColor = ConsoleColor.Cyan;
            for (int x = 0; x < mapWidth - 2; x++)
            {
                for (int y = 0; y < mapHeight - 2; y++)
                {
                    Helpers.Write(x + 1, y + 1, map[x, y]);
                }
            }
            //Console.BackgroundColor = ConsoleColor.Black;
        }
        public void DrawMap(char[,] map)
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    Helpers.Write(x, y, map[x, y]);
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
