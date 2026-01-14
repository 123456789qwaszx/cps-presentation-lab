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
    "Hide All (Dialogue Layer)",
    Sets  = new[] { CpsCommandMenuSets.VnAllOff },
    Order = 0
)]
public sealed class HideAllDialogueLayerCommandSpec : CommandSpecBase
{
    [Header("Fade")]
    [Tooltip("<= 0이면 즉시 끄기 (알파 0으로 스냅)")]
    public float duration = 0.25f;

    [Tooltip("true면 페이드가 끝날 때까지 Step 진행을 멈춤")]
    public bool wait = true;

    [Header("Interaction")]
    [Tooltip("대화박스 / 선택지의 상호작용까지 함께 끌지 여부")]
    public bool disableInteraction = true;
}


public sealed class CpsHideAllDialogueLayerCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;

    private readonly float _duration;
    private readonly bool  _wait;
    private readonly bool  _disableInteraction;

    private bool _resolved;
    private IDialogueWidgetAccess.WidgetRefs _refs;

    public CpsHideAllDialogueLayerCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        float duration,
        bool waitForCompletion,
        bool disableInteraction)
    {
        _widgets           = widgets;
        _screenId          = screenId;
        _widgetRoleKey     = widgetRoleKey;
        _duration          = Mathf.Max(0f, duration);
        _wait              = waitForCompletion;
        _disableInteraction = disableInteraction;
    }

    public override bool WaitForCompletion => _wait;

    // 스킵 시: 바로 최종 상태로 스냅
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        // 모인 Tween 리스트 (wait=true면 여기 전부 끝날 때까지 대기)
        var tweens = new List<Tween>();

        // ---- helper: Graphic ----
        void FadeGraphic(Graphic g)
        {
            if (g == null) return;

            g.DOKill(false);

            if (_duration <= 0f)
            {
                var c = g.color;
                c.a   = 0f;
                g.color = c;

                if (_disableInteraction)
                    g.raycastTarget = false;
                return;
            }

            Tween t = g
                .DOFade(0f, _duration)
                .SetEase(Ease.Linear)
                .SetUpdate(true); // timeScale 무시(대화 연출 쪽은 이쪽이 더 자연스러울 때가 많음)

            if (_disableInteraction)
            {
                t.OnComplete(() => g.raycastTarget = false);
            }

            t.BindToStep(scope);
            tweens.Add(t);
        }

        // ---- helper: CanvasGroup root (RectTransform 기준) ----
        void FadeRoot(RectTransform rt, bool interactive)
        {
            if (rt == null) return;

            var cg = rt.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = rt.gameObject.AddComponent<CanvasGroup>();

            cg.DOKill(false);

            if (_duration <= 0f)
            {
                cg.alpha = 0f;
                if (_disableInteraction && interactive)
                {
                    cg.interactable   = false;
                    cg.blocksRaycasts = false;
                }
                return;
            }

            Tween t = cg
                .DOFade(0f, _duration)
                .SetEase(Ease.Linear)
                .SetUpdate(true);

            if (_disableInteraction && interactive)
            {
                t.OnComplete(() =>
                {
                    cg.interactable   = false;
                    cg.blocksRaycasts = false;
                });
            }

            t.BindToStep(scope);
            tweens.Add(t);
        }

        // ------------------------------------
        // 1) Background (2장)
        // ------------------------------------
        FadeGraphic(_refs.BackgroundImage0);
        FadeGraphic(_refs.BackgroundImage1);
        // 필요하면 나중에 BackgroundImage3 추가:
        // FadeGraphic(_refs.BackgroundImage3);

        // ------------------------------------
        // 2) Portrait Roots (Main / SubLeft / SubRight)
        //    - 여기서는 인터랙션 의미 없으니 interactive: false
        // ------------------------------------
        FadeRoot(_refs.MainStandingPortraitRoot,   interactive: false);
        FadeRoot(_refs.SubLeftStandingPortraitRoot,  interactive: false);
        FadeRoot(_refs.SubRightStandingPortraitRoot, interactive: false);

        // ------------------------------------
        // 3) Dialogue Box / Choice Panel (인터랙티브)
        // ------------------------------------
        FadeRoot(_refs.DialogueBoxRoot,  interactive: true);
        FadeRoot(_refs.ChoicePanelRoot,  interactive: true);

        // duration <= 0f 이면 이미 스냅 완료
        if (_duration <= 0f || tweens.Count == 0)
            yield break;

        if (!_wait)
            yield break;

        // 모든 Tween 완료까지 기다리기
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

        // ---- helper: Graphic ----
        void InstantOffGraphic(Graphic g)
        {
            if (g == null) return;
            g.DOKill(false);

            var c = g.color;
            c.a   = 0f;
            g.color = c;

            if (_disableInteraction)
                g.raycastTarget = false;
        }

        // ---- helper: CanvasGroup root ----
        void InstantOffRoot(RectTransform rt, bool interactive)
        {
            if (rt == null) return;

            var cg = rt.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = rt.gameObject.AddComponent<CanvasGroup>();

            cg.DOKill(false);
            cg.alpha = 0f;

            if (_disableInteraction && interactive)
            {
                cg.interactable   = false;
                cg.blocksRaycasts = false;
            }
        }

        // Background
        InstantOffGraphic(_refs.BackgroundImage0);
        InstantOffGraphic(_refs.BackgroundImage1);
        // InstantOffGraphic(_refs.BackgroundImage3);

        // Portrait Roots
        InstantOffRoot(_refs.MainStandingPortraitRoot,   interactive: false);
        InstantOffRoot(_refs.SubLeftStandingPortraitRoot,  interactive: false);
        InstantOffRoot(_refs.SubRightStandingPortraitRoot, interactive: false);

        // Dialogue Box / Choice Panel
        InstantOffRoot(_refs.DialogueBoxRoot,  interactive: true);
        InstantOffRoot(_refs.ChoicePanelRoot,  interactive: true);
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
