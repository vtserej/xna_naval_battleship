using System;
using System.Collections.Generic;
using System.Text;
using NavalBattleship.Screens;
using NavalBattleship.Controls;
using NavalBattleship.Models;
using Xengine;
using Xengine.WindowsControls;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework; 

namespace NavalBattleship
{
    public class Manager
    {
        #region Delegates

        void ExitGame()
        {
            GameMain.Finished = true;  
        }

        #endregion

        #region Private atributes

        static Manager instance;
        GameTime gameTime;
        Escena escena = new Escena();
        Game game = new Game();
        //esta lista solo usa para los dialog screens
        List<IScreen> iDialogScreens = new List<IScreen>();
        //esta lista solo usa la interfaz iscreen
        List<IScreen> iScreens = new List<IScreen>();
        //esta es para tener una instancia de cada pantalla 
        List<Screen> screens = new List<Screen>();
        //pantalla seleccionada
        EScreen selectedScreen = new EScreen();
        //pantalla de dialogo seleccionada
        DScreen selectedDialog;
        //ultima seleccionada
        EScreen lastScreen = new EScreen();
        //penultima seleccionada
        EScreen prevLastScreen = new EScreen(); 
        Camara camara = new Camara();
        static bool inGame;
        byte fadeOut;
        bool fading;
        bool finished;
        bool dialogScreenSelected;
        Rectangle fadeArea;
        Cursor cursor;
        Color color = Color.Black;   

        #endregion

        #region Properties

        public static Manager Instance
        {
            get { return Manager.instance; }
            set { Manager.instance = value; }
        }

        public GameTime GameTime
        {
            get { return gameTime; }
            set { gameTime = value; }
        }

        public Game Game
        {
            get { return game; }
            set { game = value; }
        } 

        static public bool InGame
        {
            get { return Manager.inGame; }
            set { Manager.inGame = value; }
        }

        public Escena Escena
        {
            get { return escena; }
            set { escena = value; }
        }
        
        public Camara Camara
        {
            get { return camara; }
            set { camara = value; }
        }

        public EScreen SelectedScreen
        {
            get { return selectedScreen; }
            set 
            {
                Camara.SaveCamaraPos(0);  
                screens[(int)selectedScreen].Showed = false;  
                fading = true;
                fadeOut = 0; 
                prevLastScreen = lastScreen; 
                lastScreen = selectedScreen; 
                selectedScreen = value;
            }
        }

        public DScreen SelectedDialog
        {
            get { return selectedDialog; }
            set 
            {
                cursor.CurrentCursor = ECursor.Arrow;      
                selectedDialog = value;
                dialogScreenSelected = true; 
            }
        }

        public Cursor Cursor
        {
            get { return cursor; }
            set { cursor = value; }
        } 

        #endregion

        public Manager()
        {
            game.Escena = escena;
            instance = this; 
        }

        public void NewGame()
        {
            game.Reset();
            foreach (var item in screens)
            {
                item.FirstShowed = false;  
            }
            ((GameScreen)screens[(int)EScreen.GameScreen]).Reset(); 
            Misile.Locked = false;
            Misile.MisilCamera = false;
            this.escena.ClearEscena();
            camara.Reset();  
        }

        /// <summary>
        /// This function creates a screen object and inserts it in an internal 
        /// list and also inserts it in an interface list for external use
        /// </summary>
        public void CreateScreens()
        {
            fadeArea = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight); 
            selectedScreen = EScreen.MainScreen;
            Screen screen = new MainScreen(this);     
            screens.Add(screen);
            iScreens.Add((IScreen)screen);
            screen = new GameScreen(this);
            screens.Add(screen);
            iScreens.Add((IScreen)screen);
            screen = new CreditsScreen(this);
            screens.Add(screen);
            iScreens.Add((IScreen)screen);
            screen = new SetupScreen(this);
            screens.Add(screen);
            iScreens.Add((IScreen)screen);
            screen = new MenuScreen(this);
            screens.Add(screen);
            iScreens.Add((IScreen)screen);
            screen = new WinScreen(this);
            screens.Add(screen);
            iScreens.Add((IScreen)screen);
            screen = new DialogScreen(this, ExitGame, LeaveDialog);
            iDialogScreens.Add((IScreen)screen);  
        }

        public void QuitScreeen()
        {
            selectedScreen = lastScreen;
            lastScreen = prevLastScreen; 
        }

        public void ResumeScreen()
        {
            if (lastScreen == EScreen.CreditsScreen)
                selectedScreen = prevLastScreen;

            else
                selectedScreen = lastScreen;
        }

        public void LeaveDialog()
        {
            dialogScreenSelected = false; 
        }

        public void AddText(string text, int time, SpriteFont font, Color textColor, int half)
        {
            screens[(int)selectedScreen].AddControl(new Message(text, time, font, textColor, half, EngineContent.GetTextureByName("message")));
        }

        public void UpdateScreen(EScreen screen)
        {
            if (!fading)
            {
                iScreens[(int)screen].Update(cursor);            
            }
        }

        public void LoadModels()
        {
            game.CreateShips();  
            game.RandomAllocation(0);
            Ship.Manager = this;
            for (int i = 0; i < 6; i++)
            {
                escena.AddSetupModel(game.ShipsPlayer[i]);
                escena.AddModel(game.ShipsPlayer[i]);
                escena.AddModel(game.ShipsComputer[i]);
            }
            escena.AddModel(new SkySphere());
            escena.AddSetupModel(new SetupBoard());
            escena.AddModel(new Sea());
            escena.AddModel(game.BoardGame[0]);
            escena.AddModel(game.BoardGame[1]);
            escena.AddModel(new Sun());
            escena.AddModel(new Cloud());
            escena.AddModel(new SwitchPlayer(this.game));
            Misile.GameManager = this;   
        }

        public void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;  
            cursor.Update();  
            if (fading == false)//si no se esta desvaneciendo la pantalla
            {
                finished = false; 
                if (dialogScreenSelected == false)//si no hay ningun mensaje de dialogo
                {
                    iScreens[(int)selectedScreen].Update(cursor);
                }
                else
                {
                    iDialogScreens[(int)selectedDialog].Update(cursor); 
                }
            }
            else
            {
                if (fadeOut < 250)
                    fadeOut += 6;
            }
        }

        public void Draw()
        {
            if (fading)//the screen is fading
            {
                iScreens[(int)lastScreen].Draw(); 
                Sprite.SpriteBatch.Begin();
                Sprite.DrawAlphaSprite(fadeArea, color, fadeOut);
                Sprite.SpriteBatch.End(); 
                if (fadeOut > 245)//the screen stops fading 
                {
                    fading = false;
                    finished = true;  
                }
            }
            else
            {
                if (!finished)
                {
                    iScreens[(int)selectedScreen].Draw();
                    if (dialogScreenSelected)
                    {
                        iDialogScreens[(int)selectedDialog].Draw();   
                    }
                }
            }
            cursor.Draw();  
        }
    }
}
