using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using RectTransform = UnityEngine.RectTransform;

public enum CpsSlideFrom
{
    Left = 0,
    Right,
    Up,
    Down,
}

[Serializable]
[CommandMenuHint("Motion", "Slide In", Order = 10)]
public sealed class SlideInCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.MainStandingPortraitTrack;

    [Header("Slide")]
    public CpsSlideFrom from = CpsSlideFrom.Left;

    /// <summary>
    /// <= 0이면 Config 기본값 사용
    /// </summary>
    public float distance = -1f;

    /// <summary>
    /// <= 0이면 Config 기본값 사용
    /// </summary>
    public float duration = -1f;

    public Ease ease = Ease.OutCubic;

    /// <summary>
    /// true면 슬라이드가 끝날 때까지 Step 진행을 멈춤 (기본 false 추천)
    /// </summary>
    public bool wait = false;
}

public sealed class CpsSlideInCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly DialogueWidgetTarget _target;
    private readonly CpsSlideFrom _from;
    private readonly float _distance;
    private readonly float _duration;
    private readonly Ease  _ease;
    private readonly bool  _wait;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;
    private Vector2 _destPos;
    private bool _resolved;

    public CpsSlideInCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        CpsSlideFrom from,
        float distance,
        float duration,
        Ease ease = Ease.OutCubic,
        bool waitForCompletion = false)
    {
        _widgets  = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target   = target;
        _from     = from;
        _distance = Mathf.Max(0f, distance);
        _duration = Mathf.Max(0f, duration);
        _ease     = ease;
        _wait     = waitForCompletion;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        _rect.DOKill(false);

        Vector2 start = _destPos + GetOffset(_from, _distance);
        _rect.anchoredPosition = start;

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

        _rect.DOKill(false);
        _rect.anchoredPosition = _destPos;
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _rect != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out _refs) || _refs == null)
            return false;

        _rect = _refs.GetRect(_target);
        if (_rect == null)
            return false;

        // “dest = 현재 레이아웃 위치” (아날로그 규칙)
        _destPos = _rect.anchoredPosition;
        return true;
    }

    private static Vector2 GetOffset(CpsSlideFrom from, float distance)
    {
        switch (from)
        {
            case CpsSlideFrom.Right: return new Vector2(+distance, 0f);
            case CpsSlideFrom.Up:    return new Vector2(0f, +distance);
            case CpsSlideFrom.Down:  return new Vector2(0f, -distance);
            case CpsSlideFrom.Left:
            default:                 return new Vector2(-distance, 0f);
        }
    }
}
