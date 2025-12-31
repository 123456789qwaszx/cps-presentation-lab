public sealed class CpsUIAccessBridge : IUIAccessPort
{
    private readonly UIRouter _router;

    public CpsUIAccessBridge(UIRouter router)
    {
        _router = router;
    }

    public bool TryGetWidget(
        string widgetId,
        out WidgetHandle handle)
    {
        handle = null;

        if (_router == null)
            return false;

        UIScreen screen = _router.CurrentScreen;
        if (screen == null)
            return false;

        handle = screen.GetWidgetHandle(widgetId);
        return handle != null;
    }
}