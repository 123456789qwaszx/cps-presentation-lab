using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[Serializable]
public sealed class SetInteractableCommandSpec : CommandSpecBase
{
    public DialogueWidgetTarget target = DialogueWidgetTarget.StandingPortraitImage;

    public bool interactable = true;

    /// <summary>
    /// CanvasGroup/Selectable을 타겟 오브젝트에서 못 찾으면 부모까지 올라가서 찾을지.
    /// (대부분 UI는 루트에 CanvasGroup이 달려있어서 true가 훨씬 편함)
    /// </summary>
    public bool searchParents = true;

    /// <summary>
    /// CanvasGroup을 쓰는 경우 blocksRaycasts도 같이 제어.
    /// </summary>
    public bool blocksRaycasts = true;
}

public sealed class CpsSetInteractableCommand : CommandBase
{
    private readonly IDialogueWidgetAccess _widgets;
    private readonly string _screenId;
    private readonly string _widgetId;

    private readonly DialogueWidgetTarget _target;
    private readonly bool _interactable;
    private readonly bool _searchParents;
    private readonly bool _blocksRaycasts;

    private IDialogueWidgetAccess.WidgetRefs _refs;
    private GameObject _go;
    private bool _resolved;

    public CpsSetInteractableCommand(
        IDialogueWidgetAccess widgets,
        string screenId,
        string widgetId,
        DialogueWidgetTarget target,
        bool interactable,
        bool searchParents = true,
        bool blocksRaycasts = true)
    {
        _widgets = widgets;
        _screenId = screenId;
        _widgetId = widgetId;

        _target = target;
        _interactable = interactable;
        _searchParents = searchParents;
        _blocksRaycasts = blocksRaycasts;
    }

    public override bool WaitForCompletion => false;
    protected override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            yield break;

        Apply();
        yield break;
    }

    protected override void OnSkip(CommandRunScope scope)
    {
        if (!ResolveIfNeeded())
            return;

        Apply();
    }

    private bool ResolveIfNeeded()
    {
        if (_resolved) return _go != null;
        _resolved = true;

        if (_widgets == null)
            return false;

        if (!_widgets.TryResolve(_screenId, _widgetId, out _refs) || _refs == null)
            return false;
        
        _go = _refs.GetComponent(_target)?.gameObject;
        return _go != null;
    }

    private void Apply()
    {
        // 1) CanvasGroup이 있으면 그걸로 일괄 제어하는 게 가장 “정석”
        CanvasGroup cg = FindOnTargetOrParents<CanvasGroup>(_go, _searchParents);
        if (cg != null)
        {
            cg.interactable = _interactable;
            cg.blocksRaycasts = _interactable && _blocksRaycasts;
            return;
        }

        // 2) Selectable이 있으면 interactable로 제어
        Selectable sel = FindOnTargetOrParents<Selectable>(_go, _searchParents);
        if (sel != null)
        {
            sel.interactable = _interactable;
            return;
        }

        // 3) 마지막 fallback: Graphic.raycastTarget
        Graphic g = FindOnTargetOrParents<Graphic>(_go, _searchParents);
        if (g != null)
        {
            g.raycastTarget = _interactable;
        }
    }

    private static T FindOnTargetOrParents<T>(GameObject go, bool searchParents) where T : Component
    {
        if (go == null) return null;
        if (!searchParents) return go.GetComponent<T>();
        return go.GetComponentInParent<T>(includeInactive: true);
    }
}
