using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

[Serializable]
[CommandMenuHint("Motion", "Set Scale", Order = 60)]
public sealed class SetScaleCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.StandingPortraitImage;

    [Header("Scale")]
    /// <summary>
    /// 최종 스케일 (localScale).
    /// </summary>
    public Vector3 toScale = Vector3.one;

    /// <summary>
    /// 시작 스케일을 강제로 고정할지.
    /// true면 startScale에서부터 트윈이 시작된다.
    /// </summary>
    public bool overrideStartScale = false;

    /// <summary>
    /// overrideStartScale 이 true일 때만 사용.
    /// </summary>
    public Vector3 startScale = Vector3.one;

    [Header("Tween")]
    /// <summary>
    /// <= 0이면 즉시 적용 (트윈 없이 스냅).
    /// </summary>
    public float duration = 0f;

    public Ease ease = Ease.OutCubic;

    /// <summary>
    /// true면 트윈이 끝날 때까지 Step 진행 멈춤.
    /// </summary>
    public bool wait = false;

    /// <summary>
    /// true면 기존 scale 관련 트윈을 끊고 시작.
    /// </summary>
    public bool killTween = true;
}

public sealed class CpsSetScaleCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly DialogueWidgetTarget _target;
    private readonly Vector3 _toScale;
    private readonly bool _overrideStartScale;
    private readonly Vector3 _startScale;

    private readonly float _duration;
    private readonly Ease  _ease;
    private readonly bool  _wait;
    private readonly bool  _killTween;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;
    private bool _resolved;

    public CpsSetScaleCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        Vector3 toScale,
        bool overrideStartScale,
        Vector3 startScale,
        float duration,
        Ease ease,
        bool waitForCompletion,
        bool killTween)
    {
        _widgets  = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target            = target;
        _toScale           = toScale;
        _overrideStartScale = overrideStartScale;
        _startScale        = startScale;

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

        // 시작 스케일을 아예 박아두고 싶을 때
        if (_overrideStartScale)
            _rect.localScale = _startScale;

        if (_duration <= 0f)
        {
            // 스냅
            _rect.localScale = _toScale;
            yield break;
        }

        Tween tween = _rect
            .DOScale(_toScale, _duration)
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

        // 스킵 시 최종 포즈로 바로 고정
        _rect.localScale = _toScale;
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
}
