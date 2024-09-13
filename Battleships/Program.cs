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

                graphics.MoveCursor(player.XPositionsStored[0], player.YPositionsStored[0]);
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
            graphics.DrawShipMap(player.Map.MapWidth, player.Map.MapHeight, player.Map.MapPositionY, player.Map.MapPositionX, player.Map.ConcealedShipMap);
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
                    graphics.DrawShipMap(player.Map.MapWidth, player.Map.MapHeight, player.Map.MapPositionX, player.Map.MapPositionY, player.Map.ConcealedShipMap);
                    graphics.DrawShipMap(ai.Map.MapWidth, ai.Map.MapHeight, ai.Map.MapPositionX, ai.Map.MapPositionY, ai.Map.ConcealedShipMap);

                    graphics.MoveCursor(player.XPosition, player.YPosition);
                    player.StoreCursorPosition();

                    player.GetShot();

                    if (player.SpacebarPressed)
                    {
                        if (player.GetValidShot(ai.Map.ConcealedShipMap, ai.Map.MapPositionX))
                        {
                            ai.Map.ConcealedShipMap = player.Shoot(ai.Map.ShipMap, ai.Map.ConcealedShipMap, ai.Map.MapPositionX);
                            player.PlayerDoneShooting = true;
                        }
                    }
                    player.GetEdgeOfMapDetection(player.Map.MapBorders, ai.Map.MapPositionX);
                    graphics.MoveCursor(player.XPosition, player.YPosition);

                } while (!player.PlayerDoneShooting);

                player.PlayerDoneShooting = false;

                //
                //Ai Turn
                //

                do
                {
                    ai.GetShot();                    
                } while (!ai.GetValidShot(player.Map.ConcealedShipMap, player.Map.MapPositionX));
                ai.ShotCounter = 0;

                player.Map.ConcealedShipMap = ai.Shoot(player.Map.ShipMap, player.Map.ConcealedShipMap, player.Map.MapPositionX);

                graphics.DrawShipMap(player.Map.MapWidth, player.Map.MapHeight, player.Map.MapPositionX, player.Map.MapPositionY, player.Map.ConcealedShipMap);

                //
                //Win detection
                //

                player.Win = player.GetWin(ai.Map.ConcealedShipMap);
                ai.Win = ai.GetWin(player.Map.ConcealedShipMap);

            } while (!player.Win && !ai.Win);

            Console.WriteLine("You won!");
            Console.ReadKey();
        }
    }
}
