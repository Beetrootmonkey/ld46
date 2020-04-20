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

        public (Vector2, bool)[] _GridArr;

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

            _GridArr = new (Vector2, bool)[_GridW * _GridH];
            for (int w = 0; w < _GridW; w++)
            {
                for (int h = 0; h < _GridH; h++)
                {
                    _GridArr[w*_GridH+h] = (new Vector2(_BorderX + w * GRIDSIZE, _BorderY + h * GRIDSIZE), false);
                }
            }
        }

        public Vector2 GetFreePosition(Size size)
        {
            int freeTileCount = _GridArr.Count(v => !v.Item2);
            if (freeTileCount == 0)
            {
                return Vector2.Zero;
            }

            int rdmTileIndex = _Random.Next(0, freeTileCount - 1);
            int rdmXPos = _Random.Next(0, GRIDSIZE - size.Width - 1);
            int rdmYPos = _Random.Next(0, GRIDSIZE - size.Height - 1);

            int count = 0;
            for (int i = 0; i < _GridArr.Length; i++)
            {
                if (!_GridArr[i].Item2)
                {
                    if (count == rdmTileIndex)
                    {
                        var newPos = _GridArr[i].Item1 + new Vector2(rdmXPos, -rdmYPos);

                        _GridArr[i].Item2 = true;
                        return newPos;
                    }
                    count++;
                }
            }
            return Vector2.Zero;
        }
    }
}
