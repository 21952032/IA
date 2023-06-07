using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;
using TMPro;

public class FlaskManager : MonoBehaviour
{
    private string exePath = "Server/Server.exe";
    private Process flaskProcess;
    public TMP_Text debugText;
    // Start is called before the first frame update
    void Start()
    {
        OpenFlaskApp();
    }

    public void OpenFlaskApp()
    {
        string gameDataPath = Application.dataPath;
        string exeFilePath = Path.Combine(gameDataPath, exePath);

        debugText.text = "Inicializando servidor Flask";
        if (File.Exists(exeFilePath))
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(exeFilePath)
            {
                UseShellExecute = true,
                //Verb = "runas" // Optional: Run the process with elevated privileges
            };

            flaskProcess = new Process // Assign the Process object to the class member
            {
                StartInfo = startInfo
            };

            flaskProcess.Start();
            // Remove process.WaitForExit(); to keep the exe file running
        }
        else
        {
            debugText.text="Flask app executable not found at " + exeFilePath;
        }
    }

    public void StopServerButton()
    {
        if (flaskProcess != null && !flaskProcess.HasExited)
        {
            flaskProcess.Kill(); // Stop the exe file when this method is called
        }
        Application.Quit();
    }

    public void RestartServerButton()
    {
        if (flaskProcess != null && !flaskProcess.HasExited)
        {
            flaskProcess.Kill(); // Stop the exe file when this method is called
        }
        OpenFlaskApp();
    }
}
