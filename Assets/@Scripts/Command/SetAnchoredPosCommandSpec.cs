using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

[Serializable]
[CommandMenuHint(
    "Set Rect",
    "Set Anchored Pos",
    Order = 30)]
public sealed class SetAnchoredPosCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.MainStandingPortraitTrack;

    [Header("Mode")]
    [Tooltip("체크하면 9방향 앵커 프리셋을 적용한 뒤, 오프셋을 설정합니다. 체크하지 않으면 현재 위치에서 오프셋만 더합니다.")]
    public bool useAnchorPreset = false;

    [Tooltip("useAnchorPreset이 true일 때 사용할 9방향 앵커 프리셋입니다.")]
    public RectAnchorPreset9 anchorPreset = RectAnchorPreset9.Center;

    [Header("Position / Offset")]
    [Tooltip("useAnchorPreset=true: 프리셋 기준 오프셋.\nuseAnchorPreset=false: 현재 위치에서 더해지는 상대 오프셋.")]
    public Vector2 offset = Vector2.zero;

    [Header("Options")]
    [Tooltip("체크하면 RectTransform 관련 트윈을 끊고 적용합니다.")]
    public bool killTween = true;
}

public sealed class SetAnchoredPosCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string                _screenId;
    private readonly string                _widgetRoleKey;

    private readonly DialogueWidgetTarget  _target;
    private readonly bool                  _useAnchorPreset;
    private readonly RectAnchorPreset9     _anchorPreset;
    private readonly Vector2               _offset;
    private readonly bool                  _killTween;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform                     _rect;
    private bool                              _resolveAttempted;

    public override bool WaitForCompletion => false;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public SetAnchoredPosCommand(
        IDialogueWidgetAccess widgets,
        string                screenId,
        string                widgetRoleKey,

        DialogueWidgetTarget  target,
        bool                  useAnchorPreset,
        RectAnchorPreset9     anchorPreset,
        Vector2               offset,
        bool                  killTween)
    {
        _widgets         = widgets;
        _screenId        = screenId;
        _widgetRoleKey   = widgetRoleKey;

        _target          = target;
        _useAnchorPreset = useAnchorPreset;
        _anchorPreset    = anchorPreset;
        _offset          = offset;
        _killTween       = killTween;
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

        if (_useAnchorPreset)
        {
            // 1) 9방향 프리셋으로 anchor/pivot 세팅
            _anchorPreset.ApplyTo(_rect);

            // 2) 그 기준점에서 offset 만큼 이동 (절대 포즈 느낌)
            _rect.anchoredPosition = _offset;
        }
        else
        {
            // 기존 방식: 현재 anchoredPosition에서 offset만큼 팬/트랙
            Vector2 dest = _rect.anchoredPosition + _offset;
            _rect.anchoredPosition = dest;
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
                $"[SetAnchoredPosCommand] RectTransform not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        return true;
    }
}
