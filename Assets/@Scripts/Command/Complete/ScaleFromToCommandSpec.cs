using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

[Serializable]
[CommandMenuHint(
    "Motion",
    "Scale (From → To)",
    Order = 70
)]
public sealed class ScaleFromToCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.None;

    [Header("Scale (XY)")]
    /// <summary>
    /// 최종 스케일 (localScale X/Y)
    /// </summary>
    public Vector2 toScale = Vector2.one;

    [Header("From")]
    /// <summary>
    /// true면 fromScale을 시작 스케일로 사용하고,
    /// false면 현재 localScale에서부터 시작한다.
    /// </summary>
    public bool overrideFromScale = false;

    /// <summary>
    /// overrideFromScale이 true일 때만 사용되는 시작 스케일 (X/Y).
    /// </summary>
    public Vector2 fromScale = Vector2.one;

    [Header("Tween")]
    /// <summary>
    /// 트윈 시간. <= 0이면 즉시 toScale로 스냅.
    /// </summary>
    public float duration = 0.4f;

    public Ease ease = Ease.OutCubic;

    public bool wait = false;

    [Header("Options")]
    public bool killTween = true;
}

public sealed class ScaleFromToCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;
    private readonly bool _wait;

    private readonly DialogueWidgetTarget _target;
    private readonly Vector2 _toScaleXY;
    private readonly bool _overrideFromScale;
    private readonly Vector2 _fromScaleXY;
    private readonly float _duration;
    private readonly Ease _ease;
    private readonly bool _killTween;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;

    private bool _resolveAttempted;

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public ScaleFromToCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        bool waitForCompletion,

        DialogueWidgetTarget target,
        Vector2 toScale,
        bool overrideFromScale,
        Vector2 fromScale,
        float duration,
        Ease ease,
        bool killTween)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetRoleKey = widgetRoleKey;
        _wait = waitForCompletion;

        _target = target;
        _toScaleXY = toScale;
        _overrideFromScale = overrideFromScale;
        _fromScaleXY = fromScale;
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

        if (_overrideFromScale)
        {
            ApplyScaleXY(_rect, _fromScaleXY);
        }

        if (_duration <= 0f)
        {
            ApplyScaleXY(_rect, _toScaleXY);
            yield break;
        }

        Vector3 endScale = _rect.localScale;
        endScale.x = _toScaleXY.x;
        endScale.y = _toScaleXY.y;

        Tween tween = _rect
            .DOScale(endScale, _duration)
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

        ApplyScaleXY(_rect, _toScaleXY);
    }
    
    public override void OnCommandCompleted(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;
        
        ApplyScaleXY(_rect, _toScaleXY);
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

    private static void ApplyScaleXY(RectTransform rect, Vector2 targetXY)
    {
        Vector3 s = rect.localScale;
        s.x = targetXY.x;
        s.y = targetXY.y;
        rect.localScale = s;
    }
}