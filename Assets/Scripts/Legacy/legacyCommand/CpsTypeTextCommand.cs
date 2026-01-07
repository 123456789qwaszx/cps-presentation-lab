// using System.Collections;
// using TMPro;
// using UnityEngine;
//
// using System;
//
// [Serializable]
// public sealed class TypeBodyTextCommandSpec : CommandSpecBase
// {
//     [Header("Text Source")]
//     [TextArea]
//     public string text;          // 직접 지정 텍스트 (우선 사용)
//
//     public DialogueLine line;    // text가 비어있으면 line.text 사용
//
//     [Header("Typing Settings")]
//     /// <summary>
//     /// <= 0 이면 Config(TypeCharInterval) 기본값 사용
//     /// </summary>
//     public float charInterval = -1f;
//
//     /// <summary>
//     /// true면 이 커맨드가 끝날 때까지 Step 진행을 멈춤
//     /// </summary>
//     public bool wait = true;
// }
// public sealed class CpsTypeTextCommand : CommandBase
// {
//     private readonly IDialogueWidgetAccess _widgets;
//     private readonly string _screenId;
//     private readonly string _widgetId;
//
//     private readonly string _text;
//     private readonly float  _interval;
//     private readonly bool   _wait;
//
//     private IDialogueWidgetAccess.WidgetRefs _refs;
//     private bool _resolved;
//
//     public CpsTypeTextCommand(
//         IDialogueWidgetAccess widgets,
//         string screenId,
//         string widgetId,
//         string text,
//         float interval,
//         bool waitForCompletion = true)
//     {
//         _widgets  = widgets;
//         _screenId = screenId;
//         _widgetId = widgetId;
//
//         _text     = text ?? string.Empty;
//         _interval = Mathf.Max(0f, interval);
//         _wait     = waitForCompletion;
//     }
//
//     public override bool WaitForCompletion => _wait;
//     protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;
//
//     protected override IEnumerator ExecuteInner(CommandRunScope scope)
//     {
//         if (!ResolveIfNeeded())
//             yield break;
//
//         TMP_Text body = _refs.BodyText;
//         if (body == null)
//             yield break;
//
//         // 빈 텍스트면 그냥 바로 비우고 종료
//         if (string.IsNullOrEmpty(_text))
//         {
//             body.text = string.Empty;
//             body.maxVisibleCharacters = int.MaxValue;
//             yield break;
//         }
//
//         // 네가 이미 가지고 있는 TypeTextCommand를 감싸서 사용
//         // 시그니처: TypeTextCommand(TMP_Text text, string content, float interval, bool waitForCompletion)
//         var typeCmd = new TypeTextCommand(
//             body,
//             _text,
//             _interval,
//             waitForCompletion: _wait
//         );
//
//         // 이 안에서 자체적으로 Skip, TimeScale, Auto/Skip모드 등을 처리한다고 가정
//         yield return typeCmd.Execute(scope);
//     }
//
//     protected override void OnSkip(CommandRunScope scope)
//     {
//         if (!ResolveIfNeeded())
//             return;
//
//         TMP_Text body = _refs.BodyText;
//         if (body == null)
//             return;
//
//         // 즉시 최종 상태
//         body.text = _text;
//         body.maxVisibleCharacters = int.MaxValue;
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
//         if (_widgets.TryResolve(_screenId, _widgetId, out var refs) && refs != null)
//         {
//             _refs = refs;
//             return true;
//         }
//
//         return false;
//     }
// }
