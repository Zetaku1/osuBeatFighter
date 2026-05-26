// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable enable
using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Extensions.ObjectExtensions;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Judgements;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.BeatFighter.UI
{
    public partial class DrawableBeatFighterMascot : BeatSyncedContainer, IKeyBindingHandler<BeatFighterAction>
    {
        private BeatFighterMascotAnimation? currentAnimation;

        private readonly Dictionary<BeatFighterMascotAnimationState, BeatFighterMascotAnimation> animations;

        public readonly Bindable<BeatFighterMascotAnimationState> State;
        public readonly Bindable<JudgementResult?> LastResult;

        // Track the count of keys currently pressed down (e.g., if pressing both Left and Right click keys)
        private int activeKeyPresses;

        public DrawableBeatFighterMascot(BeatFighterMascotAnimationState startingState = BeatFighterMascotAnimationState.Idle)
        {
            State = new Bindable<BeatFighterMascotAnimationState>(startingState);
            LastResult = new Bindable<JudgementResult?>();

            animations = new Dictionary<BeatFighterMascotAnimationState, BeatFighterMascotAnimation>();
        }

        [BackgroundDependencyLoader]
        private void load(GameplayState? gameplayState)
        {
            InternalChildren = new[]
            {
                animations[BeatFighterMascotAnimationState.Idle] = new BeatFighterMascotAnimation(BeatFighterMascotAnimationState.Idle)
                    { bShouldSync = true, bIsLooping = true },
                animations[BeatFighterMascotAnimationState.Hit] = new BeatFighterMascotAnimation(BeatFighterMascotAnimationState.Hit)
                    { bShouldSync = false, bIsLooping = false },
                animations[BeatFighterMascotAnimationState.Miss] = new BeatFighterMascotAnimation(BeatFighterMascotAnimationState.Miss)
                    { bShouldSync = false, bIsLooping = false },
                animations[BeatFighterMascotAnimationState.Fail] = new BeatFighterMascotAnimation(BeatFighterMascotAnimationState.Fail)
                    { bShouldSync = true, bIsLooping = true },
            };

            if (gameplayState != null)
                ((IBindable<JudgementResult>)LastResult).BindTo(gameplayState.LastJudgementResult);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            animations.Values.ForEach(animation => animation.Hide());
            State.BindValueChanged(mascotStateChanged, true);
            LastResult.BindValueChanged(onNewResult, true);
        }

        private void onNewResult(ValueChangedEvent<JudgementResult?> newResult)
        {
            //TODO, Add an emote when hitting, or maybe completing a combo or breaking one
        }

        private void mascotStateChanged(ValueChangedEvent<BeatFighterMascotAnimationState> state)
        {
            Logger.Log($"Mascot state changed Old State: {state.OldValue} New State: {state.NewValue}");
            currentAnimation?.Hide();
            if (currentAnimation != null) currentAnimation.bHasPlayedOnce = false;
            currentAnimation = animations[state.NewValue];
            currentAnimation.Show();
        }

        //TODO: Idle animation will be handled by the animation object, I need to see if I need this funct or not.
        /*protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, ChannelAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

            if (textureAnimation.FrameCount > 0)
            {
                textureAnimation.GotoFrame(currentFrame);
                currentFrame = (currentFrame + 1) % textureAnimation.FrameCount;
            }
        }*/

        protected override void Update()
        {
            base.Update();
            State.Value = getNextState();
        }

        private BeatFighterMascotAnimationState getNextState()
        {
            if (activeKeyPresses > 0)
            {
                return BeatFighterMascotAnimationState.Hit;
            }

            return BeatFighterMascotAnimationState.Idle;
        }

        public bool OnPressed(KeyBindingPressEvent<BeatFighterAction> e)
        {
            activeKeyPresses++;
            Logger.Log($"Active Key Presses: {activeKeyPresses}");

            //We don't want to consume the input, we let the hitobject consume it
            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<BeatFighterAction> e)
        {
            // Safeguard to make sure our counter never drops below 0
            activeKeyPresses = Math.Max(0, activeKeyPresses - 1);
            Logger.Log($"Active Key Presses: {activeKeyPresses}");
        }
    }
}
