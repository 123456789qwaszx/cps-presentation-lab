using UnityEngine;

public sealed class CpsNodeCommandFactory : INodeCommandFactory
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly ITimeSource _time;
    private readonly ISignalBus _signal;
    private readonly ISignalLatch _latch;

    public CpsNodeCommandFactory(
        IDialogueWidgetAccess widgetAccess,
        ITimeSource time,
        ISignalBus signal,
        ISignalLatch latch)
    {
        _widgets = widgetAccess;
        _time    = time;
        _signal  = signal;
        _latch   = latch;
    }

    public bool TryCreate(CommandSpecBase spec, out ISequenceCommand command)
    {
        command = spec switch
        {
            null => null,

            SetTextCommandSpec s         => Create(s),
            TypeTextCommandSpec s        => Create(s),
            FadeCommandSpec s            => Create(s),
            CanvasFadeCommandSpec s      => Create(s),
            SlideInCommandSpec s         => Create(s),
            SetColorCommandSpec s        => Create(s),
            PunchScaleCommandSpec s      => Create(s),
            ShakeWidgetCommandSpec s     => Create(s),
            WaitCommandSpec s            => Create(s),
            HoldSignalCommandSpec s      => Create(s),
            RaiseSignalCommandSpec s     => Create(s),
            SetActiveCommandSpec s       => Create(s),
            SetInteractableCommandSpec s => Create(s),
            SetAnchoredPosCommandSpec s  => Create(s),
            MoveToCommandSpec s          => Create(s),
            MoveByCommandSpec s          => Create(s),
            BouncySlideInCommandSpec s   => Create(s),
            SetScaleCommandSpec s        => Create(s),
            SetRotationCommandSpec s     => Create(s),
            ShowRootLayersCommandSpec s  => Create(s),
            HideRootLayersCommandSpec s  => Create(s),
            HideTargetsCommandSpec s     => Create(s),
            ShowTargetsCommandSpec s     => Create(s),
            SetSpriteCommandSpec s       => Create(s),

            _ => null
        };

        return command != null;
    }
    
    private SetSpriteCommand Create(SetSpriteCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey,
            spriteTarget: s.spriteTarget,
            sprite:       s.sprite,
            setNativeSize:s.setNativeSize
        );
    
    private ShowTargetsCommand Create(ShowTargetsCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.wait,
            targetsMask:        s.targets,
            duration:           s.duration,
            ease:               s.ease,
            enableInteraction:  s.enableInteraction
        );
    
    private HideTargetsCommand Create(HideTargetsCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.wait,
            targetsMask:        s.targets,
            duration:           s.duration,
            ease:               s.ease,
            disableInteraction: s.disableInteraction
        );

    private HideRootLayersCommand Create(HideRootLayersCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.wait,
            layers:             s.layers,
            duration:           s.duration,
            ease:               s.ease,
            disableInteraction: s.disableInteraction
        );

    private ShowRootLayersCommand Create(ShowRootLayersCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.wait,
            layers:            s.layers,
            duration:          s.duration,
            ease:              s.ease,
            enableInteraction: s.enableInteraction
        );

    private CpsSetRotationCommand Create(SetRotationCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.target, waitForCompletion: s.wait,
            toAngle:            s.toAngle,
            overrideStartAngle: s.overrideStartAngle,
            startAngle:         s.startAngle,
            duration:           s.duration,
            ease:               s.ease,
            killTween:          s.killTween
        );

    private CpsSetScaleCommand Create(SetScaleCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.target, waitForCompletion: s.wait,
            toScale:            s.toScale,
            overrideStartScale: s.overrideStartScale,
            startScale:         s.startScale,
            duration:           s.duration,
            ease:               s.ease,
            killTween:          s.killTween
        );

    private CpsBouncySlideInCommand Create(BouncySlideInCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.target, waitForCompletion: s.wait,
            from:           s.from,
            slideDistance:  s.slideDistance,
            slideDuration:  s.slideDuration,
            slideEase:      s.slideEase,
            waveAmplitude:  s.waveAmplitude,
            waveLoops:      s.waveLoops,
            waveAxis:       s.waveAxis,
            startFromLayout:s.startFromLayout
        );

    private CpsMoveByCommand Create(MoveByCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.target, waitForCompletion: s.wait,
            delta:    s.delta,
            duration: s.duration,
            ease:     s.ease
        );

    private CpsMoveToCommand Create(MoveToCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.target, waitForCompletion: s.wait,
            position: s.position,
            duration: s.duration,
            ease:     s.ease,
            killTween:s.killTween
        );

    private CpsSetAnchoredPosCommand Create(SetAnchoredPosCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.target,
            value:    s.value,
            relative: s.relative,
            killTween:s.killTween
        );

    private CpsSetInteractableCommand Create(SetInteractableCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.target,
            interactable:   s.interactable,
            searchParents:  s.searchParents,
            blocksRaycasts: s.blocksRaycasts
        );

    private CpsSetActiveCommand Create(SetActiveCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.target,
            active: s.active
        );

    private CpsRaiseSignalCommand Create(RaiseSignalCommandSpec s)
        => new (_signal,
            key:         s.signalKey,
            raiseOnSkip: s.raiseOnSkip
        );

    private CpsHoldSignalCommand Create(HoldSignalCommandSpec s)
        => new (_latch, _time,
            key:             s.signalKey,
            consume:         s.consume,
            timeoutSeconds:  s.timeoutSeconds,
            respectTimeScale:s.respectTimeScale
        );

    private CpsWaitCommand Create(WaitCommandSpec s)
        => new (_time,
            seconds:         s.seconds,
            respectTimeScale:s.respectTimeScale
        );

    private CpsShakeWidgetCommand Create(ShakeWidgetCommandSpec s)
    {
        float dur = s.duration   > 0f ? s.duration   : 0.28f;
        int   vib = s.vibrato    > 0  ? s.vibrato    : 12;
        float rnd = s.randomness >= 0f ? Mathf.Clamp(s.randomness, 0f, 180f) : 90f;

        return new CpsShakeWidgetCommand(_widgets, s.screenId, s.widgetRoleKey, s.target, waitForCompletion: s.wait,
            axis:      s.axis,
            intensity: s.intensity,
            duration:  dur,
            vibrato:   vib,
            randomness:rnd
        );
    }

    private CpsPunchScaleCommand Create(PunchScaleCommandSpec s)
    {
        float dur = s.duration   > 0f ? s.duration   : 0.22f;
        int   vib = s.vibrato    > 0  ? s.vibrato    : 8;
        float ela = s.elasticity >= 0f ? Mathf.Clamp01(s.elasticity) : 0.75f;

        return new CpsPunchScaleCommand(_widgets, s.screenId, s.widgetRoleKey, s.target, waitForCompletion: s.wait,
            strength:  s.strength,
            duration:  dur,
            vibrato:   vib,
            elasticity:ela
        );
    }

    private CpsSetColorCommand Create(SetColorCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.target,
            color:         s.color,
            preserveAlpha:s.preserveAlpha
        );


    private CpsSlideInCommand Create(SlideInCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.target, waitForCompletion: s.wait,
            from:     s.from,
            distance: s.distance,
            duration: s.duration,
            ease:     s.ease
        );

    private CpsCanvasFadeCommand Create(CanvasFadeCommandSpec s)
        => new (widgets: _widgets, screenId: s.screenId, widgetRoleKey: s.widgetRoleKey, target: s.target, waitForCompletion: s.wait,
            toAlpha:     s.toAlpha,
            duration:    s.duration,
            ease:        s.ease,
            fromAlpha:   s.fromAlpha,
            addIfMissing:s.addIfMissing
        );

    private CpsFadeCommand Create(FadeCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.target, waitForCompletion: s.wait,
            toAlpha:   s.toAlpha,
            duration:  s.duration,
            ease:      s.ease,
            fromAlpha: s.fromAlpha
        );

    private CpsTypeTextCommand Create(TypeTextCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.target, waitForCompletion: s.wait,
            text:          s.text,
            interval:      s.interval,
            clearWhenEmpty:s.clearWhenEmpty
        );

    private CpsSetTextCommand Create(SetTextCommandSpec s)
        => new (_widgets, s.screenId, s.widgetRoleKey, s.target,
            text:          s.text,
            clearWhenEmpty:s.clearWhenEmpty
        );
}