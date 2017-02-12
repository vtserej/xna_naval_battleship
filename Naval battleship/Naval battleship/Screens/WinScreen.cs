using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using Xengine.WindowsControls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NavalBattleship.Controls;


namespace NavalBattleship.Screens
{
    public class WinScreen : Screen , IScreen
    {
        #region Delegates

        string GetWinner()
        {
            if (base.manager.Game.Winner == Player.Person)
            {
                return base.manager.Game.PlayerName; 
            }
            return Game.GameStrings.GetString("CPU");   
        }

        string PlayerName()
        {
            return manager.Game.PlayerName;
        }

        string GetWinnerMessage()
        {
            return base.manager.Game.FinishMessage + ", " + PlayerName();
        }

        string GetPlayerAccuracy()
        {
           return manager.Game.Accuracy(Player.Person);     
        }

        string GetCPUAccuracy()
        {
           return manager.Game.Accuracy(Player.Computer);
        }

        string GetPlayerScore()
        {
            return Convert.ToString(manager.Game.Score(Player.Person));     
        }

        string GetCPUScore()
        {
            return Convert.ToString(manager.Game.Score(Player.Computer));
        }

        string GetPlayerRank()
        {
            return manager.Game.Rank(Player.Person);     
        }

        string GetCPURank()
        {
            return manager.Game.Rank(Player.Computer);
        }

        string GetPlayerFleet()
        {
            return Convert.ToString(manager.Game.GetShipOverallState(Player.Person))+ " %";     
        }

        string GetCPUFleet()
        {
            return Convert.ToString(manager.Game.GetShipOverallState(Player.Computer)) + " %";
        }

        void NewGame()
        {
            base.EnterMainScreen();  
        }

        #endregion

