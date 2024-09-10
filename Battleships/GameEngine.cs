namespace Battleships

{
    public class GameEngine
    {
        public int XCursor = 5;
        public int YCursor = 5;
        public int XCursorStored = 5;
        public int YCursorStored = 5;
        public bool shipFrontPlaced = false;
        public bool shipPlacementSelected = false;
        public bool allShipsPlaced = false;
        public bool ValidShot = false;
        public bool Win = false;
        public bool Hit = false;
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
        public bool GetValidShipPlacement(string[,] shipMap, int XPositionShipFront, int YPositionShipFront, int XPositionShipBack, int YPositionShipBack, int shipSize)
        {
            if (YPositionShipFront > YPositionShipBack && YPositionShipFront - shipSize < 0)
            {
                return false;
            }
            if (YPositionShipFront < YPositionShipBack && YPositionShipFront + shipSize > 11)
            {
                return false;
            }
            if (XPositionShipFront > XPositionShipBack && XPositionShipFront - shipSize < 0)
            {
                return false;
            }
            if (XPositionShipFront < XPositionShipBack && XPositionShipFront + shipSize > 11)
            {
                return false;
            }
            if (XPositionShipFront == XPositionShipBack && YPositionShipFront == YPositionShipBack)
            {
                return false;
            }
            if (YPositionShipFront < YPositionShipBack)
            {
                for (int i = YPositionShipFront; i < (YPositionShipFront + shipSize); i++)
                {
                    if (shipMap[XPositionShipFront - 1, i - 1] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else if (YPositionShipFront > YPositionShipBack)
            {
                for (int i = YPositionShipFront; i > (YPositionShipFront - shipSize); i--)
                {
                    if (shipMap[XPositionShipFront - 1, i - 1] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else if (XPositionShipFront < XPositionShipBack)
            {
                for (int i = XPositionShipFront; i < (XPositionShipFront + (shipSize)); i++)
                {
                    if (shipMap[i - 1, YPositionShipFront - 1] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = XPositionShipFront; i > (XPositionShipFront - (shipSize)); i--)
                {
                    if (shipMap[i - 1, YPositionShipFront - 1] != GameGraphics.sea)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool GetAllShipsPlaced(int carrierMax, int battleshipMax, int submarineMax, int destroyerMax)
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
        public bool GetValidShot(string[,] shipMap, int x, int y)
        {
            if (x - 1 > 9 || x - 1 < 0 || y - 1 > 9 || y - 1 < 0)
            {
                return false;
            }

            for (int i = 0; i < shipMap.GetLength(0); i++)
            {
                if (i == x - 1)
                {
                    for (int j = 0; j < shipMap.GetLength(1); j++)
                    {
                        if (j == y - 1)
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
        public bool GetHitShot(string[,] shipMap, int x, int y)
        {
            for (int i = 0; i < shipMap.GetLength(0); i++)
            {
                if (i == x - 1)
                {
                    for (int j = 0; j < shipMap.GetLength(1); j++)
                    {
                        if (j == y - 1)
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
