using System.ComponentModel.Design;
using System.Net.Http.Headers;

namespace Battleships

{
    internal class Program
    {
        static void Main(string[] args)
        {
            //TODO Ai and player map borders
            //
            //Start
            //

            GameEngine engine = new GameEngine();
            GameGraphics playerGraphics = new GameGraphics();
            GameAI ai = new GameAI();

            ConsoleKeyInfo consoleKey;
            Console.SetWindowSize(120, 40);
            Console.SetBufferSize(120, 40);

            Console.Clear();
            Console.CursorVisible = false;

            //Initial Graphics state

            //TODO add to constructor
            playerGraphics.MapPositionX = 0;
            playerGraphics.MapPositionY = 0;
            playerGraphics.MapCheckOffsetConsideration = - 1;
            //

            playerGraphics.mapBorders = playerGraphics.GenerateMapBorders();
            playerGraphics.DrawMapBorders();

            playerGraphics.shipMap = playerGraphics.GenerateShipMap();
            playerGraphics.DrawShipMap();

            playerGraphics.shipSelection = playerGraphics.GenerateShipSelection(engine.carrierMax, engine.battleshipMax, engine.submarineMax, engine.destroyerMax);
            playerGraphics.DrawShipSelection();

            Console.SetCursorPosition(engine.XCursor, engine.YCursor);
            Helpers.MoveCursor(engine.XCursor, engine.YCursor);

            //
            //Ship placement phase
            //

            do
            {
                consoleKey = Console.ReadKey(true);

                //Clear old cursor                
                Helpers.MoveCursor(engine.XCursor, engine.YCursor, "  ");

                //Draw Ships
                playerGraphics.DrawShipMap();

                //Draw Ship Selection playerGraphics
                playerGraphics.DrawShipSelection();

                //Save cursor position in case of edge of map
                engine.StoreCursorPosition();

                //Read key press
                engine.SpacebarPressed = engine.GetKeyPress(consoleKey);

                //Placement of ships
                engine.GetShipPlacement(playerGraphics.shipMap, playerGraphics.MapCheckOffsetConsideration);                
                playerGraphics.shipSelection = playerGraphics.GenerateShipSelection(engine.carrierMax, engine.battleshipMax, engine.submarineMax, engine.destroyerMax);
                playerGraphics.DrawShipSelection();
                playerGraphics.DrawShipMap();

                //Placement of ships feedback                
                playerGraphics.DrawShipPlacementFeedback(engine.shipPlacementSelected, engine.shipFrontPlaced);

                //Edge detection
                engine.GetEdgeOfMapDetection(playerGraphics.mapBorders, playerGraphics.MapPositionX);
                
                //Move Cursor                
                Helpers.MoveCursor(engine.XCursor, engine.YCursor);

            } while (!engine.GetAllShipsPlaced());
            
            //
            //AI Generates map
            //            

            GameGraphics aiGraphic = new GameGraphics();

            //TODO add to constructor
            aiGraphic.MapPositionX = 15;
            aiGraphic.MapPositionY = 0;
            aiGraphic.MapCheckOffsetConsideration = 0;
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

                    //Clear old cursor                
                    Helpers.MoveCursor(engine.XCursor, engine.YCursor, "  ");

                    //Draw Ships
                    playerGraphics.DrawShipMap(playerGraphics.concealedShipMap);
                    aiGraphic.DrawShipMap(aiGraphic.concealedShipMap);

                    //Save cursor position in case of edge of map
                    engine.StoreCursorPosition();
                    
                    //engine.SpacebarPressed = engine.GetKeyPress(consoleKey);

                    //Read key press
                    //Shooting enemy ships logic                    
                    if (engine.GetKeyPress(consoleKey))
                    {
                        
                        if (engine.GetValidShot(aiGraphic.concealedShipMap, playerGraphics.MapCheckOffsetConsideration, aiGraphic.MapPositionX))
                        {
                            aiGraphic.concealedShipMap = aiGraphic.ShootShip(aiGraphic.shipMap, aiGraphic.concealedShipMap, engine.XCursor - aiGraphic.MapPositionX, engine.YCursor);
                            engine.PlayerDoneShooting = true;
                        }
                        
                    }

                    //Edge detection
                    engine.GetEdgeOfMapDetection(playerGraphics.mapBorders, aiGraphic.MapPositionX);

                    //Move Cursor                
                    Helpers.MoveCursor(engine.XCursor, engine.YCursor, GameGraphics.cursor);
                } while (!engine.PlayerDoneShooting);
                engine.PlayerDoneShooting = false;

                //
                //Ai Turn
                //

