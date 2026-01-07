// using System.Collections;
// using UnityEngine;
// using UnityEngine.UI;
//
// public sealed class CpsSetEmojiCommand : CommandBase
// {
//     private readonly IDialogueWidgetAccess _widgets;
//     private readonly string _screenId;
//     private readonly string _widgetId;
//
//     private readonly string _spriteName;
//     private readonly bool   _clearWhenEmpty;
//     private readonly bool   _wait;
//
//     private IDialogueWidgetAccess.WidgetRefs _refs;
//     private bool _resolved;
//
//     public CpsSetEmojiCommand(
//         IDialogueWidgetAccess widgets,
//         string screenId,
//         string widgetId,
//         string spriteName,
//         bool clearWhenEmpty,
//         bool waitForCompletion = false)
//     {
//         _widgets        = widgets;
//         _screenId       = screenId;
//         _widgetId       = widgetId;
//         _spriteName     = spriteName ?? string.Empty;
//         _clearWhenEmpty = clearWhenEmpty;
//         _wait           = waitForCompletion;
//     }
//
//     public override bool WaitForCompletion => _wait;
//     protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;
//
//     protected override IEnumerator ExecuteInner(CommandRunScope scope)
//     {
//         ApplyEmoji();
//         yield break;
//     }
//
//     protected override void OnSkip(CommandRunScope scope)
//     {
//         // 이 커맨드는 즉시 상태 세팅이라 Skip에서도 동일 로직
//         ApplyEmoji();
//     }
//
//     private void ApplyEmoji()
//     {
//         if (!ResolveIfNeeded())
//             return;
//
//         // ⭐ EmoteImage 우선, 없으면 PortraitImage로 폴백
//         Image target = _refs.EmoteImage != null
//             ? _refs.EmoteImage
//             : _refs.PortraitImage;
//
//         if (target == null)
//             return;
//
//         if (string.IsNullOrEmpty(_spriteName))
//         {
//             if (_clearWhenEmpty)
//             {
//                 target.sprite  = null;
//                 target.enabled = false;
//             }
//             return;
//         }
//
//         // 현재는 Mutsuki 전용 로더 사용
//         Sprite sprite = EmoteAtlasLoader.GetMutsukiEmote(_spriteName);
//         if (sprite == null)
//         {
//             // Atlas나 스프라이트가 없으면 그냥 변경 안 함
//             return;
//         }
//
//         target.sprite  = sprite;
//         target.enabled = true;
//     }
//
//     private bool ResolveIfNeeded()
//     {
//         if (_resolved) return _refs != null;
//         _resolved = true;
//
//         if (_widgets == null)
//             return false;
//
//         if (!_widgets.TryResolve(_screenId, _widgetId, out var refs) || refs == null)
//             return false;
//
//         _refs = refs;
//         return true;
//     }
// }
