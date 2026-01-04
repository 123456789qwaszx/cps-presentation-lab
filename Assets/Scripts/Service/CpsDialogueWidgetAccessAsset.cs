using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Lab.UI.Naming;

[CreateAssetMenu(fileName = "CpsDialogueWidgetAccess", menuName = "Dialogue/Services/CPS Widget Access")]
public sealed class CpsDialogueWidgetAccessAsset : ScriptableObject, IDialogueWidgetAccess
{
    public bool TryResolve(string screenId, string widgetId, out IDialogueWidgetAccess.WidgetRefs refs)
    {
        refs = null;

        UIRouter router = UIRuntimeRouter.Router;
        if (router == null)
        {
            Debug.LogWarning("[CpsDialogueWidgetAccess] UIRuntimeRouter. Router is null. ");
            return false;
        }

        ScreenKey key = new ScreenKey(screenId);
        if (!router.TryGetScreen(key, out UIScreen screen))
        {
            Debug.LogWarning(
                $"[CpsDialogueWidgetAccess] Failed to resolve UIScreen. screenId='{screenId}', ScreenKey='{key}'");
            return false;
        }
        
        DialogueWidgetSet set = DialogueWidgetSets.Dialogue;
        WidgetHandle body     = screen.GetWidgetHandle(set.BodyName);
        WidgetHandle name     = screen.GetWidgetHandle(set.NameName);
        WidgetHandle portrait = screen.GetWidgetHandle(set.PortraitName);

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