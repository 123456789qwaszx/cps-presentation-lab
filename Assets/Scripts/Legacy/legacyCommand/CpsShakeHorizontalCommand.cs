// using System.Collections;
// using DG.Tweening;
// using UnityEngine;
// using System;
// using UnityEngine;
//
// [Serializable]
// public sealed class ShakeHorizontalEmotionCommandSpec : CommandSpecBase
// {
//     [Header("Shake Settings")]
//     /// <summary>
//     /// 좌우 진폭 (anchoredPosition 기준).
//     /// </summary>
//     public float strengthX = 10f;
//
//     /// <summary>
//     /// 연출 총 시간. <= 0이면 아무 것도 안 함.
//     /// </summary>
//     public float duration = 0.8f;
//
//     /// <summary>
//     /// 진동 횟수.
//     /// </summary>
//     public int vibrato = 10;
//
//     /// <summary>
//     /// 랜덤성 (0~180 정도).
//     /// </summary>
//     public float randomness = 90f;
//
//     /// <summary>
//     /// true면 이 커맨드가 끝날 때까지 Step 진행을 멈춤.
//     /// 기본은 병행 연출이라 false 추천.
//     /// </summary>
//     public bool wait = false;
// }
// public sealed class CpsShakeHorizontalCommand : CommandBase
// {
//     private readonly IDialogueWidgetAccess _widgets;
//     private readonly string _screenId;
//     private readonly string _widgetId;
//
//     private readonly float _strengthX;
//     private readonly float _duration;
//     private readonly int   _vibrato;
//     private readonly float _randomness;
//     private readonly bool  _wait;
//
//     private RectTransform _rect;
//     private Vector2 _originPos;
//     private bool _resolved;
//
//     public CpsShakeHorizontalCommand(
//         IDialogueWidgetAccess widgets,
//         string screenId,
//         string widgetId,
//         float strengthX,
//         float duration,
//         int vibrato = 10,
//         float randomness = 90f,
//         bool waitForCompletion = false)
//     {
//         _widgets    = widgets;
//         _screenId   = screenId;
//         _widgetId   = widgetId;
//
//         _strengthX  = strengthX;
//         _duration   = Mathf.Max(0f, duration);
//         _vibrato    = Mathf.Max(1, vibrato);
//         _randomness = Mathf.Max(0f, randomness);
//         _wait       = waitForCompletion;
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
//         if (_duration <= 0f || Mathf.Approximately(_strengthX, 0f))
//             yield break;
//
//         Tween tween = _rect
//             .DOShakeAnchorPos(
//                 duration: _duration,
//                 strength: new Vector2(_strengthX, 0f),
//                 vibrato: _vibrato,
//                 randomness: _randomness,
//                 snapping: false,
//                 fadeOut: true
//             )
//             .SetUpdate(true);
//         tween.OnKill(() => Debug.Log("[Tween] Killed"));
//         tween.OnComplete(() => Debug.Log("[Tween] Completed"));
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
//         _rect.anchoredPosition = _originPos;
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
//         // ⭐ EmoteRect 우선, 없으면 PortraitRect
//         _rect = refs.EmoteRect != null ? refs.EmoteRect : refs.PortraitRect;
//
//         if (_rect != null)
//             _originPos = _rect.anchoredPosition;
//
//         return _rect != null;
//     }
// }
