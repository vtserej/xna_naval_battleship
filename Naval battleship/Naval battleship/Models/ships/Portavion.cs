using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework; 

namespace NavalBattleship.Models
{
    class Portavion : Ship, IModel, IShipModel   
    {
        public Portavion(Player player)
        {
            base.player = player;
            TextureShip();
            base.AccesibleName = Game.GameStrings.GetString("Carrier");
            base.file = "portavion";
            base.size = 5;
            base.midSize = size * 0.5f;
            base.scale = 0.24f;
            if (Layout.ScreenFormat == ScreenFormat.Format4X3)
            {
                base.modelShowRectangle = new Rectangle(895, 228, 124, 83);
            }
            else
            {
                base.modelShowRectangle = Layout.CalculateTotalLayout(new Rectangle(1151, 214, 124, 78));
            }   

            //la velocidad de rotacion es inversamente proporcional al tamaño (value)
            angleSpeedX = 0.5f / 16;
            angleSpeedZ = 0.25f / 16;

            //randomizo los angulos de posicion iniciales
            anglebabor = (float)(Randomizer.NextDouble() * 3 * angleDirX);
            angleproa = (float)(Randomizer.NextDouble() * 1.5f * angleDirX);

            if (player == Player.Computer)
            {
                //esto es para que no se vean los barcos de la computadora 
                visible = false;
            }
        }

        public void DrawName()
        {
            base.DrawModelName();
        }

        public new void DrawModel()
        {
            base.DrawModel(); 
        }

        public new void Create()
        {
            currentTexture = textureID; 
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

        public new void Draw()
        {
            Camara.PushMatrix();
            Camara.View = Matrix.CreateScale(base.scale) * rotationMatrix * translationMatrix * Camara.View;
            base.Draw();
            Camara.PopMatrix();    
        }
    }
}
