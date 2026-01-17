using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum DialogueWidgetTarget
{
    None = -1,

    // ---- Dialogue ----
    DialogueBox00Root  = 0,
    DialogueBox00Image = 10,
    DialogueBox00Text  = 20,

    SpeakerNameBox00Root  = 100,
    SpeakerNameBox00Text  = 110,
    SpeakerNameBox00Image = 120,
    
    AdvanceIndicator00Root  = 200,
    AdvanceIndicator00Image = 210,

    ProtagonistCutin00Root   = 300,
    ProtagonistCutin00Anchor = 310,
    ProtagonistCutin00Image  = 320,
    
    ProtagonistCutin00Emoji00Root      = 400,
    ProtagonistCutin00Emoji00Anchor    = 410,
    ProtagonistCutin00Emoji00SwayPivot = 420,
    ProtagonistCutin00Emoji00Image     = 430,
    
    ProtagonistCutin00Emoji01Root      = 500,
    ProtagonistCutin00Emoji01Anchor    = 510,
    ProtagonistCutin00Emoji01SwayPivot = 520,
    ProtagonistCutin00Emoji01Image     = 530,
    
    ProtagonistCutin00Emoji02Root      = 600,
    ProtagonistCutin00Emoji02Anchor    = 610,
    ProtagonistCutin00Emoji02SwayPivot = 620,
    ProtagonistCutin00Emoji02Image     = 630,

    // ---- Standing00 ----
    Standing00Root  = 1000,
    Standing00Track = 1010,

    Standing00PortraitRoot      = 1100,
    Standing00PortraitSwayPivot = 1110,
    Standing00PortraitShake     = 1120,
    Standing00PortraitScale     = 1130,
    Standing00PortraitVisual    = 1140,
    Standing00PortraitImage     = 1150,
    
    Standing00PortraitOverlaysRoot  = 1200,
    Standing00PortraitOverlaysImage = 1210,

    Standing00Emoji00Root      = 1300,
    Standing00Emoji00Anchor    = 1310,
    Standing00Emoji00SwayPivot = 1320,
    Standing00Emoji00Image     = 1330,

    Standing00Emoji01Root      = 1400,
    Standing00Emoji01Anchor    = 1410,
    Standing00Emoji01SwayPivot = 1420,
    Standing00Emoji01Image     = 1430,

    Standing00Emoji02Root      = 1500,
    Standing00Emoji02Anchor    = 1510,
    Standing00Emoji02SwayPivot = 1520,
    Standing00Emoji02Image     = 1530,

    Standing00Emoji03Root      = 1600,
    Standing00Emoji03Anchor    = 1610,
    Standing00Emoji03SwayPivot = 1620,
    Standing00Emoji03Image     = 1630,

    // ---- Standing01 ----
    Standing01Root  = 2000,
    Standing01Track = 2010,

    Standing01PortraitRoot      = 2100,
    Standing01PortraitSwayPivot = 2110,
    Standing01PortraitShake     = 2120,
    Standing01PortraitScale     = 2130,
    Standing01PortraitVisual    = 2140,
    Standing01PortraitImage     = 2150,
    
    Standing01PortraitOverlaysRoot  = 2200,
    Standing01PortraitOverlaysImage = 2210,

    Standing01Emoji00Root      = 2300,
    Standing01Emoji00Anchor    = 2310,
    Standing01Emoji00SwayPivot = 2320,
    Standing01Emoji00Image     = 2330,

    Standing01Emoji01Root      = 2400,
    Standing01Emoji01Anchor    = 2410,
    Standing01Emoji01SwayPivot = 2420,
    Standing01Emoji01Image     = 2430,

    Standing01Emoji02Root      = 2500,
    Standing01Emoji02Anchor    = 2510,
    Standing01Emoji02SwayPivot = 2520,
    Standing01Emoji02Image     = 2530,

    Standing01Emoji03Root      = 2600,
    Standing01Emoji03Anchor    = 2610,
    Standing01Emoji03SwayPivot = 2620,
    Standing01Emoji03Image     = 2630,

    // ---- Standing02 ----
    Standing02Root  = 3000,
    Standing02Track = 3010,

    Standing02PortraitRoot      = 3100,
    Standing02PortraitSwayPivot = 3110,
    Standing02PortraitShake     = 3120,
    Standing02PortraitScale     = 3130,
    Standing02PortraitVisual    = 3140,
    Standing02PortraitImage     = 3150,
    
    Standing02PortraitOverlaysRoot  = 3200,
    Standing02PortraitOverlaysImage = 3210,
    
    Standing02Emoji00Root      = 3300,
    Standing02Emoji00Anchor    = 3310,
    Standing02Emoji00SwayPivot = 3320,
    Standing02Emoji00Image     = 3330,

    Standing02Emoji01Root      = 3400,
    Standing02Emoji01Anchor    = 3410,
    Standing02Emoji01SwayPivot = 3420,
    Standing02Emoji01Image     = 3430,

    Standing02Emoji02Root      = 3500,
    Standing02Emoji02Anchor    = 3510,
    Standing02Emoji02SwayPivot = 3520,
    Standing02Emoji02Image     = 3530,

    Standing02Emoji03Root      = 3600,
    Standing02Emoji03Anchor    = 3610,
    Standing02Emoji03SwayPivot = 3620,
    Standing02Emoji03Image     = 3630,

    // ---- Background ----
    Background00Root  = 4000,
    Background00Image = 4010,

    Background01Root  = 4100,
    Background01Image = 4110,

    // ---- Choice ----
    ChoicePanel00Root  = 5000,
    ChoicePanel00Image = 5010,

    ChoiceButton00Root  = 5100,
    ChoiceButton00Image = 5110,
    ChoiceButton00Text  = 5120,

    ChoiceButton01Root  = 5200,
    ChoiceButton01Image = 5210,
    ChoiceButton01Text  = 5220,

    ChoiceButton02Root  = 5300,
    ChoiceButton02Image = 5310,
    ChoiceButton02Text  = 5320,

    // ---- VFX ----
    VFXBlackScreen00Root   = 6000,
    VFXBlackScreen00Image0 = 6010,
    VFXBlackScreen00Image1 = 6020,

    VFXBlackFade00Root  = 6100,
    VFXBlackFade00Image = 6110,

    // ---- Toggle ----
    TogglePanel00Root = 8000,

    SkipToggle00Root   = 8100,
    SkipToggle00Image0 = 8110,
    SkipToggle00Image1 = 8120,
    SkipToggle00Text   = 8130,

    NextToggle00Root  = 8200,
    NextToggle00Image = 8210,
    NextToggle00Text  = 8220,

    AutoToggle00Root   = 8300,
    AutoToggle00Image0 = 8310,
    AutoToggle00Image1 = 8320,

    SpeedupToggle00Root   = 8400,
    SpeedupToggle00Image0 = 8410,
    SpeedupToggle00Image1 = 8420,

    SetSpeedToggle00Root   = 8500,
    SetSpeedToggle00Image0 = 8510,
    SetSpeedToggle00Image1 = 8520,
    SetSpeedToggle00Image2 = 8530,
    SetSpeedToggle00Image3 = 8540,
}

