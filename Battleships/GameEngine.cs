using System;
using System.Threading;

namespace Battleships

{
    public interface IShipCaptain
    {   
        public int XCursor { get; set; }
        public int YCursor { get; set; }
        public int XPositionShipFront { get; set; }
        public int YPositionShipFront { get; set; }
        public int XPositionShipBack { get; set; }
        public int YPositionShipBack { get; set; }
        public int CarrierMax { get; set; }
        public int BattleshipMax { get; set; }
        public int SubmarineMax { get; set; }
        public int DestroyerMax { get; set; }
        public int CarrierLength { get; set; }
        public int BattleshipLength { get; set; }
        public int SubmarineLength { get; set; }
        public int DestroyerLength { get; set; }
        public int SelectedShipLength { get; set; }
        public void PlaceShip();
        public void GetValidShipPlacement();
        public void GetAllShipsPlaced();
        public void GetValidShot();
        

    }
    public class GameEngine
    {
        public int XCursor = 5;
        public int YCursor = 5;
        public int XCursorStored = 5;
        public int YCursorStored = 5;
        public bool SpacebarPressed = false;
        public bool shipFrontPlaced = false;
        public bool shipPlacementSelected = false;
        public bool allShipsPlaced = false;
        public bool ValidShot = false;
        public bool Win = false;        
        public bool PlayerDoneShooting = false;
        public int XPositionShipFront = 0;
        public int YPositionShipFront = 0;
        public int XPositionShipBack = 0;
        public int YPositionShipBack = 0;
        public int carrierMax = 1;
        public int battleshipMax = 2;
        public int submarineMax = 3;
        public int destroyerMax = 4;
        public int carrierLength = 5;
        public int battleshipLength = 4;
        public int submarineLength = 3;
        public int destroyerLength = 2;
        public int selectedShipLength = 0;
        public bool GetValidShipPlacement(string[,] shipMap, int MapCheckOffsetConsideration)
        {
            if (YPositionShipFront > YPositionShipBack && YPositionShipFront - selectedShipLength < 0)
            {
                return false;
            }
            if (YPositionShipFront < YPositionShipBack && YPositionShipFront + selectedShipLength > 11)
            {
                return false;
            }
            if (XPositionShipFront > XPositionShipBack && XPositionShipFront - selectedShipLength < 0)
            {
                return false;
            }
            if (XPositionShipFront < XPositionShipBack && XPositionShipFront + selectedShipLength > 11)
            {
                return false;
            }
            if (XPositionShipFront == XPositionShipBack && YPositionShipFront == YPositionShipBack)
            {
                return false;
            }
            if (YPositionShipFront < YPositionShipBack)
            {
                for (int i = YPositionShipFront; i < (YPositionShipFront + selectedShipLength); i++)
                {
                    if (shipMap[XPositionShipFront + MapCheckOffsetConsideration, i - 1] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else if (YPositionShipFront > YPositionShipBack)
            {
                for (int i = YPositionShipFront; i > (YPositionShipFront - selectedShipLength); i--)
                {
                    if (shipMap[XPositionShipFront + MapCheckOffsetConsideration, i - 1 ] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else if (XPositionShipFront < XPositionShipBack)
            {
                for (int i = XPositionShipFront; i < (XPositionShipFront + (selectedShipLength)); i++)
                {
                    if (shipMap[i - 1, YPositionShipFront + MapCheckOffsetConsideration] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = XPositionShipFront; i > (XPositionShipFront - (selectedShipLength)); i--)
                {
                    if (shipMap[i - 1, YPositionShipFront + MapCheckOffsetConsideration] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool GetAllShipsPlaced()
        {
            if (carrierMax == 0 && battleshipMax == 0 && submarineMax == 0 && destroyerMax == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GetValidShot(string[,] shipMap, int MapCheckOffsetConsideration, int MapPositionX)
        {
            if ((XCursor - MapPositionX + MapCheckOffsetConsideration) > (shipMap.GetLength(0) - 1) || (XCursor - MapPositionX + MapCheckOffsetConsideration) < 0 || YCursor + MapCheckOffsetConsideration > (shipMap.GetLength(0) - 1) || YCursor + MapCheckOffsetConsideration < 0)
            {
                return false;
            }

            for (int i = 0; i < shipMap.GetLength(0); i++)
            {
                if (i == XCursor - MapPositionX + MapCheckOffsetConsideration)
                {
                    for (int j = 0; j < shipMap.GetLength(1); j++)
                    {
                        if (j == YCursor + MapCheckOffsetConsideration)
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
                    if (carrierMax > 0)
                    {
                        selectedShipLength = carrierLength;
                        shipPlacementSelected = true;
                    }
                    break;
                case ConsoleKey.D2:
                    if (battleshipMax > 0)
                    {
                        selectedShipLength = battleshipLength;
                        shipPlacementSelected = true;
                    }
                    break;
                case ConsoleKey.D3:
                    if (submarineMax > 0)
                    {
                        selectedShipLength = submarineLength;
                        shipPlacementSelected = true;
                    }
                    break;
                case ConsoleKey.D4:
                    if (destroyerMax > 0)
                    {
                        selectedShipLength = destroyerLength;
                        shipPlacementSelected = true;
                    }
                    break;
                case ConsoleKey.Spacebar:
                    return true;                   
                    
            }
            return false;
        }
        public string[,] PlaceShip(string[,] shipMap, int MapCheckOffsetConsideration)
        {
            if (YPositionShipFront < YPositionShipBack)
            {
                for (int i = YPositionShipFront; i < (YPositionShipFront + selectedShipLength); i++)
                {
                    switch (selectedShipLength)
                    {
                        case 2:
                            shipMap[XPositionShipFront + MapCheckOffsetConsideration, i + MapCheckOffsetConsideration] = GameGraphics.destroyer;
                            break;
                        case 3:
                            shipMap[XPositionShipFront + MapCheckOffsetConsideration, i + MapCheckOffsetConsideration] = GameGraphics.submarine;
                            break;
                        case 4:
                            shipMap[XPositionShipFront + MapCheckOffsetConsideration, i + MapCheckOffsetConsideration] = GameGraphics.battleship;
                            break;
                        case 5:
                            shipMap[XPositionShipFront + MapCheckOffsetConsideration, i + MapCheckOffsetConsideration] = GameGraphics.carrier;
                            break;
                    }
                }
            }
            else if (YPositionShipFront > YPositionShipBack)
            {
                for (int i = YPositionShipFront; i > (YPositionShipFront - selectedShipLength); i--)
                {
                    switch (selectedShipLength)
                    {
                        case 2:
                            shipMap[XPositionShipFront + MapCheckOffsetConsideration, i + MapCheckOffsetConsideration] = GameGraphics.destroyer;
                            break;
                        case 3:
                            shipMap[XPositionShipFront + MapCheckOffsetConsideration, i + MapCheckOffsetConsideration] = GameGraphics.submarine;
                            break;
                        case 4:
                            shipMap[XPositionShipFront + MapCheckOffsetConsideration, i + MapCheckOffsetConsideration] = GameGraphics.battleship;
                            break;
                        case 5:
                            shipMap[XPositionShipFront + MapCheckOffsetConsideration, i + MapCheckOffsetConsideration] = GameGraphics.carrier;
                            break;
                    }
                }
            }
            else if (XPositionShipFront < XPositionShipBack)
            {
                for (int i = XPositionShipFront; i < (XPositionShipFront + (selectedShipLength)); i++)
                {
                    switch (selectedShipLength)
                    {
                        case 2:
                            shipMap[i + MapCheckOffsetConsideration, YPositionShipFront + MapCheckOffsetConsideration] = GameGraphics.destroyer;
                            break;
                        case 3:
                            shipMap[i + MapCheckOffsetConsideration, YPositionShipFront + MapCheckOffsetConsideration] = GameGraphics.submarine;
                            break;
                        case 4:
                            shipMap[i + MapCheckOffsetConsideration, YPositionShipFront + MapCheckOffsetConsideration] = GameGraphics.battleship;
                            break;
                        case 5:
                            shipMap[i + MapCheckOffsetConsideration, YPositionShipFront + MapCheckOffsetConsideration] = GameGraphics.carrier;
                            break;
                    }
                }
            }
            else
            {
                for (int i = XPositionShipFront; i > (XPositionShipFront - (selectedShipLength)); i--)
                {
                    switch (selectedShipLength)
                    {
                        case 2:
                            shipMap[i + MapCheckOffsetConsideration, YPositionShipFront + MapCheckOffsetConsideration] = GameGraphics.destroyer;
                            break;
                        case 3:
                            shipMap[i + MapCheckOffsetConsideration, YPositionShipFront + MapCheckOffsetConsideration] = GameGraphics.submarine;
                            break;
                        case 4:
                            shipMap[i + MapCheckOffsetConsideration, YPositionShipFront + MapCheckOffsetConsideration] = GameGraphics.battleship;
                            break;
                        case 5:
                            shipMap[i + MapCheckOffsetConsideration, YPositionShipFront + MapCheckOffsetConsideration] = GameGraphics.carrier;
                            break;
                    }
                }
            }
            return shipMap;

        }
        public void GetShipPlacement(string[,] shipMap, int MapCheckOffsetConsideration)
        {
            if (SpacebarPressed)
            {
                if (shipPlacementSelected)
                {
                    if (!shipFrontPlaced)
                    {
                        XPositionShipFront = XCursor;
                        YPositionShipFront = YCursor;
                        shipFrontPlaced = true;
                    }
                    else
                    {
                        XPositionShipBack = XCursor;
                        YPositionShipBack = YCursor;
                        if (GetValidShipPlacement(shipMap, MapCheckOffsetConsideration))
                        {
                            shipMap = PlaceShip(shipMap, MapCheckOffsetConsideration);
                            switch (selectedShipLength)
                            {
                                case 2:
                                    destroyerMax--;
                                    break;
                                case 3:
                                    submarineMax--;
                                    break;
                                case 4:
                                    battleshipMax--;
                                    break;
                                case 5:
                                    carrierMax--;
                                    break;
                            }
                            //playerGraphics.shipSelection = playerGraphics.GenerateShipSelection(carrierMax, battleshipMax, submarineMax, destroyerMax);
                            //playerGraphics.DrawShipSelection();
                        }
                        //playerGraphics.DrawShipMap();
                        shipFrontPlaced = false;
                        shipPlacementSelected = false;                        
                    }
                }
            }
            SpacebarPressed = false;
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
