using Xengine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NavalBattleship.Models
{
    class SkySphere : ModelClass, IModel  
    {
        Effect SkySphereEffect;

        public SkySphere()
        {
            base.file = "sphereSky"; 
        }

        private TextureCube CreateSkyBox()
        {
            return EngineContent.GetTextureCubeByName("skybox02");  
        }

        public new void Create()
        {
            base.Create();

            SkySphereEffect = EngineContent.GetEffectByName("skySphere");  

            SkySphereEffect.Parameters["ViewMatrix"].SetValue(Camara.View);
            SkySphereEffect.Parameters["ProjectionMatrix"].SetValue(Camara.Projection);
            
            SkySphereEffect.Parameters["SkyboxTexture"].SetValue(CreateSkyBox());
            // Set the Skysphere Effect to each part of the Skysphere model
            foreach (ModelMesh mesh in modelMeshes.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = SkySphereEffect;
                }
            }         
        }

        public void Update()
        { }

        public new void Draw()
        {
            // Set the View and Projection matrix for the effect
            SkySphereEffect.Parameters["ViewMatrix"].SetValue(Matrix.CreateTranslation(-10.5f, 0, -7.5f) * Camara.View);
            SkySphereEffect.Parameters["ProjectionMatrix"].SetValue(Camara.Projection);
            // Draw the sphere model that the effect projects onto
            foreach (ModelMesh mesh in modelMeshes.Meshes)
            {
                mesh.Draw();
            }

            // Undo the renderstate settings from the shader
            Sprite.Graphics.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            Sprite.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
        } 
    }
} 
