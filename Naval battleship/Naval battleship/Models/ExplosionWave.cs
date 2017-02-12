using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NavalBattleship.Models
{
    class ExplosionWave : ModelClass, IModel
    {
        int x, y;
        Player player;
        float size = 0.1f;
        bool finish;
        float alpha = 1;

        public ExplosionWave(int x, int y, Player player, int type)
        {
            base.file = "sphere";
            this.x = x;
            this.y = y;
            this.player = player;
            if (type == 0)
            {
                base.textureID = EngineContent.GetTextureByName("imagenMar"); ;
            }
            else
            {
                base.textureID = EngineContent.GetTextureByName("explosion");
            }
        }

        public new void Create()
        {
            base.Create();
        }

        public void Update()
        {
            if (finish == false)
            {
                if (size < 1)
                {
                    size += 0.05f;
                    alpha -= 0.05f;
                }
                else
                {
                    finish = true;
                    base.canDispose = true;
                }
            }
        }

        public new void Draw()
        {
            if (finish == false)
            {
                Camara.PushMatrix(); // I came from openGl old school 

                if (player == Player.Computer)
                {
                    Camara.View = Matrix.CreateTranslation(Game.BoardSeparation, 0, 0) * Camara.View;
                }

                Camara.View = Matrix.CreateScale(size) *
                              Matrix.CreateTranslation(this.x + 0.5f, 0.01f, this.y + 0.5f) *
                              Camara.View;

                foreach (ModelMesh mesh in modelMeshes.Meshes)
                {
                    // This is where the mesh orientation is set, as well as our camera and projection.
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.LightingEnabled = false;  
                        effect.Alpha = alpha;
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
            }
        }
    }
}
