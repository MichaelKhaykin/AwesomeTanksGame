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
    public class GameScreen : Screen
    {
        Texture2D tankTexture;
        Tank heroTank;
        Sprite[,] fog;

        public GameScreen(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            int screenWidth = graphics.Viewport.Width;
            int screenHeight = graphics.Viewport.Height;

            tankTexture = Content.Load<Texture2D>("Tanks/BTR_Tower");

            var fogTexture = Content.Load<Texture2D>("Tanks/BTR_Base");

            Texture2D pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            int x = screenWidth % fogTexture.Width;
            int y = screenHeight % fogTexture.Height;

            fog = new Sprite[x, y];
            
            int xAcc = 0;
            int yAcc = 0;
            for (int i = 0; i < fog.GetLength(0); i++)
            {
                for (int j = 0; j < fog.GetLength(1); j++)
                {
                    fog[i, j] = new Sprite(pixel, new Vector2(xAcc, yAcc), Color.Black, new Vector2(fogTexture.Width, fogTexture.Height));
                    yAcc += fogTexture.Height;
                }
                xAcc += fogTexture.Width;
                yAcc = 0;
            }

            LoadLevel(graphics, content);
        }

        public void LoadLevel(GraphicsDevice graphics, ContentManager content)
        {
            string filename = $"level{Main.LevelSelected}.json";

            //9 is amount of levels
            switch (Main.LevelSelected)
            {
                case 0:
                    LevelZero(graphics, content);
                    break;

                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    break;

                case 5:
                    break;

                case 6:
                    break;

                case 7:
                    break;

                case 8:
                    break;

                case 9:
                    break;
            }
        }

        public void LevelZero(GraphicsDevice graphics, ContentManager content)
        {
            //Level 0 stuff
            heroTank = new Tank(tankTexture, new Vector2(100, 200), Color.White, 1f.ToVector2(), Content, graphics, Keys.W, Keys.S, Keys.D, Keys.A, (tankTexture.Width, 72));

            Sprites.Add(heroTank);
        }

        public override void Update(GameTime gameTime)
        {
            for (int row = 0; row < fog.GetLength(0); row++)
            {
                for(int col = 0; col < fog.GetLength(1); col++)
                {
                    if (fog[row, col].HitBox.Intersects(heroTank.HitBox))
                    {
                        fog[row, col].Enabled = false;
                        fog[row, col].IsVisible = false;
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < fog.GetLength(0); i++)
            {
                for (int j = 0; j < fog.GetLength(1); j++)
                {
                    fog[i, j].Draw(spriteBatch);
                }
            }
            base.Draw(spriteBatch);
        }
    }
}
