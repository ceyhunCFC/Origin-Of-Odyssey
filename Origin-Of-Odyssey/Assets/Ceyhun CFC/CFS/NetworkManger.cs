using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManger : MonoBehaviourPunCallbacks
{
    public static NetworkManger Instance;

    [SerializeField] GameObject CompetitorCard;
    [SerializeField] GameObject CompetitorSlot;

    private string currentMode = "";

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("Connecting to Master!");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        print("Joined Lobby");
    }

    public void AutoJoinOrCreate(string mode)
    {
        currentMode = mode;
        MenuManager.Instance.OpenMenu("loading");
        PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable { { "mode", mode } }, 2);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("No room found, creating a new one...");
        CreateRoom(currentMode);
    }

    private void CreateRoom(string mode)
    {
        string roomName = $"{mode}_{Random.Range(0, 10000)}";

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "mode", mode } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "mode" };

        PhotonNetwork.CreateRoom(roomName, roomOptions);
        print($"Creating Room: {roomName} for Mode: {mode}");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        print($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");

        Player[] players = PhotonNetwork.PlayerList;

        if (players.Length == 2)
        {
            CompetitorSlot.GetComponent<Animator>().SetBool("Stop", true);

            if (PhotonNetwork.IsMasterClient)
            {
                CompetitorCard.GetComponent<PlayerListItem>().SetUp(players[1]);
            }
            else
            {
                CompetitorCard.GetComponent<PlayerListItem>().SetUp(players[0]);
            }
        }
    }

    public void StartRankedMode()
    {
        AutoJoinOrCreate("Ranked");
    }

    public void StartAdventureMode()
    {
        AutoJoinOrCreate("Adventure");
    }

    public void StartBrawlMode()
    {
        AutoJoinOrCreate("Brawl");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print($"Room creation failed: {message}");
        MenuManager.Instance.OpenMenu("error");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Player[] players = PhotonNetwork.PlayerList;

        if (players.Length == 2)
        {
            StartCoroutine(StartMatch());

            CompetitorSlot.GetComponent<Animator>().SetBool("Stop", true);

            if (PhotonNetwork.IsMasterClient)
            {
                CompetitorCard.GetComponent<PlayerListItem>().SetUp(players[1]);
            }
            else
            {
                CompetitorCard.GetComponent<PlayerListItem>().SetUp(players[0]);
            }
        }
    }

    IEnumerator StartMatch()
    {
        yield return new WaitForSeconds(8);

        if (currentMode == "Ranked")
        {
            PhotonNetwork.LoadLevel("RankedBattleMap");
        }
        else if (currentMode == "Adventure")
        {
            PhotonNetwork.LoadLevel("AdventureMap");
        }
        else if (currentMode == "Brawl")
        {
            PhotonNetwork.LoadLevel("BrawlMap");
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            print($"Room: {room.Name}, Mode: {room.CustomProperties["mode"]}");
        }
    }
}
