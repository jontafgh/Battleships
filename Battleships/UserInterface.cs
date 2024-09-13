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
                Write(0, 25, "Place the front by pressing Spacebar");
            }
            else if (shipPlacementSelected)
            {
                Write(0, 25, "Place the back by pressing Spacebar ");
            }
            else
            {
                Write(0, 25, "                                    ");
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
            Write(0, 13, $"Place your ships, select by using numbers in brackets []                        ");
            Write(0, 14, $"{carrierMax}/1                                                                  ");
            Write(0, 15, $"{Graphics[(int)UserInterface.ShipMapGraphics.Carrier]}         {battleshipMax}/2                                             ");
            Write(0, 16, $"{Graphics[(int)UserInterface.ShipMapGraphics.Carrier]}         {Graphics[(int)UserInterface.ShipMapGraphics.Battleship]}            {submarineMax}/3                      ");
            Write(0, 17, $"{Graphics[(int)UserInterface.ShipMapGraphics.Carrier]}         {Graphics[(int)UserInterface.ShipMapGraphics.Battleship]}            {Graphics[(int)UserInterface.ShipMapGraphics.Submarine]}           {destroyerMax}/4");
            Write(0, 18, $"{Graphics[(int)UserInterface.ShipMapGraphics.Carrier]}         {Graphics[(int)UserInterface.ShipMapGraphics.Battleship]}            {Graphics[(int)UserInterface.ShipMapGraphics.Submarine]}           {Graphics[(int)UserInterface.ShipMapGraphics.Destroyer]}     ");
            Write(0, 19, $"{Graphics[(int)UserInterface.ShipMapGraphics.Carrier]}         {Graphics[(int)UserInterface.ShipMapGraphics.Battleship]}            {Graphics[(int)UserInterface.ShipMapGraphics.Submarine]}           {Graphics[(int)UserInterface.ShipMapGraphics.Destroyer]}     ");
            Write(0, 20, $"[1]Carrier [2]Battleship [3]Submarine [4]Destroyer                              ");
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
    }
}
