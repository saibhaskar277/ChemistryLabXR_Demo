using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VoiceDatabase", menuName = "TTS/Voice Database")]
public class VoiceDatabaseSO : ScriptableObject
{
    public List<VoiceProfile> voices = new();

    public VoiceProfile Get(Language language)
    {
        return voices.Find(x => x.language == language);
    }
}