using System;
using System.Collections.Generic;
using System.Text;
using NavalBattleship.Controls;
using NavalBattleship.GameCode;
using NavalBattleship.Models;
using Xengine;
using Xengine.WindowsControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 

namespace NavalBattleship.Screens
{
    public class CreditsScreen: Screen, IScreen  
    {
        Credits credits; 

        public CreditsScreen(Manager manager)
        {
            base.manager = manager;
            string path = "data\\docs\\";
            if (Game.GameStrings.Language == Language.English)
            {
                path += "creditsEnglish.txt";
            }
            else
            {
                path += "creditsSpanish.txt";
            }
            credits = new Credits(path);
            credits.Create();

            if (Layout.ScreenFormat == ScreenFormat.Format4X3)
            {

                Button btnExit = new Button(new Rectangle(884, 708, 120, 40), Game.GameStrings.GetString("Exit"), new MouseClick(this.Quit));
                Color color = new Color(33, 36, 33);
                Panel upPanel = new Panel(new Rectangle(0, 0, 1024, 80), color);
                Panel downPanel = new Panel(new Rectangle(0, 688, 1024, 100), color);
                Label lblVersion = new Label(Game.GameStrings.GetString("BattleShip"), new Vector2(10, 690), Sprite.SpriteFont);
                Line lineUp = new Line(new Vector2(0, 80), new Vector2(1024, 80), Color.White, 1);
                Line lineDown = new Line(new Vector2(0, 688), new Vector2(1024, 688), Color.White, 1);
                base.AddControl(upPanel);
                base.AddControl(downPanel);
                base.AddControl(btnExit);
                base.AddControl(lblVersion);
                base.AddControl(lineUp);
                base.AddControl(lineDown);
            }
            else
            {
                Button btnExit = new Button(Layout.CalculateTotalLayout(new Rectangle(1140, 668, 120, 40)), Game.GameStrings.GetString("Exit"), new MouseClick(this.Quit));
                Color color = new Color(33, 36, 33);
                Panel upPanel = new Panel(Layout.CalculateTotalLayout(new Rectangle(0, 0, 1280, 80)), color);
                Panel downPanel = new Panel(Layout.CalculateTotalLayout(new Rectangle(0, 640, 1280, 80)), color);
                Label lblVersion = new Label(Game.GameStrings.GetString("BattleShip"), Layout.CalculateLayoutXY(10, 650), Sprite.SpriteFont);
                Line lineUp = new Line(Layout.CalculateLayoutXY(0, 80), Layout.CalculateLayoutXY(1280, 80), Color.White, 1);
                Line lineDown = new Line(Layout.CalculateLayoutXY(0, 640), Layout.CalculateLayoutXY(1280, 640), Color.White, 1);
                base.AddControl(upPanel);
                base.AddControl(downPanel);
                base.AddControl(btnExit);
                base.AddControl(lblVersion);
                base.AddControl(lineUp);
                base.AddControl(lineDown);
            }
        }

        public void Update(Cursor cursor)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
               this.Quit();
            }
            base.UpdateControls(cursor);
            credits.Update(cursor);  
        }

        public void Quit()
        {
            //AudioPlayback.StopLoop("credits.mp3");  
            credits.Reset();  
            base.manager.QuitScreeen();   
        }

        public void Draw()
        {
            credits.Draw();  
            base.DrawControls();
        }
    }
}
