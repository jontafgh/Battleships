namespace Battleships

{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo consoleKey;
            //Console.CursorSize = 100;
            Console.SetWindowSize(120, 40);
            Console.SetBufferSize(120, 40);
            int XPositionCursor = 5;
            int YPositionCursor = 5;
            int XPositionCursorStored = 5;
            int YPositionCursorStored = 5;
            //int XLastCollision = -1;
            //int YLastCollision = -1;
            string character = "  ";
            char wall = '█';
            //string ship = "▓▓";
            string sea = "  ";
            string carrier = "██";
            string battleship = "▓▓";
            string submarine = "░░";
            string destroyer = "▒▒";
            int mapWidth = 12;
            int mapHeight = 12;
            bool nosePlaced = false;
            int XPositionNose = 0;
            int YPositionNose = 0;
            int XPositionTail = 0;
            int YPositionTail = 0;
            bool shipPlacementSelected = false;
            int carrierMax = 1;
            int battleshipMax = 2;
            int submarineMax = 3;
            int destroyerMax = 4;
            int carrierLength = 5;
            int battleshipLength = 4;
            int submarineLength = 3;
            int destroyerLength = 2;
            int selectedShipLength = 0;
            Console.Clear();
            Console.CursorVisible = false;

            char[,] map = GenerateMap(mapWidth, mapHeight, wall);
            DrawMap(map, mapWidth, mapHeight);

            string[,] shipMap = GenerateShipMap(mapWidth, mapHeight, XPositionNose, YPositionNose, XPositionTail, YPositionTail, sea);
            DrawShipMap(shipMap, mapWidth, mapHeight);

            string[,] shipSelection = GenerateShipSelection(carrierMax, battleshipMax, submarineMax, destroyerMax, carrier, battleship, submarine, destroyer);
            DrawShipSelection(shipSelection);

            //Initial cursor
            Console.BackgroundColor = ConsoleColor.White;
            Console.SetCursorPosition(XPositionCursor, YPositionCursor);
            Write(XPositionCursor, YPositionCursor, character);
            Console.BackgroundColor = ConsoleColor.Black;

            do
            {
                consoleKey = Console.ReadKey(true);
                //Clear old cursor
                Console.BackgroundColor = ConsoleColor.White;
                Write(XPositionCursor, YPositionCursor, "  ");
                Console.BackgroundColor = ConsoleColor.Black;

                //Draw Ships
                DrawShipMap(shipMap, mapWidth, mapHeight);

                //Draw Ship Selection
                DrawShipSelection(shipSelection);

                // Save cursor position in case of wall
                XPositionCursorStored = XPositionCursor;
                YPositionCursorStored = YPositionCursor;

                //Read key press

                switch (consoleKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        YPositionCursor--;
                        Console.SetCursorPosition(XPositionCursor, YPositionCursor);
                        break;
                    case ConsoleKey.DownArrow:
                        YPositionCursor++;
                        Console.SetCursorPosition(XPositionCursor, YPositionCursor);
                        break;
                    case ConsoleKey.LeftArrow:
                        XPositionCursor--;
                        Console.SetCursorPosition(XPositionCursor, YPositionCursor);
                        break;
                    case ConsoleKey.RightArrow:
                        XPositionCursor++;
                        Console.SetCursorPosition(XPositionCursor, YPositionCursor);
                        break;
                    case ConsoleKey.D1:
                        if (carrierMax > 0)
                        {
                            selectedShipLength = carrierLength;
                            shipPlacementSelected = true;
                        }                        
                        break;
                    case ConsoleKey.D2:
                        if (battleshipMax > 0)
                        {
                            selectedShipLength = battleshipLength;
                            shipPlacementSelected = true;
                        }                        
                        break;
                    case ConsoleKey.D3:
                        if (submarineMax > 0)
                        {
                            selectedShipLength = submarineLength;
                            shipPlacementSelected = true;
                        }                        
                        break;
                    case ConsoleKey.D4:
                        if (destroyerMax > 0)
                        {
                            selectedShipLength = destroyerLength;
                            shipPlacementSelected = true;
                        }                        
                        break;
                    case ConsoleKey.Spacebar:
                        //Placement of ships
                        if (shipPlacementSelected)
                        {
                            if (!nosePlaced)
                            {
                                XPositionNose = XPositionCursor;
                                YPositionNose = YPositionCursor;
                                nosePlaced = true;
                            }
                            else
                            {
                                XPositionTail = XPositionCursor;
                                YPositionTail = YPositionCursor;
                                if (GetValidShipPlacement(shipMap, XPositionNose, YPositionNose, XPositionTail, YPositionTail, selectedShipLength, sea))
                                {
                                    shipMap = PlaceShip(shipMap, XPositionNose, YPositionNose, XPositionTail, YPositionTail, selectedShipLength, carrier, battleship, submarine, destroyer);
                                    switch (selectedShipLength)
                                    {
                                        case 2:
                                            destroyerMax--;
                                            break;
                                        case 3:
                                            submarineMax--;
                                            break;
                                        case 4:
                                            battleshipMax--;
                                            break;
                                        case 5:
                                            carrierMax--;
                                            break;
                                    }
                                    shipSelection = GenerateShipSelection(carrierMax, battleshipMax, submarineMax, destroyerMax, carrier, battleship, submarine, destroyer);
                                    DrawShipSelection(shipSelection);
                                }
                                DrawShipMap(shipMap, mapWidth, mapHeight);
                                nosePlaced = false;
                                shipPlacementSelected = false;
                            }
                        }
                        break;
                }

                //Placement of ships
                if (shipPlacementSelected && !nosePlaced)
                {
                    Write(0, 25, "Ok place NOSE by pressing Spacebar");
                }
                else if (shipPlacementSelected)
                {
                    Write(0, 25, "Ok place TAIL by pressing Spacebar");
                }
                else
                {
                    Write(0, 25, "                                  ");
                }
                //Edge detection
                if (map[XPositionCursor, YPositionCursor] == wall)
                {
                    XPositionCursor = XPositionCursorStored;
                    YPositionCursor = YPositionCursorStored;
                }

                //Move Cursor
                Console.BackgroundColor = ConsoleColor.White;
                Write(XPositionCursor, YPositionCursor, character);
                Console.BackgroundColor = ConsoleColor.Black;

            } while (true);
        }
        static void Write(int x, int y, string charachter)
        {
            Console.SetCursorPosition(2 * x + 1, y + 1);
            Console.Write(charachter);

        }
        static void Write(int x, int y, char wall)
        {
            Console.SetCursorPosition(2 * x + 1, y + 1);
            Console.Write(wall);
            Console.Write(wall);

        }
        static string[,] GenerateShipMap(int mapWidth, int mapHeight, int XPositionNose, int YPositionNose, int XPositionTail, int YPositionTail, string sea)
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
        static char[,] GenerateMap(int mapWidth, int mapHeight, char wall)
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

            ////Inside Wall
            //for (int i = 8; i < mapHeight - 5; i++)
            //{
            //    map[12, i] = wall;
            //}

            return map;
        }
        static void DrawShipMap(string[,] map, int mapWidth, int mapHeight)
        {
            //Console.BackgroundColor = ConsoleColor.Cyan;
            for (int x = 0; x < mapWidth - 2; x++)
            {
                for (int y = 0; y < mapHeight - 2; y++)
                {
                    Write(x + 1, y + 1, map[x, y]);
                }
            }
            //Console.BackgroundColor = ConsoleColor.Black;
        }
        static void DrawMap(char[,] map, int mapWidth, int mapHeight)
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    Write(x, y, map[x, y]);
                }
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        static string[,] PlaceShip(string[,] shipMap, int XPositionNose, int YPositionNose, int XPositionTail, int YPositionTail, int shipSize, string carrier, string battleship, string submarine, string destroyer)
        {
            if (YPositionNose < YPositionTail)
            {
                for (int i = YPositionNose; i < (YPositionNose + shipSize); i++)
                {
                    switch (shipSize)
                    {
                        case 2:
                            shipMap[XPositionNose - 1, i - 1] = destroyer;
                            break;
                        case 3:
                            shipMap[XPositionNose - 1, i - 1] = submarine;
                            break;
                        case 4:
                            shipMap[XPositionNose - 1, i - 1] = battleship;
                            break;
                        case 5:
                            shipMap[XPositionNose - 1, i - 1] = carrier;
                            break;
                    }                    
                }
            }
            else if (YPositionNose > YPositionTail)
            {
                for (int i = YPositionNose; i > (YPositionNose - shipSize); i--)
                {
                    switch (shipSize)
                    {
                        case 2:
                            shipMap[XPositionNose - 1, i - 1] = destroyer;
                            break;
                        case 3:
                            shipMap[XPositionNose - 1, i - 1] = submarine;
                            break;
                        case 4:
                            shipMap[XPositionNose - 1, i - 1] = battleship;
                            break;
                        case 5:
                            shipMap[XPositionNose - 1, i - 1] = carrier;
                            break;
                    }
                }
            }
            else if (XPositionNose < XPositionTail)
            {
                for (int i = XPositionNose; i < (XPositionNose + (shipSize)); i++)
                {
                    switch (shipSize)
                    {
                        case 2:
                            shipMap[i - 1, YPositionNose - 1] = destroyer;
                            break;
                        case 3:
                            shipMap[i - 1, YPositionNose - 1] = submarine;
                            break;
                        case 4:
                            shipMap[i - 1, YPositionNose - 1] = battleship;
                            break;
                        case 5:
                            shipMap[i - 1, YPositionNose - 1] = carrier;
                            break;
                    }
                }
            }
            else
            {
                for (int i = XPositionNose; i > (XPositionNose - (shipSize)); i--)
                {
                    switch (shipSize)
                    {
                        case 2:
                            shipMap[i - 1, YPositionNose - 1] = destroyer;
                            break;
                        case 3:
                            shipMap[i - 1, YPositionNose - 1] = submarine;
                            break;
                        case 4:
                            shipMap[i - 1, YPositionNose - 1] = battleship;
                            break;
                        case 5:
                            shipMap[i - 1, YPositionNose - 1] = carrier;
                            break;
                    }
                }
            }
            return shipMap;

        }
        static bool GetValidShipPlacement(string[,] shipMap, int XPositionNose, int YPositionNose, int XPositionTail, int YPositionTail, int shipSize, string sea)
        {
            if (YPositionNose < YPositionTail)
            {
                for (int i = YPositionNose; i < (YPositionNose + shipSize); i++)
                {
                    if (shipMap[XPositionNose - 1, i - 1] != sea)
                    {
                        return false;
                    }
                }
            }
            else if (YPositionNose > YPositionTail)
            {
                for (int i = YPositionNose; i > (YPositionNose - shipSize); i--)
                {
                    if (shipMap[XPositionNose - 1, i - 1] != sea)
                    {
                        return false;
                    }
                }
            }
            else if (XPositionNose < XPositionTail)
            {
                for (int i = XPositionNose; i < (XPositionNose + (shipSize)); i++)
                {
                    if (shipMap[i - 1, YPositionNose - 1] != sea)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = XPositionNose; i > (XPositionNose - (shipSize)); i--)
                {
                    if (shipMap[i - 1, YPositionNose - 1] != sea)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        static string[,] GenerateShipSelection(int carrierMax, int battleshipMax, int submarineMax, int destroyerMax, string carrier, string battleship, string submarine, string destroyer)
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
        static void DrawShipSelection(string[,] shipSelecion)
        {
            for (int x = 0; x < 25; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    Write(x, y + 15, shipSelecion[x, y]);
                }
            }

            Write(0, 21, "[1]Carrier    [2]Battleship [3]Submarine  [4]Destroyer");
            Write(0, 23, "Place your ships, select by using numbers in brackets []");
        }
    }
}
