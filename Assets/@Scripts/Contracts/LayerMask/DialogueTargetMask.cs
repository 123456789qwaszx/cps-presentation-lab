using System;

[Flags]
public enum DialogueTargetMask
{
    None = 0,

    // ---------------------------
    // DialogueBox
    // ---------------------------
    DialogueBox00Root              = 1 << 0,
    SpeakerNameBox00Root           = 1 << 1,

    // ---------------------------
    // Protagonist Cutin 00
    // ---------------------------
    ProtagonistCutin00Root         = 1 << 2,
    ProtagonistCutin00Emoji00Root  = 1 << 3,
    ProtagonistCutin00Emoji01Root  = 1 << 4,
    ProtagonistCutin00Emoji02Root  = 1 << 5,

    // ---------------------------
    // Standing 00 (Main)
    // ---------------------------
    Standing00PortraitRoot         = 1 << 6,
    Standing00PortraitOverlaysRoot = 1 << 7,
    Standing00Emoji00Root          = 1 << 8,
    Standing00Emoji01Root          = 1 << 9,
    Standing00Emoji02Root          = 1 << 10,
    Standing00Emoji03Root          = 1 << 11,

    // ---------------------------
    // Standing 01
    // ---------------------------
    Standing01PortraitRoot         = 1 << 12,
    Standing01PortraitOverlaysRoot = 1 << 13,
    Standing01Emoji00Root          = 1 << 14,
    Standing01Emoji01Root          = 1 << 15,
    Standing01Emoji02Root          = 1 << 16,
    Standing01Emoji03Root          = 1 << 17,

    // ---------------------------
    // Standing 02
    // ---------------------------
    Standing02PortraitRoot         = 1 << 18,
    Standing02PortraitOverlaysRoot = 1 << 19,
    Standing02Emoji00Root          = 1 << 20,
    Standing02Emoji01Root          = 1 << 21,
    Standing02Emoji02Root          = 1 << 22,
    Standing02Emoji03Root          = 1 << 23,

    // ---------------------------
    // Choice UI
    // ---------------------------
    ChoicePanel00Root              = 1 << 24,
    ChoiceButton00Root             = 1 << 25,
    ChoiceButton01Root             = 1 << 26,
    ChoiceButton02Root             = 1 << 27,

    // ---------------------------
    // Background UI
    // ---------------------------
    Background00Root               = 1 << 28,
    Background01Root               = 1 << 29,

    // ---------------------------
    // Advance Indicator
    // ---------------------------
    AdvanceIndicator00Root         = 1 << 30,

    // ======================================================
    // Practical groups
    // ======================================================

    // ---- Protagonist cutin groups ----
    ProtagonistCutin00Emojis =
        ProtagonistCutin00Emoji00Root |
        ProtagonistCutin00Emoji01Root |
        ProtagonistCutin00Emoji02Root,

    ProtagonistCutin00All =
        ProtagonistCutin00Root |
        ProtagonistCutin00Emojis,

    // ---- Background groups ----
    AllBackgrounds =
        Background00Root |
        Background01Root,

    // ---- Standing per slot ----
    Standing00Emojis =
        Standing00Emoji00Root |
        Standing00Emoji01Root |
        Standing00Emoji02Root |
        Standing00Emoji03Root,

    Standing01Emojis =
        Standing01Emoji00Root |
        Standing01Emoji01Root |
        Standing01Emoji02Root |
        Standing01Emoji03Root,

    Standing02Emojis =
        Standing02Emoji00Root |
        Standing02Emoji01Root |
        Standing02Emoji02Root |
        Standing02Emoji03Root,

    Standing00All =
        Standing00PortraitRoot |
        Standing00PortraitOverlaysRoot |
        Standing00Emojis,

    Standing01All =
        Standing01PortraitRoot |
        Standing01PortraitOverlaysRoot |
        Standing01Emojis,

    Standing02All =
        Standing02PortraitRoot |
        Standing02PortraitOverlaysRoot |
        Standing02Emojis,

    AllPortraitRoots =
        Standing00PortraitRoot |
        Standing01PortraitRoot |
        Standing02PortraitRoot,

    AllPortraitOverlays =
        Standing00PortraitOverlaysRoot |
        Standing01PortraitOverlaysRoot |
        Standing02PortraitOverlaysRoot,

    AllEmojis =
        Standing00Emojis |
        Standing01Emojis |
        Standing02Emojis,

    AllStandings =
        Standing00All |
        Standing01All |
        Standing02All,

    AllChoiceButtons =
        ChoiceButton00Root |
        ChoiceButton01Root |
        ChoiceButton02Root,

    AllChoices =
        ChoicePanel00Root |
        AllChoiceButtons,

    // ---- Core UI groups ----
    // "다음 진행 표시"는 보통 Dialogue UI에 포함시키는 게 실전에서 편함
    AllDialogueUI =
        DialogueBox00Root |
        SpeakerNameBox00Root |
        AdvanceIndicator00Root,

    // 기본 스토리 라인(배경 포함 여부는 취향인데, 일단 실용적으로 포함)
    StoryLineDefault =
        AllDialogueUI |
        AllPortraitRoots |
        AllBackgrounds,

    // ---- Everything ----
    All =
        AllDialogueUI |
        ProtagonistCutin00All |
        AllStandings |
        AllChoices |
        AllBackgrounds
}