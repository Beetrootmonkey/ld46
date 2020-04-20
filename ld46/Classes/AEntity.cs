using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Spritesheet;

namespace ld46.Classes
{
    public abstract class AEntity
    {
        public abstract Size TextureSize { get; protected set; }
        public abstract void Draw(SpriteBatch spriteBatch);

        public Vector2 Position { get; set; }
        public Rectangle CollisionBox => CalcCollissionBoxRect(Position);

        public int CurrentAnimationIndex { get; set; }
        public Dictionary<int, Animation> AnimationDictionary { get; }
        public Animation CurrentAnimation => AnimationDictionary[CurrentAnimationIndex];

        public AEntity()
        {
            AnimationDictionary = new Dictionary<int, Animation>();
        }

        public virtual Size GetCollisionBoxSize()
        {
            return new Size(TextureSize.Width, TextureSize.Height);
        }

        public virtual Rectangle CalcCollissionBoxRect(Vector2 v)
        {
            var colSize = GetCollisionBoxSize();
            return new Rectangle((int) (v.X), (int) (v.Y), colSize.Width, colSize.Height);
        }

        public void Update(GameTime gt)
        {
            AnimationDictionary[CurrentAnimationIndex].Update(gt);
        }
    }
}
