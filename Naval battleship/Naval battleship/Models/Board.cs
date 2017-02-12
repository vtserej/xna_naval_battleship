using System;
using System.Collections.Generic;
using System.Text;
using NavalBattleship.GameCode;
using Xengine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NavalBattleship.Models
{
    public class Board : ModelClass, IModel  
    {
        int[,] boardArray = new int[12, 17];
        Player player;
        VertexPositionTexture[] pointList; 
        Texture2D texture;
        Matrix boardTranslation = Matrix.CreateTranslation(Game.BoardSeparation, 0, 0);

        public Board()
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 17; j++)
                {
                    if (j == 0)
                    {
                        boardArray[i, j] = 10;
                    }
                    if (j == 16)
                    {
                        boardArray[i, j] = 10;
                    }
                    if (i == 0)
                    {
                        boardArray[i, j] = 10;
                    }
                    if (i == 11)
                    {
                        boardArray[i, j] = 10;
                    }
                }
            }
        }

        public Player Player
        {
            get 
            {
                return player;
            }
            set 
            {
                player = value; 
            }
        }

        public int[,] BoardArray
        {
            get
            {
                return boardArray;
            }
        }

        public bool Free(int x, int y, int size, int pos)
        {
            int[,] around = new int[size + 2, 3];
            bool answer = true;
            x++; y++;
            if (pos == 0)
            {
                for (int i = 0; i < size + 2; i++)
                {
                    around[i, 0] = boardArray[x - 1, y - 1 + i];
                    around[i, 1] = boardArray[x, y - 1 + i];
                    around[i, 2] = boardArray[x + 1, y - 1 + i];
                }
            }
            else
            {
                for (int i = 0; i < size + 2; i++)
                {
                    around[i, 0] = boardArray[x - 1 + i, y - 1];
                    around[i, 1] = boardArray[x - 1 + i, y];
                    around[i, 2] = boardArray[x - 1 + i, y + 1];
                }
            }

            for (int i = 0; i < size + 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (around[i, j] != 0 && around[i, j] != 10)
                    {
                        answer = false;  
                    }
                }  
            }
            return answer;
        }

        public List<Coordenada2D> Around(int x, int y, int size, Position pos)
        {
            List<Coordenada2D> returnList = new List<Coordenada2D>();
            if (pos == Position.Vertical)
            {
                if (boardArray[x, y - 1] == 0)
                {
                    returnList.Add(new Coordenada2D(x, y - 1));
                }
                if (boardArray[x, y + size] == 0)
                {
                    returnList.Add(new Coordenada2D(x, y + size));
                }
                for (int i = 0; i < size + 2; i++)
                {
                    if (boardArray[x - 1, y - 1 + i] == 0)
                    {
                        returnList.Add(new Coordenada2D(x - 1, y - 1 + i));   
                    }
                    if (boardArray[x + 1, y - 1 + i] == 0)
                    {
                        returnList.Add(new Coordenada2D(x + 1, y - 1 + i));
                    }
                }
            }
            else
            {
                if (boardArray[x - 1, y] == 0)
                {
                    returnList.Add(new Coordenada2D(x - 1, y));
                }
                if (boardArray[x + size, y] == 0)
                {
                    returnList.Add(new Coordenada2D(x + size, y));
                }
                for (int i = 0; i < size + 2; i++)
                {
                    if (boardArray[x - 1 + i, y - 1] == 0)
                    {
                        returnList.Add(new Coordenada2D(x - 1 + i, y - 1));
                    }
                    if (boardArray[x - 1 + i, y + 1] == 0)
                    {
                        returnList.Add(new Coordenada2D(x - 1 + i, y + 1));
                    }
                }
            }
            return returnList; 
        }

        public new void Create()
        {    
            texture = EngineContent.GetTextureByName("boardCell");

            pointList = new VertexPositionTexture[900]; // 150 celdas * 6 puntos cada una

            int index = 0;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 15; y++)
                {
                    //draw QUADS by triangle strip 

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

        public float Accuracy()
        {
            //devuelve la punteria de cada jugador 
            int shootCount = 0, MissCount = 0;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 17; j++)
                {
                    if (boardArray[i, j] == 9)
                    {
                        shootCount++;
                    }
                    else
                    {
                        if (boardArray[i, j] == 7)
                        {
                            MissCount++;
                        }
                    }
                }
            }
            float total = shootCount + MissCount;
            if (total == 0)
            {
                return 0;
            }
            return shootCount * 100 / total;
        }

        public new void Draw()
        {
            if (player == Player.Computer)
            {
                Sprite.BasicEffect.View = boardTranslation * Camara.View;
            }

            Sprite.BasicEffect.TextureEnabled = true;
            Sprite.BasicEffect.Texture = EngineContent.GetTextureByName("boardCell");
            Sprite.BasicEffect.LightingEnabled = false;

            Sprite.BasicEffect.Begin();

            foreach (EffectPass pass in Sprite.BasicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                Sprite.Graphics.GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Anisotropic;
                Sprite.Graphics.GraphicsDevice.SamplerStates[0].MagFilter= TextureFilter.Anisotropic;
                Sprite.Graphics.GraphicsDevice.SamplerStates[0].MipFilter  = TextureFilter.Linear;
                Sprite.Graphics.GraphicsDevice.SamplerStates[0].MaxAnisotropy = 16; 
                Sprite.BasicEffect.CommitChanges();


                Sprite.Graphics.GraphicsDevice.VertexDeclaration = Sprite.VertexDeclaration;  

                Sprite.Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(
                   PrimitiveType.TriangleList,
                   pointList,
                   0,  // vertex buffer offset to add to each element of the index buffer
                   300  // number of vertices in pointList
                   );
                
                pass.End();
            } 

            Sprite.BasicEffect.End();
            Sprite.BasicEffect.LightingEnabled = true;
            Sprite.BasicEffect.View = Camara.View;    
        }
    }
}
