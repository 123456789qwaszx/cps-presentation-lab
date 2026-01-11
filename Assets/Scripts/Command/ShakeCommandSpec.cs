using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public enum CpsShakeAxis
{
    X = 0,
    Y,
    XY,
}

[Serializable]
[CommandMenuHint(
    "Motion/Emote",
    "Shake",
    Sets = new[]
    {
        "Custom/Emote/HitStrong"
    },
    SetOrder = 30,
    Order = 20)]
public sealed class ShakeWidgetCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.StandingPortraitImage;

    [Header("Shake")]
    public CpsShakeAxis axis = CpsShakeAxis.X;

    /// <summary>
    /// 흔들림 강도(픽셀). 8~24 정도가 UI에서 효과적임.
    /// </summary>
    public float intensity = 14f;

    /// <summary>
    /// <= 0이면 고급 디폴트 사용
    /// </summary>
    public float duration = -1f;

    /// <summary>
    /// <= 0이면 고급 디폴트 사용
    /// </summary>
    public int vibrato = 0;

    /// <summary>
    /// < 0이면 고급 디폴트 사용 (0~180)
    /// </summary>
    public float randomness = -1f;

    public bool wait = false;
}


public sealed class CpsShakeWidgetCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly DialogueWidgetTarget _target;
    private readonly CpsShakeAxis _axis;

    private readonly float _intensity;
    private readonly float _duration;
    private readonly int   _vibrato;
    private readonly float _randomness;
    private readonly bool  _wait;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;
    private Vector2 _originPos;
    private bool _resolved;

    public CpsShakeWidgetCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        CpsShakeAxis axis,
        float intensity,
        float duration,
        int vibrato,
        float randomness,
        bool waitForCompletion = false)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target = target;
        _axis = axis;

        _intensity = Mathf.Max(0f, intensity);
        _duration = Mathf.Max(0f, duration);
        _vibrato = Mathf.Max(0, vibrato);
        _randomness = randomness; // sentinel 허용
        _wait = waitForCompletion;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        // 고급 디폴트
        float dur = _duration > 0f ? _duration : 0.28f;
        int vib   = _vibrato  > 0  ? _vibrato  : 12;
        float rnd = _randomness >= 0f ? Mathf.Clamp(_randomness, 0f, 180f) : 90f;

        _rect.DOKill(false);

        // 아날로그: 현재 위치를 origin으로 재캡처 (조합에서 더 직관적)
        _originPos = _rect.anchoredPosition;

        if (_intensity <= 0f || dur <= 0f)
            yield break;

        Vector2 strength = GetStrength(_axis, _intensity);

        Tween tween = _rect
            .DOShakeAnchorPos(dur, strength, vib, rnd, snapping: false, fadeOut: true)
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
        _rect.anchoredPosition = _originPos;
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

        _originPos = _rect.anchoredPosition;
        return true;
    }

    private static Vector2 GetStrength(CpsShakeAxis axis, float intensity)
    {
        switch (axis)
        {
            case CpsShakeAxis.Y:
                return new Vector2(0f, intensity);
            case CpsShakeAxis.XY:
                return new Vector2(intensity, intensity);
            case CpsShakeAxis.X:
            default:
                return new Vector2(intensity, 0f);
        }
    }
}
