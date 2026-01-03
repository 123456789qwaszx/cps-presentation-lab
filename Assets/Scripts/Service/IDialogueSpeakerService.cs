using UnityEngine;

public interface IDialogueSpeakerService
{
    string GetDisplayName(string speakerId);
    Sprite GetPortrait(string speakerId, Expression expression);
}