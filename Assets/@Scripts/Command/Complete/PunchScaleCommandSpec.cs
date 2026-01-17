using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

[Serializable]
[CommandMenuHint(
    "Motion",
    "Punch Scale",
    // Sets = new[]
    // {
    //     "Custom/Portrait/ChangeEmotion",
    //     "Custom/Emote/HitStrong"
    // },
    SetOrder = 20,
    Order = 10)]
public sealed class PunchScaleCommandSpec : CommandSpecBase
{
    [Header("Target")] public DialogueWidgetTarget target = DialogueWidgetTarget.Standing00Track;

    [Header("Punch")] [Tooltip("펀치 강도. 0.15 ~ 0.35 정도가 UI에서 예쁘게 보입니다.")]
    public float strength = 0.25f;

    [Header("Tween")] [Tooltip("펀치에 걸리는 시간(초). <= 0이면 실행하지 않습니다.")]
    public float duration = 0.22f;

    [Tooltip("진동 횟수 느낌. 6~10 정도가 자연스럽습니다.")]
    public int vibrato = 8;

    [Tooltip("탄성(0~1). 값이 클수록 더 튕기는 느낌입니다.")] [Range(0f, 1f)]
    public float elasticity = 0.75f;

    [Tooltip("체크하면 펀치가 끝날 때까지 Step 진행을 멈춥니다.")]
    public bool wait = false;
}

public sealed class PunchScaleCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;
    private readonly bool _wait;

    private readonly DialogueWidgetTarget _target;

    private readonly float _strength;
    private readonly float _duration;
    private readonly int _vibrato;
    private readonly float _elasticity;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;
    private Vector3 _originScale;
    private bool _resolveAttempted;

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public PunchScaleCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        bool waitForCompletion,
        DialogueWidgetTarget target,
        float strength,
        float duration,
        int vibrato,
        float elasticity)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetRoleKey = widgetRoleKey;
        _wait = waitForCompletion;

        _target = target;
        _strength = Mathf.Max(0f, strength);
        _duration = Mathf.Max(0f, duration);
        _vibrato = vibrato;
        _elasticity = Mathf.Clamp01(elasticity);
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        if (_strength <= 0f || _duration <= 0f)
            yield break;

        _rect.DOKill(false);

        _originScale = _rect.localScale;

        Tween tween = _rect
            .DOPunchScale(new Vector3(_strength, _strength, 0f), _duration, _vibrato, _elasticity)
            .SetUpdate(true);

        tween.OnComplete(() => { _rect.localScale = _originScale; })
            .BindToRun(scope);

        if (_wait)
            yield return tween.WaitForCompletion();
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        _rect.DOKill(false);
        _rect.localScale = _originScale;
    }

    private bool ResolveIfNeeded()
    {
        if (_rect != null)
            return true;

        if (_resolveAttempted)
            return false;

        _resolveAttempted = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null)
            return false;

        _rect = _refs.GetRect(_target);
        if (_rect == null)
        {
            Debug.LogWarning(
                $"[PunchScaleCommand] RectTransform not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        _originScale = _rect.localScale;
        return true;
    }
}