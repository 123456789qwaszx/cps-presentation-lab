using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[Serializable]
[CommandMenuHint(
    "VFX",
    "Bubble Pulse (Persistent)",
    SetOrder = 50,
    Order = 50)]
public sealed class BubblePulseCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.LineBodyImage; // 예시. 너 프로젝트 타겟에 맞춰 변경

    [Header("Key")]
    [Tooltip("비우면 자동 생성: screenId/roleKey/target 기반")]
    public string effectKey;

    [Header("Pulse")]
    [Tooltip("펄스 최대 스케일 배율. 예: 1.04")]
    public float maxScale = 1.04f;

    [Tooltip("1회 왕복(Yoyo) 주기(초). 예: 0.8")]
    public float periodSeconds = 0.8f;

    [Tooltip("시작 딜레이(초)")]
    public float startDelay = 0f;

    [Header("Replace")]
    [Tooltip("같은 effectKey가 이미 돌고 있을 때 교체 정책")]
    public ReplacePolicy replacePolicy = ReplacePolicy.Cancel;

    [Header("Behavior")]
    [Tooltip("스킵 중에는 시작하지 않음")]
    public bool ignoreWhenSkipping = true;
}

/// <summary>
/// Run-scoped persistent pulse.
/// - Non-blocking
/// - NOT step-scoped (doesn't implement IStepScopedCommand)
/// - Replaced via PersistentEffectRegistry by effectKey
/// </summary>
public sealed class BubblePulseCommand : ISequenceCommand
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly PersistentEffectRegistry _effects;

    private readonly string _screenId;
    private readonly string _widgetRoleKey;

    private readonly DialogueWidgetTarget _target;
    private readonly string _effectKey;
    private readonly float _maxScale;
    private readonly float _periodSeconds;
    private readonly float _startDelay;
    private readonly ReplacePolicy _replacePolicy;
    private readonly bool _ignoreWhenSkipping;

    private bool _resolveAttempted;
    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;

    public bool WaitForCompletion => false;

    public BubblePulseCommand(
        IDialogueWidgetAccess widgets,
        PersistentEffectRegistry effects,
        string screenId,
        string widgetRoleKey,
        DialogueWidgetTarget target,
        string effectKey,
        float maxScale,
        float periodSeconds,
        float startDelay,
        ReplacePolicy replacePolicy,
        bool ignoreWhenSkipping)
    {
        _widgets = widgets;
        _effects = effects;

        _screenId = screenId;
        _widgetRoleKey = widgetRoleKey;

        _target = target;
        _effectKey = effectKey;

        _maxScale = Mathf.Max(0.0001f, maxScale);
        _periodSeconds = Mathf.Max(0f, periodSeconds);
        _startDelay = Mathf.Max(0f, startDelay);

        _replacePolicy = replacePolicy;
        _ignoreWhenSkipping = ignoreWhenSkipping;
    }

    public IEnumerator Execute(CommandRunScope scope)
    {
        if (scope == null) yield break;
        if (_ignoreWhenSkipping && scope.IsSkipping) yield break;
        if (!ResolveIfNeeded()) yield break;
        if (_effects == null) yield break;

        string key = BuildKey();

        // Replace current effect (single-owner per key)
        _effects.ReplaceRun(
            effectKey: key,
            scope: scope,
            replacePolicy: _replacePolicy,
            create: () =>
            {
                // Ownership rule: same rect should not accumulate multiple pulses
                _rect.DOKill(false);

                // Ensure we pulse relative to current scale
                Vector3 baseScale = _rect.localScale;
                Vector3 peakScale = baseScale * _maxScale;

                float half = Mathf.Max(0.0001f, _periodSeconds * 0.5f);

                Tween t = _rect
                    .DOScale(peakScale, half)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetUpdate(true)
                    .SetDelay(_startDelay);

                // Optional: on finish/cancel, restore base scale (you can choose policy)
                return new EffectInstance(
                    cancel: () =>
                    {
                        if (t != null && t.IsActive()) t.Kill();
                        if (_rect != null) _rect.localScale = baseScale;
                    },
                    finish: () =>
                    {
                        if (t != null && t.IsActive()) t.Complete();
                        if (_rect != null) _rect.localScale = baseScale;
                    }
                );
            });

        yield break;
    }

    private string BuildKey()
    {
        if (!string.IsNullOrWhiteSpace(_effectKey))
            return _effectKey;

        // Auto key: stable + unique enough for your authoring style
        return $"{_screenId}:{_widgetRoleKey}:{_target}:BubblePulse";
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
                $"[BubblePulseCommand] Rect not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        return true;
    }
}