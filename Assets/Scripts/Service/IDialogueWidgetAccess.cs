using UnityEngine;
using TMPro;
using UnityEngine.UI;

public interface IDialogueWidgetAccess
{
    bool TryResolve(string screenId, string widgetId, out WidgetRefs refs);

    public sealed class WidgetRefs
    {
        public TMP_Text BodyText;
        public TMP_Text NameText;
        public Image PortraitImage;
        public RectTransform PortraitRect;
        public Graphic PortraitGraphic; // fade용
    }
}