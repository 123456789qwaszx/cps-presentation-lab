using System;
using DG.Tweening;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using RectTransform = UnityEngine.RectTransform;

[Serializable]
[CommandMenuHint(
    "Scene",
    "Show Layers (Dialogue)",
    Sets = new[]
    {
        CpsCommandMenuSets.VnMainEnterFirstLine,
        "Custom/Text/LineBasic",
        "Custom/Text/LineType"
    }
)]
public sealed class ShowDialogueLayersCommandSpec : CommandSpecBase
{
    [Header("Layers")] public DialogueLayerMask layers =
        DialogueLayerMask.Background1 | DialogueLayerMask.DialogueBox;

    [Header("Fade")] [Tooltip("<= 0이면 즉시 켜기 (알파 1로 스냅)")]
    public float duration = 0.25f;

    [Tooltip("true면 페이드가 끝날 때까지 Step 진행을 멈춤")]
    public bool wait = false;

    [Header("Interaction")] [Tooltip("대화박스 / 선택지의 상호작용을 자동으로 켤지 여부")]
    public bool enableInteraction = true;
}


public sealed class CpsShowDialogueLayersCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;

    private readonly DialogueLayerMask _layers;
    private readonly float _duration;
    private readonly bool _wait;
    private readonly bool _enableInteraction;

    private bool _resolved;
    private IDialogueWidgetAccess.WidgetRefs _refs;

    public CpsShowDialogueLayersCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        DialogueLayerMask layers,
        float duration,
        bool waitForCompletion,
        bool enableInteraction)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetRoleKey = widgetRoleKey;
        _layers = layers;
        _duration = Mathf.Max(0f, duration);
        _wait = waitForCompletion;
        _enableInteraction = enableInteraction;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        var tweens = new List<Tween>();

        // ---- helpers ----
        void ShowGraphic(Graphic g, bool interactive)
        {
            if (g == null) return;

            g.DOKill(false);

            // alpha 0으로 강제 초기화해놓고 시작하는 쪽이 안전
            var c = g.color;
            c.a = 0f;
            g.color = c;

            if (_duration <= 0f)
            {
                c.a = 1f;
                g.color = c;

                if (_enableInteraction && interactive)
                    g.raycastTarget = true;

                return;
            }

            Tween t = g
                .DOFade(1f, _duration)
                .SetEase(Ease.Linear)
                .SetUpdate(true);

            if (_enableInteraction && interactive)
            {
                t.OnComplete(() => g.raycastTarget = true);
            }

            t.BindToStep(scope);
            tweens.Add(t);
        }

        void ShowRoot(RectTransform rt, bool interactive)
        {
            if (rt == null) return;

            var cg = rt.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = rt.gameObject.AddComponent<CanvasGroup>();

            cg.DOKill(false);

            cg.alpha = 0f;

            if (_duration <= 0f)
            {
                cg.alpha = 1f;

                if (_enableInteraction && interactive)
                {
                    cg.interactable = true;
                    cg.blocksRaycasts = true;
                }

                return;
            }

            Tween t = cg
                .DOFade(1f, _duration)
                .SetEase(Ease.Linear)
                .SetUpdate(true);

            if (_enableInteraction && interactive)
            {
                t.OnComplete(() =>
                {
                    cg.interactable = true;
                    cg.blocksRaycasts = true;
                });
            }

            t.BindToStep(scope);
            tweens.Add(t);
        }

        // ------------------------------------
        // 1) Backgrounds
        // ------------------------------------
        if (_layers.HasFlag(DialogueLayerMask.Background1))
            ShowGraphic(_refs.BackgroundImage, interactive: false);

        if (_layers.HasFlag(DialogueLayerMask.Background2))
            ShowGraphic(_refs.BackgroundImage2, interactive: false);
        // 나중에 Background3 쓰면 여기 추가

        // ------------------------------------
        // 2) Portrait Roots
        // ------------------------------------
        if (_layers.HasFlag(DialogueLayerMask.MainPortrait))
            ShowRoot(_refs.MainStandingPortraitRoot, interactive: false);

        if (_layers.HasFlag(DialogueLayerMask.SubLeftPortrait))
            ShowRoot(_refs.SubLeftStandingPortraitRoot, interactive: false);

        if (_layers.HasFlag(DialogueLayerMask.SubRightPortrait))
            ShowRoot(_refs.SubRightStandingPortraitRoot, interactive: false);

        // ------------------------------------
        // 3) Dialogue Box / Choice Panel
        // ------------------------------------
        if (_layers.HasFlag(DialogueLayerMask.DialogueBox))
            ShowRoot(_refs.DialogueBoxRoot, interactive: true);

        if (_layers.HasFlag(DialogueLayerMask.ChoicePanel))
            ShowRoot(_refs.ChoicePanelRoot, interactive: true);

        // ------------------------------------
        // 대기 로직
        // ------------------------------------
        if (_duration <= 0f || tweens.Count == 0)
            yield break;

        if (!_wait)
            yield break;

        int remaining = tweens.Count;
        foreach (var t in tweens)
        {
            t.OnComplete(() => remaining--);
        }

        while (remaining > 0)
            yield return null;
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        void InstantOnGraphic(Graphic g, bool interactive)
        {
            if (g == null) return;
            g.DOKill(false);

            var c = g.color;
            c.a = 1f;
            g.color = c;

            if (_enableInteraction && interactive)
                g.raycastTarget = true;
        }

        void InstantOnRoot(RectTransform rt, bool interactive)
        {
            if (rt == null) return;

            var cg = rt.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = rt.gameObject.AddComponent<CanvasGroup>();

            cg.DOKill(false);
            cg.alpha = 1f;

            if (_enableInteraction && interactive)
            {
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }

        if (_layers.HasFlag(DialogueLayerMask.Background1))
            InstantOnGraphic(_refs.BackgroundImage, interactive: false);

        if (_layers.HasFlag(DialogueLayerMask.Background2))
            InstantOnGraphic(_refs.BackgroundImage2, interactive: false);
        // Background3 필요하면 여기도 추가

        if (_layers.HasFlag(DialogueLayerMask.MainPortrait))
            InstantOnRoot(_refs.MainStandingPortraitRoot, interactive: false);

        if (_layers.HasFlag(DialogueLayerMask.SubLeftPortrait))
            InstantOnRoot(_refs.SubLeftStandingPortraitRoot, interactive: false);

        if (_layers.HasFlag(DialogueLayerMask.SubRightPortrait))
            InstantOnRoot(_refs.SubRightStandingPortraitRoot, interactive: false);

        if (_layers.HasFlag(DialogueLayerMask.DialogueBox))
            InstantOnRoot(_refs.DialogueBoxRoot, interactive: true);

        if (_layers.HasFlag(DialogueLayerMask.ChoicePanel))
            InstantOnRoot(_refs.ChoicePanelRoot, interactive: true);
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _refs != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null)
            return false;

        return true;
    }
}