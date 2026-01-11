using System;
using DG.Tweening;
using UnityEngine;
using System.Collections;

[Serializable]
[CommandMenuHint(
    "Motion/Emote",
    "Sway Then Drop",
    Sets = new[]
    {
        "Custom/Emote/PopEmoji"
    },
    SetOrder = 30,
    Order = 30)]
public sealed class SwayThenDropCommandSpec : CommandSpecBase
{
    [Header("Target")] public DialogueWidgetTarget target = DialogueWidgetTarget.StandingPortraitImage;

    [Header("Sway (Fan-like)")]
    /// <summary>
    /// 아랫변 중앙을 축으로 좌우로 흔들리는 각도 (절대값 기준). 10~25 추천.
    /// </summary>
    public float swayAngle = 18f;

    /// <summary> 몇 번 왔다갔다 할지 (피크 기준). </summary>
    public int swayLoops = 2;

    /// <summary> 전체 스윙에 걸리는 대략적인 시간. </summary>
    public float swayDuration = 0.35f;

    [Header("Drop")]
    /// <summary> 아래로 내려갈 거리 (픽셀). </summary>
    public float dropDistance = 140f;

    /// <summary> 떨어지는 데 걸리는 시간. </summary>
    public float dropDuration = 0.35f;

    /// <summary> 떨어지면서 추가로 회전할 각도 (시계 방향 기준). </summary>
    public float dropAngle = 90f;

    public Ease dropEase = Ease.InCubic;

    [Header("Behavior")] public bool wait = true;

    [Header("Sway Easing")]
    /// <summary>
    /// 오른쪽(or 첫 방향)으로 갈 때 이징. (기본: 빠르게 휙, 천천히 멈추는 느낌)
    /// </summary>
    public Ease swayForwardEase = Ease.OutQuad;

    /// <summary>
    /// 반대쪽으로 돌아올 때 이징. (기본: 살짝 끌려오는 느낌)
    /// </summary>
    public Ease swayBackwardEase = Ease.InSine;

    /// <summary>
    /// true면 시간이 지날수록 진폭이 줄어들어서 사람 손 같은 감각이 남.
    ///summary>
    public bool swayDecay = true;

    [Header("Blend")] [Range(0f, 1f)]
    /// <summary>
    /// 0이면 sway 시작과 동시에 Drop 시작,
    /// 1이면 sway가 끝난 뒤 Drop 시작,
    /// 그 사이 값이면 swayDuration의 중간 어딘가에서 Drop이 슬슬 시작.
    /// </summary>
    public float dropStartRatio = 0.3f;
}


