using System.Collections;

public class LabShowLineCommand : CommandBase
{
    private readonly IDialogueViewService _presentation;
    private readonly DialogueLine _line;
    private readonly string _screedId;
    private readonly string _widgetId;

    public LabShowLineCommand(IDialogueViewService presentation, DialogueLine line, string screedId, string widgetId)
    {
      _presentation = presentation;
      _line = line;
      _screedId = screedId;
      _widgetId = widgetId;
    }

    public override SkipPolicy SkipPolicy => SkipPolicy.CompleteImmediately;

    protected override IEnumerator ExecuteInner()
    {
        IEnumerator routine = _presentation.ShowLine(_line, _screedId, _widgetId);
        
        if (routine != null)
            yield return routine;
    }
    
    
    protected override void OnSkip(NodePlayScope api)
    {
        _presentation.ShowLineImmediate(_line, _screedId, _widgetId);
    }
}
