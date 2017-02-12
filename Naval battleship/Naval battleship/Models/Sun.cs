using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NavalBattleship.Models
{
    public class Sun : ModelClass, IModel 
    {
        float angleYaw;
        float anglePitch;
        float lightIntensity;
        Texture2D sunGlow;

        public Sun()
        {
            position = new Vector3(-10, 10, -45);
            anglePitch = -29;
            angleYaw = Helper.RadToDegree(Math.Atan2(10, 45));
        }

        public new void Create()
        {
            textureID = EngineContent.GetTextureByName("sun");
            sunGlow = EngineContent.GetTextureByName("sunGlow");
        }

        public void Update()
        {
            lightIntensity =  1 - Math.Abs(((angleYaw - Camara.Yaw) + (anglePitch - Camara.Pitch))/40);
            lightIntensity += 0.15f; 
            if (lightIntensity > 1)
            {
                lightIntensity = 1;
            }
        }

        public new void Draw()
        {

            Sprite.BasicEffect.View = Matrix.CreateScale(4) *
                                      Matrix.CreateBillboard(position, Camara.Position, Vector3.Up, Camara.ForwardVector) *
                                      Camara.View;

            DxHelper.DrawSquare(textureID);   

            Sprite.BasicEffect.Alpha = lightIntensity;

            Sprite.BasicEffect.View = Matrix.CreateTranslation(0, 0, -1) * Sprite.BasicEffect.View; 
            Sprite.BasicEffect.View = Matrix.CreateScale(4) * Sprite.BasicEffect.View;  
            
            
            
            DxHelper.DrawSquare(sunGlow);
            Sprite.BasicEffect.Alpha = 1;


            //Gl.glDisable(//Gl.GL_FOG);   
            //Gl.glPushMatrix();
            //Gl.glBlendFunc(//Gl.GL_SRC_ALPHA, //Gl.GL_ONE_MINUS_SRC_ALPHA);
            //Gl.glEnable(//Gl.GL_TEXTURE_2D);
            //Gl.glEnable(//Gl.GL_BLEND);  
            //Gl.glTranslatef(position.X, position.Y, position.Z);
            //Gl.glRotatef(-Camara.Yaw, 0, 1, 0);
            //Gl.glRotatef(-Camara.Pitch, 1, 0, 0);
            //Gl.glScalef(12, 12, 1);
            //Gl.glColor4f(1, 1, 1, lightIntensity);
            //Gl.glBindTexture(//Gl.GL_TEXTURE_2D, sunGlow);
            //Gl.glCallList(initList); 
            //Gl.glColor4f(1, 1, 1, 1);
            //Gl.glBindTexture(//Gl.GL_TEXTURE_2D, textureID);
            //Gl.glScalef(0.22f, 0.22f, 1);
            //Gl.glCallList(initList);
            //Gl.glDisable(//Gl.GL_BLEND);  
            //Gl.glPopMatrix();
            //Gl.glEnable(//Gl.GL_FOG);
        }
    }
}
