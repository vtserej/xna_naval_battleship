using System;
using System.Collections.Generic;
using System.Text;
using NavalBattleship.GameCode; 
using NavalBattleship.Controls;
using NavalBattleship.Models; 
using Xengine;
using NavalBattleship.Particles;
using Xengine.WindowsControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;   


namespace NavalBattleship.Screens
{
    public delegate void PlayerShoot(int x, int y, Player player);  

    public class GameScreen : Screen, IScreen  
    {
        bool shipsShowed;
        int state; //0 nada; 1 cambio de jugador; 2 la computadora espera un time;
        int count;
        int boardCount;
        int winnerCount = 180;
        int deltaScroll;
        Rectangle viewArea;

        #region Delegates

        string GetPlayerName(object objeto)
        {
            if ((Player)objeto == Player.Person)
            {
                return manager.Game.PlayerName;
            }
            else
                return Game.GameStrings.GetString("CPU");      
        }

        string GetPlayerTurn()
        {
            if (base.manager.Game.CurrentPlayer == Player.Person)
            {
                return base.manager.Game.PlayerName;
            }
            else
                return Game.GameStrings.GetString("CPU");
        }

        void ChangeMisileCamera()
        {
            if (!Camara.Locked)
            {
                //AudioPlayback.Play("selectmenu.wav");
                if (Misile.MisilCamera == true && Camara.LockedMisileCamera == false)
                {
                    base.manager.AddText(Game.GameStrings.GetString("MisileOff"), 150, Sprite.SpriteFont, Color.DarkBlue, Globals.ScreenWidthOver2);
                    Misile.MisilCamera = false;
                    Camara.LockedMisileCamera = false; 
                }
                else
                {
                    if (Misile.MisilCamera == false && Camara.LockedMisileCamera == false && Camara.MisileLocked == false)
                    {
                        base.manager.AddText(Game.GameStrings.GetString("MisileOn"), 150, Sprite.SpriteFont, Color.DarkBlue, Globals.ScreenWidthOver2);
                        Misile.MisilCamera = true;
                    }
                }
            }
        }

        void ChangeCamera()
        {
            //AudioPlayback.Play("selectmenu.wav");  
            if (Camara.RoofLocked == true && Camara.LockedMisileCamera == false)
            {
                base.manager.AddText(Game.GameStrings.GetString("FreeCamera"), 100, Sprite.SpriteFont, Color.DarkBlue, Globals.ScreenWidthOver2);
            }
            else
            {
                if (Camara.MisileLocked == false)
                {
                    base.manager.AddText(Game.GameStrings.GetString("AirCamera"), 100, Sprite.SpriteFont, Color.DarkBlue, Globals.ScreenWidthOver2);
                }
            }
            Camara.RoofCamera();
        }

        void SwitchSounds()
        {
            //AudioPlayback.OnOff();
            //if (AudioPlayback.CanPlay == true)
            //{
            //    base.manager.AddText(Game.GameStrings.GetString("SoundsOn"), 150, Glut.GLUT_BITMAP_HELVETICA_18
            //        , Color.DarkBlue,444);
            //}
            //else
            //{
            //    base.manager.AddText(Game.GameStrings.GetString("SoundsOff"), 150, Glut.GLUT_BITMAP_HELVETICA_18
            //       , Color.DarkBlue,444);  
            //}
        }

        void MoveLeft()
        {
            Camara.Move = CameraMove.Left;     
        }

        void MoveRight()
        {
            Camara.Move = CameraMove.Right;   
        }

        void MoveUp()
        {
            Camara.Move = CameraMove.Up;
        }

        void MoveDown()
        {
            Camara.Move = CameraMove.Down;
        }

        void ZoomIn()
        {
            Camara.Move = CameraMove.ZoomIn;
        }

        void ZoomOut()
        { 
            Camara.Move = CameraMove.ZoomOut;
        }

        #endregion

