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

            //
            //Start
            //

            Player player = new Player();
            AI ai = new AI();
            UserInterface graphics = new UserInterface();

            graphics.GetInnitialConsoleEnviroment();

            graphics.DrawMapBorders(player.Map.MapWidth, player.Map.MapHeight, player.Map.MapPositionY, player.Map.MapPositionX, player.Map.MapBorders);

            graphics.DrawShipMap(player.Map.MapWidth, player.Map.MapHeight, player.Map.MapPositionY, player.Map.MapPositionX, player.Map.ShipMap);

            graphics.DrawShipSelectionUI(player.CarrierMax, player.BattleshipMax, player.SubmarineMax, player.DestroyerMax);
            graphics.DrawShipPlacementFeedback(player.ShipPlacementSelected, player.ShipFrontPlaced);

            graphics.UpdateCursorPosition(player.XPosition, player.YPosition);
            graphics.MoveCursor(player.XPosition, player.YPosition);

            //
            //Ship placement phase
            //

            do
            {
                graphics.DrawShipMap(player.Map.MapWidth, player.Map.MapHeight, player.Map.MapPositionY, player.Map.MapPositionX, player.Map.ShipMap);
                graphics.DrawShipSelectionUI(player.CarrierMax, player.BattleshipMax, player.SubmarineMax, player.DestroyerMax);

                graphics.MoveCursor(player.XPosition, player.YPosition);
                player.StoreCursorPosition();

                player.SelectShip();

                player.GetShipPlacement();

                graphics.DrawShipSelectionUI(player.CarrierMax, player.BattleshipMax, player.SubmarineMax, player.DestroyerMax);
                graphics.DrawShipMap(player.Map.MapWidth, player.Map.MapHeight, player.Map.MapPositionY, player.Map.MapPositionX, player.Map.ShipMap);
                graphics.DrawShipPlacementFeedback(player.ShipPlacementSelected, player.ShipFrontPlaced);

                //graphics.MoveCursor(player.XPositionsStored[0], player.YPositionsStored[0]);
                player.GetEdgeOfMapDetection(player.Map.MapBorders, player.Map.MapPositionX);

            } while (!player.GetAllShipsPlaced());

            //
            //AI generates map
            //           

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

            graphics.GetInnitialConsoleEnviroment();

            graphics.DrawMapBorders(player.Map.MapWidth, player.Map.MapHeight, player.Map.MapPositionX, player.Map.MapPositionY, player.Map.MapBorders);
            graphics.DrawMapBorders(ai.Map.MapWidth, ai.Map.MapHeight, ai.Map.MapPositionX, ai.Map.MapPositionY, ai.Map.MapBorders);
            graphics.DrawShipMap(player.Map.MapWidth, player.Map.MapHeight, player.Map.MapPositionY, player.Map.MapPositionX, player.Map.ShipMap);
            graphics.DrawShipMap(ai.Map.MapWidth, ai.Map.MapHeight, ai.Map.MapPositionY, ai.Map.MapPositionX, ai.Map.ConcealedShipMap);

            player.XPosition = ai.Map.MapPositionX + 2;
            player.YPosition = ai.Map.MapPositionY + 2;
            graphics.UpdateCursorPosition(ai.Map.MapPositionX + 2, ai.Map.MapPositionY + 2);
            graphics.MoveCursor(player.XPosition, player.YPosition);

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
                    graphics.DrawPlayerShootingFeedback();
                    graphics.DrawShipMap(player.Map.MapWidth, player.Map.MapHeight, player.Map.MapPositionX, player.Map.MapPositionY, player.Map.ShipMap);
                    graphics.DrawShipMap(ai.Map.MapWidth, ai.Map.MapHeight, ai.Map.MapPositionX, ai.Map.MapPositionY, ai.Map.ConcealedShipMap);
                    graphics.DrawShootingPhaseUI(player.Map.ShipMap, player.Map.PlacedDestroyers, player.Map.PlacedSubmarines, player.Map.PlacedBattleships, player.Map.PlacedCarriers, player.Map.MapPositionX, "Player");
                    graphics.DrawShootingPhaseUI(ai.Map.ShipMap, ai.Map.PlacedDestroyers, ai.Map.PlacedSubmarines, ai.Map.PlacedBattleships, ai.Map.PlacedCarriers, ai.Map.MapPositionX, "AI");

                    graphics.MoveCursor(player.XPosition, player.YPosition);
                    player.StoreCursorPosition();

                    player.GetShot();
                    player.GetEdgeOfMapDetection(player.Map.MapBorders, ai.Map.MapPositionX);
                    graphics.MoveCursor(player.XPosition, player.YPosition);

                } while (!player.GetValidShot(ai.Map.ConcealedShipMap, ai.Map.MapPositionX));

                ai.Map.ConcealedShipMap = player.Shoot(ai.Map.ShipMap, ai.Map.ConcealedShipMap, ai.Map.MapPositionX);
                ai.Map.UpdateShipMap();

                //
                //Ai Turn
                //

                do
                {
                    ai.GetShot();
                } while (!ai.GetValidShot(player.Map.ConcealedShipMap, player.Map.MapPositionX));
                ai.ShotCounter = 0;

                graphics.RemoveCursor(player.XPosition, player.YPosition, ai.Map.ConcealedShipMap, ai.Map.MapPositionX);
                graphics.DrawAiShootingFeedback(ai.XPosition, ai.YPosition, player.Map.ShipMap);

                player.Map.ConcealedShipMap = ai.Shoot(player.Map.ShipMap, player.Map.ConcealedShipMap, player.Map.MapPositionX);
                player.Map.UpdateShipMap();
                graphics.DrawShipMap(player.Map.MapWidth, player.Map.MapHeight, player.Map.MapPositionX, player.Map.MapPositionY, player.Map.ConcealedShipMap);

                //
                //Win detection
                //

                player.Win = player.GetWin(ai.Map.ConcealedShipMap);
                ai.Win = ai.GetWin(player.Map.ConcealedShipMap);

            } while (!player.Win && !ai.Win);

            graphics.DrawWinFeedback(player.Win, ai.Win);
        }
    }
}
