using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginButtonClickHandler : MonoBehaviour
{
    Button _button;

    [SerializeField] TMP_InputField _idInput;
    [SerializeField] TMP_InputField _pwInput;

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(LoginHandle);
    }

    void LoginHandle()
    {
        string id = _idInput.text;
        string pw = _pwInput.text;

        C_Login loginPkt = new C_Login();
        loginPkt.Id = id;
        loginPkt.Pw = pw;
        Manager.NetworkManager.Send(loginPkt);
    }
}
