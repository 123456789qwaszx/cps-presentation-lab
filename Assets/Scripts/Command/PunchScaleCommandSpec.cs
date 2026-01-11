using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

[Serializable]
[CommandMenuHint(
    "Motion/Emote",
    "Punch Scale",
    Sets = new[]
    {
        "Custom/Portrait/ChangeEmotion",
        "Custom/Emote/HitStrong"
    },
    SetOrder = 20,
    Order = 10)]
public sealed class PunchScaleCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.StandingPortraitImage;

    [Header("Punch")]
    /// <summary>
    /// 0.15 ~ 0.35 정도가 UI에서 예쁘게 먹음
    /// </summary>
    public float strength = 0.25f;

    /// <summary>
    /// <= 0이면 고급 디폴트 사용
    /// </summary>
    public float duration = -1f;

    /// <summary>
    /// <= 0이면 고급 디폴트 사용
    /// </summary>
    public int vibrato = 0;

    /// <summary>
    /// < 0이면 고급 디폴트 사용
    /// </summary>
    public float elasticity = -1f;

    public bool wait = false;
}

public sealed class CpsPunchScaleCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly DialogueWidgetTarget _target;

    private readonly float _strength;
    private readonly float _duration;
    private readonly int   _vibrato;
    private readonly float _elasticity;
    private readonly bool  _wait;
    
    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform _rect;
    private Vector3 _originScale;
    private bool _resolved;

    public CpsPunchScaleCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        float strength,
        float duration,
        int vibrato,
        float elasticity,
        bool waitForCompletion = false)
    {
        _widgets  = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target = target;

        _strength   = Mathf.Max(0f, strength);
        _duration   = Mathf.Max(0f, duration);
        _vibrato    = Mathf.Max(0, vibrato);
        _elasticity = elasticity; // sentinel 허용
        _wait       = waitForCompletion;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        // 고급 디폴트
        float dur = _duration > 0f ? _duration : 0.22f;
        int vib   = _vibrato  > 0  ? _vibrato  : 8;
        float ela = _elasticity >= 0f ? Mathf.Clamp01(_elasticity) : 0.75f;

        _rect.DOKill(false);

        // 혹시 이전 연출로 scale이 틀어져 있으면, 현재를 origin으로 새로 잡는다 (아날로그)
        _originScale = _rect.localScale;

        if (_strength <= 0f || dur <= 0f)
            yield break;

        Tween tween = _rect
            .DOPunchScale(new Vector3(_strength, _strength, 0f), dur, vib, ela)
            .SetUpdate(true);

        // 런 정리
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

        if (!_widgets.TryResolve(_screenId, _widgetId, out _refs) || _refs == null)
            return false;

        _rect = _refs.GetRect(_target);
        if (_rect == null)
            return false;

        _originScale = _rect.localScale;
        return true;
    }
}
