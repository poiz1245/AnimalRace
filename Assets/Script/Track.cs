using FIMSpace;
using Fusion;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : NetworkBehaviour
{
    public Checkpoint[] checkPoints;
    GameObject dummyScreenObject;
    UIScreen dummyScreen;

    public GameObject kartPrefab;
    public static Track Current { get; private set; }

    private void Awake()
    {
        Current = this;
        GameManager.SetTrack(this);

        foreach (var player in RoomPlayer.Players)
        {
            player.GameState = RoomPlayer.EGameState.GameCutscene;
            GameManager.Instance.SpawnPlayer(NetworkCallBack.NC.runner, player);
        }
    }
    private void Start()
    {
        dummyScreenObject = LevelManager.Instance.dummyScreen;
        dummyScreen = dummyScreenObject.GetComponent<UIScreen>();
        UIScreen.Focus(dummyScreen);
    }


    private void OnDestroy()
    {
        GameManager.SetTrack(null);
    }

    //[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_DefocusUI()
    {
        UIScreen.Focus(dummyScreen);
    }
}

