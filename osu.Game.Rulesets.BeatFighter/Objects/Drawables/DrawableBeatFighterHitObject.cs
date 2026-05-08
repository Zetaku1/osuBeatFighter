// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Logging;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.BeatFighter.Objects.Drawables
{
    public partial class DrawableBeatFighterHitObject : DrawableHitObject<BeatFighterHitObject>
    {
        private Sprite iconSprite = null!;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            iconSprite.Texture = textures.Get(@"Bucket");

            if (iconSprite.Texture == null)
            {
                Logger.Log("Failed to find bucket texture!", LoggingTarget.Runtime, LogLevel.Important);
            }
        }

        public DrawableBeatFighterHitObject(BeatFighterHitObject hitObject)
            : base(hitObject)
        {
            Size = new Vector2(40);
            Origin = Anchor.Centre;

            Position = hitObject.Position;

            iconSprite = new Sprite()
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Alpha = 1.0f
            };
            // todo: add visuals.
            AddRangeInternal(new Drawable[]
            {
                iconSprite
            });
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset >= 0)
                // todo: implement judgement logic
                ApplyResult(HitResult.Great);
        }

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            const double duration = 1000;

            switch (state)
            {
                case ArmedState.Hit:
                    this.FadeOut(duration, Easing.OutQuint).Expire();
                    break;

                case ArmedState.Miss:
                    this.FadeColour(Color4.Red, duration);
                    this.FadeOut(duration, Easing.InQuint).Expire();
                    break;
            }
        }
    }
}
