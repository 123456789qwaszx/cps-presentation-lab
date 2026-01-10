using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum CpsGraphicTarget
{
    Auto = 0,          // PortraitGraphic 있으면 그거, 없으면 PortraitImage, 없으면 EmojiImage
    PortraitImage,
    EmojiImage,
}

[Serializable]
public sealed class FadeCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public CpsGraphicTarget target = CpsGraphicTarget.Auto;

    [Header("Fade")]
    [Range(0f, 1f)]
    public float toAlpha = 1f;

    /// <summary>
    /// fromAlpha < 0 이면 "현재 알파에서 시작"
    /// </summary>
    public float fromAlpha = -1f;

    /// <summary>
    /// <= 0 이면 Config 기본값 사용
    /// </summary>
    public float duration = -1f;

    public Ease ease = Ease.OutCubic;

    /// <summary>
    /// true면 페이드가 끝날 때까지 Step 진행을 멈춤
    /// </summary>
    public bool wait = false;
}


public sealed class CpsFadeCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly CpsGraphicTarget _target;
    private readonly float _toAlpha;
    private readonly float _fromAlpha;   // <0 이면 현재값
    private readonly float _duration;
    private readonly Ease  _ease;
    private readonly bool  _wait;

    private Graphic _graphic;
    private bool _resolved;

    public CpsFadeCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        CpsGraphicTarget target,
        float toAlpha,
        float duration,
        Ease ease = Ease.OutCubic,
        bool waitForCompletion = false,
        float fromAlpha = -1f)
    {
        _widgets  = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target   = target;
        _toAlpha  = Mathf.Clamp01(toAlpha);
        _fromAlpha = fromAlpha;                 // sentinel 허용
        _duration = Mathf.Max(0f, duration);
        _ease     = ease;
        _wait     = waitForCompletion;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        _graphic.DOKill(false);

        // 시작 알파: 지정됐으면 강제, 아니면 현재값 유지
        if (_fromAlpha >= 0f)
            SetAlpha(_graphic, Mathf.Clamp01(_fromAlpha));

        // duration 0이면 즉시
        if (_duration <= 0f)
        {
            SetAlpha(_graphic, _toAlpha);
            yield break;
        }

        Tween tween = _graphic
            .DOFade(_toAlpha, _duration)
            .SetEase(_ease)
            .SetUpdate(true);

        tween.BindToStep(scope);

        if (_wait)
            yield return tween.WaitForCompletion();
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        _graphic.DOKill(false);
        SetAlpha(_graphic, _toAlpha);
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _graphic != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out var refs) || refs == null)
            return false;

        _graphic = ResolveTargetGraphic(refs, _target);
        return _graphic != null;
    }

    private static Graphic ResolveTargetGraphic(IDialogueWidgetAccess.WidgetRefs refs, CpsGraphicTarget target)
    {
        if (refs == null) return null;

        switch (target)
        {
            case CpsGraphicTarget.PortraitImage:
                return refs.PortraitImage; // Image는 Graphic 상속

            case CpsGraphicTarget.EmojiImage:
                // WidgetRefs에 EmojiImage/EmojiGraphic이 없다면 네 구조에 맞게 추가 필요
                return refs.EmojiImage; // <- 없으면 이 줄 컴파일 에러: 아래 Auto에서도 같이 제거

            case CpsGraphicTarget.Auto:
            default:
                if (refs.PortraitImage != null) return refs.PortraitImage;
                if (refs.EmojiImage != null) return refs.EmojiImage;
                return null;
        }
    }

    private static void SetAlpha(Graphic g, float a01)
    {
        if (g == null) return;
        Color c = g.color;
        c.a = Mathf.Clamp01(a01);
        g.color = c;
    }
}
