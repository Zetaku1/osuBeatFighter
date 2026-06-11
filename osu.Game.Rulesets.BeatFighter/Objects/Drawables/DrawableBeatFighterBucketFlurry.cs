// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Utils;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.BeatFighter.Objects.Drawables
{
    public partial class DrawableBeatFighterBucketFlurry : DrawableBeatFighterHitObject, IKeyBindingHandler<BeatFighterAction>
    {
        public DrawableBeatFighterBucketFlurry(FlurryBucket hitObject)
            : base(hitObject)
        {
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            Size = new Vector2(400);
            LifetimeStart = HitObject.StartTime - time_duration;
            StartingScale = new Vector2(1.0f);
            IconSprite = new Sprite()
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
                IconSprite
            });
        }

        [BackgroundDependencyLoader]
        private void load(ISkinSource source, TextureStore textures)
        {
            ISkin? skin = source.FindProvider(s => s.GetTexture(@"FlurryBucket") != null);

            if (skin != null)
            {
                IconSprite.Texture = skin.GetTexture(@"FlurryBucket");
            }
            else
            {
                IconSprite.Texture = textures.Get(@"FlurryBucket");
            }

            if (IconSprite.Texture == null)
            {
                Logger.Log("Failed to find bucket texture!", LoggingTarget.Runtime, LogLevel.Important);
            }
        }

protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            this.FadeInFromZero(time_fadein);

            IconSprite.FadeIn(time_fadein);
            // Move from (200,200) to (0,0) and Scale from 3 to 1 over 500ms
            IconSprite.RotateTo(RNG.NextSingle() * 360 - 360, time_duration * 2, Easing.OutQuint);

            IconSprite.MoveToX(EndPosition.X, time_duration, Easing.OutQuint);

            IconSprite.MoveToY(EndPosition.Y - 50, time_duration / 3.0f, Easing.OutQuint)
                      .Then()
                      .MoveToY(EndPosition.Y, time_duration / 1.2f, Easing.OutQuint);
            IconSprite.ScaleTo(EndScale, time_duration, Easing.OutQuint);
        }

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            switch (state)
            {
                case ArmedState.Hit:
                    this.FadeOut(time_fadeout_hit, Easing.OutQuint);
                    IconSprite.MoveToX(Position.X + 1000, time_fadeout_hit, Easing.OutQuint).Expire();
                    break;

                case ArmedState.Miss:
                    this.FadeColour(Color4.Red, time_fadeout_miss);
                    IconSprite.ScaleTo(EndScale * 0.5f, time_fadeout_miss, Easing.OutQuint);
                    IconSprite.MoveToX(EndPositionMiss.X, time_fadeout_miss, Easing.OutQuint);
                    IconSprite.MoveToY(EndPositionMiss.Y, time_fadeout_miss, Easing.OutQuint);
                    this.FadeOut(time_fadeout_miss, Easing.InQuint).Expire();
                    break;
            }
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
