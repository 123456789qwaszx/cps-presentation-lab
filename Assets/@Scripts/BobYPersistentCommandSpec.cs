// ------------------------------------------------------------
// 3) Bob Y (Persistent) : RectTransform anchored Y yoyo
// ------------------------------------------------------------

using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[Serializable]
[CommandMenuHint(
    "VFX",
    "Bob Y (Persistent)",
    SetOrder = 62,
    Order = 62)]
public sealed class BobYPersistentCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.MainStandingPortraitTrack;

    [Header("Key")]
    [Tooltip("비우면 자동 생성: screenId/roleKey/target 기반")]
    public string effectKey;

    [Header("Bob")]
    [Tooltip("Y로 움직일 진폭(픽셀). 예: 6~14")]
    public float amplitudeY = 10f;

    [Tooltip("base->peak 한 번 이동 시간(초)")]
    public float halfPeriodSeconds = 0.6f;

    public Ease ease = Ease.InOutSine;

    [Tooltip("시작 딜레이(초)")]
    public float startDelay = 0f;

    [Header("Replace")]
    public ReplacePolicy replacePolicy = ReplacePolicy.Cancel;

    [Header("Behavior")]
    public bool ignoreWhenSkipping = true;
}

public sealed class BobYPersistentCommand : ISequenceCommand
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly PersistentEffectRegistry _effects;

    private readonly string _screenId;
    private readonly string _widgetRoleKey;

    private readonly DialogueWidgetTarget _target;
    private readonly string _effectKey;
    private readonly float _amplitudeY;
    private readonly float _halfPeriod;
    private readonly Ease _ease;
    private readonly float _startDelay;
    private readonly ReplacePolicy _replacePolicy;
    private readonly bool _ignoreWhenSkipping;

    private bool _resolveAttempted;
    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;

    public bool WaitForCompletion => false;

    public BobYPersistentCommand(
        IDialogueWidgetAccess widgets,
        PersistentEffectRegistry effects,
        string screenId,
        string widgetRoleKey,
        DialogueWidgetTarget target,
        string effectKey,
        float amplitudeY,
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

        _amplitudeY = amplitudeY;
        _halfPeriod = Mathf.Max(0.0001f, halfPeriodSeconds);
        _ease = ease;
        _startDelay = Mathf.Max(0f, startDelay);

        _replacePolicy = replacePolicy;
        _ignoreWhenSkipping = ignoreWhenSkipping;
    }

    public IEnumerator Execute(CommandRunScope scope)
    {
        if (scope == null) yield break;
        if (_ignoreWhenSkipping && scope.IsSkipping) yield break;
        if (_effects == null) yield break;
        if (!ResolveIfNeeded()) yield break;

        string key = BuildKey("BobY");

        _effects.ReplaceRun(
            effectKey: key,
            scope: scope,
            replacePolicy: _replacePolicy,
            create: () =>
            {
                _rect.DOKill(false);

                Vector2 basePos = _rect.anchoredPosition;
                Vector2 peakPos = basePos + new Vector2(0f, _amplitudeY);

                Tween t = _rect
                    .DOAnchorPos(peakPos, _halfPeriod)
                    .SetEase(_ease)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetDelay(_startDelay)
                    .SetUpdate(true);

                return new EffectInstance(
                    cancel: () =>
                    {
                        if (_rect != null) _rect.DOKill(false);
                        if (_rect != null) _rect.anchoredPosition = basePos;
                    },
                    finish: () =>
                    {
                        if (_rect != null) _rect.DOKill(false);
                        if (_rect != null) _rect.anchoredPosition = basePos;
                    }
                );
            });

        yield break;
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
                $"[BobYPersistentCommand] Rect not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        return true;
    }
}