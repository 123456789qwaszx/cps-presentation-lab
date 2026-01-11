namespace Lab.UI.Naming
{
    public readonly struct DialogueRoleWidgetTags
    {
        private readonly string _roleKey;

        // ---- Line ----
        private const string LineTextSuffix    = "_LineText";
        private const string LineBodySuffix    = "_LineBody";
        private const string SpeakerNameSuffix = "_SpeakerName";

        // ---- Standing Portrait Set ----
        private const string StandingPortraitRootSuffix        = "_StandingPortraitRoot";
        private const string StandingPortraitTrackSuffix       = "_StandingPortrait_Track";
        private const string StandingPortraitRigSuffix         = "_StandingPortrait_Rig";
        private const string StandingPortraitSwayPivotSuffix   = "_StandingPortrait_SwayPivot";
        private const string StandingPortraitShakeSuffix       = "_StandingPortrait_Shake";
        private const string StandingPortraitScaleSuffix       = "_StandingPortrait_Scale";
        private const string StandingPortraitVisualSuffix      = "_StandingPortrait_Visual";
        private const string StandingPortraitImageSuffix       = "_StandingPortrait_Image";
        private const string StandingPortraitEmojiAnchorSuffix = "_StandingPortrait_EmojiAnchor";
        private const string StandingPortraitEmojiImageSuffix  = "_StandingPortrait_EmojiImage";

        // ---- Protagonist Cutin (Image만 사용) ----
        private const string ProtagonistCutinImageSuffix       = "_ProtagonistCutin_Image";

        // ---- Background Set ----
        private const string BackgroundImageSuffix             = "_Background_Image";

        public DialogueRoleWidgetTags(string roleKey)
        {
            _roleKey = roleKey ?? string.Empty;
        }

        // ---- Line ----
        public string LineTextTag        => $"{_roleKey}{LineTextSuffix}";
        public string LineBodyTag        => $"{_roleKey}{LineBodySuffix}";
        public string SpeakerNameTag     => $"{_roleKey}{SpeakerNameSuffix}";

        // ---- Standing Portrait ----
        public string StandingPortraitRootTag        => $"{_roleKey}{StandingPortraitRootSuffix}";
        public string StandingPortraitTrackTag       => $"{_roleKey}{StandingPortraitTrackSuffix}";
        public string StandingPortraitRigTag         => $"{_roleKey}{StandingPortraitRigSuffix}";
        public string StandingPortraitSwayPivotTag   => $"{_roleKey}{StandingPortraitSwayPivotSuffix}";
        public string StandingPortraitShakeTag       => $"{_roleKey}{StandingPortraitShakeSuffix}";
        public string StandingPortraitScaleTag       => $"{_roleKey}{StandingPortraitScaleSuffix}";
        public string StandingPortraitVisualTag      => $"{_roleKey}{StandingPortraitVisualSuffix}";
        public string StandingPortraitImageTag       => $"{_roleKey}{StandingPortraitImageSuffix}";
        public string StandingPortraitEmojiAnchorTag => $"{_roleKey}{StandingPortraitEmojiAnchorSuffix}";
        public string StandingPortraitEmojiImageTag  => $"{_roleKey}{StandingPortraitEmojiImageSuffix}";

        // ---- Protagonist Cutin (Image만 사용) ----
        public string ProtagonistCutinImageTag       => $"{_roleKey}{ProtagonistCutinImageSuffix}";

        // ---- Background ----
        public string BackgroundImageTag             => $"{_roleKey}{BackgroundImageSuffix}";
    }
}
