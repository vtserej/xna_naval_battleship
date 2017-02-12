using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NavalBattleship
{
    public struct Coordenada2D
    {
        int x;
        int y;

        public Coordenada2D(int px, int py)
        {
            x = px;
            y = py;
        }

        public int X
        {
            get { return x; }
            set { x = value; }            
        }

        public int Y
        {
            get { return y; }

            set { y = value; } 
        }
    }

    public struct CoordenadaModel
    {
        float x;
        float y;

        public CoordenadaModel(float px, float py)
        {
            x = px;
            y = py;
        }

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }

            set { y = value; }
        }
    }

    public struct Pointer
    {
        public int x;
        public int y;
    }

    public struct AICoordenada
    {
        public int x;
        public int y;
        public int value;
    }

    struct MyOwnVertexFormat
    {
        private Vector3 position;
        private Vector2 texCoord;

        public MyOwnVertexFormat(Vector3 position, Vector2 texCoord)
        {
            this.position = position;
            this.texCoord = texCoord;
        }

        public static VertexElement[] VertexElements =
             {
                 new VertexElement(0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0),
                 new VertexElement(0, sizeof(float)*3, VertexElementFormat.Vector2, VertexElementMethod.Default, VertexElementUsage.TextureCoordinate, 0),
             };

        public static int SizeInBytes = sizeof(float) * (3 + 2);
    }
}
