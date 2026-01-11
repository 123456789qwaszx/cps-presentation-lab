namespace Lab.UI.Naming
{
    public readonly struct DialogueRoleWidgetTags
    {
        private readonly string _roleKey;

        private const string LineTextSuffix         = "_LineText";          // 대사 본문 텍스트
        private const string LineBodySuffix         = "_LineBody";
        private const string SpeakerNameSuffix      = "_SpeakerName";       // 화자 이름
        private const string StandingPortraitSuffix = "_StandingPortrait";  // 중앙 스탠딩 일러스트
        private const string ProtagonistCutinSuffix = "_ProtagonistCutin"; // 주인공 전용 오버레이/컷인
        private const string BackgroundImageSuffix  = "_BackgroundImage";

        public DialogueRoleWidgetTags(string roleKey)
        {
            _roleKey = roleKey ?? string.Empty;
        }

        public string LineTextTag         => $"{_roleKey}{LineTextSuffix}";
        public string LineBodyTag         => $"{_roleKey}{LineBodySuffix}";
        public string SpeakerNameTag      => $"{_roleKey}{SpeakerNameSuffix}";
        public string StandingPortraitTag => $"{_roleKey}{StandingPortraitSuffix}";
        public string ProtagonistCutinTag => $"{_roleKey}{ProtagonistCutinSuffix}";
        public string BackgroundImageTag => $"{_roleKey}{BackgroundImageSuffix}";
        
    }
}