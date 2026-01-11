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
        "Custom/Text/LineType"
    },
    SetOrder = 20,
    Order = 20)]
[Serializable]
public sealed class TypeTextCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.LineText;

    [Header("Content")]
    [TextArea]
    public string text;

    [Header("Typing")]
    /// <summary>
    /// <= 0이면 Config(TypeCharInterval) 기본값 사용
    /// </summary>
    public float interval = -1f;

    /// <summary>
    /// 보통 true 추천. (타이핑이 끝날 때까지 Step을 멈춤)
    /// false면 즉시 SetText처럼 완료 처리(= 타이핑 안 함).
    /// </summary>
    public bool wait = true;

    /// <summary>
    /// text가 비어있을 때도 비우고 종료할지
    /// </summary>
    public bool clearWhenEmpty = true;
}

public sealed class CpsTypeTextCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly DialogueWidgetTarget _target;
    private readonly string _text;
    private readonly float _interval;
    private readonly bool _wait;
    private readonly bool _clearWhenEmpty;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private TMP_Text _textComponent;
    private bool _resolved;

    public CpsTypeTextCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        string text,
        float interval,
        bool waitForCompletion = true,
        bool clearWhenEmpty = true)
    {
        _widgets        = widgets;
        _screenId       = screenId;
        _widgetId       = widgetId;

        _target         = target;
        _text           = text; // null 허용
        _interval       = Mathf.Max(0f, interval); // 팩토리에서 이미 디폴트 적용된 값이 들어옴
        _wait           = waitForCompletion;
        _clearWhenEmpty = clearWhenEmpty;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        string content = _text ?? string.Empty;

        // empty 처리: S급 디폴트는 안전하게 "정리하고 끝"
        if (string.IsNullOrEmpty(content))
        {
            if (_clearWhenEmpty)
            {
                _textComponent.text = string.Empty;
                _textComponent.maxVisibleCharacters = int.MaxValue;
            }
            yield break;
        }

        // wait=false면 "즉시 완료" (SetText처럼)
        if (!_wait)
        {
            _textComponent.text = content;
            _textComponent.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        // ---- 타이핑 시작 ----
        _textComponent.text = content;

        // rich text / TMP 내부 캐시 반영
        _textComponent.ForceMeshUpdate();

        int totalVisible = _textComponent.textInfo != null
            ? _textComponent.textInfo.characterCount
            : 0;
        if (totalVisible <= 0)
        {
            _textComponent.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        _textComponent.maxVisibleCharacters = 0;

        // interval == 0이면 즉시
        if (_interval <= 0f)
        {
            _textComponent.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        float t = 0f;
        int shown = 0;

        while (shown < totalVisible)
        {
            t += Time.unscaledDeltaTime;

            while (t >= _interval && shown < totalVisible)
            {
                t -= _interval;
                shown++;
                _textComponent.maxVisibleCharacters = shown;
            }

            yield return null;
        }

        // 최종 상태 보장
        _textComponent.maxVisibleCharacters = int.MaxValue;
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        // 스킵은 즉시 최종 상태
        _textComponent.text = _text ?? string.Empty;
        _textComponent.maxVisibleCharacters = int.MaxValue;
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _textComponent != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out _refs) || _refs == null)
            return false;

        _textComponent = _refs.GetText(_target);
        return _textComponent != null;
    }
}
