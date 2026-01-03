using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public sealed class CpsShowLineCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly IDialogueSpeakerService _speakers;

    private readonly DialogueLine _line;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly float _typeInterval;
    private readonly bool _slidePortrait;
    private readonly float _slideDur;
    private readonly float _slideOffsetX;
    private readonly float _fadeDur;

    // 실행 중 resolve된 refs (OnSkip/ExecuteInner 둘 다 쓰려고 저장)
    private IDialogueWidgetAccess.WidgetRefs _refs;
    private Vector2 _portraitDestPos;

    public CpsShowLineCommand(
        IDialogueWidgetAccess widgets,
        IDialogueSpeakerService speakers,
        DialogueLine line,
        string screenId,
        string widgetId,
        float typeCharInterval,
        bool slidePortrait,
        float portraitSlideDuration,
        float portraitSlideOffsetX,
        float portraitFadeDuration)
    {
        _widgets = widgets;
        _speakers = speakers;

        _line = line;
        _screenId = screenId;
        _widgetId = widgetId;

        _typeInterval = Mathf.Max(0f, typeCharInterval);
        _slidePortrait = slidePortrait;
        _slideDur = Mathf.Max(0f, portraitSlideDuration);
        _slideOffsetX = Mathf.Max(0f, portraitSlideOffsetX);
        _fadeDur = Mathf.Max(0f, portraitFadeDuration);
    }

    //public override string DebugName => $"ShowLine({_screenId}:{_widgetId})";
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;
    public override bool WaitForCompletion => true;

    protected override void OnSkip(NodePlayScope api)
    {
        ResolveIfNeeded();

        if (_refs == null) return;

        // 1) 이름
        if (_refs.NameText != null)
            _refs.NameText.text = _speakers != null ? _speakers.GetDisplayName(_line.speakerId) : (_line.speakerId ?? "");

        // 2) 본문: 즉시 완성
        if (_refs.BodyText != null)
        {
            _refs.BodyText.text = _line.text ?? "";
            _refs.BodyText.maxVisibleCharacters = int.MaxValue;
        }

        // 3) 초상화: 즉시 최종 상태
        if (_refs.PortraitImage != null && _speakers != null)
            _refs.PortraitImage.sprite = _speakers.GetPortrait(_line.speakerId, _line.expression);

        if (_refs.PortraitRect != null)
            _refs.PortraitRect.anchoredPosition = _portraitDestPos;

        if (_refs.PortraitGraphic != null)
        {
            var c = _refs.PortraitGraphic.color;
            c.a = 1f;
            _refs.PortraitGraphic.color = c;
        }
    }

    protected override IEnumerator ExecuteInner(NodePlayScope api)
    {
        ResolveIfNeeded();
        if (_refs == null) yield break;

        // ---- 이름 ----
        if (_refs.NameText != null)
            _refs.NameText.text = _speakers != null ? _speakers.GetDisplayName(_line.speakerId) : (_line.speakerId ?? "");

        // ---- 초상화 세팅 ----
        if (_refs.PortraitImage != null && _speakers != null)
            _refs.PortraitImage.sprite = _speakers.GetPortrait(_line.speakerId, _line.expression);

        // ---- 초상화 연출 (wait 없이 돌려도 됨) ----
        if (_slidePortrait && _refs.PortraitRect != null)
        {
            // 시작 위치를 왼쪽으로 밀고, 목적지는 현재 위치(=dest)
            _refs.PortraitRect.anchoredPosition = _portraitDestPos + new Vector2(-_slideOffsetX, 0f);

            Sequence seq = DOTween.Sequence().SetUpdate(true)
                .Append(_refs.PortraitRect.DOAnchorPos(_portraitDestPos, _slideDur).SetEase(Ease.OutCubic));

            if (_refs.PortraitGraphic != null)
            {
                // 페이드 인
                var c = _refs.PortraitGraphic.color;
                c.a = 0f;
                _refs.PortraitGraphic.color = c;

                seq.Join(_refs.PortraitGraphic.DOFade(1f, _fadeDur));
            }

            api.TrackTween(seq);
            // 여기서는 타이핑과 병행하고 싶으니 기다리지 않음
        }

        // ---- 본문 타이핑 ----
        if (_refs.BodyText != null)
        {
            // 너가 만든 TypeTextCommand 재사용(표준치안/Skip/TimeScale 반영됨)
            var typeCmd = new TypeTextCommand(_refs.BodyText, _line.text ?? "", _typeInterval, waitForCompletion: true);
            yield return typeCmd.Execute(api);
        }
    }

    private void ResolveIfNeeded()
    {
        if (_refs != null) return;
        if (_widgets == null) return;

        if (_widgets.TryResolve(_screenId, _widgetId, out var refs))
        {
            _refs = refs;

            // 목적지는 “현재 위치”를 dest로 본다 (테스트/빌드 흐름에 잘 맞음)
            if (_refs?.PortraitRect != null)
                _portraitDestPos = _refs.PortraitRect.anchoredPosition;
        }
    }
}
