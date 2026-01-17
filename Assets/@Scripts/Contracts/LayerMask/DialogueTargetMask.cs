using System;

[Flags]
public enum DialogueTargetMask
{
    None = 0,

    // ---------------------------
    // DialogueBox
    // ---------------------------
    DialogueBox00Root       = 1 << 0,
    SpeakerNameBox00Root    = 1 << 1,
    ProtagonistCutin00Root  = 1 << 2,

    // ---------------------------
    // Standing 00 (Main)
    // ---------------------------
    Standing00PortraitRoot          = 1 << 3,
    Standing00PortraitOverlaysRoot  = 1 << 4,
    Standing00Emoji00Root           = 1 << 5,
    Standing00Emoji01Root           = 1 << 6,
    Standing00Emoji02Root           = 1 << 7,
    Standing00Emoji03Root           = 1 << 8,

    // ---------------------------
    // Standing 01
    // ---------------------------
    Standing01PortraitRoot          = 1 << 9,
    Standing01PortraitOverlaysRoot  = 1 << 10,
    Standing01Emoji00Root           = 1 << 11,
    Standing01Emoji01Root           = 1 << 12,
    Standing01Emoji02Root           = 1 << 13,
    Standing01Emoji03Root           = 1 << 14,

    // ---------------------------
    // Standing 02
    // ---------------------------
    Standing02PortraitRoot          = 1 << 15,
    Standing02PortraitOverlaysRoot  = 1 << 16,
    Standing02Emoji00Root           = 1 << 17,
    Standing02Emoji01Root           = 1 << 18,
    Standing02Emoji02Root           = 1 << 19,
    Standing02Emoji03Root           = 1 << 20,

    // ---------------------------
    // Choice UI
    // ---------------------------
    ChoicePanel00Root        = 1 << 21,
    ChoiceButton00Root       = 1 << 22,
    ChoiceButton01Root       = 1 << 23,
    ChoiceButton02Root       = 1 << 24,

    // ======================================================

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

    // ---- Global standing groups ----
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

    // ---- Choice groups ----
    AllChoiceButtons =
        ChoiceButton00Root |
        ChoiceButton01Root |
        ChoiceButton02Root,

    AllChoices =
        ChoicePanel00Root |
        AllChoiceButtons,

    // ---- Core UI groups ----
    AllDialogueUI =
        DialogueBox00Root |
        SpeakerNameBox00Root,

    StoryLineDefault =
        AllDialogueUI |
        AllPortraitRoots,

    // ---- Everything ----
    All =
        DialogueBox00Root |
        SpeakerNameBox00Root |
        ProtagonistCutin00Root |
        AllStandings |
        AllChoices
}