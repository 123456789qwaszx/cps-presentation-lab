using System;
using System.Collections.Generic;

public enum ReplacePolicy
{
    Cancel, // 즉시 중단 (Kill/Stop 느낌)
    Finish  // 최종 상태로 점프/완결 (Complete 느낌)
}

/// <summary>
/// A run-scoped active effect instance.
/// Cancel/Finish must be idempotent (safe to call multiple times).
/// </summary>
public readonly struct EffectInstance
{
    public readonly Action Cancel;
    public readonly Action Finish;

    public EffectInstance(Action cancel, Action finish)
    {
        Cancel = cancel;
        Finish = finish;
    }

    public void DoCancel()
    {
        try { Cancel?.Invoke(); } catch { }
    }

    public void DoFinish()
    {
        try { (Finish ?? Cancel)?.Invoke(); } catch { }
    }
}

/// <summary>
/// Registry to ensure "single active persistent effect per key".
/// - Replace: stops previous effect (Cancel/Finish) then installs new one.
/// - Auto cleanup: registers a run-scope cleanup hook once per installed instance.
/// </summary>
public sealed class PersistentEffectRegistry
{
    private readonly Dictionary<string, Entry> _active = new(StringComparer.Ordinal);

    private struct Entry
    {
        public EffectInstance Instance;
        public CommandRunScope Scope; // used only to install cleanup
        public bool IsInstalled;
    }

    /// <summary>
    /// Starts(or replaces) a run-scoped persistent effect.
    /// Returns true if a new effect was installed.
    /// </summary>
    public bool ReplaceRun(
        string effectKey,
        CommandRunScope scope,
        ReplacePolicy replacePolicy,
        Func<EffectInstance> create)
    {
        if (string.IsNullOrWhiteSpace(effectKey)) return false;
        if (scope == null || create == null) return false;

        // 1) If exists, replace it first
        if (_active.TryGetValue(effectKey, out var prev) && prev.IsInstalled)
        {
            ApplyReplace(prev.Instance, replacePolicy);
            _active.Remove(effectKey);
        }

        // 2) Create new instance
        EffectInstance next;
        try { next = create(); }
        catch { return false; }

        // 3) Install & register run cleanup (idempotent)
        var entry = new Entry { Instance = next, Scope = scope, IsInstalled = true };
        _active[effectKey] = entry;

        // Important: register cleanup so Run end clears the currently installed instance.
        // Cleanup should also remove registry entry to avoid accumulation.
        scope.TrackRun(
            cancel: () => Stop(effectKey, ReplacePolicy.Cancel),
            finish: () => Stop(effectKey, ReplacePolicy.Finish)
        );

        return true;
    }

    /// <summary>
    /// Stops the current active effect for the key (if any).
    /// </summary>
    public void Stop(string effectKey, ReplacePolicy policy)
    {
        if (string.IsNullOrWhiteSpace(effectKey)) return;

        if (_active.TryGetValue(effectKey, out var entry) && entry.IsInstalled)
        {
            ApplyReplace(entry.Instance, policy);
            _active.Remove(effectKey);
        }
    }

    /// <summary>
    /// Stops all active effects (useful for debug or external teardown).
    /// Typically you won't need this if you rely on scope.CleanupRun().
    /// </summary>
    public void StopAll(ReplacePolicy policy)
    {
        foreach (var kv in _active)
            ApplyReplace(kv.Value.Instance, policy);

        _active.Clear();
    }

    private static void ApplyReplace(EffectInstance instance, ReplacePolicy policy)
    {
        if (policy == ReplacePolicy.Finish) instance.DoFinish();
        else instance.DoCancel();
    }
}