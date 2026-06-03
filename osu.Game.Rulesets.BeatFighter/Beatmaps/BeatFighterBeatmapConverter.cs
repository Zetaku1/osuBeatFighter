// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Threading;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.BeatFighter.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osuTK;

namespace osu.Game.Rulesets.BeatFighter.Beatmaps
{
    public class BeatFighterBeatmapConverter : BeatmapConverter<BeatFighterHitObject>
    {
        public BeatFighterBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
        }

        // todo: Check for conversion types that should be supported (ie. Beatmap.HitObjects.Any(h => h is IHasXPosition))
        // https://github.com/ppy/osu/tree/master/osu.Game/Rulesets/Objects/Types
        public override bool CanConvert() => true;

        protected override IEnumerable<BeatFighterHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap, CancellationToken cancellationToken)
        {
            var positionData = original as IHasPosition;
            var comboData = original as IHasCombo;
            var sliderVelocityData = original as IHasSliderVelocity;
            var generateTicksData = original as IHasGenerateTicks;

            switch (original)
            {
                case IHasSliderVelocity:
                    return new FlurryBucket
                    {
                        Samples = original.Samples,
                        StartTime = original.StartTime,
                        Position = (original as IHasPosition)?.Position ?? Vector2.Zero,
                    }.Yield();

                default:

                    return new Bucket
                    {
                        Samples = original.Samples,
                        StartTime = original.StartTime,
                        Position = (original as IHasPosition)?.Position ?? Vector2.Zero,
                    }.Yield();
            }
        }
    }
}
