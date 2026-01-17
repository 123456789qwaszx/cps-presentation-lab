using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;


public enum CpsShakeAxis
{
    X  = 0,
    Y  = 1,
    XY = 2,
}

[Serializable]
[CommandMenuHint(
    "Motion",
    "Shake"
    // ,
    // Sets = new[]
    // {
    //     "Custom/Emote/HitStrong"
    // }
    ,
    SetOrder = 30,
    Order = 20)]
public sealed class ShakeRigsCommandSpec : CommandSpecBase
{
    [Header("Target")]
    [Tooltip("어느 초상화 세트를 흔들지 선택합니다.")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.None;

    [Header("Shake Axis")]
    public CpsShakeAxis axis = CpsShakeAxis.X;

    [Header("Strength")]
    [Tooltip("흔들림 강도(픽셀). 8~24 정도가 UI에서 효과적입니다.")]
    public float intensity = 18f;

    [Header("Tween")]
    [Tooltip("흔들리는 시간(초). <= 0이면 실행하지 않습니다.")]
    public float duration = 0.4f;

    [Tooltip("초당 진동 횟수 느낌. 10~20 정도가 자연스럽습니다.")]
    public int vibrato = 15;

    [Tooltip("각도/방향 랜덤성(0~180). 값이 클수록 방향이 더 흩어집니다.")]
    [Range(0f, 180f)]
    public float randomness = 0f;

    [Tooltip("체크하면 흔들림이 끝날 때까지 Step 진행을 멈춥니다.")]
    public bool wait = false;
}

public sealed class ShakeRigsCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string                _screenId;
    private readonly string                _widgetRoleKey;
    private readonly bool                  _wait;

    private readonly DialogueWidgetTarget   _target;
    private readonly CpsShakeAxis          _axis;
    private readonly float                 _intensity;
    private readonly float                 _duration;
    private readonly int                   _vibrato;
    private readonly float                 _randomness;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform                    _rect;
    private Vector2                          _originPos;
    private bool                             _resolveAttempted;

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public ShakeRigsCommand(
        IDialogueWidgetAccess widgets,
        string                screenId,
        string                widgetRoleKey,
        bool                  wait,
        
        DialogueWidgetTarget   target,
        CpsShakeAxis          axis,
        float                 intensity,
        float                 duration,
        int                   vibrato,
        float                 randomness)
    {
        _widgets       = widgets;
        _screenId      = screenId;
        _widgetRoleKey = widgetRoleKey;
        _wait          = wait;

        _target     = target;
        _axis       = axis;
        _intensity  = Mathf.Max(0f, intensity);
        _duration   = Mathf.Max(0f, duration);
        _vibrato    = vibrato > 0 ? vibrato : 12;
        _randomness = randomness >= 0f ? Mathf.Clamp(randomness, 0f, 180f) : 90f;
    }

    #region Execute & Skip

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        if (_intensity <= 0f || _duration <= 0f)
            yield break;

        // 기존 트윈 정리
        _rect.DOKill(false);

        // 아날로그: 현재 위치를 origin으로 재캡처 (조합에서 더 직관적)
        _originPos = _rect.anchoredPosition;

        Vector2 strength = GetStrength(_axis, _intensity);

        Tween tween = _rect
            .DOShakeAnchorPos(_duration, strength, _vibrato, _randomness, snapping: false, fadeOut: true)
            .SetUpdate(true);
        
        tween.OnComplete(() =>
        {
            _rect.anchoredPosition = _originPos;
        })
        .BindToRun(scope);

        if (_wait)
            yield return tween.WaitForCompletion();
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        _rect.DOKill(false);
        _rect.anchoredPosition = _originPos;
    }

    #endregion

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

// #if UNITY_EDITOR
//         if (!Enum.IsDefined(typeof(PortraitShakeTarget), _target))
//         {
//             Debug.LogWarning(
//                 $"[ShakeWidgetCommand] Invalid {nameof(PortraitShakeTarget)} value in command data: {_target}. " +
//                 $"screenId='{_screenId}', widgetRoleKey='{_widgetRoleKey}'");
//             return false;
//         }
// #endif
//
//         // PortraitShakeTarget → 실제 위젯 슬롯 매핑 (Shake용 레이어)
//         DialogueWidgetTarget widgetTarget = _target switch
//         {
//             PortraitShakeTarget.Main     => DialogueWidgetTarget.MainStandingPortraitShake,
//             PortraitShakeTarget.SubLeft  => DialogueWidgetTarget.SubLeftStandingPortraitShake,
//             PortraitShakeTarget.SubRight => DialogueWidgetTarget.SubRightStandingPortraitShake,
//             _                            => DialogueWidgetTarget.None
//         };
//
//         if (widgetTarget == DialogueWidgetTarget.None)
//             return false;

        _rect = _refs.GetRect(_target);
        if (_rect == null)
        {
            Debug.LogWarning(
                $"[ShakeWidgetCommand] RectTransform not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        _originPos = _rect.anchoredPosition;
        return true;
    }

    private static Vector2 GetStrength(CpsShakeAxis axis, float intensity)
    {
        return axis switch
        {
            CpsShakeAxis.Y  => new Vector2(0f, intensity),
            CpsShakeAxis.XY => new Vector2(intensity, intensity),
            _               => new Vector2(intensity, 0f), // X 기본
        };
    }
}