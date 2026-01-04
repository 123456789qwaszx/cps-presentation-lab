using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            
            case NodeCommandKind.FadeGraphic:
            {
                if (_widgets == null) return false;
                if (!_widgets.TryGetGraphic(spec.widgetId, out Graphic g) || g == null) return false;

                float to = spec.f0;                 // alpha
                float dur = spec.f1;                // duration
                bool wait = spec.b0;                // wait
                command = new FadeGraphicCommand(g, to, dur, wait);
                return true;
            }

            case NodeCommandKind.MoveAnchoredPos:
            {
                if (_widgets == null) return false;
                if (!_widgets.TryGetRectTransform(spec.widgetId, out RectTransform rt) || rt == null) return false;

                Vector2 to = spec.v0;
                float dur = spec.f0;
                bool wait = spec.b0;
                Ease ease = spec.ease;
                command = new MoveAnchoredPosCommand(rt, to, dur, ease, wait);
                return true;
            }

            case NodeCommandKind.SetTMPTextImmediate:
            {
                if (_widgets == null) return false;
                if (!_widgets.TryGetTMPText(spec.widgetId, out TMP_Text t) || t == null) return false;

                string text = spec.s0; // 또는 spec.line.text 등으로 대체 가능
                command = new SetTMPTextImmediateCommand(t, text);
                return true;
            }

            case NodeCommandKind.TypeText:
            {
                if (_widgets == null) return false;
                if (!_widgets.TryGetTMPText(spec.widgetId, out TMP_Text t) || t == null) return false;

                string text = spec.s0; // 또는 spec.line.text
                float interval = spec.f0 > 0f ? spec.f0 : (_config != null ? _config.TypeCharInterval : 0.03f);
                bool wait = spec.b0;
                command = new TypeTextCommand(t, text, interval, wait);
                return true;
            }

            default:
                return false;
        }
    }
}