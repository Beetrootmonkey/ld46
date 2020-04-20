using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Spritesheet;

namespace ld46.Classes
{
    abstract class HealthBarColors
    {
        static Color[] colors;

        public static void Init(Color[] colors)
        {
            HealthBarColors.colors = colors;
        }

        public static void Init(Texture2D texture) => Init(GetColorsFromTexture(texture));

        public static Color GetColorFromIndex(float index)
        {
            int colorIndex = MathHelper.Clamp((int) Math.Round(index * colors.Length), 0, colors.Length - 1);
            return colors[colorIndex];
        }

        public static Color[] GetColorsFromTexture(Texture2D texture)
        {
            Color[] colors = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(colors);
            return colors;
        }
    }
}
