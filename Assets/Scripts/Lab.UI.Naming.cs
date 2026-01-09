namespace Lab.UI.Naming
{
    public static class UIWidgetSets
    {
        public static readonly DialogueWidgetSet Dialogue = new("Dialogue");
    }
    
    public readonly struct DialogueWidgetSet
    {
        public readonly string BaseId;

        public DialogueWidgetSet(string baseId)
        {
            BaseId = baseId ?? "";
        }

        public string BodyName     => DialogueWidgetNames.Body(BaseId);
        public string NameName     => DialogueWidgetNames.Name(BaseId);
        public string PortraitName => DialogueWidgetNames.Portrait(BaseId);
        
        public string EmoteName    => DialogueWidgetNames.Emote(BaseId); 
    }
    
    public static class DialogueWidgetNames
    {
        public const string BodySuffix     = "_Body";
        public const string NameSuffix     = "_Name";
        public const string PortraitSuffix = "_Portrait";
        public const string EmoteSuffix    = "_Emote";

        public static string Body(string baseId)     => baseId + BodySuffix;
        public static string Name(string baseId)     => baseId + NameSuffix;
        public static string Portrait(string baseId) => baseId + PortraitSuffix;
        public static string Emote(string baseId)    => baseId + EmoteSuffix;
    }
}