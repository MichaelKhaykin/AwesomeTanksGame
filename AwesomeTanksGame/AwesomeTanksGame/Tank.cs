using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeTanksGame
{
    public class Bullet : Sprite
    {
        public Stopwatch FlyingTime { get; set; }
        public Vector2 Slope { get; set; }
        public float Speed { get; set; }

        public Bullet(Texture2D texture, Vector2 position, Color color, Vector2 scale, Texture2D pixel = null) : base(texture, position, color, scale, pixel)
        {
            FlyingTime = new Stopwatch();
            Speed = 3f;
        }
    }

    public class Tank : Sprite
    {
        private KeyboardState oldKeyboardState;
        
        public int Health { get; set; }
        private TimeSpan ElapsedSpecialPowerTimer = TimeSpan.Zero;
        private TimeSpan SpecialPowerTimer = TimeSpan.FromSeconds(10);

        private List<Bullet> Bullets;
        
        private Texture2D bulletTexture;


        public Tank(Texture2D texture, Vector2 position, Color color, Vector2 scale, ContentManager Content, GraphicsDevice graphics) 
            : base(texture, position, color, scale, null)
        {
            Bullets = new List<Bullet>();
            bulletTexture = Content.Load<Texture2D>("Bullet");
        }

        public override void Update(GameTime gameTime, GraphicsDevice graphicsDevice = null)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            
            var mouseVector = new Vector2(Main.MouseState.X, Main.MouseState.Y);
            var slope = Position - mouseVector;

            ElapsedSpecialPowerTimer += gameTime.ElapsedGameTime;

            if (keyboardState.IsKeyDown(Keys.A) && !oldKeyboardState.IsKeyUp(Keys.A))
            {
                Position = new Vector2(Position.X - 1, Position.Y);
            }
            if (keyboardState.IsKeyDown(Keys.D) && !oldKeyboardState.IsKeyUp(Keys.D))
            {
                Position = new Vector2(Position.X + 1, Position.Y);
            }
            if (keyboardState.IsKeyDown(Keys.W) && !oldKeyboardState.IsKeyUp(Keys.W))
            {
                Position = new Vector2(Position.X, Position.Y - 1);
            }
            if (keyboardState.IsKeyDown(Keys.S) && !oldKeyboardState.IsKeyUp(Keys.S))
            {
                Position = new Vector2(Position.X, Position.Y + 1);
            }

            if (Main.MouseState.LeftButton == ButtonState.Pressed && Main.oldMouseState.LeftButton == ButtonState.Released)
            {
                Bullets.Add(new Bullet(bulletTexture, new Vector2(Position.X, Position.Y), Color.White, 0.05f.ToVector2()));
                slope.Normalize();
                Bullets[Bullets.Count - 1].Slope = slope * -1;
                Bullets[Bullets.Count - 1].FlyingTime.Start();
                Bullets[Bullets.Count - 1].Rotation = Rotation + MathHelper.PiOver2;
                //shooting stuffs
            }

            if (ElapsedSpecialPowerTimer > SpecialPowerTimer)
            {
                //do something cool
                ElapsedSpecialPowerTimer = TimeSpan.Zero;
            }

            Rotation = (float)Math.Atan2(slope.Y, slope.X) + MathHelper.ToRadians(90);//Some rotation value

            oldKeyboardState = keyboardState;
            
            //bullet cleanup
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets[i].Position += Bullets[i].Slope * Bullets[i].Speed;
                
                if (Bullets[i].FlyingTime.ElapsedMilliseconds > 3000)
                {
                    Bullets.RemoveAt(i);
                    i--;
                }
            }

            base.Update(gameTime, graphicsDevice);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets[i].SpriteEffects = SpriteEffects.FlipHorizontally;
                Bullets[i].Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }
}