public interface IDialogueWidgetAccess
{
    bool TryResolve(string screenId, string widgetId, out WidgetRefs refs);

    public sealed class WidgetRefs
    {
        // ======================================================
        // Dialogue Line
        // ======================================================
        public RectTransform DialogueBox00Root;
        public Image         DialogueBox00Image;
        public TMP_Text      DialogueBox00Text;

        public RectTransform SpeakerNameBox00Root;
        public Image         SpeakerNameBox00Image;
        public TMP_Text      SpeakerNameBox00Text;

        public RectTransform AdvanceIndicator00Root;
        public Image         AdvanceIndicator00Image;

        public RectTransform ProtagonistCutin00Root;
        public RectTransform ProtagonistCutin00Anchor;
        public Image         ProtagonistCutin00Image;

        public RectTransform ProtagonistCutin00Emoji00Root;
        public RectTransform ProtagonistCutin00Emoji00Anchor;
        public RectTransform ProtagonistCutin00Emoji00SwayPivot;
        public Image         ProtagonistCutin00Emoji00Image;

        public RectTransform ProtagonistCutin00Emoji01Root;
        public RectTransform ProtagonistCutin00Emoji01Anchor;
        public RectTransform ProtagonistCutin00Emoji01SwayPivot;
        public Image         ProtagonistCutin00Emoji01Image;

