using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class OpenAIManager : MonoBehaviour
{
    [Header("API Settings")]
    public string apiKey = "YOUR_API_KEY_HERE";

    private string url = "https://api.openai.com/v1/chat/completions";

    public IEnumerator SendMessage(string userInput, System.Action<string> callback)
    {
        string json = @"
        {
            ""model"": ""gpt-4o-mini"",
            ""messages"": [
                {""role"": ""user"", ""content"": """ + userInput + @"""}
            ]
        }";

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;

            // Simple parsing (quick method)
            string reply = ExtractContent(response);

            callback?.Invoke(reply);
        }
        else
        {
            Debug.LogError(request.error);
            callback?.Invoke("Error getting response");
        }
    }

    string ExtractContent(string json)
    {
        int start = json.IndexOf("\"content\":\"") + 11;
        int end = json.IndexOf("\"", start);

        return json.Substring(start, end - start);
    }
}