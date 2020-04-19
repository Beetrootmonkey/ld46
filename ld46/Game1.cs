using ld46.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using Newtonsoft.Json.Linq;
using Spritesheet;
using Animation = Spritesheet.Animation;

namespace ld46
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static bool DebugMode;
        private DateTime _StartTime;
        private KeyboardState _PreviousKeyboardState;

        GraphicsDeviceManager _Graphics;
        SpriteBatch _SpriteBatch;

        private SpriteFont _Font;

        List<Flower> _FlowerList;
        private Player _Player;
        private Lake _Lake;
        private TimeSpan _LastFlowerHealthUpdate;


        public Game1()
        {
#if DEBUG
            DebugMode = true;
#endif

            _Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1240,
                PreferredBackBufferHeight = 800
            };
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += (Object sender, EventArgs e) => OnResizeWindow();
            _Graphics.ApplyChanges();
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
            _PreviousKeyboardState = Keyboard.GetState();
            base.Initialize();
        }

        protected void OnResizeWindow()
        {
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _StartTime = DateTime.Now;

            // Create a new SpriteBatch, which can be used to draw textures.
            _SpriteBatch = new SpriteBatch(GraphicsDevice);
            Spritesheet.Spritesheet sheet;

            //Font
            _Font = Content.Load<SpriteFont>("Default");

            // HealthbarColorGradient
            HealthBarColors.Init(Content.Load<Texture2D>("Sprites/health_bar_color_gradient"));

            //Lake
            Size lakeTextureSize = new Size(174, 106);
            sheet = new Spritesheet.Spritesheet(Content.Load<Texture2D>("Sprites/lake_spritesheet")).WithGrid((lakeTextureSize.Width, lakeTextureSize.Height), (0,0), (0,0));
            _Lake = new Lake(new Vector2(Window.ClientBounds.Width / 2 - lakeTextureSize.Width/2, Window.ClientBounds.Height/2- lakeTextureSize.Height/2), lakeTextureSize);
            _Lake.AnimationDictionary.Add(0, sheet.CreateAnimation((0, 0),(0, 1),(0, 2),(0, 3),(0, 4),(0, 5),(0, 6),(0, 7)));

            //Flower
            


            //Player
            Size playerTextureSize = new Size(40, 56);
            sheet = new Spritesheet.Spritesheet(Content.Load<Texture2D>("Sprites/player_spritesheet")).WithGrid((playerTextureSize.Width, playerTextureSize.Height), (0,0), (0,0));
            _Player = new Player(new Vector2(100, 100), playerTextureSize);
            _Player.AddAnimation(PlayerAnimation.Idle, sheet.CreateAnimation((0, 0)));
            _Player.AddAnimation(PlayerAnimation.LookingUp, sheet.CreateAnimation((0, 1), (1, 1), (2, 1), (3, 1)));
            _Player.AddAnimation(PlayerAnimation.LookingUpRight, sheet.CreateAnimation((0, 1), (1, 1), (2, 1), (3, 1)));
            _Player.AddAnimation(PlayerAnimation.LookingRight, sheet.CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0)));
            _Player.AddAnimation(PlayerAnimation.LookingRightDown, sheet.CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0)));
            _Player.AddAnimation(PlayerAnimation.LookingDown, sheet.CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0)));
            _Player.AddAnimation(PlayerAnimation.LookingDownLeft, sheet.WithFrameEffect(SpriteEffects.FlipHorizontally).CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0)));
            _Player.AddAnimation(PlayerAnimation.LookingLeft, sheet.WithFrameEffect(SpriteEffects.FlipHorizontally).CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0)));
            _Player.AddAnimation(PlayerAnimation.LookingLeftUp, sheet.WithFrameEffect(SpriteEffects.FlipHorizontally).CreateAnimation((0, 1), (1, 1), (2, 1), (3, 1)));

            //_Player.AnimationDictionary.Add(0, sheet.CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0)));
        }

        private void LoadFlower(Spritesheet.Spritesheet sheet, int count)
        {
            Size flowerTextureSize = new Size(54, 58);
            sheet = new Spritesheet.Spritesheet(Content.Load<Texture2D>("Sprites/skull_spritesheet")).WithGrid((flowerTextureSize.Width, flowerTextureSize.Height), (0,0), (0,0));
            var animationAlive = sheet.CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0));
            var animationSick = sheet.CreateAnimation((0, 1), (1, 1), (2, 1), (3, 1));
            var animationDead = sheet.CreateAnimation((0, 2));

            _FlowerList = new List<Flower>();

            Random rnd = new Random();
            for (int i = 0; i < 1; i++)
            {
                Flower flower;
                do
                {
                    int w = rnd.Next(10, Window.ClientBounds.Width);
                    int h = rnd.Next(10, Window.ClientBounds.Height);
                    flower = new Flower(new Vector2(w, h), flowerTextureSize);
                } while (flower.CollisionBox.Intersects(_Lake.CollisionBox));

                flower.AddAnimation(FlowerAnimation.Alive, animationAlive.Clone());
                flower.AddAnimation(FlowerAnimation.Sick, animationSick.Clone());
                flower.AddAnimation(FlowerAnimation.Dead, animationDead.Clone());

                _FlowerList.Add(flower);
            }
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
            {
                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R) && !_PreviousKeyboardState.IsKeyDown(Keys.R))
            {
                LoadContent();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.D) 
                    && !_PreviousKeyboardState.IsKeyDown(Keys.D))
            {
                DebugMode = !DebugMode;
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.N) && !_PreviousKeyboardState.IsKeyDown(Keys.N))
            {
                //Neue Blume
            }

            //Player
            _Player.Update(gameTime);
            UpdatePlayerPosition();

            //Lake
            _Lake.Update(gameTime);

            //Flower
            bool updateFlowerHealth = false;
            if (gameTime.TotalGameTime - _LastFlowerHealthUpdate > TimeSpan.FromSeconds(1))
            {
                _LastFlowerHealthUpdate = gameTime.TotalGameTime;
                updateFlowerHealth = true;
            }

            foreach (var flower in _FlowerList)
            {
                flower.Update(gameTime);
                if (updateFlowerHealth)
                {
                    flower.Health -= 4;
                }
            }

            _PreviousKeyboardState = Keyboard.GetState();
            base.Update(gameTime);
        }

        private void UpdatePlayerPosition()
        {
            float speed = 3;

            for (int i = 0; i < speed; i++)
            {
                Vector2 newPos = _Player._Position;
                var kbState = Keyboard.GetState();

                if (kbState.IsKeyDown(Keys.Up) && kbState.IsKeyDown(Keys.Right))
                {
                    _Player.VDirection = true;
                    _Player.HDirection = true;
                    newPos += new Vector2(1, -1);
                }
                else if (kbState.IsKeyDown(Keys.Right) && kbState.IsKeyDown(Keys.Down))
                {
                    _Player.VDirection = false;
                    _Player.HDirection = true;
                    newPos += new Vector2(1, 1);
                }
                else if (kbState.IsKeyDown(Keys.Down) && kbState.IsKeyDown(Keys.Left))
                {
                    _Player.VDirection = false;
                    _Player.HDirection = false;
                    newPos += new Vector2(-1, 1);
                }
                else if (kbState.IsKeyDown(Keys.Left) && kbState.IsKeyDown(Keys.Up))
                {
                    _Player.VDirection = true;
                    _Player.HDirection = false;
                    newPos += new Vector2(-1, -1);
                }
                else if (kbState.IsKeyDown(Keys.Up))
                {
                    _Player.VDirection = true;
                    newPos += new Vector2(0, -1);
                }
                else if (kbState.IsKeyDown(Keys.Left))
                {
                    _Player.HDirection = false;
                    newPos += new Vector2(-1, 0);
                }
                else if (kbState.IsKeyDown(Keys.Down))
                {
                    _Player.VDirection = false;
                    newPos += new Vector2(0, 1);
                }
                else if (kbState.IsKeyDown(Keys.Right))
                {
                    _Player.HDirection = true;
                    newPos += new Vector2(1, 0);
                }
                else
                {
                    _Player.Idle();
                }

                if (newPos == _Player._Position)
                {
                    return;
                }

                var newCollissionBox = _Player.CalcCollissionBox(newPos);

                //Kollision Blumen
                foreach (var flower in _FlowerList)
                {
                    if (newCollissionBox.Intersects(flower.CollisionBox))
                    {
                        if (_Player.Water > 0
                            && flower.Health != Flower.HEALTH_MAX 
                            && flower.Health != 0)
                        {
                            int healthLost = Flower.HEALTH_MAX - flower.Health;

                            int heal = Math.Min(_Player.Water, healthLost);
                            flower.Health += heal;
                            _Player.Water -= heal;
                        }

                        return;
                    }
                }

                //Kollision See
                if(newCollissionBox.Intersects(_Lake.CollisionBox))
                {
                    if (_Player.Water != Player.MAX_WATER)
                    {
                        _Player.Water = Player.MAX_WATER;
                    }
                    return;
                }
                

                //Kollision Kanten
                if (newPos.X >= 0 && newPos.Y >= 0
                && newCollissionBox.Right <= Window.ClientBounds.Width && newCollissionBox.Bottom <= Window.ClientBounds.Height)
                {
                    _Player._Position = newPos;
                }
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(105,40,13));
            _SpriteBatch.Begin();

            _Lake.Draw(_SpriteBatch);

            var drawList = new List<AEntity>(_FlowerList)
            {
                _Player
            }.OrderBy(v => v._Position.Y);

            foreach (var i in drawList)
            {
                i.Draw(_SpriteBatch);
            }

            float textY = 0;
            string currentTime = (DateTime.Now - _StartTime).ToString(@"hh\:mm\:ss");
            var currentTimeTextVec = _Font.MeasureString(currentTime);
            _SpriteBatch.DrawString(_Font, currentTime, new Vector2(0, textY), Color.White);

            textY += currentTimeTextVec.Y + 5;
            _SpriteBatch.DrawString(_Font, "Lava: " + _Player.Water, new Vector2(0, textY), Color.White);

            

            _SpriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
