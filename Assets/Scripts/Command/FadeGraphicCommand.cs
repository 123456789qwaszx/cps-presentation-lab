using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public sealed class FadeGraphicCommand : ISequenceCommand
{
    private readonly Graphic _g;
    private readonly float _to;
    private readonly float _duration;
    private readonly bool _wait;

    public FadeGraphicCommand(Graphic g, float to, float duration, bool wait = true)
    {
        _g = g;
        _to = to;
        _duration = Mathf.Max(0f, duration);
        _wait = wait;
    }

    public bool WaitForCompletion => _wait;

    public IEnumerator Execute(NodePlayScope scope)
    {
        if (_g == null) yield break;

        // Skip이면 즉시 완료 상태
        if (scope != null && scope.IsSkipping)
        {
            var c = _g.color;
            c.a = _to;
            _g.color = c;
            yield break;
        }

        Tween t = _g.DOFade(_to, _duration).SetUpdate(true);
        scope.TrackTween(t);

        if (_wait)
            yield return t.WaitForCompletion();
    }
}