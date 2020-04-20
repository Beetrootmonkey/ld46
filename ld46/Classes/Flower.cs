using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Spritesheet;

namespace ld46.Classes
{
    public enum FlowerAnimation
    {
        Alive,
        Sick,
        Dead
    }

    sealed class Flower : AEntity
    {
        public static Size FlowerTextureSize = new Size(54, 58);

        public const int HEALTH_MAX = 100;
        public const int HEALTH_NORMAL = 80;
        public const int HEALTH_CRITICAL = 25;
        public const int HEALTH_DEAD = 0;

        public override Size TextureSize { get; protected set; }

        private int _Health;
        public int Health {
            get => _Health;
            set
            {
                if (_Health > 0)
                {
                    _Health = MathHelper.Clamp(value, HEALTH_DEAD, HEALTH_MAX);
                }
            }
        }

        public Flower(int health = HEALTH_MAX)
        {
            _Health = health;
            Position = new Vector2(-1000, -1000);
            TextureSize = FlowerTextureSize;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_Health > HEALTH_DEAD)
            {
                DrawHealthbar(spriteBatch);
            }
            
            DrawFlower(spriteBatch);
            spriteBatch.Draw(CurrentAnimation, Position);
            if (Game1.DebugMode)
            {
                spriteBatch.DrawRectangle(CollisionBox, Color.Green);
                //spriteBatch.FillRectangle(new Rectangle((int) _Position.X - 2, (int)_Position.Y - 2, 4, 4), Color.Blue);
            }
        }

        public void AddAnimation(FlowerAnimation f, Animation a)
        {
            AnimationDictionary[(int) f] = a;
        }

        private void DrawHealthbar(SpriteBatch spriteBatch)
        {
            var healthbarVector = Position + new Vector2(0, TextureSize.Height);
            var healthbarBgRect = new Rectangle((int)healthbarVector.X, (int)healthbarVector.Y, TextureSize.Width, 8);
                
            var w = TextureSize.Width * _Health / 100;
            var healthbarRect = new RectangleF(healthbarBgRect.X, healthbarBgRect.Y, w, healthbarBgRect.Height);

            var healthbarRahmen = new Rectangle(healthbarBgRect.X - 1, healthbarBgRect.Y - 1, healthbarBgRect.Width + 2, healthbarBgRect.Height + 2);

            Color healthbarColor;

            //if (_Health >= HEALTH_NORMAL)
            //{
            //    healthbarColor = new Color(117,237,0);
            //}
            //else if (_Health >= HEALTH_CRITICAL)
            //{
            //    healthbarColor = new Color(255,188,0);
            //}
            //else
            //{
            //    healthbarColor = new Color(255,91,0);
            //}
            healthbarColor = HealthBarColors.GetColorFromIndex(1 - (float) _Health / HEALTH_MAX);

            spriteBatch.FillRectangle(healthbarBgRect, new Color(101,91,89));
            spriteBatch.DrawRectangle(healthbarRahmen, new Color(165,148,146));
            spriteBatch.FillRectangle(healthbarRect, healthbarColor);
        }

        private void DrawFlower(SpriteBatch spriteBatch)
        {
            if (_Health >= HEALTH_NORMAL)
            {
                ChangeAnimation(FlowerAnimation.Alive);
                return;
            }
            
            if(_Health > 0)
            {
                ChangeAnimation(FlowerAnimation.Sick);
                return;
            }

            ChangeAnimation(FlowerAnimation.Dead);
        }

        private void ChangeAnimation(FlowerAnimation newAnimation)
        {
            if (CurrentAnimationIndex == (int)newAnimation)
            {
                if (CurrentAnimation.IsStarted)
                {
                    return;
                }
            }
            else
            {
                CurrentAnimationIndex = (int)newAnimation;
            }

            CurrentAnimation.Start(Repeat.Mode.Loop);
        }

        public override Size GetCollisionBoxSize()
        {
            return new Size(TextureSize.Width, TextureSize.Height / 2);
        }

        public override Rectangle CalcCollissionBoxRect(Vector2 v)
        {
            var colSize = GetCollisionBoxSize();
            float y = v.Y + colSize.Height;

            return new Rectangle((int) (v.X), (int) (y), colSize.Width, colSize.Height);
        }
    }
}
