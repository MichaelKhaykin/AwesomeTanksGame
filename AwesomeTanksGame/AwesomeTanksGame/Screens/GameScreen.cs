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
    public class GameScreen : Screen
    {
        Tank heroTank;

        public GameScreen(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            LoadLevel(graphics, content);
        }

        public void LoadLevel(GraphicsDevice graphics, ContentManager content)
        {
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
            heroTank = new Tank(Content.Load<Texture2D>("Tanks/BTR_Tower"), new Vector2(100, 200), Color.White, 1f.ToVector2(), Content, graphics);

            Sprites.Add(heroTank);
        }
    }
}
