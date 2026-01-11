using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[Serializable]
[CommandMenuHint(
    "Visual/Portrait",
    "Set Portrait Sprite",
    Sets = new[]
    {
        "Custom/Portrait/EnterMain",
        "Custom/Portrait/ChangeEmotion",
        "Custom/Emote/PopEmoji",         // 이모지용으로도 재사용
        "Custom/Background/FadeToScene"  // 배경에도 재사용 (원하면 분리 가능)
    },
    SetOrder = 10,
    Order = 10)]
public sealed class SetSpriteCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueWidgetTarget target = DialogueWidgetTarget.StandingPortraitImage;

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

    private readonly DialogueWidgetTarget _target;
    private readonly Sprite _sprite;
    private readonly bool _clearWhenNull;
    private readonly bool _setNativeSize;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private Image _image;
    private bool _resolved;

    public CpsSetSpriteCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
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

        if (!_widgets.TryResolve(_screenId, _widgetId, out _refs) || _refs == null)
            return false;
        
        Graphic g = _refs.GetGraphic(_target);
        _image = g as Image;
        return _image != null;
    }
}
