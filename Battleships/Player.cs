using System;
using System.Threading;

namespace Battleships

{
    public interface IShipCaptain
    {
        public int XPosition { get; }
        public int YPosition { get; }
        public int XPositionShipFront { get; }
        public int YPositionShipFront { get; }
        public int XPositionShipBack { get; }
        public int YPositionShipBack { get; }
        public int DestroyerMax { get; }
        public int SubmarineMax { get; }
        public int BattleshipMax { get; }
        public int CarrierMax { get; }
        public int SelectedShipLength { get; }
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
    public class Player : IShipCaptain
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int XPositionStored { get; set; }
        public int YPositionStored { get; set; }
        public int XPositionShipFront { get; set; }
        public int YPositionShipFront { get; set; }
        public int XPositionShipBack { get; set; }
        public int YPositionShipBack { get; set; }
        public int DestroyerMax { get; set; }
        public int SubmarineMax { get; set; }
        public int BattleshipMax { get; set; }
        public int CarrierMax { get; set; }
        public int SelectedShipLength { get; set; }

        public bool SpacebarPressed { get; set; }
        public bool ShipFrontPlaced { get; set; }
        public bool ShipPlacementSelected { get; set; }
        public bool Win { get; set; }
        public bool PlayerDoneShooting { get; set; }
        public Player()
        {
            XPosition = 5;
            YPosition = 5;
            XPositionStored = 5;
            YPositionStored = 5;
            DestroyerMax = 4;
            SubmarineMax = 3;
            BattleshipMax = 2;
            CarrierMax = 1;
            SelectedShipLength = 0;
            SpacebarPressed = false;
            ShipFrontPlaced = false;
            ShipPlacementSelected = false;
            Win = false;
            PlayerDoneShooting = false;
        }
        public void SelectShip()
        {
            SpacebarPressed = false;
            ConsoleKeyInfo consoleKey = Console.ReadKey(true);
            switch (consoleKey.Key)
            {
                case ConsoleKey.UpArrow:
                    YPosition--;
                    Console.SetCursorPosition(XPosition, YPosition);
                    break;
                case ConsoleKey.DownArrow:
                    YPosition++;
                    Console.SetCursorPosition(XPosition, YPosition);
                    break;
                case ConsoleKey.LeftArrow:
                    XPosition--;
                    Console.SetCursorPosition(XPosition, YPosition);
                    break;
                case ConsoleKey.RightArrow:
                    XPosition++;
                    Console.SetCursorPosition(XPosition, YPosition);
                    break;
                case ConsoleKey.D1:
                    if (CarrierMax > 0)
                    {
                        SelectedShipLength = (int)IShipCaptain.Ship.Carrier;
                        ShipPlacementSelected = true;
                    }
                    break;
                case ConsoleKey.D2:
                    if (BattleshipMax > 0)
                    {
                        SelectedShipLength = (int)IShipCaptain.Ship.Battleship;
                        ShipPlacementSelected = true;
                    }
                    break;
                case ConsoleKey.D3:
                    if (SubmarineMax > 0)
                    {
                        SelectedShipLength = (int)IShipCaptain.Ship.Submarine;
                        ShipPlacementSelected = true;
                    }
                    break;
                case ConsoleKey.D4:
                    if (DestroyerMax > 0)
                    {
                        SelectedShipLength = (int)IShipCaptain.Ship.Destroyer;
                        ShipPlacementSelected = true;
                    }
                    break;
                case ConsoleKey.Spacebar:
                    SpacebarPressed = true;
                    break;

            }

        }
        public void GetShipPlacement(string[,] shipMap)
        {
            if (SpacebarPressed)
            {
                if (ShipPlacementSelected)
                {
                    if (!ShipFrontPlaced)
                    {
                        XPositionShipFront = XPosition;
                        YPositionShipFront = YPosition;
                        ShipFrontPlaced = true;
                    }
                    else
                    {
                        XPositionShipBack = XPosition;
                        YPositionShipBack = YPosition;
                        if (GetValidShipPlacement(shipMap))
                        {
                            shipMap = PlaceShip(shipMap);
                            switch (SelectedShipLength)
                            {
                                case 2:
                                    DestroyerMax--;
                                    break;
                                case 3:
                                    SubmarineMax--;
                                    break;
                                case 4:
                                    BattleshipMax--;
                                    break;
                                case 5:
                                    CarrierMax--;
                                    break;
                            }
                            //playerGraphics.shipSelection = playerGraphics.GenerateShipSelection(carrierMax, battleshipMax, submarineMax, destroyerMax);
                            //playerGraphics.DrawShipSelection();
                        }
                        //playerGraphics.DrawShipMap();
                        ShipFrontPlaced = false;
                        ShipPlacementSelected = false;
                    }
                }
            }
            SpacebarPressed = false;
        }
        public bool GetValidShipPlacement(string[,] shipMap)
        {
            if (YPositionShipFront > YPositionShipBack && YPositionShipFront - SelectedShipLength < 0)
            {
                return false;
            }
            if (YPositionShipFront < YPositionShipBack && YPositionShipFront + SelectedShipLength > 11)
            {
                return false;
            }
            if (XPositionShipFront > XPositionShipBack && XPositionShipFront - SelectedShipLength < 0)
            {
                return false;
            }
            if (XPositionShipFront < XPositionShipBack && XPositionShipFront + SelectedShipLength > 11)
            {
                return false;
            }
            if (XPositionShipFront == XPositionShipBack && YPositionShipFront == YPositionShipBack)
            {
                return false;
            }
            if (YPositionShipFront < YPositionShipBack)
            {
                for (int i = YPositionShipFront; i < (YPositionShipFront + SelectedShipLength); i++)
                {
                    if (shipMap[XPositionShipFront - 1, i - 1] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else if (YPositionShipFront > YPositionShipBack)
            {
                for (int i = YPositionShipFront; i > (YPositionShipFront - SelectedShipLength); i--)
                {
                    if (shipMap[XPositionShipFront - 1, i - 1] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else if (XPositionShipFront < XPositionShipBack)
            {
                for (int i = XPositionShipFront; i < (XPositionShipFront + (SelectedShipLength)); i++)
                {
                    if (shipMap[i - 1, YPositionShipFront - 1] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = XPositionShipFront; i > (XPositionShipFront - (SelectedShipLength)); i--)
                {
                    if (shipMap[i - 1, YPositionShipFront - 1] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public string[,] PlaceShip(string[,] shipMap)
        {
            if (YPositionShipFront < YPositionShipBack)
            {
                for (int i = YPositionShipFront; i < (YPositionShipFront + SelectedShipLength); i++)
                {
                    switch (SelectedShipLength)
                    {
                        case 2:
                            shipMap[XPositionShipFront - 1, i - 1] = GameGraphics.destroyer;
                            break;
                        case 3:
                            shipMap[XPositionShipFront - 1, i - 1] = GameGraphics.submarine;
                            break;
                        case 4:
                            shipMap[XPositionShipFront - 1, i - 1] = GameGraphics.battleship;
                            break;
                        case 5:
                            shipMap[XPositionShipFront - 1, i - 1] = GameGraphics.carrier;
                            break;
                    }
                }
            }
            else if (YPositionShipFront > YPositionShipBack)
            {
                for (int i = YPositionShipFront; i > (YPositionShipFront - SelectedShipLength); i--)
                {
                    switch (SelectedShipLength)
                    {
                        case 2:
                            shipMap[XPositionShipFront - 1, i - 1] = GameGraphics.destroyer;
                            break;
                        case 3:
                            shipMap[XPositionShipFront - 1, i - 1] = GameGraphics.submarine;
                            break;
                        case 4:
                            shipMap[XPositionShipFront - 1, i - 1] = GameGraphics.battleship;
                            break;
                        case 5:
                            shipMap[XPositionShipFront - 1, i - 1] = GameGraphics.carrier;
                            break;
                    }
                }
            }
            else if (XPositionShipFront < XPositionShipBack)
            {
                for (int i = XPositionShipFront; i < (XPositionShipFront + (SelectedShipLength)); i++)
                {
                    switch (SelectedShipLength)
                    {
                        case 2:
                            shipMap[i - 1, YPositionShipFront - 1] = GameGraphics.destroyer;
                            break;
                        case 3:
                            shipMap[i - 1, YPositionShipFront - 1] = GameGraphics.submarine;
                            break;
                        case 4:
                            shipMap[i - 1, YPositionShipFront - 1] = GameGraphics.battleship;
                            break;
                        case 5:
                            shipMap[i - 1, YPositionShipFront - 1] = GameGraphics.carrier;
                            break;
                    }
                }
            }
            else
            {
                for (int i = XPositionShipFront; i > (XPositionShipFront - (SelectedShipLength)); i--)
                {
                    switch (SelectedShipLength)
                    {
                        case 2:
                            shipMap[i - 1, YPositionShipFront - 1] = GameGraphics.destroyer;
                            break;
                        case 3:
                            shipMap[i - 1, YPositionShipFront - 1] = GameGraphics.submarine;
                            break;
                        case 4:
                            shipMap[i - 1, YPositionShipFront - 1] = GameGraphics.battleship;
                            break;
                        case 5:
                            shipMap[i - 1, YPositionShipFront - 1] = GameGraphics.carrier;
                            break;
                    }
                }
            }
            return shipMap;

        }
        public bool GetAllShipsPlaced()
        {
            if (CarrierMax == 0 && BattleshipMax == 0 && SubmarineMax == 0 && DestroyerMax == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void GetShot()
        {
            SpacebarPressed = false;
            ConsoleKeyInfo consoleKey = Console.ReadKey(true);
            switch (consoleKey.Key)
            {
                case ConsoleKey.UpArrow:
                    YPosition--;
                    Console.SetCursorPosition(XPosition, YPosition);
                    break;
                case ConsoleKey.DownArrow:
                    YPosition++;
                    Console.SetCursorPosition(XPosition, YPosition);
                    break;
                case ConsoleKey.LeftArrow:
                    XPosition--;
                    Console.SetCursorPosition(XPosition, YPosition);
                    break;
                case ConsoleKey.RightArrow:
                    XPosition++;
                    Console.SetCursorPosition(XPosition, YPosition);
                    break;
                case ConsoleKey.Spacebar:
                    SpacebarPressed = true;
                    break;

            }
        }
        public bool GetValidShot(string[,] shipMap, int MapPositionX)
        {
            if ((XPosition - MapPositionX - 1) > (shipMap.GetLength(0) - 1) || (XPosition - MapPositionX - 1) < 0 || YPosition - 1 > (shipMap.GetLength(0) - 1) || YPosition - 1 < 0)
            {
                return false;
            }

            for (int i = 0; i < shipMap.GetLength(0); i++)
            {
                if (i == XPosition - MapPositionX - 1)
                {
                    for (int j = 0; j < shipMap.GetLength(1); j++)
                    {
                        if (j == YPosition - 1)
                        {
                            if (shipMap[i, j] == GameGraphics.MissMarker || shipMap[i, j] == GameGraphics.hitCarrier || shipMap[i, j] == GameGraphics.hitBattleship || shipMap[i, j] == GameGraphics.hitSubmarine || shipMap[i, j] == GameGraphics.hitDestroyer)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public string[,] Shoot(string[,] shipMap, string [,] concealedShipMap, int MapPositionX)
        {
            for (int i = 0; i < shipMap.GetLength(0); i++)
            {
                if (i == XPosition - 1 - MapPositionX)
                {
                    for (int j = 0; j < shipMap.GetLength(1); j++)
                    {
                        if (j == YPosition - 1)
                        {
                            if (shipMap[i, j] == GameGraphics.sea)
                            {
                                concealedShipMap[i, j] = GameGraphics.MissMarker;
                            }
                            else if (shipMap[i, j] == GameGraphics.destroyer)
                            {
                                concealedShipMap[i, j] = GameGraphics.hitDestroyer;
                            }
                            else if (shipMap[i, j] == GameGraphics.submarine)
                            {
                                concealedShipMap[i, j] = GameGraphics.hitSubmarine;
                            }
                            else if (shipMap[i, j] == GameGraphics.battleship)
                            {
                                concealedShipMap[i, j] = GameGraphics.hitBattleship;
                            }
                            else if (shipMap[i, j] == GameGraphics.carrier)
                            {
                                concealedShipMap[i, j] = GameGraphics.hitCarrier;
                            }
                        }
                    }
                }
            }
            return concealedShipMap;
        }
        public bool GetWin(string[,] concealedShipMap)
        {
            int hitCounter = 0;
            for (int i = 0; i < concealedShipMap.GetLength(0); i++)
            {
                for (int j = 0; j < concealedShipMap.GetLength(1); j++)
                {
                    if (concealedShipMap[i, j] == GameGraphics.hitCarrier || concealedShipMap[i, j] == GameGraphics.hitBattleship || concealedShipMap[i, j] == GameGraphics.hitSubmarine || concealedShipMap[i, j] == GameGraphics.hitDestroyer)
                    {
                        hitCounter++;
                    }
                }
            }
            return (hitCounter >= 30) ? true : false;
        }

        public void StoreCursorPosition()
        {
            XPositionStored = XPosition;
            YPositionStored = YPosition;
        }
        public void GetEdgeOfMapDetection(char[,] mapBorders, int MapPositionX)
        {
            if (mapBorders[XPosition - MapPositionX, YPosition] == GameGraphics.wall)
            {
                XPosition = XPositionStored;
                YPosition = YPositionStored;
            }
        }
    }

}
