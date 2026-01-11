using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CommandMenuHint(
    "Visual/Portrait",
    "Fade Graphic",
    Sets = new[]
    {
        "Custom/Portrait/EnterMain",
        "Custom/Portrait/ExitMain"
    },
    SetOrder = 30,
    Order = 30)]
public sealed class FadeCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.StandingPortraitImage;

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
    private readonly DialogueWidgetTarget _target;

    private readonly float _toAlpha;
    private readonly float _fromAlpha;   // <0 이면 현재값
    private readonly float _duration;
    private readonly Ease  _ease;
    private readonly bool  _wait;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private bool _resolved;

    public CpsFadeCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        float toAlpha,
        float duration,
        Ease ease = Ease.OutCubic,
        bool waitForCompletion = false,
        float fromAlpha = -1f)
    {
        _widgets  = widgets;
        _screenId = screenId;
        _widgetId = widgetId;
        _target     = target;

        _toAlpha   = Mathf.Clamp01(toAlpha);
        _fromAlpha = fromAlpha;                 // sentinel 허용
        _duration  = Mathf.Max(0f, duration);
        _ease      = ease;
        _wait      = waitForCompletion;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        Graphic g = _refs.GetGraphic(_target);
        if (g == null)
            yield break;
        
        g.DOKill(false);

        // 시작 알파: 지정됐으면 강제, 아니면 현재값 유지
        if (_fromAlpha >= 0f)
            SetAlpha(g, Mathf.Clamp01(_fromAlpha));

        // duration 0이면 즉시
        if (_duration <= 0f)
        {
            SetAlpha(g, _toAlpha);
            yield break;
        }

        Tween tween = g
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

        Graphic g = _refs.GetGraphic(_target);
        g.DOKill(false);
        SetAlpha(g, _toAlpha);
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved)
            return _refs != null;
        
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out _refs) || _refs == null)
            return false;

        return true;
    }

    
    private static void SetAlpha(Graphic g, float a01)
    {
        if (g == null) return;
        Color c = g.color;
        c.a = Mathf.Clamp01(a01);
        g.color = c;
    }
}
