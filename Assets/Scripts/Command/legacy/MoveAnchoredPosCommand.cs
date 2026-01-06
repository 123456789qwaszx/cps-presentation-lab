// using System.Collections;
// using DG.Tweening;
// using UnityEngine;
// using System;
// using DG.Tweening;
// using UnityEngine;
//
// [Serializable]
// public sealed class MovePortraitCommandSpec : CommandSpecBase
// {
//     public Vector2 offset   = new Vector2(150f, 0f); // 목적지 or 오프셋
//     public float  duration  = -1f;                   // <= 0이면 Config 기본값 사용
//     public Ease   ease      = Ease.OutCubic;
//     public bool   wait      = true;
// }
// public sealed class CpsMovePortraitCommand : CommandBase
// {
//     private readonly IDialogueWidgetAccess _widgets;
//     private readonly string _screenId;
//     private readonly string _widgetId;
//
//     private readonly Vector2 _destPos;
//     private readonly float _duration;
//     private readonly Ease _ease;
//     private readonly bool _wait;
//
//     private RectTransform _rectTransform;
//     private bool _resolved;
//
//     public CpsMovePortraitCommand(
//         IDialogueWidgetAccess widgets,
//         string screenId,
//         string widgetId,
//         Vector2 destPos,
//         float duration,
//         Ease ease = Ease.OutCubic,
//         bool wait = true)
//     {
//         _widgets  = widgets;
//         _screenId = screenId;
//         _widgetId = widgetId;
//
//         _destPos   = destPos;
//         _duration  = Mathf.Max(0f, duration);
//         _ease = ease;
//         _wait = wait;
//     }
//
//     public override bool WaitForCompletion => _wait;
//     
//     protected override IEnumerator ExecuteInner(CommandRunScope scope)
//     {
//         ResolveIfNeeded();
//         if (_duration <= 0f)
//         {
//             _rectTransform.anchoredPosition = _destPos;
//             yield break;
//         }
//
//         Tween tween = _rectTransform
//             .DOAnchorPos(_destPos, _duration)
//             .SetEase(_ease)
//             .SetUpdate(true);
//
//         tween.BindToStep(scope);
//
//         if (_wait)
//             yield return tween.WaitForCompletion();
//     }
//
//     protected override void OnSkip(CommandRunScope api)
//     {
//         ResolveIfNeeded();
//
//         _rectTransform.anchoredPosition = _destPos;
//     }
//
//     
//     private void ResolveIfNeeded()
//     {
//         if (_resolved) return;
//         _resolved = true;
//         
//         if (!_widgets.TryResolve(_screenId, _widgetId, out var refs) || refs == null)
//             return;
//
//         _rectTransform = refs.PortraitRect;
//         
//         // _origin = _rt.anchoredPosition;
//     }
// }
