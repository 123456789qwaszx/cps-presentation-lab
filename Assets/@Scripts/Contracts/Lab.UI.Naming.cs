namespace Lab.UI.Naming
{
    public readonly struct DialogueRoleWidgetTags
    {
        private readonly string _roleKey;

        // ---- Line ----
        private const string LineBodySuffix    = "_LineBody";
        private const string LineTextSuffix    = "_LineText";
        private const string SpeakerNameSuffix = "_SpeakerName";
        private const string SpeakerNameBoxSuffix = "_SpeakerNameBox";
        private const string ProtagonistCutinImageSuffix       = "_ProtagonistCutin_Image";

        // ---- Main Standing Portrait Set ----
        private const string MainStandingPortraitRootSuffix        = "_MainStandingPortraitRoot";
        private const string MainStandingPortraitTrackSuffix       = "_MainStandingPortrait_Track";
        private const string MainStandingPortraitRigSuffix         = "_MainStandingPortrait_Rig";
        private const string MainStandingPortraitSwayPivotSuffix   = "_MainStandingPortrait_SwayPivot";
        private const string MainStandingPortraitShakeSuffix       = "_MainStandingPortrait_Shake";
        private const string MainStandingPortraitScaleSuffix       = "_MainStandingPortrait_Scale";
        private const string MainStandingPortraitVisualSuffix      = "_MainStandingPortrait_Visual";
        private const string MainStandingPortraitImageSuffix       = "_MainStandingPortrait_Image";
        private const string MainStandingPortraitEmojiAnchorSuffix = "_MainStandingPortrait_EmojiAnchor";
        private const string MainStandingPortraitEmojiImageSuffix  = "_MainStandingPortrait_EmojiImage";

        // ---- SubLeft Standing Portrait Set ----
        private const string SubLeftStandingPortraitRootSuffix        = "_SubLeftStandingPortraitRoot";
        private const string SubLeftStandingPortraitTrackSuffix       = "_SubLeftStandingPortrait_Track";
        private const string SubLeftStandingPortraitRigSuffix         = "_SubLeftStandingPortrait_Rig";
        private const string SubLeftStandingPortraitSwayPivotSuffix   = "_SubLeftStandingPortrait_SwayPivot";
        private const string SubLeftStandingPortraitShakeSuffix       = "_SubLeftStandingPortrait_Shake";
        private const string SubLeftStandingPortraitScaleSuffix       = "_SubLeftStandingPortrait_Scale";
        private const string SubLeftStandingPortraitVisualSuffix      = "_SubLeftStandingPortrait_Visual";
        private const string SubLeftStandingPortraitImageSuffix       = "_SubLeftStandingPortrait_Image";
        private const string SubLeftStandingPortraitEmojiAnchorSuffix = "_SubLeftStandingPortrait_EmojiAnchor";
        private const string SubLeftStandingPortraitEmojiImageSuffix  = "_SubLeftStandingPortrait_EmojiImage";

        // ---- SubRight Standing Portrait Set ----
        private const string SubRightStandingPortraitRootSuffix        = "_SubRightStandingPortraitRoot";
        private const string SubRightStandingPortraitTrackSuffix       = "_SubRightStandingPortrait_Track";
        private const string SubRightStandingPortraitRigSuffix         = "_SubRightStandingPortrait_Rig";
        private const string SubRightStandingPortraitSwayPivotSuffix   = "_SubRightStandingPortrait_SwayPivot";
        private const string SubRightStandingPortraitShakeSuffix       = "_SubRightStandingPortrait_Shake";
        private const string SubRightStandingPortraitScaleSuffix       = "_SubRightStandingPortrait_Scale";
        private const string SubRightStandingPortraitVisualSuffix      = "_SubRightStandingPortrait_Visual";
        private const string SubRightStandingPortraitImageSuffix       = "_SubRightStandingPortrait_Image";
        private const string SubRightStandingPortraitEmojiAnchorSuffix = "_SubRightStandingPortrait_EmojiAnchor";
        private const string SubRightStandingPortraitEmojiImageSuffix  = "_SubRightStandingPortrait_EmojiImage";

        // ---- Background Set (3장까지) ----
        private const string BackgroundImageSuffix             = "_Background_Image";
        private const string BackgroundImage2Suffix            = "_Background_Image2";
        private const string BackgroundImage3Suffix            = "_Background_Image3";

        // ---- Dialogue Box Root ----
        private const string DialogueBoxRootSuffix             = "_DialogueBoxRoot";

        // ---- Choice Panel + 3 Choices ----
        private const string ChoicePanelRootSuffix             = "_ChoicePanelRoot";
        private const string ChoiceButton0RootSuffix           = "_ChoiceButton0Root";
        private const string ChoiceButton1RootSuffix           = "_ChoiceButton1Root";
        private const string ChoiceButton2RootSuffix           = "_ChoiceButton2Root";

        private const string ChoiceButton0TextSuffix = "_ChoiceButton0_text";
        private const string ChoiceButton1TextSuffix = "_ChoiceButton1_text";
        private const string ChoiceButton2TextSuffix = "_ChoiceButton2_text";

        public DialogueRoleWidgetTags(string roleKey)
        {
            _roleKey = roleKey ?? string.Empty;
        }

        // ---- Line ----
        public string LineBodyTag        => $"{_roleKey}{LineBodySuffix}";
        public string LineTextTag        => $"{_roleKey}{LineTextSuffix}";

        // ---- Main Standing Portrait ----
        public string MainStandingPortraitRootTag        => $"{_roleKey}{MainStandingPortraitRootSuffix}";
        public string MainStandingPortraitTrackTag       => $"{_roleKey}{MainStandingPortraitTrackSuffix}";
        public string MainStandingPortraitRigTag         => $"{_roleKey}{MainStandingPortraitRigSuffix}";
        public string MainStandingPortraitSwayPivotTag   => $"{_roleKey}{MainStandingPortraitSwayPivotSuffix}";
        public string MainStandingPortraitShakeTag       => $"{_roleKey}{MainStandingPortraitShakeSuffix}";
        public string MainStandingPortraitScaleTag       => $"{_roleKey}{MainStandingPortraitScaleSuffix}";
        public string MainStandingPortraitVisualTag      => $"{_roleKey}{MainStandingPortraitVisualSuffix}";
        public string MainStandingPortraitImageTag       => $"{_roleKey}{MainStandingPortraitImageSuffix}";
        public string MainStandingPortraitEmojiAnchorTag => $"{_roleKey}{MainStandingPortraitEmojiAnchorSuffix}";
        public string MainStandingPortraitEmojiImageTag  => $"{_roleKey}{MainStandingPortraitEmojiImageSuffix}";

        // ---- SubLeft Standing Portrait ----
        public string SubLeftStandingPortraitRootTag        => $"{_roleKey}{SubLeftStandingPortraitRootSuffix}";
        public string SubLeftStandingPortraitTrackTag       => $"{_roleKey}{SubLeftStandingPortraitTrackSuffix}";
        public string SubLeftStandingPortraitRigTag         => $"{_roleKey}{SubLeftStandingPortraitRigSuffix}";
        public string SubLeftStandingPortraitSwayPivotTag   => $"{_roleKey}{SubLeftStandingPortraitSwayPivotSuffix}";
        public string SubLeftStandingPortraitShakeTag       => $"{_roleKey}{SubLeftStandingPortraitShakeSuffix}";
        public string SubLeftStandingPortraitScaleTag       => $"{_roleKey}{SubLeftStandingPortraitScaleSuffix}";
        public string SubLeftStandingPortraitVisualTag      => $"{_roleKey}{SubLeftStandingPortraitVisualSuffix}";
        public string SubLeftStandingPortraitImageTag       => $"{_roleKey}{SubLeftStandingPortraitImageSuffix}";
        public string SubLeftStandingPortraitEmojiAnchorTag => $"{_roleKey}{SubLeftStandingPortraitEmojiAnchorSuffix}";
        public string SubLeftStandingPortraitEmojiImageTag  => $"{_roleKey}{SubLeftStandingPortraitEmojiImageSuffix}";

        // ---- SubRight Standing Portrait ----
        public string SubRightStandingPortraitRootTag        => $"{_roleKey}{SubRightStandingPortraitRootSuffix}";
        public string SubRightStandingPortraitTrackTag       => $"{_roleKey}{SubRightStandingPortraitTrackSuffix}";
        public string SubRightStandingPortraitRigTag         => $"{_roleKey}{SubRightStandingPortraitRigSuffix}";
        public string SubRightStandingPortraitSwayPivotTag   => $"{_roleKey}{SubRightStandingPortraitSwayPivotSuffix}";
        public string SubRightStandingPortraitShakeTag       => $"{_roleKey}{SubRightStandingPortraitShakeSuffix}";
        public string SubRightStandingPortraitScaleTag       => $"{_roleKey}{SubRightStandingPortraitScaleSuffix}";
        public string SubRightStandingPortraitVisualTag      => $"{_roleKey}{SubRightStandingPortraitVisualSuffix}";
        public string SubRightStandingPortraitImageTag       => $"{_roleKey}{SubRightStandingPortraitImageSuffix}";
        public string SubRightStandingPortraitEmojiAnchorTag => $"{_roleKey}{SubRightStandingPortraitEmojiAnchorSuffix}";
        public string SubRightStandingPortraitEmojiImageTag  => $"{_roleKey}{SubRightStandingPortraitEmojiImageSuffix}";

        // ---- SpeakerNameBox(Image만 사용) ----
        
        public string SpeakerNameBoxTag              => $"{_roleKey}{SpeakerNameBoxSuffix}";
        public string SpeakerNameTag                 => $"{_roleKey}{SpeakerNameSuffix}";
        public string ProtagonistCutinImageTag       => $"{_roleKey}{ProtagonistCutinImageSuffix}";

        // ---- Background ----
        public string BackgroundImageTag             => $"{_roleKey}{BackgroundImageSuffix}";
        public string BackgroundImage2Tag            => $"{_roleKey}{BackgroundImage2Suffix}";
        public string BackgroundImage3Tag            => $"{_roleKey}{BackgroundImage3Suffix}";

        // ---- Dialogue Box Root ----
        public string DialogueBoxRootTag             => $"{_roleKey}{DialogueBoxRootSuffix}";

        // ---- Choice Panel + 3 Choices ----
        public string ChoicePanelRootTag             => $"{_roleKey}{ChoicePanelRootSuffix}";
        
        public string ChoiceButton0RootTag           => $"{_roleKey}{ChoiceButton0RootSuffix}";
        public string ChoiceButton1RootTag           => $"{_roleKey}{ChoiceButton1RootSuffix}";
        public string ChoiceButton2RootTag           => $"{_roleKey}{ChoiceButton2RootSuffix}";
        
        public string ChoiceButton0TextTag => $"{_roleKey}{ChoiceButton0TextSuffix}";
        public string ChoiceButton1TextTag => $"{_roleKey}{ChoiceButton1TextSuffix}";
        public string ChoiceButton2TextTag => $"{_roleKey}{ChoiceButton2TextSuffix}";
    }
}
