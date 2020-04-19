using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Spritesheet;

namespace ld46.Classes
{
    sealed class Player : AEntity
    {
        public const int MAX_WATER = 1000;

        public override Size TextureSize { get; protected set; }

        public int Water { get; set; }

        private Spritesheet.Spritesheet test;

        public Player(Vector2 position, Size textureSize, Spritesheet.Spritesheet spritesheet)
        {
            test = spritesheet;
            _Position = position;
            TextureSize = textureSize;
            Water = MAX_WATER;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CurrentAnimationIndex = 0;
            if (!CurrentAnimation.IsStarted)
            {
                CurrentAnimation.Start(Repeat.Mode.Loop);
            }

            spriteBatch.Draw(CurrentAnimation, _Position);
#if DEBUG
            spriteBatch.DrawRectangle(CollisionBox, Color.Green);
#endif
        }
    }
}
