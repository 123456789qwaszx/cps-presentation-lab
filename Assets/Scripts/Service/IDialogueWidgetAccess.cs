using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum DialogueWidgetTarget
{
    LineText,
    SpeakerNameText,
    StandingPortraitImage,
    StandingPortraitRect,
    ProtagonistCutinImage,
    ProtagonistCutinRect,
    BackgroundImage,
    BackgroundRect
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
        public Image BackgroundImage;
        public RectTransform BackgroundRect;
        
    }
}

public static class WidgetRefsExtensions
{
    public static Component GetComponent(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetTarget slot)
    {
        if (refs == null) return null;

        switch (slot)
        {
            case DialogueWidgetTarget.LineText:
                return refs.BodyText;
            case DialogueWidgetTarget.SpeakerNameText:
                return refs.NameText;
            case DialogueWidgetTarget.StandingPortraitImage:
                return refs.PortraitImage;
            case DialogueWidgetTarget.StandingPortraitRect:
                return refs.PortraitRect;
            case DialogueWidgetTarget.ProtagonistCutinImage:
                return refs.EmojiImage;
            case DialogueWidgetTarget.ProtagonistCutinRect:
                return refs.EmojiRect;
            case DialogueWidgetTarget.BackgroundImage:
                return refs.BackgroundImage;
            case DialogueWidgetTarget.BackgroundRect:
                return refs.BackgroundRect;
            default:
                return null;
        }
    }

    public static Graphic GetGraphic(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetTarget slot)
    {
        var c = refs.GetComponent(slot);
        return c as Graphic;
    }

    public static GameObject GetGameObject(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetTarget slot)
    {
        var c = refs.GetComponent(slot);
        return c != null ? c.gameObject : null;
    }

    public static TMP_Text GetText(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetTarget slot)
    {
        return refs.GetComponent(slot) as TMP_Text;
    }
    
    public static RectTransform GetRect(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetTarget target)
    {
        var c = refs.GetComponent(target);
        if (c == null) return null;

        if (c is RectTransform rt)
            return rt;

        if (c is Graphic g)
            return g.rectTransform;

        return c.transform as RectTransform;
    }
}