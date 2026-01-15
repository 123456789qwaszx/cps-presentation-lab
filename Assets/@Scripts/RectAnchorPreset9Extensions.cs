using UnityEngine;

public enum RectAnchorPreset9
{
    Center,         // 정중앙

    Top,            // 12시
    Bottom,         // 6시
    Left,           // 9시
    Right,          // 3시

    TopLeft,        // 11시
    TopRight,       // 1시
    BottomLeft,     // 7시
    BottomRight     // 5시
}

public static class RectAnchorPreset9Extensions
{
    /// <summary>
    /// 유니티 RectTransform 인스펙터의 3x3 그리드 느낌으로
    /// anchorMin/Max, pivot을 한 번에 세팅.
    /// </summary>
    public static void ApplyTo(this RectAnchorPreset9 preset, RectTransform rect)
    {
        if (rect == null) return;

        Vector2 anchor;
        switch (preset)
        {
            case RectAnchorPreset9.Center:
                anchor = new Vector2(0.5f, 0.5f);
                break;

            case RectAnchorPreset9.Top:
                anchor = new Vector2(0.5f, 1f);
                break;
            case RectAnchorPreset9.Bottom:
                anchor = new Vector2(0.5f, 0f);
                break;
            case RectAnchorPreset9.Left:
                anchor = new Vector2(0f, 0.5f);
                break;
            case RectAnchorPreset9.Right:
                anchor = new Vector2(1f, 0.5f);
                break;

            case RectAnchorPreset9.TopLeft:
                anchor = new Vector2(0f, 1f);
                break;
            case RectAnchorPreset9.TopRight:
                anchor = new Vector2(1f, 1f);
                break;
            case RectAnchorPreset9.BottomLeft:
                anchor = new Vector2(0f, 0f);
                break;
            case RectAnchorPreset9.BottomRight:
                anchor = new Vector2(1f, 0f);
                break;

            default:
                anchor = new Vector2(0.5f, 0.5f);
                break;
        }

        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot     = anchor;   // 유니티 기본 프리셋처럼 pivot도 같이 맞춰줌
    }
}