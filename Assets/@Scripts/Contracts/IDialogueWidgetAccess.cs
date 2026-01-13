using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum DialogueWidgetTarget
{
    // ---- Line / Text ----
    LineText,
    LineBodyImage,
    SpeakerNameBox,
    SpeakerNameText,
    ProtagonistCutinImage,

    // ---- Dialogue Box Root ----
    DialogueBoxRoot,

    // ---- Main Standing Portrait Set ----
    MainStandingPortraitRoot,
    MainStandingPortraitTrack,
    MainStandingPortraitRig,
    MainStandingPortraitSwayPivot,
    MainStandingPortraitShake,
    MainStandingPortraitScale,
    MainStandingPortraitVisual,

    MainStandingPortraitImage,        // actual Image (sprite swap / color)
    MainStandingPortraitEmojiAnchor,  // RectTransform anchor
    MainStandingPortraitEmojiImage,   // optional Image overlay

    // ---- SubLeft Standing Portrait Set ----
    SubLeftStandingPortraitRoot,
    SubLeftStandingPortraitTrack,
    SubLeftStandingPortraitRig,
    SubLeftStandingPortraitSwayPivot,
    SubLeftStandingPortraitShake,
    SubLeftStandingPortraitScale,
    SubLeftStandingPortraitVisual,

    SubLeftStandingPortraitImage,
    SubLeftStandingPortraitEmojiAnchor,
    SubLeftStandingPortraitEmojiImage,

    // ---- SubRight Standing Portrait Set ----
    SubRightStandingPortraitRoot,
    SubRightStandingPortraitTrack,
    SubRightStandingPortraitRig,
    SubRightStandingPortraitSwayPivot,
    SubRightStandingPortraitShake,
    SubRightStandingPortraitScale,
    SubRightStandingPortraitVisual,

    SubRightStandingPortraitImage,
    SubRightStandingPortraitEmojiAnchor,
    SubRightStandingPortraitEmojiImage,


    // ---- Background Set ----
    BackgroundImage,
    BackgroundImage2,
    BackgroundImage3,

    // ---- Choice Panel + 3 Choices ----
    ChoicePanelRoot,
    ChoiceButton0Root,
    ChoiceButton1Root,
    ChoiceButton2Root,
}

public interface IDialogueWidgetAccess
{
    bool TryResolve(string screenId, string widgetId, out WidgetRefs refs);

    public sealed class WidgetRefs
    {
        // ---- Line / Text ----
        public TMP_Text LineText;
        public Image LineBodyImage;
        public Image SpeakerNameBox;
        public TMP_Text SpeakerNameText;
        public Image ProtagonistCutinImage;

        // ---- Dialogue Box ----
        public RectTransform DialogueBoxRoot;

        // ---- Main Standing Portrait Set ----
        public RectTransform MainStandingPortraitRoot;
        public RectTransform MainStandingPortraitTrack;
        public RectTransform MainStandingPortraitRig;
        public RectTransform MainStandingPortraitSwayPivot;
        public RectTransform MainStandingPortraitShake;
        public RectTransform MainStandingPortraitScale;
        public RectTransform MainStandingPortraitVisual;

        public Image MainStandingPortraitImage;
        public RectTransform MainStandingPortraitEmojiAnchor;
        public Image MainStandingPortraitEmojiImage;

        // ---- SubLeft Standing Portrait Set ----
        public RectTransform SubLeftStandingPortraitRoot;
        public RectTransform SubLeftStandingPortraitTrack;
        public RectTransform SubLeftStandingPortraitRig;
        public RectTransform SubLeftStandingPortraitSwayPivot;
        public RectTransform SubLeftStandingPortraitShake;
        public RectTransform SubLeftStandingPortraitScale;
        public RectTransform SubLeftStandingPortraitVisual;

        public Image SubLeftStandingPortraitImage;
        public RectTransform SubLeftStandingPortraitEmojiAnchor;
        public Image SubLeftStandingPortraitEmojiImage;

        // ---- SubRight Standing Portrait Set ----
        public RectTransform SubRightStandingPortraitRoot;
        public RectTransform SubRightStandingPortraitTrack;
        public RectTransform SubRightStandingPortraitRig;
        public RectTransform SubRightStandingPortraitSwayPivot;
        public RectTransform SubRightStandingPortraitShake;
        public RectTransform SubRightStandingPortraitScale;
        public RectTransform SubRightStandingPortraitVisual;

        public Image SubRightStandingPortraitImage;
        public RectTransform SubRightStandingPortraitEmojiAnchor;
        public Image SubRightStandingPortraitEmojiImage;

        // ---- Background Set (3장) ----
        public Image BackgroundImage;
        public Image BackgroundImage2;
        public Image BackgroundImage3;

        // ---- Choice Panel + 3 Choices ----
        public RectTransform ChoicePanelRoot;
        public RectTransform ChoiceButton0Root;
        public RectTransform ChoiceButton1Root;
        public RectTransform ChoiceButton2Root;
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
            case DialogueWidgetTarget.SpeakerNameBox:     return refs.SpeakerNameBox;
            case DialogueWidgetTarget.SpeakerNameText:  return refs.SpeakerNameText;
            case DialogueWidgetTarget.ProtagonistCutinImage:       return refs.ProtagonistCutinImage;

