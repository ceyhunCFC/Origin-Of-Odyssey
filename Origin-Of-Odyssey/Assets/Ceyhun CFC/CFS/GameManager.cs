using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [HideInInspector] public static GameManager Instance;
    [HideInInspector] public bool Turn = false; // FALSE IS MASTER TURN - TRUE IS OTHER TURN
    [HideInInspector] public int ManaCount = 1;
    [HideInInspector] public string MasterPlayerName = "";
    [HideInInspector] public string OtherPlayerName = "";
    [HideInInspector] public int MasterHealth = 10;
    [HideInInspector] public int OtherHealth = 10;
    [HideInInspector] public string[] MasterDeck;
    [HideInInspector] public string[] OtherDeck;
    [HideInInspector] public string MasterMainCard = "";
    [HideInInspector] public string OtherrMainCard = "";
    [HideInInspector] public int TurnCount = 0;


    public Text MasterPlayerNameText;
    public Text OtherPlayerNameText;
    public Text MasterDeckText;
    public Text OtherDeckText;
    public Text WhoTurnText;
    public Text WinningName;

    public GameObject Panel;
    PhotonView PV;

    private void Awake()
    {
        /*if (Instance == null)
            Instance = this;
        else
         //   Destroy(gameObject);
        */
      //  DontDestroyOnLoad(gameObject);
        PV = GetComponent<PhotonView>();
    }

    public void MasterDamanage(int Damage)
    {
        PV.RPC("RPC_MasterDamanage", RpcTarget.AllBufferedViaServer,Damage);
    }

    [PunRPC]
    void RPC_MasterDamanage(int Damage)
    {
        if (MasterHealth - Damage > 0)
        {
            MasterHealth -= Damage;
        }
        else
        {
            Panel.SetActive(true);
            WinningName.text = OtherPlayerName + " WON!";

            StartCoroutine(LoadMainMenu());

        }

    }

    public void OtherDamanage(int Damage)
    {
        PV.RPC("RPC_OtherDamanage", RpcTarget.AllBufferedViaServer,Damage);
    }

    [PunRPC]
    void RPC_OtherDamanage(int Damage)
    {
        if (OtherHealth - Damage > 0)
        {
            OtherHealth -= Damage;
        }
        else
        {
            Panel.SetActive(true);
            WinningName.text = MasterPlayerName + " WON!";
            StartCoroutine(LoadMainMenu());
        }
     
    }


    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(3.0f);
        LeaveRoomAndReturnToMainMenu();
    }

    public void LeaveRoomAndReturnToMainMenu()
    {
        PhotonNetwork.LeaveRoom(); // Odayı terk et
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby"); // Ana menü sahnesine geç
    }

    public override void OnLeftLobby()
    {
       // SceneManager.LoadScene("LoginScene"); // Ana menü sahnesine geç
        //Destroy(GameObject.Find("RoomManager").gameObject);
    }

    public void AddMana()
    {
        PV.RPC("RPC_AddMana", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    void RPC_AddMana()
    {
        if (ManaCount<=9)
        {
            ManaCount++;
        }
        
    }

    public void FinishTurn(bool turn)
    {

        PV.RPC("RPC_FinishTurn", RpcTarget.AllBufferedViaServer, turn);
    }

    [PunRPC]
    void RPC_FinishTurn(bool turn)
    {

        Turn = turn;
        TurnCount++;
        WhoTurnText.text += ", " + turn.ToString();
    }
    
    public void SendData(string ID,string Nickname,string[] Deck)
    {
        PV.RPC("RPC_SendData", RpcTarget.All,ID, Nickname,Deck);
    }

    [PunRPC]
    void RPC_SendData(string ID, string Nickname, string[] Deck)
    {
        if (ID=="1")
        {
            MasterPlayerName = Nickname;
            MasterDeck = Deck;
            MasterMainCard = "ZeusCard";

           // MasterPlayerNameText.text = MasterPlayerName;
           //  MasterDeckText.text = MasterDeck.ToString();
        }
        else if (ID == "2")
        {
            OtherPlayerName = Nickname;
            OtherDeck = Deck;
            OtherrMainCard = "ZeusCard";


            //  OtherPlayerNameText.text = OtherPlayerName;
            //   OtherDeckText.text = OtherDeck.ToString();
        }

       
    }

}