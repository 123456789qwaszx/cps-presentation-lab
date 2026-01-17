using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[Serializable]
[CommandMenuHint(
    "Set Rect",
    "Set Root Stage (AnchorPreset)",
    Order = 20)]
public sealed class SetRootStageCommandSpec : CommandSpecBase
{
    [Header("Layers (Roots only)")]
    public DialogueLayerMask layers = DialogueLayerMask.None;

    [Header("Anchor Preset (9-slice)")]
    [Tooltip("12시/3시/6시/9시 + 대각선/센터 프리셋. 유니티 RectTransform의 그리드 느낌으로 사용합니다.")]
    public RectAnchorPreset9 anchorPreset = RectAnchorPreset9.Center;

    [Header("Offset")]
    [Tooltip("프리셋 기준에서 추가로 줄 오프셋(픽셀).")]
    public Vector2 offset = Vector2.zero;

    [Header("Options")]
    [Tooltip("체크하면 RectTransform 관련 트윈을 끊고 적용합니다.")]
    public bool killTween = true;
}

public sealed class SetRootStageCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;

    private readonly DialogueLayerMask _layers;
    private readonly RectAnchorPreset9 _anchorPreset;
    private readonly Vector2 _offset;
    private readonly bool _killTween;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private readonly List<RectTransform> _targets = new();
    private bool _resolveAttempted;

    public override bool WaitForCompletion => false;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public SetRootStageCommand(
        IDialogueWidgetAccess widgets,
        string                screenId,
        string                widgetRoleKey,

        DialogueLayerMask     layers,
        RectAnchorPreset9     anchorPreset,
        Vector2               offset,
        bool                  killTween)
    {
        _widgets       = widgets;
        _screenId      = screenId;
        _widgetRoleKey = widgetRoleKey;

        _layers        = layers;
        _anchorPreset  = anchorPreset;
        _offset        = offset;
        _killTween     = killTween;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        Apply();
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        Apply();
    }

    private void Apply()
    {
        if (_targets.Count == 0)
            return;

        for (int i = 0; i < _targets.Count; i++)
        {
            RectTransform rect = _targets[i];
            if (rect == null)
                continue;

            if (_killTween)
                rect.DOKill(false);

            _anchorPreset.ApplyTo(rect);

            rect.anchoredPosition = _offset;
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

        CollectRootTargets(_refs, _layers, _targets);
        return _targets.Count > 0;
    }

    private static void CollectRootTargets(IDialogueWidgetAccess.WidgetRefs refs, DialogueLayerMask layerMask, List<RectTransform> outList)
    {
        outList.Clear();
        if (refs == null) return;

        if (layerMask.HasFlag(DialogueLayerMask.Background0Root) && refs.BackgroundRoot00 != null)
            outList.Add(refs.BackgroundRoot00);
        if (layerMask.HasFlag(DialogueLayerMask.Background1Root) && refs.BackgroundRoot01 != null)
            outList.Add(refs.BackgroundRoot01);

        if (layerMask.HasFlag(DialogueLayerMask.MainPortraitRoot)     && refs.MainStandingPortraitRoot != null)
            outList.Add(refs.MainStandingPortraitRoot);
        if (layerMask.HasFlag(DialogueLayerMask.SubLeftPortraitRoot)  && refs.SubLeftStandingPortraitRoot != null)
            outList.Add(refs.SubLeftStandingPortraitRoot);
        if (layerMask.HasFlag(DialogueLayerMask.SubRightPortraitRoot) && refs.SubRightStandingPortraitRoot != null)
            outList.Add(refs.SubRightStandingPortraitRoot);

        if (layerMask.HasFlag(DialogueLayerMask.DialogueBoxRoot) && refs.DialogueBoxRoot != null)
            outList.Add(refs.DialogueBoxRoot);
        if (layerMask.HasFlag(DialogueLayerMask.ChoicePanelRoot) && refs.ChoicePanelRoot != null)
            outList.Add(refs.ChoicePanelRoot);
    }
}