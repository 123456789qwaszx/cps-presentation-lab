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
    
    
    public static T SetAlpha<T>(this T graphic, float alpha) where T : Graphic
    {
        if (graphic == null) return null;

        Color c = graphic.color;
        c.a = alpha;
        graphic.color = c;
        
        return graphic;
    }
}
