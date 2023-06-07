using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;

public class AudioRecorder : MonoBehaviour
{
    public int recordDuration = 2;
    private string microphoneDevice;
    public Button recordButton;
    public TMP_Text debugText;
    public FlaskServerConnection connection;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            microphoneDevice = Microphone.devices[0];
        }
    }

    public void RecordButton()
    {
        recordButton.interactable = false;
        StartCoroutine(RecordAndSaveAudio());
    }

    public void ReenableButton()
    {
        recordButton.interactable = true;
    }

    public IEnumerator RecordAndSaveAudio()
    {
        if (microphoneDevice == null)
        {
            debugText.text = "No microphone device found.";
            yield break;
        }
        debugText.text = "Recording...";
        AudioClip recordedClip = Microphone.Start(microphoneDevice, false, recordDuration, 44100);
        yield return new WaitForSeconds(recordDuration);
        Microphone.End(microphoneDevice);

        string filePath = Path.Combine(Application.dataPath, "Server", "uploads", "recordedAudio.wav");
        // Eliminar el archivo existente si existe
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        SaveAudioClipToWav(recordedClip, filePath);
        connection.StartAudioUpload("recordedAudio.wav");
    }

    private void SaveAudioClipToWav(AudioClip clip, string filePath)
    {
        // Crear la carpeta 'uploads' si no existe
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Crear un archivo vacío para guardar los datos
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            // Escribir el encabezado del archivo .wav
            byte[] header = CreateWavHeader(clip);
            fileStream.Write(header, 0, header.Length);

            // Convertir el AudioClip en un arreglo de bytes
            float[] samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);
            byte[] bytes = ConvertSamplesToBytes(samples);

            // Escribir los datos de audio en el archivo .wav
            fileStream.Write(bytes, 0, bytes.Length);
        }
    }

    private byte[] CreateWavHeader(AudioClip clip)
    {
        int samples = clip.samples;
        int channels = clip.channels;
        int frequency = clip.frequency;
        int byteRate = frequency * channels * 2;

        byte[] header = new byte[44];

        // RIFF header
        System.Text.Encoding.ASCII.GetBytes("RIFF").CopyTo(header, 0);
        BitConverter.GetBytes(36 + samples * channels * 2).CopyTo(header, 4);
        System.Text.Encoding.ASCII.GetBytes("WAVE").CopyTo(header, 8);

        // fmt chunk
        System.Text.Encoding.ASCII.GetBytes("fmt ").CopyTo(header, 12);
        BitConverter.GetBytes(16).CopyTo(header, 16); // Sub-chunk size
        BitConverter.GetBytes((short)1).CopyTo(header, 20); // PCM format
        BitConverter.GetBytes((short)channels).CopyTo(header, 22);
        BitConverter.GetBytes(frequency).CopyTo(header, 24);
        BitConverter.GetBytes(byteRate).CopyTo(header, 28);
        BitConverter.GetBytes((short)(channels * 2)).CopyTo(header, 32); // Block align
        BitConverter.GetBytes((short)16).CopyTo(header, 34); // Bits per sample

        // data chunk
        System.Text.Encoding.ASCII.GetBytes("data").CopyTo(header, 36);
        BitConverter.GetBytes(samples * channels * 2).CopyTo(header, 40);

        return header;
    }

    private byte[] ConvertSamplesToBytes(float[] samples)
    {
        byte[] bytes = new byte[samples.Length * 2];
        int rescaleFactor = 32767; // short.MaxValue

        for (int i = 0; i < samples.Length; i++)
        {
            short value = (short)(samples[i] * rescaleFactor);
            BitConverter.GetBytes(value).CopyTo(bytes, i * 2);
        }

        return bytes;
    }

}
