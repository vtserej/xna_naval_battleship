using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xengine;
using NavalBattleship.Controls;
using Microsoft.Xna.Framework;
using Xengine.WindowsControls;

namespace NavalBattleship.Controls
{
    public class Credits : Control, IControl
    {
        string[] text;
        Vector2 textPos;
        Vector2 textOrigin = new Vector2(0, startPos);
        const int startPos = -600;

        public Credits(string path)
        {
            text = RemoveEncryption(path);
            File.Delete(path + 'n');
            if (Layout.ScreenFormat == ScreenFormat.Format4X3)
            {
                areaRectangle = new Rectangle(100, 80, 700, 608);
            }
            else
            {
                areaRectangle = Layout.CalculateTotalLayout(new Rectangle(100, 80, 640, 608));  
            }

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

        public void Reset()
        {
            textOrigin.Y = startPos;
        }

        public void Create()
        {
            textPos.Y = 0; 
        }

        public void Update(Cursor cursor)
        {
            textOrigin.Y += 1.5f;
            if (textOrigin.Y > 1400)
            {
                Reset();
                textOrigin.Y = startPos - 300;
            }
        }

        public string[] RemoveEncryption(string path)
        {
            //encriptacion sumandole uno a todos los bytes
            byte[] array = File.ReadAllBytes(path);
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 0)
                {
                    array[i] = 255;
                }
                else
                {
                    array[i] = array[i] -= 1;
                }

            }
            List<string> lines = new List<string>();


            for (int i = 0; i < array.Length; i++)
            {
                string linea = string.Empty;

                byte letra = array[i];
              
                if (letra != 13)
                {
                    while (i < array.Length && array[i] != 13)
                    {
                        linea += (char)array[i];
                        i++;
                    }
                    if (linea.Length > 0)
                    {
                        lines.Add(linea);  
                    }
                }
            }
            return lines.ToArray();  
        }

        public void Draw()
        {
            Sprite.Graphics.GraphicsDevice.ScissorRectangle = areaRectangle;   
            Sprite.SpriteBatch.Begin();
            Sprite.Graphics.GraphicsDevice.RenderState.ScissorTestEnable = true;
            for (int i = 0; i < text.Length; i++)
            {
                Sprite.DrawCenterText(textPos, textOrigin, text[i], Sprite.SpriteFont);
                textPos.Y += 25; ;
            }
            textPos.Y = 0; 
            Sprite.Graphics.GraphicsDevice.RenderState.ScissorTestEnable = false;
            Sprite.SpriteBatch.End(); 
        }
    }
}
