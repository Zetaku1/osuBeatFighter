// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Logging;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osu.Game.Audio;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.BeatFighter.Objects.Drawables
{
    public partial class DrawableBeatFighterHitObject : DrawableHitObject<BeatFighterHitObject>, IKeyBindingHandler<BeatFighterAction>
    {
        public override bool HandlePositionalInput => false;

        //TODO change to const after finishing this part, they arent so that I can test with hot reload
        private double time_preempt = 450;

        private double time_fadein = 50;

        //Time betweeen the object appearing on screen and reaching its end trajectory
        private double time_duration = 1200;

        private double time_fadeout_hit = 1200;
        private double time_fadeout_miss = 1300;

        private Sprite iconSprite;
        public Vector2 StartingPosition = new Vector2(500, 500);
        public Vector2 EndPosition = new Vector2(0, 0);
        public Vector2 EndPositionMiss = new Vector2(-50, 50);

        public Vector2 StartingScale = new Vector2(1.5f);
        public Vector2 EndScale = new Vector2(0.2f);

        public float DeltaRotation = -0.5f;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            iconSprite.Texture = textures.Get(@"Bucket");
            Alpha = 0;

            if (iconSprite.Texture == null)
            {
                Logger.Log("Failed to find bucket texture!", LoggingTarget.Runtime, LogLevel.Important);
            }
        }

        protected override double InitialLifetimeOffset => time_preempt;

        public override IEnumerable<HitSampleInfo> GetSamples() => new[]
        {
            new HitSampleInfo(@"punch")
        };

        public DrawableBeatFighterHitObject(BeatFighterHitObject hitObject)
            : base(hitObject)
        {
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            Size = new Vector2(400);
            LifetimeStart = HitObject.StartTime - time_duration;
            iconSprite = new Sprite()
            {
                RelativeSizeAxes = Axes.Both,
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Scale = StartingScale,
                Position = StartingPosition,
                Rotation = RNG.NextSingle() * 360,
                Alpha = 0.0f
            };

            AddRangeInternal(new Drawable[]
            {
                iconSprite
            });
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (!userTriggered)
            {
                if (!HitObject.HitWindows.CanBeHit(timeOffset))
                    ApplyMinResult();
                return;
            }

            var result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None)
                return;

            ApplyResult(result);
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            this.FadeInFromZero(time_fadein);

            iconSprite.FadeIn(time_fadein);
            // Move from (200,200) to (0,0) and Scale from 3 to 1 over 500ms
            iconSprite.RotateTo(RNG.NextSingle() * 360 - 360, time_duration * 2, Easing.OutQuint);

            iconSprite.MoveToX(EndPosition.X, time_duration, Easing.OutQuint);

            iconSprite.MoveToY(EndPosition.Y - 50, time_duration / 3.0f, Easing.OutQuint)
                      .Then()
                      .MoveToY(EndPosition.Y, time_duration / 1.2f, Easing.OutQuint);
            iconSprite.ScaleTo(EndScale, time_duration, Easing.OutQuint);
        }

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            switch (state)
            {
                case ArmedState.Hit:
                    this.FadeOut(time_fadeout_hit, Easing.OutQuint);
                    iconSprite.MoveToX(Position.X + 1000, time_fadeout_hit, Easing.OutQuint).Expire();
                    break;

                case ArmedState.Miss:
                    this.FadeColour(Color4.Red, time_fadeout_miss);
                    //iconSprite.RotateTo(iconSprite.Rotation - 60, time_fadeout_miss, Easing.OutQuint);
                    iconSprite.ScaleTo(EndScale * 0.5f, time_fadeout_miss, Easing.OutQuint);
                    iconSprite.MoveToX(EndPositionMiss.X, time_fadeout_miss, Easing.OutQuint);
                    iconSprite.MoveToY(EndPositionMiss.Y, time_fadeout_miss, Easing.OutQuint);
                    this.FadeOut(time_fadeout_miss, Easing.InQuint).Expire();
                    break;
            }
        }

        public bool OnPressed(KeyBindingPressEvent<BeatFighterAction> e)
        {
            return UpdateResult(true);

        }

        public void OnReleased(KeyBindingReleaseEvent<BeatFighterAction> e)
        {
            return;
        }
    }
}
