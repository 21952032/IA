using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FlaskServerConnection : MonoBehaviour
{
    private string serverURL = "http://localhost:5000/upload";
    public TMP_Text debugText;
    public AudioRecorder audioRecorder;
    public Emotions emotions;
    private IEnumerator SendAudioFileToServer(string fileName)
    {
        string serverFolderPath = Path.Combine(Application.dataPath, "Server", "uploads");
        string filePath = Path.Combine(serverFolderPath, fileName);

        if (!File.Exists(filePath))
        {
            debugText.text = "Audio file not found.";
            yield break;
        }

        byte[] fileData = File.ReadAllBytes(filePath);
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", fileData, fileName, "audio/wav");

        UnityWebRequest uwr = UnityWebRequest.Post(serverURL, form);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            debugText.text = $"Error uploading audio file: {uwr.error}";
        }
        else
        {
            debugText.text = "Audio file uploaded successfully";
            int prediction= ProcessJsonResponse(uwr.downloadHandler.text);
            emotions.NewEmotion(prediction);
        }
        audioRecorder.ReenableButton();
    }

    private int ProcessJsonResponse(string jsonResponse)
    {
        int prediction = -1;
        try
        {
            prediction = int.Parse(jsonResponse);
        }
        catch (FormatException)
        {
            Debug.LogError("Error parsing JSON response as integer");
        }
        return prediction;
    }

    public void StartAudioUpload(string fileName)
    {
        StartCoroutine(SendAudioFileToServer(fileName));
    }

}
