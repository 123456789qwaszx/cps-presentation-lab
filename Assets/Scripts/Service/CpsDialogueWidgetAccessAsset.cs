using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Lab.UI.Naming;
using Lab.UI.Keys;

[CreateAssetMenu(fileName = "CpsDialogueWidgetAccess", menuName = "Dialogue/Services/CPS Widget Access")]
public sealed class CpsDialogueWidgetAccessAsset : ScriptableObject, IDialogueWidgetAccess
{
    [SerializeField] public string defaultRefKey;

    public bool TryResolve(string screenId, string widgetRoleKey, out IDialogueWidgetAccess.WidgetRefs refs)
    {
        refs = null;

        UIRouter router = UIRuntimeRouter.Router;
        if (router == null)
        {
            Debug.LogWarning("[CpsDialogueWidgetAccess] UIRuntimeRouter.Router is null.");
            return false;
        }

        ScreenKey key = new ScreenKey(screenId);
        if (!router.TryGetScreen(key, out UIScreen screen))
        {
            Debug.LogWarning($"[CpsDialogueWidgetAccess] Failed to resolve UIScreen. screenId='{screenId}', ScreenKey='{key}'");
            return false;
        }

        if (string.IsNullOrEmpty(widgetRoleKey))
            widgetRoleKey = defaultRefKey;

        DialogueRoleWidgetTags set = new DialogueRoleWidgetTags(widgetRoleKey);

        // ---- Line (정식 위젯) ----
        TMP_Text lineText      = screen.GetWidgetDirect<TMP_Text>(set.LineTextTag);
        Image lineBodyImage = screen.GetWidgetDirect<Image>(set.LineBodyTag);
        TMP_Text speakerName   = screen.GetWidgetDirect<TMP_Text>(set.SpeakerNameTag);

        // ---- Dialogue Box Root (정식 위젯) ----
        WidgetHandle dialogueBoxRootHandle = screen.GetWidgetHandle(set.DialogueBoxRootTag);

        // ---- Main Standing Portrait (Root는 정식 위젯, 나머지는 Prefab 우회로) ----
        WidgetHandle mainRootHandle = screen.GetWidgetHandle(set.MainStandingPortraitRootTag);
        RectTransform mainRoot      = mainRootHandle?.RectTransform;

        RectTransform mainTrack       = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitTrackTag);
        RectTransform mainRig         = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitRigTag);
        RectTransform mainSwayPivot   = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitSwayPivotTag);
        RectTransform mainShake       = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitShakeTag);
        RectTransform mainScale       = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitScaleTag);
        RectTransform mainVisual      = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitVisualTag);

        Image         mainImage       = screen.GetWidgetDirect<Image>(set.MainStandingPortraitImageTag);
        RectTransform mainEmojiAnchor = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitEmojiAnchorTag);
        Image         mainEmojiImage  = screen.GetWidgetDirect<Image>(set.MainStandingPortraitEmojiImageTag);

        // ---- SubLeft Standing Portrait ----
        WidgetHandle subLeftRootHandle = screen.GetWidgetHandle(set.SubLeftStandingPortraitRootTag);
        RectTransform subLeftRoot      = subLeftRootHandle?.RectTransform;

        RectTransform subLeftTrack       = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitTrackTag);
        RectTransform subLeftRig         = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitRigTag);
        RectTransform subLeftSwayPivot   = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitSwayPivotTag);
        RectTransform subLeftShake       = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitShakeTag);
        RectTransform subLeftScale       = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitScaleTag);
        RectTransform subLeftVisual      = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitVisualTag);

        Image         subLeftImage       = screen.GetWidgetDirect<Image>(set.SubLeftStandingPortraitImageTag);
        RectTransform subLeftEmojiAnchor = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitEmojiAnchorTag);
        Image         subLeftEmojiImage  = screen.GetWidgetDirect<Image>(set.SubLeftStandingPortraitEmojiImageTag);

        // ---- SubRight Standing Portrait ----
        WidgetHandle subRightRootHandle = screen.GetWidgetHandle(set.SubRightStandingPortraitRootTag);
        RectTransform subRightRoot      = subRightRootHandle?.RectTransform;

        RectTransform subRightTrack       = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitTrackTag);
        RectTransform subRightRig         = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitRigTag);
        RectTransform subRightSwayPivot   = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitSwayPivotTag);
        RectTransform subRightShake       = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitShakeTag);
        RectTransform subRightScale       = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitScaleTag);
        RectTransform subRightVisual      = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitVisualTag);

        Image         subRightImage       = screen.GetWidgetDirect<Image>(set.SubRightStandingPortraitImageTag);
        RectTransform subRightEmojiAnchor = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitEmojiAnchorTag);
        Image         subRightEmojiImage  = screen.GetWidgetDirect<Image>(set.SubRightStandingPortraitEmojiImageTag);

        // ---- Protagonist Cutin (현재는 Image만) ----
        Image pcImage = screen.GetWidgetDirect<Image>(set.ProtagonistCutinImageTag);

        // ---- Background (정식 위젯 경로 유지, 3장) ----
        WidgetHandle bgImage  = screen.GetWidgetHandle(set.BackgroundImageTag);
        WidgetHandle bgImage2 = screen.GetWidgetHandle(set.BackgroundImage2Tag);
        //WidgetHandle bgImage3 = screen.GetWidgetHandle(set.BackgroundImage3Tag);

        // ---- Choice Panel + 3 Choices (정식 위젯) ----
        WidgetHandle choicePanelRootHandle = screen.GetWidgetHandle(set.ChoicePanelRootTag);
        RectTransform choiceButton0Handle   = screen.GetWidgetDirect<RectTransform>(set.ChoiceButton0RootTag);
        RectTransform choiceButton1Handle   = screen.GetWidgetDirect<RectTransform>(set.ChoiceButton1RootTag);
        RectTransform choiceButton2Handle   = screen.GetWidgetDirect<RectTransform>(set.ChoiceButton2RootTag);

        refs = new IDialogueWidgetAccess.WidgetRefs
        {
            // Line
            LineText        = lineText,
            LineBodyImage   = lineBodyImage,
            SpeakerNameText = speakerName,

            // Dialogue Box
            DialogueBoxRoot = dialogueBoxRootHandle?.RectTransform,

            // Main Standing Portrait
            MainStandingPortraitRoot       = mainRoot,
            MainStandingPortraitTrack      = mainTrack,
            MainStandingPortraitRig        = mainRig,
            MainStandingPortraitSwayPivot  = mainSwayPivot,
            MainStandingPortraitShake      = mainShake,
            MainStandingPortraitScale      = mainScale,
            MainStandingPortraitVisual     = mainVisual,

            MainStandingPortraitImage       = mainImage,
            MainStandingPortraitEmojiAnchor = mainEmojiAnchor,
            MainStandingPortraitEmojiImage  = mainEmojiImage,

            // SubLeft Standing Portrait
            SubLeftStandingPortraitRoot       = subLeftRoot,
            SubLeftStandingPortraitTrack      = subLeftTrack,
            SubLeftStandingPortraitRig        = subLeftRig,
            SubLeftStandingPortraitSwayPivot  = subLeftSwayPivot,
            SubLeftStandingPortraitShake      = subLeftShake,
            SubLeftStandingPortraitScale      = subLeftScale,
            SubLeftStandingPortraitVisual     = subLeftVisual,

            SubLeftStandingPortraitImage       = subLeftImage,
            SubLeftStandingPortraitEmojiAnchor = subLeftEmojiAnchor,
            SubLeftStandingPortraitEmojiImage  = subLeftEmojiImage,

            // SubRight Standing Portrait
            SubRightStandingPortraitRoot       = subRightRoot,
            SubRightStandingPortraitTrack      = subRightTrack,
            SubRightStandingPortraitRig        = subRightRig,
            SubRightStandingPortraitSwayPivot  = subRightSwayPivot,
            SubRightStandingPortraitShake      = subRightShake,
            SubRightStandingPortraitScale      = subRightScale,
            SubRightStandingPortraitVisual     = subRightVisual,

            SubRightStandingPortraitImage       = subRightImage,
            SubRightStandingPortraitEmojiAnchor = subRightEmojiAnchor,
            SubRightStandingPortraitEmojiImage  = subRightEmojiImage,

            // Protagonist Cutin (Image만)
            ProtagonistCutinImage       = pcImage,

            // Background (정식 위젯)
            BackgroundImage             = bgImage?.Image,
            BackgroundImage2            = bgImage2?.Image,
            //BackgroundImage3            = bgImage3?.Image,

            // Choice Panel + 3 Choices
            ChoicePanelRoot   = choicePanelRootHandle?.RectTransform,
            ChoiceButton0Root = choiceButton0Handle,
            ChoiceButton1Root = choiceButton1Handle,
            ChoiceButton2Root = choiceButton2Handle,
        };

        return true;
    }
}
