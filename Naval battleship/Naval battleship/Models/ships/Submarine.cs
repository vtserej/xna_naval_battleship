using System;
using System.Text;
using NavalBattleship.GameCode;
using Xengine;
using Microsoft.Xna.Framework;

namespace NavalBattleship.Models
{
    class Submarine : Ship , IModel, IShipModel  
    {
        static int count;
        int type;

        public Submarine(Player player) 
        {
            base.player = player;
            TextureShip();
            type = count;
            base.file = "submarino";
            if (count == 0)
            {
                base.AccesibleName = Game.GameStrings.GetString("Submarine") + " A";              
                count++;
            }
            else
            {
                base.AccesibleName = Game.GameStrings.GetString("Submarine") + " B";
                count = 0; 
            }
            base.size = 2;
            base.midSize = size / 2;
            base.scale = 0.3f;
            if (player == Player.Computer)
            {
                if (type == 0)
                    if (Layout.ScreenFormat == ScreenFormat.Format4X3)
                    {
                        base.modelShowRectangle = new Rectangle(895, 560, 124, 83);
                    }
                    else
                    {
                        base.modelShowRectangle = Layout.CalculateTotalLayout(new Rectangle(1151, 525, 124, 78));
                    }

                else
                    if (Layout.ScreenFormat == ScreenFormat.Format4X3)
                    {
                        base.modelShowRectangle = new Rectangle(895, 639, 124, 83);
                    }
                    else
                    {
                        base.modelShowRectangle = Layout.CalculateTotalLayout(new Rectangle(1151, 599, 124, 78));
                    }
                      
                visible = false;
            }

            //la velocidad de rotacion es inversamente proporcional al tamaño (value)
            angleSpeedX = 0.5f / 16;
            angleSpeedZ = 0.25f / 16;

            //randomizo los angulos de posicion iniciales
            anglebabor = (float)(Randomizer.NextDouble() * 3 * angleDirX);
            angleproa = (float)(Randomizer.NextDouble() * 1.5f * angleDirX);
        }

        public new void Create()
        {
            base.Create();
        }

        public void Update()
        {
            base.UpdateShip(); 
        }

        public void UpdateModel()
        {
            base.UpdateShipModel(); 
        }

        public void DrawName()
        {
            base.DrawModelName();
        }

        public new void DrawModel()
        {
            base.scale = 1;
            base.DrawModel();
            base.scale = 0.3f;
        }

        public new void Draw()
        {
            Camara.PushMatrix();
            Camara.View = Matrix.CreateScale(base.scale) * rotationMatrix * translationMatrix * Camara.View;
            base.Draw();
            Camara.PopMatrix();    
        }
    }
}
