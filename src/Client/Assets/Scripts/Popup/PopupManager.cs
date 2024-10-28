using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    GameObject popupPrefab;
    GameObject popupInstance;

    void Awake()
    {
        popupPrefab = Resources.Load<GameObject>("Prefabs/UI/PopupUI");
        if (popupPrefab == null)
            Debug.LogError("PopupPrefab is not assigned in PopupManager.");
        
    }

    void Update()
    {
        List<Tuple<string, Action>> list = PopupQueue.Instance.PopAll();
        if (list.Count > 0)
        {
            if (list[0].Item2 != null)
                Popup(list[0].Item1, list[0].Item2);
            else
                Popup(list[0].Item1);
        }
    }

    void Popup(string message, Action onConfirm = null)
    {
        if (popupPrefab == null)
        {
            Debug.LogError("PopupPrefab is not assigned in PopupManager.");
            return;
        }

        // Instantiate the popup if it isn't already open
        if (popupInstance == null)
            popupInstance = Instantiate(popupPrefab, transform);

        Transform backgroundTransform = popupInstance.transform.Find("Background");
        if (backgroundTransform == null)
        {
            Debug.LogError("Background is not found in the popup prefab.");
            return;
        }

        // Set the message text
        TextMeshProUGUI messageText = backgroundTransform.Find("PopupText").GetComponent<TextMeshProUGUI>();
        if (messageText != null)
            messageText.text = message;

        // Set the confirm button callback
        Button confirmButton = backgroundTransform.Find("ConfirmButton").GetComponent<Button>();
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(() =>
            {
                onConfirm?.Invoke();
                ClosePopup();
            });
        }

        // Display the popup
        popupInstance.SetActive(true);
    }

    void ClosePopup()
    {
        if (popupInstance != null)
            popupInstance.SetActive(false);
    }
}