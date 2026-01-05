using System.Collections;
using UnityEngine;

public sealed class CpsSetPortraitSpriteCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly IDialogueSpeakerService _speakers;
    private readonly DialogueLine _line;
    private readonly string _screenId;
    private readonly string _widgetId;

    public override bool WaitForCompletion => false;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private bool _resolved;

    public CpsSetPortraitSpriteCommand(
        IDialogueWidgetAccess widgets,
        IDialogueSpeakerService speakers,
        DialogueLine line,
        string screenId,
        string widgetId)
    {
        _widgets  = widgets;
        _speakers = speakers;
        _line     = line;
        _screenId = screenId;
        _widgetId = widgetId;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        ResolveIfNeeded();
        ApplyPortrait();
        yield break;
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        ResolveIfNeeded();
        ApplyPortrait();
    }

    private void ResolveIfNeeded()
    {
        if (_resolved) return;
        _resolved = true;

        if (_widgets == null) return;
        if (_widgets.TryResolve(_screenId, _widgetId, out var refs))
            _refs = refs;
    }

    private void ApplyPortrait()
    {
        if (_refs?.PortraitImage == null || _speakers == null || _line == null)
            return;

        _refs.PortraitImage.sprite = _speakers.GetPortrait(_line.speakerId, _line.expression);
    }
}