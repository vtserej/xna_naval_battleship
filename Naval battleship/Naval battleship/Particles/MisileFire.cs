using System;
using System.Collections.Generic;
using System.Text;
using NavalBattleship.GameCode; 
using Xengine;
using Microsoft.Xna.Framework;

namespace NavalBattleship.Particles
{
    public class MisileFire : Particle, IParticle 
    {
        static Vector3 coord; 
 
        public static Vector3 Coord
        {
            set { MisileFire.coord = value; }
        }
       
        public MisileFire(Vector3 pCoord)
        {
            coord = pCoord;
            base.textureID = EngineContent.GetTextureByName("fireMisile");
            base.scale = 0.7f;  
        }

        public bool IsDeath()
        {
            return base.death; 
        }

        public void Update()
        {
            if (coord.Y < -0.3f)
            {
                base.death = true;
            }  
        }

        public void Draw()
        {
            if (base.death == false)
            {
                Sprite.BasicEffect.View = Matrix.CreateScale(scale) *
                                          Matrix.CreateBillboard(coord, Camara.Position, Vector3.Up, Camara.ForwardVector) *
                                          Camara.View;
                DrawParticle(1);
                Sprite.BasicEffect.View = Camara.View;
            }               
        }
    }
}
