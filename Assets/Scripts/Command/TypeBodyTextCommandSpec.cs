using System;
using UnityEngine;

[Serializable]
public sealed class TypeBodyTextCommandSpec : CommandSpecBase
{
    [Header("Text Source")]
    [TextArea]
    public string text;          // 직접 지정 텍스트 (우선 사용)

    public DialogueLine line;    // text가 비어있으면 line.text 사용

    [Header("Typing Settings")]
    /// <summary>
    /// <= 0 이면 Config(TypeCharInterval) 기본값 사용
    /// </summary>
    public float charInterval = -1f;

    /// <summary>
    /// true면 이 커맨드가 끝날 때까지 Step 진행을 멈춤
    /// </summary>
    public bool wait = true;
}