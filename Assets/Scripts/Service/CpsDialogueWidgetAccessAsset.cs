using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Lab.UI.Naming;
using Lab.UI.Keys;

[CreateAssetMenu(fileName = "CpsDialogueWidgetAccess", menuName = "Dialogue/Services/CPS Widget Access")]
public sealed class CpsDialogueWidgetAccessAsset : ScriptableObject, IDialogueWidgetAccess
{
    [SerializeField]public string defaultRefKey;
    public bool TryResolve(string screenId, string widgetRefKey, out IDialogueWidgetAccess.WidgetRefs refs)
    {
        refs = null;

        UIRouter router = UIRuntimeRouter.Router;
        if (router == null)
        {
            Debug.LogWarning("[CpsDialogueWidgetAccess] UIRuntimeRouter. Router is null. ");
            return false;
        }

        ScreenKey key = new ScreenKey(screenId);
        if (!router.TryGetScreen(key, out UIScreen screen))
        {
            Debug.LogWarning(
                $"[CpsDialogueWidgetAccess] Failed to resolve UIScreen. screenId='{screenId}', ScreenKey='{key}'");
            return false;
        }
        
        if (string.IsNullOrEmpty(widgetRefKey))
            widgetRefKey = defaultRefKey; // 혹은 screenId별 기본값
        
        DialogueRoleWidgetTags set = new (widgetRefKey);
        
        WidgetHandle lineText         = screen.GetWidgetHandle(set.LineTextTag);
        WidgetHandle lineBodyImage    = screen.GetWidgetHandle(set.LineBodyTag);
        WidgetHandle speakerName      = screen.GetWidgetHandle(set.SpeakerNameTag);
        WidgetHandle standingPortrait = screen.GetWidgetHandle(set.StandingPortraitTag);
        WidgetHandle protagonistCutin = screen.GetWidgetHandle(set.ProtagonistCutinTag);
        WidgetHandle background       = screen.GetWidgetHandle(set.BackgroundImageTag);
        
        refs = new IDialogueWidgetAccess.WidgetRefs
        {
            LineText        = lineText?.Text,
            LineBodyImage        = lineBodyImage.Image,
            SpeakerNameText        = speakerName?.Text,
            StandingPortraitImage   = standingPortrait?.Image,
            StandingPortraitRect    = standingPortrait?.RectTransform,
            ProtagonistCutinImage      = protagonistCutin?.Image,
            ProtagonistCutinRect       = protagonistCutin?.RectTransform,
            BackgroundImage = background?.Image,
            BackgroundRect  = background?.RectTransform,
        };
        
        return true;
    }
}