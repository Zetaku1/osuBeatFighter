// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.BeatFighter.Objects;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.BeatFighter.Replays
{
    public class BeatFighterAutoGenerator : AutoGenerator<BeatFighterReplayFrame>
    {
        public new Beatmap<BeatFighterHitObject> Beatmap => (Beatmap<BeatFighterHitObject>)base.Beatmap;

        public BeatFighterAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        protected override void GenerateFrames()
        {
            Frames.Add(new BeatFighterReplayFrame());

            foreach (BeatFighterHitObject hitObject in Beatmap.HitObjects)
            {
                Frames.Add(new BeatFighterReplayFrame
                {
                    Time = hitObject.StartTime,
                    Position = hitObject.Position,
                    // todo: add required inputs and extra frames.
                });
            }
        }
    }
}
