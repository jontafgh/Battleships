namespace Battleships

{
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
        public int MapPositionY;
        public int MapPositionX;
        public int MapCheckOffsetConsideration;
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
        public char[,] GenerateMapBorders()
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
        public void DrawShipMap()
        {
            for (int x = 0; x < mapWidth - 2; x++)
            {
                for (int y = 0; y < mapHeight - 2; y++)
                {
                    Helpers.Write(x + 1 + MapPositionX, y + 1 + MapPositionY, shipMap[x, y]);
                }
            }
        }
        public void DrawShipMap(string[,] shipMap)
        {
            for (int x = 0; x < mapWidth - 2; x++)
            {
                for (int y = 0; y < mapHeight - 2; y++)
                {
                    Helpers.Write(x + 1 + MapPositionX, y + 1 + MapPositionY, shipMap[x, y]);
                }
            }
        }
        public void DrawMapBorders()
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    Helpers.Write(x + MapPositionX, y + MapPositionY, mapBorders[x, y]);
                }
            }
            Console.ForegroundColor = ConsoleColor.Gray;
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
        public void DrawShipSelection()
        {
            for (int x = 0; x < 25; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    Helpers.Write(x, y + 15, shipSelection[x, y]);
                }
            }

            Helpers.Write(0, 21, "[1]Carrier    [2]Battleship [3]Submarine  [4]Destroyer");
            Helpers.Write(0, 23, "Place your ships, select by using numbers in brackets []");
        }
    }
}
