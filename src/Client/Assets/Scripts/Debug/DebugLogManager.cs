using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1000)]
public class DebugLogManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI logText;
    [SerializeField] ScrollRect scrollRect;
    Queue<string> logMessagesQueue = new Queue<string>();
    int maxMessages = 100;

    static DebugLogManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }

    void OnEnable()
    { 
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string message = $"{logString}\n";
        logMessagesQueue.Enqueue(message);

        if (logMessagesQueue.Count > maxMessages)
        {
            logMessagesQueue.Dequeue();
        }

        logText.text = string.Join("", logMessagesQueue.ToArray());
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0;
    }
}