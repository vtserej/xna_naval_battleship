using System;
using System.Collections.Generic;
using System.Text;

namespace NavalBattleship.Particles
{
    public interface IParticle
    {
        bool IsDeath();

        void Update();

        void Draw();
    }
}
