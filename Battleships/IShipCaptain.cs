namespace Battleships

{
    public interface IShipCaptain
    {
        public int XPosition { get; }
        public int YPosition { get; }
        public int[] XPositionsStored { get; set; }
        public int[] YPositionsStored { get; set; }
        public int XPositionShipFront { get; }
        public int YPositionShipFront { get; }
        public int XPositionShipBack { get; }
        public int YPositionShipBack { get; }
        public int DestroyerMax { get; }
        public int SubmarineMax { get; }
        public int BattleshipMax { get; }
        public int CarrierMax { get; }
        public int SelectedShipLength { get; }
        public bool SpacebarPressed { get; set; }
        public bool ShipFrontPlaced { get; set; }
        public bool ShipPlacementSelected { get; set; }
        public int ShotCounter { get; set; }
        public int HitCounter { get; set; }
        public bool Win { get; set; }
        public enum Ship
        {
            Destroyer = 2, Submarine = 3, Battleship = 4, Carrier = 5
        }
        public void SelectShip();
        public void GetShipPlacement(string[,] shipMap);
        public bool GetValidShipPlacement(string[,] shipMap);
        public string[,] PlaceShip(string[,] shipMap);        
        public bool GetAllShipsPlaced();
        public void GetShot();
        public bool GetValidShot(string[,] shipMap, int MapPositionX);
        public string[,] Shoot(string[,] shipMap, string[,] concealedShipMap, int MapPositionX);
        public bool GetWin(string[,] concealedShipMap);
    }

}
