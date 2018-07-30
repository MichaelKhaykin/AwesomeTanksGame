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
        private bool isOn { get; set; }

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

    public class SetUpScreen : Screen
    {
        SpriteFont font;

        StatefulButton soundButton;
        StatefulButton musicButton;

        Button playButton;

        MouseState oldMouse;

        public SetUpScreen(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            int screenWidth = graphics.Viewport.Width;
            int screenHeight = graphics.Viewport.Height;

            font = Content.Load<SpriteFont>("TextFont");

            var playTexture = Content.Load<Texture2D>("SetUpGameAssets/button_play");
            var scale = Vector2.One / 4;
            playButton = new Button(playTexture, new Vector2(screenWidth / 2, screenHeight / 2), Color.White, scale, null);

            var soundTexture = Content.Load<Texture2D>("SetUpGameAssets/button_music");
            var soundOtherTexture = Content.Load<Texture2D>("SetUpGameAssets/button_music_off");

            //TODO: Make extension method for texture, to have scaled width and height
            var scaleForButtons = new Vector2(0.5f, 0.5f);
            soundButton = new StatefulButton(soundTexture, new Vector2(screenWidth / 2 - (soundTexture.Width / 2) * 0.5f, screenHeight / 2 + soundTexture.Height / 2), scaleForButtons, soundOtherTexture)
            {
                IsVisible = true,
                Enabled = true
            };
        
            var musicTexture = Content.Load<Texture2D>("SetUpGameAssets/button_sound");
            var musicOtherTexture = Content.Load<Texture2D>("SetUpGameAssets/button_sound_off");

            musicButton = new StatefulButton(musicTexture, new Vector2(screenWidth / 2 - (soundTexture.Width / 2) * 0.5f, screenHeight / 2 + musicTexture.Height + (musicTexture.Height / 2) * 0.5f), scaleForButtons, musicOtherTexture)
            {
                IsVisible = true,
                Enabled = true
            };
        
            Sprites.Add(soundButton);
            Sprites.Add(musicButton);
            Sprites.Add(playButton);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            soundButton.Update(mouse, oldMouse);
            musicButton.Update(mouse, oldMouse); 

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
 