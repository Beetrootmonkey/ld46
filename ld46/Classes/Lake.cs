using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Spritesheet;

namespace ld46.Classes
{
    public sealed class Lake : AEntity
    {
        public override Size TextureSize { get; protected set; }
        
        public Lake(Vector2 position, Size textureSize)
        {
            Position = position;
            TextureSize = textureSize;
        }

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
    }
}
