// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.UI;
using osuTK;

namespace osu.Game.Rulesets.BeatFighter.UI
{
    [Cached]
    public partial class BeatFighterPlayfield : Playfield
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            // 1. Define the mascot
            var mascot = new DrawableBeatFighterMascot
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                RelativePositionAxes = Axes.Both,
                Position = new Vector2(0.1f, 0.9f),
                Scale = new Vector2(0.5f)
            };

            AddRangeInternal(new Drawable[]
            {
                HitObjectContainer,
                mascot,
            });
        }
    }
}
