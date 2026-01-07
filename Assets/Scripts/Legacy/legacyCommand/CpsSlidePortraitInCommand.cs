// using System.Collections;
// using DG.Tweening;
// using UnityEngine;
// using System;
// using DG.Tweening;
// using UnityEngine;
//
// [Serializable]
// public sealed class SlidePortraitInCommandSpec : CommandSpecBase
// {
//     [Header("Slide Settings")]
//     /// <summary>
//     /// 초상화를 왼쪽으로 얼마나 밀어서 시작할지.
//     /// 0이면 Config의 PortraitSlide.offsetX 사용.
//     /// </summary>
//     public float offsetX = 0f;
//
//     /// <summary>
//     /// 슬라이드 연출 시간.
//     /// <= 0이면 Config의 PortraitSlide.duration 사용.
//     /// </summary>
//     public float duration = -1f;
//
//     /// <summary>
//     /// 슬라이드 Ease.
//     /// </summary>
//     public Ease ease = Ease.OutCubic;
//
//     /// <summary>
//     /// true면 이 커맨드가 끝날 때까지 Step 진행을 멈춤.
//     /// (기본은 병행 연출이라 false 추천)
//     /// </summary>
//     public bool wait = false;
// }
// public sealed class CpsSlidePortraitInCommand : CommandBase
// {
//     private readonly IDialogueWidgetAccess _widgets;
//     private readonly string _screenId;
//     private readonly string _widgetId;
//
//     private readonly float _offsetX;
//     private readonly float _duration;
//     private readonly Ease  _ease;
//     private readonly bool  _wait;
//
//     private RectTransform _rect;
//     private Vector2 _destPos;
//     private bool _resolved;
//
//     public CpsSlidePortraitInCommand(
//         IDialogueWidgetAccess widgets,
//         string screenId,
//         string widgetId,
//         float offsetX,
//         float duration,
//         Ease ease = Ease.OutCubic,
//         bool waitForCompletion = false)
//     {
//         _widgets  = widgets;
//         _screenId = screenId;
//         _widgetId = widgetId;
//
//         _offsetX  = Mathf.Max(0f, offsetX);
//         _duration = Mathf.Max(0f, duration);
//         _ease     = ease;
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
//         _rect.DOKill(false);
//
//         // dest = 현재 레이아웃 위치, start = 왼쪽으로 밀린 위치
//         _rect.anchoredPosition = _destPos + new Vector2(-_offsetX, 0f);
//
//         if (_duration <= 0f)
//         {
//             _rect.anchoredPosition = _destPos;
//             yield break;
//         }
//
//         Tween tween = _rect
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
//     protected override void OnSkip(CommandRunScope scope)
//     {
//         if (!ResolveIfNeeded())
//             return;
//
//         _rect.DOKill(false);
//         _rect.anchoredPosition = _destPos;
//     }
//
//     private bool ResolveIfNeeded()
//     {
//         if (_resolved) return _rect != null;
//         _resolved = true;
//
//         if (_widgets == null)
//             return false;
//
//         if (!_widgets.TryResolve(_screenId, _widgetId, out var refs) || refs == null)
//             return false;
//
//         _rect = refs.PortraitRect;
//         if (_rect != null)
//             _destPos = _rect.anchoredPosition;
//
//         return _rect != null;
//     }
// }
