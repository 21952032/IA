using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FlaskServerConnection : MonoBehaviour
{
    private string serverURL = "http://127.0.0.1:5000/upload";
    public TextAsset audioFilePath;

    public IEnumerator SendAudioFileToServer(string fileName)
    {
        if (audioFilePath == null)
            yield break;

        string filePath = AssetDatabase.GetAssetPath(audioFilePath);
        UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(filePath, new AudioType());
        yield return uwr.SendWebRequest();

        if (uwr.error != null)
            Debug.LogError($"Error uploading audio file: {uwr.error}");
        else
            Debug.Log("Audio file uploaded successfully");
    }

    public IEnumerator AskForPrediction(string predictionUrl)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(predictionUrl);
        yield return uwr.SendWebRequest();

        if (uwr.error != null)
            Debug.LogError($"Error getting prediction: {uwr.error}");
        else
            Debug.Log("Prediction received: {uwr.downloadHandler.text}");
    }

    public void StartAudioUpload(string fileName)
    {
        StartCoroutine(SendAudioFileToServer(fileName));
    }

    public void GetPrediction(string predictionUrl)
    {
        StartCoroutine(AskForPrediction(predictionUrl));
    }
}
