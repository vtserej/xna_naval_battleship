using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using Xengine.WindowsControls;
using Microsoft.Xna.Framework;

namespace NavalBattleship.Screens
{
    public abstract class Screen
    {
        protected Manager manager;
        protected List<IControl> iControls = new List<IControl>();
        protected List<Control> controls = new List<Control>();
        List<Control> tempControls = new List<Control>();
        protected MouseClick onClick;
        private string screenName;
        bool updating, added;
        bool showed, firstShowed;

        #region Properties

        public bool FirstShowed
        {
            get { return firstShowed; }
            set { firstShowed = value; }
        }

        public  bool Showed
        {
            get { return showed; }
            set { showed = value; }
        }

        public string ScreenName
        {
            get { return screenName; }
            set { screenName = value; }
        }

        #endregion

        #region Screen delegates

        protected void ExitGame()
        {
            GameMain.Finished = true;  
        }

        protected void EnterGameMode()
        {
            Manager.InGame = true;  
        }

        protected void ExitGameMode()
        {
            Manager.InGame = false;  
        }

        protected void EnterMenuScreen()
        {
            //AudioPlayback.Play("openmenu.wav");
            manager.Cursor.CurrentCursor  = ECursor.Arrow;     
            manager.SelectedScreen = EScreen.MenuScreen;   
        }

        protected void EnterGameScreen()
        {
            manager.Game.RandomAllocation(1);   
            manager.SelectedScreen = EScreen.GameScreen;
        }

        protected void EnterMainScreen()
        {
            //AudioPlayback.PlayOne("mainTheme.mp3");
            manager.NewGame();  
            manager.SelectedScreen = EScreen.MainScreen;
        }

        protected void EnterCreditsScreen()
        {
            //AudioPlayback.StopLoop("mainTheme.mp3");
            //AudioPlayback.PlayLoop("credits.mp3");  
            Manager.InGame = false;
            manager.SelectedScreen = EScreen.CreditsScreen;
        }

        protected void EnterSetupScreen()
        {
            Camara.LocateCamara(new Vector3(6.35f, 17, 21), new Vector3(6.35f, 0, 8));
            manager.Game.PlayerName = ((Edit)GetControl("edtName")).Text;
            manager.SelectedScreen = EScreen.SetupScreen;   
        }

        protected void EnterWinScreen()
        {
            manager.Cursor.CurrentCursor = ECursor.Arrow;
            //AudioPlayback.StopAllSounds();  
            manager.SelectedScreen = EScreen.WinScreen;
        }

        protected void QuitScreen()
        {
            manager.QuitScreeen();  
        }

        #endregion

        protected IControl GetIControl(string name)
        {
            foreach (var item in iControls)
            {
                if (item.AccesibleName() == name)
                {
                    return item; 
                }
            }
            return null; 
        }

        protected Control GetControl(string name)
        {
            foreach (var item in controls)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        void AddIControl(IControl control)
        {
            control.Create();  
            if (control.ZOrder() == 1)
            {
                iControls.Insert(0, control);
            }
            else
            {
                iControls.Add(control);
            }
        }
        
        public void AddControl(Control control)
        {
            if (updating == true)
            {
                tempControls.Add(control);
                added = true;
            }
            else
            {
                controls.Add(control);
                AddIControl((IControl)control);  
            }
        }

        protected void UpdateControls(Cursor cursor)
        {
            updating = true; 
            foreach (var item in iControls)
            {
                item.Update(cursor);
            }
            updating = false;
            if (added)
            {
                added = false;
                for (int i = 0; i < tempControls.Count; i++)
                {
                    AddControl(tempControls[i]);
                }
                tempControls.Clear();
                ClearControls(); 
            }
        }

        void ClearControls()
        {
            for (int i = 0; i < iControls.Count; i++)
            {
                if (iControls[i].Discard())
                {
                    iControls.RemoveAt(i);
                }
            }
        }

        protected void DrawControls()
        {
            Sprite.Graphics.GraphicsDevice.RenderState.MultiSampleAntiAlias = false;   
            Sprite.SpriteBatch.Begin();  
            foreach (var item in iControls)
            {
                item.Draw();
            }
            Sprite.SpriteBatch.End();
            Sprite.Graphics.GraphicsDevice.RenderState.MultiSampleAntiAlias = true;   
        }
    }
}
