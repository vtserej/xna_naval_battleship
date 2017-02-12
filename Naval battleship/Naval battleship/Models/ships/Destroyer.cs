using System;
using System.Collections.Generic;
using System.Text;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework;
using Xengine; 

namespace NavalBattleship.Models
{
    class Destroyer : Ship, IModel, IShipModel  
    {
        public Destroyer(Player player)
        {
            base.player = player;
            TextureShip();
            base.AccesibleName = Game.GameStrings.GetString("Destroyer");
            base.file = "destructor";
            base.size = 3;
            base.midSize = size / 2;
            base.scale = 0.26f;
            if (Layout.ScreenFormat == ScreenFormat.Format4X3)
            {
                base.modelShowRectangle = new Rectangle(895, 477, 124, 83); 
            }
            else
            {
                base.modelShowRectangle = Layout.CalculateTotalLayout(new Rectangle(1151, 447, 124, 78)); 
            }     
            //la velocidad de rotacion es inversamente proporcional al tamaño (value)
            angleSpeedX = 0.5f / 8;
            angleSpeedZ = 0.25f / 8;

            //randomizo los angulos de posicion iniciales
            anglebabor = (float)(Randomizer.NextDouble() * 3 * angleDirX);
            angleproa = (float)(Randomizer.NextDouble() * 1.5f * angleDirX);

            if (player == Player.Computer)
            {
                //esto es para que no se vean los barcos de la computadora 
                visible = false;
            }
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
            base.DrawModel();  
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
