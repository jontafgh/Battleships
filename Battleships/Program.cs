using System.ComponentModel.Design;
using System.Net.Http.Headers;

namespace Battleships

{
    internal class Program
    {
        static void Main(string[] args)
        {
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

            //Initial playerGraphics state
            playerGraphics.mapBorders = playerGraphics.GenerateMap();
            playerGraphics.DrawMapBorders(playerGraphics.mapBorders, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);

            playerGraphics.shipMap = playerGraphics.GenerateShipMap();
            playerGraphics.DrawShipMap(playerGraphics.shipMap, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);

            playerGraphics.shipSelection = playerGraphics.GenerateShipSelection(engine.carrierMax, engine.battleshipMax, engine.submarineMax, engine.destroyerMax);
            playerGraphics.DrawShipSelection(playerGraphics.shipSelection);

            Console.SetCursorPosition(engine.XCursor, engine.YCursor);
            Helpers.MoveCursor(engine.XCursor, engine.YCursor, GameGraphics.cursor);

            //
            //Ship placement phase
            //

            do
            {
                consoleKey = Console.ReadKey(true);

                //Clear old cursor                
                Helpers.MoveCursor(engine.XCursor, engine.YCursor, "  ");

                //Draw Ships
                playerGraphics.DrawShipMap(playerGraphics.shipMap, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);

                //Draw Ship Selection playerGraphics
                playerGraphics.DrawShipSelection(playerGraphics.shipSelection);

                //Save cursor position in case of edge of map
                engine.XCursorStored = engine.XCursor;
                engine.YCursorStored = engine.YCursor;

                //Read key press
                switch (consoleKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        engine.YCursor--;
                        Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                        break;
                    case ConsoleKey.DownArrow:
                        engine.YCursor++;
                        Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                        break;
                    case ConsoleKey.LeftArrow:
                        engine.XCursor--;
                        Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                        break;
                    case ConsoleKey.RightArrow:
                        engine.XCursor++;
                        Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                        break;
                    case ConsoleKey.D1:
                        if (engine.carrierMax > 0)
                        {
                            engine.selectedShipLength = engine.carrierLength;
                            engine.shipPlacementSelected = true;
                        }
                        break;
                    case ConsoleKey.D2:
                        if (engine.battleshipMax > 0)
                        {
                            engine.selectedShipLength = engine.battleshipLength;
                            engine.shipPlacementSelected = true;
                        }
                        break;
                    case ConsoleKey.D3:
                        if (engine.submarineMax > 0)
                        {
                            engine.selectedShipLength = engine.submarineLength;
                            engine.shipPlacementSelected = true;
                        }
                        break;
                    case ConsoleKey.D4:
                        if (engine.destroyerMax > 0)
                        {
                            engine.selectedShipLength = engine.destroyerLength;
                            engine.shipPlacementSelected = true;
                        }
                        break;
                    case ConsoleKey.Spacebar:

                        //Placement of ships
                        if (engine.shipPlacementSelected)
                        {
                            if (!engine.shipFrontPlaced)
                            {
                                engine.XPositionShipFront = engine.XCursor;
                                engine.YPositionShipFront = engine.YCursor;
                                engine.shipFrontPlaced = true;
                            }
                            else
                            {
                                engine.XPositionShipBack = engine.XCursor;
                                engine.YPositionShipBack = engine.YCursor;
                                if (engine.GetValidShipPlacement(playerGraphics.shipMap, engine.XPositionShipFront, engine.YPositionShipFront, engine.XPositionShipBack, engine.YPositionShipBack, engine.selectedShipLength))
                                {
                                    playerGraphics.shipMap = playerGraphics.PlaceShip(playerGraphics.shipMap, engine.XPositionShipFront, engine.YPositionShipFront, engine.XPositionShipBack, engine.YPositionShipBack, engine.selectedShipLength);
                                    switch (engine.selectedShipLength)
                                    {
                                        case 2:
                                            engine.destroyerMax--;
                                            break;
                                        case 3:
                                            engine.submarineMax--;
                                            break;
                                        case 4:
                                            engine.battleshipMax--;
                                            break;
                                        case 5:
                                            engine.carrierMax--;
                                            break;
                                    }
                                    playerGraphics.shipSelection = playerGraphics.GenerateShipSelection(engine.carrierMax, engine.battleshipMax, engine.submarineMax, engine.destroyerMax);
                                    playerGraphics.DrawShipSelection(playerGraphics.shipSelection);
                                }
                                playerGraphics.DrawShipMap(playerGraphics.shipMap, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);
                                engine.shipFrontPlaced = false;
                                engine.shipPlacementSelected = false;
                            }
                        }
                        break;
                }

                //Placement of ships feedback                
                playerGraphics.DrawShipPlacementFeedback(engine.shipPlacementSelected, engine.shipFrontPlaced);

                //Edge detection                
                if (playerGraphics.mapBorders[engine.XCursor, engine.YCursor] == GameGraphics.wall)
                {
                    engine.XCursor = engine.XCursorStored;
                    engine.YCursor = engine.YCursorStored;
                }

                //Move Cursor                
                Helpers.MoveCursor(engine.XCursor, engine.YCursor, GameGraphics.cursor);

            } while (!engine.GetAllShipsPlaced(engine.carrierMax, engine.battleshipMax, engine.submarineMax, engine.destroyerMax));

            //
            //AI Generates map
            //

            GameGraphics aiGraphic = new GameGraphics();

            aiGraphic.shipMap = aiGraphic.GenerateShipMap();
            do
            {
                do
                {
                    do
                    {
                        engine.selectedShipLength = ai.GetShipSelection();

                        switch (engine.selectedShipLength)
                        {
                            case 2:
                                if (ai.AiDestroyerMax == 0)
                                {
                                    ai.ValidShipSelection = false;
                                }
                                else
                                {
                                    ai.ValidShipSelection = true;
                                }
                                break;
                            case 3:
                                if (ai.AiSubmarineMax == 0)
                                {
                                    ai.ValidShipSelection = false;
                                }
                                else
                                {
                                    ai.ValidShipSelection = true;
                                }
                                break;
                            case 4:
                                if (ai.AiBattleshipMax == 0)
                                {
                                    ai.ValidShipSelection = false;
                                }
                                else
                                {
                                    ai.ValidShipSelection = true;
                                }
                                break;
                            case 5:
                                if (ai.AiCarrierMax == 0)
                                {
                                    ai.ValidShipSelection = false;
                                }
                                else
                                {
                                    ai.ValidShipSelection = true;
                                }
                                break;
                        }
                    } while (!ai.ValidShipSelection);


                    engine.XPositionShipFront = ai.GetShotOrShipPlacement();
                    engine.YPositionShipFront = ai.GetShotOrShipPlacement();

                    ai.ShipDirection = ai.GetShipDirection();

                    engine.XPositionShipBack = ai.GetShipBackPlacementX(ai.ShipDirection, engine.selectedShipLength, engine.XPositionShipFront, engine.YPositionShipFront);
                    engine.YPositionShipBack = ai.GetShipBackPlacementY(ai.ShipDirection, engine.selectedShipLength, engine.XPositionShipFront, engine.YPositionShipFront);

                } while (!engine.GetValidShipPlacement(aiGraphic.shipMap, engine.XPositionShipFront + 1, engine.YPositionShipFront + 1, engine.XPositionShipBack + 1, engine.YPositionShipBack + 1, engine.selectedShipLength));

                aiGraphic.shipMap = aiGraphic.PlaceShip(aiGraphic.shipMap, engine.XPositionShipFront + 1, engine.YPositionShipFront + 1, engine.XPositionShipBack + 1, engine.YPositionShipBack + 1, engine.selectedShipLength);

                switch (engine.selectedShipLength)
                {
                    case 2:
                        if (ai.AiDestroyerMax > 0)
                        {
                            ai.AiDestroyerMax--;
                        }
                        break;
                    case 3:
                        if (ai.AiSubmarineMax > 0)
                        {
                            ai.AiSubmarineMax--;
                        }
                        break;
                    case 4:
                        if (ai.AiBattleshipMax > 0)
                        {
                            ai.AiBattleshipMax--;
                        }
                        break;
                    case 5:
                        if (ai.AiCarrierMax > 0)
                        {
                            ai.AiCarrierMax--;
                        }
                        break;
                }

            } while (!engine.GetAllShipsPlaced(ai.AiCarrierMax, ai.AiBattleshipMax, ai.AiSubmarineMax, ai.AiDestroyerMax));

            //
            //Main game ui initialization
            //

            Console.Clear();

            aiGraphic.mapBorders = aiGraphic.GenerateMap();
            aiGraphic.concealedShipMap = aiGraphic.GenerateShipMap();
            playerGraphics.concealedShipMap = playerGraphics.GenerateShipMap();

            playerGraphics.DrawMapBorders(playerGraphics.mapBorders, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);
            aiGraphic.DrawMapBorders(aiGraphic.mapBorders, GameGraphics.AiMapPositionX, GameGraphics.AiMapPositionY);
            playerGraphics.DrawShipMap(playerGraphics.shipMap, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);
            aiGraphic.DrawShipMap(aiGraphic.concealedShipMap, GameGraphics.AiMapPositionX, GameGraphics.AiMapPositionY);

            engine.XCursor = GameGraphics.AiMapPositionX + 2;
            engine.YCursor = GameGraphics.AiMapPositionY + 2;
            Console.SetCursorPosition(GameGraphics.AiMapPositionX + 2, GameGraphics.AiMapPositionY + 2);
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
                    playerGraphics.DrawShipMap(playerGraphics.concealedShipMap, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);
                    aiGraphic.DrawShipMap(aiGraphic.concealedShipMap, GameGraphics.AiMapPositionX, GameGraphics.AiMapPositionY);

                    //Draw remaining ships UI


                    //Save cursor position in case of edge of map
                    engine.XCursorStored = engine.XCursor;
                    engine.YCursorStored = engine.YCursor;

                    switch (consoleKey.Key)
                    {
                        case ConsoleKey.UpArrow:
                            engine.YCursor--;
                            Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                            break;
                        case ConsoleKey.DownArrow:
                            engine.YCursor++;
                            Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                            break;
                        case ConsoleKey.LeftArrow:
                            engine.XCursor--;
                            Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                            break;
                        case ConsoleKey.RightArrow:
                            engine.XCursor++;
                            Console.SetCursorPosition(engine.XCursor, engine.YCursor);
                            break;
                        case ConsoleKey.Spacebar:

                            //Shooting enemy ships logic
                            engine.ValidShot = engine.GetValidShot(aiGraphic.shipMap, engine.XCursor - GameGraphics.AiMapPositionX, engine.YCursor);
                            if (engine.ValidShot)
                            {
                                aiGraphic.concealedShipMap = aiGraphic.ShootShip(aiGraphic.shipMap, aiGraphic.concealedShipMap, engine.XCursor - GameGraphics.AiMapPositionX, engine.YCursor);
                                engine.ValidShot = false;
                            }
                            engine.PlayerDoneShooting = true;
                            break;
                    }


                    //Edge detection                
                    if (playerGraphics.mapBorders[engine.XCursor - GameGraphics.AiMapPositionX, engine.YCursor] == GameGraphics.wall)
                    {
                        engine.XCursor = engine.XCursorStored;
                        engine.YCursor = engine.YCursorStored;
                    }

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
                    engine.ValidShot = engine.GetValidShot(playerGraphics.concealedShipMap, ai.ShotX + 1, ai.ShotY + 1);

                    //Store shots for check if theres no valid shots left    
                    ai.shotCounter++;

                } while (!engine.ValidShot);

                //Update map
                playerGraphics.concealedShipMap = aiGraphic.ShootShip(playerGraphics.shipMap, playerGraphics.concealedShipMap, ai.ShotX + 1, ai.ShotY + 1);

                //Reset valid checker
                ai.shotCounter = 0;

                //Get if hit
                engine.Hit = engine.GetHitShot(playerGraphics.shipMap, ai.ShotX + 1, ai.ShotY + 1);
                
                //Hit logic
                if (engine.Hit)
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

                playerGraphics.DrawShipMap(playerGraphics.concealedShipMap, GameGraphics.PlayerMapPositionX, GameGraphics.PlayerMapPositionY);

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
