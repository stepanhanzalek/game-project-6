using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleSystemExample
{
    public class FireworkParticleSystem : ParticleSystem
    {
        private Color color;

        public FireworkParticleSystem(Game game, int maxExplosions) : base(game, maxExplosions * 25)
        {
        }

        public void PlaceFirework(Vector2 where, Color color) => AddParticles(where, color);

        protected override void InitializeConstants()
        {
            textureFilename = "particle";

            minNumParticles = 20;
            maxNumParticles = 20;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where, Color color)
        {
            var vel = RandomHelper.NextDirection() * RandomHelper.NextFloat(40, 500);
            var lifetime = RandomHelper.NextFloat(.5f, 1f);
            var rot = RandomHelper.NextFloat(0, MathHelper.TwoPi);
            var angVel = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);
            var acc = -vel / lifetime;
            var scale = RandomHelper.NextFloat(4, 6);

            p.Initialize(where, vel, acc, color: color, scale: scale, lifetime: lifetime, rotation: rot, angularVelocity: angVel);
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;

            particle.Scale = 5 + normalizedLifetime;
        }
    }
}
