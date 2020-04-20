using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Spritesheet;

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
