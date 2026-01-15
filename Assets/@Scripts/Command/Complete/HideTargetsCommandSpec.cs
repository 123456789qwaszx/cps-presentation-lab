using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[Serializable]
[CommandMenuHint(
    "Scene",
    "Hide Dialogue Layers (Targets)",
    Sets  = new[]
    {
        CpsCommandMenuSets.VnLayerSetup,
    },
    SetOrder = -80
)]
public sealed class HideTargetsCommandSpec : CommandSpecBase
{
    public DialogueTargetMask targets = DialogueTargetMask.MainEmoji | DialogueTargetMask.ProtagonistCutin;

    [Header("Fade")]
    [Tooltip("<= 0이면 즉시 끄기 (알파 0으로 스냅)")]
    public float duration = 0f;

    public Ease ease = Ease.Linear;

    [Tooltip("true면 페이드가 끝날 때까지 Step 진행을 멈춤")]
    public bool wait = true;

    [Header("Interaction")]
    [Tooltip("true면 숨긴 대상의 입력을 완전히 차단(interactable/blocksRaycasts=false)")]
    public bool disableInteraction = true;
}

public sealed class HideTargetsCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;
    private readonly bool  _wait;

    private readonly DialogueTargetMask _targetsMask;
    private readonly float _duration;
    private readonly Ease  _ease;
    private readonly bool  _disableInteraction;

    private IDialogueWidgetAccess.WidgetRefs _refs;

    // RectTransform 기준으로 페이드하되, 중복 방지를 위해 HashSet/리스트
    private readonly List<RectTransform> _targets = new();

    private bool _resolveAttempted;

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public HideTargetsCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        bool waitForCompletion,
        DialogueTargetMask targetsMask,
        float duration,
        Ease ease,
        bool disableInteraction)
    {
        _widgets            = widgets;
        _screenId           = screenId;
        _widgetRoleKey      = widgetRoleKey;
        _wait               = waitForCompletion;

        _targetsMask        = targetsMask;
        _duration           = Mathf.Max(0f, duration);
        _ease               = ease;
        _disableInteraction = disableInteraction;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        CollectTargetRects(_refs, _targetsMask, _targets);
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

        CollectTargetRects(_refs, _targetsMask, _targets);
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

        Debug.LogWarning($"[HideTargetsCommand] CanvasGroup missing. Added automatically: {rect.name}", rect);
        return rect.gameObject.AddComponent<CanvasGroup>();
    }

    /// <summary>
    /// DialogueTargetMask → 실제 RectTransform 리스트로 변환.
    /// 매핑 테이블(DialogueTargetMaskMap)은
    /// "덩어리 루트"에 해당하는 DialogueWidgetTarget들만 가리킨다는 전제.
    /// </summary>
    private static void CollectTargetRects(
        IDialogueWidgetAccess.WidgetRefs refs,
        DialogueTargetMask mask,
        List<RectTransform> outList)
    {
        outList.Clear();

        if (refs == null || mask == DialogueTargetMask.None)
            return;

        // 1) 마스크 → DialogueWidgetTarget 리스트
        var widgetTargets = DialogueTargetMaskMap.ResolveTargets(mask);
        if (widgetTargets == null || widgetTargets.Count == 0)
            return;

        // 2) 각 target → RectTransform (WidgetRefsExtensions.GetRect 사용)
        var set = new HashSet<RectTransform>();

        foreach (var wt in widgetTargets)
        {
            RectTransform rect = refs.GetRect(wt);
            if (rect == null)
                continue;

            if (set.Add(rect))
                outList.Add(rect);
        }
    }
}