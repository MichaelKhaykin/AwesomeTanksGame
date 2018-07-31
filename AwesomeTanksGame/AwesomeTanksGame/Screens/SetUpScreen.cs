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
using Microsoft.Xna.Framework.Media;

namespace AwesomeTanksGame.Screens
{
    class StatefulButton : Button
    {
        public override Texture2D Texture
        {
            get
            {
                if (isOn)
                {
                    return base.Texture;
                }
                else
                {
                    return otherTexture;
                }
            }
        }
        private Texture2D otherTexture;
        public bool isOn { get; set; }

        public StatefulButton(Texture2D texture, Vector2 position, Vector2 scale, Texture2D otherTexture) 
            : base(texture, position, Color.White, scale, null)
        {
            this.otherTexture = otherTexture;
        }

        public void Update(MouseState mouse, MouseState oldMouse)
        {
            if (Enabled && IsClicked(mouse) && !IsClicked(oldMouse))
            {
                isOn = !isOn;
            }
        }
    }

    class MusicButton : StatefulButton
    {
        public MusicButton(Texture2D texture, Vector2 position, Vector2 scale, Texture2D otherTexture) : base(texture, position, scale, otherTexture)
        {
        }

        public override void Update(GameTime gameTime, GraphicsDevice graphicsDevice = null)
        {
            if (!isOn)
            {
                MediaPlayer.Pause();
            }
            else
            {
                MediaPlayer.Resume();
            }
            base.Update(gameTime, graphicsDevice);
        }
    }

    class SoundButton : StatefulButton
    {
        public SoundButton(Texture2D texture, Vector2 position, Vector2 scale, Texture2D otherTexture) : base(texture, position, scale, otherTexture)
        {
        }

        public override void Update(GameTime gameTime, GraphicsDevice graphicsDevice = null)
        {
            Main.ShouldPlaySoundsDuringGame = isOn;
            base.Update(gameTime, graphicsDevice);
        }
    }

    public class SetUpScreen : Screen
    {
        SpriteFont font;

        StatefulButton soundButton;
        StatefulButton musicButton;

        Button playButton;

        MouseState oldMouse;

        Song music;

        public SetUpScreen(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            music = Content.Load<Song>("MusicAndSoundEffects/Spirit of the Girl");

            int screenWidth = graphics.Viewport.Width;
            int screenHeight = graphics.Viewport.Height;

            font = Content.Load<SpriteFont>("TextFont");

            var playTexture = Content.Load<Texture2D>("SetUpGameAssets/button_play");
            string playButtonString = "playButton";
            playButton = new Button(playTexture, new Vector2(screenWidth / 2, screenHeight / 2), Color.White, Main.SpriteScales[playButtonString].ToVector2(), null)
            {
                Enabled = true
            };

            var soundTexture = Content.Load<Texture2D>("SetUpGameAssets/button_music");
            var soundOtherTexture = Content.Load<Texture2D>("SetUpGameAssets/button_music_off");

            string soundButtonString = "soundButton";

            soundButton = new SoundButton(soundTexture, new Vector2(screenWidth / 2 - (soundTexture.Width / 2) * 0.5f, screenHeight / 2 + soundTexture.Height / 2), Main.SpriteScales[soundButtonString].ToVector2(), soundOtherTexture)
            {
                IsVisible = true,
                Enabled = true,
                isOn = true
            };
        
            var musicTexture = Content.Load<Texture2D>("SetUpGameAssets/button_sound");
            var musicOtherTexture = Content.Load<Texture2D>("SetUpGameAssets/button_sound_off");

            string musicButtonString = "musicButton";

            musicButton = new MusicButton(musicTexture, new Vector2(screenWidth / 2 - (soundTexture.Width / 2) * 0.5f, screenHeight / 2 + musicTexture.Height + (musicTexture.Height / 2) * 0.5f), Main.SpriteScales[musicButtonString].ToVector2(), musicOtherTexture)
            {
                IsVisible = true,
                Enabled = true,
                isOn = true
            };
        
            Sprites.Add(soundButton);
            Sprites.Add(musicButton);
            Sprites.Add(playButton);
            
            MediaPlayer.Play(music);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            soundButton.Update(mouse, oldMouse);
            musicButton.Update(mouse, oldMouse);


            if (playButton.IsClicked(mouse))
            {
                Main.CurrentState = States.StatsScreen;
                Main.PreviousState = States.SetUp;
            }

            oldMouse = mouse;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            soundButton.Draw(spriteBatch);
            musicButton.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
 