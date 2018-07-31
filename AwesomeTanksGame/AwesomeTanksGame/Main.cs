using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MichaelLibrary;
using System.Collections.Generic;
using AwesomeTanksGame.Screens;
using Microsoft.Xna.Framework.Media;

namespace AwesomeTanksGame
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Dictionary<States, Screen> screens = new Dictionary<States, Screen>();

        public static States CurrentState;
        public static States PreviousState;
        
        public static Dictionary<string, float> SpriteScales = new Dictionary<string, float>();

        Texture2D backGroundTexture;

        public static bool ShouldPlaySoundsDuringGame = true;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backGroundTexture = Content.Load<Texture2D>("SetUpGameAssets/bg");
            
            InitDict();

            screens.Add(States.SetUp, new SetUpScreen(GraphicsDevice, Content));
            screens.Add(States.StatsScreen, new StatsScreen(GraphicsDevice, Content));

            // TODO: use this.Content to load your game content here
        }

        public void InitDict()
        {
            SpriteScales.Add("SetUpGameAssets/button_sound", 0.5f);
            SpriteScales.Add("SetUpGameAssets/button_play", 0.25f);
            SpriteScales.Add("SetUpGameAssets/button_music", 0.5f);
            SpriteScales.Add("StatsScreenAssets/star", 1f);
            SpriteScales.Add("StatsScreenAssets/damage", 0.5f);
            SpriteScales.Add("StatsScreenAssets/speed", 0.5f);
            SpriteScales.Add("StatsScreenAssets/shield", 0.5f);
            SpriteScales.Add("StatsScreenAssets/fogvisibility", 0.5f);
            SpriteScales.Add("StatsScreenAssets/window", 1f);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            screens[CurrentState].Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            //Has to be first, so everything draws over the background
            if (CurrentState != States.Game)
            {
                spriteBatch.Draw(backGroundTexture, new Vector2(0, 0), Color.White);
            }
            
            screens[CurrentState].Draw(spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
