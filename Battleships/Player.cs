using System;
using System.Threading;

namespace Battleships

{
    public interface IShipCaptain
    {
        public int XCursor { get; }
        public int YCursor { get; }
        public int XPositionShipFront { get; }
        public int YPositionShipFront { get; }
        public int XPositionShipBack { get; }
        public int YPositionShipBack { get; }
        public int DestroyerMax { get; }
        public int SubmarineMax { get; }
        public int BattleshipMax { get; }
        public int CarrierMax { get; }
        public int DestroyerLength { get; }
        public int SubmarineLength { get; }
        public int BattleshipLength { get; }
        public int CarrierLength { get; }
        public int SelectedShipLength { get; }
        public void GetShipPlacement(string[,] shipMap);
        public bool GetValidShipPlacement(string[,] shipMap);
        public string[,] PlaceShip(string[,] shipMap);        
        public bool GetAllShipsPlaced();
        public bool GetValidShot(string[,] shipMap, int MapPositionX);
        public bool GetWin(string[,] concealedShipMap);

    }
    public class Player : IShipCaptain
    {
        public int XCursor { get; set; }
        public int YCursor { get; set; }
        public int XCursorStored { get; set; }
        public int YCursorStored { get; set; }
        public int XPositionShipFront { get; set; }
        public int YPositionShipFront { get; set; }
        public int XPositionShipBack { get; set; }
        public int YPositionShipBack { get; set; }
        public int DestroyerMax { get; set; }
        public int SubmarineMax { get; set; }
        public int BattleshipMax { get; set; }
        public int CarrierMax { get; set; }
        public int DestroyerLength { get; protected set; }
        public int SubmarineLength { get; protected set; }
        public int BattleshipLength { get; protected set; }
        public int CarrierLength { get; protected set; }
        public int SelectedShipLength { get; set; }

        public bool SpacebarPressed { get; set; }
        public bool ShipFrontPlaced { get; set; }
        public bool ShipPlacementSelected { get; set; }
        public bool AllShipsPlaced { get; set; }
        public bool ValidShot { get; set; }
        public bool Win { get; set; }
        public bool PlayerDoneShooting { get; set; }
        public Player()
        {
            XCursor = 5;
            YCursor = 5;
            XCursorStored = 5;
            YCursorStored = 5;
            DestroyerMax = 4;
            SubmarineMax = 3;
            BattleshipMax = 2;
            CarrierMax = 1;
            DestroyerLength = 2;
            SubmarineLength = 3;
            BattleshipLength = 4;
            CarrierLength = 5;
            SelectedShipLength = 0;
            SpacebarPressed = false;
            ShipFrontPlaced = false;
            ShipPlacementSelected = false;
            AllShipsPlaced = false;
            ValidShot = false;
            Win = false;
            PlayerDoneShooting = false;
        }
        public void GetShipPlacement(string[,] shipMap)
        {
            if (SpacebarPressed)
            {
                if (ShipPlacementSelected)
                {
                    if (!ShipFrontPlaced)
                    {
                        XPositionShipFront = XCursor;
                        YPositionShipFront = YCursor;
                        ShipFrontPlaced = true;
                    }
                    else
                    {
                        XPositionShipBack = XCursor;
                        YPositionShipBack = YCursor;
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
        public bool GetValidShot(string[,] shipMap, int MapPositionX)
        {
            if ((XCursor - MapPositionX - 1) > (shipMap.GetLength(0) - 1) || (XCursor - MapPositionX - 1) < 0 || YCursor - 1 > (shipMap.GetLength(0) - 1) || YCursor - 1 < 0)
            {
                return false;
            }

            for (int i = 0; i < shipMap.GetLength(0); i++)
            {
                if (i == XCursor - MapPositionX - 1)
                {
                    for (int j = 0; j < shipMap.GetLength(1); j++)
                    {
                        if (j == YCursor - 1)
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
            XCursorStored = XCursor;
            YCursorStored = YCursor;
        }
        public bool GetKeyPress(ConsoleKeyInfo consoleKey)
        {
            switch (consoleKey.Key)
            {
                case ConsoleKey.UpArrow:
                    YCursor--;
                    Console.SetCursorPosition(XCursor, YCursor);
                    break;
                case ConsoleKey.DownArrow:
                    YCursor++;
                    Console.SetCursorPosition(XCursor, YCursor);
                    break;
                case ConsoleKey.LeftArrow:
                    XCursor--;
                    Console.SetCursorPosition(XCursor, YCursor);
                    break;
                case ConsoleKey.RightArrow:
                    XCursor++;
                    Console.SetCursorPosition(XCursor, YCursor);
                    break;
                case ConsoleKey.D1:
                    if (CarrierMax > 0)
                    {
                        SelectedShipLength = CarrierLength;
                        ShipPlacementSelected = true;
                    }
                    break;
                case ConsoleKey.D2:
                    if (BattleshipMax > 0)
                    {
                        SelectedShipLength = BattleshipLength;
                        ShipPlacementSelected = true;
                    }
                    break;
                case ConsoleKey.D3:
                    if (SubmarineMax > 0)
                    {
                        SelectedShipLength = SubmarineLength;
                        ShipPlacementSelected = true;
                    }
                    break;
                case ConsoleKey.D4:
                    if (DestroyerMax > 0)
                    {
                        SelectedShipLength = DestroyerLength;
                        ShipPlacementSelected = true;
                    }
                    break;
                case ConsoleKey.Spacebar:
                    return true;

            }
            return false;
        }
        public void GetEdgeOfMapDetection(char[,] mapBorders, int MapPositionX)
        {
            if (mapBorders[XCursor - MapPositionX, YCursor] == GameGraphics.wall)
            {
                XCursor = XCursorStored;
                YCursor = YCursorStored;
            }
        }
    }

}
