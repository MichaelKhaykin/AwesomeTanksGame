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
using Newtonsoft.Json;

namespace AwesomeTanksGame.Screens
{
    struct Tile
    {
        public Rectangle Square { get; set; }
        public string Name { get; set; }
        public float ScaledWidth { get; set; }
        public float ScaledHeight { get; set; }
        public float Scale { get; set; }
    }

    public class GameScreen : Screen
    {
        Texture2D tankTexture;
        Tank heroTank;
        List<Tile> tiles = new List<Tile>();

        Sprite fogTile;
        Rectangle fogTileArea;

        FogState[,] fog;

        public GameScreen(GraphicsDevice graphics, ContentManager content)
            : base(graphics, content)
        {
            //Making map
            var rectangle = new Rectangle(3, 3, 15, 5);
            var waterRectangle = new Rectangle(4, 4, 13, 3);
            var tileTexture = Content.Load<Texture2D>("GameScreen/Grass (2)");
            var tileTexture1 = Content.Load<Texture2D>("GameScreen/Water");

            var scale = 0.5f;
            tiles.Add(new Tile() { Name = tileTexture.Name, Square = rectangle, ScaledWidth = tileTexture.Width * scale, ScaledHeight = tileTexture.Height * scale, Scale = scale });
            tiles.Add(new Tile() { Name = tileTexture1.Name, Square = waterRectangle, ScaledWidth = tileTexture1.Width * scale, ScaledHeight = tileTexture1.Height * scale, Scale = scale });

            //    tiles.Add(new Tile() { X = 100, Y = 100, Name = "Grass (2)", Continuation = new Continuation() { IsContinuation = (false, false), CountOfTiles = 0 } });

            var data = JsonConvert.SerializeObject(tiles);

            //Get data
            tiles = JsonConvert.DeserializeObject<List<Tile>>(data);
            //-----------

            // Calculate outer bounding box (tileArea) of the map
            var tileArea = new Rectangle()
            {
                X = tiles.Min(t => t.Square.X),
                Y = tiles.Min(t => t.Square.Y),
                Width = tiles.Max(t => t.Square.X + t.Square.Width),
                Height = tiles.Max(t => t.Square.Y + t.Square.Height)
            };

            // Adjust for starting position
            tileArea.Width -= tileArea.X;
            tileArea.Height -= tileArea.Y;


            int screenWidth = graphics.Viewport.Width;
            int screenHeight = graphics.Viewport.Height;

            tankTexture = Content.Load<Texture2D>("Tanks/BTR_Tower");

            Texture2D pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            //Todo make scaled width/height
            var pixelSize = 4;
            fogTileArea = new Rectangle()
            {
                X = (int)(tileArea.X * tileTexture.Width * scale),
                Y = (int)(tileArea.Y * tileTexture.Height * scale),
                Width = (int)(tileArea.Width * tileTexture.Width * scale / pixelSize),
                Height = (int)(tileArea.Height * tileTexture.Height * scale / pixelSize)
            };

            fogTile = new Sprite(pixel, new Vector2(fogTileArea.X, fogTileArea.Y), Color.Black, new Vector2(pixelSize), null)
            {
                Origin = Vector2.Zero
            };

            heroTank = new Tank(tankTexture, new Vector2(100, 200), Color.White, 1f.ToVector2(), Content, graphics, Keys.W, Keys.S, Keys.D, Keys.A, (tankTexture.Width, 72));

            Sprites.Add(heroTank);

            LoadLevel(graphics, content);

            fog = new FogState[fogTileArea.Width, fogTileArea.Height];

            for (int i = 0; i < fog.GetLength(0); i++)
            {
                for (int j = 0; j < fog.GetLength(1); j++)
                {
                    fog[i, j] = FogState.Visible;
                }
            }
        }

        public void LoadLevel(GraphicsDevice graphics, ContentManager content)
        {
            string filename = $"level{Main.LevelSelected}.json";

        }

        public void LevelZero(GraphicsDevice graphics, ContentManager content)
        {
            //Level 0 stuff
        }

        public override void Update(GameTime gameTime)
        {
            for (int x = 0; x < fogTileArea.Width; x++)
            {
                var xPos = fogTileArea.X + x * fogTile.Scale.X;
                for (int y = 0; y < fogTileArea.Height; y++)
                {
                    var yPos = fogTileArea.Y + y * fogTile.Scale.Y;

                    var radius = heroTank.ScaledWidth * 0.5f;
                    var radiusSquared = radius * radius;
                    var hypSquared = Vector2.DistanceSquared(heroTank.Position, new Vector2(xPos, yPos));

                    if (hypSquared <= radiusSquared)
                    {
                        fog[x, y] = FogState.Invisible;
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                var texture = Content.Load<Texture2D>(tiles[i].Name);

                var square = tiles[i].Square;

                for (int j = tiles[i].Square.X; j < square.Width + tiles[i].Square.X; j++)
                {
                    for (int z = tiles[i].Square.Y; z < square.Height + tiles[i].Square.Y; z++)
                    {
                        var x = j * tiles[i].ScaledWidth;
                        var y = z * tiles[i].ScaledHeight;

                        spriteBatch.Draw(texture, new Vector2(x, y), null, Color.White, 0f, Vector2.Zero, tiles[i].Scale, SpriteEffects.None, 0f);
                    }
                }
            }

            for (int x = 0; x < fogTileArea.Width; x++)
            {
                for (int y = 0; y < fogTileArea.Height; y++)
                {
                    var xPos = fogTileArea.X + x * fogTile.Scale.X;
                    var yPos = fogTileArea.Y + y * fogTile.Scale.Y;

                    if (fog[x, y] == FogState.Visible)
                    {
                        spriteBatch.Draw(fogTile.Texture, new Vector2(xPos, yPos), null, Color.Black, 0f, Vector2.Zero, fogTile.Scale, SpriteEffects.None, 0f);
                    }
                }
            }

            base.Draw(spriteBatch);
        }
    }
}

