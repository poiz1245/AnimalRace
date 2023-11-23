using Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour, IDisabledUI
{

    public TextMeshProUGUI sessionName;
    public GameObject textPrefab;
    public Transform parent;
    public Button readyUp;
    public Button playUp;
    [SerializeField] private UIScreen dummyScreen;

    static readonly Dictionary<RoomPlayer, LobbyItemUI> ListItems = new Dictionary<RoomPlayer, LobbyItemUI>();
    static bool IsSubscribed;
    private void Awake()
    {
        RoomPlayer.PlayerChanged += (player) =>
        {
            var isLeader = RoomPlayer.Local.IsLeader;
        };
    }

    void Start()
    {
        sessionName.text = $"Room Name : {ServerInfo.LobbyName}";
    }

    void UpdateDetails()
    {

    }

    public void OnDestruction()
    {

    }
    public void Setup()
    {
        if (IsSubscribed) return;

        RoomPlayer.PlayerJoined += AddPlayer;
        RoomPlayer.PlayerLeft += RemovePlayer;

        RoomPlayer.PlayerChanged += EnsureAllPlayersReady;

        readyUp.onClick.AddListener(ReadyUpListener);

        IsSubscribed = true;
    }

    private void OnDestroy()
    {
        if(!IsSubscribed) return;

        RoomPlayer.PlayerJoined -= AddPlayer;
        RoomPlayer.PlayerLeft -= RemovePlayer;

        readyUp.onClick.RemoveListener(ReadyUpListener);

        IsSubscribed= false;
    }

    void AddPlayer(RoomPlayer player)
    {
       if(ListItems.ContainsKey(player))
        {
           var toRemove = ListItems[player];
            Destroy(toRemove.gameObject);

            ListItems.Remove(player);
        }

        var obj = Instantiate(textPrefab, parent).GetComponent<LobbyItemUI>();
        obj.SetPlayer(player);

        ListItems.Add(player, obj);

        //UpdateDetails(GameManager.Instance);
    }

    void RemovePlayer(RoomPlayer player)
    {
        if(!ListItems.ContainsKey(player)) return;

        var obj = ListItems[player];
        if (obj != null)
        {
            Destroy(obj.gameObject);
            ListItems.Remove(player);
        }
    }

    void EnsureAllPlayersReady(RoomPlayer lobbyPlayer)
    {
        if(!RoomPlayer.Local.IsLeader) { return; }

        if (IsAllReady())
        {
            Debug.Log("All Players are ready");
            playUp.gameObject.SetActive(true);
        }
        else
        {
            playUp.gameObject.SetActive(false);
        }
    }

    void ReadyUpListener()
    {
        var local = RoomPlayer.Local;
        if(local && local.Object && local.Object.IsValid)
        {
            local.RPC_ChangeReadyState(!local.IsReady);
        }
    }

    static bool IsAllReady() => RoomPlayer.Players.Count > 0 && RoomPlayer.Players.All(player => player.IsReady);
}
