using System;
using System.Collections.Generic;
using Lab.UI.Keys;
using UnityEngine;
using UnityEngine.InputSystem;

public class CpsUIBootStrap : MonoBehaviour
{
    [Header("Widget Prefabs")]
    public GameObject textPrefab;
    public GameObject buttonPrefab;
    public GameObject imagePrefab;
    public GameObject togglePrefab;
    public GameObject sliderPrefab;
    public GameObject gameObjectPrefab;
    public GameObject slotPrefab;

    [Header("Root")]
    [SerializeField] private Transform uiRoot;
    [SerializeField] private UIScreenCatalog catalog;

    private void Awake()
    {
        if (uiRoot == null) uiRoot = transform;
        if (catalog == null) catalog = FindFirstObjectByType<UIScreenCatalog>();

        catalog.Init();
        
        UISlotBinder              binder  = new();
        UIPatchApplier        patcher = new();
        WidgetRectApplier rectApplier = new();
        RouteKeyResolver routeKeyResolver = new(catalog);
        
        //IHudView hudView = null;
        UIRouter router = null;
        
        // CompositeUiActionBinder uiActionBinder = new (
        //     new UIActionBinder(() => hudView),
        //     new RouteActionBinder(() => router, routeKeyResolver)
        // );
        
        WidgetFactory    widgetFactory = new(textPrefab, buttonPrefab, imagePrefab, togglePrefab, sliderPrefab, gameObjectPrefab,slotPrefab/*, uiActionBinder*/, true);
        UIComposer            composer = new(widgetFactory, rectApplier);
        
        UIContext       context = UIContext.Default;
        UIResolver     resolver = new(catalog, context);
        UIScreenFactory factory = new(uiRoot, binder, patcher, composer);
        
        router = new(resolver, factory, routeKeyResolver);
        
        UIRuntimeRouter.Router = router;
        //hudView   = new HudPresenter(() => router.CurrentScreen);
    }

    public void Update()
    {
        if(UIRuntimeRouter.Router == null) return;
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIRuntimeRouter.Router.Navigate(LabUIActionKeys.OpenDialogue);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UIRuntimeRouter.Router.Navigate(LabUIActionKeys.OpenClickerTitle);
        }
    }
}