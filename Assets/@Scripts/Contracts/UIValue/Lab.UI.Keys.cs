namespace Lab.UI.Keys
{
    public static class LabUIRouteKeys
    {
        [UIRouteDefinition] public const string Title = "lab/ui/title";
        [UIRouteDefinition] public const string Dialogue = "lab/ui/dialogue";
        [UIRouteDefinition] public const string Dialogue_02 = "lab/ui/dialogue_02";
        [UIRouteDefinition] public const string Dialogue_10 = "lab/ui/dialogue_10";
    }

    public static class LabUIActionKeys
    {
        public static readonly UIActionKey OpenClickerTitle = UIActionKeyRegistry.Get(LabUIRouteKeys.Title);
        public static readonly UIActionKey OpenDialogue = UIActionKeyRegistry.Get(LabUIRouteKeys.Dialogue);
        public static readonly UIActionKey OpenDialogue_02 = UIActionKeyRegistry.Get(LabUIRouteKeys.Dialogue_02);
        public static readonly UIActionKey OpenDialogue_10 = UIActionKeyRegistry.Get(LabUIRouteKeys.Dialogue_10);
    }

    public static class LabUIScreenKeys
    {
        [UIScreenKey] public static ScreenKey Title = new(LabUIRouteKeys.Title);
        [UIScreenKey] public static ScreenKey Dialogue = new(LabUIRouteKeys.Dialogue);
        [UIScreenKey] public static ScreenKey Dialogue_02 = new(LabUIRouteKeys.Dialogue_02);
        [UIScreenKey] public static ScreenKey Dialogue_10 = new(LabUIRouteKeys.Dialogue_10);
    }
}