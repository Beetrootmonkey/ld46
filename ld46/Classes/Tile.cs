using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ld46.Classes
{
    class Tile
    {
        public Texture2D Texture { get; set; }

        public Tile(Texture2D texture)
        {
            this.Texture = texture;
        }
    }
}
