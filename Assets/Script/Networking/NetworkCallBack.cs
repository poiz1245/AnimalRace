using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using System.Data;
using Managers;

public enum ConnectionStatus
{
    Disconnected,
    Connecting,
    Failed,
    Connected
}


//[RequireComponent(typeof(LevelManager))]
public class NetworkCallBack : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private UIScreen dummyScreen;
    [SerializeField] private GameManager gameManagerPrefab;
    [SerializeField] private RoomPlayer roomPlayerPrefab;

    public static ConnectionStatus ConnectionStatus = ConnectionStatus.Disconnected;

    public static NetworkCallBack NC;
    public NetworkRunner runner;
    GameMode gameMode;
    float yaw;

    void Update()
    {
        yaw = Input.GetAxis("Horizontal");
    }
    private void Awake()
    {
        if (NC == null)
        {
            NC = this;
        }
        else if (NC != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Application.runInBackground = true;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        QualitySettings.vSyncCount = 1;

        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene(1);
    }

    public void LoadTrack()
    {
        LevelManager.Instance.LoadTrack(2);
    }

    public void RunGame()
    {
        SetConnectionStatus(ConnectionStatus.Connecting);

        if (runner != null)
            LeaveSession();

        GameObject go = new GameObject("Seesion");
        DontDestroyOnLoad(go);

        runner = go.AddComponent<NetworkRunner>();
        runner.ProvideInput = gameMode != GameMode.Server;
        runner.AddCallbacks(this);

        Debug.Log($"Created gameobject {go.name} - starting game");

        var gameArgs = new StartGameArgs();
        gameArgs.GameMode = gameMode;
        gameArgs.SessionName = gameMode == GameMode.Host ? ServerInfo.LobbyName : ClientInfo.LobbyName;
        gameArgs.PlayerCount = ServerInfo.MaxUsers;
        gameArgs.SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
        gameArgs.DisableClientSessionCreation = true;

        runner.StartGame(gameArgs);
    }
    private void SetConnectionStatus(ConnectionStatus status)
    {
        Debug.Log($"Setting connection status to {status}");

        ConnectionStatus = status;

        if (!Application.isPlaying)
            return;

        if (status == ConnectionStatus.Disconnected || status == ConnectionStatus.Failed)
        {
            SceneManager.LoadScene(1);
            UIScreen.BackToInitial();
        }

    }
    public void SetCreateLobby() => gameMode = GameMode.Host;
    public void SetJoinLobby() => gameMode = GameMode.Client;

    public void LeaveSession()
    {
        if (runner != null)
            runner.Shutdown();
        else
            SetConnectionStatus(ConnectionStatus.Disconnected);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            if (gameMode == GameMode.Host)
                runner.Spawn(gameManagerPrefab, Vector3.zero, Quaternion.identity);

            var roomPlayer = runner.Spawn(roomPlayerPrefab, Vector3.zero, Quaternion.identity, player);
            roomPlayer.GameState = RoomPlayer.EGameState.Lobby;
        }
        SetConnectionStatus(ConnectionStatus.Connected);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        RoomPlayer.RemovePlayer(runner, player);

        SetConnectionStatus(ConnectionStatus);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var myInput = new NetworkInputData();

        myInput.yaw = yaw;

        myInput.buttons.Set(Buttons.forward, Input.GetKey(KeyCode.W));
        //myInput.buttons.Set(Buttons.forward, Input.GetKey(KeyCode.UpArrow));
        myInput.buttons.Set(Buttons.left, Input.GetKey(KeyCode.A));
        myInput.buttons.Set(Buttons.back, Input.GetKey(KeyCode.S));
        //myInput.buttons.Set(Buttons.back, Input.GetKey(KeyCode.DownArrow));
        myInput.buttons.Set(Buttons.right, Input.GetKey(KeyCode.D));
        myInput.buttons.Set(Buttons.drift, Input.GetKey(KeyCode.LeftShift));
        //myInput.buttons.Set(Buttons.drift, Input.GetKey(KeyCode.RightShift));

        input.Set(myInput);
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        SetConnectionStatus(ConnectionStatus.Disconnected);

        (string status, string message) = ShutdownReasonToHuman(shutdownReason);

        RoomPlayer.Players.Clear();

        if (runner)
            Destroy(runner.gameObject);
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        LeaveSession();
        SetConnectionStatus(ConnectionStatus.Failed);
        (string status, string messagge) = ConnectFailedReasonToHuman(reason);
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        if (runner.CurrentScene > 0)
            request.Refuse();
        else
            request.Accept();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        SetConnectionStatus(ConnectionStatus.Connected);
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        LeaveSession();
        SetConnectionStatus(ConnectionStatus.Disconnected);
    }

    private static (string, string) ShutdownReasonToHuman(ShutdownReason reason)
    {
        switch (reason)
        {
            case ShutdownReason.Ok:
                return (null, null);
            case ShutdownReason.Error:
                return ("Error", "Shutdown was caused by some internal error");
            case ShutdownReason.IncompatibleConfiguration:
                return ("Incompatible Config", "Mismatching type between client Server Mode and Shared Mode");
            case ShutdownReason.ServerInRoom:
                return ("Room name in use", "There's a room with that name! Please try a different name or wait a while.");
            case ShutdownReason.DisconnectedByPluginLogic:
                return ("Disconnected By Plugin Logic", "You were kicked, the room may have been closed");
            case ShutdownReason.GameClosed:
                return ("Game Closed", "The session cannot be joined, the game is closed");
            case ShutdownReason.GameNotFound:
                return ("Game Not Found", "This room does not exist");
            case ShutdownReason.MaxCcuReached:
                return ("Max Players", "The Max CCU has been reached, please try again later");
            case ShutdownReason.InvalidRegion:
                return ("Invalid Region", "The currently selected region is invalid");
            case ShutdownReason.GameIdAlreadyExists:
                return ("ID already exists", "A room with this name has already been created");
            case ShutdownReason.GameIsFull:
                return ("Game is full", "This lobby is full!");
            case ShutdownReason.InvalidAuthentication:
                return ("Invalid Authentication", "The Authentication values are invalid");
            case ShutdownReason.CustomAuthenticationFailed:
                return ("Authentication Failed", "Custom authentication has failed");
            case ShutdownReason.AuthenticationTicketExpired:
                return ("Authentication Expired", "The authentication ticket has expired");
            case ShutdownReason.PhotonCloudTimeout:
                return ("Cloud Timeout", "Connection with the Photon Cloud has timed out");
            default:
                Debug.LogWarning($"Unknown ShutdownReason {reason}");
                return ("Unknown Shutdown Reason", $"{(int)reason}");
        }
    }
    private static (string, string) ConnectFailedReasonToHuman(NetConnectFailedReason reason)
    {
        switch (reason)
        {
            case NetConnectFailedReason.Timeout:
                return ("Timed Out", "");
            case NetConnectFailedReason.ServerRefused:
                return ("Connection Refused", "The lobby may be currently in-game");
            case NetConnectFailedReason.ServerFull:
                return ("Server Full", "");
            default:
                Debug.LogWarning($"Unknown NetConnectFailedReason {reason}");
                return ("Unknown Connection Failure", $"{(int)reason}");
        }
    }


    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }
    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }
    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }
}
