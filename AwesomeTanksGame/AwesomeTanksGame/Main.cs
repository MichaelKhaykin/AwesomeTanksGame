using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MichaelLibrary;
using System.Collections.Generic;
using AwesomeTanksGame.Screens;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace AwesomeTanksGame
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Dictionary<States, Screen> screens = new Dictionary<States, Screen>();

        public static States CurrentState;
        public static States PreviousState;
        
        public static Dictionary<string, float> SpriteScales = new Dictionary<string, float>();

        Texture2D backGroundTexture;

        public static SoundEffectInstance buttonSoundClick;
        
        public static bool ShouldPlaySoundsDuringGame = true;
        
        MouseState oldMouse;

        TextLabel label;

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

            buttonSoundClick = Content.Load<SoundEffect>("MusicAndSoundEffects/buttonClickSound").CreateInstance();
            buttonSoundClick.IsLooped = false;

            SetUpEconomy();

            var spriteFont = Content.Load<SpriteFont>("TextFont");

            string costText = $"Money:${Economics.Money}";
            float y = spriteFont.MeasureString(costText).Y / 2;
            float x = spriteFont.MeasureString(costText).X;
            label = new TextLabel(new Vector2(x + GraphicsDevice.Viewport.Width - 350, y + 50), Color.Black, costText, spriteFont);

            InitDict();

            screens.Add(States.SetUp, new SetUpScreen(GraphicsDevice, Content));
            screens.Add(States.LevelSelect, new LevelSelectScreen(GraphicsDevice, Content));
            screens.Add(States.StatsScreen, new StatsScreen(GraphicsDevice, Content));

            // TODO: use this.Content to load your game content here
        }

        public void SetUpEconomy()
        {
            Economics.Money = 500;

            int value = 1000;

            int timesToLoop = 4;

            for (int i = 0; i < timesToLoop; i++)
            {
                Economics.HealthCosts.Add(value);
                value += 2000;
            }

            value = 5000;
            for (int i = 0; i < timesToLoop; i++)
            {
                Economics.SpeedCosts.Add(value);
                value += 2000;
            }

            value = 1000;
            for (int i = 0; i < timesToLoop; i++)
            {
                Economics.DeFoggerCosts.Add(value);
                value += 3000;
            }

            value = 3000;
            for (int i = 0; i < timesToLoop; i++)
            {
                Economics.DamageCosts.Add(value);
                value += 4000;
            }
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
            SpriteScales.Add("StatsScreenAssets/done", 1f);
            SpriteScales.Add("StatsScreenAssets/undo", 1f);
            SpriteScales.Add("LevelSelectScreen/header_levels", 1f);
            SpriteScales.Add("LevelSelectScreen/button_empty", 0.3f);
            for (int i = 0; i < 10; i++)
            {
                SpriteScales.Add($"LevelSelectScreen/{i}", 0.3f);
            }
        }

        public static BaseButton CreateButton(GraphicsDevice graphicsDevice, Texture2D box, Texture2D image, Vector2 position, Color boxColor, Color imageColor)
        {
            Vector2 boxSize = new Vector2(box.Width, box.Height);
            float boxScale = Main.SpriteScales[box.Name];
            Vector2 scaledBoxSize = boxSize * boxScale;

            Vector2 imageSize = new Vector2(image.Width, image.Height);
            float imageScale = Main.SpriteScales[image.Name];
            Vector2 scaledImageSize = imageSize * imageScale;

            RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, (int)scaledBoxSize.X, (int)scaledBoxSize.Y);
            SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);

            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();

            spriteBatch.Draw(box, Vector2.Zero, null, boxColor, 0f, Vector2.Zero, boxScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(image, (scaledBoxSize - scaledImageSize) / 2f, null, imageColor, 0f, Vector2.Zero, imageScale, SpriteEffects.None, 0f);

            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);

            float scale = 1f;
            return new BaseButton(renderTarget, position, Color.White, scale.ToVector2());
        }

        public static Button CreateButton(GraphicsDevice graphicsDevice, Texture2D box, SpriteFont font, string text, Vector2 position)
        {
            Vector2 boxSize = new Vector2(box.Width, box.Height);
            float boxScale = SpriteScales[box.Name];
            Vector2 scaledBoxSize = boxSize * boxScale;

            Vector2 textSize = font.MeasureString(text);

            RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, (int)scaledBoxSize.X, (int)scaledBoxSize.Y);
            SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);

            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();

            spriteBatch.Draw(box, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, boxScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, text, (scaledBoxSize - textSize) / 2, Color.White);

            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);

            float scale = 1f;
            return new Button(renderTarget, position, Color.White, scale.ToVector2(), null);
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouse = Mouse.GetState();

            label.Text = $"Money:${Economics.Money}";

            oldMouse = mouse;
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
                label.Draw(spriteBatch);
            }
            
            screens[CurrentState].Draw(spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
