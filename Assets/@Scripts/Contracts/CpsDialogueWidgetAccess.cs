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

        string role = widgetRoleKey ?? string.Empty;
        var cacheKey = (screen, role);

        if (_widgetRefsCache.TryGetValue(cacheKey, out refs) && refs != null)
            return true;

        DialogueRoleWidgetTags tags = GetDialogueRoleWidgetTags(role);

        refs = new IDialogueWidgetAccess.WidgetRefs
        {
            // ======================================================
            // Dialogue
            // ======================================================
            DialogueBox00Root  = screen.GetWidgetDirect<RectTransform>(tags.DialogueBox00RootTag),
            DialogueBox00Image = screen.GetWidgetDirect<Image>(tags.DialogueBox00ImageTag),
            DialogueBox00Text  = screen.GetWidgetDirect<TMP_Text>(tags.DialogueBox00TextTag),

            SpeakerNameBox00Root  = screen.GetWidgetDirect<RectTransform>(tags.SpeakerNameBox00RootTag),
            SpeakerNameBox00Image = screen.GetWidgetDirect<Image>(tags.SpeakerNameBox00ImageTag),
            SpeakerNameBox00Text  = screen.GetWidgetDirect<TMP_Text>(tags.SpeakerNameBox00TextTag),

            ProtagonistCutin00Root  = screen.GetWidgetDirect<RectTransform>(tags.ProtagonistCutin00RootTag),
            ProtagonistCutin00Image = screen.GetWidgetDirect<Image>(tags.ProtagonistCutin00ImageTag),

            // ======================================================
            // Standing00
            // ======================================================
            Standing00Root  = screen.GetWidgetDirect<RectTransform>(tags.Standing00RootTag),
            Standing00Track = screen.GetWidgetDirect<RectTransform>(tags.Standing00TrackTag),

            Standing00PortraitRoot      = screen.GetWidgetDirect<RectTransform>(tags.Standing00PortraitRootTag),
            Standing00PortraitSwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing00PortraitSwayPivotTag),
            Standing00PortraitShake     = screen.GetWidgetDirect<RectTransform>(tags.Standing00PortraitShakeTag),
            Standing00PortraitScale     = screen.GetWidgetDirect<RectTransform>(tags.Standing00PortraitScaleTag),
            Standing00PortraitVisual    = screen.GetWidgetDirect<RectTransform>(tags.Standing00PortraitVisualTag),
            Standing00PortraitImage     = screen.GetWidgetDirect<Image>(tags.Standing00PortraitImageTag),

            Standing00Emoji00Root      = screen.GetWidgetDirect<RectTransform>(tags.Standing00Emoji00RootTag),
            Standing00Emoji00Anchor    = screen.GetWidgetDirect<RectTransform>(tags.Standing00Emoji00AnchorTag),
            Standing00Emoji00SwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing00Emoji00SwayPivotTag),
            Standing00Emoji00Image     = screen.GetWidgetDirect<Image>(tags.Standing00Emoji00ImageTag),

            Standing00Emoji01Root      = screen.GetWidgetDirect<RectTransform>(tags.Standing00Emoji01RootTag),
            Standing00Emoji01Anchor    = screen.GetWidgetDirect<RectTransform>(tags.Standing00Emoji01AnchorTag),
            Standing00Emoji01SwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing00Emoji01SwayPivotTag),
            Standing00Emoji01Image     = screen.GetWidgetDirect<Image>(tags.Standing00Emoji01ImageTag),

            Standing00Emoji02Root      = screen.GetWidgetDirect<RectTransform>(tags.Standing00Emoji02RootTag),
            Standing00Emoji02Anchor    = screen.GetWidgetDirect<RectTransform>(tags.Standing00Emoji02AnchorTag),
            Standing00Emoji02SwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing00Emoji02SwayPivotTag),
            Standing00Emoji02Image     = screen.GetWidgetDirect<Image>(tags.Standing00Emoji02ImageTag),

            Standing00Emoji03Root      = screen.GetWidgetDirect<RectTransform>(tags.Standing00Emoji03RootTag),
            Standing00Emoji03Anchor    = screen.GetWidgetDirect<RectTransform>(tags.Standing00Emoji03AnchorTag),
            Standing00Emoji03SwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing00Emoji03SwayPivotTag),
            Standing00Emoji03Image     = screen.GetWidgetDirect<Image>(tags.Standing00Emoji03ImageTag),

            // ======================================================
            // Standing01
            // ======================================================
            Standing01Root  = screen.GetWidgetDirect<RectTransform>(tags.Standing01RootTag),
            Standing01Track = screen.GetWidgetDirect<RectTransform>(tags.Standing01TrackTag),

            Standing01PortraitRoot      = screen.GetWidgetDirect<RectTransform>(tags.Standing01PortraitRootTag),
            Standing01PortraitSwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing01PortraitSwayPivotTag),
            Standing01PortraitShake     = screen.GetWidgetDirect<RectTransform>(tags.Standing01PortraitShakeTag),
            Standing01PortraitScale     = screen.GetWidgetDirect<RectTransform>(tags.Standing01PortraitScaleTag),
            Standing01PortraitVisual    = screen.GetWidgetDirect<RectTransform>(tags.Standing01PortraitVisualTag),
            Standing01PortraitImage     = screen.GetWidgetDirect<Image>(tags.Standing01PortraitImageTag),

            Standing01Emoji00Root      = screen.GetWidgetDirect<RectTransform>(tags.Standing01Emoji00RootTag),
            Standing01Emoji00Anchor    = screen.GetWidgetDirect<RectTransform>(tags.Standing01Emoji00AnchorTag),
            Standing01Emoji00SwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing01Emoji00SwayPivotTag),
            Standing01Emoji00Image     = screen.GetWidgetDirect<Image>(tags.Standing01Emoji00ImageTag),

            Standing01Emoji01Root      = screen.GetWidgetDirect<RectTransform>(tags.Standing01Emoji01RootTag),
            Standing01Emoji01Anchor    = screen.GetWidgetDirect<RectTransform>(tags.Standing01Emoji01AnchorTag),
            Standing01Emoji01SwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing01Emoji01SwayPivotTag),
            Standing01Emoji01Image     = screen.GetWidgetDirect<Image>(tags.Standing01Emoji01ImageTag),

            Standing01Emoji02Root      = screen.GetWidgetDirect<RectTransform>(tags.Standing01Emoji02RootTag),
            Standing01Emoji02Anchor    = screen.GetWidgetDirect<RectTransform>(tags.Standing01Emoji02AnchorTag),
            Standing01Emoji02SwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing01Emoji02SwayPivotTag),
            Standing01Emoji02Image     = screen.GetWidgetDirect<Image>(tags.Standing01Emoji02ImageTag),

            Standing01Emoji03Root      = screen.GetWidgetDirect<RectTransform>(tags.Standing01Emoji03RootTag),
            Standing01Emoji03Anchor    = screen.GetWidgetDirect<RectTransform>(tags.Standing01Emoji03AnchorTag),
            Standing01Emoji03SwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing01Emoji03SwayPivotTag),
            Standing01Emoji03Image     = screen.GetWidgetDirect<Image>(tags.Standing01Emoji03ImageTag),

            // ======================================================
            // Standing02
            // ======================================================
            Standing02Root  = screen.GetWidgetDirect<RectTransform>(tags.Standing02RootTag),
            Standing02Track = screen.GetWidgetDirect<RectTransform>(tags.Standing02TrackTag),

            Standing02PortraitRoot      = screen.GetWidgetDirect<RectTransform>(tags.Standing02PortraitRootTag),
            Standing02PortraitSwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing02PortraitSwayPivotTag),
            Standing02PortraitShake     = screen.GetWidgetDirect<RectTransform>(tags.Standing02PortraitShakeTag),
            Standing02PortraitScale     = screen.GetWidgetDirect<RectTransform>(tags.Standing02PortraitScaleTag),
            Standing02PortraitVisual    = screen.GetWidgetDirect<RectTransform>(tags.Standing02PortraitVisualTag),
            Standing02PortraitImage     = screen.GetWidgetDirect<Image>(tags.Standing02PortraitImageTag),

            Standing02Emoji00Root      = screen.GetWidgetDirect<RectTransform>(tags.Standing02Emoji00RootTag),
            Standing02Emoji00Anchor    = screen.GetWidgetDirect<RectTransform>(tags.Standing02Emoji00AnchorTag),
            Standing02Emoji00SwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing02Emoji00SwayPivotTag),
            Standing02Emoji00Image     = screen.GetWidgetDirect<Image>(tags.Standing02Emoji00ImageTag),

            Standing02Emoji01Root      = screen.GetWidgetDirect<RectTransform>(tags.Standing02Emoji01RootTag),
            Standing02Emoji01Anchor    = screen.GetWidgetDirect<RectTransform>(tags.Standing02Emoji01AnchorTag),
            Standing02Emoji01SwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing02Emoji01SwayPivotTag),
            Standing02Emoji01Image     = screen.GetWidgetDirect<Image>(tags.Standing02Emoji01ImageTag),

            Standing02Emoji02Root      = screen.GetWidgetDirect<RectTransform>(tags.Standing02Emoji02RootTag),
            Standing02Emoji02Anchor    = screen.GetWidgetDirect<RectTransform>(tags.Standing02Emoji02AnchorTag),
            Standing02Emoji02SwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing02Emoji02SwayPivotTag),
            Standing02Emoji02Image     = screen.GetWidgetDirect<Image>(tags.Standing02Emoji02ImageTag),

            Standing02Emoji03Root      = screen.GetWidgetDirect<RectTransform>(tags.Standing02Emoji03RootTag),
            Standing02Emoji03Anchor    = screen.GetWidgetDirect<RectTransform>(tags.Standing02Emoji03AnchorTag),
            Standing02Emoji03SwayPivot = screen.GetWidgetDirect<RectTransform>(tags.Standing02Emoji03SwayPivotTag),
            Standing02Emoji03Image     = screen.GetWidgetDirect<Image>(tags.Standing02Emoji03ImageTag),

            // ======================================================
            // Background
            // ======================================================
            Background00Root  = screen.GetWidgetDirect<RectTransform>(tags.Background00RootTag),
            Background00Image = screen.GetWidgetDirect<Image>(tags.Background00ImageTag),

            Background01Root  = screen.GetWidgetDirect<RectTransform>(tags.Background01RootTag),
            Background01Image = screen.GetWidgetDirect<Image>(tags.Background01ImageTag),

            // ======================================================
            // Choice
            // ======================================================
            ChoicePanel00Root  = screen.GetWidgetDirect<RectTransform>(tags.ChoicePanel00RootTag),
            ChoicePanel00Image = screen.GetWidgetDirect<Image>(tags.ChoicePanel00ImageTag),

            ChoiceButton00Root  = screen.GetWidgetDirect<RectTransform>(tags.ChoiceButton00RootTag),
            ChoiceButton00Image = screen.GetWidgetDirect<Image>(tags.ChoiceButton00ImageTag),
            ChoiceButton00Text  = screen.GetWidgetDirect<TMP_Text>(tags.ChoiceButton00TextTag),

            ChoiceButton01Root  = screen.GetWidgetDirect<RectTransform>(tags.ChoiceButton01RootTag),
            ChoiceButton01Image = screen.GetWidgetDirect<Image>(tags.ChoiceButton01ImageTag),
            ChoiceButton01Text  = screen.GetWidgetDirect<TMP_Text>(tags.ChoiceButton01TextTag),

            ChoiceButton02Root  = screen.GetWidgetDirect<RectTransform>(tags.ChoiceButton02RootTag),
            ChoiceButton02Image = screen.GetWidgetDirect<Image>(tags.ChoiceButton02ImageTag),
            ChoiceButton02Text  = screen.GetWidgetDirect<TMP_Text>(tags.ChoiceButton02TextTag),

            // ======================================================
            // VFX
            // ======================================================
            VFXBlackScreen00Root   = screen.GetWidgetDirect<RectTransform>(tags.VFXBlackScreen00RootTag),
            VFXBlackScreen00Image0 = screen.GetWidgetDirect<Image>(tags.VFXBlackScreen00Image0Tag),
            VFXBlackScreen00Image1 = screen.GetWidgetDirect<Image>(tags.VFXBlackScreen00Image1Tag),

            VFXBlackFade00Root  = screen.GetWidgetDirect<RectTransform>(tags.VFXBlackFade00RootTag),
            VFXBlackFade00Image = screen.GetWidgetDirect<Image>(tags.VFXBlackFade00ImageTag),

            // ======================================================
            // Toggle
            // ======================================================
            TogglePanel00Root = screen.GetWidgetDirect<RectTransform>(tags.TogglePanel00RootTag),

            SkipToggle00Root   = screen.GetWidgetDirect<RectTransform>(tags.SkipToggle00RootTag),
            SkipToggle00Image0 = screen.GetWidgetDirect<Image>(tags.SkipToggle00Image0Tag),
            SkipToggle00Image1 = screen.GetWidgetDirect<Image>(tags.SkipToggle00Image1Tag),
            SkipToggle00Text   = screen.GetWidgetDirect<TMP_Text>(tags.SkipToggle00TextTag),

            NextToggle00Root  = screen.GetWidgetDirect<RectTransform>(tags.NextToggle00RootTag),
            NextToggle00Image = screen.GetWidgetDirect<Image>(tags.NextToggle00ImageTag),
            NextToggle00Text  = screen.GetWidgetDirect<TMP_Text>(tags.NextToggle00TextTag),

            AutoToggle00Root   = screen.GetWidgetDirect<RectTransform>(tags.AutoToggle00RootTag),
            AutoToggle00Image0 = screen.GetWidgetDirect<Image>(tags.AutoToggle00Image0Tag),
            AutoToggle00Image1 = screen.GetWidgetDirect<Image>(tags.AutoToggle00Image1Tag),

            SpeedupToggle00Root   = screen.GetWidgetDirect<RectTransform>(tags.SpeedupToggle00RootTag),
            SpeedupToggle00Image0 = screen.GetWidgetDirect<Image>(tags.SpeedupToggle00Image0Tag),
            SpeedupToggle00Image1 = screen.GetWidgetDirect<Image>(tags.SpeedupToggle00Image1Tag),

            SetSpeedToggle00Root   = screen.GetWidgetDirect<RectTransform>(tags.SetSpeedToggle00RootTag),
            SetSpeedToggle00Image0 = screen.GetWidgetDirect<Image>(tags.SetSpeedToggle00Image0Tag),
            SetSpeedToggle00Image1 = screen.GetWidgetDirect<Image>(tags.SetSpeedToggle00Image1Tag),
            SetSpeedToggle00Image2 = screen.GetWidgetDirect<Image>(tags.SetSpeedToggle00Image2Tag),
            SetSpeedToggle00Image3 = screen.GetWidgetDirect<Image>(tags.SetSpeedToggle00Image3Tag),
        };

        _widgetRefsCache[cacheKey] = refs;
        return true;
    }

    private DialogueRoleWidgetTags GetDialogueRoleWidgetTags(string roleKey)
    {
        roleKey ??= string.Empty;

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

        for (int i = 0; i < keysToRemove.Count; i++)
            _widgetRefsCache.Remove(keysToRemove[i]);
    }
}