using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[Serializable]
[CommandMenuHint(
    "Motion",
    "Move By (XY)",
    Order = 40)]
public sealed class MoveByCommandSpec : CommandSpecBase
{
    [Header("Target (Track or Rig)")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.MainStandingPortraitTrack;

    [Header("Delta (relative offset)")]
    [Tooltip("현재 anchoredPosition 기준으로 더해질 오프셋(픽셀 단위).")]
    public Vector2 delta = Vector2.zero;

    [Header("Tween")]
    /// <summary>
    /// 트윈 시간. <= 0이면 즉시 dest로 스냅.
    /// </summary>
    public float duration = 0.4f;

    public Ease ease = Ease.OutCubic;

    [Tooltip("체크하면 트윈이 끝날 때까지 Step 진행을 멈춥니다.")]
    public bool wait = false;

    [Header("Options")]
    [Tooltip("체크하면 기존 위치 관련 트윈을 끊고 시작합니다.")]
    public bool killTween = true;
}

public sealed class MoveByCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string                _screenId;
    private readonly string                _widgetRoleKey;
    private readonly bool                  _wait;

    private readonly DialogueWidgetTarget  _target;
    private readonly Vector2               _delta;
    private readonly float                 _duration;
    private readonly Ease                  _ease;
    private readonly bool                  _killTween;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform                     _rect;
    private bool                              _resolveAttempted;

    private bool   _hasComputedDest;
    private Vector2 _startPos;
    private Vector2 _destPos;

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public MoveByCommand(
        IDialogueWidgetAccess widgets,
        string                screenId,
        string                widgetRoleKey,
        bool                  waitForCompletion,

        DialogueWidgetTarget  target,
        Vector2               delta,
        float                 duration,
        Ease                  ease,
        bool                  killTween)
    {
        _widgets       = widgets;
        _screenId      = screenId;
        _widgetRoleKey = widgetRoleKey;
        _wait          = waitForCompletion;

        _target    = target;
        _delta     = delta;
        _duration  = Mathf.Max(0f, duration);
        _ease      = ease;
        _killTween = killTween;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        if (_killTween)
            _rect.DOKill(false);

        ComputeDestIfNeeded();

        if (_duration <= 0f)
        {
            // 스냅 이동
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

        if (_killTween)
            _rect.DOKill(false);

        ComputeDestIfNeeded();
        _rect.anchoredPosition = _destPos;
    }

    private void ComputeDestIfNeeded()
    {
        if (_hasComputedDest)
            return;

        _hasComputedDest = true;
        _startPos        = _rect.anchoredPosition;
        _destPos         = _startPos + _delta;
    }

    private bool ResolveIfNeeded()
    {
        if (_resolveAttempted)
            return _rect != null;

        _resolveAttempted = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null)
            return false;

        _rect = _refs.GetRect(_target);
        if (_rect == null)
        {
            Debug.LogWarning(
                $"[MoveByCommand] RectTransform not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        return true;
    }
}
