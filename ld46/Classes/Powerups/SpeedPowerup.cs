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
    class SpeedPowerup : APowerupBase
    {
        public override string PowerupName => "SPEED UP";

        private readonly double _Multiplier;

        public SpeedPowerup(double multiplier)
        {
            _Multiplier = multiplier;
        }

        public override void Consume(Player p, List<Flower> f)
        {
            Task.Run(() =>
            {
                p.Speed *= _Multiplier;
                Thread.Sleep(5000);
                p.Speed = Player.DEFAULT_SPEED;
            });
        }
    }
}
