// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.BeatFighter.Objects.Drawables
{
    public class DrawableBeatFighterBucketFlurry    : DrawableHitObject<FlurryBucket>
    {
        public DrawableBeatFighterBucketFlurry([CanBeNull] FlurryBucket hitObject)
            : base(hitObject) { }
    }
}
