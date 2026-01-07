// using System;
// using System.Collections;
// using UnityEngine;
//
// [Serializable]
// public sealed class SetSpeakerNameCommandSpec : CommandSpecBase
// {
//     [Header("Speaker Source")]
//     public DialogueLine line;
//     
//     // 필요해지면 확장 옵션 추가 가능:
//     // public string overrideSpeakerId;
//     // public bool  useOverrideSpeakerId;
// }
// public sealed class CpsSetSpeakerNameCommand : CommandBase
// {
//     private readonly IDialogueWidgetAccess _widgets;
//     private readonly IDialogueSpeakerService _speakers;
//     private readonly DialogueLine _line;
//     private readonly string _screenId;
//     private readonly string _widgetId;
//
//     // 이 커맨드는 한 프레임 안에 끝나는 “즉시 커맨드”
//     public override bool WaitForCompletion => false;
//     protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;
//
//     private IDialogueWidgetAccess.WidgetRefs _refs;
//     private bool _resolved;
//
//     public CpsSetSpeakerNameCommand(
//         IDialogueWidgetAccess widgets,
//         IDialogueSpeakerService speakers,
//         DialogueLine line,
//         string screenId,
//         string widgetId)
//     {
//         _widgets  = widgets;
//         _speakers = speakers;
//         _line     = line;
//         _screenId = screenId;
//         _widgetId = widgetId;
//     }
//
//     protected override IEnumerator ExecuteInner(CommandRunScope scope)
//     {
//         ResolveIfNeeded();
//         ApplyName();
//         yield break;
//     }
//
//     protected override void OnSkip(CommandRunScope scope)
//     {
//         ResolveIfNeeded();
//         ApplyName();
//     }
//
//     private void ResolveIfNeeded()
//     {
//         if (_resolved) return;
//         _resolved = true;
//
//         if (_widgets == null) return;
//         if (_widgets.TryResolve(_screenId, _widgetId, out var refs))
//             _refs = refs;
//     }
//
//     private void ApplyName()
//     {
//         if (_refs?.NameText == null) return;
//
//         string speakerId = _line?.speakerId ?? "";
//         string displayName = _speakers != null
//             ? _speakers.GetDisplayName(speakerId)
//             : speakerId;
//
//         _refs.NameText.text = displayName;
//     }
// }