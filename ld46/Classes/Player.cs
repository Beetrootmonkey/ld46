using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Spritesheet;

namespace ld46.Classes
{
    public enum PlayerAnimation
    {
        Idle,
        LookingUp,
        LookingUpRight,
        LookingRight,
        LookingRightDown,
        LookingDown,
        LookingDownLeft,
        LookingLeft,
        LookingLeftUp
    }

    sealed class Player : AEntity
    {
        public const int MAX_WATER = 1000;

        public override Size TextureSize { get; protected set; }

        public int Water { get; set; }

        private bool _CurrentHDirection;
        public bool HDirection
        {
            get => _CurrentHDirection;
            set
            {
                _CurrentHDirection = value;
                ChangeDirection();
            }
        }
        private bool _CurrentVDirection;
        public bool VDirection
        {
            get => _CurrentVDirection;
            set
            {
                _CurrentVDirection = value;
                ChangeDirection();
            }
        }

        public Player(Vector2 position, Size textureSize)
        {
            _Position = position;
            TextureSize = textureSize;
            Water = MAX_WATER;
        }

        public void AddAnimation(PlayerAnimation f, Animation a)
        {
            AnimationDictionary[(int) f] = a;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!CurrentAnimation.IsStarted)
            {
                CurrentAnimation.Start(Repeat.Mode.Loop);
            }

            spriteBatch.Draw(CurrentAnimation, _Position);

            if (Game1.DebugMode)
            {
                spriteBatch.DrawRectangle(CollisionBox, Color.Green);
            }
        }

        public void Idle()
        {
            CurrentAnimation.Stop();
            CurrentAnimation.Reset();
        }

        private void ChangeDirection()
        {
            if (VDirection && HDirection)
            {
                ChangeAnimation(PlayerAnimation.LookingUpRight);
            }
            else if (!VDirection && HDirection)
            {
                ChangeAnimation(PlayerAnimation.LookingRightDown);
            }
            else if (VDirection && !HDirection)
            {
                ChangeAnimation(PlayerAnimation.LookingLeftUp);
            }
            else if (!VDirection && !HDirection)
            {
                ChangeAnimation(PlayerAnimation.LookingDownLeft);
            }
        }

        private void ChangeAnimation(PlayerAnimation newAnimation)
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

        public override Rectangle CalcCollissionBox(Vector2 v)
        {
            int h = TextureSize.Height / 2;
            float y = v.Y + h;

            return new Rectangle((int) (v.X), (int) (y), TextureSize.Width, h);
        }
    }
}
