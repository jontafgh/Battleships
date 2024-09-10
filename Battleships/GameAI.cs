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
        public int GetShipSelection()
        {
            return Random.Next(2, 6);
        }
        public int GetShotOrShipPlacement()
        {
            return Random.Next(0, 10);
        }
        public int GetShipDirection()
        {
            return Random.Next(0, 4);
        }
        public int GetShipBackPlacementX(int shipDirection, int shipLength, int x, int y)
        {
            switch (shipDirection)
            {
                case 0:
                    return x;
                case 1:
                    return x;
                case 2:
                    return x - (shipLength - 1);
                case 3:
                    return x + (shipLength - 1);
            }
            return x;
        }
        public int GetShipBackPlacementY(int shipDirection, int shipLength, int x, int y)
        {
            switch (shipDirection)
            {
                case 0:
                    return y - (shipLength - 1);
                case 1:
                    return y + (shipLength - 1);
                case 2:
                    return y;
                case 3:
                    return y;
            }
            return y;
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

    }
}
