using System;
using UnityEngine;
using System.Collections;

[Serializable]
public sealed class HoldSignalCommandSpec : CommandSpecBase
{
    [Header("Signal")]
    public string signalKey;

    [Header("Timeout")]
    /// <summary>
    /// <= 0이면 무제한 대기
    /// </summary>
    public float timeoutSeconds = -1f;

    [Header("Clock")]
    public bool respectTimeScale = true;
}


public sealed class CpsHoldSignalCommand : CommandBase
{
    private readonly ISignalBus _signals;
    private readonly ITimeSource _time;

    private readonly string _signalKey;
    private readonly float _timeout;
    private readonly bool _respectTimeScale;

    private bool _hit;

    public CpsHoldSignalCommand(
        ISignalBus signals,
        ITimeSource time,
        string signalKey,
        float timeoutSeconds = -1f,
        bool respectTimeScale = true)
    {
        _signals = signals;
        _time = time;
        _signalKey = signalKey;
        _timeout = timeoutSeconds;
        _respectTimeScale = respectTimeScale;
    }

    public override bool WaitForCompletion => true;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (_signals == null || string.IsNullOrEmpty(_signalKey))
            yield break;

        _hit = false;
        float elapsed = 0f;

        void OnSignal(string key)
        {
            if (string.Equals(key, _signalKey, StringComparison.Ordinal))
                _hit = true;
        }

        _signals.OnSignal += OnSignal;

        try
        {
            while (!_hit)
            {
                // 타임아웃 처리
                if (_timeout > 0f)
                {
                    if (_time == null)
                        break;

                    float dt = _time.UnscaledDeltaTime * GetTimeScale(scope);
                    elapsed += dt;

                    if (elapsed >= _timeout)
                        break;
                }

                yield return null;
            }
        }
        finally
        {
            _signals.OnSignal -= OnSignal;
        }
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        // 즉시 완료. (ExecuteInner에서 구독 중이었으면 finally로 해제됨)
    }

    private float GetTimeScale(CommandRunScope scope)
    {
        if (!_respectTimeScale) return 1f;

        DialogueContext ctx = scope?.Playback;
        float ts = (ctx != null ? ctx.TimeScale : 1f);
        return ts > 0f ? ts : 0.01f;
    }
}
