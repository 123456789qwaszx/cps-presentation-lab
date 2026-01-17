using System.Collections;
using UnityEngine;

public abstract class PersistentCommandBase : CommandBase
{
    // Persistent effects are always non-blocking.
    public sealed override bool WaitForCompletion => false;

    // Default: don't start persistent effects when skipping.
    // (If you ever need "still apply during skip", override to ExecuteEvenIfSkipping in derived.)
    protected override SkipPolicy SkipPolicy => SkipPolicy.Ignore;

    // Prevent accidental StepLifetime binding for persistent commands.
    public sealed override void RegisterStepLifetime(CommandRunScope scope, MonoBehaviour host, IEnumerator routine)
    {
        // Intentionally empty: persistent effects MUST be run-scoped only.
    }

    protected sealed override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        ApplyPersistent(scope);
        yield break;
    }

    // Implementations should call _effects.ReplaceRun(...) here.
    protected abstract void ApplyPersistent(CommandRunScope scope);
}