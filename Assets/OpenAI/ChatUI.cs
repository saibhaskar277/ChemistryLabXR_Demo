using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    public OpenAIManager aiManager;

    public TMP_InputField inputField;
    public TMP_Text outputText;
    public Button sendBtn;

    private void Awake()
    {
        sendBtn.onClick.AddListener(Send);
    }

    private bool isRequestRunning = false;

    public void Send()
    {
        if (isRequestRunning) return;

        isRequestRunning = true;

        string userText = inputField.text;

        StartCoroutine(aiManager.SendMessage(userText, (response) =>
        {
            outputText.text = response;
            isRequestRunning = false;
        }));
    }
}