        public GameScreen(Manager manager)
        {
            base.manager = manager;

            if (Layout.ScreenFormat == ScreenFormat.Format4X3)
            {
                #region 4X3 format screen

                viewArea = new Rectangle(0, 0, 888, 728);

                Button camara = new Button(new Rectangle(910, 194, 32, 31), ChangeCamera, "camaraUp",
                    "camaraDown", "camaraUp", Sprite.SpriteFont);

                Button misileCamera = new Button(new Rectangle(942, 193, 26, 31), ChangeMisileCamera, "misileCameraUp",
                  "misileCameraDown", "misileCameraUp", Sprite.SpriteFont);

                Button sound = new Button(new Rectangle(967, 195, 31, 31), SwitchSounds, "soundUp",
                 "soundDown", "soundUp", Sprite.SpriteFont);

                Button menu = new Button(new Rectangle(900, 20, 109, 21), EnterMenuScreen, "menuNormal",
                 "menuUp", "menuNormal", Sprite.SpriteFont);

                LifeBar playerLife = new LifeBar(Player.Person, base.manager.Game, new Rectangle(55, 739, 145, 18));

                LifeBar cpuLife = new LifeBar(Player.Computer, base.manager.Game, new Rectangle(705, 739, 145, 18));

                Label playerTurn = new Label(new Function(GetPlayerTurn), new Rectangle(900, 3, 111, 14), Sprite.SpriteFont, Aligment.Center);

                Label playerTurnDown = new Label(new Function(GetPlayerTurn), new Rectangle(357, 733, 174, 28), Sprite.SpriteFont, Aligment.Center);

                Panel downPanel = new Panel(new Rectangle(0, 728, 888, 40), "down", Aligment.Left);

                Panel panelRight = new Panel(new Rectangle(888, 0, 136, 768), "gameRight", Aligment.Left);

                Label lblPlayer = new Label(new FunctionParameter(GetPlayerName), Player.Person, new Rectangle(211, 739, 161, 14), Sprite.SpriteFont, Aligment.Left);

                Label lblCPU = new Label(new FunctionParameter(GetPlayerName), Player.Computer, new Rectangle(533, 739, 161, 14), Sprite.SpriteFont, Aligment.Right);

                Label lblFleet = new Label(Game.GameStrings.GetString("EnemyFleet"), new Rectangle(900, 232, 111, 14), Sprite.SpriteFont, Aligment.Center);

                Button cameraRotateLeft = new Button(new Rectangle(10, 70, 60, 40), "left", new MouseDown(MoveLeft));

                Button cameraRotateRight = new Button(new Rectangle(155, 70, 60, 40), "right", new MouseDown(MoveRight));

                Button cameraZoomIn = new Button(new Rectangle(75, 55, 75, 30), "zoom in", new MouseDown(ZoomIn));

                Button cameraZoomOut = new Button(new Rectangle(75, 90, 75, 30), "zoom out", new MouseDown(ZoomOut));

                Button cameraMoveUp = new Button(new Rectangle(75, 10, 75, 40), "Up", new MouseDown(MoveUp));

                Button cameraMoveDown = new Button(new Rectangle(75, 125, 75, 40), "Down", new MouseDown(MoveDown));

                AddControl(camara);
                AddControl(misileCamera);
                AddControl(sound);
                AddControl(menu);
                AddControl(playerLife);
                AddControl(cpuLife);
                AddControl(playerTurn);
                AddControl(playerTurnDown);
                AddControl(panelRight);
                AddControl(downPanel);
                AddControl(lblPlayer);
                AddControl(lblCPU);
                AddControl(lblFleet);
                AddControl(cameraRotateLeft);
                AddControl(cameraRotateRight);
                AddControl(cameraZoomIn);
                AddControl(cameraZoomOut);
                AddControl(cameraMoveUp);
                AddControl(cameraMoveDown);

                #endregion
            }
            else
            {
                #region 16X9 format screen

                viewArea = Layout.CalculateTotalLayout(new Rectangle(0, 0, 1144, 720));

                Button camara = new Button(Layout.CalculateTotalLayout(new Rectangle(1166, 182, 32, 29)), ChangeCamera, "camaraHover",
                    "camaraDown", "camaraUp", Sprite.SpriteFont);

                Button misileCamera = new Button(Layout.CalculateTotalLayout(new Rectangle(1198, 181, 26, 29)), ChangeMisileCamera, "misileCameraUp",
                  "misileCameraDown", "misileCameraUp", Sprite.SpriteFont);

                Button sound = new Button(Layout.CalculateTotalLayout(new Rectangle(1223, 183, 31, 29)), SwitchSounds, "soundUp",
                 "soundDown", "soundUp", Sprite.SpriteFont);

                Button menu = new Button(Layout.CalculateTotalLayout(new Rectangle(1156, 20, 109, 20)), EnterMenuScreen, "menuNormal",
                 "menuUp", "menuNormal", Sprite.SpriteFont);

                LifeBar playerLife = new LifeBar(Player.Person, base.manager.Game, Layout.CalculateTotalLayout(new Rectangle(69, 691, 145, 18)));

                LifeBar cpuLife = new LifeBar(Player.Computer, base.manager.Game, Layout.CalculateTotalLayout(new Rectangle(953, 691, 145, 18)));

                Label playerTurn = new Label(new Function(GetPlayerTurn), Layout.CalculateTotalLayout(new Rectangle(1156, 2, 111, 14)), Sprite.SpriteFont, Aligment.Center);

                Label playerTurnDown = new Label(new Function(GetPlayerTurn), Layout.CalculateTotalLayout(new Rectangle(485, 693, 174, 14)), Sprite.SpriteFont, Aligment.Center);

                Panel downPanel = new Panel(Layout.CalculateTotalLayout(new Rectangle(0, 680, 1144, 40)), "down", Aligment.Left);

                Panel panelRight = new Panel(Layout.CalculateTotalLayout(new Rectangle(1144, 0, 136, 720)), "gameRight", Aligment.Left);

                Label lblPlayer = new Label(new FunctionParameter(GetPlayerName), Player.Person,Layout.CalculateTotalLayout( new Rectangle(224, 693, 161, 14)), Sprite.SpriteFont, Aligment.Left);

                Label lblCPU = new Label(new FunctionParameter(GetPlayerName), Player.Computer, Layout.CalculateTotalLayout(new Rectangle(782, 693, 161, 14)), Sprite.SpriteFont, Aligment.Right);

                Label lblFleet = new Label(Game.GameStrings.GetString("EnemyFleet"),Layout.CalculateTotalLayout( new Rectangle(1156, 218, 111, 13)), Sprite.SpriteFont, Aligment.Center);

                Button cameraRotateLeft = new Button(new Rectangle(10, 70, 60, 40), "left", new MouseDown(MoveLeft));

                Button cameraRotateRight = new Button(new Rectangle(155, 70, 60, 40), "right", new MouseDown(MoveRight));

                Button cameraZoomIn = new Button(new Rectangle(75, 55, 75, 30), "zoom in", new MouseDown(ZoomIn));

                Button cameraZoomOut = new Button(new Rectangle(75, 90, 75, 30), "zoom out", new MouseDown(ZoomOut));

                Button cameraMoveUp = new Button(new Rectangle(75, 10, 75, 40), "Up", new MouseDown(MoveUp));

                Button cameraMoveDown = new Button(new Rectangle(75, 125, 75, 40), "Down", new MouseDown(MoveDown));

                AddControl(camara);
                AddControl(misileCamera);
                AddControl(sound);
                AddControl(menu);
                AddControl(playerLife);
                AddControl(cpuLife);
                AddControl(playerTurn);
                AddControl(playerTurnDown);
                AddControl(panelRight);
                AddControl(downPanel);
                AddControl(lblPlayer);
                AddControl(lblCPU);
                AddControl(lblFleet);
                AddControl(cameraRotateLeft);
                AddControl(cameraRotateRight);
                AddControl(cameraZoomIn);
                AddControl(cameraZoomOut);
                AddControl(cameraMoveUp);
                AddControl(cameraMoveDown);

                #endregion
            }
        }

