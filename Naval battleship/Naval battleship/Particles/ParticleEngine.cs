using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;  

namespace NavalBattleship.Particles
{
    abstract public class ParticleEngine
    {
        protected bool death;
        protected int count;
        protected int life;
        static protected List<IParticle> particles;
        protected Vector3 coordinate;
        protected static Random randomize = new Random();

        public bool Death
        {
            get
            {
                return death; 
            }
        }

        protected void AddParticle(IParticle particula)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                if (particles[i].IsDeath())
                {
                    particles[i] = particula;
                    return;
                }
            }
            particles.Add(particula);
        }

        public void AddList(List<IParticle> pList)
        {
            particles = pList;   
        }

        abstract public void GenerateParticle(); 
    }
}
