using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Spritesheet;

namespace ld46.Classes
{
    abstract class APowerupBase : AEntity
    {
        public static Size PowerupTextureSize = new Size(30, 30);
        public sealed override Size TextureSize { get; protected set; }
        
        protected APowerupBase()
        {
            Position = new Vector2(-1000, -1000);
            TextureSize = PowerupTextureSize;
        }

        public abstract void Consume(Player p);

        public override void Draw(SpriteBatch spriteBatch)
        {
            //CurrentAnimationIndex = 0;
            //if (!CurrentAnimation.IsStarted)
            //{
            //    CurrentAnimation.Start(Repeat.Mode.Loop);
            //}
            spriteBatch.FillRectangle(Position, new Size2(PowerupTextureSize.Width,PowerupTextureSize.Height), Color.Blue);

            if (Game1.DebugMode)
            {
                spriteBatch.DrawRectangle(CollisionBox, Color.Green);
            }
        }
    }
}
