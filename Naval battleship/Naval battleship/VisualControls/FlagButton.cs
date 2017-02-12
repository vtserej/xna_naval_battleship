using System;
using System.Collections.Generic;
using System.Text;
using Xengine.WindowsControls;
using NavalBattleship.Models;
using Xengine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace NavalBattleship.Controls
{
    public class FlagButton : Control, IControl
    {
        Flag flag;
        bool isPressed;
        Color borderColor = Color.White;
        int borderSize = 1;
        string text = "";
        Vector2 textSize;
        Vector2 textPosition = new Vector2(0, 0);

        public FlagButton(Rectangle rectangle, MouseClick onClick, Vector3 flagPos, string text, string textureFlag)
        {
            base.areaRectangle = rectangle;
            base.mouseClick = onClick;
            this.text = text;   
            Texture2D textureID = EngineContent.GetTextureByName(textureFlag);
            flag = new Flag(flagPos, new Vector3(1.1f, 1.7f, 2), textureID, rectangle);
            textSize = Sprite.SpriteFont.MeasureString(text);
            textPosition = new Vector2(areaRectangle.X + (areaRectangle.Width - textSize.X) / 2,
                                        areaRectangle.Y + areaRectangle.Height);
            base.fontColor = Color.WhiteSmoke;  
        }

        public string AccesibleName()
        {
            return base.accesibleName;
        }

        public bool Discard()
        {
            return base.discard;
        }

        public int ZOrder()
        {
            return base.zOrder;
        }

        public void Create()
        {
            flag.Create();
            flag.Update();  
        }

        public void Update(Cursor cursor)
        {
            if (areaRectangle.Contains(cursor.Position))
            {
                flag.Update();
                borderColor = Color.LightGray;
                if (cursor.MouseState.LeftButton == ButtonState.Pressed)
                {
                    isPressed = true;
                    borderColor = Color.White;
                }
                else
                    if (isPressed)
                    {
                        if (cursor.MouseState.LeftButton == ButtonState.Released)
                        {
                            isPressed = false;
                            borderColor = Color.LightGray;
                            if (mouseClick != null)
                            {
                                base.mouseClick.Invoke();
                            }
                        }
                    }
            }
            else
            {
                isPressed = false;
                borderColor = Color.Gray;
            }
        }

        public void Draw()
        {
            Sprite.DrawSpriteLine(areaRectangle, borderColor);

            flag.Draw();

            Sprite.SpriteBatch.DrawString(Sprite.SpriteFont, text, textPosition, Color.White);
        }
    }
}
