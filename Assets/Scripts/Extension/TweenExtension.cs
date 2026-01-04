using System;
using DG.Tweening;
using TMPro;

public static class TweenExtension
{
    #region  TextWritter
    
    private const string DialogueTweenId = "DialogueText";

    public static Tween DOTypeText(
        this TMP_Text tmp,
        string fullText,
        float charInterval = 0.03f,
        Action onComplete = null,
        bool useUnscaledTime = true)
    {
        if (tmp == null) return null;

        DOTween.Kill(tmp); // 타겟 기준으로 안전하게 종료

        fullText ??= string.Empty;

        // 전체 텍스트는 먼저 세팅하고, 보이는 글자수만 0->len
        tmp.text = fullText;
        tmp.maxVisibleCharacters = 0;

        int len = fullText.Length;
        float duration = charInterval * len;

        var t = DOTween.To(
                () => tmp.maxVisibleCharacters,
                x => tmp.maxVisibleCharacters = x,
                len,
                duration)
            .SetEase(Ease.Linear)
            .SetTarget(tmp)
            .SetId(DialogueTweenId)
            .SetUpdate(useUnscaledTime);

        if (onComplete != null)
            t.OnComplete(() => onComplete());

        return t;
    }

    // 스킵 전용(최종 상태를 “강제”로 맞추는 게 핵심)
    public static void ApplyTypeTextFinal(this TMP_Text tmp)
    {
        if (tmp == null) return;
        DOTween.Kill(tmp);
        tmp.maxVisibleCharacters = int.MaxValue; // TMP는 텍스트 길이보다 크면 전부 표시
    }
    
    #endregion
}
