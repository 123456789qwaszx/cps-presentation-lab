using UnityEngine;
using System.Collections;
using DG.Tweening;

[CreateAssetMenu(fileName = "CpsDialogueView", menuName = "Dialogue/DialogueView/CPS")]
public class CpsDialogueViewAsset : DialogueViewAsset
{
    //[SerializeField] private float typeCharInterval = 0.03f;

    public override IEnumerator ShowLine(DialogueLine line, string screenId, string widgetId)
    {
        UIRouter router = UIRuntimeRouter.Router;
        if (router == null)
        {
            Debug.Log("router empty");
            yield break;
        }

        ScreenKey screenKey = new(screenId);
        if (!router.TryGetScreen(screenKey, out UIScreen screen))
            yield break;

        // 1) 본문 텍스트 (타이핑)
        WidgetHandle bodyHandle = screen?.GetWidgetHandle(widgetId+"_Body");
        Tween typing = null;
        if (bodyHandle?.Text != null)
        {
            typing = bodyHandle.Text.DOTypeText(line.text, 0.03f);
        }

        // 2) 스피커 이름
        WidgetHandle nameHandle = screen?.GetWidgetHandle(widgetId+"_Name");
        if (nameHandle?.Text != null)
        {
            string displayName = line.speakerId;
            
            DialogueRepository.Instance.TryGetSpeaker(line.speakerId, out DialogueSpeakerData speakerData);
            displayName = speakerData.displayName;
            
            nameHandle.Text.text = displayName;
        }
        
        // 3) 초상화
        Sprite portrait = DialogueRepository.Instance.GetSpeakerPortrait(line.speakerId, line.expression);
        WidgetHandle portraitHandle = screen?.GetWidgetHandle(widgetId+"_Portrait");
        if (portraitHandle?.Image != null)
        {
            portraitHandle.Image.sprite = portrait;
            portraitHandle.RectTransform.SlideInFromLeftWithFade();
        }
        
        yield break;
    }

    public override void ShowLineImmediate(DialogueLine line, string screenId, string widgetId)
    {
        UIRouter router = UIRuntimeRouter.Router;
        if (router == null)
            return;

        ScreenKey screenKey = new ScreenKey(screenId);
        if (!router.TryGetScreen(screenKey, out UIScreen screen) || screen == null)
            return;

        // 1) 본문 텍스트: 트윈 강제 종료 + 최종 텍스트
        WidgetHandle bodyHandle = screen.GetWidgetHandle(widgetId+"_Body");
        if (bodyHandle?.Text != null)
        {
            DOTween.Kill(bodyHandle.Text);
            bodyHandle.Text.text = line.text;
        }

        // 2) 이름 설정은 ShowLine과 동일하게 한 번 더 보장
        if (!string.IsNullOrEmpty(line.speakerId))
        {
            WidgetHandle nameHandle = screen.GetWidgetHandle(widgetId+"_Name");
            if (nameHandle?.Text != null)
            {
                string displayName = line.speakerId;

                if (DialogueRepository.Instance != null &&
                    DialogueRepository.Instance.TryGetSpeaker(line.speakerId, out DialogueSpeakerData speakerData) &&
                    !string.IsNullOrEmpty(speakerData.displayName))
                {
                    displayName = speakerData.displayName;
                }

                nameHandle.Text.text = displayName;
            }
        }

        // 3) 초상화도 동일하게 맞춰두기
        if (!string.IsNullOrEmpty(line.speakerId) && DialogueRepository.Instance != null)
        {
            Sprite portrait =
                DialogueRepository.Instance.GetSpeakerPortrait(line.speakerId, line.expression);

            WidgetHandle portraitHandle = screen.GetWidgetHandle(widgetId+"_Portrait");
            if (portraitHandle?.Image != null)
            {
                portraitHandle.Image.sprite = portrait;
                portraitHandle.RectTransform.ApplySlideInFinal();
            }
        }
    }
}