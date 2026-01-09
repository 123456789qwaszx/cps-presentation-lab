namespace Lab.UI.Keys
{
    public static class LabUIRouteKeys
    {
        [UIRouteDefinition] public const string Title = "lab/ui/title";
        [UIRouteDefinition] public const string Dialogue = "lab/ui/dialogue";
    }

    public static class LabUIActionKeys
    {
        public static readonly UIActionKey OpenClickerTitle = UIActionKeyRegistry.Get(LabUIRouteKeys.Title);
        public static readonly UIActionKey OpenDialogue = UIActionKeyRegistry.Get(LabUIRouteKeys.Dialogue);
    }

    public static class LabUIScreenKeys
    {
        [UIScreenKey] public static ScreenKey Title = new(LabUIRouteKeys.Title);
        [UIScreenKey] public static ScreenKey Dialogue = new(LabUIRouteKeys.Dialogue);
    }
}