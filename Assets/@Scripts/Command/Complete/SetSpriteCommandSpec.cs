using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CommandMenuHint(
    "Visual",
    "Set Sprite (Image)"
    // ,
    // Sets = new[]
    // {
    //     CpsCommandMenuSets.VnMainEnterFirstLine,
    // },
    // SetOrder = 10
    )]
public sealed class SetSpriteCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueSpriteTarget spriteTarget = DialogueSpriteTarget.MainPortrait;

    [Header("Sprite")]
    public Sprite sprite;

    [Tooltip("true면 새 스프라이트의 원본 크기로 맞춘다.")]
    public bool setNativeSize = false;
}

public sealed class SetSpriteCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;

    private readonly DialogueSpriteTarget _spriteTarget;
    private readonly Sprite _sprite;
    private readonly bool _setNativeSize;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private Image _image;
    private bool _resolveAttempted;

    public override bool WaitForCompletion => false;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public SetSpriteCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetRoleKey,
        DialogueSpriteTarget spriteTarget,
        Sprite sprite,
        bool setNativeSize)
    {
        _widgets        = widgets;
        _screenId       = screenId;
        _widgetRoleKey  = widgetRoleKey;

        _spriteTarget   = spriteTarget;
        _sprite         = sprite;
        _setNativeSize  = setNativeSize;
    }

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

        if (_sprite == null)
        {
            Debug.LogWarning(
                $"[SetSpriteCommand] sprite is null for target '{_spriteTarget}' " +
                $"on screen='{_screenId}', role='{_widgetRoleKey}'. Skipped.");
            return;
        }

        _image.sprite = _sprite;

        if (_setNativeSize)
            _image.SetNativeSize();
    }

    private bool ResolveIfNeeded()
    {
        if (_resolveAttempted)
            return _image != null;

        _resolveAttempted = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null)
            return false;

#if UNITY_EDITOR
        if (!Enum.IsDefined(typeof(DialogueSpriteTarget), _spriteTarget))
        {
            Debug.LogWarning(
                $"[SetSpriteCommand] Invalid {nameof(DialogueSpriteTarget)} value in command data: {_spriteTarget}. " +
                $"screenId='{_screenId}', widgetRoleKey='{_widgetRoleKey}'");
            return false;
        }
#endif

        if (!DialogueSpriteTargetMap.TryResolve(_spriteTarget, out DialogueWidgetTarget widgetTarget))
        {
            Debug.LogWarning(
                $"[SetSpriteCommand] Sprite target '{_spriteTarget}' has no valid DialogueWidgetTarget mapping. " +
                $"screenId='{_screenId}', widgetRoleKey='{_widgetRoleKey}'");
            return false;
        }

        Graphic g = _refs.GetGraphic(widgetTarget);
        _image = g as Image;

        if (_image == null)
        {
            if (g != null)
            {
                Debug.LogWarning(
                    $"[SetSpriteCommand] Target '{widgetTarget}' for '{_spriteTarget}' is not an Image " +
                    $"(found {g.GetType().Name}) on screen='{_screenId}', role='{_widgetRoleKey}'.",
                    g);
            }
            else
            {
                Debug.LogWarning(
                    $"[SetSpriteCommand] No Graphic found for widgetTarget '{widgetTarget}' " +
                    $"(spriteTarget='{_spriteTarget}') on screen='{_screenId}', role='{_widgetRoleKey}'.");
            }

            return false;
        }

        return true;
    }
}