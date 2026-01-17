using System;
using UnityEngine;
using UnityEngine.UI;
using IEnumerator = System.Collections.IEnumerator;

[Serializable]
[CommandMenuHint("Visual", "Set Color", Order = 30)]
public sealed class SetColorCommandSpec : CommandSpecBase
{
    [Header("Target")]
    public DialogueSpriteTarget spriteTarget = DialogueSpriteTarget.None;

    [Header("Color")]
    public Color color = Color.white;

    [Tooltip("체크하면 현재 알파(A)는 그대로 두고 색상(RGB)만 변경합니다.")]
    public bool keepAlpha = true;
}

public sealed class SetColorCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetRoleKey;

    private readonly DialogueSpriteTarget _spriteTarget;
    private readonly Color _color;
    private readonly bool _keepAlpha;
    
    private IDialogueWidgetAccess.WidgetRefs _refs;
    private Graphic _graphic;
    private bool _resolveAttempted;

    public override bool WaitForCompletion => false;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    public SetColorCommand(
        IDialogueWidgetAccess widgets,
        string                screenId,
        string                widgetRoleKey,
        DialogueSpriteTarget  spriteTarget,
        Color                 color,
        bool                  keepAlpha)
    {
        _widgets       = widgets;
        _screenId      = screenId;
        _widgetRoleKey = widgetRoleKey;

        _spriteTarget  = spriteTarget;
        _color         = color;
        _keepAlpha = keepAlpha;
    }

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        Apply();
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        Apply();
    }

    private void Apply()
    {
        Color color = _color;

        if (_keepAlpha)
        {
            Color curAlpha = _graphic.color;
            color.a = curAlpha.a;
        }

        _graphic.color = color;
    }

    private bool ResolveIfNeeded()
    {
        if (_resolveAttempted)
            return _graphic != null;

        _resolveAttempted = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetRoleKey, out _refs) || _refs == null)
            return false;

        if (!DialogueSpriteTargetMap.TryResolve(_spriteTarget, out DialogueWidgetTarget widgetTarget))
        {
            Debug.LogWarning(
                $"[SetColorCommand] Unsupported sprite target={_spriteTarget}. screen='{_screenId}', roleKey='{_widgetRoleKey}'");
            return false;
        }

        _graphic = _refs.GetGraphic(widgetTarget);
        if (_graphic == null)
        {
            Debug.LogWarning(
                $"[SetColorCommand] Graphic not found. screen='{_screenId}', roleKey='{_widgetRoleKey}', spriteTarget={_spriteTarget}, widgetTarget={widgetTarget}");
            return false;
        }

        return true;
    }
}