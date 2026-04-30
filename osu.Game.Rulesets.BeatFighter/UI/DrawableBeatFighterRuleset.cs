// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.BeatFighter.Objects;
using osu.Game.Rulesets.BeatFighter.Objects.Drawables;
using osu.Game.Rulesets.BeatFighter.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.BeatFighter.UI
{
    [Cached]
    public partial class DrawableBeatFighterRuleset : DrawableRuleset<BeatFighterHitObject>
    {
        public DrawableBeatFighterRuleset(BeatFighterRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
        }

        protected override Playfield CreatePlayfield() => new BeatFighterPlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new BeatFighterFramedReplayInputHandler(replay);

        public override DrawableHitObject<BeatFighterHitObject> CreateDrawableRepresentation(BeatFighterHitObject h) => new DrawableBeatFighterHitObject(h);

        protected override PassThroughInputManager CreateInputManager() => new BeatFighterInputManager(Ruleset?.RulesetInfo);
    }
}
