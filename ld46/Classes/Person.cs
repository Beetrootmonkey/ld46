using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ld46.Classes
{
    class Person
    {
        public Vector2 position { get; set; }

        public Person(Vector2 position)
        {
            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, float tileScale, int mapOffsetX, int mapOffsetY)
        {
            Vector2 offsetPosition = position - new Vector2(31, 54);
            Vector2 screenPos = new Vector2(offsetPosition.X * tileScale + mapOffsetX, offsetPosition.Y * tileScale + mapOffsetY);
            spriteBatch.Draw(texture, screenPos, null, Color.White, 0, Vector2.Zero, tileScale, SpriteEffects.None, 0);
        }
    }
}
