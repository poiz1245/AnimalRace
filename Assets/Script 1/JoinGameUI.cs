using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameUI : MonoBehaviour
{
    public InputField lobbyName;
    public Button confirmButton;


    private void OnEnable()
    {
        SetLobbyName(lobbyName.text);
    }
    void Start()
    {
        lobbyName.onValueChanged.AddListener(SetLobbyName);
        lobbyName.text = ClientInfo.LobbyName;
    }

    void SetLobbyName(string lobby)
    {
        ClientInfo.LobbyName = lobby;
    }

}
