using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using Xengine.WindowsControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;    

namespace NavalBattleship.Screens
{
    public class MenuScreen : Screen, IScreen  
    {
        public MenuScreen(Manager manager)
        {
            base.manager = manager;

            if (Layout.ScreenFormat == ScreenFormat.Format4X3)
            {
                Panel panel = new Panel(new Rectangle(150, 230, 198, 250), "menu", Aligment.Center);
                AddControl(panel);

                Button btnMain = new Button(new Rectangle(450, 260, 120, 40), Game.GameStrings.GetString("Main"), new MouseClick(base.EnterMainScreen));
                AddControl(btnMain);

                Button btnResume = new Button(new Rectangle(450, 310, 120, 40), Game.GameStrings.GetString("Resume"), new MouseClick(base.manager.ResumeScreen));
                AddControl(btnResume);

                Button btnExit = new Button(new Rectangle(450, 360, 120, 40), Game.GameStrings.GetString("Credits"), new MouseClick(base.EnterCreditsScreen));
                AddControl(btnExit);

                Button btnCredits = new Button(new Rectangle(450, 410, 120, 40), Game.GameStrings.GetString("Exit"), new MouseClick(base.ExitGame));
                AddControl(btnCredits);
            }
            else
            {
                Panel panel = new Panel(Layout.CalculateTotalLayout(Layout.CenterRectangle(198, 250)), "menu", Aligment.Left);
                AddControl(panel);

                Button btnMain = new Button(Layout.CalculateTotalLayout(Layout.CenterRectangleX(260, 120, 40)), Game.GameStrings.GetString("Main"), new MouseClick(base.EnterMainScreen));
                AddControl(btnMain);

                Button btnResume = new Button(Layout.CalculateTotalLayout(Layout.CenterRectangleX(310, 120, 40)), Game.GameStrings.GetString("Resume"), new MouseClick(base.manager.ResumeScreen));
                AddControl(btnResume);

                Button btnExit = new Button(Layout.CalculateTotalLayout(Layout.CenterRectangleX(360, 120, 40)), Game.GameStrings.GetString("Credits"), new MouseClick(base.EnterCreditsScreen));
                AddControl(btnExit);

                Button btnCredits = new Button(Layout.CalculateTotalLayout(Layout.CenterRectangleX(410, 120, 40)), Game.GameStrings.GetString("Exit"), new MouseClick(base.ExitGame));
                AddControl(btnCredits);
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
                base.manager.ResumeScreen();  
            }
        }

        public void Draw()
        {
            base.DrawControls();
        }
    }
}
