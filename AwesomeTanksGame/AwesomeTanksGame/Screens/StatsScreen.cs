using AwesomeTanksGame.Screens;
using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace AwesomeTanksGame
{
    class StatsButton : BaseButton
    {
        public List<Sprite> StarTextures { get; set; }
        public TextLabel Label { get; set; }
        public int Cost { get; set; } = 1;
        public string Name { get; set; }
        
        public StatsButton(Texture2D texture, Vector2 position, Vector2 scale, TextLabel label, Sprite star, string name) 
            : base(texture, position, Color.White, scale)
        {
            StarTextures = new List<Sprite>
            {
                star
            };
            Label = label;
            Name = name;
        }
         
        public void Update(MouseState mouse, MouseState oldMouse, GameTime gameTime, ref StatsButton lastButtonClicked, GraphicsDevice graphicsDevice = null)
        {
            if (Enabled && IsClicked(mouse) && !IsClicked(oldMouse) && StarTextures.Count < 4 && Economics.Money - Cost >= 0)
            {
                lastButtonClicked = this;
                var lastStar = StarTextures[StarTextures.Count - 1];
                StarTextures.Add(new Sprite(lastStar.Texture, new Vector2(lastStar.Position.X + lastStar.ScaledWidth, lastStar.Position.Y), Color.White, lastStar.Scale));
                string[] array = Label.Text.Split(':');
                Label.Text = $"{array[0]}:{StarTextures.Count}";
                Economics.Money -= Cost;
            }
            else if (StarTextures.Count >= 4)
            {
                Enabled = false;
                Color = Color.Gray;
            }
            base.Update(mouse, oldMouse, gameTime, graphicsDevice);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Label.Draw(spriteBatch);
            float y = Label.SpriteFont.MeasureString(Label.Text).Y;
            spriteBatch.DrawString(Label.SpriteFont, $"${Cost.ToString()}", new Vector2(Label.Position.X, Label.Position.Y + y * 2), Color.White);
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

        Sprite Window;

        TextLabel PerformenceLabel;

        MouseState oldMouse;

        Button doneButton;
        Button undoButton;
        
        StatsButton lastButtonClicked;

        public StatsScreen(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            Economics.Money = 10000;

            SpriteFont font = Content.Load<SpriteFont>("TextFont");
            
            var screenWidth = graphics.Viewport.Width;
            var screenHeight = graphics.Viewport.Height;

            var doneTexture = Content.Load<Texture2D>("StatsScreenAssets/done");
            doneButton = new Button(doneTexture, new Vector2(screenWidth - doneTexture.Width, screenHeight - doneTexture.Height), Color.White, Main.SpriteScales[doneTexture.Name].ToVector2(), null);
            
            var undoTexture = Content.Load<Texture2D>("StatsScreenAssets/undo");
            undoButton = new Button(undoTexture, new Vector2(undoTexture.Width, screenHeight - doneTexture.Height), Color.White, Main.SpriteScales[undoTexture.Name].ToVector2(), null);

            var tankShieldTexture = Content.Load<Texture2D>("StatsScreenAssets/shield");
            var damageTexture = Content.Load<Texture2D>("StatsScreenAssets/damage");
            var speedTexture = Content.Load<Texture2D>("StatsScreenAssets/speed");
            var fogVisibilityTexture = Content.Load<Texture2D>("StatsScreenAssets/fogvisibility");

            Texture2D starTexture = Content.Load<Texture2D>("StatsScreenAssets/star");
            var starScale = Main.SpriteScales[starTexture.Name];
            
            List<Sprite> stars = new List<Sprite>();
            int x = screenWidth / 2 - 300;

            for (int i = 0; i < 4; i++)
            {
                //We are using tankShieldTexture, doesn't matter which texture we use we just need a height (all
                //heights should be the same anyways....)
                stars.Add(new Sprite(starTexture, new Vector2(x - starTexture.Width * 1.5f, (screenHeight / 2) + tankShieldTexture.Height), Color.White, starScale.ToVector2()));
                x += 200;
            }

            var shieldLabel = new TextLabel(new Vector2(stars[0].Position.X, stars[0].Position.Y - stars[0].ScaledHeight + stars[0].ScaledHeight * 2), Color.Navy, $"Health:1", font)
            {
                IsVisible = false
            };
            var damageLabel = new TextLabel(new Vector2(stars[1].Position.X, stars[1].Position.Y - stars[1].ScaledHeight + stars[1].ScaledHeight * 2), Color.Navy, $"Damage:1", font)
            {
                IsVisible = false
            };
            var movementLabel = new TextLabel(new Vector2(stars[2].Position.X, stars[2].Position.Y - stars[2].ScaledHeight + stars[2].ScaledHeight * 2), Color.Navy, $"Speed:1", font)
            {
                IsVisible = false
            };
            var fogVisibilityLabel = new TextLabel(new Vector2(stars[3].Position.X, stars[3].Position.Y - stars[3].ScaledHeight + stars[3].ScaledHeight * 2), Color.Navy, $"DeFogger:1", font)
            {
                IsVisible = false
            };

            TankStats.Add(new StatsButton(tankShieldTexture, new Vector2(screenWidth / 2 - 300, screenHeight / 2 - 100), Main.SpriteScales[tankShieldTexture.Name].ToVector2(), shieldLabel, stars[0], "sheild"));
            TankStats.Add(new StatsButton(damageTexture, new Vector2(screenWidth / 2 - 100, screenHeight / 2 - 100), Main.SpriteScales[damageTexture.Name].ToVector2(), damageLabel, stars[1], "damage"));
            TankStats.Add(new StatsButton(speedTexture, new Vector2(screenWidth / 2 + 100, screenHeight / 2 - 100), Main.SpriteScales[speedTexture.Name].ToVector2(), movementLabel, stars[2], "speed"));
            TankStats.Add(new StatsButton(fogVisibilityTexture, new Vector2(screenWidth / 2 + 300, screenHeight / 2 - 100), Main.SpriteScales[fogVisibilityTexture.Name].ToVector2(), fogVisibilityLabel, stars[3], "fog"));
            
            var windowTexture = Content.Load<Texture2D>("StatsScreenAssets/window");
            Window = new Sprite(windowTexture, new Vector2(screenWidth / 2, screenHeight / 2), Color.White, Main.SpriteScales[windowTexture.Name].ToVector2());

            float performenceLabelX = (TankStats[1].Position.X + TankStats[2].Position.X) / 2;
            PerformenceLabel = new TextLabel(new Vector2(performenceLabelX - font.MeasureString("STATS:").X / 2, screenHeight / 2 - 200), Color.Black, "STATS:", font);

            Sprites.Add(Window);
            Sprites.Add(PerformenceLabel);

            Sprites.Add(doneButton);
            Sprites.Add(undoButton);

            Sprites.AddRange(TankStats);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            if (doneButton.IsClicked(mouse) && !doneButton.IsClicked(oldMouse))
            {
                Main.CurrentState = States.LevelSelect;
                Main.PreviousState = States.StatsScreen;
            }

            //if u want to do multiple undo's u need stack thingy
            if (undoButton.IsClicked(mouse) && !undoButton.IsClicked(oldMouse) && lastButtonClicked != null)
            {
                var list = Economics.GetListByName(lastButtonClicked.Name);
                lastButtonClicked.Cost = list[lastButtonClicked.StarTextures.Count - 2];
                Economics.Money += lastButtonClicked.Cost;
                var starToRemove = lastButtonClicked.StarTextures[lastButtonClicked.StarTextures.Count - 1];
                lastButtonClicked.StarTextures.Remove(starToRemove);
                string[] array = lastButtonClicked.Label.Text.Split(':');
                lastButtonClicked.Label.Text = $"{array[0]}:{lastButtonClicked.StarTextures.Count}";
                lastButtonClicked = null;
            }

            for (int i = 0; i < TankStats.Count; i++)
            {
                switch (TankStats[i].Name)
                {
                    case "sheild":
                        TankStats[i].Cost = Economics.HealthCosts[TankStats[i].StarTextures.Count - 1];
                        break;
                    case "damage":
                        TankStats[i].Cost = Economics.DamageCosts[TankStats[i].StarTextures.Count - 1];
                        break;
                    case "speed":
                        TankStats[i].Cost = Economics.SpeedCosts[TankStats[i].StarTextures.Count - 1];
                        break;
                    case "fog":
                        TankStats[i].Cost = Economics.DeFoggerCosts[TankStats[i].StarTextures.Count - 1];
                        break;
                }
                TankStats[i].Update(mouse, oldMouse, gameTime, ref lastButtonClicked);
            }
            oldMouse = mouse;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //Changing font size without having to create a new spritefont
            for (int i = 0; i < TankStats.Count; i++)
            {
                var label = TankStats[i].Label;
                spriteBatch.DrawString(label.SpriteFont, label.Text, label.Position, label.Color, 0f, label.Origin, 1.5f, SpriteEffects.None, 0f);
            }
        }
    }
}