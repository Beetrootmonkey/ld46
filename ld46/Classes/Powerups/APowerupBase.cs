using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Spritesheet;

namespace ld46.Classes
{
    public enum PowerUpAnimation
    {
        BluePearl,
        RedHeart,
        GoldenFruit,
        BrownBoot,
        BlackBoot
    }

    abstract class APowerupBase : AEntity
    {
        public static Size PowerupTextureSize = new Size(54, 58);
        public sealed override Size TextureSize { get; protected set; }
        
        protected APowerupBase()
        {
            Position = new Vector2(-1000, -1000);
            TextureSize = PowerupTextureSize;
        }

        public abstract void Consume(Player p);

        public override void Draw(SpriteBatch spriteBatch)
        {
            CurrentAnimationIndex = 0;
            if (!CurrentAnimation.IsStarted)
            {
                CurrentAnimation.Start(Repeat.Mode.Loop);
            }
            spriteBatch.Draw(CurrentAnimation, Position);

            if (Game1.DebugMode)
            {
                spriteBatch.DrawRectangle(CollisionBox, Color.Green);
            }
        }

        public void AddAnimation(PowerUpAnimation f, Animation a)
        {
            AnimationDictionary[(int)f] = a;
        }

        public override Size GetCollisionBoxSize()
        {
            return new Size(TextureSize.Width, TextureSize.Height);
        }
    }
}
