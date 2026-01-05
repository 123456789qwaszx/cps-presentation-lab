using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public sealed class SlidePortraitInCommandSpec : CommandSpecBase
{
    [Header("Slide Settings")]
    /// <summary>
    /// 초상화를 왼쪽으로 얼마나 밀어서 시작할지.
    /// 0이면 Config의 PortraitSlide.offsetX 사용.
    /// </summary>
    public float offsetX = 0f;

    /// <summary>
    /// 슬라이드 연출 시간.
    /// <= 0이면 Config의 PortraitSlide.duration 사용.
    /// </summary>
    public float duration = -1f;

    /// <summary>
    /// 슬라이드 Ease.
    /// </summary>
    public Ease ease = Ease.OutCubic;

    /// <summary>
    /// true면 이 커맨드가 끝날 때까지 Step 진행을 멈춤.
    /// (기본은 병행 연출이라 false 추천)
    /// </summary>
    public bool wait = false;
}