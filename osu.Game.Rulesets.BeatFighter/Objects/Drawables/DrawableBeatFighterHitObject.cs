// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable enable
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Audio;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.BeatFighter.Objects.Drawables
{
    public partial class DrawableBeatFighterHitObject : DrawableHitObject<BeatFighterHitObject>
    {
        public override bool HandlePositionalInput => false;

        //TODO change to const after finishing this part, they arent so that I can test with hot reload
        protected double time_preempt = 450;

        protected double time_fadein = 50;

        //Time betweeen the object appearing on screen and reaching its end trajectory
        protected double time_duration = 1200;

        protected double time_fadeout_hit = 1200;
        protected double time_fadeout_miss = 1300;

        protected Sprite IconSprite;

        public Texture SpriteTexture => IconSprite.Texture;

        public Vector2 StartingPosition = new Vector2(500, 500);
        public Vector2 EndPosition = new Vector2(0, 0);
        public Vector2 EndPositionMiss = new Vector2(-50, 50);

        public Vector2 StartingScale = new Vector2(1.5f);
        public Vector2 EndScale = new Vector2(0.2f);

        protected override double InitialLifetimeOffset => time_preempt;

        [BackgroundDependencyLoader]
        private void load(ISkinSource source, TextureStore textures)
        {
            Alpha = 0;
        }

        public DrawableBeatFighterHitObject(BeatFighterHitObject hitObject)
            : base(hitObject)
        {
        }

        public override IEnumerable<HitSampleInfo> GetSamples() => new[]
        {
            new HitSampleInfo(@"punch")
        };
    }
}
