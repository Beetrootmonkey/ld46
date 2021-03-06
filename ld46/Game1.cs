﻿using ld46.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended.Particles;

namespace ld46
{
    public enum GameState
    {
        Running,
        GameOver
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static bool DebugMode;
        public static Player _Player;
        public static Lake _Lake;

        private DateTime _StartTime;
        private TimeSpan _TimeSurvived;
        private KeyboardState _PreviousKeyboardState;

        private GameState _CurrentGameState;

        GraphicsDeviceManager _Graphics;
        SpriteBatch _SpriteBatch;

        private SpriteFont _Font;

        private int _ActiveFlowerCount;
        private List<Flower> _FlowerList;
        private APowerupBase _Powerup;
        private List<FadingText> _TextList;
        private Vector2 _DefaultPlayerVector;

        private TimeSpan _LastFlowerHealthUpdate;
        private DateTime _NextPowerupSpawn;

        private Texture2D _TextureBackGround;
        Random rnd = new Random();

        private (SoundEffect FlowerGrow, SoundEffect PlayerWalk, SoundEffect GatherWater, SoundEffect CollectItem) _SoundEffects;
        public static bool PlaySoundEffects = true;
        (Texture2D SoundOn, Texture2D SoundOff) _IconTextures;
        private MapGrid _MapGrid;

        private (Texture2D Ember, Texture2D Smoke) _ParticleTextures;
        private List<CustomParticleEffect> _ParticleEffects;

        private void RegisterParticleEffect(ParticleEffect particleEffect, float maxAge = -1)
        {
            _ParticleEffects.Add(new CustomParticleEffect(particleEffect, maxAge));
        }

