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
    private bool SlidePortrait => _config != null && _config.EnablePortraitSlideIn;
    private float SlideDur => _config != null ? _config.PortraitSlideDuration : 0.5f;
    private float SlideOffsetX => _config != null ? _config.PortraitSlideOffsetX : 800f;
    private float FadeDur => _config != null ? _config.PortraitFadeDuration : 0.25f;

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
                    typeCharInterval: TypeInterval,
                    slidePortrait: SlidePortrait,
                    portraitSlideDuration: SlideDur,
                    portraitSlideOffsetX: SlideOffsetX,
                    portraitFadeDuration: FadeDur
                );
                return command != null;

            default:
                return false;
        }
    }
}