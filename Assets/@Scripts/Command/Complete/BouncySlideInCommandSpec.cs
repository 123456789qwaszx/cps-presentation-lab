using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using RectTransform = UnityEngine.RectTransform;

[Serializable]
[CommandMenuHint(
    "Motion",
    "Bouncy Slide In",
    Sets = new[]
    {
        CpsCommandMenuSets.VnMainEnterFirstLine,
    },
    SetOrder = 40,
    Order = 40)]
public sealed class BouncySlideInCommandSpec : CommandSpecBase
{
    [Header("Target")] public DialogueWidgetTarget target = DialogueWidgetTarget.Standing00Track;

    [Header("Slide")] public CpsSlideFrom from = CpsSlideFrom.Left;

    [Tooltip("슬라이드 시작 오프셋 거리(픽셀). 0 이하이면 슬라이드 없이 웨이브만 사용합니다.")]
    public float slideDistance = 480f;

    [Tooltip("슬라이드에 걸리는 시간(초). 0 이하이면 슬라이드 없이 웨이브만 사용합니다.")]
    public float slideDuration = 1.2f;

    public Ease slideEase = Ease.OutCubic;

    [Header("Wave (cute bounce along the path)")] [Tooltip("경로를 따라 통통 튀는 진폭(픽셀). 0이면 웨이브 없음.")]
    public float waveAmplitude = 12f;

    [Tooltip("웨이브 반복 횟수(피크 기준). 1~5 추천.")] public int waveLoops = 4;

    [Tooltip("웨이브 방향 축. 보통 Y 또는 XY가 자연스럽습니다.")]
    public CpsShakeAxis waveAxis = CpsShakeAxis.Y;

    [Header("Behavior")] [Tooltip("체크하면 연출이 끝날 때까지 Step 진행을 멈춥니다.")]
    public bool wait = false;
}

public sealed class BouncySlideInCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;
    private readonly bool _wait;

    private readonly DialogueWidgetTarget _target;
    private readonly CpsSlideFrom _from;

    private readonly float _slideDistance;
    private readonly float _slideDuration;
    private readonly Ease _slideEase;

    private readonly float _waveAmplitude;
    private readonly int _waveLoops;
    private readonly CpsShakeAxis _waveAxis;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;
    private Vector2 _destPos;
    private bool _resolveAttempted;

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public BouncySlideInCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        bool waitForCompletion,
        DialogueWidgetTarget target,
        CpsSlideFrom from,
        float slideDistance,
        float slideDuration,
        Ease slideEase,
        float waveAmplitude,
        int waveLoops,
        CpsShakeAxis waveAxis)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetRoleKey = widgetRoleKey;
        _wait = waitForCompletion;

        _target = target;
        _from = from;

        _slideDistance = Mathf.Max(0f, slideDistance);
        _slideDuration = Mathf.Max(0f, slideDuration);
        _slideEase = slideEase;

        _waveAmplitude = Mathf.Max(0f, waveAmplitude);
        _waveLoops = Mathf.Max(0, waveLoops);
        _waveAxis = waveAxis;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        _rect.DOKill(false);

        Vector2 dest = _destPos;

        Vector2 fromPos = (_slideDistance != 0f)
            ? dest + GetOffset(_from, _slideDistance)
            : dest;

        float duration = _slideDuration;
        bool hasWave   = (_waveAmplitude != 0f && _waveLoops != 0);

        if (duration <= 0f)
        {
            _rect.anchoredPosition = dest;
            yield break;
        }

        _rect.anchoredPosition = fromPos;

        Tween tween = DOTween
            .To(
                () => 0f,
                t =>
                {
                    float eased = DOVirtual.EasedValue(0f, 1f, t, _slideEase);

                    Vector2 basePos = Vector2.Lerp(fromPos, dest, eased);

                    Vector2 offset = Vector2.zero;
                    if (hasWave)
                    {
                        float sin = Mathf.Sin(eased * Mathf.PI * _waveLoops);
                        float decay = 1f - eased;
                        float amp = _waveAmplitude * sin * decay;

                        Vector2 slideDir = (dest - fromPos).sqrMagnitude > 0f
                            ? (dest - fromPos).normalized
                            : GetSlideDir(_from);

                        Vector2 perpDir = new Vector2(-slideDir.y, slideDir.x);

                        Vector2 waveDir = _waveAxis switch
                        {
                            CpsShakeAxis.X => slideDir,
                            CpsShakeAxis.XY => (slideDir + perpDir).normalized,
                            CpsShakeAxis.Y => perpDir,
                            _ => perpDir,
                        };

                        offset = waveDir * amp;
                    }

                    _rect.anchoredPosition = basePos + offset;
                },
                1f,
                duration
            )
            .SetUpdate(true);

        tween.OnComplete(() => { _rect.anchoredPosition = dest; })
            .BindToRun(scope);

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
                $"[BouncySlideInCommand] RectTransform not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        _destPos = _rect.anchoredPosition;
        return true;
    }

    private static Vector2 GetOffset(CpsSlideFrom from, float distance)
    {
        return from switch
        {
            CpsSlideFrom.Right => new Vector2(+distance, 0f),
            CpsSlideFrom.Up => new Vector2(0f, +distance),
            CpsSlideFrom.Down => new Vector2(0f, -distance),
            _ => new Vector2(-distance, 0f),
        };
    }

    private static Vector2 GetSlideDir(CpsSlideFrom from)
    {
        return from switch
        {
            CpsSlideFrom.Right => new Vector2(+1f, 0f),
            CpsSlideFrom.Up => new Vector2(0f, +1f),
            CpsSlideFrom.Down => new Vector2(0f, -1f),
            _ => new Vector2(-1f, 0f),
        };
    }
}