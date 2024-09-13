﻿using System.ComponentModel.Design;
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
            AI ai = new AI();

            //ConsoleKeyInfo consoleKey;
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

            //playerGraphics.mapBorders = playerGraphics.GenerateMapBorders();
            playerGraphics.DrawMapBorders();

            //playerGraphics.shipMap = playerGraphics.GenerateShipMap();
            playerGraphics.DrawShipMap();

            playerGraphics.shipSelection = playerGraphics.GenerateShipSelection(engine.CarrierMax, engine.BattleshipMax, engine.SubmarineMax, engine.DestroyerMax);
            playerGraphics.DrawShipSelection();

            Console.SetCursorPosition(engine.XPosition, engine.YPosition);
            Helpers.MoveCursor(engine.XPosition, engine.YPosition);

            //
            //Ship placement phase
            //

            do
            {
                playerGraphics.DrawShipMap();
                playerGraphics.DrawShipSelection();

                Helpers.MoveCursor(engine.XPosition, engine.YPosition);
                engine.StoreCursorPosition();

                engine.SelectShip();
                
                engine.GetShipPlacement();
                playerGraphics.shipSelection = playerGraphics.GenerateShipSelection(engine.CarrierMax, engine.BattleshipMax, engine.SubmarineMax, engine.DestroyerMax);

                playerGraphics.DrawShipSelection();
                playerGraphics.DrawShipMap();
                playerGraphics.DrawShipPlacementFeedback(engine.ShipPlacementSelected, engine.ShipFrontPlaced);

                Helpers.MoveCursor(engine.XPositionsStored[0], engine.YPositionsStored[0], "  ");
                engine.GetEdgeOfMapDetection(engine.Map.MapBorders, playerGraphics.MapPositionX);                

            } while (!engine.GetAllShipsPlaced());

            //
            //AI generates map
            //           

            //aiGraphic.shipMap = aiGraphic.GenerateShipMap();
            do
            {
                do
                {
                    ai.SelectShip();
                    ai.GetShipPlacement();

                } while (!ai.GetValidShipPlacement());

                ai.PlaceShip();

            } while (!ai.GetAllShipsPlaced());

            //
            //Main game ui initialization
            //

            Console.Clear();

            //aiGraphic.mapBorders = aiGraphic.GenerateMapBorders();
            //aiGraphic.concealedShipMap = aiGraphic.GenerateShipMap();
            //playerGraphics.concealedShipMap = playerGraphics.GenerateShipMap();

            playerGraphics.DrawMapBorders();
            aiGraphic.DrawMapBorders();
            playerGraphics.DrawShipMap(playerGraphics.concealedShipMap);
            aiGraphic.DrawShipMap(aiGraphic.concealedShipMap);

            engine.XPosition = aiGraphic.MapPositionX + 2;
            engine.YPosition = aiGraphic.MapPositionY + 2;
            Console.SetCursorPosition(aiGraphic.MapPositionX + 2, aiGraphic.MapPositionY + 2);
            Helpers.MoveCursor(engine.XPosition, engine.YPosition, GameGraphics.cursor);

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
                    playerGraphics.DrawShipMap(playerGraphics.concealedShipMap);
                    aiGraphic.DrawShipMap(aiGraphic.concealedShipMap);

                    Helpers.MoveCursor(engine.XPosition, engine.YPosition);
                    engine.StoreCursorPosition();

                    engine.GetShot();

                    if (engine.SpacebarPressed)
                    {
                        if (engine.GetValidShot(ai.Map.ConcealedShipMap, aiGraphic.MapPositionX))
                        {
                            ai.Map.ConcealedShipMap = engine.Shoot(ai.Map.ShipMap, ai.Map.ConcealedShipMap, aiGraphic.MapPositionX);
                            engine.PlayerDoneShooting = true;
                        }
                    }
                    engine.GetEdgeOfMapDetection(engine.Map.MapBorders, aiGraphic.MapPositionX);
                    Helpers.MoveCursor(engine.XPosition, engine.YPosition, "  ");

                } while (!engine.PlayerDoneShooting);

                engine.PlayerDoneShooting = false;

                //
                //Ai Turn
                //

                do
                {
                    ai.GetShot();                    
                } while (!ai.GetValidShot(engine.Map.ConcealedShipMap, playerGraphics.MapPositionX));
                ai.ShotCounter = 0;

                engine.Map.ConcealedShipMap = ai.Shoot(engine.Map.ShipMap, engine.Map.ConcealedShipMap, playerGraphics.MapPositionX);

                playerGraphics.DrawShipMap(playerGraphics.concealedShipMap);

                //
                //Win detection
                //

                engine.Win = engine.GetWin(ai.Map.ConcealedShipMap);

            } while (!engine.Win);

            Console.WriteLine("You won!");
            Console.ReadKey();
        }
    }
}
