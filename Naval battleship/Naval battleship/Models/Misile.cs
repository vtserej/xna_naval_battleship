using System;
using Xengine;
using NavalBattleship.GameCode;
using NavalBattleship.Screens;
using NavalBattleship.Particles;
using Microsoft.Xna.Framework; 

namespace NavalBattleship.Models
{
    class Misile : ModelClass , IModel  
    {
        #region Private atributes

        float time;
        float cTime;
        Coordenada2D cFinal, cInicial;
        char player;
        static bool locked;
        float angleX, angleY;
        float distanceTarget;
        float speed;
        static float x, y, z;
        static float cX, cY, cZ;
        static float vX, vY, vZ;
        double angleRotVert, angleRotHorz;
        float AngleRotMisil;
        PlayerShoot shoot;
        static bool misilCamera;
        static Manager manager; 
        
        #endregion

        #region Properties

        public static Manager GameManager
        {
            get { return Misile.manager; }
            set { Misile.manager = value; }
        }

        static public bool Locked
        {
            set
            {
                locked = value;
            }
            get
            {
                return locked;
            }
        }

        public static bool MisilCamera
        {
            get { return misilCamera; }
            set { misilCamera = value; }
        }

        #endregion

        public Misile(Coordenada2D inicial, Coordenada2D final, char pPlayer, PlayerShoot shoot)
        {
            this.shoot = shoot;
            this.file = "misil";
            this.cInicial = inicial;
            this.cFinal = final;
            this.player = pPlayer;
            locked = true;
            x = 0; y = 0; z = 0;
            CalculateCoord();
            Camara.MisileLocked = true;  
            if (misilCamera)
            {
                manager.Camara.SaveCamaraPos(1);
                Camara.LockedMisileCamera = true; 
            }
            else
            {
                manager.Escena.AddGenerator(new MisilSmokeGenerator(cInicial, cFinal, player, vX, vY, vZ));
                manager.Escena.AddParticle(new MisileFire(new Vector3(cInicial.X, 0, cInicial.Y)));
            }           
        }                                   
      
        public void CalculateCoord()
        {
            angleRotHorz = Helper.RadToDegree(Math.Atan2(cFinal.Y - cInicial.Y, cFinal.X + Game.BoardSeparation - cInicial.X));
            float result1 = cFinal.Y + 0.5f - (cInicial.Y + 0.5f);
            float result2 = Game.BoardSeparation + cFinal.X + 0.5f - (cInicial.X + 0.5f);
            angleX = Helper.RadToDegree(Math.Atan2(result1, result2));
            distanceTarget = (float)Math.Sqrt(Math.Pow(result1, 2) + Math.Pow(result2, 2));
            if (distanceTarget < 7)
                angleY = 75;
            else
                angleY = 65;

            result1 = distanceTarget * Game.Gravity;
            result2 = (float)Math.Sin(Helper.DegreeToRad(angleY * 2));
            speed = (float)Math.Sqrt(result1 / result2);
            vX = speed * (float)(Math.Cos(Helper.DegreeToRad(angleY)) * Math.Cos(Helper.DegreeToRad(angleX)));
            vY = speed * (float)Math.Sin(Helper.DegreeToRad(angleY));
            vZ = speed * (float)(Math.Cos(Helper.DegreeToRad(angleY)) * Math.Sin(Helper.DegreeToRad(angleX)));
        }
        
        public new void Create()
        {
            this.textureID = EngineContent.GetTextureByName("misil");
            base.Create();
        }

        public void Update()
        {
            if (canDispose == false)
            {
                x = vX * time + cInicial.X + 0.5f;
                y = vY * time - (0.5f * Game.Gravity * (float)Math.Pow(time, 2));
                z = vZ * time + cInicial.Y + 0.5f;
                float dist1 = x - cInicial.X + 0.5f;
                float dist2 = cFinal.X + Game.BoardSeparation - cInicial.X + 0.5f;
                angleRotVert = (150 * dist1 / dist2) + 5;
                AngleRotMisil += 3.5f;
                if (!misilCamera )
                {
                    time += 0.02f;
                }
                if (misilCamera)
                {
                    time += 0.01f;
                    cTime = time - 0.12f; 
                    cX = vX * cTime + cInicial.X + 0.5f;
                    cY = vY * cTime - (0.5f * Game.Gravity * (float)Math.Pow(cTime, 2));
                    cZ = vZ * cTime + cInicial.Y + 0.5f;
                    Camara.Yaw = (float)angleRotHorz;
                    Camara.Pitch = (float)angleRotVert;  
                    Camara.LocateCamara(new Vector3(cX, cY, cZ), new Vector3(x, y, z));
                }
                if (y < -0.3f)
                {
                    if (misilCamera)
                    {
                        manager.Camara.RestoreCamaraPos(1);
                    }
                    Camara.LockedMisileCamera = false;
                    Camara.MisileLocked = false;  
                    base.canDispose = true;
                    locked = false;
                    shoot.Invoke(cFinal.X, cFinal.Y, Player.Person);
                }    
            }         
        }

        public new void Draw()
        {
            if (canDispose == false )
            {

                Camara.PushMatrix();
                Camara.View =   Matrix.CreateTranslation(0,0.5f,0) *  
                                Matrix.CreateRotationY(Helper.DegreeToRad(AngleRotMisil)) *
                                                       Matrix.CreateRotationZ(Helper.DegreeToRad(-angleRotVert)) *
                                Matrix.CreateRotationY(Helper.DegreeToRad(-angleRotHorz)) *
                                Matrix.CreateTranslation(x, y, z) *

                Camara.View;
                base.Draw();
                Camara.PopMatrix();  
            }          
        } 
    }
}
