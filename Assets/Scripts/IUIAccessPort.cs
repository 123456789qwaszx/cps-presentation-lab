public interface IUIAccessPort
{
    bool TryGetWidget(
        string widgetId,
        out WidgetHandle handle
    );
}