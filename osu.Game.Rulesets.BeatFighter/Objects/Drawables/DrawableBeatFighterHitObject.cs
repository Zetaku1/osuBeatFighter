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
        public Vector2 StartingPosition = new Vector2(600, 300);
        public Vector2 EndPosition = new Vector2(-300, -300);
        public Vector2 DeltaPosition = new Vector2(-3f, -1.5f);

        public Vector2 StartingSize = new Vector2(1.0f);
        public Vector2 EndSize = new Vector2(0.05f);
        public Vector2 DeltaSize = new Vector2(-0.005f);

        public float DeltaRotation = 0.01f;

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
            RelativeSizeAxes = Axes.Both;
            iconSprite = new Sprite()
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Position = StartingPosition,
                RelativeSizeAxes = Axes.Both,
                Size = StartingSize,
                Rotation = RNG.Next(),

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

        protected override void Update()
        {
            base.Update();

            if (Position.X > EndPosition.X || Position.Y > EndPosition.Y)
            {
                Position += DeltaPosition;
            }

            //Rotation += DeltaRotation;

            if (Size.X > EndSize.X)
            {
                Size += DeltaSize;
            }
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
