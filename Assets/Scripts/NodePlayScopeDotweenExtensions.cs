using DG.Tweening;

// DOTween 의존을 도메인에만 가두기 위한 브릿지.
// NodePlayScope Stop/Finish 시 이 Tween을 Kill/Complete 하도록 등록한다.
public static class CommandRunScopeDotweenExtensions
{
    public static Tween BindTo(this Tween t, CommandRunScope scope)
    {
        scope.RegisterTween(t);
        return t;
    }
    
    private static void RegisterTween(this CommandRunScope scope, Tween t)
    {
        if (scope == null || t == null) return;

        scope.Track(
            cancel: () => { if (t.IsActive()) t.Kill(); },
            finish: () => { if (t.IsActive()) t.Complete(); }
        );
    }
}