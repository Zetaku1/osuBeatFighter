// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Textures;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Graphics.Containers;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.BeatFighter.UI
{
    public sealed partial class BeatFighterMascotAnimation : BeatSyncedContainer
    {
        private readonly TextureAnimation textureAnimation;

        private int currentFrame;

        public double DisplayTime;

        public bool Completed => !textureAnimation.IsPlaying || textureAnimation.PlaybackPosition >= textureAnimation.Duration;

        public override void Show()
        {
            base.Show();
            DisplayTime = Time.Current;
            textureAnimation.Seek(0);
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, ChannelAmplitudes amplitudes)
        {
            // assume that if the animation is playing on its own, it's independent from the beat and doesn't need to be touched.
            if (textureAnimation.FrameCount == 0 || textureAnimation.IsPlaying)
                return;

            textureAnimation.GotoFrame(currentFrame);
            currentFrame = (currentFrame + 1) % textureAnimation.FrameCount;
        }

        //Same as TaikoMascotAnimation I just prefer to have it on this module instead of creating other dependencies
        private partial class ManualBeatFighterMascotTextureAnimation : TextureAnimation
        {
            private readonly BeatFighterMascotAnimationState state;

            public ManualBeatFighterMascotTextureAnimation(BeatFighterMascotAnimationState state)
            {
                this.state = state;

                IsPlaying = false;
            }

            [BackgroundDependencyLoader]
            private void load(ISkinSource source, TextureStore textures)
            {
                ISkin? skin = source.FindProvider(s => getAnimationFrame(s, state, 0) != null);

                if (skin != null)
                {
                    for (int frameIndex = 0; true; frameIndex++)
                    {
                        var texture = getAnimationFrame(skin, state, frameIndex);

                        if (texture == null)
                            break;

                        AddFrame(texture);
                    }
                }
                else //Use Defaults
                {
                    for (int frameIndex = 0; true; frameIndex++)
                    {
                        var texture = textures.Get($"ZetaPippiKarate{state.ToString()}-{frameIndex}");

                        if (texture == null)
                            break;

                        AddFrame(texture);
                    }
                }
            }

            private Texture getAnimationFrame(ISkin skin, BeatFighterMascotAnimationState state, int frameIndex)
            {
                var texture = skin.GetTexture($"ZetaPippiKarate{state.ToString().ToLowerInvariant()}-{frameIndex}");

                return texture;
            }
        }

        public BeatFighterMascotAnimation(BeatFighterMascotAnimationState state)
        {
            InternalChild = textureAnimation = new ManualBeatFighterMascotTextureAnimation(state).With(animation =>
            {
                Origin = Anchor.Centre;
                Anchor = Anchor.Centre;
                Position = new Vector2(-1000, -300);
                Alpha = 1.0f;
                Scale = new Vector2(1.2f);
            });
            RelativeSizeAxes = Axes.Both;

            // needs to be always present to prevent the animation clock consuming time spent when not present.
            AlwaysPresent = true;
        }
    }
}
