using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace NavalBattleship.Models
{
    public class Cloud : ModelClass, IModel   
    {
        float angle;
        int direction = Game.Alternator();
        static Random randomizer = new Random();
        protected Vector3 scaleModel;

        public Cloud()
        {
            angle = ((float)randomizer.NextDouble() * 30 + 10) * direction;
            position = new Vector3(10, 15, -35);   
            base.textureID = EngineContent.GetTextureByName("cloud");
            scaleModel = new Vector3(15, 8, 1); 
        }
        
        public new void Create()
        {
          
        }

        public void Update()
        {
            angle += 0.02f * direction;
            if (angle < -60 || angle > 60)
            {
                direction = direction * -1;
            }
        }

        public new void Draw()
        {
            Sprite.BasicEffect.View = Matrix.CreateScale(scaleModel) * 
                                      Matrix.CreateBillboard(position, Camara.Position, Vector3.Up, Camara.ForwardVector) *
                                      Matrix.CreateRotationY(Helper.DegreeToRad(angle)) *
                                      Camara.View;

            DxHelper.DrawSquare(textureID);

            Sprite.BasicEffect.View = Camara.View;    
        }
    }
}
