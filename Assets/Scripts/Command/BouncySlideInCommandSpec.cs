using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using RectTransform = UnityEngine.RectTransform;

[Serializable]
[CommandMenuHint("Motion", "Bouncy Slide In", Order = 15)]
public sealed class BouncySlideInCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.StandingPortraitImage;

    [Header("Slide")]
    public CpsSlideFrom from = CpsSlideFrom.Left;

    /// <summary> 시작 오프셋 거리 (픽셀). <= 0이면 Config 기본값 사용 </summary>
    public float slideDistance = -1f;

    /// <summary> 슬라이드에 걸리는 시간. <= 0이면 Config 기본값 사용 </summary>
    public float slideDuration = -1f;

    public Ease slideEase = Ease.OutCubic;

    [Header("Wave (cute bounce along the path)")]
    public float waveAmplitude = 12f;
    public int   waveLoops     = 2;
    public CpsShakeAxis waveAxis = CpsShakeAxis.Y;

    [Header("Behavior")]
    /// <summary>
    /// true면 레이아웃 자리에서 시작해서
    /// 살짝 튕기며 들어오는 느낌(= 공중에서 시작 안 함).
    /// false면 멀리 떨어진 곳에서 슬라이드 인.
    /// </summary>
    public bool startFromLayout = true;

    public bool wait = false;
}

public sealed class CpsBouncySlideInCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly DialogueWidgetTarget _target;
    private readonly CpsSlideFrom _from;

    private readonly float _slideDistance;
    private readonly float _slideDuration;
    private readonly Ease  _slideEase;

    private readonly float       _waveAmplitude;
    private readonly int         _waveLoops;
    private readonly CpsShakeAxis _waveAxis;

    private readonly bool _wait;
    private readonly bool _startFromLayout;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;
    private Vector2 _destPos;
    private bool _resolved;

    public CpsBouncySlideInCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        CpsSlideFrom from,
        float slideDistance,
        float slideDuration,
        Ease slideEase,
        float waveAmplitude,
        int waveLoops,
        CpsShakeAxis waveAxis,
        bool startFromLayout,
        bool waitForCompletion)
    {
        _widgets  = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target       = target;
        _from         = from;

        _slideDistance = Mathf.Max(0f, slideDistance);
        _slideDuration = Mathf.Max(0f, slideDuration);
        _slideEase     = slideEase;

        _waveAmplitude = Mathf.Max(0f, waveAmplitude);
        _waveLoops     = Mathf.Max(0,  waveLoops);
        _waveAxis      = waveAxis;

        _startFromLayout = startFromLayout;
        _wait            = waitForCompletion;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        _rect.DOKill(false);

        // dest = 원래 레이아웃 위치
        Vector2 dest = _destPos;

        // "가상" 시작점 (경로 계산용)
        Vector2 virtualStart = dest + GetOffset(_from, _slideDistance);

        bool hasSlide = _slideDuration > 0f && _slideDistance > 0f;
        bool hasWave  = _waveAmplitude > 0f && _waveLoops > 0;

        if (!hasSlide && !hasWave)
        {
            _rect.anchoredPosition = dest;
            yield break;
        }

        // 실제 첫 프레임 위치 설정
        if (_startFromLayout)
        {
            // 레이아웃 자리에서 시작 (공중 느낌 X)
            _rect.anchoredPosition = dest;
        }
        else
        {
            // 진짜 SlideIn: 멀리서 들어옴
            _rect.anchoredPosition = virtualStart;
        }

        float duration = hasSlide ? _slideDuration : 0.4f;

        Tween tween = DOTween
            .To(
                () => 0f,
                t =>
                {
                    float eased = DOVirtual.EasedValue(0f, 1f, t, _slideEase);

                    // 기본 슬라이드 위치
                    Vector2 basePos;
                    if (_startFromLayout)
                    {
                        // 레이아웃에서 살짝 뒤로 밀렸다가 제 자리로 오는 느낌
                        // t=0 → dest, t>0 → virtualStart 쪽으로 살짝 갔다가 다시 dest
                        // (원하면 이 부분을 더 커스텀해도 됨)
                        basePos = Vector2.Lerp(virtualStart, dest, eased);
                    }
                    else
                    {
                        // 멀리서 dest로 들어오는 전통 Slide
                        basePos = Vector2.Lerp(virtualStart, dest, eased);
                    }

                    // Wave 계산
                    Vector2 offset = Vector2.zero;
                    if (hasWave)
                    {
                        float sin   = Mathf.Sin(eased * Mathf.PI * _waveLoops);
                        float decay = 1f - eased;
                        float amp   = _waveAmplitude * sin * decay;

                        Vector2 slideDir = (dest - virtualStart).sqrMagnitude > 0f
                            ? (dest - virtualStart).normalized
                            : GetSlideDir(_from);

                        Vector2 perpDir = new Vector2(-slideDir.y, slideDir.x);

                        Vector2 waveDir;
                        switch (_waveAxis)
                        {
                            case CpsShakeAxis.X:
                                waveDir = slideDir;
                                break;
                            case CpsShakeAxis.XY:
                                waveDir = (slideDir + perpDir).normalized;
                                break;
                            case CpsShakeAxis.Y:
                            default:
                                waveDir = perpDir;
                                break;
                        }

                        offset = waveDir * amp;
                    }

                    _rect.anchoredPosition = basePos + offset;
                },
                1f,
                duration
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
        _rect.anchoredPosition = _destPos;
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

        // dest = 현재 레이아웃 위치
        _destPos = _rect.anchoredPosition;
        return true;
    }

    private static Vector2 GetOffset(CpsSlideFrom from, float distance)
    {
        switch (from)
        {
            case CpsSlideFrom.Right: return new Vector2(+distance, 0f);
            case CpsSlideFrom.Up:    return new Vector2(0f, +distance);
            case CpsSlideFrom.Down:  return new Vector2(0f, -distance);
            case CpsSlideFrom.Left:
            default:                 return new Vector2(-distance, 0f);
        }
    }

    private static Vector2 GetSlideDir(CpsSlideFrom from)
    {
        switch (from)
        {
            case CpsSlideFrom.Right: return new Vector2(+1f, 0f);
            case CpsSlideFrom.Up:    return new Vector2(0f, +1f);
            case CpsSlideFrom.Down:  return new Vector2(0f, -1f);
            case CpsSlideFrom.Left:
            default:                 return new Vector2(-1f, 0f);
        }
    }
}
