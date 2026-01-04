namespace Lab.UI.Keys
{
    public static class LabRouteKeys
    {
        [UIRouteDefinition] public const string Title = "lab/title";
        [UIRouteDefinition] public const string InGame = "lab/ingame";
        [UIRouteDefinition] public const string Dialogue = "lab/dialogue";
    }

    public static class LabUIActionKeys
    {
        public static readonly UIActionKey OpenClickerTitle = UIActionKeyRegistry.Get(LabRouteKeys.Title);
    
        public static readonly UIActionKey OpenInGame = UIActionKeyRegistry.Get(LabRouteKeys.InGame);
    
        public static readonly UIActionKey OpenDialogue = UIActionKeyRegistry.Get(LabRouteKeys.Dialogue);
    }

    public static class LabUIScreenKeys
    {
        [UIScreenKey]
        public static ScreenKey Title = new("title");
    
        [UIScreenKey]
        public static ScreenKey InGame = new("inGame");
    
        [UIScreenKey]
        public static ScreenKey Dialogue = new("dialogue");
    }
}