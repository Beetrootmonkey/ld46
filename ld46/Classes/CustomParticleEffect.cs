using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.TextureAtlases;

namespace ld46.Classes
{
    class CustomParticleEffect
    {
        public enum Type
        {
            Ember,
            Smoke
        }

        public ParticleEffect ParticleEffect { get; set; }
        public float TimeLeft { get; set; }

        public CustomParticleEffect(ParticleEffect particleEffect, float timeLeft)
        {
            this.ParticleEffect = particleEffect;
            this.TimeLeft = timeLeft;
        }

        public static ParticleEffect CreateEmberParticleEffect(Texture2D texture, Vector2 position, Size size)
        {
            return new ParticleEffect(autoTrigger: false)
            {
                Position = position,
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(new TextureRegion2D(texture), 500, TimeSpan.FromSeconds(1.5),
                    new ParticleProperBoxFill() { Width = size.Width, Height = size.Height })
                    {
                        Parameters = new ParticleReleaseParameters
                        {
                            Speed = new Range<float>(0f, 50f),
                            Quantity = 1,
                            Rotation = new Range<float>(0f, 0f),
                            Scale = new Range<float>(3.0f, 4.0f)
                        },
                        Modifiers =
                        {
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    new ColorInterpolator
                                    {
                                        StartValue = new HslColor(45f, 0.847f, 0.5f),
                                        EndValue = new HslColor(45f, 0f, 0f)
                                    },
                                    new OpacityInterpolator
                                    {
                                        StartValue = 1,
                                        EndValue = 0
                                    }
                                }
                            },
                            new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 60f}
                        }
                    }
                }
            };
        }

        public static ParticleEffect CreateSmokeParticleEffect(Texture2D texture, Vector2 position, Size size)
        {
            return new ParticleEffect(autoTrigger: false)
            {
                Position = position,
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(new TextureRegion2D(texture), 500, TimeSpan.FromSeconds(1.5),
                    new ParticleProperBoxFill() { Width = size.Width, Height = size.Height })
                    {
                        Parameters = new ParticleReleaseParameters
                        {
                            Speed = new Range<float>(0f, 20f),
                            Quantity = 1,
                            Rotation = new Range<float>(-0.5f, 0.5f),
                            Scale = new Range<float>(2.0f, 3.0f)
                        },
                        Modifiers =
                        {
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    new OpacityInterpolator
                                    {
                                        StartValue = 0.75f,
                                        EndValue = 0
                                    }
                                }
                            },
                            new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 50f}
                        }
                    }
                }
            };
        }
    }
}
