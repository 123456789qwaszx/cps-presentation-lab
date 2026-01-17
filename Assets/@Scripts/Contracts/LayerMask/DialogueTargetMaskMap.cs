using System;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueTargetMaskMap
{
    // Primitive flag -> RectTransform getter
    // (Group masks are resolved automatically because they are ORs of primitive bits.)
    private static readonly (DialogueTargetMask flag, Func<IDialogueWidgetAccess.WidgetRefs, RectTransform> get)[] Map =
    {
        // ---------------------------
        // Core UI
        // ---------------------------
        (DialogueTargetMask.DialogueBox00Root,      r => r.DialogueBox00Root),
        (DialogueTargetMask.SpeakerNameBox00Root,   r => r.SpeakerNameBox00Root),
        (DialogueTargetMask.ProtagonistCutin00Root, r => r.ProtagonistCutin00Root),

        // ---------------------------
        // Standing 00
        // ---------------------------
        (DialogueTargetMask.Standing00PortraitRoot,         r => r.Standing00PortraitRoot),
        (DialogueTargetMask.Standing00PortraitOverlaysRoot, r => r.Standing00PortraitOverlaysRoot),

        (DialogueTargetMask.Standing00Emoji00Root, r => r.Standing00Emoji00Root),
        (DialogueTargetMask.Standing00Emoji01Root, r => r.Standing00Emoji01Root),
        (DialogueTargetMask.Standing00Emoji02Root, r => r.Standing00Emoji02Root),
        (DialogueTargetMask.Standing00Emoji03Root, r => r.Standing00Emoji03Root),

        // ---------------------------
        // Standing 01
        // ---------------------------
        (DialogueTargetMask.Standing01PortraitRoot,         r => r.Standing01PortraitRoot),
        (DialogueTargetMask.Standing01PortraitOverlaysRoot, r => r.Standing01PortraitOverlaysRoot),

        (DialogueTargetMask.Standing01Emoji00Root, r => r.Standing01Emoji00Root),
        (DialogueTargetMask.Standing01Emoji01Root, r => r.Standing01Emoji01Root),
        (DialogueTargetMask.Standing01Emoji02Root, r => r.Standing01Emoji02Root),
        (DialogueTargetMask.Standing01Emoji03Root, r => r.Standing01Emoji03Root),

        // ---------------------------
        // Standing 02
        // ---------------------------
        (DialogueTargetMask.Standing02PortraitRoot,         r => r.Standing02PortraitRoot),
        (DialogueTargetMask.Standing02PortraitOverlaysRoot, r => r.Standing02PortraitOverlaysRoot),

        (DialogueTargetMask.Standing02Emoji00Root, r => r.Standing02Emoji00Root),
        (DialogueTargetMask.Standing02Emoji01Root, r => r.Standing02Emoji01Root),
        (DialogueTargetMask.Standing02Emoji02Root, r => r.Standing02Emoji02Root),
        (DialogueTargetMask.Standing02Emoji03Root, r => r.Standing02Emoji03Root),

        // ---------------------------
        // Choices
        // ---------------------------
        (DialogueTargetMask.ChoicePanel00Root,  r => r.ChoicePanel00Root),
        (DialogueTargetMask.ChoiceButton00Root, r => r.ChoiceButton00Root),
        (DialogueTargetMask.ChoiceButton01Root, r => r.ChoiceButton01Root),
        (DialogueTargetMask.ChoiceButton02Root, r => r.ChoiceButton02Root),
    };

    /// <summary>
    /// DialogueTargetMask -> RectTransform list (roots only).
    /// No HasFlag boxing; uses bit checks.
    /// </summary>
    public static void CollectRects(
        IDialogueWidgetAccess.WidgetRefs refs,
        DialogueTargetMask mask,
        List<RectTransform> outRects,
        bool dedupe = false)
    {
        outRects.Clear();

        if (refs == null || mask == DialogueTargetMask.None)
            return;

        if (!dedupe)
        {
            for (int i = 0; i < Map.Length; i++)
            {
                var (flag, getter) = Map[i];
                if ((mask & flag) == 0) continue;

                RectTransform rt = getter(refs);
                if (rt != null) outRects.Add(rt);
            }

            return;
        }

        // Optional dedupe (safe even if you accidentally map same rect twice)
        for (int i = 0; i < Map.Length; i++)
        {
            var (flag, getter) = Map[i];
            if ((mask & flag) == 0) continue;

            RectTransform rt = getter(refs);
            if (rt == null) continue;

            if (!outRects.Contains(rt))
                outRects.Add(rt);
        }
    }
}