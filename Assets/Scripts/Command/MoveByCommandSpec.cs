using System;
using DG.Tweening;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

[Serializable]
public sealed class MoveByCommandSpec : CommandSpecBase
{
    public CpsWidgetRefTarget target = CpsWidgetRefTarget.Auto;

    [Header("Delta (relative offset)")]
    public Vector2 delta;

    [Header("Tween")]
    [Tooltip("<=0이면 Config 기본 duration 사용.")]
    public float duration = -1f;

    public Ease ease = Ease.OutCubic;

    [Tooltip("true면 스텝이 이 이동이 끝날 때까지 대기.")]
    public bool wait = false;
}

public sealed class CpsMoveByCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly CpsWidgetRefTarget _target;
    private readonly Vector2 _delta;

    private readonly float _duration;
    private readonly Ease  _ease;
    private readonly bool  _wait;
    private readonly bool  _killTween;

    private RectTransform _rect;
    private bool _resolved;

    // ✅ Skip/Cancel 때 오버슈트 방지용 캐시
    private bool _hasComputedDest;
    private Vector2 _startPos;
    private Vector2 _destPos;

    public CpsMoveByCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        CpsWidgetRefTarget target,
        Vector2 delta,
        float duration,
        Ease ease,
        bool waitForCompletion,
        bool killTween = true)
    {
        _widgets  = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target = target;
        _delta  = delta;

        _duration = Mathf.Max(0f, duration);
        _ease     = ease;
        _wait     = waitForCompletion;
        _killTween = killTween;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        if (_killTween)
            _rect.DOKill(false);

        ComputeDestIfNeeded();

        if (_duration <= 0f)
        {
            _rect.anchoredPosition = _destPos;
            yield break;
        }

        Tween tween = _rect
            .DOAnchorPos(_destPos, _duration)
            .SetEase(_ease)
            .SetUpdate(true);

        tween.BindToStep(scope);

        if (_wait)
            yield return tween.WaitForCompletion();
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        ComputeDestIfNeeded();
        _rect.anchoredPosition = _destPos;
    }

    private void ComputeDestIfNeeded()
    {
        if (_hasComputedDest) return;
        _hasComputedDest = true;

        _startPos = _rect.anchoredPosition;
        _destPos  = _startPos + _delta;
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _rect != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out var refs) || refs == null)
            return false;

        _rect = ResolveRect(refs, _target);
        return _rect != null;
    }

    private static RectTransform ResolveRect(IDialogueWidgetAccess.WidgetRefs refs, CpsWidgetRefTarget target)
    {
        if (refs == null) return null;
        if (target == CpsWidgetRefTarget.Auto) target = CpsWidgetRefTarget.PortraitRect;

        switch (target)
        {
            case CpsWidgetRefTarget.PortraitRect:     return refs.PortraitRect;
            case CpsWidgetRefTarget.PortraitImage:    return refs.PortraitImage != null ? refs.PortraitImage.rectTransform : null;
            default:                                   return refs.PortraitRect;
        }
    }
}