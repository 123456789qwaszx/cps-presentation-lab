using DG.Tweening;
using UnityEngine;

public sealed class CpsNodeCommandFactory : INodeCommandFactory
{
    private readonly CpsCommandServiceConfig _config;
    private readonly IDialogueWidgetAccess _widgets;
    private readonly IDialogueSpeakerService _speakers;
    
    public CpsNodeCommandFactory(CpsCommandServiceConfig config)
    {
        _config   = config;
        _widgets  = config.WidgetAccess;
        _speakers = config.SpeakerService;
    }

    private float TypeInterval => _config != null ? _config.TypeCharInterval : 0.03f;
    private PortraitSlideSettings Slide => _config?.PortraitSlide;
    private MovePortraitSettings MoveCfg => _config?.MovePortrait;

    public bool TryCreate(CommandSpecBase spec, out ISequenceCommand command)
    {
        command = null;
        if (spec == null) return false;

        switch (spec)
        {
            case ShowLineCommandSpec show:
                if (show.line == null)return false;

                command = new CpsShowLineCommand(
                    widgets: _widgets,
                    speakers: _speakers,
                    line: show.line,
                    screenId: spec.screenId,
                    widgetId: spec.widgetId,
                    typeCharInterval: TypeInterval,
                    slidePortrait: Slide != null && Slide.enable,
                    portraitSlideDuration: Slide?.duration ?? 0.5f,
                    portraitSlideOffsetX: Slide?.offsetX ?? 800f,
                    portraitFadeDuration: Slide?.fadeDur ?? 0.25f
                );
                return command != null;

            case MovePortraitCommandSpec move:
            {
                Vector2 dest = move.offset != Vector2.zero
                    ? move.offset
                    : (MoveCfg != null ? MoveCfg.defaultOffset : Vector2.zero);

                float dur = move.duration > 0f
                    ? move.duration
                    : (MoveCfg != null ? Mathf.Max(0f, MoveCfg.duration) : 0.25f);

                Ease ease = MoveCfg != null ? MoveCfg.ease : move.ease;
                bool wait = MoveCfg != null ? MoveCfg.wait : move.wait;

                command = new CpsMovePortraitCommand(
                    widgets: _widgets,
                    screenId: move.screenId,
                    widgetId: move.widgetId,
                    destPos: dest,
                    duration: dur,
                    ease: ease,
                    wait: wait
                );
                return command != null;
            }
            
            case SetSpeakerNameCommandSpec nameSpec:
            {
                if (nameSpec.line == null)
                    return false;
                
                command = new CpsSetSpeakerNameCommand(
                    widgets: _widgets,
                    speakers: _speakers,
                    line: nameSpec.line,
                    screenId: nameSpec.screenId,
                    widgetId: nameSpec.widgetId
                );
                return command != null;
            }
            
            case SetPortraitSpriteCommandSpec portraitSpec:
            {
                if (portraitSpec.line == null)
                    return false;

                command = new CpsSetPortraitSpriteCommand(
                    widgets: _widgets,
                    speakers: _speakers,
                    line: portraitSpec.line,
                    screenId: portraitSpec.screenId,
                    widgetId: portraitSpec.widgetId
                );
                return command != null;
            }
            
            case FadePortraitGraphicCommandSpec fadeSpec:
            {
                float dur = fadeSpec.duration > 0f
                    ? fadeSpec.duration
                    : (Slide != null ? Mathf.Max(0f, Slide.fadeDur) : 0.25f);

                command = new CpsFadePortraitGraphicCommand(
                    widgets: _widgets,
                    screenId: fadeSpec.screenId,
                    widgetId: fadeSpec.widgetId,
                    fromAlpha: fadeSpec.fromAlpha,
                    toAlpha: fadeSpec.toAlpha,
                    duration: dur,
                    waitForCompletion: fadeSpec.wait
                );
                return command != null;
            }
            
            case TypeBodyTextCommandSpec typeSpec:
            {
                // 1) 텍스트 결정: 우선 text, 없으면 line.text
                string text = !string.IsNullOrEmpty(typeSpec.text)
                    ? typeSpec.text
                    : (typeSpec.line != null ? typeSpec.line.text : string.Empty);

                // 2) 타이핑 간격: Spec 우선, 없으면 Config 기본값
                float interval = typeSpec.charInterval > 0f
                    ? typeSpec.charInterval
                    : TypeInterval; // _config.TypeCharInterval

                command = new CpsTypeTextCommand(
                    widgets: _widgets,
                    screenId: typeSpec.screenId,
                    widgetId: typeSpec.widgetId,
                    text: text,
                    interval: interval,
                    waitForCompletion: typeSpec.wait
                );
                return command != null;
            }
            
            case SlidePortraitInCommandSpec slideSpec:
            {
                // offsetX: Spec 우선, 0이면 Config 기본값 사용
                float offsetX = Mathf.Abs(slideSpec.offsetX) > 0f
                    ? slideSpec.offsetX
                    : (Slide != null ? Slide.offsetX : 800f);

                // duration: Spec 우선, <=0이면 Config 기본값 사용
                float dur = slideSpec.duration > 0f
                    ? slideSpec.duration
                    : (Slide != null ? Mathf.Max(0f, Slide.duration) : 0.5f);

                Ease ease = slideSpec.ease;   // 필요하면 Slide 쪽에 ease 추가해서 거기서 가져와도 됨
                bool wait = slideSpec.wait;

                command = new CpsSlidePortraitInCommand(
                    widgets: _widgets,
                    screenId: slideSpec.screenId,
                    widgetId: slideSpec.widgetId,
                    offsetX: offsetX,
                    duration: dur,
                    ease: ease,
                    waitForCompletion: wait
                );
                return command != null;
            }
            
            case PunchScaleEmotionCommandSpec punchSpec:
            {
                command = new CpsPunchScaleCommand(
                    widgets: _widgets,
                    screenId: punchSpec.screenId,
                    widgetId: punchSpec.widgetId,
                    punch: punchSpec.punch,
                    duration: punchSpec.duration,
                    vibrato: punchSpec.vibrato,
                    elasticity: punchSpec.elasticity,
                    waitForCompletion: punchSpec.wait
                );
                return command != null;
            }
            
            case ShakeHorizontalEmotionCommandSpec shakeSpec:
            {
                command = new CpsShakeHorizontalCommand(
                    widgets: _widgets,
                    screenId: shakeSpec.screenId,
                    widgetId: shakeSpec.widgetId,
                    strengthX: shakeSpec.strengthX,
                    duration: shakeSpec.duration,
                    vibrato: shakeSpec.vibrato,
                    randomness: shakeSpec.randomness,
                    waitForCompletion: shakeSpec.wait
                );
                return command != null;
            }
            
            case SetEmojiCommandSpec emojiSpec:
            {
                command = new CpsSetEmojiCommand(
                    widgets: _widgets,
                    screenId: emojiSpec.screenId,
                    widgetId: emojiSpec.widgetId,
                    spriteName: emojiSpec.spriteName,
                    clearWhenEmpty: emojiSpec.clearWhenEmpty,
                    waitForCompletion: emojiSpec.wait
                );
                return command != null;
            }
            
            default:
                return false;
        }
    }
}