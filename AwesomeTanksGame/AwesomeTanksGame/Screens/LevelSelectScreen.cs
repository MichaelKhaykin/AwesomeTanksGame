using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AwesomeTanksGame.Screens
{
    public class LevelSelectScreen : Screen
    {
        List<BaseButton> LevelButtons;
        List<Texture2D> NumberTextures;
        Sprite LevelsHeader;
        
        public LevelSelectScreen(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            int screenWidth = graphics.Viewport.Width;
            int screenHeight = graphics.Viewport.Height;

            int count = 10;

            LevelButtons = new List<BaseButton>();
            NumberTextures = new List<Texture2D>();

            var buttonTexture = Content.Load<Texture2D>("LevelSelectScreen/button_empty");
            for (int i = 0; i < count; i++)
            {
                NumberTextures.Add(Content.Load<Texture2D>($"LevelSelectScreen/{i}"));
            }

            float x = 700;
            float y = screenHeight / 2;
            float scale = 0.3f;

            for (int i = 0; i < count; i++)
            {
                if (i % 5 == 0)
                {
                    y += buttonTexture.Height * scale;
                    x = 700;
                }
                else
                {
                    x += buttonTexture.Width * scale;
                }


                //gray out all buttons except the first one
                var imageColor = i == 0 ? Color.White : Color.Gray;

                var button = Main.CreateButton(graphics, buttonTexture, NumberTextures[i], new Vector2(x, y), Color.White, imageColor);
                //only enable the button if it is the first one;
                button.Enabled = i == 0;

                LevelButtons.Add(button);
            }

            var texture = Content.Load<Texture2D>("LevelSelectScreen/header_levels");
            LevelsHeader = new Sprite(texture, new Vector2(screenWidth / 2, texture.Height), Color.White, Main.SpriteScales[texture.Name].ToVector2());
            
            Sprites.Add(LevelsHeader);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < LevelButtons.Count; i++)
            {
                LevelButtons[i].Update(gameTime);
                //Tutorial case
                if (LevelButtons[i].IsClicked(Main.MouseState) && LevelButtons[i].IsClicked(Main.oldMouseState) && LevelButtons[i].Enabled)
                {
                    Main.CurrentState = States.Game;
                    Main.PreviousState = States.LevelSelect;
                    Main.LevelSelected = i;
                }
            }
            
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < LevelButtons.Count; i++)
            {
                LevelButtons[i].Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }
}
