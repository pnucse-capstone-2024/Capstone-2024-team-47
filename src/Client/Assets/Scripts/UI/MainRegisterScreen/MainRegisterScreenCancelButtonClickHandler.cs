using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainRegisterScreenCancelButtonClickHandler : MonoBehaviour
{
    Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(CancelHandle);
    }

    void CancelHandle()
    {
        SceneManager.LoadScene("LoginScene");
    }
}
