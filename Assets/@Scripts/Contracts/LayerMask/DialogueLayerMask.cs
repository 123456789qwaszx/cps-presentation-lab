using System;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum DialogueLayerMask
{
    None = 0,

    Background00Root = 1 << 0,
    Background01Root = 1 << 1,
    // 나중에 필요하면: Background3 = 1 << 2,

    MainStandingRoot  = 1 << 3,  // 중앙/주인공
    LeftStandingRoot  = 1 << 4,  // 좌측 서브
    RightStandingRoot = 1 << 5,  // 우측 서브

    DialogueBox00Root = 1 << 6,
    ChoicePanel00Root = 1 << 7,
    TogglePanel00Root = 1 << 8,
    
    VFXBlackScreen00Root = 1 << 9,
    VFXBlackFade00Root   = 1 << 10,
    
    // ---- Group masks (단순 그룹) ----
    AllBackgrounds  = Background00Root | Background01Root,
    AllPortraits    = MainStandingRoot | LeftStandingRoot | RightStandingRoot,
    AllUI           = DialogueBox00Root | ChoicePanel00Root | TogglePanel00Root,
    ALLVFX          = VFXBlackScreen00Root | VFXBlackFade00Root,
    
    All             = AllBackgrounds | AllPortraits | AllUI | ALLVFX
}

public static class DialogueLayerRoots
{
    private static readonly (DialogueLayerMask mask, Func<IDialogueWidgetAccess.WidgetRefs, RectTransform> get)[] Map =
    {
        (DialogueLayerMask.Background00Root,     r => r.Background00Root),
        (DialogueLayerMask.Background01Root,     r => r.Background01Root),

        (DialogueLayerMask.MainStandingRoot,     r => r.Standing00Root),
        (DialogueLayerMask.LeftStandingRoot,  r => r.Standing01Root),
        (DialogueLayerMask.RightStandingRoot, r => r.Standing02Root),

        (DialogueLayerMask.DialogueBox00Root,     r => r.DialogueBox00Root),
        (DialogueLayerMask.ChoicePanel00Root,     r => r.ChoicePanel00Root),
        (DialogueLayerMask.TogglePanel00Root,     r => r.TogglePanel00Root),
        (DialogueLayerMask.VFXBlackScreen00Root,  r => r.VFXBlackScreen00Root),
        (DialogueLayerMask.VFXBlackFade00Root,    r => r.VFXBlackFade00Root),
    };

    public static void Collect(
        IDialogueWidgetAccess.WidgetRefs refs,
        DialogueLayerMask layerMask,
        List<RectTransform> outList)
    {
        outList.Clear();
        if (refs == null || layerMask == 0)
            return;

        for (int i = 0; i < Map.Length; i++)
        {
            var (flag, getter) = Map[i];
            if ((layerMask & flag) == 0) continue;

            var rt = getter(refs);
            if (rt != null) outList.Add(rt);
        }
    }
}