using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

// SetX계열 = State/Setup 계열
// Move/Slide/Shake 계열 = Motion/Emotion 계열
// 따라서
// 애니메이션은 옵션 일 뿐, 기본은 스냅포즈.

// 그래서
// 기본 값은 아래처럼 사용.
// 기본 의미 : 포즈 / 상태 세팅
// duration = 0f (기본: 스냅)
// wait = false (기본: Step 진행 막지 않음)
// KillTween = true (기존 회전 트윈 정리 + 상태 최우선으로 맞춘다.)

// 혹은 그냥 Tween을 다 떼어낸다?
// "순수 포즈 세팅"용. 트윈 없음으로.

[Serializable]
[CommandMenuHint(
    "Set Rect",
    "Set Rotation (Z)"
)]
public sealed class SetRotationCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.None;

    [Header("Rotation (Z axis)")]
    /// <summary>
    /// 최종 Z각도 (localEulerAngles.z 기준).
    /// </summary>
    public float toAngle = 0f;

    [Header("Options")]
    /// <summary>
    /// true면 기존 회전 관련 트윈을 끊고, 이 포즈를 최우선으로 맞춘다.
    /// </summary>
    public bool killTween = true;
}

public sealed class SetRotationCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string                _screenId;
    private readonly string                _widgetRoleKey;

    private readonly DialogueWidgetTarget  _target;
    private readonly float                 _toAngle;
    private readonly bool                  _killTween;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform                    _rect;

    private bool _resolveAttempted;

    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public SetRotationCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        DialogueWidgetTarget target,
        float toAngle,
        bool killTween)
    {
        _widgets       = widgets;
        _screenId      = screenId;
        _widgetRoleKey = widgetRoleKey;

        _target    = target;
        _toAngle   = toAngle;
        _killTween = killTween;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        if (_killTween)
            _rect.DOKill(false);

        SetLocalEulerZ(_rect, _toAngle);
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        if (_killTween)
            _rect.DOKill(false);

        SetLocalEulerZ(_rect, _toAngle);
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

        Component comp = _refs.GetComponent(_target);
        if (comp == null)
        {
            Debug.LogWarning(
                $"[SetRotationCommand] Widget target not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        _rect = comp as RectTransform ?? comp.GetComponent<RectTransform>();
        if (_rect == null)
        {
            Debug.LogWarning(
                $"[SetRotationCommand] Target has no RectTransform. screen='{_screenId}', roleKey='{_widgetRoleKey}', comp={comp.GetType().Name}",
                comp);
            return false;
        }

        return true;
    }

    private static void SetLocalEulerZ(RectTransform rect, float z)
    {
        Vector3 e = rect.localEulerAngles;
        e.z = z;
        rect.localEulerAngles = e;
    }
}