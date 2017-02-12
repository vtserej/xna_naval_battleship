using Xengine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace NavalBattleship.Models
{
    public class Sea : ModelClass, IModel  
    {
        VertexBuffer waterVertexBuffer;
        IndexBuffer waterIndexBuffer;
        VertexDeclaration myVertexDeclaration;
        Texture waterBumps;
        int waterWidth = 128;
        int waterHeight = 128;
        Effect effect;
        TextureCube skyboxTexture;

        public Sea()
        {
 
        }

        private VertexPositionTexture[] CreateWaterVertices()
        {
            VertexPositionTexture[] waterVertices = new VertexPositionTexture[waterWidth * waterHeight];

            int i = 0;
            for (int z = 0; z < waterHeight; z++)
            {
                for (int x = 0; x < waterWidth; x++)
                {
                    Vector3 position = new Vector3(x, 0, -z);
                    Vector2 texCoord = new Vector2((float)x / 30.0f, (float)z / 30.0f);

                    waterVertices[i++] = new VertexPositionTexture(position, texCoord);
                }
            }

            myVertexDeclaration = new VertexDeclaration(Sprite.Graphics.GraphicsDevice, VertexPositionTexture.VertexElements);

            return waterVertices;
        }

        private int[] CreateWaterIndices()
        {
            int[] waterIndices = new int[(waterWidth) * 2 * (waterHeight - 1)];

            int i = 0;
            int z = 0;
            while (z < waterHeight - 1)
            {
                for (int x = 0; x < waterWidth; x++)
                {
                    waterIndices[i++] = x + z * waterWidth;
                    waterIndices[i++] = x + (z + 1) * waterWidth;
                }
                z++;

                if (z < waterHeight - 1)
                {
                    for (int x = waterWidth - 1; x >= 0; x--)
                    {
                        waterIndices[i++] = x + (z + 1) * waterWidth;
                        waterIndices[i++] = x + z * waterWidth;
                    }
                }
                z++;
            }

            return waterIndices;
        }

        private void CreateBuffers(VertexPositionTexture[] vertices, int[] indices)
        {
            waterVertexBuffer = new VertexBuffer(Sprite.Graphics.GraphicsDevice, VertexPositionTexture.SizeInBytes * vertices.Length, BufferUsage.WriteOnly);
            waterVertexBuffer.SetData(vertices);

            waterIndexBuffer = new IndexBuffer(Sprite.Graphics.GraphicsDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            waterIndexBuffer.SetData(indices);
        }
        
        public new void Create()
        {
            VertexPositionTexture[] waterVertices = CreateWaterVertices();
            int[] waterIndices = CreateWaterIndices();
            CreateBuffers(waterVertices, waterIndices);

            waterBumps = EngineContent.GetTextureByName("waterbumps");
            effect = EngineContent.GetEffectByName("oceanwater");
            skyboxTexture = EngineContent.GetTextureCubeByName("skybox02");  
        }

        public void Update()
        {
        
        }
        
        public new void Draw()
        {
            float time = (float)Manager.Instance.GameTime.TotalGameTime.TotalMilliseconds / 1500.0f;
            Vector4 waveFreqs = new Vector4(1, 2, 0.5f, 1.5f);
            Vector4 waveHeights = new Vector4(0.3f, 0, 0.2f, 0.3f);
            Vector4 waveLengths = new Vector4(10, 5, 15, 7);

            Vector2[] waveDirs = new Vector2[4];
            waveDirs[0] = new Vector2(-1, 0);
            waveDirs[1] = new Vector2(-1, 0.5f);
            waveDirs[2] = new Vector2(-1, 0.7f);
            waveDirs[3] = new Vector2(-1, -0.5f);

            for (int i = 0; i < 4; i++)
                waveDirs[i].Normalize();

            effect.CurrentTechnique = effect.Techniques["OceanWater"];
            effect.Parameters["xWorld"].SetValue(Matrix.Identity * Matrix.CreateTranslation(-50, 0, 100));
            effect.Parameters["xView"].SetValue(Camara.View);
            effect.Parameters["xBumpMap"].SetValue(waterBumps);
            effect.Parameters["xProjection"].SetValue(Camara.Projection);
            effect.Parameters["xBumpStrength"].SetValue(0.5f);

            effect.Parameters["xCubeMap"].SetValue(skyboxTexture);
            effect.Parameters["xTexStretch"].SetValue(4.0f);
            effect.Parameters["xCameraPos"].SetValue(Camara.Position);
            effect.Parameters["xTime"].SetValue(time);

            effect.Parameters["xWaveSpeeds"].SetValue(waveFreqs);
            effect.Parameters["xWaveHeights"].SetValue(0);
            effect.Parameters["xWaveLengths"].SetValue(waveLengths);
            effect.Parameters["xWaveDir0"].SetValue(waveDirs[0]);
            effect.Parameters["xWaveDir1"].SetValue(waveDirs[1]);
            effect.Parameters["xWaveDir2"].SetValue(waveDirs[2]);
            effect.Parameters["xWaveDir3"].SetValue(waveDirs[3]);

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                Sprite.Graphics.GraphicsDevice.Vertices[0].SetSource(waterVertexBuffer, 0, VertexPositionTexture.SizeInBytes);
                Sprite.Graphics.GraphicsDevice.Indices = waterIndexBuffer;
                Sprite.Graphics.GraphicsDevice.VertexDeclaration = myVertexDeclaration;
                Sprite.Graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, waterWidth * waterHeight, 0, waterWidth * 2 * (waterHeight - 1) - 2);

                pass.End();
            }
            effect.End();
        }          
    }
}
