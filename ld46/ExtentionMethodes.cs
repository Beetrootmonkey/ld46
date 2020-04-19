using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ld46
{
    public enum CollisionSide
    {
        None,
        Top,
        Right,
        Bottom,
        Left
    }

    static class ExtentionMethodes
    {
        private static CollisionSide Intersects(this Rectangle self, Rectangle other)
        {
            if (other.Right >= self.Left && other.Left < self.Left)
            {
                return CollisionSide.Left;
            }

            if (other.Left <= self.Left && other.Right > self.Right)
            {
                return CollisionSide.Right;
            }

            if (other.Top >= self.Bottom && other.Bottom < self.Bottom)
            {
                return CollisionSide.Top;
            }

            if (other.Bottom <= self.Bottom && other.Top > self.Top)
            {
                return CollisionSide.Bottom;
            }

            return CollisionSide.None;
        }
    }
}
