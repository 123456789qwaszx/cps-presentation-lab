using System;
using DG.Tweening;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RectTransform = UnityEngine.RectTransform;

[Serializable]
[CommandMenuHint(
    "Scene",
    "Hide Dialogue Layers (Root)",
    Sets  = new[] { CpsCommandMenuSets.VnAllOff },
    Order = -100
)]
public sealed class HideRootLayersCommandSpec : CommandSpecBase
{
    public DialogueLayerMask layers = DialogueLayerMask.All;

    [Header("Fade")]
    [Tooltip("<= 0이면 즉시 끄기 (알파 0으로 스냅)")]
    public float duration = 0f;

    public Ease ease = Ease.Linear;

    [Tooltip("true면 페이드가 끝날 때까지 Step 진행을 멈춤")]
    public bool wait = true;

    [Header("Interaction")]
    [Tooltip("true면 숨긴 레이어의 입력을 완전히 차단(interactable/blocksRaycasts=false)")]
    public bool disableInteraction = true;
}

public sealed class HideRootLayersCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;
    private readonly bool  _wait;

    private readonly DialogueLayerMask _layers;
    private readonly float _duration;
    private readonly Ease  _ease;
    private readonly bool  _disableInteraction;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private readonly List<RectTransform> _targets = new();

    private bool _resolveAttempted;

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public HideRootLayersCommand(IDialogueWidgetAccess widgets, string screenId, string widgetRoleKey, bool waitForCompletion,
        DialogueLayerMask layers,
        float duration,
        Ease ease,
        bool disableInteraction)
    {
        _widgets            = widgets;
        _screenId           = screenId;
        _widgetRoleKey      = widgetRoleKey;
        _wait               = waitForCompletion;
        _layers             = layers;
        _duration           = Mathf.Max(0f, duration);
        _ease               = ease;
        _disableInteraction = disableInteraction;
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
            SnapOffTargets(_targets);
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
                .DOFade(0f, _duration)
                .SetEase(_ease)
                .SetUpdate(true);

            remaining++;

            tween.OnComplete(() =>
                {
                    canvasGroup.alpha = 0f;

                    if (_disableInteraction)
                    {
                        canvasGroup.interactable   = false;
                        canvasGroup.blocksRaycasts = false;
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
        SnapOffTargets(_targets);
    }

    private void SnapOffTargets(List<RectTransform> targets)
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
            canvasGroup.alpha = 0f;

            if (_disableInteraction)
            {
                canvasGroup.interactable   = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }

    private static void CollectLayerRoots(IDialogueWidgetAccess.WidgetRefs refs, DialogueLayerMask layerMask, List<RectTransform> outList)
    {
        outList.Clear();
        if (refs == null) return;

        if (layerMask.HasFlag(DialogueLayerMask.Background0Root)) outList.Add(refs.BackgroundRoot0);
        if (layerMask.HasFlag(DialogueLayerMask.Background1Root)) outList.Add(refs.BackgroundRoot1);

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

        Debug.LogWarning($"[HideRootLayersCommand] CanvasGroup missing. Added automatically: {rect.name}", rect);
        return rect.gameObject.AddComponent<CanvasGroup>();
    }
}