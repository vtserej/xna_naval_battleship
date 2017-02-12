using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Xengine;
using Microsoft.Xna.Framework.Graphics;

namespace NavalBattleship.Models
{
    public class Flag : ModelClass, IModel
    {
        Vector3[,] points = new Vector3[maxX, maxY];	// The Array For The Points On The Grid Of Our "Wave"
        float hold;					                // Temporarily Holds A Floating Point Value     
        Vector3 scaleFlag;
        Texture2D textureFlag;
        VertexPositionTexture[] pointList;
        const int maxX = 30, maxY = 10;
        Rectangle flagRect;

        public Flag(Vector3 posFlag, Vector3 scaleFlag, Texture2D textureFlag, Rectangle flagRectangle)
        {
            base.position = posFlag;
            this.scaleFlag = scaleFlag;
            this.textureFlag = textureFlag;
            this.flagRect = flagRectangle;
        }

        public new void Create()
        {
            pointList = new VertexPositionTexture[maxX * maxY * 6];

            for (int x = 0; x < maxX; x++)
            {
                // Loop Through The Y Plane
                for (int y = 0; y < maxY; y++)
                {
                    // Apply The Wave To Our Mesh
                    points[x, y].X = x;
                    points[x, y].Y = y;
                    points[x, y].Z = (float)(Math.Sin(Helper.DegreeToRad(((x / 5.0f) * 30.0f))));
                }
            }
        }

        public void Update()
        {
            for (int y = 0; y < maxY; y++)			// Loop Through The Y Plane
            {
                hold = points[0, y].Z;			    // Store Current Value One Left Side Of Wave
                for (int x = 0; x < maxX - 1; x++)		// Loop Through The X Plane
                {
                    // Current Wave Value Equals Value To The Right
                    points[x, y].Z = points[x + 1, y].Z;
                }
                points[maxX - 1, y].Z = hold;			    // Last Value Becomes The Far Left Stored Value
                points[maxX - 1, y].Z = points[0, y].Z;
            }

            int index = 0;

            float float_x, float_y, float_xb, float_yb;		// Used To Break The Flag Into Tiny Quads   

            for (int x = 0; x < maxX - 1; x++)
            {
                // Loop Through The Y Plane
                for (int y = 0; y < maxY - 1; y++)
                {
                    float_x = (float)x / (maxX - 1);		// Create A Floating Point X Value
                    float_y = (float)y / (maxY - 1);		// Create A Floating Point Y Value
                    float_xb = (float)(x + 1) / (maxX - 1);		// Create A Floating Point Y Value+0.0227f
                    float_yb = (float)(y + 1) / (maxY - 1);		// Create A Floating Point Y Value+0.0227f

                    // first triangle
                    pointList[index].Position = points[x, y];
                    pointList[index + 1].Position = points[x, y + 1];
                    pointList[index + 2].Position = points[x + 1, y];

                    //second triangle
                    pointList[index + 3].Position = points[x, y + 1];
                    pointList[index + 4].Position = points[x + 1, y + 1];
                    pointList[index + 5].Position = points[x + 1, y];

                    //first triangle texture coordinates
                    pointList[index].TextureCoordinate.X = float_x;
                    pointList[index].TextureCoordinate.Y = float_y;

                    pointList[index + 1].TextureCoordinate.X = float_x;
                    pointList[index + 1].TextureCoordinate.Y = float_yb;

                    pointList[index + 2].TextureCoordinate.X = float_xb;
                    pointList[index + 2].TextureCoordinate.Y = float_y;

                    //second triangle texture coordinates
                    pointList[index + 3].TextureCoordinate.X = float_x;
                    pointList[index + 3].TextureCoordinate.Y = float_yb;

                    pointList[index + 4].TextureCoordinate.X = float_xb;
                    pointList[index + 4].TextureCoordinate.Y = float_yb;

                    pointList[index + 5].TextureCoordinate.X = float_xb;
                    pointList[index + 5].TextureCoordinate.Y = float_y;

                    index += 6;
                }
            }
        }

        public new void Draw()
        {
            Viewport original = Sprite.Graphics.GraphicsDevice.Viewport;
            Viewport modified = original;
            modified.Height = flagRect.Height;
            modified.Width = flagRect.Width;
            modified.X = flagRect.X;
            modified.Y = flagRect.Y;
            Sprite.Graphics.GraphicsDevice.Viewport = modified;

            Sprite.BasicEffect.Projection = Matrix.CreatePerspectiveFieldOfView((float)Helper.PiOver4(), 2, 1, 100);
            Sprite.BasicEffect.View = 
                                      Matrix.CreateTranslation(new Vector3(-maxX/(float)2, -maxY/(float)2, 0)) *
                                      Matrix.CreateScale(scaleFlag) * 
                                      Matrix.CreateLookAt(new Vector3(0, 0, 25), new Vector3(0, 0, 0), Vector3.Up);

            Sprite.BasicEffect.LightingEnabled = false;
            Sprite.BasicEffect.TextureEnabled = true;
            Sprite.BasicEffect.Texture = textureFlag;

            Sprite.BasicEffect.Begin();

            foreach (EffectPass pass in Sprite.BasicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                Sprite.Graphics.GraphicsDevice.VertexDeclaration = new VertexDeclaration(Sprite.Graphics.GraphicsDevice,
                          VertexPositionTexture.VertexElements);

                Sprite.Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(
                   PrimitiveType.TriangleList,
                   pointList,
                   0,  // vertex buffer offset to add to each element of the index buffer
                   maxX * maxY * 2  // number of vertices in pointList
                   );

                pass.End();
            }

            Sprite.BasicEffect.End();

            Sprite.BasicEffect.View = Camara.View;

            Sprite.BasicEffect.Projection = Camara.Projection;

            Sprite.Graphics.GraphicsDevice.Viewport = original;

            //Sprite.SpriteBatch.Begin();

            //Sprite.DrawSprite(flagRect, Color.Red);

            //Sprite.SpriteBatch.End();  
        }
    }
}
