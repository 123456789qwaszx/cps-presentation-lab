using System;
using UnityEngine;

[Serializable]
public sealed class ShakeHorizontalEmotionCommandSpec : CommandSpecBase
{
    [Header("Shake Settings")]
    /// <summary>
    /// 좌우 진폭 (anchoredPosition 기준).
    /// </summary>
    public float strengthX = 10f;

    /// <summary>
    /// 연출 총 시간. <= 0이면 아무 것도 안 함.
    /// </summary>
    public float duration = 0.2f;

    /// <summary>
    /// 진동 횟수.
    /// </summary>
    public int vibrato = 10;

    /// <summary>
    /// 랜덤성 (0~180 정도).
    /// </summary>
    public float randomness = 90f;

    /// <summary>
    /// true면 이 커맨드가 끝날 때까지 Step 진행을 멈춤.
    /// 기본은 병행 연출이라 false 추천.
    /// </summary>
    public bool wait = false;
}