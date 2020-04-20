using Microsoft.Xna.Framework;
using MonoGame.Extended.Particles.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld46.Classes
{
    class ParticleProperBoxFill : Profile
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public override void GetOffsetAndHeading(out Vector2 offset, out Vector2 heading)
        {
            offset = new Vector2(Random.NextSingle(Width * -0.5f, Width * 0.5f),
                Random.NextSingle(Height * -0.5f, Height * 0.5f));

            heading = new Vector2(0, -1);
        }
    }
}