        public Game1()
        {
#if DEBUG
            DebugMode = true;
#endif

            _Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1216,
                PreferredBackBufferHeight = 768
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

        public static bool PlaySoundEffect(SoundEffect soundEffect) => PlaySoundEffect(soundEffect, 0.5f);
        public static bool PlaySoundEffect(SoundEffect soundEffect, float volume) => PlaySoundEffect(soundEffect, volume, 0);
        public static bool PlaySoundEffect(SoundEffect soundEffect, float volume, float pitch) => PlaySoundEffect(soundEffect, volume, pitch, 0);
        public static bool PlaySoundEffect(SoundEffect soundEffect, float volume, float pitch, float pan)
        {
            if (PlaySoundEffects)
            {
                return soundEffect.Play(volume, pitch, pan);
            }
            return false;
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

            // Partikeltextur laden (hier: Partikeltextur generieren)
            _ParticleTextures = (
                new Texture2D(GraphicsDevice, 1, 1),
                Content.Load<Texture2D>("Sprites/smoke"));
            _ParticleTextures.Ember.SetData(new[] { Color.White });

            // Partikeleffektliste
            _ParticleEffects = new List<CustomParticleEffect>();

            // Sounds laden
            _SoundEffects = (
                Content.Load<SoundEffect>("Sounds/Fire"),
                Content.Load<SoundEffect>("Sounds/Walk_fast"),
                Content.Load<SoundEffect>("Sounds/Fill"),
                Content.Load<SoundEffect>("Sounds/Coin"));

            // Texturen für Icons laden
            _IconTextures = (
                Content.Load<Texture2D>("Sprites/icon_sound_on"),
                Content.Load<Texture2D>("Sprites/icon_sound_off"));

            _MapGrid = new MapGrid(Window.ClientBounds.Width, Window.ClientBounds.Height);

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
            RegisterParticleEffect(CustomParticleEffect.CreateEmberParticleEffect(_ParticleTextures.Ember, _Lake.Position + new Vector2(lakeTextureSize.Width / 2, lakeTextureSize.Height / 2), _Lake.GetCollisionBoxSize()));


            //Player

            Size playerTextureSize = new Size(40, 56);
            _DefaultPlayerVector = new Vector2(Window.ClientBounds.Width / 2 - lakeTextureSize.Width / 2 - 2 - playerTextureSize.Width, Window.ClientBounds.Height / 2 - playerTextureSize.Height);
            sheet = new Spritesheet.Spritesheet(Content.Load<Texture2D>("Sprites/player_spritesheet")).WithGrid((playerTextureSize.Width, playerTextureSize.Height), (0,0), (0,0));
            _Player = new Player(_DefaultPlayerVector, playerTextureSize);
            _Player.AddAnimation(PlayerAnimation.Idle, sheet.CreateAnimation((0, 0)));
            (int x, int y)[] animFront = { (0, 0), (1, 0), (0, 0), (2, 0) };
            (int x, int y)[] animBack = { (0, 1), (1, 1), (0, 1), (2, 1) };
            _Player.AddAnimation(PlayerAnimation.LookingUpRight, sheet.CreateAnimation(animBack));
            _Player.AddAnimation(PlayerAnimation.LookingRightDown, sheet.CreateAnimation(animFront));
            _Player.AddAnimation(PlayerAnimation.LookingDownLeft, sheet.WithFrameEffect(SpriteEffects.FlipHorizontally).CreateAnimation(animFront));
            _Player.AddAnimation(PlayerAnimation.LookingLeftUp, sheet.WithFrameEffect(SpriteEffects.FlipHorizontally).CreateAnimation(animBack));

            //Flower
            _ActiveFlowerCount = 2;
            _FlowerList = new List<Flower>();
            SpawnFlower(10, Flower.HEALTH_DEAD);
            SpawnFlower(_ActiveFlowerCount);

            //Powerup
            _Powerup = null;
            _NextPowerupSpawn = default(DateTime);

            //FadingText
            _TextList = new List<FadingText>();

            _TextureBackGround = Content.Load<Texture2D>("Sprites/background");

            _CurrentGameState = GameState.Running;
        }

        private void SpawnRandomPowerup()
        {
            Spritesheet.Spritesheet sheet = new Spritesheet.Spritesheet(Content.Load<Texture2D>("Sprites/items_spritesheet")).WithGrid((Flower.FlowerTextureSize.Width, Flower.FlowerTextureSize.Height), (0, 0), (0, 0));
            var animationBrownBoot = sheet.CreateAnimation((0, 1));
            var animationGoldenBoot = sheet.CreateAnimation((2, 1));
            var animationRedHeart = sheet.CreateAnimation((1, 0));
            var animationBomb = sheet.CreateAnimation((1, 1));
            var animationTorch = sheet.CreateAnimation((3, 0));

            APowerupBase powerup;

            int p = rnd.Next(0, (int) PowerUpAnimation.Last);
            switch ((PowerUpAnimation)p)
            {
                case PowerUpAnimation.Torch:
                    powerup = new TorchPowerup();
                    powerup.AddAnimation(0, animationTorch.Clone());
                    break;
                case PowerUpAnimation.RedHeart:
                    powerup = new HeartPowerup();
                    powerup.AddAnimation(0, animationRedHeart.Clone());
                    break;
                case PowerUpAnimation.Bomb:
                    powerup = new BombPowerup();
                    powerup.AddAnimation(0, animationBomb.Clone());
                    break;
                case PowerUpAnimation.GoldenBoot:
                    powerup = new SpeedPowerup(2.5);
                    powerup.AddAnimation(0, animationGoldenBoot.Clone());
                    break;
                case PowerUpAnimation.BrownBoot:
                default:
                    powerup = new SpeedPowerup(1.5);
                    powerup.AddAnimation(0, animationBrownBoot.Clone());
                    break;
            }

            var vec = _MapGrid.GetFreePosition(powerup.GetCollisionBoxSize());
            powerup.Position = vec;

            _Powerup = powerup;
        }

        private void SpawnFlower(int count, int health = Flower.HEALTH_MAX)
        {
            Spritesheet.Spritesheet sheet = new Spritesheet.Spritesheet(Content.Load<Texture2D>("Sprites/skull_spritesheet")).WithGrid((Flower.FlowerTextureSize.Width, Flower.FlowerTextureSize.Height), (0,0), (0,0));
            var animationAlive = sheet.CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0));
            var animationSick = sheet.CreateAnimation((0, 1), (1, 1), (2, 1), (3, 1));
            var animationDead = sheet.CreateAnimation((0, 2));

