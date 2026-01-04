using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(
    fileName = "CpsCommandServiceConfig",
    menuName = "Dialogue/Command Service Config/CPS")]
public sealed class CpsCommandServiceConfig : ScriptableObject
{
    [Header("SO 어댑터 Drag & Drop")]
    [SerializeField] private CpsDialogueWidgetAccessAsset widgetAccessAsset;
    [SerializeField] private DialogueSpeakerServiceAsset speakerServiceAsset;

    [Header("Defaults")]
    [SerializeField] private float typeCharInterval = 0.03f;

    [Header("Portrait Slide In")]
    [SerializeField] private PortraitSlideSettings portraitSlide = new();

    [Header("Move Portrait")]
    [SerializeField] private MovePortraitSettings movePortrait = new();

    public IDialogueWidgetAccess WidgetAccess => widgetAccessAsset;
    public IDialogueSpeakerService SpeakerService => speakerServiceAsset;

    public float TypeCharInterval => typeCharInterval;

    public PortraitSlideSettings PortraitSlide => portraitSlide;
    public MovePortraitSettings MovePortrait => movePortrait;
}

[System.Serializable]
public sealed class PortraitSlideSettings
{
    public bool  enable     = true;
    public float duration   = 0.5f;
    public float offsetX    = 800f;
    public float fadeDur    = 0.25f;
}

[System.Serializable]
public sealed class MovePortraitSettings
{
    [Tooltip("대사 중 포트레이트를 살짝 움직일 기본 오프셋")]
    public Vector2 defaultOffset = new Vector2(150f, 0f);

    [Tooltip("기본 이동 시간")]
    public float duration = 0.25f;

    [Tooltip("기본 Ease")]
    public Ease ease = Ease.OutCubic;

    [Tooltip("기본적으로 이 커맨드가 끝날 때까지 기다릴지 여부")]
    public bool wait = true;
}