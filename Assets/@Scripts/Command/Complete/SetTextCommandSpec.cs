using System;
using System.Collections;
using TMPro;
using UnityEngine;

[CommandMenuHint(
    "Text",
    "Set Text",
    Sets = new[]
    {
        CpsCommandMenuSets.VnMainEnterFirstLine,
    },
    SetOrder = 10,
    Order = 10)]
[Serializable]
public sealed class SetTextCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.LineText;

    [Header("Content")]
    [TextArea]
    public string text;

    [Header("Behavior")]
    [Tooltip("텍스트가 비었을 때 기존 내용을 지울지 여부")]
    public bool clearWhenEmpty = true;
}

public sealed class SetTextCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;

    private readonly DialogueWidgetTarget _target;
    private readonly string _text;
    private readonly bool _clearWhenEmpty;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private TMP_Text _textComponent;
    private bool _resolveAttempted;

    public SetTextCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        DialogueWidgetTarget target,
        string text,
        bool clearWhenEmpty = true)
    {
        _widgets       = widgets;
        _screenId      = screenId;
        _widgetRoleKey = widgetRoleKey;

        _target         = target;
        _text           = text;
        _clearWhenEmpty = clearWhenEmpty;
    }

    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        Apply();
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        Apply();
    }

    private void Apply()
    {
        if (_textComponent == null)
            return;

        string content = _text ?? string.Empty;

        if (string.IsNullOrEmpty(content) && !_clearWhenEmpty)
            return;

        _textComponent.text = content;

        // 이전 타이핑 효과 흔적 제거 (항상 전체가 보이도록)
        _textComponent.maxVisibleCharacters = int.MaxValue;
    }

    private bool ResolveIfNeeded()
    {
        if (_textComponent != null)
            return true;

        if (_resolveAttempted)
            return false;

        _resolveAttempted = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null)
            return false;

        _textComponent = _refs.GetText(_target);
        if (_textComponent == null)
        {
            Debug.LogWarning(
                $"[SetTextCommand] TMP_Text not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        return true;
    }
}