using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ld46.Classes
{
    abstract class ATile
    {
        public abstract void Draw(SpriteBatch spriteBatch, int i, int j, int tileSize, float tileScale, int mapOffsetX, int mapOffsetY);
    }
}
