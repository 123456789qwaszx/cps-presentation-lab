using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

[Serializable]
[CommandMenuHint("Motion", "Set Rotation", Order = 70)]
public sealed class SetRotationCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.StandingPortraitImage;

    [Header("Rotation (Z axis)")]
    /// <summary>
    /// 최종 Z각도 (localEulerAngles.z 기준).
    /// </summary>
    public float toAngle = 0f;

    /// <summary>
    /// 시작 각도를 강제로 고정할지.
    /// true면 startAngle에서부터 트윈이 시작된다.
    /// </summary>
    public bool overrideStartAngle = false;

    /// <summary>
    /// overrideStartAngle 이 true일 때만 사용.
    /// </summary>
    public float startAngle = 0f;

    [Header("Tween")]
    /// <summary>
    /// <= 0이면 즉시 적용 (트윈 없이 스냅).
    /// </summary>
    public float duration = 0f;

    public Ease ease = Ease.OutCubic;

    /// <summary>
    /// true면 트윈이 끝날 때까지 Step 진행을 멈춤.
    /// </summary>
    public bool wait = false;

    /// <summary>
    /// true면 기존 회전 관련 트윈을 끊고 시작.
    /// </summary>
    public bool killTween = true;
}


public sealed class CpsSetRotationCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly DialogueWidgetTarget _target;
    private readonly float _toAngle;
    private readonly bool  _overrideStartAngle;
    private readonly float _startAngle;

    private readonly float _duration;
    private readonly Ease  _ease;
    private readonly bool  _wait;
    private readonly bool  _killTween;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;
    private bool _resolved;

    public CpsSetRotationCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        float toAngle,
        bool overrideStartAngle,
        float startAngle,
        float duration,
        Ease ease,
        bool waitForCompletion,
        bool killTween)
    {
        _widgets  = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target            = target;
        _toAngle           = toAngle;
        _overrideStartAngle = overrideStartAngle;
        _startAngle        = startAngle;

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

        // 시작 각도를 아예 박아두고 싶을 때
        if (_overrideStartAngle)
            SetLocalEulerZ(_rect, _startAngle);

        if (_duration <= 0f)
        {
            // 스냅
            SetLocalEulerZ(_rect, _toAngle);
            yield break;
        }

        // DOBlendableLocalRotateBy를 쓰는 방법도 있지만,
        // 여기서는 절대각도로 맞추는 게 목적이라 custom tween으로 간다.
        Tween tween = DOTween
            .To(
                () => GetLocalEulerZ(_rect),
                angle => SetLocalEulerZ(_rect, angle),
                _toAngle,
                _duration
            )
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

        SetLocalEulerZ(_rect, _toAngle);
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
        return _rect != null;
    }

    private static float GetLocalEulerZ(RectTransform rect)
    {
        return rect.localEulerAngles.z;
    }

    private static void SetLocalEulerZ(RectTransform rect, float z)
    {
        Vector3 e = rect.localEulerAngles;
        e.z = z;
        rect.localEulerAngles = e;
    }
}
