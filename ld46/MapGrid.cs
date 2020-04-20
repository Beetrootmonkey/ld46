using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace ld46
{
    class MapGrid
    {
        public const int GRIDSIZE = 64;
        private const int MIN_BORDER = 20;
        private readonly Random _Random = new Random();

        private readonly int _BorderX;
        private readonly int _BorderY;

        private readonly int _GridW;
        private readonly int _GridH;

        public (RectangleF, bool)[] _GridArr;

        public MapGrid(int mapWidth, int mapHeight)
        {
            int wRest = ((mapWidth - MIN_BORDER * 2) % GRIDSIZE) / 2;
            _BorderX = MIN_BORDER + wRest;
            mapWidth -= _BorderX;
            _GridW = mapWidth / GRIDSIZE;
            
            int hRest = ((mapHeight - MIN_BORDER * 2) % GRIDSIZE) / 2;
            _BorderY = MIN_BORDER + hRest;
            mapHeight -= _BorderY;
            _GridH = mapHeight / GRIDSIZE;

            _GridArr = new (RectangleF, bool)[_GridW * _GridH];
            for (int w = 0; w < _GridW; w++)
            {
                for (int h = 0; h < _GridH; h++)
                {
                    _GridArr[w*_GridH+h] = (new RectangleF(_BorderX + w * GRIDSIZE, _BorderY + h * GRIDSIZE, GRIDSIZE, GRIDSIZE), false);
                }
            }
        }

        public Vector2 GetFreePosition(Size size)
        {
            var freeTiles = _GridArr.Where(v => !v.Item2 
                                                && !v.Item1.Intersects(Game1._Player.CollisionBox)
                                                && !v.Item1.Intersects(Game1._Lake.CollisionBox)).ToList();
            if (freeTiles.Count == 0)
            {
                return Vector2.Zero;
            }

            int rdmTileIndex = _Random.Next(0, freeTiles.Count - 1);
            int rdmXPos = _Random.Next(0, GRIDSIZE - size.Width - 1);
            int rdmYPos = _Random.Next(0, GRIDSIZE - size.Height - 1);

            var randomTile = freeTiles[rdmTileIndex];

            for (int i = 0; i < _GridArr.Length; i++)
            {
                var t = _GridArr[i];
                if (t.Item1 == randomTile.Item1)
                {
                    var newPos = new Vector2(t.Item1.X + rdmXPos, t.Item1.Y + rdmYPos);
                    _GridArr[i].Item2 = true;
                    return newPos;
                }
            }

            return Vector2.Zero;
        }
    }
}
