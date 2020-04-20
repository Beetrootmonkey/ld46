using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Spritesheet;

namespace ld46.Classes
{
    sealed class Powerup : AEntity
    {
        public override Size TextureSize { get; protected set; }
        
        public Powerup(Vector2 position, Size textureSize)
        {
            Position = position;
            TextureSize = new Size(30,30);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CurrentAnimationIndex = 0;
            if (!CurrentAnimation.IsStarted)
            {
                CurrentAnimation.Start(Repeat.Mode.Loop);
            }
            spriteBatch.FillRectangle(Position, new Size2(5,5), Color.Blue);

            if (Game1.DebugMode)
            {
                spriteBatch.DrawRectangle(CollisionBox, Color.Green);
            }
        }
    }
}
