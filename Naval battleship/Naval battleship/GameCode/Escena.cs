using System.Collections.Generic;
using NavalBattleship.Models;
using NavalBattleship.Particles;
using Xengine;
using Microsoft.Xna.Framework;
using System;

namespace NavalBattleship
{
    public class Escena
    {
        List<IModel> models = new List<IModel>();
        List<IShipModel> gameModels = new List<IShipModel>();
        List<IModel> setupModels = new List<IModel>();
        List<IModel> iTargets = new List<IModel>();
        List<ModelClass> targets = new List<ModelClass>();
        List<IModel> tempModels = new List<IModel>();
        List<ParticleEngine> generator = new List<ParticleEngine>(15);
        List<IParticle> sorteable = new List<IParticle>(110);  
        bool updating, added;

#if DEBUG
        public int GetParticleCount()
        {
            return sorteable.Count;  
        }
#endif

        public void ClearEscena()
        {
            for (int i = 0; i < models.Count; i++)
            {
                if (models[i] is ExplosionSquare || models[i] is Misile || models[i] is ExplosionWave)
                {
                    models.RemoveAt(i);
                    i--;
                }
            }
            generator.Clear();
            sorteable.Clear();  
        }

        public void AddModel(ModelClass model)
        {
            IModel newModel = (IModel)model;
            if (model is IShipModel)
            {
                gameModels.Add((IShipModel)model);  
            }
            if (updating != true)
            {
                newModel.Create();
                models.Add(newModel);
            }
            else
            {
                added = true;
                newModel.Create();
                tempModels.Add(newModel); 
            }
        }

        public void AddSetupModel(ModelClass model)
        {
            IModel iModel = (IModel)model;
            if (model.IsTarget)
            {
                targets.Add(model);
                iTargets.Add(iModel);  
            }
            (iModel).Create();
            setupModels.Add(iModel);   
        }

        public void AddParticle(Particle particle)
        {
            for (int i = 0; i < sorteable.Count; i++)
            {
                if (sorteable[i].IsDeath())
                {
                    sorteable[i] = (IParticle)particle;
                    return;
                }
            }
            sorteable.Add((IParticle)particle);
        }

        public void AddGenerator(ParticleEngine pGenerator)
        {
            // añado la lista de particulas al generador 
            pGenerator.AddList(sorteable);
            // añado al generador a la lista de generadores
            generator.Add(pGenerator);
        }

        public void DeleteUnused()
        {
            // la utilizo para eliminar las particulas muertas de mi 
            // lista de particulas
            for (int i = 0; i < sorteable.Count; i++)
            {
                if (sorteable[i].IsDeath() == true)
                {
                    sorteable.RemoveAt(i);  
                }
            }
        }

        public void Update()
        {
            foreach (var item in sorteable)
            {
                item.Update();  
            }
            updating = true; 
            foreach (var item in models)
            {
                item.Update();
            }
            updating = false;
            if (added)
            {
                //si se inserto un elemento a la lista de objetos 
                //mientras se estaba pintando
                added = false;
                for (int i = 0; i < tempModels.Count; i++)
                {
                    models.Add(tempModels[i]);
                }
                tempModels.Clear();
            }
            // Invoca a los generadores de particulas
            foreach (var item in generator)
            {
                item.GenerateParticle();
            }
        }

        public void UpdateShipModels()
        {
            foreach (var item in gameModels)
            {
                item.UpdateModel();  
            }
        }

        public void UpdateSetup()
        {
            foreach (var item in setupModels)
            {
                item.Update();
            }
        }

        public void Draw()
        {
            foreach (var item in models)
            {
                item.Draw();
            }
            // Dibuja las particulas
            Sprite.Graphics.GraphicsDevice.RenderState.DepthBufferEnable = false;    
            foreach (var item in sorteable)
            {
                item.Draw();
            }
            Sprite.Graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;  
        }

        public void DrawModel()
        {
            //draw the ship
            foreach (var item in gameModels)
            {
                item.DrawModel();
            }
            //draw the name
            Sprite.SpriteBatch.Begin();
            foreach (var item in gameModels)
            {
                item.DrawName();
            }
            Sprite.SpriteBatch.End();
        }

        public void DrawSetup()
        {
            foreach (var item in setupModels)
            {
                item.Draw();  
            }
        }

        #region  Seleccion de objetos con el mouse

        Ray Selection(float mouseX, float mouseY)
        {
            Vector3 nearsource = new Vector3(mouseX, mouseY, 0f);
            Vector3 farsource = new Vector3(mouseX, mouseY, 1f);

            Matrix world = Matrix.CreateTranslation(0, 0, 0);

            Vector3 nearPoint = Sprite.Graphics.GraphicsDevice.Viewport.Unproject(nearsource, Camara.Projection, Camara.View, world);

            Vector3 farPoint = Sprite.Graphics.GraphicsDevice.Viewport.Unproject(farsource, Camara.Projection, Camara.View, world);

            Vector3 direction = farPoint - nearPoint;

            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        public string ShipSelection(float mouseX, float mouseY, out Player player)
        {
            Ray pickRay = Selection(mouseX, mouseY);   

            float distanceToScreen = -1;
            int index = -1;

            for (int i = 0; i < iTargets.Count; i++)
            {
                object distance = pickRay.Intersects((targets[i].ObjectBoundingSphere));
                if (distance != null)
                {
                    if ((float)distance > 0)
                    {
                        if ((float)distance > distanceToScreen)
                            index = i;
                    }
                }
            }
            if (index != -1)
            {
                player = Player.Person;
                return targets[index].AccesibleName;
            }
            else
            {
                player = Player.None;
                return string.Empty;
            }
        }

        public Coordenada2D BoardSelection(float mouseX, float mouseY)
        {
            Ray ray = Selection(mouseX, mouseY);

            object distance = ray.Intersects(new Plane(0, 1, 0, -Game.BoardAltitude));

            if (distance != null)
            {
                float X0 = ((float)distance * ray.Direction.X) + ray.Position.X - Game.BoardSeparation;
                float Z0 = ((float)distance * ray.Direction.Z) + ray.Position.Z;


                if (X0 >= 0 && X0 < 10 && Z0 >= 0 && Z0 < 15)
                {
                    return new Coordenada2D((int)X0, (int)Z0);
                }
            }

            return new Coordenada2D(-1, 0);
        }

        public bool BoardMouse(float mouseX, float mouseY)
        {
            Coordenada2D punto = BoardSelection(mouseX, mouseY);
            if (punto.X != -1)
                return true;

            return false; 
        }

        #endregion
    }
}
