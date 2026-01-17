using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

[Serializable]
[CommandMenuHint(
    "Motion",
    "Move To (XY)",
    Order = 20)]
public sealed class MoveToCommandSpec : CommandSpecBase
{
    [Header("Target (Track or Rig)")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.Standing00Track;

    [Header("Destination (absolute anchoredPosition)")] [Tooltip("도착 지점(절대 anchoredPosition, 픽셀 단위).")]
    public Vector2 toPosition = Vector2.zero;

    [Header("Tween")]
    /// <summary>
    /// 트윈 시간. <= 0이면 즉시 toPosition으로 스냅.
    /// </summary>
    public float duration = 0.4f;

    public Ease ease = Ease.OutCubic;

    [Tooltip("체크하면 트윈이 끝날 때까지 Step 진행을 멈춥니다.")]
    public bool wait = false;

    [Header("Options")] [Tooltip("체크하면 기존 위치 관련 트윈을 끊고 시작합니다.")]
    public bool killTween = true;
}

public sealed class MoveToCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;
    private readonly bool _wait;

    private readonly DialogueWidgetTarget _target;
    private readonly Vector2 _toPosition;
    private readonly float _duration;
    private readonly Ease _ease;
    private readonly bool _killTween;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;
    private bool _resolveAttempted;

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public MoveToCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        bool waitForCompletion,
        DialogueWidgetTarget target,
        Vector2 toPosition,
        float duration,
        Ease ease,
        bool killTween)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetRoleKey = widgetRoleKey;
        _wait = waitForCompletion;

        _target = target;
        _toPosition = toPosition;
        _duration = Mathf.Max(0f, duration);
        _ease = ease;
        _killTween = killTween;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        if (_killTween)
            _rect.DOKill(false);

        if (_duration <= 0f)
        {
            _rect.anchoredPosition = _toPosition;
            yield break;
        }

        Tween tween = _rect
            .DOAnchorPos(_toPosition, _duration)
            .SetEase(_ease)
            .SetUpdate(true);

        tween.OnComplete(() => { _rect.anchoredPosition = _toPosition; })
            .BindToRun(scope);

        if (_wait)
            yield return tween.WaitForCompletion();
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        if (_killTween)
            _rect.DOKill(false);

        // 스킵 시 최종 위치로 바로 고정
        _rect.anchoredPosition = _toPosition;
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
                $"[MoveToCommand] RectTransform not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        return true;
    }
}