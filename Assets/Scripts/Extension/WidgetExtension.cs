using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class WidgetExtension
{
    private static readonly Dictionary<int, Vector2> _originPos = new();
    
    public static Sequence SlideInFromLeftWithFade(
        this RectTransform rectTr,
        Graphic graphic,
        Vector2 destinationPos,
        float duration = 0.5f,
        float startPosOffsetX = 800f,
        float overshootStrength = 1.2f,
        float alphaStart = 0.2f,
        float alphaLeadTime = 0.1f,
        float endFadeValue = 1f,
        Action callback = null)
    {
        if (rectTr == null) return null;

        rectTr.DOKill();
        graphic?.DOKill();

        rectTr.anchoredPosition = destinationPos + new Vector2(-startPosOffsetX, 0f);

        float alphaDuration = Mathf.Max(0f, duration - alphaLeadTime);

        Sequence seq = DOTween.Sequence()
            .SetUpdate(true)
            .Append(rectTr.DOAnchorPos(destinationPos, duration)
                .SetEase(Ease.OutBack, overshootStrength));

        Tween fade = graphic?.SetAlpha(alphaStart)
            ?.DOFade(endFadeValue, alphaDuration)
            ?.SetEase(Ease.OutQuad);

        if (fade != null)
            seq.Join(fade);

        if (callback != null)
            seq.OnComplete(callback.Invoke);

        return seq;
    }
    
    public static Sequence SlideInFromLeftWithFade(
        this RectTransform rectTr,
        float duration = 0.5f,
        float startPosOffsetX = 800f,
        float overshootStrength = 1.2f,
        float alphaStart = 0.2f,
        float alphaLeadTime = 0.1f,
        float endFadeValue = 1f,
        Action callback = null)
    {
        if (rectTr == null) return null;

        var graphic = rectTr.GetComponent<Graphic>();
        var destinationPos = rectTr.GetOrCaptureOrigin();

        return rectTr.SlideInFromLeftWithFade(
            graphic,
            destinationPos,
            duration, startPosOffsetX, overshootStrength,
            alphaStart, alphaLeadTime, endFadeValue, callback);
    }
    
    public static void ApplySlideInFinal(this RectTransform rt)
    {
        if (rt == null) return;

        rt.DOKill();

        var g = rt.GetComponent<Graphic>();
        if (g != null)
        {
            g.DOKill();
            g.SetAlpha(1f);
        }

        // 목적지는 "origin"으로 맞추는 편이 안전
        rt.anchoredPosition = rt.GetOrCaptureOrigin();
    }
    
    
    public static void CaptureOrigin(this RectTransform rt)
    {
        if (rt == null) return;
        _originPos[rt.GetInstanceID()] = rt.anchoredPosition;
    }

    private static Vector2 GetOrCaptureOrigin(this RectTransform rt)
    {
        int id = rt.GetInstanceID();
        if (!_originPos.TryGetValue(id, out var pos))
        {
            pos = rt.anchoredPosition;
            _originPos[id] = pos;
        }
        return pos;
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
