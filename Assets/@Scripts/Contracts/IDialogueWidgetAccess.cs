using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum DialogueWidgetTarget
{
    None = -1,
    // ---- Dialogue Line ----
    DialogueBoxRoot,
    LineText,
    LineBodyImage,
    SpeakerNameBox,
    SpeakerNameText,
    ProtagonistCutinImage,

    // ---- Main Standing Portrait Set ----
    MainStandingPortraitRoot,
    MainStandingPortraitTrack,
    MainStandingPortraitRig,
    MainStandingPortraitSwayPivot,
    MainStandingPortraitShake,
    MainStandingPortraitScale,
    MainStandingPortraitVisual,
    MainStandingPortraitImage,
    MainStandingPortraitEmojiAnchor,
    MainStandingPortraitEmojiImage,

    // ---- SubLeft Standing Portrait ----
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

    // ---- SubRight Standing Portrait ----
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
    
    // ---- Background ----
    BackgroundImage0,
    BackgroundImage1,

    // ---- Choice Panel----
    ChoicePanelRoot,
    ChoiceButton0Root,
    ChoiceButton1Root,
    ChoiceButton2Root,
    ChoiceButton0Text,
    ChoiceButton1Text,
    ChoiceButton2Text,
}

public interface IDialogueWidgetAccess
{
    bool TryResolve(string screenId, string widgetId, out WidgetRefs refs);

    public sealed class WidgetRefs
    {
        // ---- Dialogue Line ----
        public RectTransform DialogueBoxRoot;
        public TMP_Text LineText;
        public Image LineBodyImage;
        public Image SpeakerNameBox;
        public TMP_Text SpeakerNameText;
        public Image ProtagonistCutinImage;

        // ---- Main Standing Portrait ----
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

        // ---- SubLeft Standing Portrait ----
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

        // ---- SubRight Standing Portrait ----
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

        // ---- Background ----
        public Image BackgroundImage0;
        public Image BackgroundImage1;

        // ---- Choice Panel ----
        public RectTransform ChoicePanelRoot;
        public Image ChoiceButton0Root;
        public Image ChoiceButton1Root;
        public Image ChoiceButton2Root;
        public TMP_Text ChoiceButton0Text;
        public TMP_Text ChoiceButton1Text;
        public TMP_Text ChoiceButton2Text;
    }
}

public static class WidgetRefsExtensions
{
    public static Component GetComponent(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetTarget slot)
    {
        if (refs == null) return null;

        switch (slot)
        {
            // ---- Dialogue Line ----
            case DialogueWidgetTarget.DialogueBoxRoot:       return refs.DialogueBoxRoot;
            case DialogueWidgetTarget.LineText:              return refs.LineText;
            case DialogueWidgetTarget.LineBodyImage:         return refs.LineBodyImage;
            case DialogueWidgetTarget.SpeakerNameBox:        return refs.SpeakerNameBox;
            case DialogueWidgetTarget.SpeakerNameText:       return refs.SpeakerNameText;
            case DialogueWidgetTarget.ProtagonistCutinImage: return refs.ProtagonistCutinImage;

            // ---- Main Standing Portrait ---
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

            // ---- SubLeft Standing Portrait ----
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

            // ---- SubRight Standing Portrait----
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
            
            // ---- Background ----
            case DialogueWidgetTarget.BackgroundImage0: return refs.BackgroundImage0;
            case DialogueWidgetTarget.BackgroundImage1: return refs.BackgroundImage1;

            // ---- Choice Panel ----
            case DialogueWidgetTarget.ChoicePanelRoot:   return refs.ChoicePanelRoot;
            case DialogueWidgetTarget.ChoiceButton0Root: return refs.ChoiceButton0Root;
            case DialogueWidgetTarget.ChoiceButton1Root: return refs.ChoiceButton1Root;
            case DialogueWidgetTarget.ChoiceButton2Root: return refs.ChoiceButton2Root;
            case DialogueWidgetTarget.ChoiceButton0Text: return refs.ChoiceButton0Text;
            case DialogueWidgetTarget.ChoiceButton1Text: return refs.ChoiceButton1Text;
            case DialogueWidgetTarget.ChoiceButton2Text: return refs.ChoiceButton2Text;

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
