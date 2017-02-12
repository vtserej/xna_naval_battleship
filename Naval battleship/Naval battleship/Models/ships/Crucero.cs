using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace NavalBattleship.Models
{
    public class Crucero : Ship, IModel, IShipModel 
    {
        Matrix transform;

        public Crucero(Player player)
        {
            base.player = player;
            TextureShip();
            base.AccesibleName = Game.GameStrings.GetString("Cruiser");
            base.file = "crucero";  

            
            base.size = 4;
            base.sinkDirection = -1;  
            base.midSize = size / 2;
            base.scale = 0.26f;
            if (Layout.ScreenFormat == ScreenFormat.Format4X3)
            {
                base.modelShowRectangle = new Rectangle(895, 311, 124, 83);
            }
            else
            {
                base.modelShowRectangle = Layout.CalculateTotalLayout(new Rectangle(1151, 296, 124, 74));
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

        public new void Create()
        {
            base.Create();
            transform = modelMeshes.Meshes[0].ParentBone.Transform;  
        }

        public void Update()
        {
            UpdateShip();
            rotorAngle += 1f;
            if (rotorAngle > 3600)
            {
                //0.0027 es 1/360, el CPU multiplica 10 veces mas rapido que lo 
                //que divide el resultado es un angulo coterminal
                rotorAngle = rotorAngle * 0.00277f;
            }
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
