using System;
using System.Collections;
using TMPro;
using UnityEngine;

public enum CpsTextTarget
{
    Auto = 0,   // BodyText 있으면 Body, 없으면 NameText
    BodyText,
    NameText,
}


[Serializable]
public sealed class SetTextCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public CpsTextTarget target = CpsTextTarget.Auto;

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

    private readonly CpsTextTarget _target;
    private readonly string _text;
    private readonly bool _clearWhenEmpty;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private bool _resolved;

    public CpsSetTextCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        CpsTextTarget target,
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
        Apply();
        yield break;
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        Apply();
    }

    private void Apply()
    {
        if (!ResolveIfNeeded())
            return;

        TMP_Text targetText = ResolveTargetText(_refs, _target);
        if (targetText == null)
            return;

        string content = _text ?? string.Empty;

        if (string.IsNullOrEmpty(content) && !_clearWhenEmpty)
            return;

        targetText.text = content;

        // ✅ 타이핑 흔적 제거 (S급 디폴트)
        targetText.maxVisibleCharacters = int.MaxValue;
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _refs != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (_widgets.TryResolve(_screenId, _widgetId, out var refs) && refs != null)
        {
            _refs = refs;
            return true;
        }

        return false;
    }

    private static TMP_Text ResolveTargetText(IDialogueWidgetAccess.WidgetRefs refs, CpsTextTarget target)
    {
        if (refs == null) return null;

        switch (target)
        {
            case CpsTextTarget.BodyText:
                return refs.BodyText;

            case CpsTextTarget.NameText:
                return refs.NameText;

            case CpsTextTarget.Auto:
            default:
                return refs.BodyText != null ? refs.BodyText : refs.NameText;
        }
    }
}