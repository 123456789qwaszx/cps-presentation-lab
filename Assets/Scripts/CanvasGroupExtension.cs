using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class CanvasGroupExtension
{
    public static void SetAlpha(this CanvasGroup canvasGroup, float alpha, bool setInteract = false)
    {
        if (!canvasGroup) return;
        
        if (alpha > 0.1f)
        {
            canvasGroup.alpha = alpha;
        }
        else
        {
            canvasGroup.alpha = 0;
        }
        
        if (setInteract)
            SetInteractable(canvasGroup, alpha >= 0.1f);
    }
    
    public static void SetInteractable(this CanvasGroup canvasGroup, bool interactable)
    {
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = interactable;
    }
}
