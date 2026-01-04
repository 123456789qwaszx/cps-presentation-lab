using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public sealed class MovePortraitCommandSpec : CommandSpecBase
{
    public Vector2 offset   = new Vector2(150f, 0f); // 목적지 or 오프셋
    public float  duration  = -1f;                   // <= 0이면 Config 기본값 사용
    public Ease   ease      = Ease.OutCubic;
    public bool   wait      = true;
}