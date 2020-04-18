using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace ld46.Classes
{
    class AreaTile : ATile
    {
        public Color Color { get; set; }

        public AreaTile(Color color)
        {
            this.Color = color;
        }

        public override void Draw(SpriteBatch spriteBatch, int i, int j, int tileSize, float tileScale, int mapOffsetX, int mapOffsetY)
        {
            var col = Color;
            col.A = 30;
            spriteBatch.FillRectangle(i * tileSize * tileScale + mapOffsetX, j * tileSize * tileScale + mapOffsetY, tileSize * tileScale, tileSize * tileScale, col);
        }
    }
}
