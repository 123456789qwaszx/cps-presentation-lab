using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum DialogueWidgetTarget
{
    None = -1,

    // ======================================================
    // Dialogue Line
    // ======================================================
    DialogueBoxRoot       = 0,
    LineText              = 10,
    LineBodyImage         = 20,
    SpeakerNameBox        = 30,
    SpeakerNameText       = 40,
    ProtagonistCutinImage = 50,

    // ======================================================
    // Main Standing Portrait Set (500 ~ 590)
    // ======================================================
    MainStandingPortraitRoot        = 500,
    MainStandingPortraitTrack       = 510,
    MainStandingPortraitRig         = 520,
    MainStandingPortraitSwayPivot   = 530,
    MainStandingPortraitShake       = 540,
    MainStandingPortraitScale       = 550,
    MainStandingPortraitVisual      = 560,
    MainStandingPortraitImage       = 570,
    MainStandingPortraitEmojiAnchor = 580,
    MainStandingPortraitEmojiImage  = 590,

    // ======================================================
    // SubLeft Standing Portrait (1000 ~ 1090)
    // ======================================================
    SubLeftStandingPortraitRoot        = 1000,
    SubLeftStandingPortraitTrack       = 1010,
    SubLeftStandingPortraitRig         = 1020,
    SubLeftStandingPortraitSwayPivot   = 1030,
    SubLeftStandingPortraitShake       = 1040,
    SubLeftStandingPortraitScale       = 1050,
    SubLeftStandingPortraitVisual      = 1060,
    SubLeftStandingPortraitImage       = 1070,
    SubLeftStandingPortraitEmojiAnchor = 1080,
    SubLeftStandingPortraitEmojiImage  = 1090,

    // ======================================================
    // SubRight Standing Portrait (1500 ~ 1590)
    // ======================================================
    SubRightStandingPortraitRoot        = 1500,
    SubRightStandingPortraitTrack       = 1510,
    SubRightStandingPortraitRig         = 1520,
    SubRightStandingPortraitSwayPivot   = 1530,
    SubRightStandingPortraitShake       = 1540,
    SubRightStandingPortraitScale       = 1550,
    SubRightStandingPortraitVisual      = 1560,
    SubRightStandingPortraitImage       = 1570,
    SubRightStandingPortraitEmojiAnchor = 1580,
    SubRightStandingPortraitEmojiImage  = 1590,

    // ======================================================
    // Background (2000 ~ 2030)  ✅ 00/01
    // ======================================================
    BackgroundRoot00  = 2000,
    BackgroundRoot01  = 2010,
    BackgroundImage00 = 2020,
    BackgroundImage01 = 2030,

    // (선택) 기존 이름 참조가 남아있다면 컴파일 깨지지 않게 alias 제공
    // BackgroundRoot0  = BackgroundRoot00,
    // BackgroundRoot1  = BackgroundRoot01,
    // BackgroundImage0 = BackgroundImage00,
    // BackgroundImage1 = BackgroundImage01,

    // ======================================================
    // Choice Panel (3000 ~ 3090)
    // ======================================================
    ChoicePanelRoot    = 3000,
    ChoiceButton0Root  = 3010,
    ChoiceButton1Root  = 3020,
    ChoiceButton2Root  = 3030,

    ChoiceButton0Text  = 3040,
    ChoiceButton1Text  = 3050,
    ChoiceButton2Text  = 3060,

    // ✅ Image 추가
    ChoiceButton0Image = 3070,
    ChoiceButton1Image = 3080,
    ChoiceButton2Image = 3090,

    // ======================================================
    // VFX / Toggle Panel (4000 ~ 4220)
    // ======================================================
    VFXBlackScreen00Root = 4000,
    VFXBlackOut00Image   = 4010,
    VFXBlackOut01Image   = 4020,

    BlackFade00Root      = 4030,
    BlackFade00Image     = 4040,

    SkipToggle00Root     = 4050,
    SkipToggle00Image    = 4060,
    SkipToggle01Image    = 4070,
    SkipToggle00Text     = 4080,

    NextToggle00Root     = 4090,
    NextToggle00Image    = 4100,
    NextToggle00Text     = 4110,

    AutoToggle00Root     = 4120,
    AutoToggle00Image    = 4130,
    AutoToggle01Image    = 4140,

    SpeedupToggle00Root  = 4150,
    SpeedupToggle00Image = 4160,
    SpeedupToggle01Image = 4170,

    SetSpeedToggle00Root  = 4180,
    SetSpeedToggle00Image = 4190,
    SetSpeedToggle01Image = 4200,
    SetSpeedToggle02Image = 4210,
    SetSpeedToggle03Image = 4220,
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

        // ---- Background (00/01) ----
        public RectTransform BackgroundRoot00;
        public RectTransform BackgroundRoot01;
        public Image BackgroundImage00;
        public Image BackgroundImage01;

        // ---- Choice Panel ----
        public RectTransform ChoicePanelRoot;
        public RectTransform ChoiceButton0Root;
        public RectTransform ChoiceButton1Root;
        public RectTransform ChoiceButton2Root;

        public TMP_Text ChoiceButton0Text;
        public TMP_Text ChoiceButton1Text;
        public TMP_Text ChoiceButton2Text;

        // ✅ ChoiceButton Image 3개 추가
        public Image ChoiceButton0Image;
        public Image ChoiceButton1Image;
        public Image ChoiceButton2Image;

        // ---- VFX Panel ----
        public RectTransform VFXBlackScreen00Root;
        public Image VFXBlackOut00Image;
        public Image VFXBlackOut01Image;

        public RectTransform BlackFade00Root;
        public Image BlackFade00Image;

        // ---- Skip Toggle ----
        public RectTransform SkipToggle00Root;
        public Image SkipToggle00Image;
        public Image SkipToggle01Image;
        public TMP_Text SkipToggle00Text;

        // ---- Next Toggle ----
        public RectTransform NextToggle00Root;
        public Image NextToggle00Image;
        public TMP_Text NextToggle00Text;

        // ---- Auto Toggle ----
        public RectTransform AutoToggle00Root;
        public Image AutoToggle00Image;
        public Image AutoToggle01Image;

        // ---- Speedup Toggle ----
        public RectTransform SpeedupToggle00Root;
        public Image SpeedupToggle00Image;
        public Image SpeedupToggle01Image;

        // ---- SetSpeed Toggle ----
        public RectTransform SetSpeedToggle00Root;
        public Image SetSpeedToggle00Image;
        public Image SetSpeedToggle01Image;
        public Image SetSpeedToggle02Image;
        public Image SetSpeedToggle03Image;
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

            // ---- Main Standing Portrait ----
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

            // ---- SubRight Standing Portrait ----
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

            // ---- Background (00/01) ----
            case DialogueWidgetTarget.BackgroundRoot00:  return refs.BackgroundRoot00;
            case DialogueWidgetTarget.BackgroundRoot01:  return refs.BackgroundRoot01;
            case DialogueWidgetTarget.BackgroundImage00: return refs.BackgroundImage00;
            case DialogueWidgetTarget.BackgroundImage01: return refs.BackgroundImage01;

            // ---- Choice Panel ----
            case DialogueWidgetTarget.ChoicePanelRoot:    return refs.ChoicePanelRoot;
            case DialogueWidgetTarget.ChoiceButton0Root:  return refs.ChoiceButton0Root;
            case DialogueWidgetTarget.ChoiceButton1Root:  return refs.ChoiceButton1Root;
            case DialogueWidgetTarget.ChoiceButton2Root:  return refs.ChoiceButton2Root;

            case DialogueWidgetTarget.ChoiceButton0Text:  return refs.ChoiceButton0Text;
            case DialogueWidgetTarget.ChoiceButton1Text:  return refs.ChoiceButton1Text;
            case DialogueWidgetTarget.ChoiceButton2Text:  return refs.ChoiceButton2Text;

            case DialogueWidgetTarget.ChoiceButton0Image: return refs.ChoiceButton0Image;
            case DialogueWidgetTarget.ChoiceButton1Image: return refs.ChoiceButton1Image;
            case DialogueWidgetTarget.ChoiceButton2Image: return refs.ChoiceButton2Image;

            // ---- VFX / Toggle Panel ----
            case DialogueWidgetTarget.VFXBlackScreen00Root: return refs.VFXBlackScreen00Root;
            case DialogueWidgetTarget.VFXBlackOut00Image:   return refs.VFXBlackOut00Image;
            case DialogueWidgetTarget.VFXBlackOut01Image:   return refs.VFXBlackOut01Image;

            case DialogueWidgetTarget.BlackFade00Root:      return refs.BlackFade00Root;
            case DialogueWidgetTarget.BlackFade00Image:     return refs.BlackFade00Image;

            case DialogueWidgetTarget.SkipToggle00Root:     return refs.SkipToggle00Root;
            case DialogueWidgetTarget.SkipToggle00Image:    return refs.SkipToggle00Image;
            case DialogueWidgetTarget.SkipToggle01Image:    return refs.SkipToggle01Image;
            case DialogueWidgetTarget.SkipToggle00Text:     return refs.SkipToggle00Text;

            case DialogueWidgetTarget.NextToggle00Root:     return refs.NextToggle00Root;
            case DialogueWidgetTarget.NextToggle00Image:    return refs.NextToggle00Image;
            case DialogueWidgetTarget.NextToggle00Text:     return refs.NextToggle00Text;

            case DialogueWidgetTarget.AutoToggle00Root:     return refs.AutoToggle00Root;
            case DialogueWidgetTarget.AutoToggle00Image:    return refs.AutoToggle00Image;
            case DialogueWidgetTarget.AutoToggle01Image:    return refs.AutoToggle01Image;

            case DialogueWidgetTarget.SpeedupToggle00Root:  return refs.SpeedupToggle00Root;
            case DialogueWidgetTarget.SpeedupToggle00Image: return refs.SpeedupToggle00Image;
            case DialogueWidgetTarget.SpeedupToggle01Image: return refs.SpeedupToggle01Image;

            case DialogueWidgetTarget.SetSpeedToggle00Root:  return refs.SetSpeedToggle00Root;
            case DialogueWidgetTarget.SetSpeedToggle00Image: return refs.SetSpeedToggle00Image;
            case DialogueWidgetTarget.SetSpeedToggle01Image: return refs.SetSpeedToggle01Image;
            case DialogueWidgetTarget.SetSpeedToggle02Image: return refs.SetSpeedToggle02Image;
            case DialogueWidgetTarget.SetSpeedToggle03Image: return refs.SetSpeedToggle03Image;

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