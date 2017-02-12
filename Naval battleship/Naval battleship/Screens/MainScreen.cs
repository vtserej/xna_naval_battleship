using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using Xengine.WindowsControls;
using NavalBattleship.Models;
using Microsoft.Xna.Framework;
using NavalBattleship.Controls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 

namespace NavalBattleship.Screens
{
    public class MainScreen : Screen, IScreen
    {
        #region Delegates

        void SelectSpanish()
        {
            if (Game.GameStrings.Language != Language.Spanish)
            {
                Game.GameStrings.SelectLanguage(Language.Spanish);
                manager.AddText(Game.GameStrings.GetString("ChangesMade"), 100, Sprite.SpriteFont, Color.Black, Globals.ScreenWidthOver2);
            }
        }

        void SelectEnglish()
        {
            if (Game.GameStrings.Language != Language.English)
            {
                Game.GameStrings.SelectLanguage(Language.English);
                manager.AddText(Game.GameStrings.GetString("ChangesMade"), 100, Sprite.SpriteFont, Color.Black, Globals.ScreenWidthOver2);
            }
        }

        #endregion

        Flag flag;  

        public MainScreen(Manager manager)
        {
            base.manager = manager;

            if (Layout.ScreenFormat == ScreenFormat.Format4X3)
            {
                flag = new Flag(new Vector3(1, 1, 0.01f), new Vector3(1.2f, 2, 1), EngineContent.GetTextureByName(Game.GameStrings.GetString("Title")), new Rectangle(430, 85, 520, 140));

                flag.Create();

                Edit edit = new Edit(new Rectangle(690, 660, 120, 40), "edtName", Game.GameStrings.GetString("DefaultName"));

                Panel panel = new Panel(new Rectangle(0, 0, 1024, 768), "portada", Aligment.Left);

                Button button = new Button(new Rectangle(830, 660, 100, 40), Game.GameStrings.GetString("Enter"), new MouseClick(base.EnterSetupScreen));

                Label textInsertar = new Label(Game.GameStrings.GetString("InsertName"), new Vector2(730, 622), Sprite.SpriteFont);

                FlagButton languageSpanish = new FlagButton(new Rectangle(80, 642, 100, 50), SelectSpanish, new Vector3(-5.55f, -5.1f, -15), Game.GameStrings.GetString("Spanish"), "espanna");

                FlagButton languageEnglish = new FlagButton(new Rectangle(200, 642, 100, 50), SelectEnglish, new Vector3(-3.6f, -5.1f, -15), Game.GameStrings.GetString("English"), "uk");

                Label textLanguage = new Label(Game.GameStrings.GetString("SelectLanguage"), new Rectangle(82, 622, 218, 20), Sprite.SpriteFont, Aligment.Center);

                base.AddControl(panel);
                base.AddControl(textInsertar);
                base.AddControl(button);
                base.AddControl(edit);
                base.AddControl(languageSpanish);
                base.AddControl(languageEnglish);
                base.AddControl(textLanguage);
            }
            else
            {
                flag = new Flag(new Vector3(1, 1, 0.01f), new Vector3(1.2f, 2, 1), EngineContent.GetTextureByName(Game.GameStrings.GetString("Title")), Layout.CalculateTotalLayout(new Rectangle(538, 79, 520, 140)));

                flag.Create();

                Edit edit = new Edit(Layout.CalculateTotalLayout(new Rectangle(940, 612, 120, 40)), "edtName", Game.GameStrings.GetString("DefaultName"));

                Panel panel = new Panel(Layout.CalculateTotalLayout(new Rectangle(0, 0, 1280, 720)), "portada", Aligment.Left);

                Button button = new Button(Layout.CalculateTotalLayout(new Rectangle(1080, 612, 100, 40)), Game.GameStrings.GetString("Enter"), new MouseClick(base.EnterSetupScreen));

                Label textInsertar = new Label(Game.GameStrings.GetString("InsertName"), Layout.CalculateLayoutXY(980, 574), Sprite.SpriteFont);

                FlagButton languageSpanish = new FlagButton(Layout.CalculateTotalLayout(new Rectangle(80, 596, 100, 50)), SelectSpanish, new Vector3(-5.55f, -5.1f, -15), Game.GameStrings.GetString("Spanish"), "espanna");

                FlagButton languageEnglish = new FlagButton(Layout.CalculateTotalLayout(new Rectangle(200, 596, 100, 50)), SelectEnglish, new Vector3(-3.6f, -5.1f, -15), Game.GameStrings.GetString("English"), "uk");

                Label textLanguage = new Label(Game.GameStrings.GetString("SelectLanguage"), Layout.CalculateTotalLayout(new Rectangle(82, 574, 218, 20)), Sprite.SpriteFont, Aligment.Center);

                base.AddControl(panel);
                base.AddControl(textInsertar);
                base.AddControl(button);
                base.AddControl(edit);
                base.AddControl(languageSpanish);
                base.AddControl(languageEnglish);
                base.AddControl(textLanguage);
            }
    
        }

        public void Update(Cursor cursor)
        {
            base.UpdateControls(cursor);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                GameMain.FinishPrompt = true;

            flag.Update();  
        }

        public void Quit()
        { }

        public void Draw()
        {
            base.DrawControls();
            flag.Draw();  
        }
    }
}
