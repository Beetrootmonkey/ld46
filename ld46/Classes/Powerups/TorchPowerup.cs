using System.Collections.Generic;

namespace ld46.Classes
{
    class TorchPowerup : APowerupBase
    {
        public override string PowerupName => "FLAME ON";

        public override void Consume(Player p, List<Flower> f)
        {
            foreach (var flower in f)
            {
                flower.Health = Flower.HEALTH_MAX;
            }
        }
    }
}
