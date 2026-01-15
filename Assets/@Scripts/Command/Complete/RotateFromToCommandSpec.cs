using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

[Serializable]
[CommandMenuHint(
    "Motion",
    "Rotate (From → To)"
)]
public sealed class RotateFromToCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.None;

    [Header("Rotation (localEulerAngles)")]
    /// <summary>
    /// 최종 Euler 각도 (localEulerAngles 기준, X/Y/Z 모두 사용 가능).
    /// </summary>
    public Vector3 toEuler = Vector3.zero;
    
    [Header("From")]
    /// <summary>
    /// true면 fromEuler를 시작 각도로 사용하고,
    /// false면 현재 localEulerAngles에서부터 시작한다.
    /// </summary>
    public bool overrideFromEuler = false;

    /// <summary>
    /// overrideFromEuler가 true일 때만 사용되는 시작 각도.
    /// </summary>
    public Vector3 fromEuler = Vector3.zero;

    [Header("Tween")]
    /// <summary>
    /// 트윈 시간. <= 0이면 즉시 toEuler로 스냅.
    /// </summary>
    public float duration = 0.4f;

    public Ease ease = Ease.OutCubic;

    /// <summary>
    /// true면 트윈이 끝날 때까지 Step 진행을 멈춤.
    /// </summary>
    public bool wait = false;

    [Header("Options")]
    /// <summary>
    /// true면 기존 회전 관련 트윈을 끊고 시작.
    /// </summary>
    public bool killTween = true;
}

public sealed class RotateFromToCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;
    private readonly bool _wait;

    private readonly DialogueWidgetTarget _target;
    private readonly Vector3 _toEuler;
    private readonly bool _overrideFromEuler;
    private readonly Vector3 _fromEuler;
    private readonly float _duration;
    private readonly Ease _ease;
    private readonly bool _killTween;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;

    private bool _resolveAttempted;

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public RotateFromToCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        bool waitForCompletion,

        DialogueWidgetTarget target,
        Vector3 toEuler,
        bool overrideFromEuler,
        Vector3 fromEuler,
        float duration,
        Ease ease,
        bool killTween)
    {
        _widgets       = widgets;
        _screenId      = screenId;
        _widgetRoleKey = widgetRoleKey;
        _wait          = waitForCompletion;

        _target            = target;
        _toEuler           = toEuler;
        _overrideFromEuler = overrideFromEuler;
        _fromEuler         = fromEuler;
        _duration          = Mathf.Max(0f, duration);
        _ease              = ease;
        _killTween         = killTween;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        if (_killTween)
            _rect.DOKill(false);

        if (_overrideFromEuler)
        {
            SetLocalEuler(_rect, _fromEuler);
        }

        if (_duration <= 0f)
        {
            SetLocalEuler(_rect, _toEuler);
            yield break;
        }

        Tween tween = _rect
            .DOLocalRotate(_toEuler, _duration, RotateMode.Fast)
            .SetEase(_ease)
            .SetUpdate(true);

        tween.OnComplete(() =>
            {
                SetLocalEuler(_rect, _toEuler);
            })
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

        SetLocalEuler(_rect, _toEuler);
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
        
        return true;
    }

    private static void SetLocalEuler(RectTransform rect, Vector3 euler)
    {
        rect.localEulerAngles = euler;
    }
}