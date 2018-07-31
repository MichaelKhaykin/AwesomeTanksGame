using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace AwesomeTanksGame
{
    class StatsButton : Button
    {
        public List<Sprite> StarTextures { get; set; }
        public TextLabel Label { get; set; }
        private string baseText;

        public StatsButton(Texture2D texture, Vector2 position, Vector2 scale, TextLabel label, Sprite star, string baseText) 
            : base(texture, position, Color.White, scale, null)
        {
            StarTextures = new List<Sprite>
            {
                star
            };
            Label = label;
            this.baseText = baseText;
        }
         
        public void Update(MouseState mouse, MouseState oldMouse, GameTime gameTime, GraphicsDevice graphicsDevice = null)
        {
            if (Enabled && IsClicked(mouse) && !IsClicked(oldMouse) && StarTextures.Count < 4)
            {
                var lastStar = StarTextures[StarTextures.Count - 1];
                StarTextures.Add(new Sprite(lastStar.Texture, new Vector2(lastStar.Position.X + lastStar.ScaledWidth, lastStar.Position.Y), Color.White, lastStar.Scale));
                Label.Text = $"{baseText} {StarTextures.Count}";
            }
            base.Update(gameTime, graphicsDevice);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Label.Draw(spriteBatch);
            for (int i = 0; i < StarTextures.Count; i++)
            {
                StarTextures[i].Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }

    class StatsScreen : Screen
    {
        List<StatsButton> TankStats = new List<StatsButton>();
        
        MouseState oldMouse;

        public StatsScreen(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            SpriteFont font = Content.Load<SpriteFont>("TextFont");
            
            var screenWidth = graphics.Viewport.Width;
            var screenHeight = graphics.Viewport.Height;


            var tankShieldTexture = Content.Load<Texture2D>("StatsScreenAssets/shield");
            var damageTexture = Content.Load<Texture2D>("StatsScreenAssets/damage");

            Texture2D starTexture = Content.Load<Texture2D>("StatsScreenAssets/star");
            var starScale = Main.SpriteScales[starTexture.Name];


            List<Sprite> stars = new List<Sprite>();
            int x = screenWidth / 2 - 400;

            for (int i = 0; i < 4; i++)
            {
                //We are using tankShieldTexture, doesn't matter which texture we use we just need a height (all
                //heights should be the same anyways....)
                stars.Add(new Sprite(starTexture, new Vector2(x, (screenHeight / 2) + tankShieldTexture.Height), Color.White, starScale.ToVector2()));
                x += 200;
            }

            var shieldLabel = new TextLabel(new Vector2(stars[0].Position.X, stars[0].Position.Y - stars[0].ScaledHeight + stars[0].ScaledHeight * 2), Color.Navy, $"Shield Level: 1", font)
            {
                IsVisible = false
            };
            var damageLabel = new TextLabel(new Vector2(stars[1].Position.X, stars[1].Position.Y - stars[1].ScaledHeight + stars[1].ScaledHeight * 2), Color.Navy, $"Turret Damage Level: 1", font)
            {
                IsVisible = false
            };
            //           var movementSpeedLabel = new TextLabel(new Vector2(), Color.White, $"Shield Level: 1", font);
            //            var bulletSpeedLabel = new TextLabel(new Vector2(), Color.White, $"Shield Level: 1", font);

            TankStats.Add(new StatsButton(tankShieldTexture, new Vector2(screenWidth / 2 - 400, screenHeight / 2), Main.SpriteScales[tankShieldTexture.Name].ToVector2(), shieldLabel, stars[0], "Shield Level:"));
            TankStats.Add(new StatsButton(damageTexture, new Vector2(screenWidth / 2 - 200, screenHeight / 2), Main.SpriteScales[damageTexture.Name].ToVector2(), damageLabel, stars[1], "Turret Damage Level:"));

            //tankMovementSpeed = new StatsButton(tankShieldTexture, new Vector2(screenWidth / 2 - 400, screenHeight / 2), Main.SpriteScales[tankShieldTexture.Name].ToVector2(), shieldLabel, stars[0]);
            //bulletSpeed = new StatsButton(tankShieldTexture, new Vector2(screenWidth / 2 - 400, screenHeight / 2), Main.SpriteScales[tankShieldTexture.Name].ToVector2(), shieldLabel, stars[0]);

            for (int i = 0; i < TankStats.Count; i++)
            {
                TankStats[i].Enabled = true;
                Sprites.Add(TankStats[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            for (int i = 0; i < TankStats.Count; i++)
            {
                TankStats[i].Update(mouse, oldMouse, gameTime);
            }
            oldMouse = mouse;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Changing font size without having to create a new spritefont
            for (int i = 0; i < TankStats.Count; i++)
            {
                var label = TankStats[i].Label;
                spriteBatch.DrawString(label.SpriteFont, label.Text, label.Position, label.Color, 0f, label.Origin, 1.5f, SpriteEffects.None, 0f);
            }
            base.Draw(spriteBatch);
        }
    }
}