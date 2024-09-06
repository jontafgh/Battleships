namespace Battleships

{
    internal class Program
    {
        static void Main(string[] args)
        {
            //"██"
            ConsoleKeyInfo consoleKey;
            Console.CursorSize = 100;
            Console.SetWindowSize(120, 40);
            Console.SetBufferSize(120, 40);
            int XPositionCursor = 5;
            int YPositionCursor = 5;
            int XPositionCursorStored = 5;
            int YPositionCursorStored = 5;
            int XLastCollision = -1;
            int YLastCollision = -1;
            string character = "██";
            char wall = '█';
            int mapWidth = 20;
            int mapHeight = 20;


            Console.Clear();
            Console.CursorVisible = false;

            char[,] map = GenerateMap(mapWidth, mapHeight, wall);
            DrawMap(map, mapWidth, mapHeight);

            //Initial cursor
            Console.SetCursorPosition(XPositionCursor, YPositionCursor);
            Write(XPositionCursor, YPositionCursor, character);

            do
            {
                consoleKey = Console.ReadKey(true);
                //Clear old cursor
                Write(XPositionCursor, YPositionCursor, "  ");

                //Draw Ships
                PlaceShip(10, 10, 10, 12, 3, "  ");
                PlaceShip(10, 8, 10, 6, 2, "  ");

                PlaceShip(9, 16, 6, 16, 4, "  ");
                PlaceShip(13, 16, 17, 16, 5, "  ");
                

                XPositionCursorStored = XPositionCursor;
                YPositionCursorStored = YPositionCursor;
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
                }
                //Edge detection
                if (map[XPositionCursor, YPositionCursor] == wall) 
                {
                    //if (XPositionCursor != XLastCollision && YPositionCursor != YLastCollision)
                    //{
                    //    Console.Beep(800, 100);
                    //    XLastCollision = XPositionCursor;
                    //    YLastCollision = YPositionCursor;
                    //}
                    XPositionCursor = XPositionCursorStored;
                    YPositionCursor = YPositionCursorStored;
                }
                //else
                //{
                //    XLastCollision = -1;
                //    YLastCollision = -1;
                //}


                Write(XPositionCursor, YPositionCursor, character);


                
                

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

        static void DrawMap(char[,] map, int mapWidth, int mapHeight)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    Write(x, y, map[x, y]);
                }
            }
        }

        static void PlaceShip(int XPositionNose, int YPositionNose, int XPositionTail, int YPositionTail, int shipSize, string shipPart)
        {
            if (YPositionNose < YPositionTail)
            {
                for (int i = YPositionNose; i < (YPositionNose + shipSize); i++)
                {
                    //Console.SetCursorPosition(XPositionNose, i);
                    Console.BackgroundColor = ConsoleColor.Green;
                    //Console.WriteLine(shipPart);
                    Write(XPositionNose, i, shipPart);
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if (YPositionNose > YPositionTail)
            {
                for (int i = YPositionNose; i > (YPositionNose - shipSize); i--)
                {
                    //Console.SetCursorPosition(XPositionNose, i);
                    Console.BackgroundColor = ConsoleColor.Green;
                    //Console.WriteLine(shipPart);
                    Write(XPositionNose, i, shipPart);
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if (XPositionNose < XPositionTail)
            {
                for (int i = XPositionNose; i < (XPositionNose + (shipSize)); i++)
                {
                    //Console.SetCursorPosition(i, YPositionNose);
                    Console.BackgroundColor = ConsoleColor.Green;
                    //Console.WriteLine(shipPart);
                    Write(i, YPositionNose, shipPart);
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                for (int i = XPositionNose; i > (XPositionNose - (shipSize)); i--)
                {
                    //Console.SetCursorPosition(i, YPositionNose);
                    Console.BackgroundColor = ConsoleColor.Green;
                    //Console.WriteLine(shipPart);
                    Write(i, YPositionNose, shipPart);
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }

        }
    }
}
