namespace Battleships

{
    public static class Helpers
    {
        public static void Write(int x, int y, string charachter)
        {
            Console.SetCursorPosition(2 * x + 1, y + 1);
            Console.Write(charachter);

        }
        public static void Write(int x, int y, char wall)
        {
            Console.SetCursorPosition(2 * x + 1, y + 1);
            Console.Write(wall);
            Console.Write(wall);

        }
        public static void MoveCursor(int XCursor, int YCursor, string cursor)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Helpers.Write(XCursor, YCursor, cursor);
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
