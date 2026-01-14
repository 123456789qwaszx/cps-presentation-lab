using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Lab.UI.Naming;

[CreateAssetMenu(fileName = "CpsDialogueWidgetAccess", menuName = "Dialogue/Services/CPS Widget Access")]
public sealed class CpsDialogueWidgetAccessAsset : ScriptableObject, IDialogueWidgetAccess
{
    public bool TryResolve(string screenId, string widgetRoleKey, out IDialogueWidgetAccess.WidgetRefs refs)
    {
        refs = null;

        UIRouter router = UIRuntimeRouter.Router;
        if (router == null) 
        { Debug.LogWarning("[CpsDialogueWidgetAccess] UIRuntimeRouter.Router is null."); return false; }

        ScreenKey key = new ScreenKey(screenId);
        if (!router.TryGetScreen(key, out UIScreen screen)) 
        { Debug.LogWarning($"[CpsDialogueWidgetAccess] Failed to resolve UIScreen. screenId='{screenId}', ScreenKey='{key}'"); return false; }
        
        DialogueRoleWidgetTags set = new (widgetRoleKey);

        refs = new IDialogueWidgetAccess.WidgetRefs
        {
            // ---- Dialogue Line ----
            DialogueBoxRoot       = screen.GetWidgetHandle(set.DialogueBoxRootTag)?.RectTransform,
            LineText              = screen.GetWidgetDirect<TMP_Text>(set.LineTextTag),
            LineBodyImage         = screen.GetWidgetDirect<Image>(set.LineBodyTag),
            SpeakerNameBox        = screen.GetWidgetDirect<Image>(set.SpeakerNameBoxTag),
            SpeakerNameText       = screen.GetWidgetDirect<TMP_Text>(set.SpeakerNameTag),
            ProtagonistCutinImage = screen.GetWidgetDirect<Image>(set.ProtagonistCutinImageTag),
            
            // ---- Main Standing Portrait ----
            MainStandingPortraitRoot        = screen.GetWidgetHandle(set.MainStandingPortraitRootTag)?.RectTransform,
            MainStandingPortraitTrack       = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitTrackTag),
            MainStandingPortraitRig         = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitRigTag),
            MainStandingPortraitSwayPivot   = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitSwayPivotTag),
            MainStandingPortraitShake       = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitShakeTag),
            MainStandingPortraitScale       = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitScaleTag),
            MainStandingPortraitVisual      = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitVisualTag),
            MainStandingPortraitImage       = screen.GetWidgetDirect<Image>(set.MainStandingPortraitImageTag),
            MainStandingPortraitEmojiAnchor = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitEmojiAnchorTag),
            MainStandingPortraitEmojiImage  = screen.GetWidgetDirect<Image>(set.MainStandingPortraitEmojiImageTag),

            // ---- SubLeft Standing Portrait ----
            SubLeftStandingPortraitRoot        = screen.GetWidgetHandle(set.SubLeftStandingPortraitRootTag)?.RectTransform,
            SubLeftStandingPortraitTrack       = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitTrackTag),
            SubLeftStandingPortraitRig         = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitRigTag),
            SubLeftStandingPortraitSwayPivot   = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitSwayPivotTag),
            SubLeftStandingPortraitShake       = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitShakeTag),
            SubLeftStandingPortraitScale       = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitScaleTag),
            SubLeftStandingPortraitVisual      = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitVisualTag),
            SubLeftStandingPortraitImage       = screen.GetWidgetDirect<Image>(set.SubLeftStandingPortraitImageTag),
            SubLeftStandingPortraitEmojiAnchor = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitEmojiAnchorTag),
            SubLeftStandingPortraitEmojiImage  = screen.GetWidgetDirect<Image>(set.SubLeftStandingPortraitEmojiImageTag),

            // ---- SubRight Standing Portrait ----
            SubRightStandingPortraitRoot        = screen.GetWidgetHandle(set.SubRightStandingPortraitRootTag)?.RectTransform,
            SubRightStandingPortraitTrack       = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitTrackTag),
            SubRightStandingPortraitRig         = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitRigTag),
            SubRightStandingPortraitSwayPivot   = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitSwayPivotTag),
            SubRightStandingPortraitShake       = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitShakeTag),
            SubRightStandingPortraitScale       = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitScaleTag),
            SubRightStandingPortraitVisual      = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitVisualTag),
            SubRightStandingPortraitImage       = screen.GetWidgetDirect<Image>(set.SubRightStandingPortraitImageTag),
            SubRightStandingPortraitEmojiAnchor = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitEmojiAnchorTag),
            SubRightStandingPortraitEmojiImage  = screen.GetWidgetDirect<Image>(set.SubRightStandingPortraitEmojiImageTag),

            // ---- Background ----
            BackgroundImage0 = screen.GetWidgetHandle(set.BackgroundImage0Tag)?.Image,
            BackgroundImage1 = screen.GetWidgetHandle(set.BackgroundImage1Tag)?.Image,

            // ---- Choice Panel ----
            ChoicePanelRoot = screen.GetWidgetHandle(set.ChoicePanelRootTag)?.RectTransform,
            ChoiceButton0Root = screen.GetWidgetDirect<Image>(set.ChoiceButton0RootTag),
            ChoiceButton1Root = screen.GetWidgetDirect<Image>(set.ChoiceButton1RootTag),
            ChoiceButton2Root = screen.GetWidgetDirect<Image>(set.ChoiceButton2RootTag),
            ChoiceButton0Text = screen.GetWidgetDirect<TMP_Text>(set.ChoiceButton0TextTag),
            ChoiceButton1Text = screen.GetWidgetDirect<TMP_Text>(set.ChoiceButton1TextTag),
            ChoiceButton2Text = screen.GetWidgetDirect<TMP_Text>(set.ChoiceButton2TextTag),
        };

        return true;
    }
}