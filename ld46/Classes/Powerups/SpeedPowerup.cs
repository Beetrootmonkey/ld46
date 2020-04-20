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

        public override void Consume(Player p)
        {
            Task.Run(() =>
            {
                int origSpeed = p.Speed;
                p.Speed *= 2;
                Thread.Sleep(5000);
                p.Speed = origSpeed;
            });
        }
    }
}
