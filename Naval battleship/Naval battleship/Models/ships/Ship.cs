using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xengine;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NavalBattleship.Models
{
    public class Ship : ModelClass
    {  
        #region Variables de movimiento

        protected float angleproa;  //angulo de rot. con respecto a su largo
        protected float anglebabor; //angulo de rot. con respecto a su ancho
        protected int angleDirX;    //direccion de angleproa
        protected int angleDirZ;    //direccion de anglepopa
        protected float angleSpeedX;//velocidad de rotacion eje x
        protected float angleSpeedZ;//velocidad de rotacion eje z
        protected float midSize;    //mitad del tamaño del barco
        protected Matrix movementMatrix;

        #endregion Variables de Movimiento

        #region Variables de hundimiento

        protected bool sinked;       //hundido
        protected bool sinking;      //hundiendo 
        protected float sinkCount;   //se usa para hundir el barco
        protected float sinkLevel;
        protected float sinkDirection = 1;
        protected Matrix sinkMatrix = Matrix.Identity;
 
        #endregion

        #region otras variables

        static Manager manager;
        protected Rectangle modelShowRectangle;
        protected float modelShowAngle;
        protected bool visible = true;    //indica a si el barco es visible 
        protected Player player;       //indica si el barco es mio o de la computadora
        protected int hits;         //cantidad de golpes sostenidos
        protected float size;       //tamaño y vida del barco        
        protected Position posicionShip;     //posicion del barco
        protected float rotorAngle;
        protected Texture2D currentTexture;
        protected Texture2D textureBurn;
        protected static Random Randomizer = new Random();
        protected Coordenada2D coordenadaXY;  //posición del barco en la matriz de coordenadas
        protected int modelNameList;
        protected Matrix translationMatrix;
        protected Matrix rotationMatrix;
        protected Vector2 textPos = Vector2.Zero;

        #endregion
    
        #region Properties

        public static Manager Manager
        {
            set { Ship.manager = value; }
        }

        protected void TextureShip()
        {
            if (player == Player.Computer)
            {
                //textureID = EngineContent.GetTextureByName("barcosCPU.jpg");
            }
            else
            {
                //textureID = EngineContent.GetTextureByName("barcosPlayer.jpg");
            }
            currentTexture = textureID; 
        }

        public bool Visible
        {
            get
            {
                return visible; 
            }
            set
            {
                visible = value; 
            }
        }

        public bool Sinked
        {
            get
            {
                return sinked;
            }
        }

        public bool Sinking
        {
            get
            {
                return sinking;
            }
        }

        public Position Posicion
        {
            get
            {
                return posicionShip;
            }
            set
            {
                posicionShip = value;
                if (posicionShip == Position.Horizontal)
                {
                    rotationMatrix = Matrix.CreateRotationY(Helper.DegreeToRad(90));
                }
                else
                {
                    rotationMatrix = Matrix.CreateRotationY(0);
                }
            } 
        }

        public Coordenada2D Coordenada
        {
            get
            {
                return coordenadaXY;
            }
        }

        public Player PlayerOwner
        {
            get
            {
                return player;
            }
        }

        public int Size
        {
            get
            {
                return (int)size;
            }
        }

        public int GetLife
        {
            get
            {
                //retorna la vida en porciento %
                return (int)(((size - hits) * 100) / size);
            }
        }

        #endregion

        public Ship()
        {
            this.IsTarget = true;

            //randomizo la direccion de rotacion 
            angleDirX = Game.Alternator();
            angleDirZ = Game.Alternator();
            textureBurn = EngineContent.GetTextureByName("barcoquemao");
            currentTexture = textureID;
        }

        public void ResetShip()
        {
            if (player == Player.Computer)
            {
                visible = false; 
            }
            currentTexture = textureID; 
            sinked = false;
            sinking = false;
            sinkCount = 0;
            sinkLevel = 0;
            hits = 0;
            modelShowAngle = 0;
            movementMatrix = Matrix.Identity;
            sinkMatrix = Matrix.Identity;   
        }

        public void PosXY(int x, int y)
        {
            this.coordenadaXY.X  = x;
            this.coordenadaXY.Y = y;
            if (posicionShip == Position.Horizontal)
            {
                this.translationMatrix = Matrix.CreateTranslation(new Vector3(x + midSize, 0, y + 0.5f));
                base.position = new Vector3(x + midSize, 0, y + 0.5f);
            }
            else
            {
                this.translationMatrix = Matrix.CreateTranslation(new Vector3(x + 0.5f, 0, y + midSize));
                base.position = new Vector3(x + 0.5f, 0, y + midSize);
            }
            if (player == Player.Computer)
            {
                this.translationMatrix = Matrix.CreateTranslation(Game.BoardSeparation, 0, 0) * translationMatrix;      
            }
        }

        public void SetHit()
        {
            if (this.hits != size) 
            {
                this.hits++;
            }
            if (this.hits == size)
            {
                Camara.Explosion();  
                if (player == 0)
                {
                    manager.AddText(AccesibleName, 230, Sprite.SpriteFont, Color.Blue, Globals.ScreenWidthOver2);
                    //AudioPlayback.Play("alarm.wav");  
                }
                else
                {
                    manager.AddText(AccesibleName, 230, Sprite.SpriteFont, Color.Red, Globals.ScreenWidthOver2);
                }


                if (visible == false)
                {
                    visible = true;
                }
                currentTexture = textureBurn; 
                this.sinking = true;
                //esto es para que cuando se hunda no se mueva tanto
                angleSpeedX = angleSpeedX * 0.3f;
                angleSpeedZ = angleSpeedZ * 0.3f;
            } 
        }
       
        protected void DrawBase()
        {
            if (hits == size)
            {
                foreach (ModelMesh mesh in modelMeshes.Meshes)
                {
                    // This is where the mesh orientation is set, as well as our camera and projection.
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.World = mesh.ParentBone.Transform * Matrix.CreateFromYawPitchRoll(0, -MathHelper.PiOver2, 0);
                        effect.View = Camara.View;
                        effect.Projection = Camara.Projection;
                        effect.TextureEnabled = true;
                        effect.Texture = textureBurn;
                    }
                    // Draw the mesh, using the effects set above.
                    mesh.Draw();
                }
            }
            else
            {
                base.Draw(); 
            }
        }             

        protected void UpdateShip()
        {
            anglebabor += angleSpeedX * angleDirX;
            angleproa += angleSpeedZ * angleDirZ;

            if (anglebabor > 3 || anglebabor < -3)
            {
                angleDirX = angleDirX * -1;
            }
            if (angleproa > 1.35f || angleproa < -1.35f)
            {
                angleDirZ = angleDirZ * -1;
            }  
        }

        protected void UpdateShipModel()
        {
            if (player == Player.Computer)
            {
                if (!sinking && !sinked)
                {
                    modelShowAngle += 1;
                }
                else
                {
                    modelShowAngle = 180;
                }
            }
        }

        virtual public void CalculateSinkMatrix()
        {
            //lo rotate 80 degrees then sink
            if (sinkCount < 320)
            {        
                sinkMatrix = Matrix.CreateTranslation(0, -sinkLevel, 0);  
                sinkMatrix = Matrix.CreateRotationX(Helper.DegreeToRad(sinkDirection*(sinkCount * 0.25f)))* sinkMatrix;      
                sinkLevel += 0.005f; 
            }
            else
            {
                sinkMatrix = Matrix.CreateTranslation(0, -sinkLevel + (-sinkCount / 160) + 2, 0);
                sinkMatrix = Matrix.CreateRotationX(Helper.DegreeToRad(sinkDirection * 80)) * sinkMatrix;
                sinkLevel += 0.02f;
            }
            sinkCount++;
            if (sinkCount == 400)
            {
                //Sound.Stop(3); 
            }
            if (sinkCount == 730)
            {
                sinking = false;
                sinked = true; 
            }
        }

        protected new void Draw()
        {
            if (sinked == false && visible == true)
            {
                Camara.PushMatrix();

                movementMatrix = Matrix.CreateRotationX(Helper.DegreeToRad(angleproa)) * Matrix.CreateRotationZ(Helper.DegreeToRad(anglebabor));     
               
                if (sinking == true)
                    CalculateSinkMatrix(); 

                Camara.View = movementMatrix * sinkMatrix * Camara.View;
                DrawBase();
                Camara.PopMatrix();  
            }
        }

        protected void DrawModelName()
        {
            if (player == Player.Computer)
            {
                if (textPos == Vector2.Zero)
                {
                    textPos = new Vector2(modelShowRectangle.X + 4, modelShowRectangle.Y + (modelShowRectangle.Height - 20));
                }
                Sprite.SpriteBatch.DrawString(Sprite.SpriteFont, base.AccesibleName, textPos, Color.White);
            }
        }

        protected void DrawModel()
        {
            if (player == Player.Computer)
            {
                Camara.SelectedMatrix = CameraMatrixSelected.Projection;
                Camara.PushMatrix();
                Camara.SelectedMatrix = CameraMatrixSelected.View;
                Camara.PushMatrix();
                Camara.View = Matrix.CreateRotationY(Helper.DegreeToRad(modelShowAngle)) *
                              Matrix.CreateLookAt(new Vector3((size * 2) + 1, (size * 2) + 1, (size * 2) + 1), new Vector3(0, 0, 0), Vector3.Up);   

                Viewport original = Sprite.Graphics.GraphicsDevice.Viewport;
                Viewport modified = original;
                modified.Height = modelShowRectangle.Height;
                modified.Width = modelShowRectangle.Width;
                modified.X = modelShowRectangle.X;
                modified.Y = modelShowRectangle.Y;
                Camara.Projection = Matrix.CreatePerspectiveFieldOfView((float)Helper.PiOver4(), modified.AspectRatio, 1, 100);
                Sprite.Graphics.GraphicsDevice.Viewport = modified;
                DrawBase();
                Camara.PopMatrix();
                Camara.SelectedMatrix = CameraMatrixSelected.Projection;   
                Sprite.Graphics.GraphicsDevice.Viewport = original;
                Camara.PopMatrix();
                Camara.SelectedMatrix = CameraMatrixSelected.View;   
            }          
        }
    }
}