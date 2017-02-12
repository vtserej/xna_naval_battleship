using System;
using System.Collections.Generic;
using System.Text;
using NavalBattleship.GameCode;
using Xengine;
using Xengine.WindowsControls; 
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework; 

namespace NavalBattleship.Controls
{
    public class LifeBar : Control, IControl  
    {
        int life;
        int percentLife;
        Game game;
        int updateCount = 5;
        Color color;
        Player player;
        Rectangle lifeRec;

        public LifeBar(Player player, Game game, Rectangle rect)
        {
            this.areaRectangle = rect;
            lifeRec = new Rectangle(areaRectangle.X + 1, areaRectangle.Y + 1,
                                    areaRectangle.Width - 2, areaRectangle.Height - 2); 
            this.player = player; 
            this.game = game; 
            zOrder = 0;
        }

        public string AccesibleName()
        {
            return base.accesibleName;
        }
        public bool Discard()
        {
            return false;
        }

        public int ZOrder()
        {
            return base.zOrder;
        }



        public void Create()
        { }

        public void Update(Cursor cursor)
        {
            updateCount++;
            if (updateCount == 6)
            {
                life = game.GetShipOverallState(player);
                percentLife = (life * (areaRectangle.Width - 2)) / 100;
                lifeRec.X = lifeRec.X + (lifeRec.Width - percentLife);      
                lifeRec.Width = percentLife;  

                #region Color selection

                if (life > 83)
                {
                    color = Color.Blue;
                }
                else
                {
                    if (life < 84 && life > 60)
                    {
                        color = Color.LimeGreen;
                    }
                    else
                    {
                        if (life < 61 && life > 30)
                        {
                            color = Color.Yellow;
                        }
                        else
                        {
                            if (life < 31)
                            {
                                color = Color.Red;
                            }
                        }
                    }
                }

                #endregion

                updateCount = 0; 
            }
        }

        public void Draw()
        {
            Sprite.DrawBoxFrame(areaRectangle, Color.White, Color.Black, 1);
            Sprite.DrawSprite(lifeRec, color);
        }
    }
}
