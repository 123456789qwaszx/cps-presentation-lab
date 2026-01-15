using System;

[Flags]
public enum DialogueTargetMask
{
    None = 0,

    /// <summary>대사 박스(말풍선) 프레임/바디</summary>
    DialogueBox     = 1 << 1,

    /// <summary>이름 박스 + 이름 텍스트</summary>
    NameBox         = 1 << 2,

    /// <summary>주인공 컷인 이미지</summary>
    ProtagonistCutin = 1 << 3,

    // ────────────── 초상화 이미지 단위 ──────────────
    /// <summary>메인(중앙) 캐릭터 초상화 이미지</summary>
    MainPortraitImage    = 1 << 4,

    /// <summary>좌측 서브 캐릭터 초상화 이미지</summary>
    SubLeftPortraitImage = 1 << 5,

    /// <summary>우측 서브 캐릭터 초상화 이미지</summary>
    SubRightPortraitImage = 1 << 6,

    // ────────────── 이모티콘(감정 아이콘) ──────────────
    /// <summary>메인 초상화 위 이모지</summary>
    MainEmoji        = 1 << 7,

    /// <summary>좌측 서브 초상화 위 이모지</summary>
    SubLeftEmoji     = 1 << 8,

    /// <summary>우측 서브 초상화 위 이모지</summary>
    SubRightEmoji    = 1 << 9,

    // ────────────── 선택지 버튼 ──────────────
    /// <summary>선택지 1 버튼 + 텍스트</summary>
    Choice0          = 1 << 10,

    /// <summary>선택지 2 버튼 + 텍스트</summary>
    Choice1          = 1 << 11,

    /// <summary>선택지 3 버튼 + 텍스트</summary>
    Choice2          = 1 << 12,

    /// <summary>선택지 패널 전체(루트)만 따로 제어하고 싶을 때</summary>
    ChoicePanel      = 1 << 13,

    // ────────────── 그룹 마스크 ──────────────
    AllPortraitImages =
        MainPortraitImage |
        SubLeftPortraitImage |
        SubRightPortraitImage,

    AllEmojis =
        MainEmoji |
        SubLeftEmoji |
        SubRightEmoji,

    AllChoices =
        Choice0 |
        Choice1 |
        Choice2,

    // 기본 스토리 대사 UI (대사 박스 + 텍스트 + 이름 박스)
    StoryLineDefault =
        //LineText |
        DialogueBox |
        NameBox,

    // 선택지 기본 세트 (버튼들만)
    ChoiceDefault =
        AllChoices,

    // 전체 (필요하면 사용)
    All =
        StoryLineDefault |
        ProtagonistCutin |
        AllPortraitImages |
        AllEmojis |
        ChoicePanel |
        AllChoices
}