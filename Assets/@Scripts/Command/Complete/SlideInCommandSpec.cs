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
    [Header("Target (Track)")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.MainStandingPortraitTrack;

    [Header("Slide")]
    public CpsSlideFrom from = CpsSlideFrom.Left;

    [Tooltip("슬라이드 시작 위치를 얼마나 멀리에서 잡을지 (픽셀). <= 0이면 기본값 사용.")]
    public float distance = 480f;

    [Header("Tween")]
    [Tooltip("트윈 시간. <= 0이면 즉시 도착 위치로 스냅.")]
    public float duration = 1.2f;

    public Ease ease = Ease.OutCubic;

    [Tooltip("체크하면 슬라이드가 끝날 때까지 Step 진행을 멈춥니다.")]
    public bool wait = false;
}

public sealed class SlideInCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;

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

    public SlideInCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        bool wait,
        DialogueWidgetTarget target,
        CpsSlideFrom from,
        float distance,
        float duration,
        Ease ease = Ease.OutCubic)
    {
        _widgets  = widgets;
        _screenId = screenId;
        _widgetRoleKey = widgetRoleKey;
        _wait     = wait;

        _target   = target;
        _from     = from;
        _distance = Mathf.Max(0f, distance);
        _duration = Mathf.Max(0f, duration);
        _ease     = ease;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        _rect.DOKill();

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
        
        tween.OnComplete(() =>
        {
            _rect.anchoredPosition = _destPos;
        })
        .BindToRun(scope);

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

        if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null)
            return false;

        _rect = _refs.GetRect(_target);
        if (_rect == null)
            return false;

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