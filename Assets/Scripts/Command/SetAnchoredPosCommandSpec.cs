using System;
using DG.Tweening;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

[Serializable]
public sealed class SetAnchoredPosCommandSpec : CommandSpecBase
{
    public CpsWidgetRefTarget target = CpsWidgetRefTarget.Auto;

    [Header("Position")]
    public Vector2 value;

    [Tooltip("true면 현재 anchoredPosition에 value를 더함(오프셋). false면 절대 좌표.")]
    public bool relative = true;

    [Tooltip("true면 기존 트윈을 끊고 적용.")]
    public bool killTween = true;
}


public sealed class CpsSetAnchoredPosCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly CpsWidgetRefTarget _target;
    private readonly Vector2 _value;
    private readonly bool _relative;
    private readonly bool _killTween;

    private RectTransform _rect;
    private bool _resolved;

    public CpsSetAnchoredPosCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        CpsWidgetRefTarget target,
        Vector2 value,
        bool relative = true,
        bool killTween = true)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target = target;
        _value = value;
        _relative = relative;
        _killTween = killTween;
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
        if (_killTween)
            _rect.DOKill(false);

        Vector2 dest = _relative ? (_rect.anchoredPosition + _value) : _value;
        _rect.anchoredPosition = dest;
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _rect != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out var refs) || refs == null)
            return false;

        // Auto는 Rect 이동이니까 PortraitRect가 기본
        _rect = ResolveRect(refs, _target);
        return _rect != null;
    }

    private static RectTransform ResolveRect(IDialogueWidgetAccess.WidgetRefs refs, CpsWidgetRefTarget target)
    {
        if (refs == null) return null;
        if (target == CpsWidgetRefTarget.Auto) target = CpsWidgetRefTarget.PortraitRect;

        switch (target)
        {
            case CpsWidgetRefTarget.PortraitRect:  return refs.PortraitRect;
            case CpsWidgetRefTarget.PortraitImage: return refs.PortraitImage != null ? refs.PortraitImage.rectTransform : null;
            default: return refs.PortraitRect;
        }
    }
}
