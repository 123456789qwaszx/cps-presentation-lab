using System;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum DialogueLayerMask
{
    None = 0,

    // ---- Primitive layers (single bits) ----
    // BG1: 기본 배경(씬의 메인)
    // BG2: 보조 배경(이펙트, 페이드용, 컷 전환용 등)
    Background0Root     = 1 << 0,
    Background1Root     = 1 << 1,
    // 나중에 필요하면: Background3 = 1 << 2,

    MainPortraitRoot    = 1 << 3,  // 중앙/주인공
    SubLeftPortraitRoot = 1 << 4,  // 좌측 서브
    SubRightPortraitRoot= 1 << 5,  // 우측 서브

    DialogueBoxRoot     = 1 << 6,
    ChoicePanelRoot     = 1 << 7,
    TogglePanelRoot     = 1 << 8,

    // ---- Group masks (단순 그룹) ----
    AllBackgrounds  = Background0Root | Background1Root,
    AllPortraits    = MainPortraitRoot | SubLeftPortraitRoot | SubRightPortraitRoot,
    AllUI           = DialogueBoxRoot | ChoicePanelRoot | TogglePanelRoot,
    //
    // // ---- VN Presets (실제 연출에서 자주 쓰는 상태들) ----
    // // 1) 일반 스토리: 배경 1 + 대화창
    // Story_Default   = Background0Root | DialogueBoxRoot,
    //
    // // 2) 주로 나오는 패턴들
    // Story_Solo      = Background0Root | MainPortraitRoot | DialogueBoxRoot,                           // 1인 화자
    // Story_Duo       = Background0Root | MainPortraitRoot | SubLeftPortraitRoot | DialogueBoxRoot,         // 2인 (메인 + 서브)
    // Story_Trio      = Background0Root | MainPortraitRoot | SubLeftPortraitRoot | SubRightPortraitRoot | DialogueBoxRoot, // 3인
    //
    // // 3) 선택지: 보통 대화창 대신 선택지만 뜸
    // Choice_Default  = Background0Root | ChoicePanelRoot,
    //
    // // (원하면 BG2를 섞은 변형도 나중에 추가 가능)
    // // Story_Solo_WithSubBg  = Background1 | Background2 | MainPortrait | DialogueBox;
    // // Choice_WithSubBg      = Background1 | Background2 | ChoicePanel;
    //
    // ---- Everything ----
    All             = AllBackgrounds | AllPortraits | AllUI
}

public static class DialogueLayerRoots
{
    private static readonly (DialogueLayerMask mask, Func<IDialogueWidgetAccess.WidgetRefs, RectTransform> get)[] Map =
    {
        (DialogueLayerMask.Background0Root,     r => r.BackgroundRoot00),
        (DialogueLayerMask.Background1Root,     r => r.BackgroundRoot01),

        (DialogueLayerMask.MainPortraitRoot,    r => r.MainStandingPortraitRoot),
        (DialogueLayerMask.SubLeftPortraitRoot, r => r.SubLeftStandingPortraitRoot),
        (DialogueLayerMask.SubRightPortraitRoot,r => r.SubRightStandingPortraitRoot),

        (DialogueLayerMask.DialogueBoxRoot,     r => r.DialogueBoxRoot),
        (DialogueLayerMask.ChoicePanelRoot,     r => r.ChoicePanel00Root),
        (DialogueLayerMask.TogglePanelRoot,     r => r.TogglePanel00Root),
    };

    public static void Collect(
        IDialogueWidgetAccess.WidgetRefs refs,
        DialogueLayerMask layerMask,
        List<RectTransform> outList)
    {
        outList.Clear();
        if (refs == null || layerMask == 0)
            return;

        // HasFlag 대신 bit 연산(빠르고 박싱 없음)
        for (int i = 0; i < Map.Length; i++)
        {
            var (flag, getter) = Map[i];
            if ((layerMask & flag) == 0) continue;

            var rt = getter(refs);
            if (rt != null) outList.Add(rt);
        }
    }
}