using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public sealed class FadeGraphicCommand : CommandBase
{
    private readonly Graphic _g;
    private readonly float _to;
    private readonly float _dur;
    private readonly bool _wait;

    public FadeGraphicCommand(Graphic g, float to, float dur, bool wait = true)
    {
        _g = g;
        _to = to;
        _dur = Mathf.Max(0f, dur);
        _wait = wait;
    }

    public override bool WaitForCompletion => _wait;

    protected override void OnSkip(NodePlayScope api)
    {
        if (_g == null) return;
        var c = _g.color;
        c.a = _to;
        _g.color = c;
    }

    protected override IEnumerator ExecuteInner(NodePlayScope api)
    {
        if (_g == null) yield break;

        Tween t = _g.DOFade(_to, _dur).SetUpdate(true);
        api.TrackTween(t);

        if (_wait)
            yield return t.WaitForCompletion();
    }
}