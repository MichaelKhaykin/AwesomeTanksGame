using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AwesomeTanksGame.Screens
{
    public class LevelSelectScreen : Screen
    {
        List<BaseButton> LevelButtons;
        List<Texture2D> NumberTextures;
        Sprite LevelsHeader;

        //        Sprite Window;

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

            //            var windowTexture = Content.Load<Texture2D>("StatsScreenAssets/window");
            //            Window = new Sprite(windowTexture, new Vector2(screenWidth / 2, screenHeight / 2 + windowTexture.Height / 8), Color.Gray, Main.SpriteScales[windowTexture.Name].ToVector2() / 2);

            Sprites.Add(LevelsHeader);
            Sprites.AddRange(LevelButtons);
            //          Sprites.Add(Window);
        }
    }
}