            for (int i = 0; i < count; i++)
            {
                var flower = new Flower(health);
                var vec = _MapGrid.GetFreePosition(flower.GetCollisionBoxSize());
                flower.Position = vec;

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

            if (Keyboard.GetState().IsKeyDown(Keys.M) && !_PreviousKeyboardState.IsKeyDown(Keys.M))
            {
                PlaySoundEffects = !PlaySoundEffects;
            }

#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.D)
                                                                && !_PreviousKeyboardState.IsKeyDown(Keys.D))
            {
                DebugMode = !DebugMode;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.N) && !_PreviousKeyboardState.IsKeyDown(Keys.N))
            {
                SpawnFlower(1);
            }
#endif

            if (_CurrentGameState == GameState.Running)
            {
                int currentGameTime = (int)(DateTime.Now - _StartTime).TotalSeconds;

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

                foreach (var flower in _FlowerList.Where(v => v.Health > 0))
                {
                    flower.Update(gameTime);
                    if (updateFlowerHealth)
                    {
                        int dmg = 1 + (int)Math.Floor((double)currentGameTime / 20);

                        flower.Health -= dmg;
                        _TextList.Add(new FadingText(_Font, "-" + dmg, flower.Position + new Vector2(flower.TextureSize.Width / 2, flower.TextureSize.Height / 2 - 5), Color.White));

                        if (flower.Health <= 0)
                        {
                            _Player.Life--;
                            RegisterParticleEffect(CustomParticleEffect.CreateSmokeParticleEffect(_ParticleTextures.Smoke, flower.Position + new Vector2(Flower.FlowerTextureSize.Width / 2, Flower.FlowerTextureSize.Height / 2), flower.GetCollisionBoxSize()), 30);
                        }
                    }
                }

                if (_Player.Life <= 0)
                {
                    _TimeSurvived = DateTime.Now - _StartTime;
                    _CurrentGameState = GameState.GameOver;
                }

                if (currentGameTime / 20 + 1> _ActiveFlowerCount)
                {
                    _ActiveFlowerCount++;
                    SpawnFlower(1);
                    SpawnFlower(1, Flower.HEALTH_DEAD);
                }

                //Spawn Powerup
                if (_Powerup == null && _NextPowerupSpawn < DateTime.Now)
                {
                    SpawnRandomPowerup();
                }

                // Partikeleffekte updaten
                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                List<CustomParticleEffect> markedForRemoval = new List<CustomParticleEffect>();

                foreach (var entry in _ParticleEffects)
                {
                    entry.ParticleEffect.Update(deltaTime);
                    if (entry.TimeLeft > 0)
                    {
                        entry.TimeLeft--;
                    }
                    if (entry.TimeLeft == 0)
                    {
                        // Zum Löschen markieren
                        foreach (var emitter in entry.ParticleEffect.Emitters)
                        {
                            emitter.Parameters.Quantity = 0;
                        }
                    }

                    foreach (var emitter in entry.ParticleEffect.Emitters)
                    {
                        if (emitter.Parameters.Quantity == 0 && emitter.ActiveParticles == 0)
                        {
                            markedForRemoval.Add(entry);
                            break;
                        }
                    }
                }

                foreach (var entry in markedForRemoval)
                {
                    _ParticleEffects.Remove(entry);
                }

                _PreviousKeyboardState = Keyboard.GetState();
                base.Update(gameTime);
            }
        }

        private void UpdatePlayerPosition()
        {
            bool isWalking = false;
            for (int i = 0; i < _Player.Speed; i++)
            {
                Vector2 offsetX = Vector2.Zero;
                Vector2 offsetY = Vector2.Zero;
                var kbState = Keyboard.GetState();
                bool idle = true;

                if (kbState.IsKeyDown(Keys.Up) || kbState.IsKeyDown(Keys.W))
                {
                    _Player.VDirection = true;
                    offsetY += new Vector2(0, -1);
                    idle = false;
                }
                if (kbState.IsKeyDown(Keys.Left) || kbState.IsKeyDown(Keys.A))
                {
                    _Player.HDirection = false;
                    offsetX += new Vector2(-1, 0);
                    idle = false;
                }
                if (kbState.IsKeyDown(Keys.Down) || kbState.IsKeyDown(Keys.S))
                {
                    _Player.VDirection = false;
                    offsetY += new Vector2(0, 1);
                    idle = false;
                }
                if (kbState.IsKeyDown(Keys.Right) || kbState.IsKeyDown(Keys.D))
                {
                    _Player.HDirection = true;
                    offsetX += new Vector2(1, 0);
                    idle = false;
                }
                if (idle)
                {
                    _Player.Idle();
                    break;
                }

                var validOffsets = new List<Vector2>();


                foreach (var offset in new Vector2[] { offsetX, offsetY })
                {
                    var newPlayerPos = _Player.Position + offset;
                    if (newPlayerPos == _Player.Position)
                    {
                        continue;
                    }

                    var newCollissionBox = _Player.CalcCollissionBoxRect(newPlayerPos);
                    bool collisionWithFlower = false;

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
                                _TextList.Add(new FadingText(_Font, "+" + heal, flower.Position + new Vector2(flower.TextureSize.Width / 2, flower.TextureSize.Height / 2 - 5), Color.White));
                                flower.Health += heal;
                                _TextList.Add(new FadingText(_Font, "-" + heal, _Player.Position + new Vector2(_Player.TextureSize.Width / 2, _Player.TextureSize.Height / 2 - 5), Color.White));
                                _Player.Water -= heal;

                                if (heal > 0)
                                {
                                    PlaySoundEffect(_SoundEffects.FlowerGrow, 0.5f, -0.2f);
                                    RegisterParticleEffect(CustomParticleEffect.CreateEmberParticleEffect(_ParticleTextures.Ember, flower.Position + new Vector2(Flower.FlowerTextureSize.Width / 2, Flower.FlowerTextureSize.Height / 2), flower.GetCollisionBoxSize()), 30);
                                }
                            }

                            collisionWithFlower = true;
                        }

                        if (_Player.CollisionBox.Intersects(flower.CollisionBox))
                        {
                            _Player.Position = _DefaultPlayerVector;
                            return;
                        }
                    }

                    if (collisionWithFlower)
                    {
                        continue;
                    }

                    //Kollision See
                    if (newCollissionBox.Intersects(_Lake.CollisionBox))
                    {
                        if (_Player.Water != Player.MAX_WATER)
                        {
                            int missing = Player.MAX_WATER - _Player.Water;
                            _TextList.Add(new FadingText(_Font, "+" + missing, _Player.Position + new Vector2(_Player.TextureSize.Width / 2, _Player.TextureSize.Height / 2 - 5), Color.White));
                            _Player.Water = Player.MAX_WATER;
                            PlaySoundEffect(_SoundEffects.GatherWater, 0.3f);
                        }
                        continue;
                    }

                    //Killision Powerup
                    if (_Powerup != null && newCollissionBox.Intersects(_Powerup.CollisionBox))
                    {
                        _Powerup.Consume(_Player, _FlowerList);
                        PlaySoundEffect(_SoundEffects.CollectItem, 0.3f);
                        _TextList.Add(new FadingText(_Font, _Powerup.PowerupName, _Player.Position + new Vector2(_Player.TextureSize.Width / 2, _Player.TextureSize.Height / 2 - 5), Color.White));

                        int next = rnd.Next(5, 20);
                        _NextPowerupSpawn = DateTime.Now.AddSeconds(next);
                        _Powerup = null;
                    }

                    //Kollision Kanten
                    if (newPlayerPos.X >= 0 && newPlayerPos.Y >= 0
                    && newCollissionBox.Right <= Window.ClientBounds.Width && newCollissionBox.Bottom <= Window.ClientBounds.Height)
                    {
                        _Player.Position += offset;
                        isWalking = true;
                    }

                    //Aufraumen
                    for (var i2 = 0; i2 < _TextList.Count; i2++)
                    {
                        if (_TextList[i2].CanDelete)
                        {
                            _TextList.RemoveAt(i2);
                        }
                    }
                }
            }

            if (isWalking)
            {
                if (_Player.WalkSoundCounter == 0)
                {
                    PlaySoundEffect(_SoundEffects.PlayerWalk, (float)rnd.NextDouble() * 0.05f + 0.1f, (float)rnd.NextDouble() * 0.2f);
                }

                _Player.WalkSoundCounter += _Player.Speed;
                if (_Player.WalkSoundCounter >= 60)
                {
                    _Player.WalkSoundCounter = 0;
                }
            }
            else
            {
                _Player.WalkSoundCounter = 0;
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

            if (_CurrentGameState == GameState.Running)
            {
                _SpriteBatch.Draw(_TextureBackGround, Vector2.Zero, Color.White);
                _Lake.Draw(_SpriteBatch);

                //if (DebugMode)
                //{
                //    foreach (var v in _MapGrid._GridArr)
                //    {
                //        _SpriteBatch.DrawRectangle(v.Item1, Color.Aqua);
                //    }
                //}

                var drawList = new List<AEntity>();
                drawList.AddRange(_FlowerList);
                drawList.Add(_Player);
                if (_Powerup != null)
                {
                    drawList.Add(_Powerup);
                }

                foreach (var i in drawList.OrderBy(v => v.Position.Y))
                {
                    i.Draw(_SpriteBatch);
                }

                foreach (var fadingText in _TextList)
                {
                    fadingText.Draw(_SpriteBatch);
                }

                foreach (var entry in _ParticleEffects)
                {
                    _SpriteBatch.Draw(entry.ParticleEffect);
                }


                float textY = 0;
                Vector2 textVec;
                string text;

                //Rundenzeit
                text = (DateTime.Now - _StartTime).ToString(@"hh\:mm\:ss");
                textVec = _Font.MeasureString(text);
                _SpriteBatch.DrawString(_Font, text, new Vector2(0, textY), Color.White);
                textY += textVec.Y + 5;

                //Anzahl Leben
                text = "Life: " + _Player.Life;
                textVec = _Font.MeasureString(text);
                _SpriteBatch.DrawString(_Font, text, new Vector2(0, textY), Color.White);
                textY += textVec.Y + 5;

                //Lava
                _SpriteBatch.DrawString(_Font, "Lava: " + _Player.Water, new Vector2(0, textY), Color.White);

                // Sound-Status
                _SpriteBatch.Draw(PlaySoundEffects ? _IconTextures.SoundOn : _IconTextures.SoundOff, new Vector2(Window.ClientBounds.Width - 64 - 8, 8), Color.White);
            }
            else if (_CurrentGameState == GameState.GameOver)
            {
                string text = "GAME OVER";
                var textSize = _Font.MeasureString(text);
                var textVec = new Vector2(Window.ClientBounds.Width / 2 - textSize.X / 2, Window.ClientBounds.Height / 2 - textSize.Y /2);
                _SpriteBatch.DrawString(_Font, text, textVec, Color.White);

                text = "Awesome :) You survived " + _TimeSurvived.ToString(@"hh\:mm\:ss") + "!";
                textSize = _Font.MeasureString(text);
                textVec = new Vector2(Window.ClientBounds.Width / 2 - textSize.X / 2, textVec.Y + textSize.Y + 5);
                _SpriteBatch.DrawString(_Font, text, textVec, Color.White);

                text = "Press R to Restart";
                textSize = _Font.MeasureString(text);
                textVec = new Vector2(Window.ClientBounds.Width / 2 - textSize.X / 2, textVec.Y + textSize.Y + 20);
                _SpriteBatch.DrawString(_Font, text, textVec, Color.White);
            }

            _SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
