using DG.Tweening;
using UnityEngine;

public sealed class CpsNodeCommandFactory : INodeCommandFactory
{
    private readonly CpsCommandServiceConfig _config;
    private readonly IDialogueWidgetAccess _widgets;
    private readonly IDialogueSpeakerService _speakers;
    
    public CpsNodeCommandFactory(CpsCommandServiceConfig config)
    {
        _config   = config;
        _widgets  = config.WidgetAccess;
        _speakers = config.SpeakerService;
    }
    private ITimeSource Time => _config != null ? _config.TimeSource : null;
    private ISignalBus Signals => _config != null ? _config.SignalBus : null;

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
                    widgetId: s.widgetId,
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
                    widgetId: t.widgetId,
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
                    widgetId: f.widgetId,
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
                    widgetId: s.widgetId,
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
                    widgetId: s.widgetId,
                    target: s.target,
                    sprite: s.sprite,
                    clearWhenNull: s.clearWhenNull,
                    setNativeSize: s.setNativeSize
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
                    widgetId: p.widgetId,
                    target: p.target,
                    strength: p.strength,
                    duration: dur,
                    vibrato: vib,
                    elasticity: ela,
                    waitForCompletion: p.wait
                );
                return command != null;
            }
            
            case ShakeCommandSpec s:
            {
                float dur = s.duration > 0f ? s.duration : 0.28f;
                int vib   = s.vibrato  > 0  ? s.vibrato  : 12;
                float rnd = s.randomness >= 0f ? Mathf.Clamp(s.randomness, 0f, 180f) : 90f;

                command = new CpsShakeCommand(
                    widgets: _widgets,
                    screenId: s.screenId,
                    widgetId: s.widgetId,
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
                    time: Time,
                    seconds: w.seconds,
                    respectTimeScale: w.respectTimeScale
                );
                return command != null;
            }

            case HoldSignalCommandSpec h:
            {
                command = new CpsHoldSignalCommand(
                    signals: Signals,
                    time: Time,
                    signalKey: h.signalKey,
                    timeoutSeconds: h.timeoutSeconds,
                    respectTimeScale: h.respectTimeScale
                );
                return command != null;
            }
            
            default:
                return false;
        }
    }
}