        #region Game Screen Functions

        public void Reset()
        {
            winnerCount = 180; 
        }

        void Computer()
        {
            count++;
            //generate a wait time when the player misses a shot
            if (count == 100 && manager.Game.CurrentPlayer == Player.Person)
            {
                count = 0;
                state = 1;
            }
            else
            //generate a wait time when the CPU misses a shot
            if (count == 100 && manager.Game.CurrentPlayer == Player.Computer)
            {
                state = 0;
                count = 0;
                ComputerResponse();
            }
        }

        void Change()
        {
            count++;
            if (count == 110)
            {
                if (base.manager.Game.CurrentPlayer == Player.Computer)
                {
                    //--si la computadora acerto un tiro y vuelve a tirar---
                    state = 0;
                    count = 0;
                    ComputerResponse();
                }
                else
                {
                    //--si la computadora fallo y me toca a mi---
                    state = 0;
                    count = 0;
                    manager.Escena.DeleteUnused();   
                }
            }
        }

        void ComputerResponse()
        {
            AICoordenada tiro = base.manager.Game.ComputerAi.AICoordinate();
            PlayerShoot(tiro.x, tiro.y, base.manager.Game.CurrentPlayer);
        }

        /// <summary>
        /// This function performs all the logic when a player (CPU or human)
        /// make a shot to the other
        /// <summary>
        void PlayerShoot(int x, int y, Player player)
        {
            if (player == Player.Person)//si es el jugador
            {
                int ship = base.manager.Game.GetCoordenada(x, y, 1);
                if (ship != 0) //si di en el blanco
                {
                    manager.Escena.AddModel(new ExplosionSquare(x, y, 1, Player.Computer));
                    manager.Escena.AddModel(new ExplosionWave(x, y, Player.Computer, 1));
                    manager.Escena.AddGenerator(new FireGenerator(new Coordenada2D(x, y), Player.Computer));
                    manager.Game.SetCoordenada(x, y, 9, 1);
                    manager.Game.SetHit(Player.Computer, ship);
                    if (manager.Game.ShipsComputer[Game.Convertir(ship)].GetLife == 0)
                    {
                        int shipPos = (int)manager.Game.ShipsComputer[Game.Convertir(ship)].Posicion;
                        int shipSize = manager.Game.ShipsComputer[Game.Convertir(ship)].Size;
                        Coordenada2D coord2D = manager.Game.ShipsComputer[Game.Convertir(ship)].Coordenada;
                        base.manager.Escena.AddGenerator(new SmokeGenerator(shipSize, shipPos, coord2D, 1));
                    }
                    //AudioPlayback.PlayAny("explosion.wav", "misileExplode.wav");  
                    // si no queda ningun barco enemigo, (gané)
                    if (manager.Game.RemainingShips(Player.Computer) == 0)
                    {
                        base.manager.AddText(Game.GameStrings.GetString("Victory"), Globals.ScreenWidthOver2, Sprite.SpriteFont, Color.Blue, Globals.ScreenWidthOver2);
                        base.manager.Game.Winner = Player.Person;
                        base.manager.Game.FinishMessage = Game.GameStrings.GetString("WinMessage");
                        Game.Finished = true;  
                    }
                }
                else // si di en el agua
                {
                    manager.Escena.AddModel(new ExplosionSquare(x, y, 0, Player.Computer));
                    manager.Escena.AddModel(new ExplosionWave(x, y, Player.Computer, 0));
                    manager.Game.SetCoordenada(x, y, 7, 1);
                    //AudioPlayback.Play("waterexplosion.wav");
                    manager.Game.CurrentPlayer = Player.Computer;
                    state = 1;
                    manager.AddText(Game.GameStrings.GetString("CPUTurn"), 85, Sprite.SpriteFont, Color.Red, Globals.ScreenWidthOver2);     
                }
            }
            else //si es la computadora
            {
                int ship = base.manager.Game.GetCoordenada(x, y, 0);
                if (ship != 0 && ship != 7 && ship != 9)
                {
                    // si me sonó la cacharra
                    manager.Escena.AddModel(new ExplosionSquare(x, y, 1, 0));
                    manager.Escena.AddModel(new ExplosionWave(x, y, 0, 1));
                    manager.Escena.AddGenerator(new FireGenerator(new Coordenada2D(x, y), 0));
                    manager.Game.SetCoordenada(x, y, 9, 0);
                    manager.Game.SetHit(Player.Person, ship);
                    if (manager.Game.ShipsPlayer[Game.Convertir(ship)].GetLife == 0)
                    {
                        int shipPos = (int)manager.Game.ShipsPlayer[Game.Convertir(ship)].Posicion;
                        int shipSize = manager.Game.ShipsPlayer[Game.Convertir(ship)].Size;
                        Coordenada2D coord2D = manager.Game.ShipsPlayer[Game.Convertir(ship)].Coordenada;
                        base.manager.Escena.AddGenerator(new SmokeGenerator(shipSize, shipPos, coord2D, 0));
                    }
                    //AudioPlayback.PlayAny("explosion.wav", "misileExplode.wav");
                    manager.Game.ComputerAi.LastHit(true);
                    // si no me queda ningun barco, (perdí)
                    if (manager.Game.RemainingShips(Player.Person) == 0)
                    {
                        base.manager.AddText(Game.GameStrings.GetString("Defeat"), 220, Sprite.SpriteFont, Color.Red, Globals.ScreenWidthOver2);
                        base.manager.Game.Winner = Player.Person;
                        base.manager.Game.FinishMessage = Game.GameStrings.GetString("LooseMessage");
                        Game.Finished = true;
                    }
                    else
                    {
                        state = 2;
                    }
                }
                else
                {
                    // si se fue en blanco
                    manager.Escena.AddModel(new ExplosionSquare(x, y, 0, 0));
                    manager.Escena.AddModel(new ExplosionWave(x, y, 0, 0));
                    manager.Game.SetCoordenada(x, y, 7, 0);
                    //AudioPlayback.Play("waterexplosion.wav");
                    manager.Game.ComputerAi.LastHit(false);
                    manager.Game.CurrentPlayer = Player.Person;
                    state = 1;
                    manager.AddText(Game.GameStrings.GetString("PlayerTurn"), 85, Sprite.SpriteFont, Color.Red, Globals.ScreenWidthOver2);
                }
            }
        }

