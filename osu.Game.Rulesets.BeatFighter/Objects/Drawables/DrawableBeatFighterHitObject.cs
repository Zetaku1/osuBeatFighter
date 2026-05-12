// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Logging;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Utils;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.BeatFighter.Objects.Drawables
{
    public partial class DrawableBeatFighterHitObject : DrawableHitObject<BeatFighterHitObject>
    {
        private Sprite iconSprite = null!;
        public Vector2 StartingPosition = new Vector2(500, 500);
        public Vector2 EndPosition = new Vector2(0, 0);
        public Vector2 DeltaPosition = new Vector2(-2f, -2f);

        public Vector2 StartingScale = new Vector2(1.0f);
        public Vector2 EndScale = new Vector2(0.05f);
        public Vector2 DeltaSize = new Vector2(-0.005f);

        public float DeltaRotation = -0.5f;

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
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            Size = new Vector2(400);
            iconSprite = new Sprite()
            {
                RelativeSizeAxes = Axes.Both,
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Scale = new Vector2(1.5f),
                Position = StartingPosition,
                Rotation = RNG.NextSingle() * 360,
                Alpha = 1.0f
            };

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

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            const float duration = 1000.0f;
            // Move from (200,200) to (0,0) and Scale from 3 to 1 over 500ms
            iconSprite.RotateTo(RNG.NextSingle() * 360 - 360, duration + 300, Easing.OutQuint);

            iconSprite.MoveToX(EndPosition.X, duration, Easing.OutQuint)
                      .Then()
                      .FadeOut();

            iconSprite.MoveToY(EndPosition.Y - 50, duration / 3.0f, Easing.OutQuint)
                      .Then()
                      .MoveToY(EndPosition.Y, duration / 1.2f, Easing.OutQuint);
            iconSprite.ScaleTo(EndScale, duration, Easing.OutQuint);
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
