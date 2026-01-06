using System.Collections;
using DG.Tweening;
using UnityEngine;
using System;
using UnityEngine;

// 버튼/아이콘 “톡” 강조: vibrato 8~12, elasticity 0.6~1.0, duration 0.15~0.3
// 귀엽고 젤리처럼: vibrato 12~20, elasticity 1.0~1.5
// 묵직/딱딱: vibrato 2~6, elasticity 0~0.4
[Serializable]
public sealed class PunchScaleEmotionCommandSpec : CommandSpecBase
{
    [Header("Punch Scale Settings")]
    /// <summary>
    /// 현재 스케일 기준으로 얼마나 튀어오를지 (1 + punch 정도 느낌)
    /// </summary>
    public float punch = 0.2f;

    /// <summary>
    /// 연출 총 시간. <= 0이면 아무 것도 안 함.
    /// </summary>
    public float duration = 0.7f;

    /// <summary>
    /// 진동 횟수 (DOTween 기본값 10 정도).
    /// </summary>
    public int vibrato = 6;

    /// <summary>
    /// 탄성 (0~1+).
    /// </summary>
    public float elasticity = 0.4f;

    /// <summary>
    /// true면 이 커맨드가 끝날 때까지 Step 진행을 멈춤.
    /// 기본은 병행 연출이라 false 추천.
    /// </summary>
    public bool wait = false;
}
public sealed class CpsPunchScaleCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly float _punch;
    private readonly float _duration;
    private readonly int   _vibrato;
    private readonly float _elasticity;
    private readonly bool  _wait;

    private RectTransform _rect;
    private Vector3 _originScale;
    private bool _resolved;

    public CpsPunchScaleCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        float punch,
        float duration,
        int vibrato = 10,
        float elasticity = 1f,
        bool waitForCompletion = false)
    {
        _widgets    = widgets;
        _screenId   = screenId;
        _widgetId   = widgetId;

        _punch      = punch;
        _duration   = Mathf.Max(0f, duration);
        _vibrato    = Mathf.Max(1, vibrato);
        _elasticity = Mathf.Max(0f, elasticity);
        _wait       = waitForCompletion;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        _rect.DOKill(false);

        if (_duration <= 0f || Mathf.Approximately(_punch, 0f))
            yield break;

        Vector3 punchVec = new Vector3(_punch, _punch, 0f);

        Tween tween = _rect
            .DOPunchScale(punchVec, _duration, _vibrato, _elasticity)
            .SetUpdate(true);
        tween.OnKill(() => Debug.Log("[Tween] Killed"));
        tween.OnComplete(() => Debug.Log("[Tween] Completed"));

        tween.BindToStep(scope);

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
        if (_resolved) return _rect != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out var refs) || refs == null)
            return false;

        // ⭐ EmoteRect 우선, 없으면 PortraitRect
        _rect = refs.EmoteRect != null ? refs.EmoteRect : refs.PortraitRect;

        if (_rect != null)
            _originScale = _rect.localScale;

        return _rect != null;
    }
}
