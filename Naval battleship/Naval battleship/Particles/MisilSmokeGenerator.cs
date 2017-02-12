using System;
using System.Collections;
using System.Text;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework; 

namespace NavalBattleship.Particles
{
    class MisilSmokeGenerator : ParticleEngine 
    {
        char player;
        Coordenada2D cFinal, cInicial;
        float x, y, z;
        float vX, vY, vZ;
        int fireCount, smokeCount;
        float time = -0.03f;

        public MisilSmokeGenerator(Coordenada2D inicial, Coordenada2D final,
                                   char pPlayer, float pX, float pY, float pZ)
        {
            vX = pX; vY = pY; vZ = pZ;
            cInicial = inicial;
            cFinal = final;
            player = pPlayer;  
        }

        public override void GenerateParticle()
        {
            if (death == false)
            {
                if (fireCount > 0)
                {
                    x = vX * time + cInicial.X + 0.5f;
                    y = vY * time - (0.5f * Game.Gravity * (float)Math.Pow(time, 2));
                    z = vZ * time + cInicial.Y + 0.5f;
                    coordinate = new Vector3(x, y, z);
                    MisileFire.Coord = coordinate;
                }
                else
                {
                    fireCount++; 
                }
                if (smokeCount == 2)
                {
                    smokeCount = 0;
                    AddParticle(new MisileSmoke(coordinate));
                }            
                smokeCount++;
                time += 0.02f;
                if (y < -0.3f)
                {
                    base.death = true;
                }    
            }
        }
    }
}