public sealed class CpsSwayThenDropCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly DialogueWidgetTarget _target;

    private readonly float _swayAngle;
    private readonly int _swayLoops;
    private readonly float _swayDuration;

    private readonly float _dropDistance;
    private readonly float _dropDuration;
    private readonly float _dropAngle;
    private readonly Ease _dropEase;

    private readonly bool _wait;

    private readonly Ease _swayForwardEase;
    private readonly Ease _swayBackwardEase;
    private readonly bool _swayDecay;

    private readonly float _dropStartRatio;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;

    private Vector2 _originPos;
    private float _originRotZ;
    private Vector2 _finalPos;
    private float _finalRotZ;

    private Vector2 _originalPivot;
    //private bool _pivotAdjusted;

    private bool _resolved;

    public CpsSwayThenDropCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        float swayAngle,
        int swayLoops,
        float swayDuration,
        float dropDistance,
        float dropDuration,
        float dropAngle,
        Ease dropEase,
        Ease swayForwardEase,
        Ease swayBackwardEase,
        bool swayDecay,
        float dropStartRatio,
        bool waitForCompletion)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target = target;
        _swayAngle = Mathf.Abs(swayAngle);
        _swayLoops = Mathf.Max(0, swayLoops);
        _swayDuration = Mathf.Max(0f, swayDuration);

        _dropDistance = dropDistance;
        _dropDuration = Mathf.Max(0f, dropDuration);
        _dropAngle = dropAngle;
        _dropEase = dropEase;

        _swayForwardEase = swayForwardEase;
        _swayBackwardEase = swayBackwardEase;
        _swayDecay = swayDecay;

        _dropStartRatio = Mathf.Clamp01(dropStartRatio);

        _wait = waitForCompletion;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        _rect.DOKill(false);

        float swayDur = _swayDuration;
        float dropDur = _dropDuration;
        float dropDelay = swayDur * _dropStartRatio; // sway 도중 언제부터 떨어질지

        // 전체 길이: sway가 더 길 수도, drop 구간이 더 길 수도 있으니 둘 다 고려
        float totalDuration = 0f;
        if (swayDur > 0f || dropDur > 0f)
        {
            totalDuration = Mathf.Max(
                swayDur,
                dropDelay + dropDur
            );
        }

        if (totalDuration <= 0f)
        {
            // duration 정보가 없다면 그냥 즉시 최종 포즈로
            _rect.anchoredPosition = _finalPos;
            SetLocalEulerZ(_rect, _finalRotZ);
            yield break;
        }

        Tween tween = DOTween.To(
                () => 0f,
                tNorm =>
                {
                    // tNorm: 0~1
                    float time = tNorm * totalDuration;

                    // ----- 1) Sway 회전 성분 -----
                    float swayAngle = 0f;
                    if (swayDur > 0f && _swayAngle > 0f)
                    {
                        float ts = Mathf.Clamp01(time / swayDur);

                        // loops 번 왕복하는 sin 파형
                        float phase = ts * Mathf.PI * 2f * _swayLoops;
                        float raw = Mathf.Sin(phase); // -1 ~ +1

                        float envelope = 1f;
                        if (_swayDecay)
                        {
                            float eased = DOVirtual.EasedValue(0f, 1f, ts, _swayForwardEase);
                            envelope = 1f - eased; // 앞에 크고 뒤에 작아짐
                        }

                        swayAngle = raw * _swayAngle * envelope;
                    }

                    // ----- 2) Drop 위치/회전 성분 -----
                    Vector2 pos = _originPos;
                    float dropRotOffset = 0f;

                    if (dropDur > 0f)
                    {
                        if (time <= dropDelay)
                        {
                            // 아직 Drop 시작 전
                            pos = _originPos;
                            dropRotOffset = 0f;
                        }
                        else
                        {
                            float td = Mathf.Clamp01((time - dropDelay) / dropDur);
                            float easedDrop = DOVirtual.EasedValue(0f, 1f, td, _dropEase);

                            pos = Vector2.Lerp(_originPos, _finalPos, easedDrop);
                            dropRotOffset = _dropAngle * easedDrop;
                        }
                    }
                    else
                    {
                        // dropDuration == 0인 경우 바로 최종 위치/각도
                        pos = _finalPos;
                        dropRotOffset = _dropAngle;
                    }

                    // ----- 3) 최종 포즈 적용 -----
                    _rect.anchoredPosition = pos;
                    SetLocalEulerZ(_rect, _originRotZ + swayAngle + dropRotOffset);
                },
                1f,
                totalDuration
            )
            .SetUpdate(true);

        tween.BindToStep(scope);

        if (_wait)
            yield return tween.WaitForCompletion();
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        _rect.DOKill(false);
        _rect.anchoredPosition = _finalPos;
        SetLocalEulerZ(_rect, _finalRotZ);
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
        if (_rect == null)
            return false;

        // 1) pivot을 bottom-center로 보정 (아랫변 중앙을 축으로 회전)
        _originalPivot = _rect.pivot;
        Vector2 bottomCenter = new Vector2(0.5f, 0f);

        if (_originalPivot != bottomCenter)
        {
            // anchoredPosition 유지 보정
            Vector2 delta = Vector2.Scale(_rect.sizeDelta, _originalPivot - bottomCenter);
            _rect.pivot = bottomCenter;
            _rect.anchoredPosition += delta;
            //_pivotAdjusted = true;
        }

        // 2) 원래 상태 기준 정보 캐시
        _originPos = _rect.anchoredPosition;
        _originRotZ = _rect.localEulerAngles.z;

        // 3) 최종 상태(떨어진 위치/각도) 계산
        _finalPos = _originPos + new Vector2(0f, -_dropDistance);
        _finalRotZ = _originRotZ + _dropAngle;

        return true;
    }

    private static void SetLocalEulerZ(RectTransform rect, float z)
    {
        Vector3 e = rect.localEulerAngles;
        e.z = z;
        rect.localEulerAngles = e;
    }
}