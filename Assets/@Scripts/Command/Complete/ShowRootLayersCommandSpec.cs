using System;
using DG.Tweening;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
[CommandMenuHint(
    "Scene",
    "Show Dialogue Layers (Root)",
    Sets = new[]
    {
        CpsCommandMenuSets.VnLayerSetup,
        CpsCommandMenuSets.VnLayerRestore,
    },
    SetOrder = -90
)]
public sealed class ShowRootLayersCommandSpec : CommandSpecBase
{
    [Header("Layers")]
    public DialogueLayerMask layers = DialogueLayerMask.MainPortraitRoot |DialogueLayerMask.Background1Root | DialogueLayerMask.DialogueBoxRoot;

    [Header("Fade")]
    [Tooltip("<= 0이면 즉시 켜기 (알파 1로 스냅)")]
    public float duration = 0f;

    public Ease ease = Ease.Linear;

    [Tooltip("true면 페이드가 끝날 때까지 Step 진행을 멈춤")]
    public bool wait = true;

    [Header("Interaction")]
    [Tooltip("대화박스 / 선택지의 상호작용을 자동으로 켤지 여부")]
    public bool enableInteraction = true;
}

public sealed class ShowRootLayersCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string                _screenId;
    private readonly string                _widgetRoleKey;
    private readonly bool                  _wait;

    private readonly DialogueLayerMask     _layers;
    private readonly float                 _duration;
    private readonly Ease                  _ease;
    private readonly bool                  _enableInteraction;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private readonly List<RectTransform>     _targets = new();

    private bool _resolveAttempted;

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public ShowRootLayersCommand(IDialogueWidgetAccess widgets, string screenId, string widgetRoleKey, bool waitForCompletion,
        DialogueLayerMask layers,
        float duration,
        Ease ease,
        bool enableInteraction)
    {
        _widgets           = widgets;
        _screenId          = screenId;
        _widgetRoleKey     = widgetRoleKey;
        _wait              = waitForCompletion;

        _layers            = layers;
        _duration          = Mathf.Max(0f, duration);
        _ease              = ease;
        _enableInteraction = enableInteraction;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        CollectLayerRoots(_refs, _layers, _targets);
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
            canvasGroup.alpha = 0f;

            remaining++;

            Tween tween = canvasGroup
                .DOFade(1f, _duration)
                .SetEase(_ease)
                .SetUpdate(true);

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

        CollectLayerRoots(_refs, _layers, _targets);
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

    private void CollectLayerRoots(IDialogueWidgetAccess.WidgetRefs refs, DialogueLayerMask layerMask, List<RectTransform> outList)
    {
        outList.Clear();
        if (refs == null) return;

        if (layerMask.HasFlag(DialogueLayerMask.Background0Root)) outList.Add(refs.BackgroundRoot00);
        if (layerMask.HasFlag(DialogueLayerMask.Background1Root)) outList.Add(refs.BackgroundRoot01);

        if (layerMask.HasFlag(DialogueLayerMask.MainPortraitRoot))     outList.Add(refs.MainStandingPortraitRoot);
        if (layerMask.HasFlag(DialogueLayerMask.SubLeftPortraitRoot))  outList.Add(refs.SubLeftStandingPortraitRoot);
        if (layerMask.HasFlag(DialogueLayerMask.SubRightPortraitRoot)) outList.Add(refs.SubRightStandingPortraitRoot);

        if (layerMask.HasFlag(DialogueLayerMask.DialogueBoxRoot)) outList.Add(refs.DialogueBoxRoot);
        if (layerMask.HasFlag(DialogueLayerMask.ChoicePanelRoot)) outList.Add(refs.ChoicePanelRoot);
    }

    private bool ResolveIfNeeded()
    {
        if (_resolveAttempted)
            return _refs != null;

        _resolveAttempted = true;

        if (_widgets == null)
            return false;

        _widgets.TryResolve(_screenId, _widgetRoleKey, out _refs);
        return _refs != null;
    }

    private static CanvasGroup GetOrAddCanvasGroup(RectTransform rect)
    {
        if (rect == null)
            return null;

        CanvasGroup canvasGroup = rect.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
            return canvasGroup;

        Debug.LogWarning(
            $"[CpsShowDialogueLayersCommand] CanvasGroup missing. Added automatically: {rect.name}",
            rect);

        return rect.gameObject.AddComponent<CanvasGroup>();
    }
}