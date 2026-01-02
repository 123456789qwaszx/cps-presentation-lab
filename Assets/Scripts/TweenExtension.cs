using System;
using DG.Tweening;
using TMPro;

public static class TweenExtension
{
    #region  TextWritter
    
    public static Tween DOTypeText(
        this TMP_Text tmp,
        string text,
        float charInterval = 0.03f,
        Action onComplete = null)
    {
        DOTween.Kill(tmp);

        tmp.text = string.Empty;
        
        var tween = DOTween.To(
                () => "",
                x => tmp.text = x,
                text,
                charInterval * text.Length)
            .SetEase(Ease.Linear)
            .SetTarget(tmp)
            .SetUpdate(true);

        if (onComplete != null)
            tween.OnComplete(() => onComplete());

        return tween;
    }
    
    #endregion
}
