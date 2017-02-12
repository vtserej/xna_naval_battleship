using System;
using System.Collections.Generic;
using System.Text;

namespace NavalBattleship.Particles
{
    public class ExplosionPiece: Particle ,IParticle    
    {
        public bool IsDeath()
        {
            return false;
        }

        public void Update()
        { }

        public void Draw()
        { }
    }
}
