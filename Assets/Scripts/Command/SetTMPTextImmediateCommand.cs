using System.Collections;
using TMPro;

public sealed class SetTMPTextImmediateCommand : CommandBase
{
    private readonly TMP_Text _target;
    private readonly string _text;

    public SetTMPTextImmediateCommand(TMP_Text target, string text)
    {
        _target = target;
        _text = text ?? "";
    }

    // 스킵 중이어도 실행해도 무방(오히려 최종 상태를 보장)
    protected override SkipPolicy SkipPolicy => SkipPolicy.ExecuteEvenIfSkipping;
    public override bool WaitForCompletion => true;

    protected override IEnumerator ExecuteInner(NodePlayScope api)
    {
        if (_target == null) yield break;
        _target.text = _text;
        yield break;
    }
}