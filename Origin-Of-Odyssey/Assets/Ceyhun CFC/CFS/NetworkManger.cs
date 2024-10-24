using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;

public class NetworkManger : MonoBehaviourPunCallbacks
{
    public static NetworkManger Instance;

    /*[SerializeField] InputField roomNameInputField;
    [SerializeField] Text ErrorText;
    [SerializeField] Text RoomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject startGameButton;*/

    [SerializeField] GameObject CompetitorCard;
    [SerializeField] GameObject CompetitorSlot;

    private bool isRanked=false;

    string PlayerDeckTotal = "";

    private ExitGames.Client.Photon.Hashtable _myCustomPlayer = new ExitGames.Client.Photon.Hashtable();

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

        for (int i = 0; i < AuthManager.playerDeckArray.Length; i++)
        {
            PlayerDeckTotal += AuthManager.playerDeckArray[i] + ",";
        }


        PhotonNetwork.NickName = AuthManager.userName;
      
    }


    public void CreateRoom()
    {

        /* if (string.IsNullOrEmpty(roomNameInputField.text))
         {
             return;
         }

         PhotonNetwork.CreateRoom(roomNameInputField.text);*/

        PhotonNetwork.JoinRandomOrCreateRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
       
        MenuManager.Instance.OpenMenu("room");
      //  RoomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

      /*  foreach (Transform child in PlayerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);

           
           
        }*/


       



        if (players.Count() == 2)
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



       // startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

   /* public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

        
    }*/


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
       // ErrorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("BattleMap");
    }

    public void StartRankedGame()
    {
        PhotonNetwork.LoadLevel("RankedBattleMap");
    }

    public void RankedButton()
    {
        isRanked = true;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");

    }

    public void BackMainMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenuScene");
    }

    
    public override void OnLeftLobby()
    {
         SceneManager.LoadScene("MainMenuScene"); // Ana menü sahnesine geç
     //   Destroy(GameObject.Find("RoomManager").gameObject);
    }

    /* public override void OnRoomListUpdate(List<RoomInfo> roomList)
     {
         foreach (Transform trans in roomListContent)
         {
             Destroy(trans.gameObject);
         }

         for (int i = 0; i < roomList.Count; i++)
         {
             if (roomList[i].RemovedFromList)
                 continue;
             Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
         }
     }*/

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       // Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        Player[] players = PhotonNetwork.PlayerList;

        if (players.Count() == 2)
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
        if(isRanked)
        {
            StartRankedGame();
        }
        else
        {
            StartGame();
        }
    }

}
