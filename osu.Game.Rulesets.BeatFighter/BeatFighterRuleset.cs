// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.BeatFighter.Beatmaps;
using osu.Game.Rulesets.BeatFighter.Mods;
using osu.Game.Rulesets.BeatFighter.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osuTK.Graphics;

namespace osu.Game.Rulesets.BeatFighter
{
    public partial class BeatFighterRuleset : Ruleset
    {
        public override string Description => "BeatFighter!";

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) =>
            new DrawableBeatFighterRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) =>
            new BeatFighterBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) =>
            new BeatFighterDifficultyCalculator(RulesetInfo, beatmap);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.Automation:
                    return new[] { new BeatFighterModAutoplay() };

                default:
                    return Array.Empty<Mod>();
            }
        }

        public override string ShortName => "beatfighterruleset";

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.Z, BeatFighterAction.Button1),
            new KeyBinding(InputKey.X, BeatFighterAction.Button2),
        };

        public override Drawable CreateIcon() => new Icon(ShortName[0]);

        public partial class Icon : CompositeDrawable
        {
            public Icon(char c)
            {
                InternalChildren = new Drawable[]
                {
                    new SpriteText
                    {
                        X = 1,
                        Y = -9, // Idk why it allows negative values, but ill flow with it
                        Text = 'B'.ToString(),
                        Font = OsuFont.Default.With(size: 30),
                        Colour = Color4.White,
                        Shadow = true,
                    },
                    new SpriteText
                    {
                        X = 6,
                        Y = -4,
                        Text = 'F'.ToString(),
                        Font = OsuFont.Default.With(size: 30),
                        Colour = Color4.White,
                        Shadow = true,
                    }
                };
            }
        }

        public override IEnumerable<HitResult> GetValidHitResults()
        {
            return new[]
            {
                HitResult.Great,
                HitResult.Ok,
                HitResult.Meh,
                HitResult.Miss,

                HitResult.LargeTickHit,
                HitResult.LargeTickMiss,
                HitResult.SmallTickHit,
                HitResult.SmallTickMiss,
                HitResult.SliderTailHit,
                HitResult.SmallBonus,
                HitResult.LargeBonus,
                HitResult.IgnoreHit,
                HitResult.IgnoreMiss,
            };
        }

        // Leave this line intact. It will bake the correct version into the ruleset on each build/release.
        public override string RulesetAPIVersionSupported => CURRENT_RULESET_API_VERSION;
    }
}