            // ---- Dialogue Box ----
            case DialogueWidgetTarget.DialogueBoxRoot:  return refs.DialogueBoxRoot;

            // ---- Main Standing Portrait Set ----
            case DialogueWidgetTarget.MainStandingPortraitRoot:        return refs.MainStandingPortraitRoot;
            case DialogueWidgetTarget.MainStandingPortraitTrack:       return refs.MainStandingPortraitTrack;
            case DialogueWidgetTarget.MainStandingPortraitRig:         return refs.MainStandingPortraitRig;
            case DialogueWidgetTarget.MainStandingPortraitSwayPivot:   return refs.MainStandingPortraitSwayPivot;
            case DialogueWidgetTarget.MainStandingPortraitShake:       return refs.MainStandingPortraitShake;
            case DialogueWidgetTarget.MainStandingPortraitScale:       return refs.MainStandingPortraitScale;
            case DialogueWidgetTarget.MainStandingPortraitVisual:      return refs.MainStandingPortraitVisual;

            case DialogueWidgetTarget.MainStandingPortraitImage:       return refs.MainStandingPortraitImage;
            case DialogueWidgetTarget.MainStandingPortraitEmojiAnchor: return refs.MainStandingPortraitEmojiAnchor;
            case DialogueWidgetTarget.MainStandingPortraitEmojiImage:  return refs.MainStandingPortraitEmojiImage;

            // ---- SubLeft Standing Portrait Set ----
            case DialogueWidgetTarget.SubLeftStandingPortraitRoot:        return refs.SubLeftStandingPortraitRoot;
            case DialogueWidgetTarget.SubLeftStandingPortraitTrack:       return refs.SubLeftStandingPortraitTrack;
            case DialogueWidgetTarget.SubLeftStandingPortraitRig:         return refs.SubLeftStandingPortraitRig;
            case DialogueWidgetTarget.SubLeftStandingPortraitSwayPivot:   return refs.SubLeftStandingPortraitSwayPivot;
            case DialogueWidgetTarget.SubLeftStandingPortraitShake:       return refs.SubLeftStandingPortraitShake;
            case DialogueWidgetTarget.SubLeftStandingPortraitScale:       return refs.SubLeftStandingPortraitScale;
            case DialogueWidgetTarget.SubLeftStandingPortraitVisual:      return refs.SubLeftStandingPortraitVisual;

            case DialogueWidgetTarget.SubLeftStandingPortraitImage:       return refs.SubLeftStandingPortraitImage;
            case DialogueWidgetTarget.SubLeftStandingPortraitEmojiAnchor: return refs.SubLeftStandingPortraitEmojiAnchor;
            case DialogueWidgetTarget.SubLeftStandingPortraitEmojiImage:  return refs.SubLeftStandingPortraitEmojiImage;

            // ---- SubRight Standing Portrait Set ----
            case DialogueWidgetTarget.SubRightStandingPortraitRoot:        return refs.SubRightStandingPortraitRoot;
            case DialogueWidgetTarget.SubRightStandingPortraitTrack:       return refs.SubRightStandingPortraitTrack;
            case DialogueWidgetTarget.SubRightStandingPortraitRig:         return refs.SubRightStandingPortraitRig;
            case DialogueWidgetTarget.SubRightStandingPortraitSwayPivot:   return refs.SubRightStandingPortraitSwayPivot;
            case DialogueWidgetTarget.SubRightStandingPortraitShake:       return refs.SubRightStandingPortraitShake;
            case DialogueWidgetTarget.SubRightStandingPortraitScale:       return refs.SubRightStandingPortraitScale;
            case DialogueWidgetTarget.SubRightStandingPortraitVisual:      return refs.SubRightStandingPortraitVisual;

            case DialogueWidgetTarget.SubRightStandingPortraitImage:       return refs.SubRightStandingPortraitImage;
            case DialogueWidgetTarget.SubRightStandingPortraitEmojiAnchor: return refs.SubRightStandingPortraitEmojiAnchor;
            case DialogueWidgetTarget.SubRightStandingPortraitEmojiImage:  return refs.SubRightStandingPortraitEmojiImage;


            // ---- Background Set ----
            case DialogueWidgetTarget.BackgroundImage:             return refs.BackgroundImage;
            case DialogueWidgetTarget.BackgroundImage2:            return refs.BackgroundImage2;
            case DialogueWidgetTarget.BackgroundImage3:            return refs.BackgroundImage3;

            // ---- Choice Panel + 3 Choices ----
            case DialogueWidgetTarget.ChoicePanelRoot:             return refs.ChoicePanelRoot;
            case DialogueWidgetTarget.ChoiceButton0Root:           return refs.ChoiceButton0Root;
            case DialogueWidgetTarget.ChoiceButton1Root:           return refs.ChoiceButton1Root;
            case DialogueWidgetTarget.ChoiceButton2Root:           return refs.ChoiceButton2Root;

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
