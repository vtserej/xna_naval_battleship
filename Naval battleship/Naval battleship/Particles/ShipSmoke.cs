using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework; 

namespace NavalBattleship.Particles
{
    public class ShipSmoke : Particle, IParticle   
    {
        float angleRot;
        int direction = Game.Alternator();
        Vector3 translation = Vector3.Zero;

        public ShipSmoke(Vector3 pCoord)
        {
            alpha -= (float)randomizer.NextDouble() * 0.1f;    
            this.position = pCoord;
            base.scale = (float)randomizer.NextDouble();
            base.textureID = EngineContent.GetTextureByName("shipSmoke");
            
            if (scale > 0.5f)
                angleRot += 90;
        }

        public bool IsDeath()
        {
            return base.death; 
        }

        public void Update()
        {
            //---Propiedades de la particula----
            time++;                       // vida
            alpha -= 0.008f ;              // intensidad
            scale += 0.035f;              // tamaño  
            angleRot += 1 * direction;    // rotacion 
            translation.Y += 0.025f;          //traslacion
            //-----------------------------------
        }

        public void Draw()
        {
            if (base.death == false)
            { 
                Sprite.BasicEffect.View = Matrix.CreateRotationZ(Helper.DegreeToRad(angleRot)) *
                                          Matrix.CreateScale(scale) *
                                          Matrix.CreateBillboard(position + translation, Camara.Position, Vector3.Up, Camara.ForwardVector) *
                                          Camara.View;
                DrawParticle(alpha);
                Sprite.BasicEffect.View = Camara.View;


                if (time == 120)
                {
                    base.death = true;
                }
            }
        }
    }
}
