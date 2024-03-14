using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    PhotonView PV;
    GameManager _GameManager;

    string OwnName = "";
    string OwnDeck = "";
    string CompetitorName = "";
    string CompetitorDeck = "";

    public Text OwnNameText;
    public Text OwnDeckText;
    public Text CompetitorNameText;
    public Text CompetitorDeckText;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (!PV.IsMine)
        {
            Destroy(GetComponent<PlayerController>().gameObject);
        }

        GetDataForUI();
    }


    void GetDataForUI()
    {
        if (OwnName == "" || OwnDeck== "" || CompetitorName == "" || CompetitorDeck == "")
        {
            if (PV.IsMine)
            {
                if (PV.Owner.IsMasterClient)
                {
                    OwnName = _GameManager.MasterPlayerName;
                    OwnNameText.text = OwnName;

                    OwnDeck = _GameManager.MasterDeck;
                    OwnDeckText.text = OwnDeck;

                    CompetitorName = _GameManager.OtherPlayerName;
                    CompetitorNameText.text = CompetitorName;

                    CompetitorDeck = _GameManager.OtherDeck;
                    CompetitorDeckText.text = CompetitorDeck;

                    print("Im MasterClient");
                }
                else
                {
                    OwnName = _GameManager.OtherPlayerName;
                    OwnNameText.text = OwnName;

                    OwnDeck = _GameManager.OtherDeck;
                    OwnDeckText.text = OwnDeck;

                    CompetitorName = _GameManager.MasterPlayerName;
                    CompetitorNameText.text = CompetitorName;

                    CompetitorDeck = _GameManager.MasterDeck;
                    CompetitorDeckText.text = CompetitorDeck;

                    print("Im OtherClient");
                }
            }

            Invoke("GetDataForUI", 1);
        }
    }
}
