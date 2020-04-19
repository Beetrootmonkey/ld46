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
    abstract class AEntity
    {
        public abstract Size TextureSize { get; protected set; }
        public abstract void Draw(SpriteBatch spriteBatch);

        public Vector2 _Position { get; set; }
        public Rectangle CollisionBox => CalcCollissionBox(_Position);

        public int CurrentAnimationIndex { get; set; }
        public Dictionary<int, Animation> AnimationDictionary { get; }
        public Animation CurrentAnimation => AnimationDictionary[CurrentAnimationIndex];

        public AEntity()
        {
            AnimationDictionary = new Dictionary<int, Animation>();
        }

        public virtual Rectangle CalcCollissionBox(Vector2 v)
        {
            return new Rectangle((int) (v.X), (int) (v.Y), TextureSize.Width, TextureSize.Height);
        }

        public void Update(GameTime gt)
        {
            AnimationDictionary[CurrentAnimationIndex].Update(gt);
        }
    }
}
