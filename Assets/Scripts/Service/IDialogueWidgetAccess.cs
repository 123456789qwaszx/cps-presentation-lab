using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum DialogueWidgetSlot
{
    LineText,
    SpeakerNameText,
    StandingPortraitImage,
    StandingPortraitRect,
    ProtagonistCutinImage,
    ProtagonistCutinRect
}

public interface IDialogueWidgetAccess
{
    bool TryResolve(string screenId, string widgetId, out WidgetRefs refs);

    public sealed class WidgetRefs
    {
        public TMP_Text BodyText;
        public TMP_Text NameText;
        public Image PortraitImage;
        public RectTransform PortraitRect;
        public Image EmojiImage;
        public RectTransform EmojiRect;
    }
}

public static class WidgetRefsExtensions
{
    public static Component GetComponent(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetSlot slot)
    {
        if (refs == null) return null;

        switch (slot)
        {
            case DialogueWidgetSlot.LineText:
                return refs.BodyText;
            case DialogueWidgetSlot.SpeakerNameText:
                return refs.NameText;
            case DialogueWidgetSlot.StandingPortraitImage:
                return refs.PortraitImage;
            case DialogueWidgetSlot.StandingPortraitRect:
                return refs.PortraitRect;
            case DialogueWidgetSlot.ProtagonistCutinImage:
                return refs.EmojiImage;
            case DialogueWidgetSlot.ProtagonistCutinRect:
                return refs.EmojiRect;
            default:
                return null;
        }
    }

    public static Graphic GetGraphic(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetSlot slot)
    {
        var c = refs.GetComponent(slot);
        return c as Graphic;
    }

    public static GameObject GetGameObject(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetSlot slot)
    {
        var c = refs.GetComponent(slot);
        return c != null ? c.gameObject : null;
    }

    public static TMP_Text GetText(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetSlot slot)
    {
        return refs.GetComponent(slot) as TMP_Text;
    }
}