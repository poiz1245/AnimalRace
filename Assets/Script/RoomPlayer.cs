using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomPlayer : NetworkBehaviour
{
    public enum EGameState
    {
        Lobby,
        GameCutscene,
        GameReady
    }

    public static readonly List<RoomPlayer> Players = new List<RoomPlayer>();

    public static Action<RoomPlayer> PlayerJoined;
    public static Action<RoomPlayer> PlayerLeft;
    public static Action<RoomPlayer> PlayerChanged;

    public static RoomPlayer Local;

    [Networked(OnChanged = nameof(OnStateChanged))] public NetworkBool IsReady { get; set; }
    [Networked(OnChanged = nameof(OnStateChanged))] public NetworkString<_32> Username { get; set; }


    [Networked] public NetworkBool HasFinished { get; set; }
    [Networked] public KartMove Kart { get; set; }
    [Networked] public EGameState GameState { get; set; }
    [Networked] public int KartId { get; set; }

    public bool IsLeader => Object != null && Object.IsValid && Object.HasStateAuthority;
    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasInputAuthority)
        {
            Local = this;

            PlayerChanged?.Invoke(this);
            RPC_SetPlayerStats(ClientInfo.Username, ClientInfo.KartId);
        }

        Players.Add(this);
        PlayerJoined?.Invoke(this);

        DontDestroyOnLoad(gameObject);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority, InvokeResim = true)]
    void RPC_SetPlayerStats(NetworkString<_32> username, int kartId)
    {
        Username = username;
        KartId = kartId;
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_SetKartId(int id)
    {
        KartId = id;
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_ChangeReadyState(NetworkBool state)
    {
        IsReady = state;
    }

    private void OnDisable()
    {
        PlayerLeft?.Invoke(this);
        Players.Remove(this);
    }

    static void OnStateChanged(Changed<RoomPlayer> changed) => PlayerChanged?.Invoke(changed.Behaviour);

    public static void RemovePlayer(NetworkRunner runner, PlayerRef p)
    {
        var roomPlayer = Players.FirstOrDefault(x => x.Object.InputAuthority == p);
        if(roomPlayer != null)
        {
            if(roomPlayer.Kart!= null)
                runner.Despawn(roomPlayer.Kart.Object);

            Players.Remove(roomPlayer);
            runner.Despawn(roomPlayer.Object);
        }
    }

 
}