        public RectTransform ProtagonistCutin00Emoji02Root;
        public RectTransform ProtagonistCutin00Emoji02Anchor;
        public RectTransform ProtagonistCutin00Emoji02SwayPivot;
        public Image         ProtagonistCutin00Emoji02Image;

        // ======================================================
        // Standing00
        // ======================================================
        public RectTransform Standing00Root;
        public RectTransform Standing00Track;

        public RectTransform Standing00PortraitRoot;
        public RectTransform Standing00PortraitSwayPivot;
        public RectTransform Standing00PortraitShake;
        public RectTransform Standing00PortraitScale;
        public RectTransform Standing00PortraitVisual;
        public Image         Standing00PortraitImage;
        
        public RectTransform Standing00PortraitOverlaysRoot;
        public Image         Standing00PortraitOverlaysImage;

        public RectTransform Standing00Emoji00Root;
        public RectTransform Standing00Emoji00Anchor;
        public RectTransform Standing00Emoji00SwayPivot;
        public Image         Standing00Emoji00Image;

        public RectTransform Standing00Emoji01Root;
        public RectTransform Standing00Emoji01Anchor;
        public RectTransform Standing00Emoji01SwayPivot;
        public Image         Standing00Emoji01Image;

        public RectTransform Standing00Emoji02Root;
        public RectTransform Standing00Emoji02Anchor;
        public RectTransform Standing00Emoji02SwayPivot;
        public Image         Standing00Emoji02Image;

        public RectTransform Standing00Emoji03Root;
        public RectTransform Standing00Emoji03Anchor;
        public RectTransform Standing00Emoji03SwayPivot;
        public Image         Standing00Emoji03Image;

        // ======================================================
        // Standing01
        // ======================================================
        public RectTransform Standing01Root;
        public RectTransform Standing01Track;

        public RectTransform Standing01PortraitRoot;
        public RectTransform Standing01PortraitSwayPivot;
        public RectTransform Standing01PortraitShake;
        public RectTransform Standing01PortraitScale;
        public RectTransform Standing01PortraitVisual;
        public Image         Standing01PortraitImage;
        
        public RectTransform Standing01PortraitOverlaysRoot;
        public Image         Standing01PortraitOverlaysImage;

        public RectTransform Standing01Emoji00Root;
        public RectTransform Standing01Emoji00Anchor;
        public RectTransform Standing01Emoji00SwayPivot;
        public Image         Standing01Emoji00Image;

        public RectTransform Standing01Emoji01Root;
        public RectTransform Standing01Emoji01Anchor;
        public RectTransform Standing01Emoji01SwayPivot;
        public Image         Standing01Emoji01Image;

        public RectTransform Standing01Emoji02Root;
        public RectTransform Standing01Emoji02Anchor;
        public RectTransform Standing01Emoji02SwayPivot;
        public Image         Standing01Emoji02Image;

        public RectTransform Standing01Emoji03Root;
        public RectTransform Standing01Emoji03Anchor;
        public RectTransform Standing01Emoji03SwayPivot;
        public Image         Standing01Emoji03Image;

        // ======================================================
        // Standing02
        // ======================================================
        public RectTransform Standing02Root;
        public RectTransform Standing02Track;

        public RectTransform Standing02PortraitRoot;
        public RectTransform Standing02PortraitSwayPivot;
        public RectTransform Standing02PortraitShake;
        public RectTransform Standing02PortraitScale;
        public RectTransform Standing02PortraitVisual;
        public Image         Standing02PortraitImage;
        
