#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public sealed class UIRouteKeyAttribute : PropertyAttribute
{
}

[CustomPropertyDrawer(typeof(UIRouteKeyAttribute))]
public sealed class UIRouteKeyDrawer : PropertyDrawer
{
    private string[] _options;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (_options == null)
        {
            _options = UIRoutes.All;
        }

        string current = property.stringValue;
        int currentIndex = System.Array.IndexOf(_options, current);
        if (currentIndex < 0) currentIndex = 0;

        int newIndex = EditorGUI.Popup(position, label.text, currentIndex, _options);

        if (newIndex >= 0 && newIndex < _options.Length)
        {
            property.stringValue = _options[newIndex];
        }
    }
}
#endif