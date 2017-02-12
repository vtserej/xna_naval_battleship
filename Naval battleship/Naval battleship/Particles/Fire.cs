using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace NavalBattleship.Particles
{
    class Fire : Particle , IParticle  
    {
        float x, y;
        float angleRot;
        float transVert = -0.3f;
        float transHorz;
        float horzInc = 0.00025f; 
        bool begin = true;
        Vector3 translate;
        int direction;

        public Fire(float px, float py, float pz, Player player)
        {
            base.scale = 1;
            alpha = 0;
            alpha += (float)(randomizer.NextDouble() * 0.1);   
            direction = Game.Alternator();
            if (direction == 1)
            {
                transHorz = -0.09f;
            }
            else
            {
                transHorz = 0.09f;
            }
            x = px + 0.5f;
            y = pz + 0.5f + (float)(randomizer.NextDouble() * 0.1) * direction;
            angleRot = 55 * (float)randomizer.NextDouble();
            this.textureID = EngineContent.GetTextureByName("fire");
            if (player == Player.Computer)
            {
                x += Game.BoardSeparation;
            }
            position = new Vector3(x, 0, y); ;
            translate = new Vector3(transHorz, transVert, 0);   
        }

        public bool IsDeath()
        {
            return death; 
        }

        public void Update()
        {
            if (alpha < 0.35f && begin == true)
            {
                alpha += 0.04f;
                if (alpha > 0.35f)
                    alpha = 1 - (float)(randomizer.NextDouble() * 0.1);
            }
            else
            {
                begin = false;
                alpha -= 0.02f;
            }
            if (randomizer.Next(7) == 0)
            {
                horzInc = 0;
            }
            scale -= 0.017f * timeElapse; ;
            angleRot += 1 * timeElapse; ;
            transVert += 0.015f * timeElapse;
            transHorz += horzInc * direction * timeElapse; ;
            translate.X = transHorz;
            translate.Y = (transVert + 0.7f) * timeElapse; ; 
            time++;
        }

        public void Draw()
        {
            if (base.death == false  )
            {
                if (alpha > 0)
                {
                    Sprite.BasicEffect.View = Matrix.CreateScale(scale) *
                                              Matrix.CreateRotationZ(Helper.DegreeToRad(angleRot)) *
                                              Matrix.CreateBillboard(position + translate, Camara.Position, Vector3.Up, Camara.ForwardVector) *
                                              Camara.View;;
                    DrawParticle(alpha);
                    Sprite.BasicEffect.View = Camara.View;
                }
                else
                {
                    base.death = true;
                } 
            } 
        }
    }
}
