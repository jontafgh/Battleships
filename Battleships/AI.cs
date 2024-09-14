using System.Xml.Linq;

namespace Battleships

{
    public class AI : IShipCaptain
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
        public AI()
        {
            Map = new Map(15, 0);
            DestroyerMax = 4;
            SubmarineMax = 3;
            BattleshipMax = 2;
            CarrierMax = 1;
            SelectedShipLength = 0;            
            XPositionsStored = new int[5];
            YPositionsStored = new int[5];
            ShotCounter = 0;
            HitCounter = 0;
        }
        public void SelectShip()
        {
            Random Random = new Random();
            bool ValidShipSelection = false;
            do
            {
                SelectedShipLength = Random.Next(2, 6);

                switch (SelectedShipLength)
                {
                    case 2:
                        if (DestroyerMax == 0)
                        {
                            ValidShipSelection = false;
                        }
                        else
                        {
                            ValidShipSelection = true;
                        }
                        break;
                    case 3:
                        if (SubmarineMax == 0)
                        {
                            ValidShipSelection = false;
                        }
                        else
                        {
                            ValidShipSelection = true;
                        }
                        break;
                    case 4:
                        if (BattleshipMax == 0)
                        {
                            ValidShipSelection = false;
                        }
                        else
                        {
                            ValidShipSelection = true;
                        }
                        break;
                    case 5:
                        if (CarrierMax == 0)
                        {
                            ValidShipSelection = false;
                        }
                        else
                        {
                            ValidShipSelection = true;
                        }
                        break;
                }
            } while (!ValidShipSelection);
        }
        public void GetShipPlacement()
        {
            Random Random = new Random();
            XPositionShipFront = Random.Next(0, 10);
            YPositionShipFront = Random.Next(0, 10);

            int shipDirection = Random.Next(0, 4);

            switch (shipDirection)
            {
                case 0:
                    XPositionShipBack = XPositionShipFront;
                    YPositionShipBack = YPositionShipFront - (SelectedShipLength - 1);
                    break;
                case 1:
                    XPositionShipBack = XPositionShipFront;
                    YPositionShipBack = YPositionShipFront + (SelectedShipLength - 1);
                    break;
                case 2:
                    XPositionShipBack = XPositionShipFront - (SelectedShipLength - 1);
                    YPositionShipBack = YPositionShipFront;
                    break;
                case 3:
                    XPositionShipBack = XPositionShipFront + (SelectedShipLength - 1);
                    YPositionShipBack = YPositionShipFront;
                    break;
            }
        }
        public bool GetValidShipPlacement()
        {
            if (YPositionShipFront > YPositionShipBack && YPositionShipFront - SelectedShipLength < 0)
            {
                return false;
            }
            if (YPositionShipFront < YPositionShipBack && YPositionShipFront + SelectedShipLength > 9)
            {
                return false;
            }
            if (XPositionShipFront > XPositionShipBack && XPositionShipFront - SelectedShipLength < 0)
            {
                return false;
            }
            if (XPositionShipFront < XPositionShipBack && XPositionShipFront + SelectedShipLength > 9)
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
                    if (Map.ShipMap[XPositionShipFront, i] != (int)UserInterface.ShipMapGraphics.Sea)
                    {
                        return false;
                    }
                }
            }
            else if (YPositionShipFront > YPositionShipBack)
            {
                for (int i = YPositionShipFront; i > (YPositionShipFront - SelectedShipLength); i--)
                {
                    if (Map.ShipMap[XPositionShipFront, i] != (int)UserInterface.ShipMapGraphics.Sea)
                    {
                        return false;
                    }
                }
            }
            else if (XPositionShipFront < XPositionShipBack)
            {
                for (int i = XPositionShipFront; i < (XPositionShipFront + (SelectedShipLength)); i++)
                {
                    if (Map.ShipMap[i, YPositionShipFront] != (int)UserInterface.ShipMapGraphics.Sea)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = XPositionShipFront; i > (XPositionShipFront - (SelectedShipLength)); i--)
                {
                    if (Map.ShipMap[i, YPositionShipFront] != (int)UserInterface.ShipMapGraphics.Sea)
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
                            Map.ShipMap[XPositionShipFront, i] = (int)UserInterface.ShipMapGraphics.Destroyer;
                            Map.PlacedDestroyers[DestroyerMax].X[i - YPositionShipFront] = (XPositionShipFront);
                            Map.PlacedDestroyers[DestroyerMax].Y[i - YPositionShipFront] = (i);
                            break;
                        case 3:
                            Map.ShipMap[XPositionShipFront, i] = (int)UserInterface.ShipMapGraphics.Submarine;
                            Map.PlacedSubmarines[SubmarineMax].X[i - YPositionShipFront] = (XPositionShipFront);
                            Map.PlacedSubmarines[SubmarineMax].Y[i - YPositionShipFront] = (i);
                            break;
                        case 4:
                            Map.ShipMap[XPositionShipFront, i] = (int)UserInterface.ShipMapGraphics.Battleship;
                            Map.PlacedBattleships[BattleshipMax].X[i - YPositionShipFront] = (XPositionShipFront);
                            Map.PlacedBattleships[BattleshipMax].Y[i - YPositionShipFront] = (i);
                            break;
                        case 5:
                            Map.ShipMap[XPositionShipFront, i] = (int)UserInterface.ShipMapGraphics.Carrier;
                            Map.PlacedCarriers[CarrierMax].X[i - YPositionShipFront] = (XPositionShipFront);
                            Map.PlacedCarriers[CarrierMax].Y[i - YPositionShipFront] = (i);
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
                            Map.ShipMap[XPositionShipFront, i] = (int)UserInterface.ShipMapGraphics.Destroyer;
                            Map.PlacedDestroyers[DestroyerMax].X[i - YPositionShipFront + 1] = (XPositionShipFront);
                            Map.PlacedDestroyers[DestroyerMax].Y[i - YPositionShipFront + 1] = (i);
                            break;
                        case 3:
                            Map.ShipMap[XPositionShipFront, i] = (int)UserInterface.ShipMapGraphics.Submarine;
                            Map.PlacedSubmarines[SubmarineMax].X[i - YPositionShipFront + 2] = (XPositionShipFront);
                            Map.PlacedSubmarines[SubmarineMax].Y[i - YPositionShipFront + 2] = (i);
                            break;
                        case 4:
                            Map.ShipMap[XPositionShipFront, i] = (int)UserInterface.ShipMapGraphics.Battleship;
                            Map.PlacedBattleships[BattleshipMax].X[i - YPositionShipFront + 3] = (XPositionShipFront);
                            Map.PlacedBattleships[BattleshipMax].Y[i - YPositionShipFront + 3] = (i);
                            break;
                        case 5:
                            Map.ShipMap[XPositionShipFront, i] = (int)UserInterface.ShipMapGraphics.Carrier;
                            Map.PlacedCarriers[CarrierMax].X[i - YPositionShipFront + 4] = (XPositionShipFront);
                            Map.PlacedCarriers[CarrierMax].Y[i - YPositionShipFront + 4] = (i);
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
                            Map.ShipMap[i, YPositionShipFront] = (int)UserInterface.ShipMapGraphics.Destroyer;
                            Map.PlacedDestroyers[DestroyerMax].X[i - XPositionShipFront] = (i);
                            Map.PlacedDestroyers[DestroyerMax].Y[i - XPositionShipFront] = (YPositionShipFront);
                            break;
                        case 3:
                            Map.ShipMap[i, YPositionShipFront] = (int)UserInterface.ShipMapGraphics.Submarine;
                            Map.PlacedSubmarines[SubmarineMax].X[i - XPositionShipFront] = (i);
                            Map.PlacedSubmarines[SubmarineMax].Y[i - XPositionShipFront] = (YPositionShipFront);
                            break;
                        case 4:
                            Map.ShipMap[i, YPositionShipFront] = (int)UserInterface.ShipMapGraphics.Battleship;
                            Map.PlacedBattleships[BattleshipMax].X[i - XPositionShipFront] = (i);
                            Map.PlacedBattleships[BattleshipMax].Y[i - XPositionShipFront] = (YPositionShipFront);
                            break;
                        case 5:
                            Map.ShipMap[i, YPositionShipFront] = (int)UserInterface.ShipMapGraphics.Carrier;
                            Map.PlacedCarriers[CarrierMax].X[i - XPositionShipFront] = (i);
                            Map.PlacedCarriers[CarrierMax].Y[i - XPositionShipFront] = (YPositionShipFront);
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
                            Map.ShipMap[i, YPositionShipFront] = (int)UserInterface.ShipMapGraphics.Destroyer;
                            Map.PlacedDestroyers[DestroyerMax].X[i - XPositionShipFront + 1] = (i);
                            Map.PlacedDestroyers[DestroyerMax].Y[i - XPositionShipFront + 1] = (YPositionShipFront);
                            break;
                        case 3:
                            Map.ShipMap[i, YPositionShipFront] = (int)UserInterface.ShipMapGraphics.Submarine;
                            Map.PlacedSubmarines[SubmarineMax].X[i - XPositionShipFront + 2] = (i);
                            Map.PlacedSubmarines[SubmarineMax].Y[i - XPositionShipFront + 2] = (YPositionShipFront);
                            break;
                        case 4:
                            Map.ShipMap[i, YPositionShipFront] = (int)UserInterface.ShipMapGraphics.Battleship;
                            Map.PlacedBattleships[BattleshipMax].X[i - XPositionShipFront + 3] = (i);
                            Map.PlacedBattleships[BattleshipMax].Y[i - XPositionShipFront + 3] = (YPositionShipFront);
                            break;
                        case 5:
                            Map.ShipMap[i, YPositionShipFront] = (int)UserInterface.ShipMapGraphics.Carrier;
                            Map.PlacedCarriers[CarrierMax].X[i - XPositionShipFront + 4] = (i);
                            Map.PlacedCarriers[CarrierMax].Y[i - XPositionShipFront + 4] = (YPositionShipFront);
                            break;
                    }
                }
            }
            switch (SelectedShipLength)
            {
                case (int)UserInterface.ShipMapGraphics.Destroyer:
                    if (DestroyerMax > 0)
                    {
                        DestroyerMax--;
                    }
                    break;
                case (int)UserInterface.ShipMapGraphics.Submarine:
                    if (SubmarineMax > 0)
                    {
                        SubmarineMax--;
                    }
                    break;
                case (int)UserInterface.ShipMapGraphics.Battleship:
                    if (BattleshipMax > 0)
                    {
                        BattleshipMax--;
                    }
                    break;
                case (int)UserInterface.ShipMapGraphics.Carrier:
                    if (CarrierMax > 0)
                    {
                        CarrierMax--;
                    }
                    break;
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
            Random Random = new Random();
            int fiftyFifty = 0;

            if (ShotCounter == 4)
            {
                HitCounter = 0;
                ShotCounter = 0;
            }

            switch (HitCounter)
            {
                case 0:
                    XPosition = Random.Next(0, 10);
                    YPosition = Random.Next(0, 10);
                    break;
                case 1:
                    fiftyFifty = Random.Next(0, 2);
                    XPosition = (fiftyFifty == 1) ? Random.Next(XPositionsStored[0] - 1, XPositionsStored[0] + 2) : XPositionsStored[0];
                    int direction = 0;
                    do
                    {
                        direction = Random.Next(YPositionsStored[0] - 1, YPositionsStored[0] + 2);
                    } while (direction == YPositionsStored[0]);
                    YPosition = (XPosition == XPositionsStored[0]) ? direction : YPositionsStored[0];
                    break;
                case 2:
                case 3:
                case 4:
                    fiftyFifty = Random.Next(0, 2);
                    if (XPositionsStored[HitCounter - 1] > XPositionsStored[0])
                    {
                        XPosition = (fiftyFifty == 1) ? XPositionsStored[HitCounter - 1] + 1 : XPositionsStored[HitCounter - 1] - HitCounter;
                    }
                    else if (XPositionsStored[HitCounter - 1] < XPositionsStored[0])
                    {
                        XPosition = (fiftyFifty == 1) ? XPositionsStored[HitCounter - 1] - 1 : XPositionsStored[HitCounter - 1] + HitCounter;
                    }
                    else
                    {
                        XPosition = XPositionsStored[HitCounter - 1];
                    }

                    fiftyFifty = Random.Next(0, 2);
                    if (YPositionsStored[HitCounter - 1] > YPositionsStored[0])
                    {
                        YPosition = (fiftyFifty == 1) ? YPositionsStored[HitCounter - 1] + 1 : YPositionsStored[HitCounter - 1] - HitCounter;
                    }
                    else if (YPositionsStored[HitCounter - 1] < YPositionsStored[0])
                    {
                        YPosition = (fiftyFifty == 1) ? YPositionsStored[HitCounter - 1] - 1 : YPositionsStored[HitCounter - 1] + HitCounter;
                    }
                    else
                    {
                        YPosition = YPositionsStored[HitCounter - 1];
                    }
                    break;
            }
            ShotCounter++;
        }
        public bool GetValidShot(int[,] shipMap, int MapPositionX)
        {
            if ((XPosition - MapPositionX) > (shipMap.GetLength(0) - 1) || (XPosition - MapPositionX) < 0 || YPosition > (shipMap.GetLength(0) - 1) || YPosition < 0)
            {
                return false;
            }

            for (int i = 0; i < shipMap.GetLength(0); i++)
            {
                if (i == XPosition - MapPositionX)
                {
                    for (int j = 0; j < shipMap.GetLength(1); j++)
                    {
                        if (j == YPosition)
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
            bool hit = false;
            for (int i = 0; i < shipMap.GetLength(0); i++)
            {
                if (i == XPosition - MapPositionX)
                {
                    for (int j = 0; j < shipMap.GetLength(1); j++)
                    {
                        if (j == YPosition)
                        {
                            if (shipMap[i, j] == (int)UserInterface.ShipMapGraphics.Sea)
                            {
                                concealedShipMap[i, j] = (int)UserInterface.ShipMapGraphics.MissMarker;
                            }
                            else if (shipMap[i, j] == (int)UserInterface.ShipMapGraphics.Destroyer)
                            {
                                concealedShipMap[i, j] = (int)UserInterface.ShipMapGraphics.HitDestroyer;
                                hit = true;
                            }
                            else if (shipMap[i, j] == (int)UserInterface.ShipMapGraphics.Submarine)
                            {
                                concealedShipMap[i, j] = (int)UserInterface.ShipMapGraphics.HitSubmarine;
                                hit = true;
                            }
                            else if (shipMap[i, j] == (int)UserInterface.ShipMapGraphics.Battleship)
                            {
                                concealedShipMap[i, j] = (int)UserInterface.ShipMapGraphics.HitBattleship;
                                hit = true;
                            }
                            else if (shipMap[i, j] == (int)UserInterface.ShipMapGraphics.Carrier)
                            {
                                concealedShipMap[i, j] = (int)UserInterface.ShipMapGraphics.HitCarrier;
                                hit = true;
                            }
                        }
                    }
                }
            }

            if (hit)
            {
                XPositionsStored[HitCounter] = XPosition;
                YPositionsStored[HitCounter] = YPosition;
                HitCounter++;

                switch (HitCounter)
                {
                    case 0:
                    case 1:
                        break;
                    case 2:
                        if (concealedShipMap[XPosition, YPosition] == (int)UserInterface.ShipMapGraphics.HitDestroyer && concealedShipMap[XPositionsStored[1], YPositionsStored[1]] == (int)UserInterface.ShipMapGraphics.HitDestroyer)
                        {
                            HitCounter = 0;
                            XPositionsStored = new int[5];
                            YPositionsStored = new int[5];
                        }
                        else if (concealedShipMap[XPosition, YPosition] != concealedShipMap[XPositionsStored[1], YPositionsStored[1]])
                        {
                            HitCounter--;
                        }
                        break;
                    case 3:
                        if (concealedShipMap[XPosition, YPosition] == (int)UserInterface.ShipMapGraphics.HitSubmarine && concealedShipMap[XPositionsStored[2], YPositionsStored[2]] == (int)UserInterface.ShipMapGraphics.HitSubmarine)
                        {
                            HitCounter = 0;
                            XPositionsStored = new int[5];
                            YPositionsStored = new int[5];
                        }
                        else if (concealedShipMap[XPosition, YPosition] != concealedShipMap[XPositionsStored[2], YPositionsStored[2]])
                        {
                            HitCounter--;
                        }
                        break;
                    case 4:
                        if (concealedShipMap[XPosition, YPosition] == (int)UserInterface.ShipMapGraphics.HitBattleship && concealedShipMap[XPositionsStored[3], YPositionsStored[3]] == (int)UserInterface.ShipMapGraphics.HitBattleship)
                        {
                            HitCounter = 0;
                            XPositionsStored = new int[5];
                            YPositionsStored = new int[5];
                        }
                        else if (concealedShipMap[XPosition, YPosition] != concealedShipMap[XPositionsStored[3], YPositionsStored[3]])
                        {
                            HitCounter--;
                        }
                        break;
                    default:
                        HitCounter = 0;
                        XPositionsStored = new int[5];
                        YPositionsStored = new int[5];
                        break;
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
    }
}
