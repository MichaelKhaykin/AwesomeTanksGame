using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AwesomeTanksGame
{
    public class BaseButton : Button
    {
        public BaseButton(Texture2D texture, Vector2 position, Color color, Vector2 scale) 
            : base(texture, position, color, scale, null)
        {

        }

        public virtual void Update(MouseState mouse, MouseState oldMouse, GameTime gameTime, GraphicsDevice graphicsDevice = null)
        {
            if (IsClicked(mouse) && !IsClicked(oldMouse) && Main.ShouldPlaySoundsDuringGame)
            {
                Main.buttonSoundClick.Play();
            }
            base.Update(gameTime, graphicsDevice);
        }
    }
}
