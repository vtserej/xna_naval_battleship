using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace NavalBattleship.Particles
{
    public class MisileSmoke : Particle ,IParticle    
    {
        public MisileSmoke(Vector3 pCoord)
        {
            position = pCoord;
            base.scale = 0.5f;
            base.textureID = EngineContent.GetTextureByName("smoke");   
        }

        public bool IsDeath()
        {
            return death; 
        }

        public void Update()
        {
            if (base.death == false)
            {
                time++;
                alpha -= 0.025f;
                scale += 0.03f;
                if (time == 39)
                {
                    base.death = true;
                }  
            }    
        }

        public void Draw()
        {
            if (base.death == false)
            {              
                Sprite.BasicEffect.View = Matrix.CreateScale(scale) *    
                                          Matrix.CreateBillboard(position,Camara.Position,Vector3.Up,Camara.ForwardVector) *
                                          Camara.View;      
                DrawParticle(alpha);
                Sprite.BasicEffect.View = Camara.View;      
            }  
        }
    }
}
