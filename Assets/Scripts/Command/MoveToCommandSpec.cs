using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using IEnumerator = System.Collections.IEnumerator;

[Serializable]
[CommandMenuHint("Motion", "Move To", Order = 20)]
public sealed class MoveToCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.StandingPortraitTrack;

    [Header("Destination (absolute anchoredPosition)")]
    public Vector2 position;

    [Header("Tween")]
    [Tooltip("<=0이면 Config 기본 duration 사용.")]
    public float duration = -1f;

    public Ease ease = Ease.OutCubic;

    [Tooltip("true면 스텝이 이 이동이 끝날 때까지 대기.")]
    public bool wait = false;

    [Tooltip("true면 기존 트윈을 끊고 시작.")]
    public bool killTween = true;
}

public sealed class CpsMoveToCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly DialogueWidgetTarget _target;
    private readonly Vector2 _position;

    private readonly float _duration;
    private readonly Ease  _ease;
    private readonly bool  _wait;
    private readonly bool  _killTween;
    
    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;
    private bool _resolved;

    public CpsMoveToCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        Vector2 position,
        float duration,
        Ease ease,
        bool waitForCompletion,
        bool killTween = true)
    {
        _widgets  = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target   = target;
        _position = position;

        _duration = Mathf.Max(0f, duration);
        _ease     = ease;
        _wait     = waitForCompletion;
        _killTween = killTween;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        if (_killTween)
            _rect.DOKill(false);

        if (_duration <= 0f)
        {
            _rect.anchoredPosition = _position;
            yield break;
        }

        Tween tween = _rect
            .DOAnchorPos(_position, _duration)
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

        if (_killTween)
            _rect.DOKill(false);

        _rect.anchoredPosition = _position;
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _rect != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out _refs) || _refs == null)
            return false;

        _rect = _refs.GetRect(_target);
        
        return _rect != null;
    }
}
