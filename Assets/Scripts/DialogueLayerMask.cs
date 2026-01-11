[System.Flags]
public enum DialogueLayerMask
{
    None = 0,

    // ---- Primitive layers (single bits) ----
    // BG1: 기본 배경(씬의 메인)
    // BG2: 보조 배경(이펙트, 페이드용, 컷 전환용 등)
    Background1     = 1 << 0,
    Background2     = 1 << 1,
    // 나중에 필요하면: Background3 = 1 << 2,

    MainPortrait    = 1 << 3,  // 중앙/주인공
    SubLeftPortrait = 1 << 4,  // 좌측 서브
    SubRightPortrait= 1 << 5,  // 우측 서브

    DialogueBox     = 1 << 6,
    ChoicePanel     = 1 << 7,

    // ---- Group masks (단순 그룹) ----
    AllBackgrounds  = Background1 | Background2,
    AllPortraits    = MainPortrait | SubLeftPortrait | SubRightPortrait,
    AllUI           = DialogueBox | ChoicePanel,

    // ---- VN Presets (실제 연출에서 자주 쓰는 상태들) ----
    // 1) 일반 스토리: 배경 1 + 대화창
    Story_Default   = Background1 | DialogueBox,

    // 2) 주로 나오는 패턴들
    Story_Solo      = Background1 | MainPortrait | DialogueBox,                           // 1인 화자
    Story_Duo       = Background1 | MainPortrait | SubLeftPortrait | DialogueBox,         // 2인 (메인 + 서브)
    Story_Trio      = Background1 | MainPortrait | SubLeftPortrait | SubRightPortrait | DialogueBox, // 3인

    // 3) 선택지: 보통 대화창 대신 선택지만 뜸
    Choice_Default  = Background1 | ChoicePanel,

    // (원하면 BG2를 섞은 변형도 나중에 추가 가능)
    // Story_Solo_WithSubBg  = Background1 | Background2 | MainPortrait | DialogueBox;
    // Choice_WithSubBg      = Background1 | Background2 | ChoicePanel;

    // ---- Everything ----
    All             = AllBackgrounds | AllPortraits | AllUI
}