using System;
using UnityEngine;

[Flags]
public enum RestoreRectFlags
{
    None        = 0,
    Position    = 1 << 0,
    Rotation    = 1 << 1,
    Scale       = 1 << 2,
    Alpha       = 1 << 3,

    // 선택: 레이아웃까지 되돌리고 싶을 때만 사용
    SizeDelta   = 1 << 4,
    Anchors     = 1 << 5,
    Pivot       = 1 << 6,

    SafeDefault = Position | Rotation | Scale | Alpha,
    All         = Position | Rotation | Scale | Alpha | SizeDelta | Anchors | Pivot,
}


[DisallowMultipleComponent]
public sealed class CpsRectBaseline : MonoBehaviour
{
    [System.Serializable]
    public struct Snapshot
    {
        // RectTransform
        public Vector3    anchoredPosition3D;
        public Quaternion localRotation;
        public Vector3    localScale;

        public Vector2    sizeDelta;
        public Vector2    anchorMin;
        public Vector2    anchorMax;
        public Vector2    pivot;

        // Alpha
        public float alpha;
        public bool  hasAlpha;
    }

    [SerializeField] private bool _captured;
    [SerializeField] private Snapshot _snapshot;

    public bool IsCaptured => _captured;
    public Snapshot Data => _snapshot;

    public void Capture(RectTransform rect, float? alphaOrNull)
    {
        if (rect == null) return;

        _snapshot.anchoredPosition3D = rect.anchoredPosition3D;
        _snapshot.localRotation      = rect.localRotation;
        _snapshot.localScale         = rect.localScale;

        _snapshot.sizeDelta = rect.sizeDelta;
        _snapshot.anchorMin = rect.anchorMin;
        _snapshot.anchorMax = rect.anchorMax;
        _snapshot.pivot     = rect.pivot;

        if (alphaOrNull.HasValue)
        {
            _snapshot.alpha    = Mathf.Clamp01(alphaOrNull.Value);
            _snapshot.hasAlpha = true;
        }
        else
        {
            _snapshot.alpha    = 1f;
            _snapshot.hasAlpha = false;
        }

        _captured = true;
    }

    public void ForceMarkCaptured(Snapshot snapshot)
    {
        _snapshot = snapshot;
        _captured = true;
    }
}