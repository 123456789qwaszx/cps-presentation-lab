using DG.Tweening;

// DOTween 의존을 도메인에만 가두기 위한 브릿지.
// CommandRunScope Stop/Finish 시 이 Tween을 Kill/Complete 하도록 등록한다.
public static class CommandRunScopeDotweenExtensions
{
    public static Tween BindToStep(this Tween t, CommandRunScope scope)
    {
        scope.RegisterTweenToTrackStep(t);
        return t;
    }
    
    public static Tween BindToRun(this Tween t, CommandRunScope scope)
    {
        scope.RegisterTweenToTrackRun(t);
        return t;
    }
    
    private static void RegisterTweenToTrackStep(this CommandRunScope scope, Tween t)
    {
        if (scope == null || t == null) return;

        scope.TrackStep(
            cancel: () => { if (t.IsActive()) t.Kill(); },
            finish: () => { if (t.IsActive()) t.Complete(); }
        );
    }
    
    private static void RegisterTweenToTrackRun(this CommandRunScope scope, Tween t)
    {
        if (scope == null || t == null) return;

        scope.TrackRun(
            cancel: () => { if (t.IsActive()) t.Kill(); },
            finish: () => { if (t.IsActive()) t.Complete(); }
        );
    }
}