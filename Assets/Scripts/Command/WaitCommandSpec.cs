using System;
using UnityEngine;
using System.Collections;

[Serializable]
public sealed class WaitCommandSpec : CommandSpecBase
{
    [Header("Time")]
    public float seconds = 0.2f;

    [Header("Clock")]
    public bool respectTimeScale = true; // ctx.TimeScale 반영 (StepGateAdvancer와 동일 감각)
}

public sealed class CpsWaitCommand : CommandBase
{
    private readonly ITimeSource _time;
    private readonly float _seconds;
    private readonly bool _respectTimeScale;

    public CpsWaitCommand(ITimeSource time, float seconds, bool respectTimeScale = true)
    {
        _time = time;
        _seconds = Mathf.Max(0f, seconds);
        _respectTimeScale = respectTimeScale;
    }

    public override bool WaitForCompletion => true;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (_seconds <= 0f || _time == null)
            yield break;

        float remaining = _seconds;

        while (remaining > 0f)
        {
            // StepGateAdvancer와 같은 감각: unscaled dt * timeScale(최소 0.01)
            float scale = GetTimeScale(scope);
            float dt = _time.UnscaledDeltaTime * scale;

            remaining -= dt;
            yield return null;
        }
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        // 즉시 완료
    }

    private float GetTimeScale(CommandRunScope scope)
    {
        if (!_respectTimeScale) return 1f;

        // 네 프로젝트 규칙과 동일하게: <=0이면 0.01로 바닥값
        DialogueContext ctx = scope?.Playback;
        float ts = (ctx != null ? ctx.TimeScale : 1f);
        return ts > 0f ? ts : 0.01f;
    }
}
