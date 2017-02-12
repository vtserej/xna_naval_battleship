using System;
using System.Collections.Generic;
using System.Text;
using NavalBattleship.Controls;
using NavalBattleship.GameCode; 
using Xengine;
using Xengine.WindowsControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace NavalBattleship.Screens
{
    public class DialogScreen : Screen,IScreen
    {
        Rectangle rect;

        public DialogScreen(Manager manager, MouseClick onClickYes, MouseClick onClickNo)
        {
            base.manager = manager;
            if (Layout.ScreenFormat == ScreenFormat.Format4X3)
            {
                rect = Layout.CenterRectangle(400, 150);
                Label message = new Label(Game.GameStrings.GetString("ExitGame"), new Rectangle(412, 324, 200, 30),
                                    Sprite.SpriteFont, Aligment.Center);
                Button yes = new Button(new Rectangle(362, 390, 100, 35), Game.GameStrings.GetString("Yes"), onClickYes);
                Button no = new Button(new Rectangle(562, 390, 100, 35), Game.GameStrings.GetString("No"), onClickNo);
                //Panel borderPanel = new Panel(new Rectangle(312, 289, 400, 150), Color.White, 1, Aligment.Left);
                AddControl(message);
                AddControl(yes);
                AddControl(no);
                //AddControl(borderPanel);   
            }
            else
            {
                rect = Layout.CalculateTotalLayout(Layout.CenterRectangle(400, 150));
                Label message = new Label(Game.GameStrings.GetString("ExitGame"), Layout.CalculateTotalLayout(Layout.CenterRectangleX(320, 200, 30)),
                                    Sprite.SpriteFont, Aligment.Center);
                Button yes = new Button(Layout.CalculateTotalLayout(Layout.CenterRectangleOverMiddleX(380, 100, 35, -100)),
                                    Game.GameStrings.GetString("Yes"), onClickYes);
                Button no = new Button(Layout.CalculateTotalLayout(Layout.CenterRectangleOverMiddleX(380, 100, 35, 100)),
                                    Game.GameStrings.GetString("No"), onClickNo);
                //Panel borderPanel = new Panel(new Rectangle(312, 289, 400, 150), Color.White, 1, Aligment.Left);
                AddControl(message);
                AddControl(yes);
                AddControl(no);
                //AddControl(borderPanel); 
            }
        }

        public void Update(Cursor cursor)
        {
            base.UpdateControls(cursor);  
        }

        public void Quit()
        { }

        public void Draw()
        {        
            Sprite.SpriteBatch.Begin(); 
            Sprite.DrawAlphaSprite(rect, Color.Gray, 240);
            Sprite.SpriteBatch.End(); 
            base.DrawControls();
        }
    }
}
