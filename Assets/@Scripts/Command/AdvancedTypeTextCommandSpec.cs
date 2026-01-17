using System;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

[CommandMenuHint(
    "Text",
    "Advanced Type Text",
    Sets = new[]
    {
        CpsCommandMenuSets.VnMainEnterFirstLine,
    },
    SetOrder = 25,
    Order = 25)]
[Serializable]
public sealed class AdvancedTypeTextCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.DialogueBox00Text;

    [Header("Content")]
    [TextArea]
    public string text;

    [Header("[Inline Tags]\n" +
            "  <wait=1.5>  extra delay\n" +
            "  <speed3>    faster typing\n" +
            "  <pause>     default pause\n" +
            "\n" +
            "Typing"
    )]
    [Tooltip("기본 글자 간 간격(초). 태그가 없을 때의 기본 속도.")]
    public float baseInterval = 0.06f;

    [Tooltip("<pause> 또는 <wait> 태그가 수치 없이 쓰였을 때의 기본 대기 시간(초).")]
    public float defaultPauseSeconds = 0.25f;

    [Tooltip("true면 이 커맨드가 끝날 때까지 Step 진행을 기다린다.")]
    public bool wait = true;
    
    
}

public sealed class AdvancedTypeTextCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly ITimeSource _time;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;

    private readonly DialogueWidgetTarget _target;
    private readonly string _rawText;
    private readonly float _baseInterval;
    private readonly float _defaultPauseSeconds;
    private readonly bool _wait;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private TMP_Text _textComponent;
    private bool _resolveAttempted;
    private bool _isFinalized;

    public AdvancedTypeTextCommand(
        IDialogueWidgetAccess widgets,
        ITimeSource time,
        string screenId,
        string widgetRoleKey,
        DialogueWidgetTarget target,
        string text,
        float baseInterval,
        float defaultPauseSeconds,
        bool waitForCompletion = true)
    {
        _widgets             = widgets;
        _time                = time;
        _screenId            = screenId;
        _widgetRoleKey       = widgetRoleKey;

        _target              = target;
        _rawText             = text ?? string.Empty;
        _baseInterval        = Mathf.Max(0f, baseInterval);
        _defaultPauseSeconds = Mathf.Max(0f, defaultPauseSeconds);
        _wait                = waitForCompletion;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    #region Inline tags

    private enum InlineEventType
    {
        Wait,
        Speed
    }

    private struct InlineEvent
    {
        public int index;            // 이 글자 index 이후에 적용
        public InlineEventType type;
        public float value;          // Wait: seconds, Speed: multiplier
    }

    /// <summary>
    /// 태그가 들어간 원문을 파싱해서:
    /// - TMP에 넣을 실제 텍스트(태그 제거)
    /// - 글자 인덱스 기준 이벤트(Wait/Speed)를 반환한다.
    /// 
    /// 지원 태그 예:
    /// &lt;pause&gt;                → defaultPauseSeconds만큼 대기
    /// &lt;wait&gt; / &lt;wait0.3&gt; / &lt;wait=0.3&gt;  → 지정 시간 대기
    /// &lt;speed0.5&gt; / &lt;speed=0.5&gt;            → 이후 interval * 0.5
    /// </summary>
    private string ParseInlineTags(string raw, out List<InlineEvent> events)
    {
        events = new List<InlineEvent>();

        if (string.IsNullOrEmpty(raw))
            return string.Empty;

        var sb           = new System.Text.StringBuilder(raw.Length);
        int visibleIndex = 0;

        for (int i = 0; i < raw.Length; i++)
        {
            char c = raw[i];

            if (c == '<')
            {
                int end = raw.IndexOf('>', i + 1);
                if (end < 0)
                {
                    // 잘못된 태그: 그냥 출력
                    sb.Append(c);
                    visibleIndex++;
                    continue;
                }

                string tagBody = raw.Substring(i + 1, end - (i + 1)); // "wait0.3", "speed=0.5" 등
                i = end; // '>'까지 소비

                ParseTag(tagBody, visibleIndex, events);
                continue;
            }

            sb.Append(c);
            visibleIndex++;
        }

        return sb.ToString();
    }

    private void ParseTag(string tagBody, int visibleIndex, List<InlineEvent> events)
    {
        if (string.IsNullOrEmpty(tagBody))
            return;

        string lower = tagBody.ToLowerInvariant().Trim();

        // <pause>
        if (lower == "pause")
        {
            if (_defaultPauseSeconds > 0f)
            {
                events.Add(new InlineEvent
                {
                    index = visibleIndex,
                    type  = InlineEventType.Wait,
                    value = _defaultPauseSeconds
                });
            }
            return;
        }

        // <wait>, <wait0.3>, <wait=0.3>
        if (lower.StartsWith("wait"))
        {
            float seconds = _defaultPauseSeconds;
            string tail   = lower.Substring("wait".Length).Trim();

            if (!string.IsNullOrEmpty(tail))
            {
                if (tail[0] == '=')
                    tail = tail.Substring(1).Trim();

                if (float.TryParse(tail, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsed))
                    seconds = Mathf.Max(0f, parsed);
            }

            if (seconds > 0f)
            {
                events.Add(new InlineEvent
                {
                    index = visibleIndex,
                    type  = InlineEventType.Wait,
                    value = seconds
                });
            }

            return;
        }

        // <speed0.5>, <speed=0.5>
        if (lower.StartsWith("speed"))
        {
            string tail = lower.Substring("speed".Length).Trim();

            if (!string.IsNullOrEmpty(tail))
            {
                if (tail[0] == '=')
                    tail = tail.Substring(1).Trim();

                if (float.TryParse(tail, NumberStyles.Float, CultureInfo.InvariantCulture, out float mul))
                {
                    mul = Mathf.Max(0.01f, mul);

                    events.Add(new InlineEvent
                    {
                        index = visibleIndex,
                        type  = InlineEventType.Speed,
                        value = mul
                    });
                }
            }

            return;
        }

        // 그 외 태그들은 지금은 무시 (나중에 확장 가능)
    }

    #endregion

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        _isFinalized = false;

        if (string.IsNullOrEmpty(_rawText))
        {
            _textComponent.text = string.Empty;
            _textComponent.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        // 1) 태그 파싱
        List<InlineEvent> events;
        string display = ParseInlineTags(_rawText, out events);

        _textComponent.text = display;
        _textComponent.ForceMeshUpdate();

        int totalVisible = _textComponent.textInfo != null
            ? _textComponent.textInfo.characterCount
            : 0;

        if (totalVisible <= 0)
        {
            _textComponent.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        if (_baseInterval <= 0f)
        {
            // interval <= 0이면 즉시 출력, 태그 wait/speed 무시
            _textComponent.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        _textComponent.maxVisibleCharacters = 0;

        int   shown      = 0;
        int   eventIndex = 0;
        float interval   = _baseInterval;

        while (shown < totalVisible)
        {
            if (IsCanceled(scope) || _isFinalized)
                yield break;

            // 2) 현재 위치에 적용할 이벤트 처리
            while (eventIndex < events.Count && events[eventIndex].index == shown)
            {
                InlineEvent ev = events[eventIndex];

                switch (ev.type)
                {
                    case InlineEventType.Wait:
                        if (ev.value > 0f)
                        {
                            yield return Wait(scope, ev.value);
                            if (IsCanceled(scope) || _isFinalized)
                                yield break;
                        }
                        break;

                    case InlineEventType.Speed:
                        interval = Mathf.Max(0.01f, _baseInterval * ev.value);
                        break;
                }

                eventIndex++;
            }

            // 3) 글자 하나 증가
            shown++;
            _textComponent.maxVisibleCharacters = shown;

            if (shown >= totalVisible)
                break;

            // 4) 다음 글자까지 대기
            if (interval > 0f)
            {
                yield return Wait(scope, interval);
                if (IsCanceled(scope) || _isFinalized)
                    yield break;
            }
            else
            {
                // interval이 0으로 떨어지는 상황은 막았지만,
                // 혹시라도 방어적으로 한 프레임 쉬어주기
                yield return null;
            }
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

        // 최종 텍스트도 태그 제거 버전으로
        List<InlineEvent> _;
        string display = ParseInlineTags(_rawText, out _);

        _textComponent.text = display;
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
                $"[AdvancedTypeTextCommand] TMP_Text not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        return true;
    }
}
