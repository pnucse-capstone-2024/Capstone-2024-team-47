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
            PopupQueue.Instance.Push("���̵� �Է����ּ���!");
            return;
        }

        if (pw == "")
        {
            PopupQueue.Instance.Push("��й�ȣ�� �Է����ּ���!");
            return; 
        }

        if (pwRe == "")
        {
            PopupQueue.Instance.Push("��й�ȣ Ȯ���� �Է����ּ���!");
            return;
        }

        if (pw != pwRe)
        {
            PopupQueue.Instance.Push("��й�ȣ�� Ȯ��ĭ�� �ٸ��ϴ�!");
            return;
        }

        C_Register registerPkt = new C_Register();
        registerPkt.Id = id;
        registerPkt.Pw = pw;
        Manager.NetworkManager.Send(registerPkt);
    }
}