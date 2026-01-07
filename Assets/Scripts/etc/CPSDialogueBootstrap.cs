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
    
    [Header("Starter")]
    private DialogueStarter _dialogueStarter;
    public DialogueStarter DialogueStarter => _dialogueStarter;
    

    private void Awake()
    {
        StepGatePlanBuilder gatePlanner = new ();

        // Compose runner (subscribes to signals)
        UnityInputSource input      = new();
        UnityTimeSource time        = new();
        SignalLatch latch           = new();
        signals.OnSignal += latch.Latch;
        StepGateAdvancer gateRunner = new StepGateAdvancer(input, time, signals, latch);

        // Optional extension ports
        CommandExecutor executor = commandExecuter;
        SequencePlayer sequencePlayer = new(executor);
        
        if (cpsCommandServiceConfig == null)
        {
            Debug.LogError("[DialogueBootstrap] CommandServiceConfig is not assigned.");
            return;
        }
        
        CpsNodeCommandFactory nodeFactory = new (cpsCommandServiceConfig, time,signals, latch);
        executor.Initialize(sequencePlayer, nodeFactory);
        
        PresentationModes playbackModes = new ();
        

        
        PresentationSession session  = new (gatePlanner, gateRunner, commandExecuter, routeCatalog, playbackModes);

        _session = session;
        _gateRunner = gateRunner;

        _dialogueStarter = new DialogueStarter(_session);
    }
    
    
    [SerializeField] private bool enableDebugHotkeys = true;
    
    private void Update()
    {
        if (_session == null) return;

        if (enableDebugHotkeys)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _session.Context.Modes.IsAutoMode = !_session.Context.IsAutoMode;
                Debug.Log($"[Dialogue] AutoMode = {_session.Context.IsAutoMode}");
            }
        
            if (Input.GetKeyDown(KeyCode.K))
            {
                _session.Context.Modes.IsSkipping = !_session.Context.IsSkipping;
                Debug.Log($"[Dialogue] IsSkipping = {_session.Context.IsSkipping}");
            }
        }
        
        _session?.Tick();
    }

    private void OnDestroy()
    {
        _gateRunner?.Dispose();
    }
}