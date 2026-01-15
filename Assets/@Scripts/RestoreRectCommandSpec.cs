using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

// [Serializable]
// [CommandMenuHint(
//     "Rect",
//     "Restore Rect Baseline",
//     Sets = new[]
//     {
//         CpsCommandMenuSets.VnLayerRestore,
//     },
//     SetOrder = -85
// )]
public sealed class RestoreRectCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.MainStandingPortraitTrack;

    [Header("Restore")]
    public RestoreRectFlags flags = RestoreRectFlags.SafeDefault;

    [Tooltip("Baseline이 없으면 현재 상태를 Baseline으로 1회 캡처합니다.")]
    public bool captureBaselineIfMissing = true;
}


public sealed class RestoreRectCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string                _screenId;
    private readonly string                _widgetRoleKey;

    private readonly DialogueWidgetTarget  _target;
    private readonly RestoreRectFlags      _flags;
    private readonly bool                  _captureBaselineIfMissing;

    private bool _resolveAttempted;
    private IDialogueWidgetAccess.WidgetRefs _refs;
    private RectTransform  _rect;
    private CanvasGroup    _cg;
    private Graphic        _graphic;
    private TMP_Text       _tmp;
    private CpsRectBaseline _baseline;

    public override bool WaitForCompletion => false;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public RestoreRectCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        DialogueWidgetTarget target,
        RestoreRectFlags flags,
        bool captureBaselineIfMissing)
    {
        _widgets                 = widgets;
        _screenId                = screenId;
        _widgetRoleKey           = widgetRoleKey;
        _target                  = target;
        _flags                   = flags;
        _captureBaselineIfMissing = captureBaselineIfMissing;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        // 1) 현재 alpha 읽어오기 (baseline 캡처용)
        float? currentAlpha = ReadAlpha();

        // 2) baseline 확보
        if (!_baseline.IsCaptured)
        {
            if (!_captureBaselineIfMissing)
            {
                Debug.LogWarning(
                    $"[RestoreRectCommand] Baseline not captured and auto-capture is disabled. " +
                    $"screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}",
                    _rect);
                yield break;
            }

            _baseline.Capture(_rect, currentAlpha);
        }

        // 3) restore
        var snap = _baseline.Data;

        if ((_flags & RestoreRectFlags.Position) != 0)
            _rect.anchoredPosition3D = snap.anchoredPosition3D;

        if ((_flags & RestoreRectFlags.Rotation) != 0)
            _rect.localRotation = snap.localRotation;

        if ((_flags & RestoreRectFlags.Scale) != 0)
            _rect.localScale = snap.localScale;

        if ((_flags & RestoreRectFlags.SizeDelta) != 0)
            _rect.sizeDelta = snap.sizeDelta;

        if ((_flags & RestoreRectFlags.Anchors) != 0)
        {
            _rect.anchorMin = snap.anchorMin;
            _rect.anchorMax = snap.anchorMax;
        }

        if ((_flags & RestoreRectFlags.Pivot) != 0)
            _rect.pivot = snap.pivot;

        if ((_flags & RestoreRectFlags.Alpha) != 0 && snap.hasAlpha)
            WriteAlpha(snap.alpha);

        // 즉시 종료
        yield break;
    }

    private bool ResolveIfNeeded()
    {
        if (_resolveAttempted)
            return _refs != null && _rect != null;

        _resolveAttempted = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null)
            return false;

        // 네가 이미 쓰고 있는 확장 메서드 패턴 가정:
        // public static Component GetComponent(this WidgetRefs refs, DialogueWidgetTarget slot)
        Component c = _refs.GetComponent(_target);
        if (c == null)
        {
            Debug.LogWarning(
                $"[RestoreRectCommand] Widget target not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', target={_target}");
            return false;
        }

        _rect = c as RectTransform ?? c.GetComponent<RectTransform>();
        if (_rect == null)
        {
            Debug.LogWarning(
                $"[RestoreRectCommand] Target has no RectTransform. screen='{_screenId}', roleKey='{_widgetRoleKey}', comp={c.GetType().Name}",
                c);
            return false;
        }

        _cg      = _rect.GetComponent<CanvasGroup>();
        _graphic = _rect.GetComponent<Graphic>();
        _tmp     = _rect.GetComponent<TMP_Text>();

        _baseline = _rect.GetComponent<CpsRectBaseline>();
        if (_baseline == null)
            _baseline = _rect.gameObject.AddComponent<CpsRectBaseline>();

        return true;
    }

    private float? ReadAlpha()
    {
        if (_cg != null)
            return _cg.alpha;

        if (_graphic != null)
            return _graphic.color.a;

        if (_tmp != null)
            return _tmp.color.a;

        return null;
    }

    private void WriteAlpha(float a)
    {
        a = Mathf.Clamp01(a);

        if (_cg != null)
        {
            _cg.alpha = a;
            return;
        }

        if (_graphic != null)
        {
            var col = _graphic.color;
            col.a   = a;
            _graphic.color = col;
            return;
        }

        if (_tmp != null)
        {
            var col = _tmp.color;
            col.a   = a;
            _tmp.color = col;
        }
    }
}