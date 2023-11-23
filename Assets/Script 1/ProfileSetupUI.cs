using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSetupUI : MonoBehaviour
{
    public InputField nicknameInput;
    public Button confrimButton;

    // Start is called before the first frame update
    void Start()
    {
        nicknameInput.onValueChanged.AddListener(x => ClientInfo.Username = x);
        nicknameInput.onValueChanged.AddListener(x =>
        {
            confrimButton.interactable = !string.IsNullOrEmpty(x);
        });
    }

    public void AssertProfileSetUP()
    {
        if(string.IsNullOrEmpty(ClientInfo.Username))
        {
            UIScreen.Focus(GetComponent<UIScreen>());
        }
    }

}
