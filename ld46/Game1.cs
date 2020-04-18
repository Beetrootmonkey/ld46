using ld46.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace ld46
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texturePerson;
        Texture2D textureDesk;
        Texture2D textureFloor;
        Texture2D textureWall;

        Texture2D mapData;
        Texture2D mapAreaData;

        int mapWidth;
        int mapHeight;
        int tileSize;
        float tileScale;
        int mapOffsetX;
        int mapOffsetY;
        List<ATile[,]> map;
        List<Person> persons;

        Person player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1240;
            graphics.PreferredBackBufferHeight = 800;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += (Object sender, EventArgs e) => OnResizeWindow();
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            base.Initialize();
        }

        protected void OnResizeWindow()
        {
            int originalTileSize = 40;
            tileSize = originalTileSize;

            float currentTileSize = Math.Min((float)Window.ClientBounds.Width / mapWidth, (float)Window.ClientBounds.Height / mapHeight);
            tileScale = currentTileSize / originalTileSize;
            mapOffsetX = (Window.ClientBounds.Width - Math.Min(Window.ClientBounds.Width, Window.ClientBounds.Height)) / 2;
            mapOffsetY = (Window.ClientBounds.Height - Math.Min(Window.ClientBounds.Width, Window.ClientBounds.Height)) / 2;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texturePerson = Content.Load<Texture2D>("Sprites/person");
            textureDesk = Content.Load<Texture2D>("Sprites/desk");
            textureFloor = Content.Load<Texture2D>("Sprites/floor");
            textureWall = Content.Load<Texture2D>("Sprites/wall");

            mapData = Content.Load<Texture2D>("MapData/map1");
            mapAreaData = Content.Load<Texture2D>("MapData/map1_areas");
            mapWidth = mapData.Width;
            mapHeight = mapData.Height;

            OnResizeWindow();

            map = new List<ATile[,]>();
            var wallArray = new WallTile[mapWidth, mapHeight];
            var areasArray = new AreaTile[mapWidth, mapHeight];

            map.Add(wallArray);
            map.Add(areasArray);

            var colors = new Color[mapWidth * mapHeight];

            mapData.GetData<Color>(colors);
            for (int j = 0; j < mapHeight; j++)
            {
                for (int i = 0; i < mapWidth; i++)
                {
                    Color color = colors[i + j * mapWidth];
                    if (color == new Color(255, 255, 255))
                    {
                        wallArray[i, j] = new WallTile(textureWall);
                    }
                    else if (color == new Color(124, 95, 65))
                    {
                        wallArray[i, j] = new WallTile(textureDesk);
                    }
                }
            }

            mapAreaData.GetData<Color>(colors);
            for (int j = 0; j < mapHeight; j++)
            {
                for (int i = 0; i < mapWidth; i++)
                {
                    Color color = colors[i + j * mapWidth];
                    if (color.A != 0)
                    {
                        areasArray[i, j] = new AreaTile(color);
                    }
                }
            }

            persons = new List<Person>();
            player = new Person(new Vector2(100, 100));
            persons.Add(player);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float speed = 3;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                player.position += new Vector2(0, -1) * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                player.position += new Vector2(-1, 0) * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                player.position += new Vector2(0, 1) * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                player.position += new Vector2(1, 0) * speed;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (var layer in map)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    for (int i = 0; i < mapWidth; i++)
                    {
                        if (layer[i, j] != null)
                        {
                            layer[i, j].Draw(spriteBatch, i, j, tileSize, tileScale, mapOffsetX, mapOffsetY);
                        }
                    }
                }
            }

            foreach (var person in persons)
            {
                person.Draw(spriteBatch, texturePerson, tileScale, mapOffsetX, mapOffsetY);
            }


            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
