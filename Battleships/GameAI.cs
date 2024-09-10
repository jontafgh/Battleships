namespace Battleships

{
    public class GameAI
    {
        public Random Random = new Random();
        public int AiBattleshipMax = 2;
        public int AiSubmarineMax = 3;
        public int AiDestroyerMax = 4;
        public int AiCarrierMax = 1;
        public int ShipDirection;
        public bool ValidShipSelection = false;
        public bool ValidPlacement = false;
        public int ShotX;
        public int ShotY;
        public int[] StoredHitsX = new int[5];
        public int[] StoredHitsY = new int[5];
        public int[] StoredShotsX = new int[5];
        public int[] StoredShotsY = new int[5];
        public int shotCounter = 0;
        public int hitCounter = 0;
        public bool noValidShots = false;
        public int selectedShipLength = 0;
        public int XPositionShipFront = 0;
        public int YPositionShipFront = 0;
        public int XPositionShipBack = 0;
        public int YPositionShipBack = 0;
        public bool Hit = false;

        public int GetShipSelection()
        {
            return Random.Next(2, 6);
        }
        public void SelectShip()
        {

            do
            {
                selectedShipLength = GetShipSelection();

                switch (selectedShipLength)
                {
                    case 2:
                        if (AiDestroyerMax == 0)
                        {
                            ValidShipSelection = false;
                        }
                        else
                        {
                            ValidShipSelection = true;
                        }
                        break;
                    case 3:
                        if (AiSubmarineMax == 0)
                        {
                            ValidShipSelection = false;
                        }
                        else
                        {
                            ValidShipSelection = true;
                        }
                        break;
                    case 4:
                        if (AiBattleshipMax == 0)
                        {
                            ValidShipSelection = false;
                        }
                        else
                        {
                            ValidShipSelection = true;
                        }
                        break;
                    case 5:
                        if (AiCarrierMax == 0)
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
        public int GetShotOrShipPlacement()
        {
            return Random.Next(0, 10);
        }
        public int GetShipDirection()
        {
            return Random.Next(0, 4);
        }
        public int GetShipBackPlacementX()
        {
            switch (ShipDirection)
            {
                case 0:
                    return XPositionShipFront;
                case 1:
                    return XPositionShipFront;
                case 2:
                    return XPositionShipFront - (selectedShipLength - 1);
                case 3:
                    return XPositionShipFront + (selectedShipLength - 1);
            }
            return XPositionShipFront;
        }
        public int GetShipBackPlacementY()
        {
            switch (ShipDirection)
            {
                case 0:
                    return YPositionShipFront - (selectedShipLength - 1);
                case 1:
                    return YPositionShipFront + (selectedShipLength - 1);
                case 2:
                    return YPositionShipFront;
                case 3:
                    return YPositionShipFront;
            }
            return YPositionShipFront;
        }
        public void PlaceShip()
        {
            XPositionShipFront = GetShotOrShipPlacement();
            YPositionShipFront = GetShotOrShipPlacement();

            ShipDirection = GetShipDirection();

            XPositionShipBack = GetShipBackPlacementX();
            YPositionShipBack = GetShipBackPlacementY();
        }
        public bool GetValidCoordinates(int XPositionShipFront, int YPositionShipFront, int XPositionShipBack, int YPositionShipBack, int shipSize)
        {
            if (YPositionShipFront > YPositionShipBack && YPositionShipBack - shipSize < 0)
            {
                return false;
            }
            if (YPositionShipFront < YPositionShipBack && YPositionShipBack + shipSize > 9)
            {
                return false;
            }
            if (XPositionShipFront > XPositionShipBack && XPositionShipBack - shipSize < 0)
            {
                return false;
            }
            if (XPositionShipFront < XPositionShipBack && XPositionShipBack + shipSize > 9)
            {
                return false;
            }
            return true;
        }
        public int GetShotAfterOneHit(int axis)
        {
            int fiftyFifty = Random.Next(0, 2);
            return (fiftyFifty == 1) ? Random.Next(axis - 1, axis + 2) : axis;
        }
        public int GetShotAfterOneHit(int axis, int previousAxis, int savedAxis)
        {
            int direction = 0;
            do
            {
                direction = Random.Next(axis - 1, axis + 2);
            } while (direction == axis);
            return (previousAxis == savedAxis) ? direction : axis;
        }
        public int GetShotAfterTwoOrMoreHits(int[] storedHits, int axis, int hitCounter)
        {
            int fiftyFifty = Random.Next(0, 2);

            if (storedHits[hitCounter - 1] > storedHits[0])
            {
                return (fiftyFifty == 1) ? axis + 1 : axis - hitCounter;
            }
            else if (storedHits[hitCounter - 1] < storedHits[0])
            {
                return (fiftyFifty == 1) ? axis - 1 : axis + hitCounter;
            }
            else
            {
                return axis;
            }

        }
        public bool GetAllShipsPlaced()
        {
            if (AiCarrierMax == 0 && AiBattleshipMax == 0 && AiSubmarineMax == 0 && AiDestroyerMax == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void DecreaseShipsLeftToPlace()
        {
            switch (selectedShipLength)
            {
                case 2:
                    if (AiDestroyerMax > 0)
                    {
                        AiDestroyerMax--;
                    }
                    break;
                case 3:
                    if (AiSubmarineMax > 0)
                    {
                        AiSubmarineMax--;
                    }
                    break;
                case 4:
                    if (AiBattleshipMax > 0)
                    {
                        AiBattleshipMax--;
                    }
                    break;
                case 5:
                    if (AiCarrierMax > 0)
                    {
                        AiCarrierMax--;
                    }
                    break;
            }
        }
        public bool GetValidShipPlacement(string[,] shipMap, int MapCheckOffsetConsideration)
        {
            if (YPositionShipFront > YPositionShipBack && YPositionShipFront - selectedShipLength < 0)
            {
                return false;
            }
            if (YPositionShipFront < YPositionShipBack && YPositionShipFront + selectedShipLength > 9)
            {
                return false;
            }
            if (XPositionShipFront > XPositionShipBack && XPositionShipFront - selectedShipLength < 0)
            {
                return false;
            }
            if (XPositionShipFront < XPositionShipBack && XPositionShipFront + selectedShipLength > 9)
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
                    if (shipMap[XPositionShipFront + MapCheckOffsetConsideration, i + MapCheckOffsetConsideration] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else if (YPositionShipFront > YPositionShipBack)
            {
                for (int i = YPositionShipFront; i > (YPositionShipFront - selectedShipLength); i--)
                {
                    if (shipMap[XPositionShipFront + MapCheckOffsetConsideration, i + MapCheckOffsetConsideration] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else if (XPositionShipFront < XPositionShipBack)
            {
                for (int i = XPositionShipFront; i < (XPositionShipFront + (selectedShipLength)); i++)
                {
                    if (shipMap[i + MapCheckOffsetConsideration, YPositionShipFront + MapCheckOffsetConsideration] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = XPositionShipFront; i > (XPositionShipFront - (selectedShipLength)); i--)
                {
                    if (shipMap[i + MapCheckOffsetConsideration, YPositionShipFront + MapCheckOffsetConsideration] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            return true;
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
        public bool GetValidShot(string[,] shipMap, int MapCheckOffsetConsideration, int MapPositionX)
        {
            if ((ShotX - MapPositionX - MapCheckOffsetConsideration) > (shipMap.GetLength(0) - 1) || (ShotX - MapPositionX - MapCheckOffsetConsideration) < 0 || ShotY - MapCheckOffsetConsideration > (shipMap.GetLength(0) - 1) || ShotY - MapCheckOffsetConsideration < 0)
            {
                return false;
            }

            for (int i = 0; i < shipMap.GetLength(0); i++)
            {
                if (i == ShotX - MapPositionX - MapCheckOffsetConsideration)
                {
                    for (int j = 0; j < shipMap.GetLength(1); j++)
                    {
                        if (j == ShotY - MapCheckOffsetConsideration)
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
        public bool GetHitShot(string[,] shipMap)
        {
            for (int i = 0; i < shipMap.GetLength(0); i++)
            {
                if (i == ShotX)
                {
                    for (int j = 0; j < shipMap.GetLength(1); j++)
                    {
                        if (j == ShotY)
                        {
                            if (shipMap[i, j] == GameGraphics.carrier || shipMap[i, j] == GameGraphics.battleship || shipMap[i, j] == GameGraphics.submarine || shipMap[i, j] == GameGraphics.destroyer)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