        public RectTransform Standing02PortraitOverlaysRoot;
        public Image         Standing02PortraitOverlaysImage;

        public RectTransform Standing02Emoji00Root;
        public RectTransform Standing02Emoji00Anchor;
        public RectTransform Standing02Emoji00SwayPivot;
        public Image         Standing02Emoji00Image;

        public RectTransform Standing02Emoji01Root;
        public RectTransform Standing02Emoji01Anchor;
        public RectTransform Standing02Emoji01SwayPivot;
        public Image         Standing02Emoji01Image;

        public RectTransform Standing02Emoji02Root;
        public RectTransform Standing02Emoji02Anchor;
        public RectTransform Standing02Emoji02SwayPivot;
        public Image         Standing02Emoji02Image;

        public RectTransform Standing02Emoji03Root;
        public RectTransform Standing02Emoji03Anchor;
        public RectTransform Standing02Emoji03SwayPivot;
        public Image         Standing02Emoji03Image;

        // ======================================================
        // Background
        // ======================================================
        public RectTransform Background00Root;
        public Image         Background00Image;

        public RectTransform Background01Root;
        public Image         Background01Image;

        // ======================================================
        // Choice
        // ======================================================
        public RectTransform ChoicePanel00Root;
        public Image         ChoicePanel00Image;

        public RectTransform ChoiceButton00Root;
        public Image         ChoiceButton00Image;
        public TMP_Text      ChoiceButton00Text;

        public RectTransform ChoiceButton01Root;
        public Image         ChoiceButton01Image;
        public TMP_Text      ChoiceButton01Text;

        public RectTransform ChoiceButton02Root;
        public Image         ChoiceButton02Image;
        public TMP_Text      ChoiceButton02Text;

        // ======================================================
        // VFX
        // ======================================================
        public RectTransform VFXBlackScreen00Root;
        public Image         VFXBlackScreen00Image0;
        public Image         VFXBlackScreen00Image1;

        public RectTransform VFXBlackFade00Root;
        public Image         VFXBlackFade00Image;

        // ======================================================
        // Toggle
        // ======================================================
        public RectTransform TogglePanel00Root;

        public RectTransform SkipToggle00Root;
        public Image         SkipToggle00Image0;
        public Image         SkipToggle00Image1;
        public TMP_Text      SkipToggle00Text;

        public RectTransform NextToggle00Root;
        public Image         NextToggle00Image;
        public TMP_Text      NextToggle00Text;

        public RectTransform AutoToggle00Root;
        public Image         AutoToggle00Image0;
        public Image         AutoToggle00Image1;

        public RectTransform SpeedupToggle00Root;
        public Image         SpeedupToggle00Image0;
        public Image         SpeedupToggle00Image1;

        public RectTransform SetSpeedToggle00Root;
        public Image         SetSpeedToggle00Image0;
        public Image         SetSpeedToggle00Image1;
        public Image         SetSpeedToggle00Image2;
        public Image         SetSpeedToggle00Image3;
    }
}

