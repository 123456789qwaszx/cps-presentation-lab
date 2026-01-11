#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class LabCommandMenuUtility
{
    private static readonly string[] _favoriteNames =
    {
        "ShowLineCommandSpec",
        "SetTextCommandSpec",
        "TypeTextCommandSpec",
        "FadeCommandSpec",
        "SlideInCommandSpec",
        "WaitCommandSpec",
        "RaiseSignalCommandSpec",
    };

    /// <summary>
    /// 외부에서 GenericMenu를 만들어 넘겨줄 때,
    /// 이 함수가 "커맨드 선택 항목"들을 채워준다.
    /// (ShowAsContext는 호출자 책임)
    /// </summary>
    public static void BuildCommandSelectionMenu(
        GenericMenu menu,
        IReadOnlyList<Type> allTypes,
        Action<Type> onSelected)
    {
        if (menu == null)
            return;

        if (allTypes == null || allTypes.Count == 0)
        {
            menu.AddDisabledItem(new GUIContent("No CommandSpecBase types found"));
            return;
        }

        var favorites = allTypes.Where(IsFavorite)
            .OrderBy(t => t.Name, StringComparer.Ordinal)
            .ToList();

        var others = allTypes.Where(t => !IsFavorite(t))
            .OrderBy(t => t.Name, StringComparer.Ordinal)
            .ToList();

        // 1) 즐겨쓰는 커맨드들 (Common/)
        foreach (var t in favorites)
        {
            string path = $"Common/{t.Name}";
            menu.AddItem(new GUIContent(path), false, () => onSelected?.Invoke(t));
        }

        if (favorites.Count > 0)
            menu.AddSeparator("Common/");

        // 2) 나머지 커맨드 분류
        foreach (var t in others)
        {
            string path = GetCategoryPath(t);
            menu.AddItem(new GUIContent(path), false, () => onSelected?.Invoke(t));
        }
    }

    /// <summary>
    /// 예전처럼 "메뉴 한 번 띄우고 끝" 쓰고 싶을 때 쓰는 편의 함수.
    /// </summary>
    public static void ShowCommandSelectionMenu(
        IReadOnlyList<Type> allTypes,
        Action<Type> onSelected)
    {
        var menu = new GenericMenu();
        BuildCommandSelectionMenu(menu, allTypes, onSelected);
        menu.ShowAsContext();
    }

    private static bool IsFavorite(Type t)
        => _favoriteNames.Any(n => string.Equals(n, t.Name, StringComparison.OrdinalIgnoreCase));

    private static string GetCategoryPath(Type t)
    {
        string name = t.Name;

        // 이름 패턴 기반으로 카테고리 분류 (원하는 대로 튜닝 가능)
        if (name.Contains("Text", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Line", StringComparison.OrdinalIgnoreCase))
            return $"Text/{name}";

        if (name.Contains("Sprite", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Image",  StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Emoji",  StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Color",  StringComparison.OrdinalIgnoreCase))
            return $"Visual/{name}";

        if (name.Contains("Move",    StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Slide",   StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Shake",   StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Punch",   StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Scale",   StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Rotation",StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Sway",    StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Drop",    StringComparison.OrdinalIgnoreCase))
            return $"Motion/{name}";

        if (name.Contains("Signal", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Wait",   StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Hold",   StringComparison.OrdinalIgnoreCase))
            return $"Flow/{name}";

        return $"Other/{name}";
    }
}
#endif