        #endregion

        public void Update(Cursor cursor)
        {
            base.manager.Camara.Update();

            #region Show routines

            if (Showed == false)
            {
                Showed = true;
                if (FirstShowed == false)
                {
                    FirstShowed = true;
                    Camara.LocateCamara(new Vector3(10.5f, 7, 23), new Vector3(10.5f, 0, 7.5f ));
                }
                else
                {
                    base.manager.Camara.RestoreCamaraPos(0);
                }
            }

            #endregion

            #region keyboard routines

            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.A) )
                Camara.Move = CameraMove.Left;      

            if (kb.IsKeyDown(Keys.D))
                Camara.Move = CameraMove.Right;

            if (kb.IsKeyDown(Keys.Escape) ) //escape key
            {
                //Sound.StopLoop("cameraRotate.wav");  
                base.EnterMenuScreen();
                return;
            }
//#if DEBUG
            if (kb.IsKeyDown(Keys.R))
            {
                base.EnterWinScreen();
                return;
            }
//#endif
            if (kb.IsKeyDown(Keys.E))
            {
                if (shipsShowed == true)
                {
                    shipsShowed = false;
                }
                else
                {
                    shipsShowed = true;
                }
                base.manager.Game.ShowComputerShips(shipsShowed);
            }

            #endregion

