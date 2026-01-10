using System;
using System.Collections;
using TMPro;
using UnityEngine;

[Serializable]
public sealed class SetTextCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.LineText;

    [Header("Content")]
    [TextArea]
    public string text;

    [Header("Behavior")]
    public bool clearWhenEmpty = true; // text가 비었을 때도 비울지 여부
}


public sealed class CpsSetTextCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly DialogueWidgetTarget _target;
    private readonly string _text;
    private readonly bool _clearWhenEmpty;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private bool _resolved;

    public CpsSetTextCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        string text,
        bool clearWhenEmpty = true)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target = target;
        _text = text; // null 허용 (아래에서 처리)
        _clearWhenEmpty = clearWhenEmpty;
    }

    public override bool WaitForCompletion => false;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;
        
        Apply();
        yield break;
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;
        
        Apply();
    }

    private void Apply()
    {
        TMP_Text targetText = _refs.GetText(_target);
        if (targetText == null)
            return;

        string content = _text ?? string.Empty;

        if (string.IsNullOrEmpty(content) && !_clearWhenEmpty)
            return;

        targetText.text = content;

        // 타이핑 흔적 제거
        targetText.maxVisibleCharacters = int.MaxValue;
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _refs != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out _refs) || _refs == null)
            return false;

        return true;
    }
}