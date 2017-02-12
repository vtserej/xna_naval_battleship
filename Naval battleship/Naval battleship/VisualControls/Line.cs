using System;
using System.Collections.Generic;
using System.Text;
using Xengine.WindowsControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xengine;

namespace NavalBattleship.Controls
{
    public class Line : Control, IControl 
    {
        Color color = Color.White;

        public Line(Vector2 begin, Vector2 end, Color color, int lineWidth)
        {
            this.color = color;
            areaRectangle = new Rectangle((int)begin.X, (int)begin.Y, (int)Math.Abs(end.X - begin.X), lineWidth);         
        }
        
        public int ZOrder()
        {
            return base.zOrder; 
        }

        public bool Discard()
        {
            return true;
        }
        
        public string AccesibleName()
        {
            return base.accesibleName;
        }
        
        public void Create()
        {
            
        }
        
        public void Update(Cursor cursor)
        { }

        public void Draw()
        {
            Sprite.DrawSprite(areaRectangle, color);    
        }
}
}
