using DG.Tweening;

public static class ScopeTweenTrackExt
{
    public static void TrackTween(this NodePlayScope scope, Tween t)
    {
        if (scope == null || t == null) return;

        scope.Track(
            cancel: () => { if (t.IsActive()) t.Kill(); },
            finish: () => { if (t.IsActive()) t.Complete(); }
        );
    }
}