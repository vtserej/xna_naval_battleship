using System;
using System.Collections;
using System.Text;
using Xengine;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework; 

namespace NavalBattleship.Particles
{
    class SmokeGenerator : ParticleEngine  
    {
        float x, y;
        float boardSeparation;

        public SmokeGenerator(int size, int pos, Coordenada2D pCoord, int player)
        {
            //Tengo que generar el humo a lo largo de todo el barco por lo que necesito el 
            //tamaño (size) y la posicion del barco (pos) 
            base.coordinate = new Vector3(pCoord.X, 0, pCoord.Y);       
            if (player == 1)
            {
                boardSeparation = Game.BoardSeparation;   
            }
            if (pos == 1)
            {
                x = size; // 1 acostado
                y = 0.5f;
            }
            else
            {
                y = size; // 0 parado
                x = 0.5f;
            }
        }

        public override void GenerateParticle()
        {
            if (base.death == false)
            {
                count++;
                if (count == 18)
                {
                    float aX = boardSeparation + coordinate.X + (float)randomize.NextDouble() * x;
                    float aY = coordinate.Z + (float)randomize.NextDouble() * y;
                    if (y < 1)
                    {
                        aY += 0.25f;
                    }
                    if (x < 1)
                    {
                        aX += 0.25f;
                    }
                    Vector3 coordenada = new Vector3(aX, Game.BoardAltitude, aY);
                    AddParticle(new ShipSmoke(coordenada)); 
                    count = 0;
                    life++;
                }
                if (life == 17)
                {
                    FireGenerator.StopFire = false;  
                    base.death = true;
                }
            }       
        }
    }
}
