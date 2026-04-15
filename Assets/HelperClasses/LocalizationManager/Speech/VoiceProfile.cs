using UnityEngine;

[CreateAssetMenu(fileName = "VoiceProfile", menuName = "TTS/Voice Profile")]
public class VoiceProfile : ScriptableObject
{
    public Language language;
    [Range(0.5f, 2f)] public float speechRate = 1f;
    [Range(0.5f, 2f)] public float pitch = 1f;
}