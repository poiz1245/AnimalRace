using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CreateGameUI : MonoBehaviour
{
    public InputField lobbyName;
    public Dropdown track;
    public Dropdown gameMode;
    public Slider playerCountSlider;
    public Image trackImage;
    public Text playerCountSliderText;
    public Image playerCountIcon;
    public Button confirmButton;

    public Sprite padlockSprite, publicLobbyIcon;

    void Start()
    {
        playerCountSlider.wholeNumbers = true;
        playerCountSlider.minValue = 1;
        playerCountSlider.maxValue = 4;
        playerCountSlider.value = 2;
        playerCountSlider.onValueChanged.AddListener(x => ServerInfo.MaxUsers = (int)x);

        //inputfield의 값이 변경 될때마다 호출(onValueChanged)
        lobbyName.onValueChanged.AddListener(x =>
        {   
            ServerInfo.LobbyName = x;
            confirmButton.interactable = !string.IsNullOrEmpty(x);
        });

        lobbyName.text = ServerInfo.LobbyName = "Session" + Random.Range(0, 1000);

    }
    private void Update()
    {
        SetPlayerCount();
        ServerInfo.MaxUsers = (int)playerCountSlider.value;
    }
    public void SetGameType(int gameType)
    {
        ServerInfo.GameMode = gameType;
    }
    public void SetTrack(int trackId)
    {
        //ServerInfo.TrackId = trackId;
    }
    public void SetPlayerCount()
    {
        playerCountSlider.value = ServerInfo.MaxUsers;
        playerCountSliderText.text = $"{ServerInfo.MaxUsers}";
       // playerCountIcon.sprite = ServerInfo.MaxUsers > 1 ? publicLobbyIcon : padlockSprite;
    }

    private bool lobbyIsValid;

    public void ValidateLobby()
    {
        lobbyIsValid = string.IsNullOrEmpty(ServerInfo.LobbyName) == false;
    }

    public void TryFocusScreen(UIScreen screen)
    {
        if (lobbyIsValid)
        {
            UIScreen.Focus(screen);
        }
    }

    public void TryCreateLobby(NetworkCallBack launcher)
    {
        if (lobbyIsValid)
        {
            launcher.RunGame();
            lobbyIsValid = false;
        }
    }
}
