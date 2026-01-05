using System;
using UnityEngine;

[Serializable]
public sealed class SetEmojiCommandSpec : CommandSpecBase
{
    [Header("Emoji Settings")]
    [Tooltip("EmoteAtlas 내의 스프라이트 이름 (예: Mutsuki_Emote_Baffled)")]
    public string spriteName;

    [Tooltip("spriteName이 비어있을 때, 기존 이모지를 지울지 여부")]
    public bool clearWhenEmpty = true;

    [Tooltip("이 커맨드가 끝날 때까지 Step 진행을 멈출지 여부 (보통 false)")]
    public bool wait = false;
}