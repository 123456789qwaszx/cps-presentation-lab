using System.Collections;
using TMPro;
using UnityEngine;

public sealed class TypeTextCommand : CommandBase
{
    private readonly TMP_Text _target;
    private readonly string _fullText;
    private readonly float _charInterval;
    private readonly bool _waitForCompletion;

    public TypeTextCommand(TMP_Text target, string fullText, float charInterval = 0.02f, bool waitForCompletion = true)
    {
        _target = target;
        _fullText = fullText ?? "";
        _charInterval = Mathf.Max(0f, charInterval);
        _waitForCompletion = waitForCompletion;
    }

    public override bool WaitForCompletion => _waitForCompletion;
    
    //protected override string DebugName => $"TypeText({(_target != null ? _target.name : "null")})";

    protected override void OnSkip(NodePlayScope api)
    {
        if (_target == null) return;

        _target.text = _fullText;
        // TMP는 maxVisibleCharacters를 크게 주면 전체 표시됨
        _target.maxVisibleCharacters = int.MaxValue;
    }

    protected override IEnumerator ExecuteInner(NodePlayScope api)
    {
        if (_target == null) yield break;
        if (api == null) yield break;

        // 텍스트 세팅 후, TMP가 characterCount를 계산하도록 갱신
        _target.text = _fullText;
        _target.maxVisibleCharacters = 0;
        _target.ForceMeshUpdate();

        int totalChars = _target.textInfo.characterCount;

        // 내용이 없거나, interval이 0이면 즉시 완성
        if (totalChars <= 0 || _charInterval <= 0f)
        {
            _target.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        for (int visible = 1; visible <= totalChars; visible++)
        {
            if (api.Token.IsCancellationRequested) yield break;

            // 스킵은 "즉시 완료 상태"로
            if (api.IsSkipping)
            {
                OnSkip(api);
                yield break;
            }

            _target.maxVisibleCharacters = visible;

            // GateRunner 철학과 동일: unscaled + api.TimeScale
            float t = 0f;
            while (t < _charInterval)
            {
                if (api.Token.IsCancellationRequested) yield break;

                if (api.IsSkipping)
                {
                    OnSkip(api);
                    yield break;
                }

                t += Time.unscaledDeltaTime * api.TimeScale;
                yield return null;
            }
        }

        // 안전하게 최종 상태 보장
        _target.maxVisibleCharacters = int.MaxValue;
    }
}
