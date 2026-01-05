using System;
using UnityEngine;
using DG.Tweening;

[Serializable]
public sealed class PunchScaleEmotionCommandSpec : CommandSpecBase
{
    [Header("Punch Scale Settings")]
    /// <summary>
    /// 현재 스케일 기준으로 얼마나 튀어오를지 (1 + punch 정도 느낌)
    /// </summary>
    public float punch = 0.2f;

    /// <summary>
    /// 연출 총 시간. <= 0이면 아무 것도 안 함.
    /// </summary>
    public float duration = 0.3f;

    /// <summary>
    /// 진동 횟수 (DOTween 기본값 10 정도).
    /// </summary>
    public int vibrato = 10;

    /// <summary>
    /// 탄성 (0~1+).
    /// </summary>
    public float elasticity = 1.0f;

    /// <summary>
    /// true면 이 커맨드가 끝날 때까지 Step 진행을 멈춤.
    /// 기본은 병행 연출이라 false 추천.
    /// </summary>
    public bool wait = false;
}