using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ld46.Classes
{
    class WallTile : ATile
    {
        public Texture2D Texture { get; set; }

        public WallTile(Texture2D texture)
        {
            this.Texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch, int i, int j, int tileSize, float tileScale, int mapOffsetX, int mapOffsetY)
        {
            spriteBatch.Draw(Texture, new Vector2(i * tileSize * tileScale + mapOffsetX, j * tileSize * tileScale + mapOffsetY), null, Color.White, 0, Vector2.Zero, tileScale, SpriteEffects.None, 0);
        }
    }
}
