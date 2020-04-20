using System;
using System.Collections.Generic;
using System.Linq;

namespace ld46.Classes
{
    class BombPowerup : APowerupBase
    {
        public override string PowerupName => _PowerupName;

        private string _PowerupName;
        private Random _Random = new Random();

        public override void Consume(Player p, List<Flower> f)
        {
            int countDead = f.Count(v => v.Health == Flower.HEALTH_DEAD);
            if (countDead == 0)
            {
                _PowerupName = "DUD :P";
                return;
            }

            _PowerupName = "BOOOOOM!";
            int r = _Random.Next(0, countDead);
            int counter = 0;
            for (int i = 0; i < f.Count; i++)
            {
                if (f[i].Health == Flower.HEALTH_DEAD)
                {
                    if (counter == r)
                    {
                        f.RemoveAt(i);
                        break;
                    }
                    counter++;
                }
            }

            countDead = f.Count(v => v.Health == Flower.HEALTH_DEAD);
            if (countDead > 0)
            {
                r = _Random.Next(0, countDead);
                counter = 0;
                for (int i = 0; i < f.Count; i++)
                {
                    if (f[i].Health == Flower.HEALTH_DEAD)
                    {
                        if (counter == r)
                        {
                            f.RemoveAt(i);
                            break;
                        }
                        counter++;
                    }
                }
            }
        }
    }
}
