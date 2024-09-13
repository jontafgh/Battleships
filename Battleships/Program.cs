using System.ComponentModel.Design;
using System.Net.Http.Headers;

namespace Battleships

{
    internal class Program
    {
        static void Main(string[] args)
        {
            //TODO
            //Logic if there is ships that are not completly shot down
            //Make GameAi inherit from GameEngine


            //
            //Start
            //

            Player engine = new Player();
            GameGraphics playerGraphics = new GameGraphics();
            GameGraphics aiGraphic = new GameGraphics();
            GameAI ai = new GameAI();

            ConsoleKeyInfo consoleKey;
            Console.SetWindowSize(120, 40);
            Console.SetBufferSize(120, 40);

            Console.Clear();
            Console.CursorVisible = false;

            //TODO add to constructor
            playerGraphics.MapPositionX = 0;
            playerGraphics.MapPositionY = 0;
            playerGraphics.MapCheckOffsetConsideration = -1;
            //
            //TODO add to constructor
            aiGraphic.MapPositionX = 15;
            aiGraphic.MapPositionY = 0;
            aiGraphic.MapCheckOffsetConsideration = 0;
            //

            playerGraphics.mapBorders = playerGraphics.GenerateMapBorders();
            playerGraphics.DrawMapBorders();

            playerGraphics.shipMap = playerGraphics.GenerateShipMap();
            playerGraphics.DrawShipMap();

            playerGraphics.shipSelection = playerGraphics.GenerateShipSelection(engine.CarrierMax, engine.BattleshipMax, engine.SubmarineMax, engine.DestroyerMax);
            playerGraphics.DrawShipSelection();

            Console.SetCursorPosition(engine.XCursor, engine.YCursor);
            Helpers.MoveCursor(engine.XCursor, engine.YCursor);

            //
            //Ship placement phase
            //

            do
            {
                consoleKey = Console.ReadKey(true);
                Helpers.MoveCursor(engine.XCursor, engine.YCursor, "  ");

                playerGraphics.DrawShipMap();
                playerGraphics.DrawShipSelection();

                engine.StoreCursorPosition();

                engine.SpacebarPressed = engine.GetKeyPress(consoleKey);
                engine.GetShipPlacement(playerGraphics.shipMap);
                playerGraphics.shipSelection = playerGraphics.GenerateShipSelection(engine.CarrierMax, engine.BattleshipMax, engine.SubmarineMax, engine.DestroyerMax);

                playerGraphics.DrawShipSelection();
                playerGraphics.DrawShipMap();
                playerGraphics.DrawShipPlacementFeedback(engine.ShipPlacementSelected, engine.ShipFrontPlaced);

                engine.GetEdgeOfMapDetection(playerGraphics.mapBorders, playerGraphics.MapPositionX);
                Helpers.MoveCursor(engine.XCursor, engine.YCursor);

            } while (!engine.GetAllShipsPlaced());

            //
            //AI generates map
            //           

            aiGraphic.shipMap = aiGraphic.GenerateShipMap();
            do
            {
                do
                {
                    ai.SelectShip();
                    ai.PlaceShip();

                } while (!ai.GetValidShipPlacement(aiGraphic.shipMap, aiGraphic.MapCheckOffsetConsideration));

                aiGraphic.shipMap = ai.PlaceShip(aiGraphic.shipMap, aiGraphic.MapCheckOffsetConsideration);

                ai.DecreaseShipsLeftToPlace();

            } while (!ai.GetAllShipsPlaced());

            //
            //Main game ui initialization
            //

            Console.Clear();

            aiGraphic.mapBorders = aiGraphic.GenerateMapBorders();
            aiGraphic.concealedShipMap = aiGraphic.GenerateShipMap();
            playerGraphics.concealedShipMap = playerGraphics.GenerateShipMap();

            playerGraphics.DrawMapBorders();
            aiGraphic.DrawMapBorders();
            playerGraphics.DrawShipMap(playerGraphics.concealedShipMap);
            aiGraphic.DrawShipMap(aiGraphic.concealedShipMap);

            engine.XCursor = aiGraphic.MapPositionX + 2;
            engine.YCursor = aiGraphic.MapPositionY + 2;
            Console.SetCursorPosition(aiGraphic.MapPositionX + 2, aiGraphic.MapPositionY + 2);
            Helpers.MoveCursor(engine.XCursor, engine.YCursor, GameGraphics.cursor);

            //
            //Main game phase
            //

            do
            {
                //
                //Player Turn
                //

                do
                {
                    consoleKey = Console.ReadKey(true);

                    Helpers.MoveCursor(engine.XCursor, engine.YCursor, "  ");

                    playerGraphics.DrawShipMap(playerGraphics.concealedShipMap);
                    aiGraphic.DrawShipMap(aiGraphic.concealedShipMap);

                    engine.StoreCursorPosition();

                    if (engine.GetKeyPress(consoleKey))
                    {
                        if (engine.GetValidShot(aiGraphic.concealedShipMap, aiGraphic.MapPositionX))
                        {
                            aiGraphic.MarkShotOnMap(engine.XCursor, engine.YCursor, aiGraphic.MapPositionX, playerGraphics.MapCheckOffsetConsideration);
                            engine.PlayerDoneShooting = true;
                        }
                    }
                    engine.GetEdgeOfMapDetection(playerGraphics.mapBorders, aiGraphic.MapPositionX);
                    Helpers.MoveCursor(engine.XCursor, engine.YCursor);

                } while (!engine.PlayerDoneShooting);

                engine.PlayerDoneShooting = false;

                //
                //Ai Turn
                //

                do
                {
                    ai.GetShot();                    
                } while (!ai.GetValidShot(playerGraphics.concealedShipMap, aiGraphic.MapCheckOffsetConsideration, playerGraphics.MapPositionX));
                ai.shotCounter = 0;

                playerGraphics.MarkShotOnMap(ai.ShotX, ai.ShotY, playerGraphics.MapPositionX, aiGraphic.MapCheckOffsetConsideration);

                if (ai.GetShotHitOrNot(playerGraphics.shipMap))
                {                    
                    ai.StoreShotHits();                    
                    ai.CheckForTypeOfShipHit(playerGraphics.concealedShipMap);
                }

                playerGraphics.DrawShipMap(playerGraphics.concealedShipMap);

                //
                //Win detection
                //

                engine.Win = engine.GetWin(aiGraphic.concealedShipMap);

            } while (!engine.Win);

            Console.WriteLine("You won!");
            Console.ReadKey();
        }
    }
}
