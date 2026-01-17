namespace Lab.UI.Naming
{
    public readonly struct DialogueRoleWidgetTags
    {
        private readonly string _roleKey;

        // ---- Dialogue Line ----
        private const string DialogueBoxRootSuffix       = "_DialogueBox00_Root";
        private const string LineBodySuffix              = "_DialogueBox00_Image";
        private const string ProtagonistCutinImageSuffix = "_ProtagonistCutin00_Image";
        private const string LineTextSuffix              = "_DialogueBox00_Text";
        private const string SpeakerNameSuffix           = "_SpeakerNameBox00_Text";
        private const string SpeakerNameBoxSuffix        = "_SpeakerNameBox00_Image";

        // ---- Main Standing Portrait ----
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

        // ---- SubLeft Standing Portrait ----
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

        // ---- SubRight Standing Portrait ----
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

        // ---- Background (00/01) ----
        private const string BackgroundRoot00Suffix  = "_Background00_Root";
        private const string BackgroundImage00Suffix = "_Background00_Image";
        
        private const string BackgroundRoot01Suffix  = "_Background01_Root";
        private const string BackgroundImage01Suffix = "_Background01_Image";

        // ---- Choice Panel ----
        private const string ChoicePanelRootSuffix    = "_ChoicePanel00_Root";
        private const string ChoicePanelImageSuffix   = "_ChoicePanel00_Image";
        
        private const string ChoiceButton0RootSuffix  = "_ChoiceButton00_Root";
        private const string ChoiceButton0TextSuffix  = "_ChoiceButton00_Text";
        private const string ChoiceButton0ImageSuffix = "_ChoiceButton00_Image";
        
        private const string ChoiceButton1RootSuffix  = "_ChoiceButton01_Root";
        private const string ChoiceButton1TextSuffix  = "_ChoiceButton01_Text";
        private const string ChoiceButton1ImageSuffix = "_ChoiceButton01_Image";
        
        private const string ChoiceButton2RootSuffix  = "_ChoiceButton02_Root";
        private const string ChoiceButton2TextSuffix  = "_ChoiceButton02_Text";
        private const string ChoiceButton2ImageSuffix = "_ChoiceButton02_Image";

        // ---- VFX Panel ----
        private const string VFXBlackScreen00RootSuffix = "_VFXBlackScreen00_Root";
        private const string VFXBlackOut00ImageSuffix   = "_VFXBlackScreen00_Image0";
        private const string VFXBlackOut01ImageSuffix   = "_VFXBlackScreen00_Image1";

        private const string BlackFade00RootSuffix  = "_VFXBlackFade00_Root";
        private const string BlackFade00ImageSuffix = "_VFXBlackFade00_Image";

        private const string SkipToggle00RootSuffix  = "_SkipToggle00_Root";
        private const string SkipToggle00ImageSuffix = "_SkipToggle00_Image0";
        private const string SkipToggle01ImageSuffix = "_SkipToggle00_Image1";
        private const string SkipToggle00TextSuffix  = "_SkipToggle00_Text";

        private const string NextToggle00RootSuffix  = "_NextToggle00_Root";
        private const string NextToggle00ImageSuffix = "_NextToggle00_Image";
        private const string NextToggle00TextSuffix  = "_NextToggle00_Text";

        private const string AutoToggle00RootSuffix  = "_AutoToggle00_Root";
        private const string AutoToggle00ImageSuffix = "_AutoToggle00_Image0";
        private const string AutoToggle01ImageSuffix = "_AutoToggle00_Image1";

        private const string SpeedupToggle00RootSuffix  = "_SpeedupToggle00_Root";
        private const string SpeedupToggle00ImageSuffix = "_SpeedupToggle00_Image0";
        private const string SpeedupToggle01ImageSuffix = "_SpeedupToggle00_Image1";

        private const string SetSpeedToggle00RootSuffix  = "_SetSpeedToggle00_Root";
        private const string SetSpeedToggle00ImageSuffix = "_SetSpeedToggle00_Image";
        private const string SetSpeedToggle01ImageSuffix = "_SpeedToggle00_Image";
        private const string SetSpeedToggle02ImageSuffix = "_SpeedToggle01_Image";
        private const string SetSpeedToggle03ImageSuffix = "_SpeedToggle02_Image";

        public DialogueRoleWidgetTags(string roleKey)
        {
            _roleKey = roleKey ?? string.Empty;
        }

        // ---- Dialogue Line ----
        public string DialogueBoxRootTag       => $"{_roleKey}{DialogueBoxRootSuffix}";
        public string LineBodyTag              => $"{_roleKey}{LineBodySuffix}";
        public string LineTextTag              => $"{_roleKey}{LineTextSuffix}";
        public string SpeakerNameBoxTag        => $"{_roleKey}{SpeakerNameBoxSuffix}";
        public string SpeakerNameTag           => $"{_roleKey}{SpeakerNameSuffix}";
        public string ProtagonistCutinImageTag => $"{_roleKey}{ProtagonistCutinImageSuffix}";

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

        // ---- Background (00/01) ----
        public string BackgroundRoot00Tag  => $"{_roleKey}{BackgroundRoot00Suffix}";
        public string BackgroundRoot01Tag  => $"{_roleKey}{BackgroundRoot01Suffix}";
        public string BackgroundImage00Tag => $"{_roleKey}{BackgroundImage00Suffix}";
        public string BackgroundImage01Tag => $"{_roleKey}{BackgroundImage01Suffix}";

        // ---- Choice Panel + 3 Choices ----
        public string ChoicePanelRootTag    => $"{_roleKey}{ChoicePanelRootSuffix}";
        public string ChoicePanelImageTag  => $"{_roleKey}{ChoicePanelImageSuffix}";
        
        public string ChoiceButton0RootTag  => $"{_roleKey}{ChoiceButton0RootSuffix}";
        public string ChoiceButton1RootTag  => $"{_roleKey}{ChoiceButton1RootSuffix}";
        public string ChoiceButton2RootTag  => $"{_roleKey}{ChoiceButton2RootSuffix}";

        public string ChoiceButton0TextTag  => $"{_roleKey}{ChoiceButton0TextSuffix}";
        public string ChoiceButton1TextTag  => $"{_roleKey}{ChoiceButton1TextSuffix}";
        public string ChoiceButton2TextTag  => $"{_roleKey}{ChoiceButton2TextSuffix}";

        public string ChoiceButton0ImageTag => $"{_roleKey}{ChoiceButton0ImageSuffix}";
        public string ChoiceButton1ImageTag => $"{_roleKey}{ChoiceButton1ImageSuffix}";
        public string ChoiceButton2ImageTag => $"{_roleKey}{ChoiceButton2ImageSuffix}";

        // ---- VFX Panel ----
        public string VFXBlackScreen00RootTag => $"{_roleKey}{VFXBlackScreen00RootSuffix}";
        public string VFXBlackOut00ImageTag   => $"{_roleKey}{VFXBlackOut00ImageSuffix}";
        public string VFXBlackOut01ImageTag   => $"{_roleKey}{VFXBlackOut01ImageSuffix}";

        public string BlackFade00RootTag      => $"{_roleKey}{BlackFade00RootSuffix}";
        public string BlackFade00ImageTag     => $"{_roleKey}{BlackFade00ImageSuffix}";

        public string SkipToggle00RootTag     => $"{_roleKey}{SkipToggle00RootSuffix}";
        public string SkipToggle00ImageTag    => $"{_roleKey}{SkipToggle00ImageSuffix}";
        public string SkipToggle01ImageTag    => $"{_roleKey}{SkipToggle01ImageSuffix}";
        public string SkipToggle00TextTag     => $"{_roleKey}{SkipToggle00TextSuffix}";

        public string NextToggle00RootTag     => $"{_roleKey}{NextToggle00RootSuffix}";
        public string NextToggle00ImageTag    => $"{_roleKey}{NextToggle00ImageSuffix}";
        public string NextToggle00TextTag     => $"{_roleKey}{NextToggle00TextSuffix}";

        public string AutoToggle00RootTag     => $"{_roleKey}{AutoToggle00RootSuffix}";
        public string AutoToggle00ImageTag    => $"{_roleKey}{AutoToggle00ImageSuffix}";
        public string AutoToggle01ImageTag    => $"{_roleKey}{AutoToggle01ImageSuffix}";

        public string SpeedupToggle00RootTag  => $"{_roleKey}{SpeedupToggle00RootSuffix}";
        public string SpeedupToggle00ImageTag => $"{_roleKey}{SpeedupToggle00ImageSuffix}";
        public string SpeedupToggle01ImageTag => $"{_roleKey}{SpeedupToggle01ImageSuffix}";

        public string SetSpeedToggle00RootTag  => $"{_roleKey}{SetSpeedToggle00RootSuffix}";
        public string SetSpeedToggle00ImageTag => $"{_roleKey}{SetSpeedToggle00ImageSuffix}";
        public string SetSpeedToggle01ImageTag => $"{_roleKey}{SetSpeedToggle01ImageSuffix}";
        public string SetSpeedToggle02ImageTag => $"{_roleKey}{SetSpeedToggle02ImageSuffix}";
        public string SetSpeedToggle03ImageTag => $"{_roleKey}{SetSpeedToggle03ImageSuffix}";
    }
}
