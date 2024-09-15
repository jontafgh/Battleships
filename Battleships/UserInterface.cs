using System.Numerics;

namespace Battleships

{
    public class UserInterface
    {
        public string Cursor = "  ";
        public string Wall = "██";
        public string Sea = "  ";
        public string Carrier = "██";
        public string Battleship = "▓▓";
        public string Submarine = "░░";
        public string Destroyer = "▒▒";
        public string HitCarrier = "╬█";
        public string HitBattleship = "╫▓";
        public string HitSubmarine = "╪░";
        public string HitDestroyer = "┼▒";
        public string MissMarker = "xx";
        public enum ShipMapGraphics
        {
            Sea = 0, MissMarker = 1, Wall = 10, Cursor = 11,
            Destroyer = 2, Submarine = 3, Battleship = 4, Carrier = 5,
            HitDestroyer = -2, HitSubmarine = -3, HitBattleship = -4, HitCarrier = -5
        }
        public Dictionary<int, string> Graphics { get; private set; }
        public UserInterface()
        {
            Graphics = new Dictionary<int, string>();
            Graphics.Add((int)UserInterface.ShipMapGraphics.Cursor, Cursor);
            Graphics.Add((int)UserInterface.ShipMapGraphics.Wall, Wall);
            Graphics.Add((int)UserInterface.ShipMapGraphics.Sea, Sea);
            Graphics.Add((int)UserInterface.ShipMapGraphics.Carrier, Carrier);
            Graphics.Add((int)UserInterface.ShipMapGraphics.Battleship, Battleship);
            Graphics.Add((int)UserInterface.ShipMapGraphics.Submarine, Submarine);
            Graphics.Add((int)UserInterface.ShipMapGraphics.Destroyer, Destroyer);
            Graphics.Add((int)UserInterface.ShipMapGraphics.HitCarrier, HitCarrier);
            Graphics.Add((int)UserInterface.ShipMapGraphics.HitBattleship, HitBattleship);
            Graphics.Add((int)UserInterface.ShipMapGraphics.HitSubmarine, HitSubmarine);
            Graphics.Add((int)UserInterface.ShipMapGraphics.HitDestroyer, HitDestroyer);
            Graphics.Add((int)UserInterface.ShipMapGraphics.MissMarker, MissMarker);
        }
        public void DrawShipPlacementFeedback(bool shipPlacementSelected, bool shipFrontPlaced)
        {
            if (shipPlacementSelected && !shipFrontPlaced)
            {
                Write(0, 20, "Place the front by pressing Spacebar                                           ");
            }
            else if (shipPlacementSelected)
            {
                Write(0, 20, "Place the back by pressing Spacebar                                           ");
            }
            else
            {
                Write(0, 20, $"Select ship by pressing the number in the brackets []                        ");
            }
        }
        public void DrawShipMap(int mapWidth, int mapHeight, int MapPositionX, int MapPositionY, int[,] shipMap)
        {
            for (int x = 0; x < mapWidth - 2; x++)
            {
                for (int y = 0; y < mapHeight - 2; y++)
                {
                    bool value = Graphics.TryGetValue(shipMap[x, y], out string? toWrite);
                    Write(x + 1 + MapPositionX, y + 1 + MapPositionY, (value) ? toWrite : Sea);
                }
            }
        }
        public void DrawMapBorders(int mapWidth, int mapHeight, int MapPositionX, int MapPositionY, int[,] mapBorders)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    bool value = Graphics.TryGetValue((int)UserInterface.ShipMapGraphics.Wall, out string? toWrite);
                    Write(x + MapPositionX, y + MapPositionY, toWrite);
                }
            }
        }
        public void DrawShipSelectionUI(int carrierMax, int battleshipMax, int submarineMax, int destroyerMax)
        {

            Write(0, 12, $"{carrierMax}/1                                                                  ");
            Write(0, 13, $"{Graphics[(int)UserInterface.ShipMapGraphics.Carrier]}         {battleshipMax}/2                                             ");
            Write(0, 14, $"{Graphics[(int)UserInterface.ShipMapGraphics.Carrier]}         {Graphics[(int)UserInterface.ShipMapGraphics.Battleship]}            {submarineMax}/3                      ");
            Write(0, 15, $"{Graphics[(int)UserInterface.ShipMapGraphics.Carrier]}         {Graphics[(int)UserInterface.ShipMapGraphics.Battleship]}            {Graphics[(int)UserInterface.ShipMapGraphics.Submarine]}           {destroyerMax}/4");
            Write(0, 16, $"{Graphics[(int)UserInterface.ShipMapGraphics.Carrier]}         {Graphics[(int)UserInterface.ShipMapGraphics.Battleship]}            {Graphics[(int)UserInterface.ShipMapGraphics.Submarine]}           {Graphics[(int)UserInterface.ShipMapGraphics.Destroyer]}     ");
            Write(0, 17, $"{Graphics[(int)UserInterface.ShipMapGraphics.Carrier]}         {Graphics[(int)UserInterface.ShipMapGraphics.Battleship]}            {Graphics[(int)UserInterface.ShipMapGraphics.Submarine]}           {Graphics[(int)UserInterface.ShipMapGraphics.Destroyer]}     ");
            Write(0, 18, $"[1]Carrier [2]Battleship [3]Submarine [4]Destroyer                              ");
            Write(0, 19, $"Place your ships:                        ");
        }
        public void Write(int x, int y, string charachter)
        {
            Console.SetCursorPosition(2 * x + 1, y + 1);
            Console.Write(charachter);

        }
        public void MoveCursor(int XCursor, int YCursor)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Write(XCursor, YCursor, Graphics[(int)UserInterface.ShipMapGraphics.Cursor]);
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public void RemoveCursor(int XCursor, int YCursor, int[,] shipMap, int mapPositionX)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Write(XCursor, YCursor, Graphics[shipMap[XCursor - 1 - mapPositionX, YCursor - 1]]);
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public void UpdateCursorPosition(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }
        public void GetInnitialConsoleEnviroment()
        {
            Console.SetWindowSize(120, 40);
            Console.SetBufferSize(120, 40);
            Console.Clear();
            Console.CursorVisible = false;
        }
        public void DrawShootingPhaseUI(int[,] concealedShipMap, Dictionary<int, ShipPartXY> placedDestroyers, Dictionary<int, ShipPartXY> placedSubmarines, Dictionary<int, ShipPartXY> placedBattleships, Dictionary<int, ShipPartXY> placedCarriers, int mapPositionX, string player)
        {
            Write(0 + mapPositionX, 12, $"{player} Ship Status:");
            Write(0 + mapPositionX, 13, $"{Graphics[concealedShipMap[placedDestroyers[1].X[0], placedDestroyers[1].Y[0]]]} {Graphics[concealedShipMap[placedDestroyers[3].X[0], placedDestroyers[3].Y[0]]]}");
            Write(0 + mapPositionX, 14, $"{Graphics[concealedShipMap[placedDestroyers[1].X[1], placedDestroyers[1].Y[1]]]} {Graphics[concealedShipMap[placedDestroyers[3].X[1], placedDestroyers[3].Y[1]]]}");
            Write(0 + mapPositionX, 15, $"");
            Write(0 + mapPositionX, 16, $"{Graphics[concealedShipMap[placedDestroyers[2].X[0], placedDestroyers[2].Y[0]]]} {Graphics[concealedShipMap[placedDestroyers[4].X[0], placedDestroyers[4].Y[0]]]}");
            Write(0 + mapPositionX, 17, $"{Graphics[concealedShipMap[placedDestroyers[2].X[1], placedDestroyers[2].Y[1]]]} {Graphics[concealedShipMap[placedDestroyers[4].X[1], placedDestroyers[4].Y[1]]]}");

            Write(3 + mapPositionX, 15, $"{Graphics[concealedShipMap[placedSubmarines[1].X[0], placedSubmarines[1].Y[0]]]} {Graphics[concealedShipMap[placedSubmarines[2].X[0], placedSubmarines[2].Y[0]]]} {Graphics[concealedShipMap[placedSubmarines[3].X[0], placedSubmarines[3].Y[0]]]}");
            Write(3 + mapPositionX, 16, $"{Graphics[concealedShipMap[placedSubmarines[1].X[1], placedSubmarines[1].Y[1]]]} {Graphics[concealedShipMap[placedSubmarines[2].X[1], placedSubmarines[2].Y[1]]]} {Graphics[concealedShipMap[placedSubmarines[3].X[1], placedSubmarines[3].Y[1]]]}");
            Write(3 + mapPositionX, 17, $"{Graphics[concealedShipMap[placedSubmarines[1].X[2], placedSubmarines[1].Y[2]]]} {Graphics[concealedShipMap[placedSubmarines[2].X[2], placedSubmarines[2].Y[2]]]} {Graphics[concealedShipMap[placedSubmarines[3].X[2], placedSubmarines[3].Y[2]]]}");

            Write(8 + mapPositionX, 14, $"{Graphics[concealedShipMap[placedBattleships[1].X[0], placedBattleships[1].Y[0]]]} {Graphics[concealedShipMap[placedBattleships[2].X[0], placedBattleships[2].Y[0]]]}");
            Write(8 + mapPositionX, 15, $"{Graphics[concealedShipMap[placedBattleships[1].X[1], placedBattleships[1].Y[1]]]} {Graphics[concealedShipMap[placedBattleships[2].X[1], placedBattleships[2].Y[1]]]}");
            Write(8 + mapPositionX, 16, $"{Graphics[concealedShipMap[placedBattleships[1].X[2], placedBattleships[1].Y[2]]]} {Graphics[concealedShipMap[placedBattleships[2].X[2], placedBattleships[2].Y[2]]]}");
            Write(8 + mapPositionX, 17, $"{Graphics[concealedShipMap[placedBattleships[1].X[3], placedBattleships[1].Y[3]]]} {Graphics[concealedShipMap[placedBattleships[2].X[3], placedBattleships[2].Y[3]]]}");

            Write(11 + mapPositionX, 13, $"{Graphics[concealedShipMap[placedCarriers[1].X[0], placedCarriers[1].Y[0]]]}");
            Write(11 + mapPositionX, 14, $"{Graphics[concealedShipMap[placedCarriers[1].X[1], placedCarriers[1].Y[1]]]}");
            Write(11 + mapPositionX, 15, $"{Graphics[concealedShipMap[placedCarriers[1].X[2], placedCarriers[1].Y[2]]]}");
            Write(11 + mapPositionX, 16, $"{Graphics[concealedShipMap[placedCarriers[1].X[3], placedCarriers[1].Y[3]]]}");
            Write(11 + mapPositionX, 17, $"{Graphics[concealedShipMap[placedCarriers[1].X[4], placedCarriers[1].Y[4]]]}");
        }
        public void DrawAiShootingFeedback(int x, int y, int[,] shipMap)
        {
            Random random = new Random();
            string[] animation = ["/", "-", "\\", "|", "/", "-", "\\", "|"];
            string[] animation2 = ["   ", ".  ", ".. ", "...", "   ", ".  ", ".. ", "..."];
            Write(0, 19, "                                                                                         ");
            Write(0, 19, "AI is lining up a shot!");
            for (int i = 0; i < animation.Length; i++)
            {
                int x2 = 0;
                int y2 = 0;
                do
                {
                    x2 = random.Next(x , x + 3);
                    y2 = random.Next(y , y + 3);
                }while ((x2-1 < 0 || x2-1 > shipMap.GetLength(0) - 1) || (y2-1 < 0 || y2-1 > shipMap.GetLength(0) - 1));


                Console.BackgroundColor = ConsoleColor.Gray;
                Write(x2 , y2,  "  ");
                Thread.Sleep(random.Next(100, 500));
                Console.BackgroundColor = ConsoleColor.Black;
                Write(x2 , y2 , Graphics[shipMap[x2 - 1, y2 - 1]]);

                Write(0, 20, animation[i] + animation2[i]);

            }
            Write(0, 19, "                       ");
            Write(0, 20, "                       ");
        }
        public void DrawPlayerShootingFeedback()
        {
            Write(0, 19, "Shoot at enemy ships by pressing Spacebar");
        }
        public void DrawWinFeedback(bool winPlayer, bool winAI)
        {
            Write(0, 19, "                                                                                         ");

            if (winPlayer)
            {
                Write(0, 19, "Congratulations, you've won!");
            }
            if (winAI)
            {
                Write(0, 19, "AI wins!");
            }
            Write(0, 20, "Press any key to exit.");
            Console.ReadKey();
        }
    }
}
