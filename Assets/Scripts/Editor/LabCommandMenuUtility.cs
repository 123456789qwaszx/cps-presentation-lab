#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class LabCommandMenuUtility
{
    private sealed class MenuItemInfo
    {
        public Type Type;
        public CommandMenuHintAttribute Hint;
        public string Category;
        public string Label;
        public int Order;
        public bool Favorite;

        public string[] Sets;
        public int SetOrder;
    }

    public static void BuildCommandSelectionMenu(
    GenericMenu menu,
    IReadOnlyList<Type> allTypes,
    Action<Type> onSelectedSingle,
    Action<IReadOnlyList<Type>> onSelectedSet,
    bool showFavoritesOnlyInFavorites = true)
{
    if (menu == null) throw new ArgumentNullException(nameof(menu));

    if (allTypes == null || allTypes.Count == 0)
    {
        menu.AddDisabledItem(new GUIContent("No CommandSpecBase types found"));
        return;
    }

    var items = allTypes
        .Where(t => t != null && !t.IsAbstract)
        .Select(t =>
        {
            var hint = t.GetCustomAttribute<CommandMenuHintAttribute>();

            return new MenuItemInfo
            {
                Type     = t,
                Hint     = hint,
                Category = (hint?.Category ?? "Other").Trim(),
                Label    = (hint?.DisplayName ?? t.Name).Trim(),
                Order    = hint?.Order ?? 0,
                Favorite = hint?.Favorite ?? false,
                Sets     = hint?.Sets,
                SetOrder = hint?.SetOrder ?? 0
            };
        })
        .ToList();

    // 타입 -> Item 맵 (Recent에서 Label 찾을 때 사용)
    var itemByType = new Dictionary<Type, MenuItemInfo>();
    foreach (var it in items)
    {
        if (it.Type != null && !itemByType.ContainsKey(it.Type))
            itemByType.Add(it.Type, it);
    }

    // ------------------------------------------------------------
    // 0) 세트 메뉴: Custom/PortraitStart 같은 “매크로” 항목
    // ------------------------------------------------------------
    var setMap = new Dictionary<string, List<MenuItemInfo>>(StringComparer.OrdinalIgnoreCase);

    foreach (var it in items)
    {
        if (it.Sets == null) continue;

        foreach (var setPathRaw in it.Sets)
        {
            var setPath = (setPathRaw ?? "").Trim();
            if (string.IsNullOrEmpty(setPath)) continue;

            if (!setMap.TryGetValue(setPath, out var list))
            {
                list = new List<MenuItemInfo>();
                setMap[setPath] = list;
            }
            list.Add(it);
        }
    }

    if (setMap.Count > 0)
    {
        foreach (var kv in setMap.OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase))
        {
            string setPath = kv.Key;
            var list = kv.Value
                .OrderBy(x => x.SetOrder)
                .ThenBy(x => x.Label, StringComparer.OrdinalIgnoreCase)
                .ToList();

            // ex) Custom/PortraitStart (Add 5)
            string leaf = $"{setPath} (Add {list.Count})";
            menu.AddItem(new GUIContent(leaf), false, () =>
            {
                // 세트에 포함된 타입들도 전부 Recent에 기록
                foreach (var it in list)
                    LabCommandRecentRegistry.Record(it.Type);

                onSelectedSet?.Invoke(list.Select(x => x.Type).ToList());
            });

            // 옵션: 세트 안에 개별 항목도 넣으려면 여기에 추가
            menu.AddSeparator(setPath + "/");
        }

        // 세트 섹션과 아래 섹션들 사이에 구분선
        menu.AddSeparator("");
    }

    // ------------------------------------------------------------
    // 0.5) Recent 섹션
    // ------------------------------------------------------------
    var recentTypes = LabCommandRecentRegistry.GetRecentTypes(allTypes);

    // Recent 에서 실제로 존재하는 타입만 필터
    var recentItems = recentTypes
        .Select(t => itemByType.TryGetValue(t, out var info) ? info : null)
        .Where(info => info != null)
        .ToList();

    if (recentItems.Count > 0)
    {
        foreach (var i in recentItems)
        {
            string path = $"Recent/{i.Label}";
            menu.AddItem(new GUIContent(path), false, () =>
            {
                LabCommandRecentRegistry.Record(i.Type);
                onSelectedSingle?.Invoke(i.Type);
            });
        }

        // Recent 서브메뉴 안에서 구분선
        menu.AddSeparator("Recent/");

        // Recent 섹션과 그 아래(Favorites or Category) 사이 구분선
        menu.AddSeparator("");
    }

    // ------------------------------------------------------------
    // 1) Favorites 섹션 (원하면 나중에 제거 가능)
    // ------------------------------------------------------------
    var favorites = items
        .Where(i => i.Favorite)
        .OrderBy(i => i.Order)
        .ThenBy(i => i.Label, StringComparer.OrdinalIgnoreCase)
        .ToList();

    foreach (var i in favorites)
    {
        string path = $"Favorites/{i.Label}";
        menu.AddItem(new GUIContent(path), false, () =>
        {
            LabCommandRecentRegistry.Record(i.Type);
            onSelectedSingle?.Invoke(i.Type);
        });
    }

    if (favorites.Count > 0)
        menu.AddSeparator("Favorites/");

    // ------------------------------------------------------------
    // 2) Category 메뉴
    // ------------------------------------------------------------
    var groups = items
        .Where(i => !showFavoritesOnlyInFavorites || !i.Favorite)
        .GroupBy(i => string.IsNullOrEmpty(i.Category) ? "Other" : i.Category)
        .OrderBy(g => string.Equals(g.Key, "Other", StringComparison.OrdinalIgnoreCase) ? 1 : 0)
        .ThenBy(g => g.Key, StringComparer.OrdinalIgnoreCase)
        .ToList();

    foreach (var g in groups)
    {
        foreach (var i in g
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Label, StringComparer.OrdinalIgnoreCase))
        {
            string path = $"{g.Key}/{i.Label}";
            menu.AddItem(new GUIContent(path), false, () =>
            {
                LabCommandRecentRegistry.Record(i.Type);
                onSelectedSingle?.Invoke(i.Type);
            });
        }
    }
}

}
#endif
