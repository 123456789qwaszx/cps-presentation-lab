using System;
using System.Collections;
using TMPro;
using UnityEngine;

[Serializable]
public sealed class TypeTextCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public CpsTextTarget target = CpsTextTarget.BodyText;

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

    private readonly CpsTextTarget _target;
    private readonly string _text;
    private readonly float _interval;
    private readonly bool _wait;
    private readonly bool _clearWhenEmpty;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private bool _resolved;

    public CpsTypeTextCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        CpsTextTarget target,
        string text,
        float interval,
        bool waitForCompletion = true,
        bool clearWhenEmpty = true)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target = target;
        _text = text; // null 허용
        _interval = Mathf.Max(0f, interval);
        _wait = waitForCompletion;
        _clearWhenEmpty = clearWhenEmpty;
    }

    public override bool WaitForCompletion => _wait;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        TMP_Text targetText = ResolveTargetText(_refs, _target);
        if (targetText == null)
            yield break;

        string content = _text ?? string.Empty;

        // empty 처리: S급 디폴트는 안전하게 "정리하고 끝"
        if (string.IsNullOrEmpty(content))
        {
            if (_clearWhenEmpty)
            {
                targetText.text = string.Empty;
                targetText.maxVisibleCharacters = int.MaxValue;
            }
            yield break;
        }

        // wait=false면 "즉시 완료" (SetText처럼) = 가장 단순하고 예측 가능
        if (!_wait)
        {
            targetText.text = content;
            targetText.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        // ---- 타이핑 시작 ----
        targetText.text = content;

        // rich text / TMP 내부 캐시 반영 (characterCount 얻기 위함)
        targetText.ForceMeshUpdate();

        int totalVisible = targetText.textInfo != null ? targetText.textInfo.characterCount : 0;
        if (totalVisible <= 0)
        {
            targetText.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        targetText.maxVisibleCharacters = 0;

        // interval == 0이면 즉시
        if (_interval <= 0f)
        {
            targetText.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        // unscaled 기준(= DOTween SetUpdate(true) 감각과 맞춤)
        float t = 0f;
        int shown = 0;

        while (shown < totalVisible)
        {
            // 한 프레임씩 시간 누적
            t += Time.unscaledDeltaTime;

            // interval만큼 지날 때마다 한 글자 노출
            while (t >= _interval && shown < totalVisible)
            {
                t -= _interval;
                shown++;
                targetText.maxVisibleCharacters = shown;
            }

            yield return null;
        }

        // 최종 상태 보장
        targetText.maxVisibleCharacters = int.MaxValue;
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        TMP_Text targetText = ResolveTargetText(_refs, _target);
        if (targetText == null)
            return;

        // 스킵은 즉시 최종 상태
        targetText.text = _text ?? string.Empty;
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
