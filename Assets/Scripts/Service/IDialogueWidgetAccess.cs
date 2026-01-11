using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum DialogueWidgetTarget
{
    LineText,
    LineBodyImage,
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
        public TMP_Text LineText;
        public Image LineBodyImage;
        public TMP_Text SpeakerNameText;
        public Image StandingPortraitImage;
        public RectTransform StandingPortraitRect;
        public Image ProtagonistCutinImage;
        public RectTransform ProtagonistCutinRect;
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
                return refs.LineText;
            case DialogueWidgetTarget.LineBodyImage:
                return refs.LineBodyImage;
            case DialogueWidgetTarget.SpeakerNameText:
                return refs.SpeakerNameText;
            case DialogueWidgetTarget.StandingPortraitImage:
                return refs.StandingPortraitImage;
            case DialogueWidgetTarget.StandingPortraitRect:
                return refs.StandingPortraitRect;
            case DialogueWidgetTarget.ProtagonistCutinImage:
                return refs.ProtagonistCutinImage;
            case DialogueWidgetTarget.ProtagonistCutinRect:
                return refs.ProtagonistCutinRect;
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