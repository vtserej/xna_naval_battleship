using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NavalBattleship.Models
{
    public class SetupBoard : ModelClass, IModel
    {
        Texture2D texture;
        VertexPositionTexture[] pointList; 

        public new void Create()
        {
            texture = EngineContent.GetTextureByName("boardCell");

            pointList = new VertexPositionTexture[900]; // 150 celdas * 6 puntos cada una

            int index = 0;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 15; y++)
                {
                    pointList[index] = new VertexPositionTexture(
                    new Vector3(x, Game.BoardAltitude, y), new Vector2(0, 0));

                    pointList[index + 1] = new VertexPositionTexture(
                    new Vector3(x + 1, Game.BoardAltitude, y), new Vector2(1, 0));

                    pointList[index + 2] = new VertexPositionTexture(
                    new Vector3(x, Game.BoardAltitude, y + 1), new Vector2(0, 1));

                    pointList[index + 3] = new VertexPositionTexture(
                    new Vector3(x, Game.BoardAltitude, y + 1), new Vector2(0, 1));

                    pointList[index + 4] = new VertexPositionTexture(
                    new Vector3(x + 1, Game.BoardAltitude, y), new Vector2(1, 0));

                    pointList[index + 5] = new VertexPositionTexture(
                    new Vector3(x + 1, Game.BoardAltitude, y + 1), new Vector2(1, 1));

                    index += 6;
                }
            }
        }

        public void Update()
        { }

        public new void Draw()
        {
            Sprite.BasicEffect.TextureEnabled = true;
            Sprite.BasicEffect.Texture = EngineContent.GetTextureByName("boardCell");
            Sprite.BasicEffect.LightingEnabled = false;

            Sprite.BasicEffect.Begin();

            //Sprite.Graphics.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;      
            //Sprite.Graphics.GraphicsDevice.RenderState.CullMode = CullMode.None;

            foreach (EffectPass pass in Sprite.BasicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                Sprite.Graphics.GraphicsDevice.VertexDeclaration = new VertexDeclaration(Sprite.Graphics.GraphicsDevice,
                                            VertexPositionTexture.VertexElements);

                Sprite.Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(
                   PrimitiveType.TriangleList,
                   pointList,
                   0,  // vertex buffer offset to add to each element of the index buffer
                   300  // number of vertices in pointList
                   );

                pass.End();
            }

            Sprite.Graphics.GraphicsDevice.RenderState.FillMode = FillMode.Solid;

            Sprite.BasicEffect.End(); 
        }  
    }
}
