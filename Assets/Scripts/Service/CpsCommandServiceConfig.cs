using UnityEngine;

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

    [Header("Portrait Slide In (optional)")]
    [SerializeField] private bool enablePortraitSlideIn = true;
    [SerializeField] private float portraitSlideDuration = 0.5f;
    [SerializeField] private float portraitSlideOffsetX = 800f;
    [SerializeField] private float portraitFadeDuration = 0.25f;

    public IDialogueWidgetAccess WidgetAccess => widgetAccessAsset;
    public IDialogueSpeakerService SpeakerService => speakerServiceAsset;

    public float TypeCharInterval => typeCharInterval;

    public bool EnablePortraitSlideIn => enablePortraitSlideIn;
    public float PortraitSlideDuration => portraitSlideDuration;
    public float PortraitSlideOffsetX => portraitSlideOffsetX;
    public float PortraitFadeDuration => portraitFadeDuration;
}