namespace Battleships

{
    public class Map
    {
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }
        public int MapPositionX { get; private set; }
        public int MapPositionY { get; private set; }
        public int[,] ShipMap { get; set; }
        public int[,] ConcealedShipMap { get; set; }
        public int[,] MapBorders { get; set; }
        public Dictionary<int, ShipPartXY> PlacedDestroyers { get; private set; }
        public Dictionary<int, ShipPartXY> PlacedSubmarines { get; private set; }
        public Dictionary<int, ShipPartXY> PlacedBattleships { get; private set; }
        public Dictionary<int, ShipPartXY> PlacedCarriers { get; private set; }
        public Map(int mapPositionX, int mapPositionY)
        {
            MapWidth = 12;
            MapHeight = 12; 
            MapPositionX = mapPositionX;
            MapPositionY = mapPositionY;
            ShipMap = new int[MapWidth - 2, MapHeight - 2];
            ConcealedShipMap = new int[MapWidth - 2, MapHeight - 2];
            for (int i = 0; i < MapWidth - 2; i++)
            {
                for (int j = 0; j < MapHeight - 2; j++)
                {
                    ShipMap[i, j] = (int)UserInterface.ShipMapGraphics.Sea;
                    ConcealedShipMap[i, j] = (int)UserInterface.ShipMapGraphics.Sea;
                }
            }

            MapBorders = new int[MapWidth, MapHeight];
            for (int i = 0; i < MapWidth; i++)
            {
                MapBorders[i, 0] = (int)UserInterface.ShipMapGraphics.Wall;
            }
            for (int i = 0; i < MapWidth; i++)
            {
                MapBorders[i, MapHeight - 1] = (int)UserInterface.ShipMapGraphics.Wall;
            }
            for (int i = 0; i < MapHeight; i++)
            {
                MapBorders[0, i] = (int)UserInterface.ShipMapGraphics.Wall;
            }
            for (int i = 0; i < MapHeight; i++)
            {
                MapBorders[MapWidth - 1, i] = (int)UserInterface.ShipMapGraphics.Wall;
            }

            PlacedDestroyers = new Dictionary<int, ShipPartXY>()
            {
                {1, new ShipPartXY(2)}, {2, new ShipPartXY(2)}, {3, new ShipPartXY(2)}, {4, new ShipPartXY(2)}
            };
            PlacedSubmarines = new Dictionary<int, ShipPartXY>()
            {
                {1, new ShipPartXY(3)}, {2, new ShipPartXY(3)}, {3, new ShipPartXY(3)}
            };
            PlacedBattleships = new Dictionary<int, ShipPartXY>()
            {
                {1, new ShipPartXY(4)}, {2, new ShipPartXY(4)}
            };
            PlacedCarriers = new Dictionary<int, ShipPartXY>()
            {
                {1, new ShipPartXY(5)}
            };
        }
        public void UpdateShipMap()
        {
            for (int i = 0; i < ShipMap.GetLength(0); i++)
            {
                for (int j = 0; j < ShipMap.GetLength(1); j++)
                {
                    if (ConcealedShipMap[i, j] == (int)UserInterface.ShipMapGraphics.MissMarker)
                    {
                        ShipMap[i, j] = (int)UserInterface.ShipMapGraphics.MissMarker;
                    }
                    else if (ConcealedShipMap[i, j] == (int)UserInterface.ShipMapGraphics.HitDestroyer)
                    {
                        ShipMap[i, j] = (int)UserInterface.ShipMapGraphics.HitDestroyer;
                    }
                    else if (ConcealedShipMap[i, j] == (int)UserInterface.ShipMapGraphics.HitSubmarine)
                    {
                        ShipMap[i, j] = (int)UserInterface.ShipMapGraphics.HitSubmarine;
                    }
                    else if (ConcealedShipMap[i, j] == (int)UserInterface.ShipMapGraphics.HitBattleship)
                    {
                        ShipMap[i, j] = (int)UserInterface.ShipMapGraphics.HitBattleship;
                    }
                    else if (ConcealedShipMap[i, j] == (int)UserInterface.ShipMapGraphics.HitCarrier)
                    {
                        ShipMap[i, j] = (int)UserInterface.ShipMapGraphics.HitCarrier;
                    }
                }
            }
        }
    }
}
