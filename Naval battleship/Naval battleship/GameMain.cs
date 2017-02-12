using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Xengine;
using Xengine.WindowsControls;

namespace NavalBattleship
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameMain : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Manager manager;
        static bool finished;
        static bool finishPrompt;

        public static bool FinishPrompt
        {
            get { return GameMain.finishPrompt; }
            set { GameMain.finishPrompt = value; }
        }

        public static bool Finished
        {
            get { return GameMain.finished; }
            set { GameMain.finished = value; }
        }

        void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        { 
            // If your game is running on a PC, use CheckDeviceMultiSampleType to query for antialiasing support. 

            // Many PCs support 4x antialiasing, and most will support 2x antialiasing. Once you have determined that 
            // antialiasing is supported, set the MultiSampleType 
            // and MultiSampleQuality properties appropriately, and then return from the event handler.

            int quality = 0;
            GraphicsAdapter adapter = e.GraphicsDeviceInformation.Adapter;
            SurfaceFormat format = adapter.CurrentDisplayMode.Format;
            // Check for 4xAA
            if (adapter.CheckDeviceMultiSampleType(DeviceType.Hardware, format,
                false, MultiSampleType.FourSamples, out quality))
            {
                // even if a greater quality is returned, we only want quality 0
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleQuality = 0;
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleType =
                    MultiSampleType.FourSamples;
            }
            // Check for 2xAA
            else
            if (adapter.CheckDeviceMultiSampleType(DeviceType.Hardware, format,
                false, MultiSampleType.TwoSamples, out quality))
            {
                // even if a greater quality is returned, we only want quality 0
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleQuality = 0;
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleType =
                    MultiSampleType.TwoSamples;
            }
            return;
        }

        public GameMain()
        {
#if DEBUG
            graphics = DxManager.XNAInit(this, "content", 1280, 720);
#endif

#if !DEBUG
            graphics = DxManager.XNAInit(this, "content");
#endif

            manager = new Manager();
            graphics.PreferMultiSampling = true;
         //   graphics.PreparingDeviceSettings +=
          //    new EventHandler<PreparingDeviceSettingsEventArgs>(Graphics_PreparingDeviceSettings);
#if !DEBUG
            this.graphics.IsFullScreen = true;
#endif

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            if (graphics.GraphicsDevice.DisplayMode.Height == 768 &&
              graphics.GraphicsDevice.DisplayMode.Width == 1024)
            {
                this.graphics.IsFullScreen = true;
            }
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Sprite.SpriteBatch = spriteBatch;
            Sprite.Graphics = graphics;
            Sprite.BasicEffect = new BasicEffect(graphics.GraphicsDevice, null);
            Sprite.BasicEffect.EnableDefaultLighting();   

            //effect load
            EngineContent.SetEffectList("effects\\");
            EngineContent.LoadEffects();          
            
            //texture load 
            EngineContent.SetTextureList("textures\\");
            EngineContent.LoadTextures();

            //texture cube load
            EngineContent.SetTextureCubeList("cubeMaps\\");
            EngineContent.LoadTextureCube();  

            //model load
            EngineContent.SetModelList("models\\");
            EngineContent.LoadModels();

            //cursor load
            Cursor cursor = new Cursor(); 
            cursor.AddCursor(ECursor.Aim, "aim");
            cursor.AddCursor(ECursor.Arrow, "custom");
            cursor.CurrentCursor = ECursor.Arrow;
            manager.Cursor = cursor;

            //font load
            SpriteFont spriteFont = Content.Load<SpriteFont>("fonts\\TextFont12");
            Sprite.SpriteFont = spriteFont;
            Sprite.Create();

            manager.CreateScreens();
            manager.Camara.InitCamara();
            manager.LoadModels();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non EngineContent content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (finished)
                Exit();
            if (finishPrompt)
            {
                finishPrompt = false;  
                manager.SelectedDialog = DScreen.LeaveScreen; 
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F4))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.RightAlt))
                {
                    manager.SelectedDialog = DScreen.LeaveScreen;
                }
            }

            // TODO: Add your update logic here
            manager.Update(gameTime);  

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            Sprite.Graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;

            // TODO: Add your drawing code here
            manager.Draw(); 

            base.Draw(gameTime);
        }
    }
}
