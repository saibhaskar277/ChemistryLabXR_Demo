using UnityEngine;
using System.Diagnostics;

public class Speech : MonoBehaviour
{
    [SerializeField] private VoiceDatabaseSO voiceDatabase;

    private AndroidJavaObject tts;
    private bool isInitialized;
    private VoiceProfile currentProfile;

#if UNITY_EDITOR_WIN
    private Process editorSpeechProcess;
#endif

    private void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        using var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        tts = new AndroidJavaObject(
            "android.speech.tts.TextToSpeech",
            activity,
            new TTSInitListener(() =>
            {
                isInitialized = true;
                ApplyVoice(LocalizationManager.Instance.CurrentLanguage);
            })
        );
#endif
    }

    private void OnEnable()
    {
        EventManager.ListenEvent<OnSpeechPlayEvent>(PlaySpeech);
        EventManager.ListenEvent<OnSpeechStopEvent>(StopSpeech);
        EventManager.ListenEvent<OnSpeechLocalizationKeyEvent>(PlayLocalizedSpeech);
    }

    private void OnDisable()
    {
        EventManager.StopListening<OnSpeechPlayEvent>(PlaySpeech);
        EventManager.StopListening<OnSpeechStopEvent>(StopSpeech);
        EventManager.StopListening<OnSpeechLocalizationKeyEvent>(PlayLocalizedSpeech);
    }

    private void PlaySpeech(OnSpeechPlayEvent e)
    {
        Speak(e.text);
    }

    private void StopSpeech(OnSpeechStopEvent e)
    {
        Stop();
    }

    private void PlayLocalizedSpeech(OnSpeechLocalizationKeyEvent e)
    {
        Speak(LocalizationManager.Instance.GetText(e.key));
    }

    public void ApplyVoice(Language language)
    {
        currentProfile = voiceDatabase.Get(language);

        if (currentProfile == null)
        {
            UnityEngine.Debug.LogWarning($"No voice profile found for {language}");
            return;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        if (!isInitialized) return;

        tts.Call<int>("setSpeechRate", currentProfile.speechRate);
        tts.Call<int>("setPitch", currentProfile.pitch);
#endif
    }

    public void Speak(LocalizationKey key)
    {
        Speak(LocalizationManager.Instance.GetText(key));
    }

    public void Speak(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        Stop();

#if UNITY_ANDROID && !UNITY_EDITOR
        if (!isInitialized) return;
        tts.Call<int>("speak", text, 0, null, null);

#elif UNITY_EDITOR_WIN
        int editorRate = 0;

        if (currentProfile != null)
        {
            editorRate = Mathf.RoundToInt((currentProfile.speechRate - 1f) * 5f);
            editorRate = Mathf.Clamp(editorRate, -10, 10);
        }

        var escaped = text.Replace("'", "''");

        var psi = new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments =
                $"-Command Add-Type -AssemblyName System.Speech; " +
                $"$speak = New-Object System.Speech.Synthesis.SpeechSynthesizer; " +
                $"$speak.Rate = {editorRate}; " +
                $"$speak.Speak('{escaped}')",
            CreateNoWindow = true,
            UseShellExecute = false
        };

        editorSpeechProcess = Process.Start(psi);

#else
        Debug.Log("TTS: " + text);
#endif
    }

    public void Stop()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!isInitialized) return;
        tts.Call("stop");

#elif UNITY_EDITOR_WIN
        if (editorSpeechProcess != null && !editorSpeechProcess.HasExited)
        {
            editorSpeechProcess.Kill();
            editorSpeechProcess = null;
        }
#endif
    }

    private class TTSInitListener : AndroidJavaProxy
    {
        private readonly System.Action callback;

        public TTSInitListener(System.Action callback)
            : base("android.speech.tts.TextToSpeech$OnInitListener")
        {
            this.callback = callback;
        }

        public void onInit(int status)
        {
            callback?.Invoke();
        }
    }
}