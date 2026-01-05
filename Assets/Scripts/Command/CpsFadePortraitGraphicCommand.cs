using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public sealed class CpsFadePortraitGraphicCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly float _fromAlpha;
    private readonly float _toAlpha;
    private readonly float _duration;
    private readonly bool  _wait;

    private Graphic _graphic;
    private bool _resolved;

    public CpsFadePortraitGraphicCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        float fromAlpha,
        float toAlpha,
        float duration,
        bool waitForCompletion = false)
    {
        _widgets   = widgets;
        _screenId  = screenId;
        _widgetId  = widgetId;

        _fromAlpha = Mathf.Clamp01(fromAlpha);
        _toAlpha   = Mathf.Clamp01(toAlpha);
        _duration  = Mathf.Max(0f, duration);
        _wait      = waitForCompletion;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        _graphic.DOKill(false);

        var c = _graphic.color;
        c.a = _fromAlpha;
        _graphic.color = c;

        if (_duration <= 0f)
        {
            c.a = _toAlpha;
            _graphic.color = c;
            yield break;
        }

        Tween tween = _graphic
            .DOFade(_toAlpha, _duration)
            .SetUpdate(true);

        tween.BindToStep(scope);

        if (_wait)
            yield return tween.WaitForCompletion();
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        _graphic.DOKill(false);

        var c = _graphic.color;
        c.a = _toAlpha;
        _graphic.color = c;
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _graphic != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out var refs) || refs == null)
            return false;

        _graphic = refs.PortraitGraphic;
        return _graphic != null;
    }
}
