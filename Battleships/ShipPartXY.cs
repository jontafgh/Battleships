namespace Battleships

{
    public class ShipPartXY
    {
        public int[] X { get; set; }
        public int[] Y { get; set; }
        public ShipPartXY(int parts)
        {
            X = new int[parts];
            Y = new int[parts];
        }
    }
}
