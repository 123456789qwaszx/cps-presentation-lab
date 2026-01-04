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
            case DefaultShowLineCommandSpec show:
                if (show.line == null)return false;

                command = new CpsShowLineCommand(
                    widgets: _widgets,
                    speakers: _speakers,
                    line: show.line,
                    screenId: spec.screenId,
                    widgetId: spec.widgetId,
                    typeCharInterval: TypeInterval,
                    slidePortrait: Slide != null && Slide.enable,
                    portraitSlideDuration: Slide?.duration ?? 0.5f,
                    portraitSlideOffsetX: Slide?.offsetX ?? 800f,
                    portraitFadeDuration: Slide?.fadeDur ?? 0.25f
                );
                return command != null;

            case MovePortraitCommandSpec move:
            {
                Vector2 dest = move.offset != Vector2.zero
                    ? move.offset
                    : (MoveCfg != null ? MoveCfg.defaultOffset : Vector2.zero);

                float dur = move.duration > 0f
                    ? move.duration
                    : (MoveCfg != null ? Mathf.Max(0f, MoveCfg.duration) : 0.25f);

                Ease ease = MoveCfg != null ? MoveCfg.ease : move.ease;
                bool wait = MoveCfg != null ? MoveCfg.wait : move.wait;

                command = new CpsMovePortraitCommand(
                    widgets: _widgets,
                    screenId: move.screenId,
                    widgetId: move.widgetId,
                    destPos: dest,
                    duration: dur,
                    ease: ease,
                    wait: wait
                );
                return command != null;
            }

            default:
                return false;
        }
    }
}