using System;
using DG.Tweening;
using UnityEngine;
using System.Collections;

[Serializable]
[CommandMenuHint("Visual", "Canvas Fade", Order = 20)]
public sealed class CanvasFadeCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.StandingPortraitRoot;
    // Rect 쪽 타겟을 기준으로 CanvasGroup을 찾는다는 느낌

    [Header("Fade")]
    [Range(0f, 1f)]
    public float toAlpha = 1f;

    /// <summary>
    /// fromAlpha < 0 이면 "현재 alpha에서 시작"
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

    [Header("Options")]
    /// <summary>
    /// true면 대상에 CanvasGroup이 없을 경우 자동으로 붙여서 사용.
    /// false면 없으면 조용히 아무 것도 안 함.
    /// </summary>
    public bool addIfMissing = true;
}


public sealed class CpsCanvasFadeCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;
    private readonly DialogueWidgetTarget _target;

    private readonly float _toAlpha;
    private readonly float _fromAlpha;
    private readonly float _duration;
    private readonly Ease  _ease;
    private readonly bool  _wait;
    private readonly bool  _addIfMissing;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private bool _resolved;

    public CpsCanvasFadeCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        DialogueWidgetTarget target,
        float toAlpha,
        float duration,
        Ease ease = Ease.OutCubic,
        bool waitForCompletion = false,
        float fromAlpha = -1f,
        bool addIfMissing = true)
    {
        _widgets       = widgets;
        _screenId      = screenId;
        _widgetRoleKey = widgetRoleKey;
        _target        = target;

        _toAlpha      = Mathf.Clamp01(toAlpha);
        _fromAlpha    = fromAlpha;
        _duration     = Mathf.Max(0f, duration);
        _ease         = ease;
        _wait         = waitForCompletion;
        _addIfMissing = addIfMissing;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        CanvasGroup group = ResolveCanvasGroup();
        if (group == null)
            yield break;

        group.DOKill(false);

        // 시작 알파: 지정됐으면 강제, 아니면 현재값 유지
        if (_fromAlpha >= 0f)
            group.alpha = Mathf.Clamp01(_fromAlpha);

        // duration 0이면 즉시 세팅
        if (_duration <= 0f)
        {
            group.alpha = _toAlpha;
            yield break;
        }

        Tween tween = group
            .DOFade(_toAlpha, _duration)
            .SetEase(_ease)
            .SetUpdate(true); // timeScale 무시 (Graphic fade와 동일 정책)

        tween.BindToStep(scope);

        if (_wait)
            yield return tween.WaitForCompletion();
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        CanvasGroup group = ResolveCanvasGroup();
        if (group == null)
            return;

        group.DOKill(false);
        group.alpha = _toAlpha;
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved)
            return _refs != null;

        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null)
            return false;

        return true;
    }

    private CanvasGroup ResolveCanvasGroup()
    {
        if (_refs == null)
            return null;

        // 너가 이미 쓰고 있는 패턴 가정: WidgetRefsExtensions.GetRectTransform(...)
        RectTransform rt = _refs.GetRect(_target);
        if (rt == null)
            return null;

        CanvasGroup group = rt.GetComponent<CanvasGroup>();
        if (group == null && _addIfMissing)
            group = rt.gameObject.AddComponent<CanvasGroup>();

        return group;
    }
}
