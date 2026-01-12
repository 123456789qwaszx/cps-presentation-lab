using System;
using UnityEngine;
using UnityEngine.UI;
using IEnumerator = System.Collections.IEnumerator;

[Serializable]
[CommandMenuHint("Visual", "Set Color", Order = 30)]
public sealed class SetColorCommandSpec : CommandSpecBase
{
    public DialogueWidgetTarget target = DialogueWidgetTarget.MainStandingPortraitImage;

    [Header("Color")]
    public Color color = Color.white;

    [Tooltip("true면 target Graphic의 alpha는 유지하고 RGB만 바꿈.")]
    public bool preserveAlpha = true;
}

public sealed class CpsSetColorCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly DialogueWidgetTarget _target;
    private readonly Color _color;
    private readonly bool _preserveAlpha;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private Graphic _graphic;
    private bool _resolved;

    public CpsSetColorCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        Color color,
        bool preserveAlpha = true)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target = target;
        _color = color;
        _preserveAlpha = preserveAlpha;
    }

    public override bool WaitForCompletion => false;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        Apply();
        yield break;
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        Apply();
    }

    private void Apply()
    {
        Color c = _color;

        if (_preserveAlpha)
        {
            var cur = _graphic.color;
            c.a = cur.a;
        }

        _graphic.color = c;
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _graphic != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out _refs) || _refs == null)
            return false;

        _graphic = _refs.GetGraphic(_target);
        return _graphic != null;
    }
}
