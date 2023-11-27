using Cinemachine;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] GameObject endingPanel;
    [SerializeField] public TextMeshProUGUI winnerName;

    public GameObject[] checkPoints;
    public Transform[] spawnPoints;
    public NetworkObject kartPrefab;

    public GameObject VC;
    //ICameraControll cameraControll;

    public static event Action<GameManager> OnLobbyDetailsUpdated;

    public static GameManager Instance { get; private set; }

    public static Track CurrentTrack { get; private set; }

    [Networked(OnChanged = nameof(OnLobbyDetailsChangedCallback))] public NetworkString<_32> LobbyName { get; set; }
    [Networked(OnChanged = nameof(OnLobbyDetailsChangedCallback))] public int Maxusers { get; set; }

    [Networked] public string userName { get; set; }
    [Networked] public NetworkBool isFinish { get; set; }

    static void OnLobbyDetailsChangedCallback(Changed<GameManager> changed)
    {
        OnLobbyDetailsUpdated?.Invoke(changed.Behaviour);
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public override void FixedUpdateNetwork()
    {
        if (isFinish)
        {
            endingPanel.SetActive(true);
        }
    }

    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasStateAuthority)
        {
            LobbyName = ServerInfo.LobbyName;
            Maxusers = ServerInfo.MaxUsers;
        }
    }

    public static void SetTrack(Track track)
    {
        CurrentTrack = track;
    }

    public void SpawnPlayer(NetworkRunner runner, RoomPlayer player)
    {
        var index = RoomPlayer.Players.IndexOf(player);
        var point = spawnPoints[index]; //스폰위치 index받아서 안겹치게 스폰
        var rotate = Quaternion.Euler(0, 90, 0);

        var prefabId = player.KartId;
        var prefab = ResourceManager.Instance.kartDefinitions[prefabId].prefab;

        runner.Spawn(
            prefab,
            point.position,
            rotate,
            player.Object.InputAuthority
        );

        player.GameState = RoomPlayer.EGameState.GameCutscene;
    }

    public void SpawnVC(NetworkRunner runner, RoomPlayer player)
    {
        runner.Spawn(VC, new Vector3(0, 2, 0), Quaternion.identity, player.Object.InputAuthority);
    }

    public void EndGameButton()
    {
        NetworkCallBack.NC.OnShutdown(NetworkCallBack.NC.runner, Fusion.ShutdownReason.GameClosed);
    }

}
