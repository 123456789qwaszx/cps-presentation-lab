using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum DialogueWidgetTarget
{
    // ---- Line / Text ----
    LineText,
    LineBodyImage,
    SpeakerNameText,

    // ---- Standing Portrait Set (PortraitSet v1) ----
    StandingPortraitRoot,
    
    StandingPortraitTrack,
    StandingPortraitRig,
    StandingPortraitSwayPivot,
    StandingPortraitShake,
    StandingPortraitScale,
    StandingPortraitVisual,

    StandingPortraitImage,        // actual Image (sprite swap / color)
    StandingPortraitEmojiAnchor,  // RectTransform anchor
    StandingPortraitEmojiImage,   // optional Image overlay (if you keep it as a widget)

    // ---- Protagonist Cutin (현재는 Image만 사용) ----
    ProtagonistCutinImage,

    // ---- Background Set ----
    BackgroundImage,
}

public interface IDialogueWidgetAccess
{
    bool TryResolve(string screenId, string widgetId, out WidgetRefs refs);

    public sealed class WidgetRefs
    {
        // ---- Line / Text ----
        public TMP_Text LineText;
        public Image    LineBodyImage;
        public TMP_Text SpeakerNameText;

        // ---- Standing Portrait Set ----
        public RectTransform StandingPortraitRoot; 
        public RectTransform StandingPortraitTrack;
        public RectTransform StandingPortraitRig;
        public RectTransform StandingPortraitSwayPivot;
        public RectTransform StandingPortraitShake;
        public RectTransform StandingPortraitScale;
        public RectTransform StandingPortraitVisual;

        public Image         StandingPortraitImage;
        public RectTransform StandingPortraitEmojiAnchor;
        public Image         StandingPortraitEmojiImage;

        // ---- Protagonist Cutin (Image만) ----
        public Image         ProtagonistCutinImage;

        // ---- Background Set ----
        public Image         BackgroundImage;
    }
}

public static class WidgetRefsExtensions
{
    public static Component GetComponent(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetTarget slot)
    {
        if (refs == null) return null;

        switch (slot)
        {
            // ---- Line / Text ----
            case DialogueWidgetTarget.LineText:         return refs.LineText;
            case DialogueWidgetTarget.LineBodyImage:    return refs.LineBodyImage;
            case DialogueWidgetTarget.SpeakerNameText:  return refs.SpeakerNameText;

            // ---- Standing Portrait Set ----
            case DialogueWidgetTarget.StandingPortraitRoot:        return refs.StandingPortraitRoot;
            case DialogueWidgetTarget.StandingPortraitTrack:       return refs.StandingPortraitTrack;
            case DialogueWidgetTarget.StandingPortraitRig:         return refs.StandingPortraitRig;
            case DialogueWidgetTarget.StandingPortraitSwayPivot:   return refs.StandingPortraitSwayPivot;
            case DialogueWidgetTarget.StandingPortraitShake:       return refs.StandingPortraitShake;
            case DialogueWidgetTarget.StandingPortraitScale:       return refs.StandingPortraitScale;
            case DialogueWidgetTarget.StandingPortraitVisual:      return refs.StandingPortraitVisual;

            case DialogueWidgetTarget.StandingPortraitImage:       return refs.StandingPortraitImage;
            case DialogueWidgetTarget.StandingPortraitEmojiAnchor: return refs.StandingPortraitEmojiAnchor;
            case DialogueWidgetTarget.StandingPortraitEmojiImage:  return refs.StandingPortraitEmojiImage;

            // ---- Protagonist Cutin (Image만) ----
            case DialogueWidgetTarget.ProtagonistCutinImage:       return refs.ProtagonistCutinImage;

            // ---- Background Set ----
            case DialogueWidgetTarget.BackgroundImage:             return refs.BackgroundImage;

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