        public WinScreen(Manager manager)
        {
            base.manager = manager;

            if (Layout.ScreenFormat == ScreenFormat.Format4X3)
            {
                #region format 4X3

                #region up and down panel

                Color color = new Color(33, 36, 33);

                Panel upPanel = new Panel(new Rectangle(0, 0, 1024, 80), color);
                AddControl(upPanel);

                Panel downPanel = new Panel(new Rectangle(0, 688, 1024, 100), color);
                AddControl(downPanel);

                Label lblVersion = new Label(Game.GameStrings.GetString("BattleShip"), new Vector2(10, 690), Sprite.SpriteFont);
                AddControl(lblVersion);

                Line lineUp = new Line(new Vector2(0, 80), new Vector2(1024, 80), Color.White, 1);
                AddControl(lineUp);

                Line lineDown = new Line(new Vector2(0, 688), new Vector2(1024, 688), Color.White, 1);
                AddControl(lineDown);

                #endregion

                Panel pnlHoldDown = new Panel(new Rectangle(204, 338, 716, 349), "credits", Aligment.Center);
                AddControl(pnlHoldDown);

                Panel pnlPlayer = new Panel(new Rectangle(242, 182, 110, 110), "logoPlayer", Aligment.Left);
                AddControl(pnlPlayer);

                Panel pnlComputer = new Panel(new Rectangle(672, 182, 110, 110), "logoCPU", Aligment.Left);
                AddControl(pnlComputer);

                Label lblMessage = new Label(GetWinnerMessage, new Rectangle(290, 82, 445, 49), Sprite.SpriteFont, Aligment.Center);
                AddControl(lblMessage);

                Label lblPlayer = new Label(PlayerName, new Rectangle(415, 338, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(lblPlayer);

                Label lblCPU = new Label(Game.GameStrings.GetString("CPU"), new Rectangle(580, 338, 200, 50), Sprite.SpriteFont, Aligment.Right);
                AddControl(lblCPU);

                #region Game Statistics

                Label aimText = new Label(Game.GameStrings.GetString("Aiming"), new Rectangle(168, 410, 200, 50), Sprite.SpriteFont, Aligment.Center);
                AddControl(aimText);

                Label aimPlayer = new Label(GetPlayerAccuracy, new Rectangle(400, 410, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(aimPlayer);

                Label aimCPU = new Label(GetCPUAccuracy, new Rectangle(665, 410, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(aimCPU);

                Label scoreText = new Label(Game.GameStrings.GetString("Score"), new Rectangle(168, 475, 200, 50), Sprite.SpriteFont, Aligment.Center);
                AddControl(scoreText);

                Label scorePlayer = new Label(GetPlayerScore, new Rectangle(400, 475, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(scorePlayer);

                Label scoreCPU = new Label(GetCPUScore, new Rectangle(665, 475, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(scoreCPU);

                Label rankingText = new Label(Game.GameStrings.GetString("Rank"), new Rectangle(168, 542, 200, 50), Sprite.SpriteFont, Aligment.Center);
                AddControl(rankingText);

                Label rankingPlayer = new Label(GetPlayerRank, new Rectangle(400, 542, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(rankingPlayer);

                Label rankingCPU = new Label(GetCPURank, new Rectangle(665, 542, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(rankingCPU);

                Label fleetText = new Label(Game.GameStrings.GetString("Fleet"), new Rectangle(168, 608, 200, 50), Sprite.SpriteFont, Aligment.Center);
                AddControl(fleetText);

                Label fleetPlayer = new Label(GetPlayerFleet, new Rectangle(400, 608, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(fleetPlayer);

                Label fleetCPU = new Label(GetCPUFleet, new Rectangle(665, 608, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(fleetCPU);

                #endregion

                Button btnAcept = new Button(new Rectangle(684, 708, 137, 40), Game.GameStrings.GetString("Acept"), new MouseClick(base.manager.ResumeScreen));
                AddControl(btnAcept);

                Button btnNewGame = new Button(new Rectangle(854, 708, 137, 40), Game.GameStrings.GetString("NewGame"), new MouseClick(NewGame));
                AddControl(btnNewGame);

                #endregion
            }
            else
            {
                #region format 16X9

                #region up and down panel

                Color color = new Color(33, 36, 33);

                Panel upPanel = new Panel(Layout.CalculateTotalLayout(new Rectangle(0, 0, 1280, 80)), color);
                AddControl(upPanel);

                Panel downPanel = new Panel(Layout.CalculateTotalLayout(new Rectangle(0, 640, 1280, 80)), color);
                AddControl(downPanel);

                Label lblVersion = new Label(Game.GameStrings.GetString("BattleShip"), Layout.CalculateLayoutXY(10, 644), Sprite.SpriteFont);
                AddControl(lblVersion);

                Line lineUp = new Line(Layout.CalculateLayoutXY(0, 80), new Vector2(1280, 80), Color.White, 1);
                AddControl(lineUp);

                Line lineDown = new Line(Layout.CalculateLayoutXY(0, 640), new Vector2(1280, 640), Color.White, 1);
                AddControl(lineDown);

                #endregion

                Panel pnlHoldDown = new Panel(new Rectangle(204, 338, 716, 349), "credits", Aligment.Center);
                AddControl(pnlHoldDown);

                Panel pnlPlayer = new Panel(new Rectangle(242, 182, 110, 110), "logoPlayer", Aligment.Left);
                AddControl(pnlPlayer);

                Panel pnlComputer = new Panel(new Rectangle(672, 182, 110, 110), "logoCPU", Aligment.Left);
                AddControl(pnlComputer);

                Label lblMessage = new Label(GetWinnerMessage, new Rectangle(290, 82, 445, 49), Sprite.SpriteFont, Aligment.Center);
                AddControl(lblMessage);

                Label lblPlayer = new Label(PlayerName, new Rectangle(415, 338, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(lblPlayer);

                Label lblCPU = new Label(Game.GameStrings.GetString("CPU"), new Rectangle(580, 338, 200, 50), Sprite.SpriteFont, Aligment.Right);
                AddControl(lblCPU);

                #region Game Statistics

                Label aimText = new Label(Game.GameStrings.GetString("Aiming"), new Rectangle(168, 410, 200, 50), Sprite.SpriteFont, Aligment.Center);
                AddControl(aimText);

                Label aimPlayer = new Label(GetPlayerAccuracy, new Rectangle(400, 410, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(aimPlayer);

                Label aimCPU = new Label(GetCPUAccuracy, new Rectangle(665, 410, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(aimCPU);

                Label scoreText = new Label(Game.GameStrings.GetString("Score"), new Rectangle(168, 475, 200, 50), Sprite.SpriteFont, Aligment.Center);
                AddControl(scoreText);

                Label scorePlayer = new Label(GetPlayerScore, new Rectangle(400, 475, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(scorePlayer);

                Label scoreCPU = new Label(GetCPUScore, new Rectangle(665, 475, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(scoreCPU);

                Label rankingText = new Label(Game.GameStrings.GetString("Rank"), new Rectangle(168, 542, 200, 50), Sprite.SpriteFont, Aligment.Center);
                AddControl(rankingText);

                Label rankingPlayer = new Label(GetPlayerRank, new Rectangle(400, 542, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(rankingPlayer);

                Label rankingCPU = new Label(GetCPURank, new Rectangle(665, 542, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(rankingCPU);

                Label fleetText = new Label(Game.GameStrings.GetString("Fleet"), new Rectangle(168, 608, 200, 50), Sprite.SpriteFont, Aligment.Center);
                AddControl(fleetText);

                Label fleetPlayer = new Label(GetPlayerFleet, new Rectangle(400, 608, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(fleetPlayer);

                Label fleetCPU = new Label(GetCPUFleet, new Rectangle(665, 608, 200, 50), Sprite.SpriteFont, Aligment.Left);
                AddControl(fleetCPU);

                #endregion

                Button btnAcept = new Button(new Rectangle(905, 664, 137, 40), Game.GameStrings.GetString("Acept"), new MouseClick(base.manager.ResumeScreen));
                AddControl(btnAcept);

                Button btnNewGame = new Button(new Rectangle(1098, 664, 137, 40), Game.GameStrings.GetString("NewGame"), new MouseClick(NewGame));
                AddControl(btnNewGame);

                #endregion
            }
        }

        public void Quit()
        {
            manager.QuitScreeen();  
        }

        public void Update(Cursor cursor)
        {
            base.UpdateControls(cursor);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) //escape key
            {
                //AudioPlayback.ResumeAllSounds(); 
                base.manager.ResumeScreen();
            }
        }

        public void Draw()
        {
            //Gl.glClearColor(0, 0, 0, 1); 
            base.DrawControls();
        }
    }
}
