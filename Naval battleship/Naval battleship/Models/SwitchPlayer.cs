using System;
using System.Collections.Generic;
using System.Text;
using NavalBattleship.GameCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xengine; 

namespace NavalBattleship.Models
{
    public class SwitchPlayer : ModelClass , IModel ,IShipModel 
    {
        Texture2D texturePlayer;
        Texture2D textureCPU;
        Rectangle areaRectangle;
        float angleRot;
        Game game;
        int updateFreq;


        public SwitchPlayer(Game game)
        {
            this.game = game;
            if (Layout.ScreenFormat == ScreenFormat.Format4X3)
            {
                areaRectangle = new Rectangle(901, 48, 110, 110);
            }
            else
            {
                areaRectangle = Layout.CalculateTotalLayout(new Rectangle(1157, 45, 110, 110));
            }
            
        }

        public new void Create()
        {   
            texturePlayer = EngineContent.GetTextureByName("logoPlayer"); 
            textureCPU = EngineContent.GetTextureByName("logoCPU");
            textureID = texturePlayer;
            base.file = "sphere";
            base.Create(); 
        }

        public void Update()
        {
            angleRot += 0.6f;
            updateFreq++;
            if (updateFreq == 15)
            {
                updateFreq = 0; 
                if (game.CurrentPlayer == Player.Person)
                {
                    textureID = texturePlayer;
                }
                else
                {
                    textureID = textureCPU;
                } 
            }     
        }

        public void UpdateModel()
        {
        }

        public void DrawName()
        {
        }

        public void DrawModel()
        {
            Camara.SelectedMatrix = CameraMatrixSelected.Projection;
            Camara.PushMatrix();
            Camara.SelectedMatrix = CameraMatrixSelected.View;
            Camara.PushMatrix();
            Camara.View = Matrix.CreateRotationY(Helper.DegreeToRad(angleRot)) *
                          Matrix.CreateScale(0.5f) *  
                          Matrix.CreateLookAt(new Vector3(5, 5, 5), new Vector3(0, 0, 0), Vector3.Up);

            Viewport original = Sprite.Graphics.GraphicsDevice.Viewport;
            Viewport modified = original;
            modified.Height = areaRectangle.Height;
            modified.Width = areaRectangle.Width;
            modified.X = areaRectangle.X;
            modified.Y = areaRectangle.Y;
            Camara.Projection = Matrix.CreatePerspectiveFieldOfView((float)Helper.PiOver4(), modified.AspectRatio, 1, 100);
            Sprite.Graphics.GraphicsDevice.Viewport = modified;
            foreach (ModelMesh mesh in modelMeshes.Meshes)
            {
                // This is where the mesh orientation is set, as well as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Alpha = 1; 
                    effect.World = mesh.ParentBone.Transform * Matrix.CreateFromYawPitchRoll(0, -MathHelper.PiOver2, 0);
                    effect.View = Camara.View;
                    effect.Projection = Camara.Projection;
                    effect.TextureEnabled = true;
                    effect.Texture = textureID; 
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
            Camara.PopMatrix();
            Camara.SelectedMatrix = CameraMatrixSelected.Projection;
            Sprite.Graphics.GraphicsDevice.Viewport = original;
            Camara.PopMatrix();
            Camara.SelectedMatrix = CameraMatrixSelected.View;   
        }

        public new void Draw()
        {
        }
    }
}
