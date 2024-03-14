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

    public string MasterPlayerName = "";
    public string OtherPlayerName = "";
    public string MasterDeck = "";
    public string OtherDeck = "";

    public Text MasterPlayerNameText;
    public Text OtherPlayerNameText;
    public Text MasterDeckText;
    public Text OtherDeckText;

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

    public void SendData(string ID,string Nickname,string Deck)
    {
        PV.RPC("RPC_SendData", RpcTarget.AllBufferedViaServer,ID, Nickname,Deck);
    }

    [PunRPC]
    void RPC_SendData(string ID, string Nickname, string Deck)
    {
        if (ID=="1")
        {
            MasterPlayerName = Nickname;
            MasterDeck = Deck;

            MasterPlayerNameText.text = MasterPlayerName;
            MasterDeckText.text = MasterDeck;
        }
        else if (ID == "2")
        {
            OtherPlayerName = Nickname;
            OtherDeck = Deck;

            OtherPlayerNameText.text = OtherPlayerName;
            OtherDeckText.text = OtherDeck;
        }

       
    }

}
