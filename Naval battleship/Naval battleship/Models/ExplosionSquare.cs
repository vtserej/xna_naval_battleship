using System;
using Xengine;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NavalBattleship.Models
{
    class ExplosionSquare : ModelClass , IModel  
    {
        int x, y;
        string textureName;
        Player player;
        VertexPositionTexture[] pointList;
        Matrix translate;
        Vector3 color;

        public ExplosionSquare(int x, int y, int type, Player player)
        {
            this.x = x;
            this.y = y;
            this.player = player;
            textureName = "explosionBoard";
            switch (type)
            {
                case 0:
                    {
                        //green
                        color = new Vector3(0, 1, 0);
                        break;
                    }
                case 1:
                    {
                        //red
                        color = new Vector3(1, 0, 0);
                        break;
                    }
                default:
                    //blue
                    color = Color.SkyBlue.ToVector3();
                    break;
            }
        }

        public new void Create()    
        {
            if (player == Player.Computer)
            {
                translate = Matrix.CreateTranslation(Game.BoardSeparation, 0, 0);
            }
            else
                translate = Matrix.Identity;  

             textureID =  EngineContent.GetTextureByName(textureName);

            pointList = new VertexPositionTexture[6];

            pointList[0] = new VertexPositionTexture(
                   new Vector3(0, Game.BoardAltitude, 0), new Vector2(0, 0));

            pointList[1] = new VertexPositionTexture(
                new Vector3(1, Game.BoardAltitude, 0), new Vector2(1, 0));

            pointList[2] = new VertexPositionTexture(
                new Vector3(0, Game.BoardAltitude, 1), new Vector2(0, 1));

            pointList[3] = new VertexPositionTexture(
                new Vector3(0, Game.BoardAltitude, 1), new Vector2(0, 1));

            pointList[4] = new VertexPositionTexture(
                new Vector3(1, Game.BoardAltitude, 0), new Vector2(1, 0));

            pointList[5] = new VertexPositionTexture(
                new Vector3(1, Game.BoardAltitude, 1), new Vector2(1, 1));
        }

        public void Update()
        { }

        public new void Draw()
        {
            Sprite.BasicEffect.View = Matrix.CreateTranslation(x, 0.02f, y) *
                                      translate * 
                                      Camara.View;

            Sprite.BasicEffect.TextureEnabled = true;
            Sprite.BasicEffect.Texture = textureID;

            Sprite.BasicEffect.LightingEnabled = false;
            Vector3 diffuseColor = Sprite.BasicEffect.DiffuseColor;
            Sprite.BasicEffect.DiffuseColor = color;   

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

            Sprite.BasicEffect.View = Camara.View;

            Sprite.BasicEffect.DiffuseColor = diffuseColor; 
            
            Sprite.BasicEffect.End();

            Sprite.BasicEffect.LightingEnabled = true;
        }
    }
}
