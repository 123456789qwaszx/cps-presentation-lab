using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "CpsDialogueView", menuName = "Dialogue/DialogueView/CPS")]
public class CpsDialogueViewAsset : DialogueViewAsset
{
    ScreenKey screenKey;
    public override IEnumerator ShowLine(DialogueLine line, string screenId, string widgetId)
    {
        UIRouter router = UIRuntimeRouter.Router;
        if (router == null)
            yield break;

        ScreenKey screenKey = new (screenId);

        if (!router.TryGetScreen(screenKey, out UIScreen screen) || screen == null)
            yield break;

        
        WidgetHandle handle = screen.GetWidgetHandle(widgetId);
        if (handle?.Text != null)
        {
            handle.Text.text = line.text;
        }

        // 나중에 여기서 Lab 호출, 타이핑, 카메라 연출 등 마음껏 추가 가능
        yield break;
    }

    public override void ShowLineImmediate(DialogueLine line, string screenId, string widgetId)
    {
        var router = UIRuntimeRouter.Router;
        if (router == null)
            return;

        var screenKey = new ScreenKey(screenId);

        if (!router.TryGetScreen(screenKey, out UIScreen screen) || screen == null)
            return;

        WidgetHandle handle = screen.GetWidgetHandle(widgetId);
        if (handle?.Text != null)
        {
            handle.Text.text = line.text;
        }
    }
}