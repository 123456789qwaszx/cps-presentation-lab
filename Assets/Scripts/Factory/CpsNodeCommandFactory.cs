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

    public bool TryCreate(NodeCommandSpec spec, out ISequenceCommand command)
    {
        command = null;
        if (spec == null) return false;

        switch (spec.kind)
        {
            case NodeCommandKind.ShowLine:
                if (spec.line == null)return false;

                command = new CpsShowLineCommand(
                    widgets: _widgets,
                    speakers: _speakers,
                    line: spec.line,
                    screenId: spec.screenId,
                    widgetId: spec.widgetId,
                    typeCharInterval: TypeInterval,
                    slidePortrait: Slide != null && Slide.enable,
                    portraitSlideDuration: Slide?.duration ?? 0.5f,
                    portraitSlideOffsetX: Slide?.offsetX ?? 800f,
                    portraitFadeDuration: Slide?.fadeDur ?? 0.25f
                );
                return command != null;

            case NodeCommandKind.ShakeCamera:
            {
                // "기본 오프셋을 현재 위치 기준 상대값"인 상태.
                Vector2 dest = MoveCfg != null ? MoveCfg.defaultOffset : Vector2.zero;
                float   dur  = MoveCfg != null ? Mathf.Max(0f, MoveCfg.duration) : 0.25f;
                Ease    ease = MoveCfg != null ? MoveCfg.ease : Ease.OutCubic;
                bool    wait = MoveCfg != null && MoveCfg.wait;

                command = new CpsMovePortraitCommand(
                    widgets: _widgets,
                    screenId: spec.screenId,
                    widgetId: spec.widgetId,
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