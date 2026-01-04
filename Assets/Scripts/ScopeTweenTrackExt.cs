using System;
using DG.Tweening;

public static class ScopeTweenTrackExt
{
    public static void TrackTween(this NodePlayScope scope, Tween t, Action finalize = null)
    {
        if (scope == null || t == null)
        {
            finalize?.Invoke();
            return;
        }

        scope.Track(
            cancel:  () => { if (t.IsActive()) t.Kill(false); },
            finish:  () => { if (t.IsActive()) t.Kill(true);  }, // Finish=완료상태로 만들고 Kill
            finalize: finalize
        );
    }
}