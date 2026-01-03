using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSpeakerService", menuName = "Dialogue/Services/Speaker Service")]
public sealed class DialogueSpeakerServiceAsset : ScriptableObject, IDialogueSpeakerService
{
    public string GetDisplayName(string speakerId)
    {
        string display = speakerId ?? "";
        if (DialogueRepository.Instance != null &&
            DialogueRepository.Instance.TryGetSpeaker(speakerId, out DialogueSpeakerData data) &&
            !string.IsNullOrEmpty(data.displayName))
        {
            display = data.displayName;
        }
        return display;
    }

    public Sprite GetPortrait(string speakerId, Expression expression)
    {
        if (DialogueRepository.Instance == null) return null;
        return DialogueRepository.Instance.GetSpeakerPortrait(speakerId, expression);
    }
}