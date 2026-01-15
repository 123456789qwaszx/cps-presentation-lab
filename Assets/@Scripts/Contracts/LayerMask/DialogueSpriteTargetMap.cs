public enum DialogueSpriteTarget
{
    // Portraits
    MainPortrait,
    SubLeftPortrait,
    SubRightPortrait,

    // Emojis
    MainEmoji,
    SubLeftEmoji,
    SubRightEmoji,

    // Backgrounds
    Background0,
    Background1,

    // Choice buttons
    Choice0,
    Choice1,
    Choice2,

    // Cutin
    ProtagonistCutin,
}

public static class DialogueSpriteTargetMap
{
    /// <summary>
    /// DialogueSpriteTarget → 실제 위젯 슬롯(DialogueWidgetTarget) 매핑.
    /// "어떤 논리적인 스프라이트 타겟이 어느 위젯에 연결되는가"에 대한
    /// 계약을 한 곳에 모아둔다.
    /// </summary>
    public static bool TryResolve(
        DialogueSpriteTarget spriteTarget,
        out DialogueWidgetTarget widgetTarget)
    {
        widgetTarget = spriteTarget switch
        {
            DialogueSpriteTarget.MainPortrait     => DialogueWidgetTarget.MainStandingPortraitImage,
            DialogueSpriteTarget.SubLeftPortrait  => DialogueWidgetTarget.SubLeftStandingPortraitImage,
            DialogueSpriteTarget.SubRightPortrait => DialogueWidgetTarget.SubRightStandingPortraitImage,

            DialogueSpriteTarget.MainEmoji        => DialogueWidgetTarget.MainStandingPortraitEmojiImage,
            DialogueSpriteTarget.SubLeftEmoji     => DialogueWidgetTarget.SubLeftStandingPortraitEmojiImage,
            DialogueSpriteTarget.SubRightEmoji    => DialogueWidgetTarget.SubRightStandingPortraitEmojiImage,

            DialogueSpriteTarget.Background0      => DialogueWidgetTarget.BackgroundImage0,
            DialogueSpriteTarget.Background1      => DialogueWidgetTarget.BackgroundImage1,

            DialogueSpriteTarget.Choice0          => DialogueWidgetTarget.ChoiceButton0Root,
            DialogueSpriteTarget.Choice1          => DialogueWidgetTarget.ChoiceButton1Root,
            DialogueSpriteTarget.Choice2          => DialogueWidgetTarget.ChoiceButton2Root,

            DialogueSpriteTarget.ProtagonistCutin => DialogueWidgetTarget.ProtagonistCutinImage,

            _ => DialogueWidgetTarget.None
        };

        return widgetTarget != DialogueWidgetTarget.None;
    }
}