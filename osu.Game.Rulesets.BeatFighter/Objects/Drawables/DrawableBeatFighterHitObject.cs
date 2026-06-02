// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable enable
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.BeatFighter.Objects.Drawables
{
    public partial class DrawableBeatFighterHitObject : DrawableHitObject<BeatFighterHitObject>
    {
        [BackgroundDependencyLoader]
        private void load(ISkinSource source, TextureStore textures)
        {
            Alpha = 0;
        }

        public DrawableBeatFighterHitObject(BeatFighterHitObject hitObject)
            : base(hitObject)
        {
        }
    }
}
