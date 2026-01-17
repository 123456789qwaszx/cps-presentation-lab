using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[Serializable]
[CommandMenuHint(
    "Scene",
    "Show Dialogue Layers (Targets)",
    Sets  = new[]
    {
        CpsCommandMenuSets.VnLayerSetup,
    },
    SetOrder = -80
)]
public sealed class ShowTargetsCommandSpec : CommandSpecBase
{
    public DialogueTargetMask targets = DialogueTargetMask.None;

    [Header("Fade")]
    [Tooltip("<= 0이면 즉시 켜기 (알파 1로 스냅)")]
    public float duration = 0f;

    public Ease ease = Ease.Linear;

    [Tooltip("true면 페이드가 끝날 때까지 Step 진행을 멈춤")]
    public bool wait = true;

    [Header("Interaction")]
    [Tooltip("true면 보여줄 때 입력을 다시 허용(interactable/blocksRaycasts=true)")]
    public bool enableInteraction = true;
}

public sealed class ShowTargetsCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;
    private readonly bool _wait;

    private readonly DialogueTargetMask _targetsMask;
    private readonly float _duration;
    private readonly Ease _ease;
    private readonly bool _enableInteraction;

    private IDialogueWidgetAccess.WidgetRefs _refs;

    private readonly List<RectTransform> _targets = new();

    private bool _resolveAttempted;

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public ShowTargetsCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        bool waitForCompletion,
        DialogueTargetMask targetsMask,
        float duration,
        Ease ease,
        bool enableInteraction)
    {
        _widgets           = widgets;
        _screenId          = screenId;
        _widgetRoleKey     = widgetRoleKey;
        _wait              = waitForCompletion;

        _targetsMask       = targetsMask;
        _duration          = Mathf.Max(0f, duration);
        _ease              = ease;
        _enableInteraction = enableInteraction;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        if (_targets.Count == 0)
            yield break;

        if (_duration <= 0f)
        {
            SnapOnTargets(_targets);
            yield break;
        }

        int remaining = 0;

        for (int i = 0; i < _targets.Count; i++)
        {
            RectTransform rect = _targets[i];
            if (rect == null)
                continue;

            CanvasGroup canvasGroup = GetOrAddCanvasGroup(rect);
            if (canvasGroup == null)
                continue;

            canvasGroup.DOKill(false);

            Tween tween = canvasGroup
                .DOFade(1f, _duration)
                .SetEase(_ease)
                .SetUpdate(true);

            remaining++;

            tween.OnComplete(() =>
                {
                    canvasGroup.alpha = 1f;

                    if (_enableInteraction)
                    {
                        canvasGroup.interactable   = true;
                        canvasGroup.blocksRaycasts = true;
                    }

                    remaining--;
                })
                .BindToStep(scope);
        }

        if (!_wait)
            yield break;

        while (remaining > 0)
            yield return null;
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        SnapOnTargets(_targets);
    }

    private void SnapOnTargets(List<RectTransform> targets)
    {
        if (targets == null || targets.Count == 0)
            return;

        for (int i = 0; i < targets.Count; i++)
        {
            RectTransform rect = targets[i];
            if (rect == null)
                continue;

            CanvasGroup canvasGroup = GetOrAddCanvasGroup(rect);
            if (canvasGroup == null)
                continue;

            canvasGroup.DOKill(false);
            canvasGroup.alpha = 1f;

            if (_enableInteraction)
            {
                canvasGroup.interactable   = true;
                canvasGroup.blocksRaycasts = true;
            }
        }
    }

    private bool ResolveIfNeeded()
    {
        if (_resolveAttempted)
            return _refs != null;

        _resolveAttempted = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null)
            return false;

        // 보통 primitive만 매핑하므로 dedupe=false가 기본.
        // 혹시 실수로 같은 RectTransform이 두 번 들어갈 여지가 있으면 true로.
        DialogueTargetMaskMap.CollectRects(_refs, _targetsMask, _targets, dedupe: false);

        return true;
    }

    private static CanvasGroup GetOrAddCanvasGroup(RectTransform rect)
    {
        if (rect == null)
            return null;

        CanvasGroup canvasGroup = rect.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
            return canvasGroup;

        Debug.LogWarning($"[ShowTargetsCommand] CanvasGroup missing. Added automatically: {rect.name}", rect);
        return rect.gameObject.AddComponent<CanvasGroup>();
    }
}