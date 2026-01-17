public sealed class NodeCommandFactory : INodeCommandFactory
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly ITimeSource _time;
    private readonly ISignalBus _signal;
    private readonly ISignalLatch _latch;

    private readonly PersistentEffectRegistry _effects;

    public NodeCommandFactory(
        IDialogueWidgetAccess widgetAccess,
        ITimeSource time,
        ISignalBus signal,
        ISignalLatch latch,
        PersistentEffectRegistry effects)
    {
        _widgets = widgetAccess;
        _time = time;
        _signal = signal;
        _latch = latch;

        _effects = effects;
    }

    public bool TryCreate(CommandSpecBase spec, out ISequenceCommand command)
    {
        command = spec switch
        {
            null => null,

            WaitCommandSpec s => Create(s),
            HoldSignalCommandSpec s => Create(s),
            RaiseSignalCommandSpec s => Create(s),
            ShowRootLayersCommandSpec s => Create(s),
            HideRootLayersCommandSpec s => Create(s),
            HideTargetsCommandSpec s => Create(s),
            ShowTargetsCommandSpec s => Create(s),
            SetSpriteCommandSpec s => Create(s),
            SetRotationCommandSpec s => Create(s),
            RotateFromToCommandSpec s => Create(s),
            SetScaleCommandSpec s => Create(s),
            ScaleFromToCommandSpec s => Create(s),
            SetColorCommandSpec s => Create(s),
            SetPositionOffsetCommandSpec s => Create(s),
            SetRootStageCommandSpec s => Create(s),
            MoveToCommandSpec s => Create(s),
            MoveByCommandSpec s => Create(s),
            SlideInCommandSpec s => Create(s),
            ShakeRigsCommandSpec s => Create(s),
            PunchScaleCommandSpec s => Create(s),
            BouncySlideInCommandSpec s => Create(s),
            FadeLayersCommandSpec s => Create(s),
            SwayThenDropCommandSpec s => Create(s),
            SetTextCommandSpec s => Create(s),
            TypeTextCommandSpec s => Create(s),
            AdvancedTypeTextCommandSpec s => Create(s),
            BubblePulseCommandSpec s => Create(s),
            StopPersistentEffectCommandSpec s => Create(s),
            BlinkAlphaPersistentCommandSpec s => Create(s),
            GlowColorPersistentCommandSpec s => Create(s),
            BobYPersistentCommandSpec s => Create(s),

            _ => null
        };

        return command != null;
    }

    private BlinkAlphaPersistentCommand Create(BlinkAlphaPersistentCommandSpec s)
        => new(_widgets, _effects, s.screenId, s.widgetRoleKey,
            target: s.target,
            effectKey: s.effectKey,
            minAlpha: s.minAlpha,
            maxAlpha: s.maxAlpha,
            halfPeriodSeconds: s.halfPeriodSeconds,
            startDelay: s.startDelay,
            ease: s.ease,
            replacePolicy: s.replacePolicy,
            ignoreWhenSkipping: s.ignoreWhenSkipping);

    private GlowColorPersistentCommand Create(GlowColorPersistentCommandSpec s)
        => new(_widgets, _effects, s.screenId, s.widgetRoleKey,
            target: s.target,
            effectKey: s.effectKey,
            glowColor: s.glowColor,
            glowStrength: s.glowStrength,
            halfPeriodSeconds: s.halfPeriodSeconds,
            ease: s.ease,
            startDelay: s.startDelay,
            replacePolicy: s.replacePolicy,
            ignoreWhenSkipping: s.ignoreWhenSkipping);

    private BobYPersistentCommand Create(BobYPersistentCommandSpec s)
        => new(_widgets, _effects, s.screenId, s.widgetRoleKey,
            target: s.target,
            effectKey: s.effectKey,
            amplitudeY: s.amplitudeY,
            halfPeriodSeconds: s.halfPeriodSeconds,
            ease: s.ease,
            startDelay: s.startDelay,
            replacePolicy: s.replacePolicy,
            ignoreWhenSkipping: s.ignoreWhenSkipping);

    private BubblePulseCommand Create(BubblePulseCommandSpec s)
        => new(
            widgets: _widgets,
            effects: _effects,
            screenId: s.screenId,
            widgetRoleKey: s.widgetRoleKey,
            target: s.target,
            effectKey: s.effectKey,
            maxScale: s.maxScale,
            periodSeconds: s.periodSeconds,
            startDelay: s.startDelay,
            replacePolicy: s.replacePolicy,
            ignoreWhenSkipping: s.ignoreWhenSkipping
        );

    private StopPersistentEffectCommand Create(StopPersistentEffectCommandSpec s)
        => new(
            effects: _effects,
            effectKey: s.effectKey,
            policy: s.policy,
            executeEvenIfSkipping: s.executeEvenIfSkipping
        );

    private AdvancedTypeTextCommand Create(AdvancedTypeTextCommandSpec s)
        => new(_widgets, _time, s.screenId, s.widgetRoleKey,
            target: s.target,
            text: s.text,
            baseInterval: s.baseInterval,
            defaultPauseSeconds: s.defaultPauseSeconds,
            waitForCompletion: s.wait
        );

    private TypeTextCommand Create(TypeTextCommandSpec s)
        => new(_widgets, _time, s.screenId, s.widgetRoleKey, waitForCompletion: s.wait,
            target: s.target,
            text: s.text,
            interval: s.interval
        );

    private SetTextCommand Create(SetTextCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey,
            target: s.target,
            text: s.text,
            clearWhenEmpty: s.clearWhenEmpty
        );

    private SwayThenDropCommand Create(SwayThenDropCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, s.wait,
            swayTarget: s.swayTarget,
            dropTarget: s.dropTarget,
            swayAngle: s.swayAngle,
            swayLoops: s.swayLoops,
            swayDuration: s.swayDuration,
            dropDistance: s.dropDistance,
            dropDuration: s.dropDuration,
            dropEase: s.dropEase,
            swayForwardEase: s.swayForwardEase,
            swayDecay: s.swayDecay,
            dropStartRatio: s.dropStartRatio
        );

    private FadeLayersCommand Create(FadeLayersCommandSpec s)
        => new(widgets: _widgets, screenId: s.screenId, widgetRoleKey: s.widgetRoleKey, waitForCompletion: s.wait,
            layers: s.layers,
            toAlpha: s.toAlpha,
            duration: s.duration,
            ease: s.ease,
            fromAlpha: s.fromAlpha,
            addIfMissing: s.addIfMissing
        );

    private BouncySlideInCommand Create(BouncySlideInCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, s.wait,
            target: s.target,
            from: s.from,
            slideDistance: s.slideDistance,
            slideDuration: s.slideDuration,
            slideEase: s.slideEase,
            waveAmplitude: s.waveAmplitude,
            waveLoops: s.waveLoops,
            waveAxis: s.waveAxis
        );


    private PunchScaleCommand Create(PunchScaleCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, s.wait,
            target: s.target,
            strength: s.strength,
            duration: s.duration,
            vibrato: s.vibrato,
            elasticity: s.elasticity
        );


    private ShakeRigsCommand Create(ShakeRigsCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, s.wait,
            target: s.target,
            axis: s.axis,
            intensity: s.intensity,
            duration: s.duration,
            vibrato: s.vibrato,
            randomness: s.randomness
        );


    private SlideInCommand Create(SlideInCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, wait: s.wait,
            target: s.target,
            from: s.from,
            distance: s.distance,
            duration: s.duration,
            ease: s.ease
        );

    private MoveByCommand Create(MoveByCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, s.wait,
            target: s.target,
            delta: s.delta,
            duration: s.duration,
            ease: s.ease,
            killTween: s.killTween
        );

    private MoveToCommand Create(MoveToCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, s.wait,
            target: s.target,
            toPosition: s.toPosition,
            duration: s.duration,
            ease: s.ease,
            killTween: s.killTween
        );

    private SetRootStageCommand Create(SetRootStageCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey,
            layers: s.layers,
            anchorPreset: s.anchorPreset,
            offset: s.offset,
            killTween: s.killTween
        );

    private SetPositionOffsetCommand Create(SetPositionOffsetCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey,
            target: s.target,
            offset: s.offset,
            resetToZero: s.resetToZero,
            killTween: s.killTween
        );

    private SetColorCommand Create(SetColorCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey,
            spriteTarget: s.spriteTarget,
            color: s.color,
            keepAlpha: s.keepAlpha
        );

    private ScaleFromToCommand Create(ScaleFromToCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, s.wait,
            target: s.target,
            toScale: s.toScale,
            overrideFromScale: s.overrideFromScale,
            fromScale: s.fromScale,
            duration: s.duration,
            ease: s.ease,
            killTween: s.killTween
        );

    private SetScaleCommand Create(SetScaleCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey,
            target: s.target,
            toScale: s.toScale,
            killTween: s.killTween
        );

    private RotateFromToCommand Create(RotateFromToCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, s.wait,
            target: s.target,
            toEuler: s.toEuler,
            overrideFromEuler: s.overrideFromEuler,
            fromEuler: s.fromEuler,
            duration: s.duration,
            ease: s.ease,
            killTween: s.killTween
        );

    private SetRotationCommand Create(SetRotationCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, s.target,
            toAngle: s.toAngle,
            killTween: s.killTween
        );

    private SetSpriteCommand Create(SetSpriteCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey,
            spriteTarget: s.spriteTarget,
            sprite: s.sprite,
            setNativeSize: s.setNativeSize
        );

    private ShowTargetsCommand Create(ShowTargetsCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, s.wait,
            targetsMask: s.targets,
            duration: s.duration,
            ease: s.ease,
            enableInteraction: s.enableInteraction
        );

    private HideTargetsCommand Create(HideTargetsCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, s.wait,
            targetsMask: s.targets,
            duration: s.duration,
            ease: s.ease,
            disableInteraction: s.disableInteraction
        );

    private HideRootLayersCommand Create(HideRootLayersCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, s.wait,
            layers: s.layers,
            duration: s.duration,
            ease: s.ease,
            disableInteraction: s.disableInteraction
        );

    private ShowRootLayersCommand Create(ShowRootLayersCommandSpec s)
        => new(_widgets, s.screenId, s.widgetRoleKey, s.wait,
            layers: s.layers,
            duration: s.duration,
            ease: s.ease,
            enableInteraction: s.enableInteraction
        );

    private RaiseSignalCommand Create(RaiseSignalCommandSpec s)
        => new(_signal,
            key: s.signalKey,
            raiseOnSkip: s.raiseOnSkip
        );

    private HoldSignalCommand Create(HoldSignalCommandSpec s)
        => new(_latch, _time,
            key: s.signalKey,
            consume: s.consume,
            timeoutSeconds: s.timeoutSeconds,
            respectTimeScale: s.respectTimeScale
        );

    private CpsWaitCommand Create(WaitCommandSpec s)
        => new(_time,
            seconds: s.seconds,
            respectTimeScale: s.respectTimeScale
        );
}