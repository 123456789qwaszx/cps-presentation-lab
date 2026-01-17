using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
[CommandMenuHint(
    "VFX",
    "Blink Alpha (Persistent)",
    SetOrder = 60,
    Order = 60)]
public sealed class BlinkAlphaPersistentCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.DialogueBoxRoot;

    [Header("Key")]
    [Tooltip("비우면 자동 생성: screenId/roleKey/target 기반")]
    public string effectKey;

    [Header("Blink")]
    [Range(0f, 1f)] public float minAlpha = 0.25f;
    [Range(0f, 1f)] public float maxAlpha = 1.00f;

    [Tooltip("min->max 또는 max->min 한 번 이동 시간(초)")]
    public float halfPeriodSeconds = 0.35f;

    [Tooltip("시작 딜레이(초)")]
    public float startDelay = 0f;

    public Ease ease = Ease.InOutSine;

    [Header("Replace")]
    public ReplacePolicy replacePolicy = ReplacePolicy.Cancel;

    [Header("Behavior")]
    [Tooltip("Skip 중에도 켜야 한다면 false로.")]
    public bool ignoreWhenSkipping = true;
}

public sealed class BlinkAlphaPersistentCommand : PersistentCommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly PersistentEffectRegistry _effects;

    private readonly string _screenId;
    private readonly string _widgetRoleKey;

    private readonly DialogueWidgetTarget _target;
    private readonly string _effectKey;
    private readonly float _minAlpha;
    private readonly float _maxAlpha;
    private readonly float _halfPeriod;
    private readonly float _startDelay;
    private readonly Ease _ease;
    private readonly ReplacePolicy _replacePolicy;
    private readonly bool _ignoreWhenSkipping;

    private bool _resolveAttempted;
    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;

    public BlinkAlphaPersistentCommand(
        IDialogueWidgetAccess widgets,
        PersistentEffectRegistry effects,
        string screenId,
        string widgetRoleKey,
        DialogueWidgetTarget target,
        string effectKey,
        float minAlpha,
        float maxAlpha,
        float halfPeriodSeconds,
        float startDelay,
        Ease ease,
        ReplacePolicy replacePolicy,
        bool ignoreWhenSkipping)
    {
        _widgets = widgets;
        _effects = effects;
        _screenId = screenId;
        _widgetRoleKey = widgetRoleKey;

        _target = target;
        _effectKey = effectKey;

        _minAlpha = Mathf.Clamp01(minAlpha);
        _maxAlpha = Mathf.Clamp01(maxAlpha);
        _halfPeriod = Mathf.Max(0.0001f, halfPeriodSeconds);
        _startDelay = Mathf.Max(0f, startDelay);

        _ease = ease;
        _replacePolicy = replacePolicy;
        _ignoreWhenSkipping = ignoreWhenSkipping;
    }

    protected override SkipPolicy SkipPolicy =>
        _ignoreWhenSkipping ? SkipPolicy.Ignore : SkipPolicy.ExecuteEvenIfSkipping;

    protected override void ApplyPersistent(CommandRunScope scope)
    {
        if (scope == null) return;
        if (_effects == null) return;
        if (!ResolveIfNeeded()) return;

        string key = BuildKey("BlinkAlpha");

        _effects.ReplaceRun(
            effectKey: key,
            scope: scope,
            replacePolicy: _replacePolicy,
            create: () =>
            {
                CanvasGroup cg = GetOrAddCanvasGroup(_rect);
                if (cg == null)
                    return default;

                cg.DOKill(false);

                float baseAlpha = cg.alpha;

                float a0 = baseAlpha;
                float aMin = _minAlpha;
                float aMax = _maxAlpha;

                // min==max -> no loop, but still keep ownership and restore on stop.
                if (Mathf.Abs(aMax - aMin) <= 0.0001f)
                {
                    cg.alpha = aMax;

                    return new EffectInstance(
                        cancel: () => { if (cg != null) cg.alpha = baseAlpha; },
                        finish: () => { if (cg != null) cg.alpha = baseAlpha; }
                    );
                }

                // Start by moving to the farther endpoint, then loop between endpoints.
                float startTarget = (Mathf.Abs(a0 - aMax) > Mathf.Abs(a0 - aMin)) ? aMax : aMin;

                Tween kickoff = cg
                    .DOFade(startTarget, _halfPeriod)
                    .SetEase(_ease)
                    .SetDelay(_startDelay)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        if (cg == null) return;

                        // kill kickoff and chain safely, then create the infinite yoyo
                        cg.DOKill(false);
                        cg.alpha = startTarget;

                        cg.DOFade(startTarget == aMax ? aMin : aMax, _halfPeriod)
                          .SetEase(_ease)
                          .SetLoops(-1, LoopType.Yoyo)
                          .SetUpdate(true);
                    });

                return new EffectInstance(
                    cancel: () =>
                    {
                        if (cg != null) cg.DOKill(false);
                        if (cg != null) cg.alpha = baseAlpha;
                    },
                    finish: () =>
                    {
                        if (cg != null) cg.DOKill(false);
                        if (cg != null) cg.alpha = baseAlpha;
                    }
                );
            });
    }

    private string BuildKey(string kind)
    {
        if (!string.IsNullOrWhiteSpace(_effectKey)) return _effectKey;
        return $"{_screenId}:{_widgetRoleKey}:{_target}:{kind}";
    }

    private bool ResolveIfNeeded()
    {
        if (_rect != null) return true;
        if (_resolveAttempted) return false;
        _resolveAttempted = true;

        if (_widgets == null) return false;
        if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null) return false;

        _rect = _refs.GetRect(_target);
        if (_rect == null)
        {
            Debug.LogWarning(
                $"[BlinkAlphaPersistentCommand] Rect not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        return true;
    }

    private static CanvasGroup GetOrAddCanvasGroup(RectTransform rect)
    {
        if (rect == null) return null;
        CanvasGroup cg = rect.GetComponent<CanvasGroup>();
        if (cg != null) return cg;
        return rect.gameObject.AddComponent<CanvasGroup>();
    }
}