public enum DialogueSpriteTarget
{
    None = -1,

    // Portrait images (Standing 0~2)
    Standing00Portrait,
    Standing01Portrait,
    Standing02Portrait,

    // Portrait overlays (optional layer)
    Standing00PortraitOverlay,
    Standing01PortraitOverlay,
    Standing02PortraitOverlay,

    // Emojis (each standing has 4 emoji slots)
    Standing00Emoji00,
    Standing00Emoji01,
    Standing00Emoji02,
    Standing00Emoji03,

    Standing01Emoji00,
    Standing01Emoji01,
    Standing01Emoji02,
    Standing01Emoji03,

    Standing02Emoji00,
    Standing02Emoji01,
    Standing02Emoji02,
    Standing02Emoji03,
    
    // Cutin
    ProtagonistCutin00,
    ProtagonistCutin00Emoji00,
    ProtagonistCutin00Emoji01,
    ProtagonistCutin00Emoji02,
    
    // Backgrounds
    Background00,
    Background01,
    
    AdvanceIndicator00Image,
}


public static class DialogueSpriteTargetMap
{
    /// <summary>
    /// DialogueSpriteTarget -> DialogueWidgetTarget(Image 슬롯) 매핑.
    /// "논리적 스프라이트 타겟이 실제 어느 Image 위젯에 연결되는가" 계약.
    /// </summary>
    public static bool TryResolve(
        DialogueSpriteTarget spriteTarget,
        out DialogueWidgetTarget widgetTarget)
    {
        widgetTarget = spriteTarget switch
        {
            // Portrait
            DialogueSpriteTarget.Standing00Portrait => DialogueWidgetTarget.Standing00PortraitImage,
            DialogueSpriteTarget.Standing01Portrait => DialogueWidgetTarget.Standing01PortraitImage,
            DialogueSpriteTarget.Standing02Portrait => DialogueWidgetTarget.Standing02PortraitImage,

            // Overlay (single image)
            DialogueSpriteTarget.Standing00PortraitOverlay => DialogueWidgetTarget.Standing00PortraitOverlaysImage,
            DialogueSpriteTarget.Standing01PortraitOverlay => DialogueWidgetTarget.Standing01PortraitOverlaysImage,
            DialogueSpriteTarget.Standing02PortraitOverlay => DialogueWidgetTarget.Standing02PortraitOverlaysImage,

            // Emojis
            DialogueSpriteTarget.Standing00Emoji00 => DialogueWidgetTarget.Standing00Emoji00Image,
            DialogueSpriteTarget.Standing00Emoji01 => DialogueWidgetTarget.Standing00Emoji01Image,
            DialogueSpriteTarget.Standing00Emoji02 => DialogueWidgetTarget.Standing00Emoji02Image,
            DialogueSpriteTarget.Standing00Emoji03 => DialogueWidgetTarget.Standing00Emoji03Image,

            DialogueSpriteTarget.Standing01Emoji00 => DialogueWidgetTarget.Standing01Emoji00Image,
            DialogueSpriteTarget.Standing01Emoji01 => DialogueWidgetTarget.Standing01Emoji01Image,
            DialogueSpriteTarget.Standing01Emoji02 => DialogueWidgetTarget.Standing01Emoji02Image,
            DialogueSpriteTarget.Standing01Emoji03 => DialogueWidgetTarget.Standing01Emoji03Image,

            DialogueSpriteTarget.Standing02Emoji00 => DialogueWidgetTarget.Standing02Emoji00Image,
            DialogueSpriteTarget.Standing02Emoji01 => DialogueWidgetTarget.Standing02Emoji01Image,
            DialogueSpriteTarget.Standing02Emoji02 => DialogueWidgetTarget.Standing02Emoji02Image,
            DialogueSpriteTarget.Standing02Emoji03 => DialogueWidgetTarget.Standing02Emoji03Image,

            // Cutin
            DialogueSpriteTarget.ProtagonistCutin00 => DialogueWidgetTarget.ProtagonistCutin00Image,
            DialogueSpriteTarget.ProtagonistCutin00Emoji00 => DialogueWidgetTarget.ProtagonistCutin00Emoji00Image,
            DialogueSpriteTarget.ProtagonistCutin00Emoji01 => DialogueWidgetTarget.ProtagonistCutin00Emoji01Image,
            DialogueSpriteTarget.ProtagonistCutin00Emoji02 => DialogueWidgetTarget.ProtagonistCutin00Emoji02Image,
            
            // Background
            DialogueSpriteTarget.Background00 => DialogueWidgetTarget.Background00Image,
            DialogueSpriteTarget.Background01 => DialogueWidgetTarget.Background01Image,
            
            DialogueSpriteTarget.AdvanceIndicator00Image => DialogueWidgetTarget.AdvanceIndicator00Image,

            _ => DialogueWidgetTarget.None
        };

        return widgetTarget != DialogueWidgetTarget.None;
    }
}