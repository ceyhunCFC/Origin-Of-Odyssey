using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public bool Turn = false; // FALSE IS MASTER TURN - TRUE IS OTHER TURN

    public string MasterPlayerName = "";
    public string OtherPlayerName = "";
    public string[] MasterDeck;
    public string[] OtherDeck;
    public string MasterMainCard = "";
    public string OtherrMainCard = "";


    public Text MasterPlayerNameText;
    public Text OtherPlayerNameText;
    public Text MasterDeckText;
    public Text OtherDeckText;
    public Text WhoTurnText;

    PhotonView PV;

    private void Awake()
    {

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        PV = GetComponent<PhotonView>();
    }

    public void FinishTurn(bool turn)
    {

        PV.RPC("RPC_FinishTurn", RpcTarget.AllBufferedViaServer, turn);
    }

    [PunRPC]
    void RPC_FinishTurn(bool turn)
    {

        Turn = turn;
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
