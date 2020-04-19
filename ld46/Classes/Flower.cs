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
        public const int HEALTH_MAX = 100;
        private const int HEALTH_NORMAL = 80;
        private const int HEALTH_CRITICAL = 25;
        private const int HEALTH_DEAD = 0;

        public override Size TextureSize { get; protected set; }

        private int _Health;
        public int Health {
            get => _Health;
            set
            {
                if (_Health > 0)
                {
                    int newValue = value < HEALTH_MAX ? value : HEALTH_MAX;
                    _Health = newValue;
                }
            }
        }

        public Flower(Vector2 position, Size textureSize)
        {
            _Health = HEALTH_MAX;
            _Position = position;
            TextureSize = textureSize;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_Health > HEALTH_DEAD)
            {
                DrawHealthbar(spriteBatch);
            }
            
            DrawFlower(spriteBatch);
            spriteBatch.Draw(CurrentAnimation, _Position);
            if (Game1.DebugMode)
            {
                spriteBatch.DrawRectangle(CollisionBox, Color.Green);
            }
        }

        public void AddAnimation(FlowerAnimation f, Animation a)
        {
            AnimationDictionary[(int) f] = a;
        }

        private void DrawHealthbar(SpriteBatch spriteBatch)
        {
            var healthbarVector = _Position + new Vector2(0, -20);
            var healthbarBgRect = new Rectangle((int)healthbarVector.X, (int)healthbarVector.Y, TextureSize.Width, 15);
                
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
    }
}
