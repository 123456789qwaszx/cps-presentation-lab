using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

[Serializable]
[CommandMenuHint(
    "Set Rect",
    "Offset Position (default = ResetToZero)",
    Sets = new[]
    {
        CpsCommandMenuSets.ResetUI,
    },
    SetOrder = -80
    )]
public sealed class SetPositionOffsetCommandSpec : CommandSpecBase
{
    [Header("Target (Track or Rig)")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.Standing00Track;

    [Header("Offset (Relative)")]
    [Tooltip("현재 anchoredPosition 기준으로 더해질 오프셋(픽셀 단위).")]
    public Vector2 offset = Vector2.zero;

    [Header("Reset (Track Origin)")]
    [Tooltip("체크하면 현재 설정된 오프셋을 무시하고 anchoredPosition을 (0,0)으로 리셋합니다.")]
    public bool resetToZero = true;

    [Header("Options")]
    [Tooltip("체크하면 RectTransform 관련 트윈을 끊고 적용합니다.")]
    public bool killTween = true;
}

public sealed class SetPositionOffsetCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string                _screenId;
    private readonly string                _widgetRoleKey;

    private readonly DialogueWidgetTarget  _target;
    private readonly Vector2               _offset;
    private readonly bool                  _resetToZero;
    private readonly bool                  _killTween;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform                     _rect;
    private bool                              _resolveAttempted;

    public override bool WaitForCompletion => false;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public SetPositionOffsetCommand(
        IDialogueWidgetAccess widgets,
        string                screenId,
        string                widgetRoleKey,
        DialogueWidgetTarget  target,
        Vector2               offset,
        bool                  resetToZero,
        bool                  killTween)
    {
        _widgets       = widgets;
        _screenId      = screenId;
        _widgetRoleKey = widgetRoleKey;

        _target      = target;
        _offset      = offset;
        _resetToZero = resetToZero;
        _killTween   = killTween;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        Apply();
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        Apply();
    }

    private void Apply()
    {
        if (_killTween)
            _rect.DOKill(false);

        if (_resetToZero)
        {
            _rect.anchoredPosition = Vector2.zero;
        }
        else
        {
            _rect.anchoredPosition = _rect.anchoredPosition + _offset;
        }
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
        if (_rect == null)
        {
            Debug.LogWarning(
                $"[SetPositionOffsetCommand] RectTransform not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        return true;
    }
}