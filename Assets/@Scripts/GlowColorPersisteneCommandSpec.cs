using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CommandMenuHint(
    "VFX",
    "Glow Color (Persistent)",
    SetOrder = 61,
    Order = 61)]
public sealed class GlowColorPersistentCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.LineBodyImage;

    [Header("Key")]
    [Tooltip("비우면 자동 생성: screenId/roleKey/target 기반")]
    public string effectKey;

    [Header("Color")]
    public Color glowColor = Color.white;

    [Tooltip("glowColor로 보간 강도 (0~1). 1이면 완전 glowColor, 0이면 원색 유지")]
    [Range(0f, 1f)] public float glowStrength = 0.35f;

    [Tooltip("원색->글로우 한 번 이동 시간(초)")]
    public float halfPeriodSeconds = 0.45f;

    public Ease ease = Ease.InOutSine;

    [Tooltip("시작 딜레이(초)")]
    public float startDelay = 0f;

    [Header("Replace")]
    public ReplacePolicy replacePolicy = ReplacePolicy.Cancel;

    [Header("Behavior")]
    public bool ignoreWhenSkipping = true;
}

public sealed class GlowColorPersistentCommand : PersistentCommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly PersistentEffectRegistry _effects;

    private readonly string _screenId;
    private readonly string _widgetRoleKey;

    private readonly DialogueWidgetTarget _target;
    private readonly string _effectKey;
    private readonly Color _glowColor;
    private readonly float _glowStrength;
    private readonly float _halfPeriod;
    private readonly Ease _ease;
    private readonly float _startDelay;
    private readonly ReplacePolicy _replacePolicy;
    private readonly bool _ignoreWhenSkipping;

    private bool _resolveAttempted;
    private IDialogueWidgetAccess.WidgetRefs _refs;
    private Graphic _graphic;

    public GlowColorPersistentCommand(
        IDialogueWidgetAccess widgets,
        PersistentEffectRegistry effects,
        string screenId,
        string widgetRoleKey,
        DialogueWidgetTarget target,
        string effectKey,
        Color glowColor,
        float glowStrength,
        float halfPeriodSeconds,
        Ease ease,
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

        _glowColor = glowColor;
        _glowStrength = Mathf.Clamp01(glowStrength);
        _halfPeriod = Mathf.Max(0.0001f, halfPeriodSeconds);
        _ease = ease;
        _startDelay = Mathf.Max(0f, startDelay);

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

        string key = BuildKey("GlowColor");

        _effects.ReplaceRun(
            effectKey: key,
            scope: scope,
            replacePolicy: _replacePolicy,
            create: () =>
            {
                if (_graphic == null)
                    return default;

                _graphic.DOKill(false);

                Color baseColor = _graphic.color;
                Color targetColor = Color.Lerp(baseColor, _glowColor, _glowStrength);

                Tween t = _graphic
                    .DOColor(targetColor, _halfPeriod)
                    .SetEase(_ease)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetDelay(_startDelay)
                    .SetUpdate(true);

                return new EffectInstance(
                    cancel: () =>
                    {
                        if (_graphic != null) _graphic.DOKill(false);
                        if (_graphic != null) _graphic.color = baseColor;
                    },
                    finish: () =>
                    {
                        if (_graphic != null) _graphic.DOKill(false);
                        if (_graphic != null) _graphic.color = baseColor;
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
        if (_graphic != null) return true;
        if (_resolveAttempted) return false;
        _resolveAttempted = true;

        if (_widgets == null) return false;
        if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null) return false;

        _graphic = _refs.GetGraphic(_target);
        if (_graphic == null)
        {
            Debug.LogWarning(
                $"[GlowColorPersistentCommand] Graphic not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        return true;
    }
}