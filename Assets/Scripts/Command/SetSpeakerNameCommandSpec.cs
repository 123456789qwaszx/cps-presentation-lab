using System;
using UnityEngine;

[Serializable]
public sealed class SetSpeakerNameCommandSpec : CommandSpecBase
{
    [Header("Speaker Source")]
    public DialogueLine line;
    
    // 필요해지면 확장 옵션 추가 가능:
    // public string overrideSpeakerId;
    // public bool  useOverrideSpeakerId;
}

[Serializable]
public sealed class SetPortraitSpriteCommandSpec : CommandSpecBase
{
    [Header("Speaker / Expression Source")]
    public DialogueLine line;
    
    // 역시 필요하면 나중에 override 옵션을 추가해도 됨:
    // public string overrideSpeakerId;
    // public DialogueExpression overrideExpression;
    // public bool   useOverride;
}

[Serializable]
public sealed class FadePortraitGraphicCommandSpec : CommandSpecBase
{
    [Header("Fade Settings")]
    [Range(0f, 1f)]
    public float fromAlpha = 0f;

    [Range(0f, 1f)]
    public float toAlpha = 1f;

    /// <summary>
    /// <= 0 이면 Config 기본값 사용 (예: PortraitSlideSettings의 fadeDur 같은 것)
    /// </summary>
    public float duration = -1f;

    public bool wait = false;
}