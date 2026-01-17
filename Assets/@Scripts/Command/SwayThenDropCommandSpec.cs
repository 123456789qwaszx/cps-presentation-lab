    using System;
    using DG.Tweening;
    using UnityEngine;
    using System.Collections;

    [Serializable]
    [CommandMenuHint(
        "Motion",
        "Sway Then Drop",
        // Sets = new[]
        // {
        //     "Custom/Emote/PopEmoji"
        // },
        SetOrder = 30,
        Order = 30)]
    public sealed class SwayThenDropCommandSpec : CommandSpecBase
    {
        [Header("Targets")] [Tooltip("좌우로 흔들릴 피벗(보통 SwayPivot).")]
        public DialogueWidgetTarget swayTarget = DialogueWidgetTarget.Standing00PortraitSwayPivot;

        [Tooltip("아래로 떨어질 트랙/루트(보통 MainStandingPortraitTrack).")]
        public DialogueWidgetTarget dropTarget = DialogueWidgetTarget.Standing00Track;

        [Header("Sway (Fan-like)")]
        /// <summary>아랫변 중앙을 축으로 좌우로 흔들리는 각도 (절대값 기준). 10~25 추천.</summary>
        public float swayAngle = 18f;

        /// <summary>몇 번 왔다갔다 할지 (피크 기준).</summary>
        public int swayLoops = 3;

        /// <summary>전체 스윙에 걸리는 시간.</summary>
        public float swayDuration = 1.2f;

        [Header("Drop")]
        /// <summary>아래로 내려갈 거리 (픽셀).</summary>
        public float dropDistance = 800f;

        /// <summary>떨어지는 데 걸리는 시간.</summary>
        public float dropDuration = 0.24f;

        /// <summary>떨어지면서 추가로 회전할 각도 (시계 방향 기준).</summary>
        public float dropAngle = 0f;

        public Ease dropEase = Ease.InCubic;

        [Header("Behavior")] [Tooltip("체크하면 연출이 끝날 때까지 Step 진행을 멈춥니다.")]
        public bool wait = true;

        [Header("Sway Easing")]
        /// <summary>오른쪽(or 첫 방향)으로 갈 때 이징.</summary>
        public Ease swayForwardEase = Ease.OutQuad;

        /// <summary>true면 시간이 지날수록 진폭이 줄어듦.</summary>
        public bool swayDecay = true;

        [Header("Blend")] [Range(0f, 1f)]
        /// <summary>
        /// 0이면 sway 시작과 동시에 Drop 시작,
        /// 1이면 sway가 끝난 뒤 Drop 시작,
        /// 그 사이 값이면 swayDuration의 중간 어딘가에서 Drop이 시작.
        /// </summary>
        public float dropStartRatio = 0.3f;
    }

    public sealed class SwayThenDropCommand : CommandBase
    {
        private readonly IDialogueWidgetAccess _widgets;
        private readonly string _screenId;
        private readonly string _widgetRoleKey;
        private readonly bool _wait;

        private readonly DialogueWidgetTarget _swayTarget;
        private readonly DialogueWidgetTarget _dropTarget;

        private readonly float _swayAngle;
        private readonly int _swayLoops;
        private readonly float _swayDuration;

        private readonly float _dropDistance;
        private readonly float _dropDuration;
        private readonly Ease _dropEase;

        private readonly Ease _swayForwardEase;
        private readonly bool _swayDecay;

        private readonly float _dropStartRatio;

        private IDialogueWidgetAccess.WidgetRefs _refs;
        private RectTransform _swayRect;
        private RectTransform _dropRect;

        private Vector2 _originDropPos;
        private float _originSwayRotationZ;

        private Vector2 _finalDropPos;

        private bool _resolveAttempted;

        public override bool WaitForCompletion => _wait;
        protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

        public SwayThenDropCommand(
            IDialogueWidgetAccess widgets,
            string screenId,
            string widgetRoleKey,
            bool wait,
            DialogueWidgetTarget swayTarget,
            DialogueWidgetTarget dropTarget,
            float swayAngle,
            int swayLoops,
            float swayDuration,
            float dropDistance,
            float dropDuration,
            Ease dropEase,
            Ease swayForwardEase,
            bool swayDecay,
            float dropStartRatio)
        {
            _widgets = widgets;
            _screenId = screenId;
            _widgetRoleKey = widgetRoleKey;
            _wait = wait;

            _swayTarget = swayTarget;
            _dropTarget = dropTarget;

            _swayAngle = Mathf.Abs(swayAngle);
            _swayLoops = Mathf.Max(0, swayLoops);
            _swayDuration = Mathf.Max(0f, swayDuration);

            _dropDistance = dropDistance;
            _dropDuration = Mathf.Max(0f, dropDuration);
            _dropEase = dropEase;

            _swayForwardEase = swayForwardEase;
            _swayDecay = swayDecay;

            _dropStartRatio = Mathf.Clamp01(dropStartRatio);
        }

        protected override IEnumerator ExecuteInner(CommandRunScope scope)
        {
            if (!ResolveIfNeeded())
                yield break;

            _swayRect.DOKill(false);
            _dropRect.DOKill(false);
            
            _originSwayRotationZ = _swayRect.localEulerAngles.z;
            _originDropPos = _dropRect.anchoredPosition;

            _finalDropPos = _originDropPos + new Vector2(0f, -_dropDistance);

            float swayDur = Mathf.Max(0f, _swayDuration);
            float dropDur = Mathf.Max(0f, _dropDuration);
            float dropDelay = swayDur * Mathf.Clamp01(_dropStartRatio);

            float endSway = swayDur;
            float endDrop = dropDelay + dropDur;
            float totalEnd = Mathf.Max(endSway, endDrop);

            if (totalEnd <= 0f)
            {
                ApplyFinal();
                yield break;
            }
            
            if (dropDur <= 0f)
            {
                _dropRect.anchoredPosition = _finalDropPos;
            }
            else
            {
                _dropRect
                    .DOAnchorPos(_finalDropPos, dropDur)
                    .SetEase(_dropEase)
                    .SetDelay(dropDelay)
                    .SetUpdate(true)
                    .OnComplete(()=> ApplyFinal())
                    .BindToStep(scope);
            }
            
            if (swayDur <= 0f || _swayAngle <= 0f || _swayLoops <= 0)
            {
                SetLocalEulerZ(_swayRect, _originSwayRotationZ);
            }
            else
            {
                DOVirtual.Float(
                        0f,
                        1f,
                        swayDur,
                        ts =>
                        {
                            float phase = ts * Mathf.PI * 2f * _swayLoops;
                            float raw   = Mathf.Sin(phase);

                            float envelope = 1f;
                            if (_swayDecay)
                            {
                                float eased = DOVirtual.EasedValue(0f, 1f, ts, _swayForwardEase);
                                envelope = 1f - eased;
                            }

                            float swayAngle = raw * _swayAngle * envelope;
                            SetLocalEulerZ(_swayRect, _originSwayRotationZ + swayAngle);
                        }
                    )
                    .SetEase(Ease.Linear)
                    .SetUpdate(true)
                    .OnComplete(()=> ApplyFinal())
                    .BindToStep(scope);
            }

            
            if (_wait)
            {
                Tween waitTween = DOVirtual.DelayedCall(totalEnd, () => { }, ignoreTimeScale: true)
                    .SetUpdate(true)
                    .BindToStep(scope);
                
                yield return waitTween.WaitForCompletion();
            }
        }

        protected override void OnSkip(CommandRunScope scope)
        {
            if (!ResolveIfNeeded())
                return;

            _swayRect.DOKill(false);
            _dropRect.DOKill(false);

            _dropRect.anchoredPosition = _finalDropPos;
            SetLocalEulerZ(_swayRect, _originSwayRotationZ);
        }

        private bool ResolveIfNeeded()
        {
            if (_swayRect != null && _dropRect != null)
                return true;

            if (_resolveAttempted)
                return false;

            _resolveAttempted = true;

            if (_widgets == null)
                return false;

            if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null)
                return false;

            _swayRect = _refs.GetRect(_swayTarget);
            _dropRect = _refs.GetRect(_dropTarget);

            if (_swayRect == null || _dropRect == null)
            {
                Debug.LogWarning(
                    $"[SwayThenDropCommand] Rect not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', " +
                    $"sway={_swayTarget}, drop={_dropTarget}");
                return false;
            }

            // 원래 상태 캐시
            _originSwayRotationZ = _swayRect.localEulerAngles.z;
            _originDropPos = _dropRect.anchoredPosition;

            _finalDropPos = _originDropPos + new Vector2(0f, -_dropDistance);

            return true;
        }
        
        void ApplyFinal()
        {
            if (_dropRect != null)
                _dropRect.anchoredPosition = _finalDropPos;

            SetLocalEulerZ(_swayRect, _originSwayRotationZ);
        }

        private static void SetLocalEulerZ(RectTransform rect, float z)
        {
            if (rect == null)
                return;

            Vector3 e = rect.localEulerAngles;
            e.z = z;
            rect.localEulerAngles = e;
        }
    }