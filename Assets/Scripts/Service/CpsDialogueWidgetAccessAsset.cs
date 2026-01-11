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
        WidgetHandle lineText      = screen.GetWidgetHandle(set.LineTextTag);
        WidgetHandle lineBodyImage = screen.GetWidgetHandle(set.LineBodyTag);
        WidgetHandle speakerName   = screen.GetWidgetHandle(set.SpeakerNameTag);

        
        // ---- Standing Portrait (Root는 정식 위젯, Prefab 우회로: GetWidgetDirect) ----
        WidgetHandle spRootHandle = screen.GetWidgetHandle(set.StandingPortraitRootTag);
        RectTransform spRoot      = spRootHandle?.RectTransform;
        
        RectTransform spTrack       = screen.GetWidgetDirect<RectTransform>(set.StandingPortraitTrackTag);
        RectTransform spRig         = screen.GetWidgetDirect<RectTransform>(set.StandingPortraitRigTag);
        RectTransform spSwayPivot   = screen.GetWidgetDirect<RectTransform>(set.StandingPortraitSwayPivotTag);
        RectTransform spShake       = screen.GetWidgetDirect<RectTransform>(set.StandingPortraitShakeTag);
        RectTransform spScale       = screen.GetWidgetDirect<RectTransform>(set.StandingPortraitScaleTag);
        RectTransform spVisual      = screen.GetWidgetDirect<RectTransform>(set.StandingPortraitVisualTag);

        Image         spImage       = screen.GetWidgetDirect<Image>(set.StandingPortraitImageTag);
        RectTransform spEmojiAnchor = screen.GetWidgetDirect<RectTransform>(set.StandingPortraitEmojiAnchorTag);
        Image         spEmojiImage  = screen.GetWidgetDirect<Image>(set.StandingPortraitEmojiImageTag);

        // ---- Protagonist Cutin (현재는 Image만) ----
        WidgetHandle pcImage = screen.GetWidgetHandle(set.ProtagonistCutinImageTag);

        // ---- Background (정식 위젯 경로 유지) ----
        WidgetHandle bgImage = screen.GetWidgetHandle(set.BackgroundImageTag);

        refs = new IDialogueWidgetAccess.WidgetRefs
        {
            // Line
            LineText        = lineText?.Text,
            LineBodyImage   = lineBodyImage?.Image,
            SpeakerNameText = speakerName?.Text,

            // Standing Portrait (우회로)
            StandingPortraitRoot       = spRoot,
            StandingPortraitTrack       = spTrack,
            StandingPortraitRig         = spRig,
            StandingPortraitSwayPivot   = spSwayPivot,
            StandingPortraitShake       = spShake,
            StandingPortraitScale       = spScale,
            StandingPortraitVisual      = spVisual,

            StandingPortraitImage       = spImage,
            StandingPortraitEmojiAnchor = spEmojiAnchor,
            StandingPortraitEmojiImage  = spEmojiImage,

            // Protagonist Cutin (Image만)
            ProtagonistCutinImage       = pcImage?.Image,

            // Background (정식 위젯)
            BackgroundImage             = bgImage?.Image,
        };

        return true;
    }
}
