using UnityEngine;
using Lab.UI.Keys;

public class TestHelper : MonoBehaviour
{
    private UIBootStrap _bootstrap;
    private UIOpener _uiOpener;
    
    void Start()
    {
        if (_bootstrap == null)
            _bootstrap = FindFirstObjectByType<UIBootStrap>();
        
        _uiOpener = _bootstrap.Opener;
    }
    
    public void OnOpenTitle()
    {
        _uiOpener.Open(LabUIActionKeys.OpenClickerTitle);
    }
}