            #region mouse routines

            MouseState m = manager.Cursor.MouseState;

            if (m.ScrollWheelValue != deltaScroll)
            {
                Camara.Scrolling = true;  
                int delta = deltaScroll - m.ScrollWheelValue;
                if (delta < 0)
                    Camara.Move = CameraMove.ZoomIn;
                else
                    Camara.Move = CameraMove.ZoomOut;
                deltaScroll = m.ScrollWheelValue; 
            }   

            Point point = manager.Cursor.Position;
            if (viewArea.Contains(point))
            {
                boardCount++;
                if (boardCount == 5)
                {
                    boardCount = 0;
                    if (manager.Escena.BoardMouse(point.X, point.Y) == true && base.manager.Game.CurrentPlayer == Player.Person)
                    {
                        manager.Cursor.CurrentCursor = ECursor.Aim;
                    }
                    else
                    {
                        manager.Cursor.CurrentCursor = ECursor.Arrow;
                    }
                }
                if (manager.Cursor.MouseState.LeftButton == ButtonState.Pressed)
                {
                    Coordenada2D shoot = base.manager.Escena.BoardSelection(point.X, point.Y);

                    if (shoot.X != -1)
                    {
                        int boardCell = base.manager.Game.GetCoordenada(shoot.X, shoot.Y, 1);
                        if (base.manager.Game.CurrentPlayer == Player.Person && boardCell != 7 && boardCell != 9
                          && boardCell != 11 && Misile.Locked == false && Game.Finished == false && Camara.IsShaking == false)
                        {
                            //AudioPlayback.Play("misile1.wav");
                            Coordenada2D inicial = manager.Game.GetLaunchCoord();
                            manager.Escena.AddModel(new Misile(inicial, shoot, 'P', PlayerShoot));
                        }
                    }
                }
            }
            else
            {
                manager.Cursor.CurrentCursor = ECursor.Arrow;
            }           

            #endregion

            #region Cambio de turno

            switch (state)
            {
                case 1:
                        Change();
                        break;
                case 2:
                        Computer();
                        break;
                default:
                    break;
            }

            #endregion

            #region Ganador

            if (base.manager.Game.Winner != Player.None)
            {
                winnerCount--;
                if (winnerCount == 0)
                {
                    base.EnterWinScreen();
                }
            }
           
            #endregion

            base.manager.Escena.Update();
            base.manager.Escena.UpdateShipModels();   
            base.UpdateControls(cursor);
        }

        public void Quit()
        { }

        public void Draw()
        {
            //Sprite.Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            //Sprite.BasicEffect.FogEnabled = true;
            //Sprite.BasicEffect.FogColor = Color.CornflowerBlue.ToVector3();
            //Sprite.BasicEffect.FogStart = 15;
            //Sprite.BasicEffect.FogEnd = 25;

            manager.Escena.Draw();

            //Sprite.BasicEffect.FogEnabled = false;    

            base.DrawControls();
            manager.Escena.DrawModel();   
        }
    }
}
