using System;
using UnityEngine;
using System.Collections;

public enum CpsWidgetRefTarget
{
    Auto = 0,
    BodyText,
    NameText,
    PortraitGraphic,
    PortraitRect,
    PortraitImage,
}

[Serializable]
public sealed class SetActiveCommandSpec : CommandSpecBase
{
    public CpsWidgetRefTarget target = CpsWidgetRefTarget.Auto;
    public bool active = true;
}

public sealed class CpsSetActiveCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly CpsWidgetRefTarget _target;
    private readonly bool _active;

    private GameObject _go;
    private bool _resolved;

    public CpsSetActiveCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        CpsWidgetRefTarget target,
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

        if (!_widgets.TryResolve(_screenId, _widgetId, out var refs) || refs == null)
            return false;

        Component c = ResolveComponent(refs, _target, defaultAuto: CpsWidgetRefTarget.PortraitGraphic);
        _go = c != null ? c.gameObject : null;

        return _go != null;
    }

    private static Component ResolveComponent(IDialogueWidgetAccess.WidgetRefs refs, CpsWidgetRefTarget t, CpsWidgetRefTarget defaultAuto)
    {
        if (refs == null) return null;
        if (t == CpsWidgetRefTarget.Auto) t = defaultAuto;

        switch (t)
        {
            case CpsWidgetRefTarget.BodyText:        return refs.BodyText;
            case CpsWidgetRefTarget.NameText:        return refs.NameText;
            case CpsWidgetRefTarget.PortraitRect:    return refs.PortraitRect;
            case CpsWidgetRefTarget.PortraitImage:   return refs.PortraitImage;
            default:                                 return refs.PortraitImage;
        }
    }
}
