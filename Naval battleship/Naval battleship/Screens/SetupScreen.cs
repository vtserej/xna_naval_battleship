using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using Xengine.WindowsControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input; 

namespace NavalBattleship.Screens
{
    public class SetupScreen : Screen, IScreen
    {
        string selectedShip = Game.GameStrings.GetString("None");
        bool pressed;

        #region Delegates

        string PlayerName()
        {
            return manager.Game.PlayerName;   
        }

        string SelectedShip()
        {
            if (selectedShip == string.Empty)
            {
                selectedShip = Game.GameStrings.GetString("None");
            }
            return selectedShip;
        }

        #endregion

        public SetupScreen(Manager manager)
        {
            base.manager = manager;

            if (Layout.ScreenFormat == ScreenFormat.Format4X3)
            {
                Label textPlayer = new Label(new Function(PlayerName), new Vector2(200, 735), Sprite.SpriteFont);

                Label tip = new Label(Game.GameStrings.GetString("Tip"), new Vector2(50, 7), Sprite.SpriteFont);

                Label texNamePlayer = new Label(Game.GameStrings.GetString("PlayerName"), new Vector2(50, 735), Sprite.SpriteFont);

                Label shipSel = new Label(Game.GameStrings.GetString("SelectedShip"), new Vector2(550, 735), Sprite.SpriteFont);

                Label textVersion = new Label("Version 1.0", new Vector2(876, 10), Sprite.SpriteFont);

                Label ship = new Label(new Function(SelectedShip), new Vector2(690, 735), Sprite.SpriteFont);

                Button btnComenzar = new Button(new Rectangle(857, 262, 137, 35), Game.GameStrings.GetString("Start"), new MouseClick(base.EnterGameScreen));

                Button btnOrganizar = new Button(new Rectangle(857, 216, 137, 35), Game.GameStrings.GetString("Setup"), new MouseClick(manager.Game.PlayerAllocation));

                Button btnMenu = new Button(new Rectangle(857, 660, 137, 35), Game.GameStrings.GetString("Menu"), new MouseClick(base.EnterMenuScreen));

                Button btnSalir = new Button(new Rectangle(857, 706, 137, 35), Game.GameStrings.GetString("Exit"), new MouseClick(base.ExitGame));

                Panel upPanel = new Panel(new Rectangle(0, 0, 820, 40), "down", Aligment.Left);

                Panel downPanel = new Panel(new Rectangle(0, 728, 820, 40), "down", Aligment.Left);

                Panel panelRight = new Panel(new Rectangle(820, 0, 210, 768), "SetupRight", Aligment.Left);

                base.AddControl(texNamePlayer);
                base.AddControl(tip);
                base.AddControl(ship);
                base.AddControl(shipSel);
                base.AddControl(btnOrganizar);
                base.AddControl(btnMenu);
                base.AddControl(btnSalir);
                base.AddControl(textPlayer);
                base.AddControl(upPanel);
                base.AddControl(downPanel);
                base.AddControl(textVersion);
                base.AddControl(btnComenzar);
                base.AddControl(panelRight);
            }
            else
            {
                Label textPlayer = new Label(new Function(PlayerName), Layout.CalculateTotalLayout(new Rectangle(200, 685, 200, 30)), Sprite.SpriteFont, Aligment.Left);

                Label tip = new Label(Game.GameStrings.GetString("Tip"), Layout.CalculateTotalLayout(new Rectangle(69, 6, 200, 30)), Sprite.SpriteFont, Aligment.Left);

                Label texNamePlayer = new Label(Game.GameStrings.GetString("PlayerName"), Layout.CalculateTotalLayout(new Rectangle(69, 685, 200, 30)), Sprite.SpriteFont, Aligment.Left);

                Label shipSel = new Label(Game.GameStrings.GetString("SelectedShip"), Layout.CalculateTotalLayout(new Rectangle(770, 685, 200, 30)), Sprite.SpriteFont, Aligment.Left);

                Label ship = new Label(new Function(SelectedShip), Layout.CalculateTotalLayout(new Rectangle(910, 685, 200, 30)), Sprite.SpriteFont, Aligment.Left);

                Label textVersion = new Label("Version 1.0", Layout.CalculateTotalLayout(new Rectangle(1100, 6, 150, 30)), Sprite.SpriteFont, Aligment.Center);

                Button btnComenzar = new Button(Layout.CalculateTotalLayout(new Rectangle(1107, 244, 137, 35)), Game.GameStrings.GetString("Start"), new MouseClick(base.EnterGameScreen));

                Button btnOrganizar = new Button(Layout.CalculateTotalLayout(new Rectangle(1107, 202, 137, 35)), Game.GameStrings.GetString("Setup"), new MouseClick(manager.Game.PlayerAllocation));

                Button btnMenu = new Button(Layout.CalculateTotalLayout(new Rectangle(1107, 619, 137, 35)), Game.GameStrings.GetString("Menu"), new MouseClick(base.EnterMenuScreen));

                Button btnSalir = new Button(Layout.CalculateTotalLayout(new Rectangle(1107, 660, 137, 35)), Game.GameStrings.GetString("Exit"), new MouseClick(base.ExitGame));

                Panel upPanel = new Panel(Layout.CalculateTotalLayout(new Rectangle(0, 0, 1070, 40)), "down", Aligment.Left);

                Panel downPanel = new Panel(Layout.CalculateTotalLayout(new Rectangle(0, 680, 1070, 40)), "down", Aligment.Left);

                Panel panelRight = new Panel(Layout.CalculateTotalLayout(new Rectangle(1070, 0, 210, 720)), "SetupRight", Aligment.Left);

                base.AddControl(texNamePlayer);
                base.AddControl(tip);
                base.AddControl(ship);
                base.AddControl(shipSel);
                base.AddControl(btnOrganizar);
                base.AddControl(btnMenu);
                base.AddControl(btnSalir);
                base.AddControl(textPlayer);
                base.AddControl(upPanel);
                base.AddControl(downPanel);
                base.AddControl(textVersion);
                base.AddControl(btnComenzar);
                base.AddControl(panelRight);
            }
        }

        public void Quit()
        { }

        public void Update(Cursor cursor)
        {
            base.UpdateControls(cursor);
            base.manager.Escena.UpdateSetup();

            if (cursor.MouseState.LeftButton == ButtonState.Pressed && pressed == false)
            {
                pressed = true;
                Player player;
                Point point = manager.Cursor.Position;
                selectedShip = manager.Escena.ShipSelection(point.X, point.Y, out player);
            }
            else
            {
                if (cursor.MouseState.LeftButton == ButtonState.Released)
                {
                    pressed = false;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                base.EnterMenuScreen();
            }
        }

        public void Draw()
        {
            manager.Escena.DrawSetup();   
            base.DrawControls();
        }
    }
}
