using System;
using System.Threading;

namespace Battleships

{
    public class Player : IShipCaptain
    {
        public Map Map {  get; private set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int[] XPositionsStored { get; set; }
        public int[] YPositionsStored { get; set; }
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
        public int ShotCounter { get; set; }
        public int HitCounter { get; set; }
        public bool Win { get; set; }

        public bool PlayerDoneShooting { get; set; }
        public Player()
        {
            Map = new Map(0, 0);
            XPosition = 5;
            YPosition = 5;
            XPositionsStored = new int[1];
            YPositionsStored = new int[1];
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
        public void GetShipPlacement()
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
                        if (GetValidShipPlacement())
                        {
                            PlaceShip();
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
                        }
                        ShipFrontPlaced = false;
                        ShipPlacementSelected = false;
                    }
                }
            }
            SpacebarPressed = false;
        }
        public bool GetValidShipPlacement()
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
                    if (Map.ShipMap[XPositionShipFront - 1, i - 1] != (int)UserInterface.ShipMapGraphics.Sea)
                    {
                        return false;
                    }
                }
            }
            else if (YPositionShipFront > YPositionShipBack)
            {
                for (int i = YPositionShipFront; i > (YPositionShipFront - SelectedShipLength); i--)
                {
                    if (Map.ShipMap[XPositionShipFront - 1, i - 1] != (int)UserInterface.ShipMapGraphics.Sea)
                    {
                        return false;
                    }
                }
            }
            else if (XPositionShipFront < XPositionShipBack)
            {
                for (int i = XPositionShipFront; i < (XPositionShipFront + (SelectedShipLength)); i++)
                {
                    if (Map.ShipMap[i - 1, YPositionShipFront - 1] != (int)UserInterface.ShipMapGraphics.Sea)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = XPositionShipFront; i > (XPositionShipFront - (SelectedShipLength)); i--)
                {
                    if (Map.ShipMap[i - 1, YPositionShipFront - 1] != (int)UserInterface.ShipMapGraphics.Sea)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public void PlaceShip()
        {
            if (YPositionShipFront < YPositionShipBack)
            {
                for (int i = YPositionShipFront; i < (YPositionShipFront + SelectedShipLength); i++)
                {
                    switch (SelectedShipLength)
                    {
                        case 2:
                            Map.ShipMap[XPositionShipFront - 1, i - 1] = (int)UserInterface.ShipMapGraphics.Destroyer;
                            break;
                        case 3:
                            Map.ShipMap[XPositionShipFront - 1, i - 1] = (int)UserInterface.ShipMapGraphics.Submarine;
                            break;
                        case 4:
                            Map.ShipMap[XPositionShipFront - 1, i - 1] = (int)UserInterface.ShipMapGraphics.Battleship;
                            break;
                        case 5:
                            Map.ShipMap[XPositionShipFront - 1, i - 1] = (int)UserInterface.ShipMapGraphics.Carrier;
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
                            Map.ShipMap[XPositionShipFront - 1, i - 1] = (int)UserInterface.ShipMapGraphics.Destroyer;
                            break;
                        case 3:
                            Map.ShipMap[XPositionShipFront - 1, i - 1] = (int)UserInterface.ShipMapGraphics.Submarine;
                            break;
                        case 4:
                            Map.ShipMap[XPositionShipFront - 1, i - 1] = (int)UserInterface.ShipMapGraphics.Battleship;
                            break;
                        case 5:
                            Map.ShipMap[XPositionShipFront - 1, i - 1] = (int)UserInterface.ShipMapGraphics.Carrier;
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
                            Map.ShipMap[i - 1, YPositionShipFront - 1] = (int)UserInterface.ShipMapGraphics.Destroyer;
                            break;
                        case 3:
                            Map.ShipMap[i - 1, YPositionShipFront - 1] = (int)UserInterface.ShipMapGraphics.Submarine;
                            break;
                        case 4:
                            Map.ShipMap[i - 1, YPositionShipFront - 1] = (int)UserInterface.ShipMapGraphics.Battleship;
                            break;
                        case 5:
                            Map.ShipMap[i - 1, YPositionShipFront - 1] = (int)UserInterface.ShipMapGraphics.Carrier;
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
                            Map.ShipMap[i - 1, YPositionShipFront - 1] = (int)UserInterface.ShipMapGraphics.Destroyer;
                            break;
                        case 3:
                            Map.ShipMap[i - 1, YPositionShipFront - 1] = (int)UserInterface.ShipMapGraphics.Submarine;
                            break;
                        case 4:
                            Map.ShipMap[i - 1, YPositionShipFront - 1] = (int)UserInterface.ShipMapGraphics.Battleship;
                            break;
                        case 5:
                            Map.ShipMap[i - 1, YPositionShipFront - 1] = (int)UserInterface.ShipMapGraphics.Carrier;
                            break;
                    }
                }
            }
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
        public bool GetValidShot(int[,] shipMap, int MapPositionX)
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
                            if (shipMap[i, j] == (int)UserInterface.ShipMapGraphics.MissMarker || shipMap[i, j] == (int)UserInterface.ShipMapGraphics.HitCarrier || shipMap[i, j] == (int)UserInterface.ShipMapGraphics.HitBattleship || shipMap[i, j] == (int)UserInterface.ShipMapGraphics.HitSubmarine || shipMap[i, j] == (int)UserInterface.ShipMapGraphics.HitDestroyer)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public int[,] Shoot(int[,] shipMap, int[,] concealedShipMap, int MapPositionX)
        {
            for (int i = 0; i < shipMap.GetLength(0); i++)
            {
                if (i == XPosition - 1 - MapPositionX)
                {
                    for (int j = 0; j < shipMap.GetLength(1); j++)
                    {
                        if (j == YPosition - 1)
                        {
                            if (shipMap[i, j] == (int)UserInterface.ShipMapGraphics.Sea)
                            {
                                concealedShipMap[i, j] = (int)UserInterface.ShipMapGraphics.MissMarker;
                            }
                            else if (shipMap[i, j] == (int)UserInterface.ShipMapGraphics.Destroyer)
                            {
                                concealedShipMap[i, j] = (int)UserInterface.ShipMapGraphics.HitDestroyer;
                            }
                            else if (shipMap[i, j] == (int)UserInterface.ShipMapGraphics.Submarine)
                            {
                                concealedShipMap[i, j] = (int)UserInterface.ShipMapGraphics.HitSubmarine;
                            }
                            else if (shipMap[i, j] == (int)UserInterface.ShipMapGraphics.Battleship)
                            {
                                concealedShipMap[i, j] = (int)UserInterface.ShipMapGraphics.HitBattleship;
                            }
                            else if (shipMap[i, j] == (int)UserInterface.ShipMapGraphics.Carrier)
                            {
                                concealedShipMap[i, j] = (int)UserInterface.ShipMapGraphics.HitCarrier;
                            }
                        }
                    }
                }
            }
            return concealedShipMap;
        }
        public bool GetWin(int[,] concealedShipMap)
        {
            int hitCounter = 0;
            for (int i = 0; i < concealedShipMap.GetLength(0); i++)
            {
                for (int j = 0; j < concealedShipMap.GetLength(1); j++)
                {
                    if (concealedShipMap[i, j] == (int)UserInterface.ShipMapGraphics.HitCarrier || concealedShipMap[i, j] == (int)UserInterface.ShipMapGraphics.HitBattleship || concealedShipMap[i, j] == (int)UserInterface.ShipMapGraphics.HitSubmarine || concealedShipMap[i, j] == (int)UserInterface.ShipMapGraphics.HitDestroyer)
                    {
                        hitCounter++;
                    }
                }
            }
            return (hitCounter >= 30) ? true : false;
        }

        public void StoreCursorPosition()
        {
            XPositionsStored[0] = XPosition;
            YPositionsStored[0] = YPosition;
        }
        public void GetEdgeOfMapDetection(int[,] mapBorders, int MapPositionX)
        {
            if (mapBorders[XPosition - MapPositionX, YPosition] == (int)UserInterface.ShipMapGraphics.Wall)
            {
                XPosition = XPositionsStored[0];
                YPosition = YPositionsStored[0];
            }
        }
    }

}
