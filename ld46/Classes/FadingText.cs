using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld46.Classes
{
    public class FadingText
    {
        private readonly SpriteFont _Font;
        private readonly string _Text;
        private Color _Color;
        private Vector2 _Position;

        public bool CanDelete { get; set; }

        public FadingText(SpriteFont font, string text, Vector2 position, Color color)
        {
            _Font = font;
            _Text = text;
            _Position = position;
            _Color = color;

            var textSize = _Font.MeasureString(_Text);
            _Position.X -= textSize.X / 2;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (CanDelete || _Color.A <= 0)
            {
                CanDelete = true;
                return;
            }

            spriteBatch.DrawString(_Font, _Text, _Position, _Color);
            _Position.Y--;

            int dec = 8;
            _Color = new Color(_Color.R - dec, _Color.G - dec, _Color.B - dec, _Color.A - dec);
        }
    }
}
