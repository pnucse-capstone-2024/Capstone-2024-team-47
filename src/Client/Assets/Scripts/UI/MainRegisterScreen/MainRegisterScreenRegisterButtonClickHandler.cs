using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainRegisterScreenRegisterButtonClickHandler : MonoBehaviour
{
    Button _button;

    [SerializeField] TMP_InputField _idInput;
    [SerializeField] TMP_InputField _pwInput;
    [SerializeField] TMP_InputField _pwReInput;

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(RegisterHandle);
    }

    void RegisterHandle()
    {
        string id = _idInput.text;
        string pw = _pwInput.text;
        string pwRe = _pwReInput.text;
        if (id == "")
        {
            PopupQueue.Instance.Push("아이디를 입력해주세요!");
            return;
        }

        if (pw == "")
        {
            PopupQueue.Instance.Push("비밀번호를 입력해주세요!");
            return; 
        }

        if (pwRe == "")
        {
            PopupQueue.Instance.Push("비밀번호 확인을 입력해주세요!");
            return;
        }

        if (pw != pwRe)
        {
            PopupQueue.Instance.Push("비밀번호가 확인칸과 다릅니다!");
            return;
        }

        C_Register registerPkt = new C_Register();
        registerPkt.Id = id;
        registerPkt.Pw = pw;
        Manager.NetworkManager.Send(registerPkt);
    }
}