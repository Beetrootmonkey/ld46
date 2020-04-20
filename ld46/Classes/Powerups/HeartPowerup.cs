using System.Collections.Generic;

namespace ld46.Classes
{
    class HeartPowerup : APowerupBase
    {
        public override string PowerupName => "1 UP";

        public override void Consume(Player p, List<Flower> f)
        {
            p.Life++;
        }
    }
}
