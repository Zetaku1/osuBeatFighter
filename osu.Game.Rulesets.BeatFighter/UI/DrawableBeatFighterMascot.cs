// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Graphics.Containers;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.BeatFighter.UI
{
    public partial class DrawableBeatFighterMascot : BeatSyncedContainer
    {
        private readonly TextureAnimation textureAnimation;
        private int currentFrame;

        public DrawableBeatFighterMascot()
        {
            // Add the general animation component as a child
            InternalChild = textureAnimation = new TextureAnimation
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
            };
        }

        [BackgroundDependencyLoader]
        private void load(ISkinSource skin)
        {
            // Load your OWN textures here, not pippidon!
            // For example: "bucket-idle-0", "bucket-idle-1", etc.
            for (int i = 0; i < 4; i++)
            {
                var tex = skin.GetTexture($"bucket-idle-{i}");
                if (tex != null) textureAnimation.AddFrame(tex);
            }
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, ChannelAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

            // This is where the magic happens! Every beat, you advance the frame.
            if (textureAnimation.FrameCount > 0)
            {
                textureAnimation.GotoFrame(currentFrame);
                currentFrame = (currentFrame + 1) % textureAnimation.FrameCount;
            }
        }
    }
}
