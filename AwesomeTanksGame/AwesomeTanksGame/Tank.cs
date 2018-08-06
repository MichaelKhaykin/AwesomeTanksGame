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

        public Dictionary<Keys, Action<GameTime>> Movements;

        public int Health { get; set; }
        private TimeSpan ElapsedSpecialPowerTimer = TimeSpan.Zero;
        private TimeSpan SpecialPowerTimer = TimeSpan.FromSeconds(10);

        private List<Bullet> Bullets;

        private Texture2D bulletTexture;

        private Vector2 forwardVector = Vector2.UnitX;
        private (int moveSpeed, int spinSpeed) speed;

        public override float Rotation
        {
            get => base.Rotation;
            set
            {
                base.Rotation = value;
                forwardVector = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            }
        }

        public Tank(Texture2D texture, Vector2 position, Vector2 scale, ContentManager Content, GraphicsDevice graphics, (int moveSpeed, int spinSpeed) speed)
            : this(texture, position, Color.White, scale, Content, graphics, Keys.W, Keys.S, Keys.D, Keys.A, speed) { }

        public Tank(Texture2D texture, Vector2 position, Color color, Vector2 scale, ContentManager Content, GraphicsDevice graphics, Keys forward, Keys backward, Keys spinLeft, Keys spinRight, (int moveSpeed, int spinSpeed) speed)
            : base(texture, position, color, scale, null)
        {
            Bullets = new List<Bullet>();
            bulletTexture = Content.Load<Texture2D>("Bullet");

            this.speed = speed;

            Movements = new Dictionary<Keys, Action<GameTime>>
            {
                [forward] = new Action<GameTime>((gameTime) => Position += forwardVector * speed.moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds),
                [backward] = new Action<GameTime>((gameTime) => Position -= forwardVector * speed.moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds),
                [spinLeft] = new Action<GameTime>((gameTime) => Rotation += MathHelper.ToRadians(speed.spinSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds),
                [spinRight] = new Action<GameTime>((gameTime) => Rotation -= MathHelper.ToRadians(speed.spinSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds)
            };
        }

        public override void Update(GameTime gameTime, GraphicsDevice graphicsDevice = null)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            
            ElapsedSpecialPowerTimer += gameTime.ElapsedGameTime;
            
            HandleMovements(keyboardState, gameTime);

            if (Main.MouseState.LeftButton == ButtonState.Pressed && Main.oldMouseState.LeftButton == ButtonState.Released)
            {
                Bullets.Add(new Bullet(bulletTexture, new Vector2(Position.X, Position.Y), Color.White, 0.05f.ToVector2()));
                Bullets[Bullets.Count - 1].Slope = forwardVector;
                Bullets[Bullets.Count - 1].FlyingTime.Start();
                Bullets[Bullets.Count - 1].Rotation = Rotation;
                //shooting stuffs
            }

            if (ElapsedSpecialPowerTimer > SpecialPowerTimer)
            {
                //do something cool
                ElapsedSpecialPowerTimer = TimeSpan.Zero;
            }

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

        public void HandleMovements(KeyboardState keyboardState, GameTime gameTime)
        {
            foreach (var currentKey in keyboardState.GetPressedKeys())
            {
                if (!Movements.ContainsKey(currentKey))
                {
                    continue;
                }
                Movements[currentKey](gameTime);
            }
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
