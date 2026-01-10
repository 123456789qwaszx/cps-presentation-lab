using System;
using UnityEngine;
using UnityEngine.UI;
using IEnumerator = System.Collections.IEnumerator;

[Serializable]
public sealed class SetColorCommandSpec : CommandSpecBase
{
    public CpsWidgetRefTarget target = CpsWidgetRefTarget.Auto;

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

    private readonly CpsWidgetRefTarget _target;
    private readonly Color _color;
    private readonly bool _preserveAlpha;

    private Graphic _graphic;
    private bool _resolved;

    public CpsSetColorCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        CpsWidgetRefTarget target,
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

        if (!_widgets.TryResolve(_screenId, _widgetId, out var refs) || refs == null)
            return false;

        _graphic = ResolveGraphic(refs, _target);
        return _graphic != null;
    }

    private static Graphic ResolveGraphic(IDialogueWidgetAccess.WidgetRefs refs, CpsWidgetRefTarget target)
    {
        if (refs == null) return null;
        if (target == CpsWidgetRefTarget.Auto) target = CpsWidgetRefTarget.PortraitGraphic;

        switch (target)
        {
            case CpsWidgetRefTarget.PortraitImage:   return refs.PortraitImage; // Image : Graphic
            default:                                 return refs.PortraitImage;
        }
    }
}
