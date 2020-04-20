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
    class HeartPowerup : APowerupBase
    {
        public override string PowerupName => "EXTRA LIFE";

        public override void Consume(Player p)
        {
            p.Life++;
        }
    }
}
