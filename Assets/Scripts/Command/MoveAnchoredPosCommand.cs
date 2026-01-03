using System.Collections;
using DG.Tweening;
using UnityEngine;

public sealed class MoveAnchoredPosCommand : CommandBase
{
    private readonly RectTransform _rt;
    private readonly Vector2 _to;
    private readonly float _dur;
    private readonly Ease _ease;
    private readonly bool _wait;

    public MoveAnchoredPosCommand(RectTransform rt, Vector2 to, float dur, Ease ease = Ease.OutCubic, bool wait = true)
    {
        _rt = rt;
        _to = to;
        _dur = Mathf.Max(0f, dur);
        _ease = ease;
        _wait = wait;
    }

    public override bool WaitForCompletion => _wait;

    protected override void OnSkip(NodePlayScope api)
    {
        if (_rt == null) return;
        _rt.anchoredPosition = _to;
    }

    protected override IEnumerator ExecuteInner(NodePlayScope api)
    {
        if (_rt == null) yield break;

        Tween t = _rt.DOAnchorPos(_to, _dur).SetEase(_ease).SetUpdate(true);
        api.TrackTween(t);

        if (_wait)
            yield return t.WaitForCompletion();
    }
}