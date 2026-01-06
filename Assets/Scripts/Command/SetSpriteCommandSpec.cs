using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[Serializable]
public sealed class SetSpriteCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public CpsGraphicTarget target = CpsGraphicTarget.Auto;

    [Header("Sprite")]
    public Sprite sprite;

    [Header("Behavior")]
    public bool clearWhenNull = true;
    public bool setNativeSize = false;
}

public sealed class CpsSetSpriteCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly CpsGraphicTarget _target;
    private readonly Sprite _sprite;
    private readonly bool _clearWhenNull;
    private readonly bool _setNativeSize;

    private Image _image;
    private bool _resolved;

    public CpsSetSpriteCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        CpsGraphicTarget target,
        Sprite sprite,
        bool clearWhenNull = true,
        bool setNativeSize = false)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target = target;
        _sprite = sprite;
        _clearWhenNull = clearWhenNull;
        _setNativeSize = setNativeSize;
    }

    public override bool WaitForCompletion => false;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        Apply();
        yield break;
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        Apply();
    }

    private void Apply()
    {
        if (!ResolveIfNeeded())
            return;

        if (_sprite == null && !_clearWhenNull)
            return;

        _image.sprite = _sprite;

        if (_setNativeSize && _sprite != null)
            _image.SetNativeSize();
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _image != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out var refs) || refs == null)
            return false;

        _image = ResolveTargetImage(refs, _target);
        return _image != null;
    }

    private static Image ResolveTargetImage(IDialogueWidgetAccess.WidgetRefs refs, CpsGraphicTarget target)
    {
        if (refs == null) return null;

        // Image 슬롯이 있다면 가장 확실하게 그걸 사용
        switch (target)
        {
            case CpsGraphicTarget.PortraitImage:
                return refs.PortraitImage;

            case CpsGraphicTarget.PortraitGraphic:
            {
                // PortraitGraphic이 Image일 수도 있음
                return refs.PortraitGraphic as Image;
            }

            case CpsGraphicTarget.EmojiImage:
                return refs.EmojiImage;

            case CpsGraphicTarget.Auto:
            default:
                if (refs.PortraitImage != null) return refs.PortraitImage;
                if (refs.PortraitGraphic is Image img) return img;
                if (refs.EmojiImage != null) return refs.EmojiImage;
                return null;
        }
    }
}
