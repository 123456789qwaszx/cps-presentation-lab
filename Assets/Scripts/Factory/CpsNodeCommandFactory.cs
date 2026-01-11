using DG.Tweening;
using UnityEngine;

public sealed class CpsNodeCommandFactory : INodeCommandFactory
{
    private readonly CpsCommandServiceConfig _config;
    private readonly IDialogueWidgetAccess _widgets;
    private readonly ITimeSource _time;
    private readonly ISignalBus _signal;
    private readonly ISignalLatch _latch;
    
    public CpsNodeCommandFactory(CpsCommandServiceConfig config, ITimeSource time, ISignalBus signal, ISignalLatch latch)
    {
        _config   = config;
        _widgets  = config.WidgetAccess;
        _time   = time;
        _signal = signal;
        _latch = latch;
    }

    private float TypeInterval => _config != null ? _config.TypeCharInterval : 0.03f;
    private PortraitSlideSettings Slide => _config?.PortraitSlide;
    private MovePortraitSettings MoveCfg => _config?.MovePortrait;

    public bool TryCreate(CommandSpecBase spec, out ISequenceCommand command)
    {
        command = null;
        if (spec == null) return false;

        switch (spec)
        {
            case SetTextCommandSpec s:
            {
                command = new CpsSetTextCommand(
                    widgets: _widgets,
                    screenId: s.screenId,
                    widgetId: s.widgetRoleKey,
                    target: s.target,
                    text: s.text,
                    clearWhenEmpty: s.clearWhenEmpty
                );
                return command != null;
            }
            
            case TypeTextCommandSpec t:
            {
                float interval = t.interval > 0f ? t.interval : TypeInterval;

                command = new CpsTypeTextCommand(
                    widgets: _widgets,
                    screenId: t.screenId,
                    widgetId: t.widgetRoleKey,
                    target: t.target,
                    text: t.text,
                    interval: interval,
                    waitForCompletion: t.wait,
                    clearWhenEmpty: t.clearWhenEmpty
                );
                return command != null;
            }
            
            case FadeCommandSpec f:
            {
                // 디폴트 duration: Config에 별도 Fade 세팅이 없으면 일단 0.25f
                float defaultDur = (Slide != null ? Mathf.Max(0f, Slide.fadeDur) : 0.25f);
                float dur = f.duration > 0f ? f.duration : defaultDur;

                command = new CpsFadeCommand(
                    widgets: _widgets,
                    screenId: f.screenId,
                    widgetId: f.widgetRoleKey,
                    target: f.target,
                    toAlpha: f.toAlpha,
                    duration: dur,
                    ease: f.ease,
                    waitForCompletion: f.wait,
                    fromAlpha: f.fromAlpha
                );
                return command != null;
            }
            
            case SlideInCommandSpec s:
            {
                // 고급 디폴트 (일단)
                float defaultDistance = (Slide != null ? Mathf.Max(0f, Slide.offsetX) : 800f);
                float defaultDuration = (Slide != null ? Mathf.Max(0f, Slide.duration) : 0.5f);

                float dist = s.distance > 0f ? s.distance : defaultDistance;
                float dur  = s.duration  > 0f ? s.duration  : defaultDuration;

                command = new CpsSlideInCommand(
                    widgets: _widgets,
                    screenId: s.screenId,
                    widgetId: s.widgetRoleKey,
                    target: s.target,
                    from: s.from,
                    distance: dist,
                    duration: dur,
                    ease: s.ease,
                    waitForCompletion: s.wait
                );
                return command != null;
            }
            
            case SetSpriteCommandSpec s:
            {
                command = new CpsSetSpriteCommand(
                    widgets: _widgets,
                    screenId: s.screenId,
                    widgetId: s.widgetRoleKey,
                    target: s.target,
                    sprite: s.sprite,
                    clearWhenNull: s.clearWhenNull,
                    setNativeSize: s.setNativeSize
                );
                return command != null;
            }
            
            case SetColorCommandSpec c:
            {
                command = new CpsSetColorCommand(
                    widgets: _widgets,
                    screenId: c.screenId,
                    widgetId: c.widgetRoleKey,
                    target: c.target,
                    color: c.color,
                    preserveAlpha: c.preserveAlpha
                );
                return command != null;
            }
            
            case PunchScaleCommandSpec p:
            {
                float dur = p.duration > 0f ? p.duration : 0.22f;
                int vib   = p.vibrato  > 0  ? p.vibrato  : 8;
                float ela = p.elasticity >= 0f ? Mathf.Clamp01(p.elasticity) : 0.75f;

                command = new CpsPunchScaleCommand(
                    widgets: _widgets,
                    screenId: p.screenId,
                    widgetId: p.widgetRoleKey,
                    target: p.target,
                    strength: p.strength,
                    duration: dur,
                    vibrato: vib,
                    elasticity: ela,
                    waitForCompletion: p.wait
                );
                return command != null;
            }
            
            case ShakeWidgetCommandSpec s:
            {
                float dur = s.duration > 0f ? s.duration : 0.28f;
                int vib   = s.vibrato  > 0  ? s.vibrato  : 12;
                float rnd = s.randomness >= 0f ? Mathf.Clamp(s.randomness, 0f, 180f) : 90f;

                command = new CpsShakeWidgetCommand(
                    widgets: _widgets,
                    screenId: s.screenId,
                    widgetId: s.widgetRoleKey,
                    target: s.target,
                    axis: s.axis,
                    intensity: s.intensity,
                    duration: dur,
                    vibrato: vib,
                    randomness: rnd,
                    waitForCompletion: s.wait
                );
                return command != null;
            }
            
            case WaitCommandSpec w:
            {
                command = new CpsWaitCommand(
                    time: _time,
                    seconds: w.seconds,
                    respectTimeScale: w.respectTimeScale
                );
                return command != null;
            }

            case HoldSignalCommandSpec h:
            {
                command = new CpsHoldSignalCommand(
                    latch: _latch,
                    time: _time,
                    key: h.signalKey,
                    consume: h.consume,
                    timeoutSeconds: h.timeoutSeconds,
                    respectTimeScale: h.respectTimeScale
                );
                return command != null;
            }
            
            case RaiseSignalCommandSpec r:
            {
                command = new CpsRaiseSignalCommand(
                    signals: _signal,
                    key: r.signalKey,
                    raiseOnSkip: r.raiseOnSkip
                );
                return command != null;
            }
            
            case SetActiveCommandSpec a:
            {
                command = new CpsSetActiveCommand(
                    widgets: _widgets,
                    screenId: a.screenId,
                    widgetId: a.widgetRoleKey,
                    target: a.target,
                    active: a.active
                );
                return command != null;
            }

            case SetInteractableCommandSpec i:
            {
                command = new CpsSetInteractableCommand(
                    widgets: _widgets,
                    screenId: i.screenId,
                    widgetId: i.widgetRoleKey,
                    target: i.target,
                    interactable: i.interactable,
                    searchParents: i.searchParents,
                    blocksRaycasts: i.blocksRaycasts
                );
                return command != null;
            }
            
            case SetAnchoredPosCommandSpec p:
            {
                command = new CpsSetAnchoredPosCommand(
                    widgets: _widgets,
                    screenId: p.screenId,
                    widgetId: p.widgetRoleKey,
                    target: p.target,
                    value: p.value,
                    relative: p.relative,
                    killTween: p.killTween
                );
                return command != null;
            }

            case MoveToCommandSpec m:
            {
                float defaultDur = (MoveCfg != null ? Mathf.Max(0f, MoveCfg.duration) : 0.25f);
                float dur = m.duration > 0f ? m.duration : defaultDur;

                Ease ease = (MoveCfg != null ? MoveCfg.ease : m.ease);
                bool wait = (MoveCfg != null ? MoveCfg.wait : m.wait);

                command = new CpsMoveToCommand(
                    widgets: _widgets,
                    screenId: m.screenId,
                    widgetId: m.widgetRoleKey,
                    target: m.target,
                    position: m.position,
                    duration: dur,
                    ease: ease,
                    waitForCompletion: wait,
                    killTween: m.killTween
                );
                return command != null;
            }
            
            case MoveByCommandSpec m:
            {
                float defaultDur = (MoveCfg != null ? Mathf.Max(0f, MoveCfg.duration) : 0.25f);
                float dur = m.duration > 0f ? m.duration : defaultDur;

                Ease ease = (MoveCfg != null ? MoveCfg.ease : m.ease);
                bool wait = (MoveCfg != null ? MoveCfg.wait : m.wait);

                command = new CpsMoveByCommand(
                    widgets: _widgets,
                    screenId: m.screenId,
                    widgetId: m.widgetRoleKey,
                    target: m.target,
                    delta: m.delta,
                    duration: dur,
                    ease: ease,
                    waitForCompletion: wait
                    // killTween: m.killTween (필요하면 추가)
                );
                return command != null;
            }
            
            case BouncySlideInCommandSpec s:
            {
                float defaultDist = (Slide != null ? Mathf.Max(0f, Slide.offsetX) : 800f);
                float defaultDur  = (Slide != null ? Mathf.Max(0f, Slide.duration) : 0.5f);

                float dist = s.slideDistance > 0f ? s.slideDistance : defaultDist;
                float dur  = s.slideDuration  > 0f ? s.slideDuration  : defaultDur;

                command = new CpsBouncySlideInCommand(
                    widgets: _widgets,
                    screenId: s.screenId,
                    widgetId: s.widgetRoleKey,
                    target:  s.target,
                    from:    s.from,
                    slideDistance: dist,
                    slideDuration: dur,
                    slideEase: s.slideEase,
                    waveAmplitude: s.waveAmplitude,
                    waveLoops: s.waveLoops,
                    waveAxis: s.waveAxis,
                    waitForCompletion: s.wait,
                    startFromLayout: s.startFromLayout
                );
                return command != null;
            }
            
            case SwayThenDropCommandSpec s:
            {
                command = new CpsSwayThenDropCommand(
                    widgets: _widgets,
                    screenId: s.screenId,
                    widgetId: s.widgetRoleKey,
                    target: s.target,
                    swayAngle: s.swayAngle,
                    swayLoops: s.swayLoops,
                    swayDuration: s.swayDuration,
                    dropDistance: s.dropDistance,
                    dropDuration: s.dropDuration,
                    dropAngle: s.dropAngle,
                    dropEase: s.dropEase,
                    swayForwardEase: s.swayForwardEase,
                    swayBackwardEase: s.swayBackwardEase,
                    swayDecay: s.swayDecay,
                    dropStartRatio: s.dropStartRatio,
                    waitForCompletion: s.wait
                );
                return command != null;
            }
            
            case SetScaleCommandSpec s:
            {
                command = new CpsSetScaleCommand(
                    widgets: _widgets,
                    screenId: s.screenId,
                    widgetId: s.widgetRoleKey,
                    target: s.target,
                    toScale: s.toScale,
                    overrideStartScale: s.overrideStartScale,
                    startScale: s.startScale,
                    duration: s.duration,
                    ease: s.ease,
                    waitForCompletion: s.wait,
                    killTween: s.killTween
                );
                return command != null;
            }

            case SetRotationCommandSpec r:
            {
                command = new CpsSetRotationCommand(
                    widgets: _widgets,
                    screenId: r.screenId,
                    widgetId: r.widgetRoleKey,
                    target: r.target,
                    toAngle: r.toAngle,
                    overrideStartAngle: r.overrideStartAngle,
                    startAngle: r.startAngle,
                    duration: r.duration,
                    ease: r.ease,
                    waitForCompletion: r.wait,
                    killTween: r.killTween
                );
                return command != null;
            }
            
            case CanvasFadeCommandSpec c:
            {
                float defaultDur = (Slide != null ? Mathf.Max(0f, Slide.fadeDur) : 0.25f);
                float dur = c.duration > 0f ? c.duration : defaultDur;

                command = new CpsCanvasFadeCommand(
                    widgets: _widgets,
                    screenId: c.screenId,
                    widgetRoleKey: c.widgetRoleKey,
                    target: c.target,
                    toAlpha: c.toAlpha,
                    duration: dur,
                    ease: c.ease,
                    waitForCompletion: c.wait,
                    fromAlpha: c.fromAlpha,
                    addIfMissing: c.addIfMissing
                );
                return command != null;
            }
            
            default:
                return false;
        }
    }
}

