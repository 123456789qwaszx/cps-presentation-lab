using System;
using System.Collections.Generic;
using System.Linq;

public static class DialogueTargetMaskMap
{
    /// <summary>
    /// Primitive 플래그 ↔ 실제 DialogueWidgetTarget[] 매핑 테이블.
    /// Group/프리셋(StoryLineDefault, AllPortraitImages, AllChoices, All 등)은
    /// 여기 넣지 않는다.
    /// </summary>
    private static readonly (DialogueTargetMask mask, DialogueWidgetTarget[] targets)[] Table =
{
    // ────────────── 기본 대사 UI ──────────────
    (
        DialogueTargetMask.DialogueBox,
        new[]
        {
            DialogueWidgetTarget.DialogueBoxRoot,
        }
    ),
    (
        DialogueTargetMask.NameBox,
        new[]
        {
            // SpeakerNameBox(GameObject)에 CanvasGroup 하나만 붙이면,
            // 그 아래 Text까지 같이 페이드됨
            DialogueWidgetTarget.SpeakerNameBox,
        }
    ),

    // ────────────── 초상화 ──────────────
    (
        DialogueTargetMask.MainPortraitImage,
        new[]
        {
            DialogueWidgetTarget.MainStandingPortraitVisual,
        }
    ),
    (
        DialogueTargetMask.SubLeftPortraitImage,
        new[]
        {
            DialogueWidgetTarget.SubLeftStandingPortraitVisual,
        }
    ),
    (
        DialogueTargetMask.SubRightPortraitImage,
        new[]
        {
            DialogueWidgetTarget.SubRightStandingPortraitVisual,
        }
    ),

    // ────────────── 이모티콘 ──────────────
    (
        DialogueTargetMask.MainEmoji,
        new[]
        {
            DialogueWidgetTarget.MainStandingPortraitEmojiAnchor,
        }
    ),
    (
        DialogueTargetMask.SubLeftEmoji,
        new[]
        {
            DialogueWidgetTarget.SubLeftStandingPortraitEmojiAnchor,
        }
    ),
    (
        DialogueTargetMask.SubRightEmoji,
        new[]
        {
            DialogueWidgetTarget.SubRightStandingPortraitEmojiAnchor,
        }
    ),

    // ────────────── 컷인 ──────────────
    (
        DialogueTargetMask.ProtagonistCutin,
        new[]
        {
            DialogueWidgetTarget.ProtagonistCutinImage,
        }
    ),

    // ────────────── 선택지 ──────────────
    (
        DialogueTargetMask.Choice0,
        new[]
        {
            DialogueWidgetTarget.ChoiceButton0Root,
        }
    ),
    (
        DialogueTargetMask.Choice1,
        new[]
        {
            DialogueWidgetTarget.ChoiceButton1Root,
        }
    ),
    (
        DialogueTargetMask.Choice2,
        new[]
        {
            DialogueWidgetTarget.ChoiceButton2Root,
        }
    ),
    (
        DialogueTargetMask.ChoicePanel,
        new[]
        {
            DialogueWidgetTarget.ChoicePanelRoot,
        }
    ),
};

    /// <summary>
    /// 복합 DialogueTargetMask를 실제 DialogueWidgetTarget 리스트로 풀어준다.
    /// Group/프리셋(StoryLineDefault, AllPortraitImages, AllChoices, All 등)도
    /// Primitive들의 OR 결과로 자동 해석된다.
    /// </summary>
    public static IReadOnlyList<DialogueWidgetTarget> ResolveTargets(DialogueTargetMask mask)
    {
        if (mask == DialogueTargetMask.None)
            return Array.Empty<DialogueWidgetTarget>();

        var set = new HashSet<DialogueWidgetTarget>();

        foreach (var (flag, targets) in Table)
        {
            if (flag == DialogueTargetMask.None)
                continue;

            if (mask.HasFlag(flag))
            {
                foreach (var t in targets)
                    set.Add(t);
            }
        }

        return set.ToList();
    }
}
