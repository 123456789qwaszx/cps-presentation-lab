using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[Serializable]
[CommandMenuHint(
    "Scene",
    "Fade Root Layers",
    SetOrder = 25,
    Order    = 15)]
public sealed class FadeLayersCommandSpec : CommandSpecBase
{
    [Header("Layers")]
    [Tooltip("페이드할 Root 레이어들을 선택합니다. (Background / Portrait / DialogueBox / ChoicePanel 등)")]
    public DialogueLayerMask layers =
        DialogueLayerMask.None;

    [Header("Fade")]
    [Range(0f, 1f)]
    [Tooltip("목표 알파 값 (0=완전 투명, 1=완전 불투명).")]
    public float toAlpha = 1f;

    [Tooltip("0 이상이면 이 값에서부터 페이드 시작, 음수면 현재 alpha에서 시작합니다.")]
    public float fromAlpha = 0.1f;

    [Tooltip("페이드 시간(초). 0 이하이면 즉시 toAlpha로 스냅합니다.")]
    public float duration = 0.8f;

    public Ease ease = Ease.OutCubic;

    public bool wait = false;

    [Header("Options")]
    [Tooltip("대상 Rect에 CanvasGroup이 없으면 자동으로 추가.")]
    public bool addIfMissing = true;
}

public sealed class FadeLayersCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string                _screenId;
    private readonly string                _widgetRoleKey;

    private readonly DialogueLayerMask     _layers;
    private readonly float                 _toAlpha;
    private readonly float                 _fromAlpha;
    private readonly float                 _duration;
    private readonly Ease                  _ease;
    private readonly bool                  _wait;
    private readonly bool                  _addIfMissing;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private bool                              _resolveAttempted;

    private readonly List<RectTransform> _targets = new();

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public FadeLayersCommand(
        IDialogueWidgetAccess widgets,
        string                screenId,
        string                widgetRoleKey,
        bool                  waitForCompletion,

        DialogueLayerMask     layers,
        float                 toAlpha,
        float                 fromAlpha,
        float                 duration,
        Ease                  ease,
        bool                  addIfMissing)
    {
        _widgets       = widgets;
        _screenId      = screenId;
        _widgetRoleKey = widgetRoleKey;

        _layers       = layers;
        _toAlpha      = Mathf.Clamp01(toAlpha);
        _fromAlpha    = fromAlpha;
        _duration     = duration;
        _ease         = ease;
        _wait         = waitForCompletion;
        _addIfMissing = addIfMissing;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        DialogueLayerRoots.Collect(_refs, _layers, _targets);
        if (_targets.Count == 0)
            yield break;

        float dur = _duration;

        if (dur <= 0f)
        {
            SnapAlpha(_targets, _toAlpha, _fromAlpha, _addIfMissing);
            yield break;
        }

        int remaining = 0;

        for (int i = 0; i < _targets.Count; i++)
        {
            RectTransform rect = _targets[i];
            if (rect == null)
                continue;

            CanvasGroup group = GetOrAddCanvasGroup(rect, _addIfMissing);
            if (group == null)
                continue;

            group.DOKill(false);

            if (_fromAlpha >= 0f)
                group.alpha = Mathf.Clamp01(_fromAlpha);

            remaining++;

            Tween tween = group
                .DOFade(_toAlpha, dur)
                .SetEase(_ease)
                .SetUpdate(true);

            tween.OnComplete(() =>
                {
                    group.alpha = _toAlpha;
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

        DialogueLayerRoots.Collect(_refs, _layers, _targets);
        if (_targets.Count == 0)
            return;

        SnapAlpha(_targets, _toAlpha, _fromAlpha, _addIfMissing);
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

        return true;
    }

    private static CanvasGroup GetOrAddCanvasGroup(RectTransform rect, bool addIfMissing)
    {
        if (rect == null)
            return null;

        CanvasGroup group = rect.GetComponent<CanvasGroup>();
        if (group != null)
            return group;

        if (!addIfMissing)
            return null;

        Debug.LogWarning($"[CanvasFadeCommand] CanvasGroup missing. Added automatically: {rect.name}", rect);

        return rect.gameObject.AddComponent<CanvasGroup>();
    }

    private static void SnapAlpha(
        List<RectTransform> targets,
        float               toAlpha,
        float               fromAlpha,
        bool                addIfMissing)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            RectTransform rect = targets[i];
            if (rect == null)
                continue;

            CanvasGroup group = GetOrAddCanvasGroup(rect, addIfMissing);
            if (group == null)
                continue;

            group.DOKill(false);

            group.alpha = toAlpha;
        }
    }
}