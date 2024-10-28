using NetworkCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainLoginScreenRegisterButtonClickHandler : MonoBehaviour
{
    Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(LoadRegisterScene);
    }

    void LoadRegisterScene()
    {
        SceneManager.LoadScene("RegisterScene");
    }
}
