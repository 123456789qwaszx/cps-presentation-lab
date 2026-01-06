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
            
            default:
                return false;
        }
    }
}

