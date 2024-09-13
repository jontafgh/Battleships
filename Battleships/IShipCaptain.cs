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
        public void SelectShip();
        public void GetShipPlacement();
        public bool GetValidShipPlacement();
        public void PlaceShip();        
        public bool GetAllShipsPlaced();
        public void GetShot();
        public bool GetValidShot(int[,] shipMap, int MapPositionX);
        public int[,] Shoot(int[,] shipMap, int[,] concealedShipMap, int MapPositionX);
        public bool GetWin(int[,] concealedShipMap);
    }

}