public static class WidgetRefsExtensions
{
    public static Component GetComponent(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetTarget target)
    {
        if (refs == null) return null;

        switch (target)
        {
            // ---- Dialogue ----
            case DialogueWidgetTarget.DialogueBox00Root:  return refs.DialogueBox00Root;
            case DialogueWidgetTarget.DialogueBox00Image: return refs.DialogueBox00Image;
            case DialogueWidgetTarget.DialogueBox00Text:  return refs.DialogueBox00Text;
    
            case DialogueWidgetTarget.AdvanceIndicator00Root:  return refs.AdvanceIndicator00Root;
            case DialogueWidgetTarget.AdvanceIndicator00Image: return refs.AdvanceIndicator00Image;

            case DialogueWidgetTarget.SpeakerNameBox00Root:  return refs.SpeakerNameBox00Root;
            case DialogueWidgetTarget.SpeakerNameBox00Image: return refs.SpeakerNameBox00Image;
            case DialogueWidgetTarget.SpeakerNameBox00Text:  return refs.SpeakerNameBox00Text;

            case DialogueWidgetTarget.ProtagonistCutin00Root:   return refs.ProtagonistCutin00Root;
            case DialogueWidgetTarget.ProtagonistCutin00Anchor: return refs.ProtagonistCutin00Anchor;
            case DialogueWidgetTarget.ProtagonistCutin00Image:  return refs.ProtagonistCutin00Image;
            
            case DialogueWidgetTarget.ProtagonistCutin00Emoji00Root:      return refs.ProtagonistCutin00Emoji00Root;
            case DialogueWidgetTarget.ProtagonistCutin00Emoji00Anchor:    return refs.ProtagonistCutin00Emoji00Anchor;
            case DialogueWidgetTarget.ProtagonistCutin00Emoji00SwayPivot: return refs.ProtagonistCutin00Emoji00SwayPivot;
            case DialogueWidgetTarget.ProtagonistCutin00Emoji00Image:     return refs.ProtagonistCutin00Emoji00Image;
    
            case DialogueWidgetTarget.ProtagonistCutin00Emoji01Root:      return refs.ProtagonistCutin00Emoji01Root;
            case DialogueWidgetTarget.ProtagonistCutin00Emoji01Anchor:    return refs.ProtagonistCutin00Emoji01Anchor;
            case DialogueWidgetTarget.ProtagonistCutin00Emoji01SwayPivot: return refs.ProtagonistCutin00Emoji01SwayPivot;
            case DialogueWidgetTarget.ProtagonistCutin00Emoji01Image:     return refs.ProtagonistCutin00Emoji01Image;
            
            case DialogueWidgetTarget.ProtagonistCutin00Emoji02Root:      return refs.ProtagonistCutin00Emoji02Root;
            case DialogueWidgetTarget.ProtagonistCutin00Emoji02Anchor:    return refs.ProtagonistCutin00Emoji02Anchor;
            case DialogueWidgetTarget.ProtagonistCutin00Emoji02SwayPivot: return refs.ProtagonistCutin00Emoji02SwayPivot;
            case DialogueWidgetTarget.ProtagonistCutin00Emoji02Image:     return refs.ProtagonistCutin00Emoji02Image;

            // ---- Standing00 ----
            case DialogueWidgetTarget.Standing00Root:  return refs.Standing00Root;
            case DialogueWidgetTarget.Standing00Track: return refs.Standing00Track;

            case DialogueWidgetTarget.Standing00PortraitRoot:      return refs.Standing00PortraitRoot;
            case DialogueWidgetTarget.Standing00PortraitSwayPivot: return refs.Standing00PortraitSwayPivot;
            case DialogueWidgetTarget.Standing00PortraitShake:     return refs.Standing00PortraitShake;
            case DialogueWidgetTarget.Standing00PortraitScale:     return refs.Standing00PortraitScale;
            case DialogueWidgetTarget.Standing00PortraitVisual:    return refs.Standing00PortraitVisual;
            case DialogueWidgetTarget.Standing00PortraitImage:     return refs.Standing00PortraitImage;;
                
            case DialogueWidgetTarget.Standing00PortraitOverlaysRoot:  return refs.Standing00PortraitOverlaysRoot;
            case DialogueWidgetTarget.Standing00PortraitOverlaysImage: return refs.Standing00PortraitOverlaysImage;

            case DialogueWidgetTarget.Standing00Emoji00Root:      return refs.Standing00Emoji00Root;
            case DialogueWidgetTarget.Standing00Emoji00Anchor:    return refs.Standing00Emoji00Anchor;
            case DialogueWidgetTarget.Standing00Emoji00SwayPivot: return refs.Standing00Emoji00SwayPivot;
            case DialogueWidgetTarget.Standing00Emoji00Image:     return refs.Standing00Emoji00Image;

            case DialogueWidgetTarget.Standing00Emoji01Root:      return refs.Standing00Emoji01Root;
            case DialogueWidgetTarget.Standing00Emoji01Anchor:    return refs.Standing00Emoji01Anchor;
            case DialogueWidgetTarget.Standing00Emoji01SwayPivot: return refs.Standing00Emoji01SwayPivot;
            case DialogueWidgetTarget.Standing00Emoji01Image:     return refs.Standing00Emoji01Image;

            case DialogueWidgetTarget.Standing00Emoji02Root:      return refs.Standing00Emoji02Root;
            case DialogueWidgetTarget.Standing00Emoji02Anchor:    return refs.Standing00Emoji02Anchor;
            case DialogueWidgetTarget.Standing00Emoji02SwayPivot: return refs.Standing00Emoji02SwayPivot;
            case DialogueWidgetTarget.Standing00Emoji02Image:     return refs.Standing00Emoji02Image;

            case DialogueWidgetTarget.Standing00Emoji03Root:      return refs.Standing00Emoji03Root;
            case DialogueWidgetTarget.Standing00Emoji03Anchor:    return refs.Standing00Emoji03Anchor;
            case DialogueWidgetTarget.Standing00Emoji03SwayPivot: return refs.Standing00Emoji03SwayPivot;
            case DialogueWidgetTarget.Standing00Emoji03Image:     return refs.Standing00Emoji03Image;

            // ---- Standing01 ----
            case DialogueWidgetTarget.Standing01Root:  return refs.Standing01Root;
            case DialogueWidgetTarget.Standing01Track: return refs.Standing01Track;

            case DialogueWidgetTarget.Standing01PortraitRoot:      return refs.Standing01PortraitRoot;
            case DialogueWidgetTarget.Standing01PortraitSwayPivot: return refs.Standing01PortraitSwayPivot;
            case DialogueWidgetTarget.Standing01PortraitShake:     return refs.Standing01PortraitShake;
            case DialogueWidgetTarget.Standing01PortraitScale:     return refs.Standing01PortraitScale;
            case DialogueWidgetTarget.Standing01PortraitVisual:    return refs.Standing01PortraitVisual;
            case DialogueWidgetTarget.Standing01PortraitImage:     return refs.Standing01PortraitImage;
            
            case DialogueWidgetTarget.Standing01PortraitOverlaysRoot:  return refs.Standing01PortraitOverlaysRoot;
            case DialogueWidgetTarget.Standing01PortraitOverlaysImage: return refs.Standing01PortraitOverlaysImage;

            case DialogueWidgetTarget.Standing01Emoji00Root:      return refs.Standing01Emoji00Root;
            case DialogueWidgetTarget.Standing01Emoji00Anchor:    return refs.Standing01Emoji00Anchor;
            case DialogueWidgetTarget.Standing01Emoji00SwayPivot: return refs.Standing01Emoji00SwayPivot;
            case DialogueWidgetTarget.Standing01Emoji00Image:     return refs.Standing01Emoji00Image;

            case DialogueWidgetTarget.Standing01Emoji01Root:      return refs.Standing01Emoji01Root;
            case DialogueWidgetTarget.Standing01Emoji01Anchor:    return refs.Standing01Emoji01Anchor;
            case DialogueWidgetTarget.Standing01Emoji01SwayPivot: return refs.Standing01Emoji01SwayPivot;
            case DialogueWidgetTarget.Standing01Emoji01Image:     return refs.Standing01Emoji01Image;

            case DialogueWidgetTarget.Standing01Emoji02Root:      return refs.Standing01Emoji02Root;
            case DialogueWidgetTarget.Standing01Emoji02Anchor:    return refs.Standing01Emoji02Anchor;
            case DialogueWidgetTarget.Standing01Emoji02SwayPivot: return refs.Standing01Emoji02SwayPivot;
            case DialogueWidgetTarget.Standing01Emoji02Image:     return refs.Standing01Emoji02Image;

            case DialogueWidgetTarget.Standing01Emoji03Root:      return refs.Standing01Emoji03Root;
            case DialogueWidgetTarget.Standing01Emoji03Anchor:    return refs.Standing01Emoji03Anchor;
            case DialogueWidgetTarget.Standing01Emoji03SwayPivot: return refs.Standing01Emoji03SwayPivot;
            case DialogueWidgetTarget.Standing01Emoji03Image:     return refs.Standing01Emoji03Image;

            // ---- Standing02 ----
            case DialogueWidgetTarget.Standing02Root:  return refs.Standing02Root;
            case DialogueWidgetTarget.Standing02Track: return refs.Standing02Track;

            case DialogueWidgetTarget.Standing02PortraitRoot:      return refs.Standing02PortraitRoot;
            case DialogueWidgetTarget.Standing02PortraitSwayPivot: return refs.Standing02PortraitSwayPivot;
            case DialogueWidgetTarget.Standing02PortraitShake:     return refs.Standing02PortraitShake;
            case DialogueWidgetTarget.Standing02PortraitScale:     return refs.Standing02PortraitScale;
            case DialogueWidgetTarget.Standing02PortraitVisual:    return refs.Standing02PortraitVisual;
            case DialogueWidgetTarget.Standing02PortraitImage:     return refs.Standing02PortraitImage;
            
            case DialogueWidgetTarget.Standing02PortraitOverlaysRoot:  return refs.Standing02PortraitOverlaysRoot;
            case DialogueWidgetTarget.Standing02PortraitOverlaysImage: return refs.Standing02PortraitOverlaysImage;

            case DialogueWidgetTarget.Standing02Emoji00Root:      return refs.Standing02Emoji00Root;
            case DialogueWidgetTarget.Standing02Emoji00Anchor:    return refs.Standing02Emoji00Anchor;
            case DialogueWidgetTarget.Standing02Emoji00SwayPivot: return refs.Standing02Emoji00SwayPivot;
            case DialogueWidgetTarget.Standing02Emoji00Image:     return refs.Standing02Emoji00Image;

            case DialogueWidgetTarget.Standing02Emoji01Root:      return refs.Standing02Emoji01Root;
            case DialogueWidgetTarget.Standing02Emoji01Anchor:    return refs.Standing02Emoji01Anchor;
            case DialogueWidgetTarget.Standing02Emoji01SwayPivot: return refs.Standing02Emoji01SwayPivot;
            case DialogueWidgetTarget.Standing02Emoji01Image:     return refs.Standing02Emoji01Image;

            case DialogueWidgetTarget.Standing02Emoji02Root:      return refs.Standing02Emoji02Root;
            case DialogueWidgetTarget.Standing02Emoji02Anchor:    return refs.Standing02Emoji02Anchor;
            case DialogueWidgetTarget.Standing02Emoji02SwayPivot: return refs.Standing02Emoji02SwayPivot;
            case DialogueWidgetTarget.Standing02Emoji02Image:     return refs.Standing02Emoji02Image;

            case DialogueWidgetTarget.Standing02Emoji03Root:      return refs.Standing02Emoji03Root;
            case DialogueWidgetTarget.Standing02Emoji03Anchor:    return refs.Standing02Emoji03Anchor;
            case DialogueWidgetTarget.Standing02Emoji03SwayPivot: return refs.Standing02Emoji03SwayPivot;
            case DialogueWidgetTarget.Standing02Emoji03Image:     return refs.Standing02Emoji03Image;

            // ---- Background ----
            case DialogueWidgetTarget.Background00Root:  return refs.Background00Root;
            case DialogueWidgetTarget.Background00Image: return refs.Background00Image;
            
            case DialogueWidgetTarget.Background01Root:  return refs.Background01Root;
            case DialogueWidgetTarget.Background01Image: return refs.Background01Image;

            // ---- Choice ----
            case DialogueWidgetTarget.ChoicePanel00Root:  return refs.ChoicePanel00Root;
            case DialogueWidgetTarget.ChoicePanel00Image: return refs.ChoicePanel00Image;

            case DialogueWidgetTarget.ChoiceButton00Root:  return refs.ChoiceButton00Root;
            case DialogueWidgetTarget.ChoiceButton00Image: return refs.ChoiceButton00Image;
            case DialogueWidgetTarget.ChoiceButton00Text:  return refs.ChoiceButton00Text;

            case DialogueWidgetTarget.ChoiceButton01Root:  return refs.ChoiceButton01Root;
            case DialogueWidgetTarget.ChoiceButton01Image: return refs.ChoiceButton01Image;
            case DialogueWidgetTarget.ChoiceButton01Text:  return refs.ChoiceButton01Text;

            case DialogueWidgetTarget.ChoiceButton02Root:  return refs.ChoiceButton02Root;
            case DialogueWidgetTarget.ChoiceButton02Image: return refs.ChoiceButton02Image;
            case DialogueWidgetTarget.ChoiceButton02Text:  return refs.ChoiceButton02Text;

            // ---- VFX ----
            case DialogueWidgetTarget.VFXBlackScreen00Root:   return refs.VFXBlackScreen00Root;
            case DialogueWidgetTarget.VFXBlackScreen00Image0: return refs.VFXBlackScreen00Image0;
            case DialogueWidgetTarget.VFXBlackScreen00Image1: return refs.VFXBlackScreen00Image1;

            case DialogueWidgetTarget.VFXBlackFade00Root:  return refs.VFXBlackFade00Root;
            case DialogueWidgetTarget.VFXBlackFade00Image: return refs.VFXBlackFade00Image;

            // ---- Toggle ----
            case DialogueWidgetTarget.TogglePanel00Root: return refs.TogglePanel00Root;

            case DialogueWidgetTarget.SkipToggle00Root:   return refs.SkipToggle00Root;
            case DialogueWidgetTarget.SkipToggle00Image0: return refs.SkipToggle00Image0;
            case DialogueWidgetTarget.SkipToggle00Image1: return refs.SkipToggle00Image1;
            case DialogueWidgetTarget.SkipToggle00Text:   return refs.SkipToggle00Text;

            case DialogueWidgetTarget.NextToggle00Root:  return refs.NextToggle00Root;
            case DialogueWidgetTarget.NextToggle00Image: return refs.NextToggle00Image;
            case DialogueWidgetTarget.NextToggle00Text:  return refs.NextToggle00Text;

            case DialogueWidgetTarget.AutoToggle00Root:   return refs.AutoToggle00Root;
            case DialogueWidgetTarget.AutoToggle00Image0: return refs.AutoToggle00Image0;
            case DialogueWidgetTarget.AutoToggle00Image1: return refs.AutoToggle00Image1;

            case DialogueWidgetTarget.SpeedupToggle00Root:   return refs.SpeedupToggle00Root;
            case DialogueWidgetTarget.SpeedupToggle00Image0: return refs.SpeedupToggle00Image0;
            case DialogueWidgetTarget.SpeedupToggle00Image1: return refs.SpeedupToggle00Image1;

            case DialogueWidgetTarget.SetSpeedToggle00Root:   return refs.SetSpeedToggle00Root;
            case DialogueWidgetTarget.SetSpeedToggle00Image0: return refs.SetSpeedToggle00Image0;
            case DialogueWidgetTarget.SetSpeedToggle00Image1: return refs.SetSpeedToggle00Image1;
            case DialogueWidgetTarget.SetSpeedToggle00Image2: return refs.SetSpeedToggle00Image2;
            case DialogueWidgetTarget.SetSpeedToggle00Image3: return refs.SetSpeedToggle00Image3;

            default:
                return null;
        }
    }

    public static Graphic GetGraphic(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetTarget target)
        => refs.GetComponent(target) as Graphic;

    public static TMP_Text GetText(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetTarget target)
        => refs.GetComponent(target) as TMP_Text;

    public static GameObject GetGameObject(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetTarget target)
    {
        var c = refs.GetComponent(target);
        return c != null ? c.gameObject : null;
    }

    public static RectTransform GetRect(this IDialogueWidgetAccess.WidgetRefs refs, DialogueWidgetTarget target)
    {
        var c = refs.GetComponent(target);
        if (c == null) return null;

        if (c is RectTransform rt) return rt;
        if (c is Graphic g) return g.rectTransform;
        return c.transform as RectTransform;
    }
}