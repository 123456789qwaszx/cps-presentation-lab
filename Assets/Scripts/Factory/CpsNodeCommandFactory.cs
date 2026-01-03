using UnityEngine;

public sealed class CpsNodeCommandFactory : INodeCommandFactory
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly IDialogueSpeakerService _speakers;

    private readonly float _typeInterval;
    private readonly bool _slidePortrait;
    private readonly float _slideDur;
    private readonly float _slideOffsetX;
    private readonly float _fadeDur;

    public CpsNodeCommandFactory(CpsCommandServiceConfig config)
    {
        _widgets = config != null ? config.WidgetAccess : null;
        _speakers = config != null ? config.SpeakerService : null;

        _typeInterval = config != null ? config.TypeCharInterval : 0.03f;
        _slidePortrait = config != null && config.EnablePortraitSlideIn;
        _slideDur = config != null ? config.PortraitSlideDuration : 0.5f;
        _slideOffsetX = config != null ? config.PortraitSlideOffsetX : 800f;
        _fadeDur = config != null ? config.PortraitFadeDuration : 0.25f;
    }

    public bool TryCreate(NodeCommandSpec spec, out ISequenceCommand command)
    {
        command = null;
        if (spec == null) return false;

        switch (spec.kind)
        {
            case NodeCommandKind.ShowLine:
                if (spec.line == null) return false;

                command = new CpsShowLineCommand(
                    widgets: _widgets,
                    speakers: _speakers,
                    line: spec.line,
                    screenId: spec.screenId,
                    widgetId: spec.widgetId,
                    typeCharInterval: _typeInterval,
                    slidePortrait: _slidePortrait,
                    portraitSlideDuration: _slideDur,
                    portraitSlideOffsetX: _slideOffsetX,
                    portraitFadeDuration: _fadeDur
                );
                return command != null;

            default:
                return false;
        }
    }
}