using System.Collections;
using DG.Tweening;
using UnityEngine;

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

        tween.BindTo(scope);

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
