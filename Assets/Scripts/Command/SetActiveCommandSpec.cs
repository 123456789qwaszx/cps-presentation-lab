using System;
using UnityEngine;
using System.Collections;

[Serializable]
[CommandMenuHint("State", "Set Active", Order = 10)]
public sealed class SetActiveCommandSpec : CommandSpecBase
{
    public DialogueWidgetTarget target = DialogueWidgetTarget.StandingPortraitImage;
    public bool active = true;
}

public sealed class CpsSetActiveCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly DialogueWidgetTarget _target;
    private readonly bool _active;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private GameObject _go;
    private bool _resolved;

    public CpsSetActiveCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        bool active)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target = target;
        _active = active;
    }

    public override bool WaitForCompletion => false;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        _go.SetActive(_active);
        yield break;
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        _go.SetActive(_active);
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _go != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out _refs) || _refs == null)
            return false;
        
        _go = _refs.GetGameObject(_target);

        return _go != null;
    }

}
