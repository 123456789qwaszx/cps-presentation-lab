using System;
using System.Collections;
using UnityEngine;

[Serializable]
[CommandMenuHint(
    "VFX",
    "Stop Persistent Effect",
    SetOrder = 51,
    Order = 51)]
public sealed class StopPersistentEffectCommandSpec : CommandSpecBase
{
    [Tooltip("중단할 effectKey. BubblePulse에서 자동 키를 썼다면 동일 규칙으로 직접 입력하거나, BubblePulseSpec.effectKey를 명시해두는 걸 권장")]
    public string effectKey;

    [Tooltip("Cancel=즉시 중단, Finish=최종상태로 점프/정리")]
    public ReplacePolicy policy = ReplacePolicy.Cancel;

    [Tooltip("스킵 중에도 실행할지(대개 true 추천)")]
    public bool executeEvenIfSkipping = true;
}

public sealed class StopPersistentEffectCommand : ISequenceCommand
{
    private readonly PersistentEffectRegistry _effects;
    private readonly string _effectKey;
    private readonly ReplacePolicy _policy;
    private readonly bool _executeEvenIfSkipping;

    public bool WaitForCompletion => false;

    public StopPersistentEffectCommand(
        PersistentEffectRegistry effects,
        string effectKey,
        ReplacePolicy policy,
        bool executeEvenIfSkipping)
    {
        _effects = effects;
        _effectKey = effectKey;
        _policy = policy;
        _executeEvenIfSkipping = executeEvenIfSkipping;
    }

    public IEnumerator Execute(CommandRunScope scope)
    {
        if (_effects == null) yield break;
        if (scope != null && scope.IsSkipping && !_executeEvenIfSkipping) yield break;
        if (string.IsNullOrWhiteSpace(_effectKey)) yield break;

        _effects.Stop(_effectKey, _policy);
        yield break;
    }
}