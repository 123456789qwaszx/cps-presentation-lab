using System.Collections.Generic;

public static class UIRoutes
{
    // Navigation
    public const string NavHome         = "nav/home";
    public const string NavShop         = "nav/shop";
    public const string NavClickerTitle = "nav/clickerTitle";
    public const string NavInGame       = "nav/inGame";

    // HUD
    public const string UiGold = "ui/gold";
    public const string UiHp   = "ui/hp";
    public const string UiGem  = "ui/gem";

#if UNITY_EDITOR
    // 에디터용 전체 목록
    public static readonly string[] All;

    static UIRoutes()
    {
        var fields = typeof(UIRoutes).GetFields(
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.FlattenHierarchy);

        var list = new List<string>();
        foreach (var f in fields)
        {
            if (f.FieldType == typeof(string))
                list.Add((string)f.GetValue(null));
        }

        All = list.ToArray();
    }
#endif
}