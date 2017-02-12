using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NavalBattleship.Particles
{ 
    // Base class for particles
    public class Particle
    {
        protected Texture2D textureID;
        protected bool death;
        protected Vector3 position;
        protected static Random randomizer = new Random(); 
        protected int time;
        protected float alpha = 1;
        protected float scale = 1;
        protected static VertexPositionTexture[] pointList;
        protected const float timeElapse = 1f;

        static Particle()
        {
            CreateParticle(); 
        }

        static void CreateParticle()
        {
            pointList = new VertexPositionTexture[6];

            pointList[0] = new VertexPositionTexture(
                new Vector3(-0.5f, -0.5f, 0), new Vector2(0, 0));

            pointList[1] = new VertexPositionTexture(
                new Vector3(0.5f, -0.5f, 0), new Vector2(1, 0));

            pointList[2] = new VertexPositionTexture(
                new Vector3(-0.5f, 0.5f, 0), new Vector2(0, 1));

            pointList[3] = new VertexPositionTexture(
                new Vector3(-0.5f, 0.5f, 0), new Vector2(0, 1));

            pointList[4] = new VertexPositionTexture(
                new Vector3(0.5f, -0.5f, 0), new Vector2(1, 0));

            pointList[5] = new VertexPositionTexture(
                new Vector3(0.5f, 0.5f, 0), new Vector2(1, 1));


        }

        protected void DrawParticle(float alpha)
        {
            Sprite.BasicEffect.TextureEnabled = true;
            Sprite.BasicEffect.Texture = textureID;
            Sprite.BasicEffect.Alpha = alpha; 

            Sprite.BasicEffect.LightingEnabled = false;

            Sprite.BasicEffect.Begin();   

            foreach (EffectPass pass in Sprite.BasicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                Sprite.Graphics.GraphicsDevice.VertexDeclaration = Sprite.VertexDeclaration;

                Sprite.Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(
                    PrimitiveType.TriangleList,
                    pointList,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    2  // number of vertices in pointList
                    );
                pass.End();
            }

            Sprite.BasicEffect.End();

            Sprite.BasicEffect.Alpha = 1;
            Sprite.BasicEffect.LightingEnabled = true;
        }
    }
}
