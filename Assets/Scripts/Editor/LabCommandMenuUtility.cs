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

        // ------------------------------------------------------------
        // 0) ✅ 세트 메뉴: Custom/PortraitStart 같은 “매크로” 항목 만들기
        // ------------------------------------------------------------
        // setPath -> 포함될 커맨드들
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
            // 세트는 메뉴 상단에 배치하는 게 체감 좋음
            // 세트 이름 정렬
            foreach (var kv in setMap.OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase))
            {
                string setPath = kv.Key;
                var list = kv.Value
                    .OrderBy(x => x.SetOrder)
                    .ThenBy(x => x.Label, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                // ✅ 클릭 가능한 “세트 추가” 항목(리프)
                // ex) Custom/PortraitStart (Add 5)
                string leaf = $"{setPath} (Add {list.Count})";
                menu.AddItem(new GUIContent(leaf), false, () =>
                {
                    onSelectedSet?.Invoke(list.Select(x => x.Type).ToList());
                });

                // (옵션) 세트 안에 개별 항목도 보여주고 싶으면 아래 같이 추가 가능
                // foreach (var it in list) menu.AddItem(new GUIContent($"{setPath}/- {it.Label}"), false, () => onSelectedSingle?.Invoke(it.Type));

                menu.AddSeparator(setPath + "/");
            }

            menu.AddSeparator("");
        }

        // ------------------------------------------------------------
        // 1) Favorites
        // ------------------------------------------------------------
        var favorites = items
            .Where(i => i.Favorite)
            .OrderBy(i => i.Order)
            .ThenBy(i => i.Label, StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var i in favorites)
        {
            string path = $"Favorites/{i.Label}";
            menu.AddItem(new GUIContent(path), false, () => onSelectedSingle?.Invoke(i.Type));
        }

        if (favorites.Count > 0)
            menu.AddSeparator("Favorites/");

        // ------------------------------------------------------------
        // 2) Category 메뉴(기존)
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
                menu.AddItem(new GUIContent(path), false, () => onSelectedSingle?.Invoke(i.Type));
            }
        }
    }
}
#endif
