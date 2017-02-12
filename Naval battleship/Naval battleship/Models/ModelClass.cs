using System;
using System.Collections.Generic;
using System.Text;
using Xengine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NavalBattleship.Models
{
    abstract public class ModelClass
    {
        protected Texture2D textureID;
        private bool isTarget;
        protected bool canDispose;
        protected string file;
        protected float scale;
        private string accesibleName;
        protected Model modelMeshes;
        protected Vector3 position;
        protected Vector3 rotation;
        protected int meshIndexforSelection;

        public BoundingSphere ObjectBoundingSphere
        {
            get {
                ModelMesh m = modelMeshes.Meshes[meshIndexforSelection];
                return m.BoundingSphere.Transform(m.ParentBone.Transform * Matrix.CreateFromYawPitchRoll(0, -MathHelper.PiOver2, 0) *
                                        Matrix.CreateScale(scale) * Matrix.CreateTranslation(position));
            } 
        }

        public string AccesibleName
        {
            get { return accesibleName; }
            set { accesibleName = value; }
        }

        public bool IsTarget
        {
            get { return isTarget; }
            set { isTarget = value; }
        }

        protected void Create()
        {
            modelMeshes = EngineContent.GetModelByName(file);
            float mayor = -1;
            int index = 0;

            //calculates the mesh index with the bigger bounding sphere radius
            foreach (var item in modelMeshes.Meshes)
            {
                if (item.BoundingSphere.Radius > mayor)
                {
                    meshIndexforSelection = index;
                    mayor = item.BoundingSphere.Radius;
                }
                index++;
            } 
        }

        public virtual void Draw()
        {
            foreach (ModelMesh mesh in modelMeshes.Meshes)
            {
                // This is where the mesh orientation is set, as well as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                   
                    effect.EnableDefaultLighting();
                    effect.World = mesh.ParentBone.Transform * Matrix.CreateFromYawPitchRoll(0, -MathHelper.PiOver2, 0);
                    effect.View = Camara.View;
                    effect.Projection = Camara.Projection;
                    if (EngineContent.GetTextureByName(mesh.Name) != null)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = EngineContent.GetTextureByName(mesh.Name);
                    }
                    else
                    {
                        effect.TextureEnabled = false;   
                    }
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }
    }
}
