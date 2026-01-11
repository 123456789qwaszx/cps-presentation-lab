#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
public static class LabSequenceEditorMenuInstaller
{
    static LabSequenceEditorMenuInstaller()
    {
        SequenceEditorMenuHooks.ShowCommandMenu =
            (allTypes, onSingle, onBatch, extendMenu) =>
            {
                var menu = new GenericMenu();

                // Lab 전용: Favorites + Category + Custom Sets(배치)
                LabCommandMenuUtility.BuildCommandSelectionMenu(
                    menu,
                    allTypes,
                    onSelectedSingle: onSingle,
                    onSelectedSet: onBatch,
                    showFavoritesOnlyInFavorites: true
                );

                // 에디터 공통 메뉴(Delete 등) 주입
                extendMenu?.Invoke(menu);

                menu.ShowAsContext();
                return true;
            };
    }
}
#endif