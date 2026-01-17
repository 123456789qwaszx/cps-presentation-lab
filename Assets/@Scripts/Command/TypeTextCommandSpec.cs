using System;
using System.Collections;
using TMPro;
using UnityEngine;

[CommandMenuHint(
    "Text",
    "Type Text",
    Sets = new[]
    {
        CpsCommandMenuSets.VnMainEnterFirstLine,
    },
    SetOrder = 20,
    Order = 20)]
[Serializable]
public sealed class TypeTextCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.DialogueBox00Text;

    [Header("Content")]
    [TextArea]
    public string text;

    [Header("Typing")]
    public float interval = 0.1f;

    public bool wait = false;
}

public sealed class TypeTextCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly ITimeSource _time;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;

    private readonly DialogueWidgetTarget _target;
    private readonly string _text;
    private readonly float _interval;
    private readonly bool _wait;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private TMP_Text _textComponent;
    private bool _resolveAttempted;
    private bool _isFinalized;

    public TypeTextCommand(
        IDialogueWidgetAccess widgets,
        ITimeSource time,
        string screenId,
        string widgetRoleKey,
        DialogueWidgetTarget target,
        string text,
        float interval,
        bool waitForCompletion = true)
    {
        _widgets       = widgets;
        _time          = time;
        _screenId      = screenId;
        _widgetRoleKey = widgetRoleKey;

        _target   = target;
        _text     = text;
        _interval = Mathf.Max(0f, interval);
        _wait     = waitForCompletion;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        string content = _text ?? string.Empty;

        if (string.IsNullOrEmpty(content))
        {
            _textComponent.text = string.Empty;
            _textComponent.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        _textComponent.text = content;
        _textComponent.ForceMeshUpdate();

        int totalVisible = _textComponent.textInfo != null
            ? _textComponent.textInfo.characterCount
            : 0;

        if (totalVisible <= 0)
        {
            _textComponent.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        if (_interval <= 0f)
        {
            _textComponent.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        _textComponent.maxVisibleCharacters = 0;
        
        float elapsed = 0f;
        int   shown = 0;

        while (shown < totalVisible)
        {
            if (IsCanceled(scope))
                yield break;
            
            if(_isFinalized)
                yield break;

            elapsed += _time.UnscaledDeltaTime * scope.TimeScale;

            int targetShown = Mathf.Min(
                totalVisible,
                Mathf.FloorToInt(elapsed / _interval)
            );

            if (targetShown != shown)
            {
                shown = targetShown;
                _textComponent.maxVisibleCharacters = shown;
            }

            if (shown >= totalVisible)
                break;

            yield return null;
        }

        _textComponent.maxVisibleCharacters = int.MaxValue;
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        ApplyFinalText();
    }
    
    public override void OnCommandCompleted(CommandRunScope scope)
    {
        ApplyFinalText();
    }
    
    private void ApplyFinalText()
    {
        if (_isFinalized)
            return;

        _isFinalized = true;
        
        if (!ResolveIfNeeded())
            return;

        string content = _text ?? string.Empty;
        _textComponent.text = content;
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
                $"[TypeTextCommand] TMP_Text not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        return true;
    }
}