                do
                {
                    //Check for valid shots so to not get stuck in a loop
                    if (ai.shotCounter == 4)
                    {
                        ai.hitCounter = 0;
                        ai.shotCounter = 0;
                    }

                    switch (ai.hitCounter)
                    {
                        case 0:
                            //TODO
                            //Logic if there is ships that are not completly shot down

                            //Get coordinates
                            ai.ShotX = ai.GetShotOrShipPlacement();
                            ai.ShotY = ai.GetShotOrShipPlacement();

                            break;
                        case 1:

                            //Get coordinates with previous hit in mind
                            ai.ShotX = ai.GetShotAfterOneHit(ai.StoredHitsX[0]);
                            ai.ShotY = ai.GetShotAfterOneHit(ai.StoredHitsY[0], ai.ShotX, ai.StoredHitsX[0]);

                            break;
                        case 2:
                        case 3:
                        case 4:

                            ai.ShotX = ai.GetShotAfterTwoOrMoreHits(ai.StoredHitsX, ai.StoredHitsX[ai.hitCounter - 1], ai.hitCounter);
                            ai.ShotY = ai.GetShotAfterTwoOrMoreHits(ai.StoredHitsY, ai.StoredHitsY[ai.hitCounter - 1], ai.hitCounter);

                            break;


                    }

                    //Get if coordinates is valid
                    engine.ValidShot = ai.GetValidShot(playerGraphics.concealedShipMap, aiGraphic.MapCheckOffsetConsideration, playerGraphics.MapPositionX);

                    //Store shots for check if theres no valid shots left    
                    ai.shotCounter++;

                } while (!engine.ValidShot);

                //Update map
                playerGraphics.concealedShipMap = aiGraphic.ShootShip(playerGraphics.shipMap, playerGraphics.concealedShipMap, ai.ShotX + 1, ai.ShotY + 1);

                //Reset valid checker
                ai.shotCounter = 0;

                //Get if hit
                ai.Hit = ai.GetHitShot(playerGraphics.shipMap);

                //Hit logic
                if (ai.Hit)
                {
                    //Save hits
                    ai.StoredHitsX[ai.hitCounter] = ai.ShotX;
                    ai.StoredHitsY[ai.hitCounter] = ai.ShotY;
                    ai.hitCounter++;

                    //Check for type of ship
                    switch (ai.hitCounter)
                    {
                        case 0:
                        case 1:
                            break;
                        case 2:
                            if (playerGraphics.concealedShipMap[ai.ShotX, ai.ShotY] == GameGraphics.hitDestroyer && playerGraphics.concealedShipMap[ai.StoredHitsX[1], ai.StoredHitsY[1]] == GameGraphics.hitDestroyer)
                            {
                                ai.hitCounter = 0;
                                ai.StoredHitsX = new int[5];
                                ai.StoredHitsY = new int[5];
                            }
                            else if (playerGraphics.concealedShipMap[ai.ShotX, ai.ShotY] != playerGraphics.concealedShipMap[ai.StoredHitsX[1], ai.StoredHitsY[1]])
                            {
                                ai.hitCounter--;
                            }
                            break;
                        case 3:
                            if (playerGraphics.concealedShipMap[ai.ShotX, ai.ShotY] == GameGraphics.hitSubmarine && playerGraphics.concealedShipMap[ai.StoredHitsX[2], ai.StoredHitsY[2]] == GameGraphics.hitSubmarine)
                            {
                                ai.hitCounter = 0;
                                ai.StoredHitsX = new int[5];
                                ai.StoredHitsY = new int[5];
                            }
                            else if (playerGraphics.concealedShipMap[ai.ShotX, ai.ShotY] != playerGraphics.concealedShipMap[ai.StoredHitsX[2], ai.StoredHitsY[2]])
                            {
                                ai.hitCounter--;
                            }
                            break;
                        case 4:
                            if (playerGraphics.concealedShipMap[ai.ShotX, ai.ShotY] == GameGraphics.hitBattleship && playerGraphics.concealedShipMap[ai.StoredHitsX[3], ai.StoredHitsY[3]] == GameGraphics.hitBattleship)
                            {
                                ai.hitCounter = 0;
                                ai.StoredHitsX = new int[5];
                                ai.StoredHitsY = new int[5];
                            }
                            else if (playerGraphics.concealedShipMap[ai.ShotX, ai.ShotY] != playerGraphics.concealedShipMap[ai.StoredHitsX[3], ai.StoredHitsY[3]])
                            {
                                ai.hitCounter--;
                            }
                            break;
                        default:
                            ai.hitCounter = 0;
                            ai.StoredHitsX = new int[5];
                            ai.StoredHitsY = new int[5];
                            break;
                    }
                }

                playerGraphics.DrawShipMap(playerGraphics.concealedShipMap);

                //
                //End of ai turn
                //



                //Win detection
                engine.Win = engine.GetWin(aiGraphic.concealedShipMap);

            } while (!engine.Win);

            Console.WriteLine("You won!");
            Console.ReadKey();
        }
    }
}
