using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CpsDialogueWidgetAccess", menuName = "Dialogue/Services/CPS Widget Access")]
public sealed class CpsDialogueWidgetAccessAsset : ScriptableObject, IDialogueWidgetAccess
{
    public bool TryResolve(string screenId, string widgetId, out IDialogueWidgetAccess.WidgetRefs refs)
    {
        refs = null;

        UIRouter router = UIRuntimeRouter.Router;
        if (router == null) return false;

        ScreenKey key = new ScreenKey(screenId);
        if (!router.TryGetScreen(key, out UIScreen screen) || screen == null) return false;

        var body = screen.GetWidgetHandle(widgetId + "_Body");
        var name = screen.GetWidgetHandle(widgetId + "_Name");
        var portrait = screen.GetWidgetHandle(widgetId + "_Portrait");

        refs = new IDialogueWidgetAccess.WidgetRefs
        {
            BodyText = body?.Text,
            NameText = name?.Text,
            PortraitImage = portrait?.Image,
            PortraitRect = portrait?.RectTransform,
            PortraitGraphic = portrait?.Image // Image는 Graphic 상속
        };

        return true;
    }

    public bool TryGetGraphic(string widgetId, out Graphic g)
    {
        g = null;
        return false;
    }

    public bool TryGetRectTransform(string widgetId, out RectTransform rt)
    {
        rt = null;
        return false;
    }
    public bool TryGetTMPText(string widgetId, out TMP_Text text)
    {
        text = null;
        return false;
    }
}