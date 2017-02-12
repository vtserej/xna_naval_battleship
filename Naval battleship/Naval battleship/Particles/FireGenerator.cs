using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace NavalBattleship.Particles
{
    class FireGenerator : ParticleEngine
    {
        Player player;
        float x, y;
        static bool stop;

        public static bool StopFire
        {
            get
            {
                return stop; 
            }
            set
            {
                stop = value; 
            }
        }

        public FireGenerator(Coordenada2D coordenada, Player pPlayer)
        {
            base.coordinate = new Vector3(coordenada.X, 0, coordenada.Y);
            player = pPlayer;  
        }

        public override void GenerateParticle()
        {
            if (base.death == false)
            {
                count++;
                if (count == 8)
                {
                    x = coordinate.X;
                    y = coordinate.Z;
                    AddParticle(new Fire(x, 0, y, player));
                    count = 0; 
                    life++;
                }
                if (life == 64)
                {
                    base.death = true;
                }
            }       
        }
    }
}
