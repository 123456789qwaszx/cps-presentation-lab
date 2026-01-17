using System.Collections.Generic;
using Lab.UI.Naming;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CpsDialogueWidgetAccess : IDialogueWidgetAccess
{
    private readonly Dictionary<(UIScreen screen, string roleKey), IDialogueWidgetAccess.WidgetRefs> _widgetRefsCache = new();
    private readonly Dictionary<string, DialogueRoleWidgetTags> _widgetTagsCache = new();

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

        string role = widgetRoleKey;
        var cacheKey = (screen, role);

        if (_widgetRefsCache.TryGetValue(cacheKey, out refs) && refs != null)
            return true;

        DialogueRoleWidgetTags set = GetDialogueRoleWidgetTags(role);

        refs = new IDialogueWidgetAccess.WidgetRefs
        {
            // ---- Dialogue Line ----
            DialogueBoxRoot       = screen.GetWidgetDirect<RectTransform>(set.DialogueBoxRootTag),
            LineText              = screen.GetWidgetDirect<TMP_Text>(set.LineTextTag),
            LineBodyImage         = screen.GetWidgetDirect<Image>(set.LineBodyTag),
            SpeakerNameBox        = screen.GetWidgetDirect<Image>(set.SpeakerNameBoxTag),
            SpeakerNameText       = screen.GetWidgetDirect<TMP_Text>(set.SpeakerNameTag),
            ProtagonistCutinImage = screen.GetWidgetDirect<Image>(set.ProtagonistCutinImageTag),

            // ---- Main Standing Portrait ----
            MainStandingPortraitRoot        = screen.GetWidgetDirect<RectTransform>(set.MainStandingPortraitRootTag),
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
            SubLeftStandingPortraitRoot        = screen.GetWidgetDirect<RectTransform>(set.SubLeftStandingPortraitRootTag),
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
            SubRightStandingPortraitRoot        = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitRootTag),
            SubRightStandingPortraitTrack       = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitTrackTag),
            SubRightStandingPortraitRig         = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitRigTag),
            SubRightStandingPortraitSwayPivot   = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitSwayPivotTag),
            SubRightStandingPortraitShake       = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitShakeTag),
            SubRightStandingPortraitScale       = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitScaleTag),
            SubRightStandingPortraitVisual      = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitVisualTag),
            SubRightStandingPortraitImage       = screen.GetWidgetDirect<Image>(set.SubRightStandingPortraitImageTag),
            SubRightStandingPortraitEmojiAnchor = screen.GetWidgetDirect<RectTransform>(set.SubRightStandingPortraitEmojiAnchorTag),
            SubRightStandingPortraitEmojiImage  = screen.GetWidgetDirect<Image>(set.SubRightStandingPortraitEmojiImageTag),

            // ---- Background (00/01) ----
            BackgroundRoot00  = screen.GetWidgetDirect<RectTransform>(set.BackgroundRoot00Tag),
            BackgroundRoot01  = screen.GetWidgetDirect<RectTransform>(set.BackgroundRoot01Tag),
            BackgroundImage00 = screen.GetWidgetDirect<Image>(set.BackgroundImage00Tag),
            BackgroundImage01 = screen.GetWidgetDirect<Image>(set.BackgroundImage01Tag),

            // ---- Choice Panel ----
            ChoicePanelRoot    = screen.GetWidgetDirect<RectTransform>(set.ChoicePanelRootTag),
            ChoiceButton0Root  = screen.GetWidgetDirect<RectTransform>(set.ChoiceButton0RootTag),
            ChoiceButton1Root  = screen.GetWidgetDirect<RectTransform>(set.ChoiceButton1RootTag),
            ChoiceButton2Root  = screen.GetWidgetDirect<RectTransform>(set.ChoiceButton2RootTag),

            ChoiceButton0Text  = screen.GetWidgetDirect<TMP_Text>(set.ChoiceButton0TextTag),
            ChoiceButton1Text  = screen.GetWidgetDirect<TMP_Text>(set.ChoiceButton1TextTag),
            ChoiceButton2Text  = screen.GetWidgetDirect<TMP_Text>(set.ChoiceButton2TextTag),

            ChoiceButton0Image = screen.GetWidgetDirect<Image>(set.ChoiceButton0ImageTag),
            ChoiceButton1Image = screen.GetWidgetDirect<Image>(set.ChoiceButton1ImageTag),
            ChoiceButton2Image = screen.GetWidgetDirect<Image>(set.ChoiceButton2ImageTag),

            // ---- VFX Panel ----
            VFXBlackScreen00Root = screen.GetWidgetDirect<RectTransform>(set.VFXBlackScreen00RootTag),
            VFXBlackOut00Image   = screen.GetWidgetDirect<Image>(set.VFXBlackOut00ImageTag),
            VFXBlackOut01Image   = screen.GetWidgetDirect<Image>(set.VFXBlackOut01ImageTag),

            BlackFade00Root      = screen.GetWidgetDirect<RectTransform>(set.BlackFade00RootTag),
            BlackFade00Image     = screen.GetWidgetDirect<Image>(set.BlackFade00ImageTag),

            // ---- Skip Toggle ----
            SkipToggle00Root     = screen.GetWidgetDirect<RectTransform>(set.SkipToggle00RootTag),
            SkipToggle00Image    = screen.GetWidgetDirect<Image>(set.SkipToggle00ImageTag),
            SkipToggle01Image    = screen.GetWidgetDirect<Image>(set.SkipToggle01ImageTag),
            SkipToggle00Text     = screen.GetWidgetDirect<TMP_Text>(set.SkipToggle00TextTag),

            // ---- Next Toggle ----
            NextToggle00Root     = screen.GetWidgetDirect<RectTransform>(set.NextToggle00RootTag),
            NextToggle00Image    = screen.GetWidgetDirect<Image>(set.NextToggle00ImageTag),
            NextToggle00Text     = screen.GetWidgetDirect<TMP_Text>(set.NextToggle00TextTag),

            // ---- Auto Toggle ----
            AutoToggle00Root     = screen.GetWidgetDirect<RectTransform>(set.AutoToggle00RootTag),
            AutoToggle00Image    = screen.GetWidgetDirect<Image>(set.AutoToggle00ImageTag),
            AutoToggle01Image    = screen.GetWidgetDirect<Image>(set.AutoToggle01ImageTag),

            // ---- Speedup Toggle ----
            SpeedupToggle00Root  = screen.GetWidgetDirect<RectTransform>(set.SpeedupToggle00RootTag),
            SpeedupToggle00Image = screen.GetWidgetDirect<Image>(set.SpeedupToggle00ImageTag),
            SpeedupToggle01Image = screen.GetWidgetDirect<Image>(set.SpeedupToggle01ImageTag),

            // ---- SetSpeed Toggle ----
            SetSpeedToggle00Root  = screen.GetWidgetDirect<RectTransform>(set.SetSpeedToggle00RootTag),
            SetSpeedToggle00Image = screen.GetWidgetDirect<Image>(set.SetSpeedToggle00ImageTag),
            SetSpeedToggle01Image = screen.GetWidgetDirect<Image>(set.SetSpeedToggle01ImageTag),
            SetSpeedToggle02Image = screen.GetWidgetDirect<Image>(set.SetSpeedToggle02ImageTag),
            SetSpeedToggle03Image = screen.GetWidgetDirect<Image>(set.SetSpeedToggle03ImageTag),
        };

        _widgetRefsCache[cacheKey] = refs;
        return true;
    }

    private DialogueRoleWidgetTags GetDialogueRoleWidgetTags(string roleKey)
    {
        if (_widgetTagsCache.TryGetValue(roleKey, out DialogueRoleWidgetTags tags))
            return tags;

        tags = new DialogueRoleWidgetTags(roleKey);
        _widgetTagsCache[roleKey] = tags;
        return tags;
    }

    public void InvalidateForScreen(UIScreen screen)
    {
        if (screen == null) return;

        var keysToRemove = new List<(UIScreen, string)>();
        foreach (var kv in _widgetRefsCache)
        {
            if (kv.Key.screen == screen)
                keysToRemove.Add(kv.Key);
        }

        foreach (var k in keysToRemove)
            _widgetRefsCache.Remove(k);
    }
}
