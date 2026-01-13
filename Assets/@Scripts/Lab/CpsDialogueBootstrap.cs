using Lab.UI.Keys;
using UnityEngine;

public sealed class CpsDialogueBootstrap : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private RouteCatalogSO routeCatalog;
    [SerializeField] private CpsCommandServiceConfig cpsCommandServiceConfig;

    [Header("Ports / Adapters")]
    [SerializeField] private CommandExecutor commandExecuter;

    [SerializeField] private UnitySignalBus signals;

    [Header("Session / Runner")]
    private PresentationSession _session;
    private StepGateAdvancer _gateRunner;


    private void Awake()
    {
        StepGatePlanBuilder gatePlanner = new();

        // Compose runner (subscribes to signals)
        UnityInputSource input = new();
        UnityTimeSource time = new();
        SignalLatch latch = new();
        signals.OnSignal += latch.Latch;
        StepGateAdvancer gateAdvancer = new StepGateAdvancer(input, time, signals, latch);

        // Optional extension ports
        CommandExecutor executor = commandExecuter;
        SequencePlayer sequencePlayer = new(executor);

        if (cpsCommandServiceConfig == null)
        {
            Debug.LogError("[DialogueBootstrap] CommandServiceConfig is not assigned.");
            return;
        }

        CpsNodeCommandFactory nodeFactory = new(cpsCommandServiceConfig, time, signals, latch);
        executor.Initialize(sequencePlayer, nodeFactory);

        PlaybackSettings modes = new();
        PresentationSession session = new(gatePlanner, gateAdvancer, commandExecuter, modes);

        _session = session;
        _gateRunner = gateAdvancer;
    }


    [SerializeField] private bool enableDebugHotkeys = true;
    [SerializeField] private string testRouteKey;

    private void Update()
    {
        if (_session == null) return;

        if (enableDebugHotkeys)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                UIRuntimeRouter.Router.Navigate(LabUIActionKeys.OpenDialogue_02);
                PlayRoute(testRouteKey);
                Debug.Log($"Start Session = {testRouteKey}");
            }
        }

        _session?.Tick();
    }

    private void OnDestroy()
    {
        _gateRunner?.Dispose();
    }
    
    
    public void PlayRoute(string routeKey)
    {
        if (routeCatalog == null)
        {
            Debug.LogError("[PresentationRoutePlayer] RouteCatalog is not assigned.");
            return;
        }

        if (!routeCatalog.TryResolve(routeKey, out Route route, out SequenceSpecSO sequence))
        {
            Debug.LogWarning($"[PresentationRoutePlayer] Failed to resolve routeKey='{routeKey}'");
            return;
        }
        
        if (_session == null)
        {
            Debug.LogWarning("[PresentationEntryPoint] Session is null. Call StartSession() before PlayRoute.");
            return;
        }

        _session.Start(route, sequence);
    }
}