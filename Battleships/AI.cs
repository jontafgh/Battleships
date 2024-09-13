using System.Xml.Linq;

namespace Battleships

{
    public class AI : IShipCaptain
    {
        public Random Random = new Random();
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int XPositionShipFront { get; set; }
        public int YPositionShipFront { get; set; }
        public int XPositionShipBack { get; set; }
        public int YPositionShipBack { get; set; }
        public int DestroyerMax { get; set; }
        public int SubmarineMax { get; set; }
        public int BattleshipMax { get; set; }
        public int CarrierMax { get; set; }
        public int SelectedShipLength { get; set; }


       
        public int[] StoredHitsX { get; set; }
        public int[] StoredHitsY { get; set; }
        public int ShotCounter { get; set; }
        public int HitCounter { get; set; }

        public AI()
        {
            DestroyerMax = 4;
            SubmarineMax = 3;
            BattleshipMax = 2;
            CarrierMax = 1;
            SelectedShipLength = 0;            
            StoredHitsX = new int[5];
            StoredHitsY = new int[5];
            ShotCounter = 0;
            HitCounter = 0;
        }
        public void SelectShip()
        {
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
        public void GetShipPlacement(string[,] shipMap)
        {
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
        public bool GetValidShipPlacement(string[,] shipMap)
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
                    if (shipMap[XPositionShipFront, i] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else if (YPositionShipFront > YPositionShipBack)
            {
                for (int i = YPositionShipFront; i > (YPositionShipFront - SelectedShipLength); i--)
                {
                    if (shipMap[XPositionShipFront, i] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else if (XPositionShipFront < XPositionShipBack)
            {
                for (int i = XPositionShipFront; i < (XPositionShipFront + (SelectedShipLength)); i++)
                {
                    if (shipMap[i, YPositionShipFront] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = XPositionShipFront; i > (XPositionShipFront - (SelectedShipLength)); i--)
                {
                    if (shipMap[i, YPositionShipFront] != GameGraphics.sea)
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
                            shipMap[XPositionShipFront, i] = GameGraphics.destroyer;
                            break;
                        case 3:
                            shipMap[XPositionShipFront, i] = GameGraphics.submarine;
                            break;
                        case 4:
                            shipMap[XPositionShipFront, i] = GameGraphics.battleship;
                            break;
                        case 5:
                            shipMap[XPositionShipFront, i] = GameGraphics.carrier;
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
                            shipMap[XPositionShipFront, i] = GameGraphics.destroyer;
                            break;
                        case 3:
                            shipMap[XPositionShipFront, i] = GameGraphics.submarine;
                            break;
                        case 4:
                            shipMap[XPositionShipFront, i] = GameGraphics.battleship;
                            break;
                        case 5:
                            shipMap[XPositionShipFront, i] = GameGraphics.carrier;
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
                            shipMap[i, YPositionShipFront] = GameGraphics.destroyer;
                            break;
                        case 3:
                            shipMap[i, YPositionShipFront] = GameGraphics.submarine;
                            break;
                        case 4:
                            shipMap[i, YPositionShipFront] = GameGraphics.battleship;
                            break;
                        case 5:
                            shipMap[i, YPositionShipFront] = GameGraphics.carrier;
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
                            shipMap[i, YPositionShipFront] = GameGraphics.destroyer;
                            break;
                        case 3:
                            shipMap[i, YPositionShipFront] = GameGraphics.submarine;
                            break;
                        case 4:
                            shipMap[i, YPositionShipFront] = GameGraphics.battleship;
                            break;
                        case 5:
                            shipMap[i, YPositionShipFront] = GameGraphics.carrier;
                            break;
                    }
                }
            }
            switch (SelectedShipLength)
            {
                case (int)IShipCaptain.Ship.Destroyer:
                    if (DestroyerMax > 0)
                    {
                        DestroyerMax--;
                    }
                    break;
                case (int)IShipCaptain.Ship.Submarine:
                    if (SubmarineMax > 0)
                    {
                        SubmarineMax--;
                    }
                    break;
                case (int)IShipCaptain.Ship.Battleship:
                    if (BattleshipMax > 0)
                    {
                        BattleshipMax--;
                    }
                    break;
                case (int)IShipCaptain.Ship.Carrier:
                    if (CarrierMax > 0)
                    {
                        CarrierMax--;
                    }
                    break;
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
                    XPosition = (fiftyFifty == 1) ? Random.Next(StoredHitsX[0] - 1, StoredHitsX[0] + 2) : StoredHitsX[0];
                    int direction = 0;
                    do
                    {
                        direction = Random.Next(StoredHitsY[0] - 1, StoredHitsY[0] + 2);
                    } while (direction == StoredHitsY[0]);
                    YPosition = (XPosition == StoredHitsX[0]) ? direction : StoredHitsY[0];
                    break;
                case 2:
                case 3:
                case 4:
                    fiftyFifty = Random.Next(0, 2);
                    if (StoredHitsX[HitCounter - 1] > StoredHitsX[0])
                    {
                        XPosition = (fiftyFifty == 1) ? StoredHitsX[HitCounter - 1] + 1 : StoredHitsX[HitCounter - 1] - HitCounter;
                    }
                    else if (StoredHitsX[HitCounter - 1] < StoredHitsX[0])
                    {
                        XPosition = (fiftyFifty == 1) ? StoredHitsX[HitCounter - 1] - 1 : StoredHitsX[HitCounter - 1] + HitCounter;
                    }
                    else
                    {
                        XPosition = StoredHitsX[HitCounter - 1];
                    }

                    fiftyFifty = Random.Next(0, 2);
                    if (StoredHitsY[HitCounter - 1] > StoredHitsY[0])
                    {
                        YPosition = (fiftyFifty == 1) ? StoredHitsY[HitCounter - 1] + 1 : StoredHitsY[HitCounter - 1] - HitCounter;
                    }
                    else if (StoredHitsY[HitCounter - 1] < StoredHitsY[0])
                    {
                        XPosition = (fiftyFifty == 1) ? StoredHitsY[HitCounter - 1] - 1 : StoredHitsY[HitCounter - 1] + HitCounter;
                    }
                    else
                    {
                        XPosition = StoredHitsY[HitCounter - 1];
                    }
                    break;
            }
            ShotCounter++;
        }
        public bool GetValidShot(string[,] shipMap, int MapPositionX)
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
        public string[,] Shoot(string[,] shipMap, string[,] concealedShipMap, int MapPositionX)
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
                            if (shipMap[i, j] == GameGraphics.sea)
                            {
                                concealedShipMap[i, j] = GameGraphics.MissMarker;
                            }
                            else if (shipMap[i, j] == GameGraphics.destroyer)
                            {
                                concealedShipMap[i, j] = GameGraphics.hitDestroyer;
                                hit = true;
                            }
                            else if (shipMap[i, j] == GameGraphics.submarine)
                            {
                                concealedShipMap[i, j] = GameGraphics.hitSubmarine;
                                hit = true;
                            }
                            else if (shipMap[i, j] == GameGraphics.battleship)
                            {
                                concealedShipMap[i, j] = GameGraphics.hitBattleship;
                                hit = true;
                            }
                            else if (shipMap[i, j] == GameGraphics.carrier)
                            {
                                concealedShipMap[i, j] = GameGraphics.hitCarrier;
                                hit = true;
                            }
                        }
                    }
                }
            }

            if (hit)
            {
                StoredHitsX[HitCounter] = XPosition;
                StoredHitsY[HitCounter] = YPosition;
                HitCounter++;

                switch (HitCounter)
                {
                    case 0:
                    case 1:
                        break;
                    case 2:
                        if (concealedShipMap[XPosition, YPosition] == GameGraphics.hitDestroyer && concealedShipMap[StoredHitsX[1], StoredHitsY[1]] == GameGraphics.hitDestroyer)
                        {
                            HitCounter = 0;
                            StoredHitsX = new int[5];
                            StoredHitsY = new int[5];
                        }
                        else if (concealedShipMap[XPosition, YPosition] != concealedShipMap[StoredHitsX[1], StoredHitsY[1]])
                        {
                            HitCounter--;
                        }
                        break;
                    case 3:
                        if (concealedShipMap[XPosition, YPosition] == GameGraphics.hitSubmarine && concealedShipMap[StoredHitsX[2], StoredHitsY[2]] == GameGraphics.hitSubmarine)
                        {
                            HitCounter = 0;
                            StoredHitsX = new int[5];
                            StoredHitsY = new int[5];
                        }
                        else if (concealedShipMap[XPosition, YPosition] != concealedShipMap[StoredHitsX[2], StoredHitsY[2]])
                        {
                            HitCounter--;
                        }
                        break;
                    case 4:
                        if (concealedShipMap[XPosition, YPosition] == GameGraphics.hitBattleship && concealedShipMap[StoredHitsX[3], StoredHitsY[3]] == GameGraphics.hitBattleship)
                        {
                            HitCounter = 0;
                            StoredHitsX = new int[5];
                            StoredHitsY = new int[5];
                        }
                        else if (concealedShipMap[XPosition, YPosition] != concealedShipMap[StoredHitsX[3], StoredHitsY[3]])
                        {
                            HitCounter--;
                        }
                        break;
                    default:
                        HitCounter = 0;
                        StoredHitsX = new int[5];
                        StoredHitsY = new int[5];
                        break;
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
    }
}
