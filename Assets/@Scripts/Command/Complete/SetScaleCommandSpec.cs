using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

[Serializable]
[CommandMenuHint("Set Rect", "Set Scale (XY)", Order = 60)]
public sealed class SetScaleCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.None;

    [Header("Scale")]
    public Vector2 toScale = Vector2.one;

    [Header("Options")]
    public bool killTween = true;
}

public sealed class SetScaleCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;

    private readonly DialogueWidgetTarget _target;
    private readonly Vector2 _toScaleXY;
    private readonly bool _killTween;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;
    private bool _resolveAttempted;

    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public SetScaleCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        DialogueWidgetTarget target,
        Vector2 toScale,
        bool killTween)
    {
        _widgets       = widgets;
        _screenId      = screenId;
        _widgetRoleKey = widgetRoleKey;

        _target     = target;
        _toScaleXY  = toScale;
        _killTween  = killTween;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        if (_killTween)
            _rect.DOKill(false);

        ApplyScaleXY(_rect, _toScaleXY);
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        if (_killTween)
            _rect.DOKill(false);

        ApplyScaleXY(_rect, _toScaleXY);
    }

    private bool ResolveIfNeeded()
    {
        if (_resolveAttempted)
            return _rect != null;

        _resolveAttempted = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null)
            return false;

        _rect = _refs.GetRect(_target);

        return true;
    }

    private static void ApplyScaleXY(RectTransform rect, Vector2 targetXY)
    {
        Vector3 scale = rect.localScale;
        scale.x = targetXY.x;
        scale.y = targetXY.y;
        rect.localScale = scale;
    }
}