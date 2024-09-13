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
        }
    }
}
