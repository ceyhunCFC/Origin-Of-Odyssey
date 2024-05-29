using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CardController : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    GameManager _GameManager;

    private void Start()
    {
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        PV = GetComponent<PhotonView>();
    }

    public void UsedCard(int Damage,bool isMaster)
    {

        if (isMaster)
        {
           
            _GameManager.OtherDamanage(Damage);

        }
        else
        {
            _GameManager.MasterDamanage(Damage);
        }
    }

    public void DestroyObject(int TargetCardIndex)
    {
        PV.RPC("RPC_DestroyObject", RpcTarget.All, TargetCardIndex);
    }

    [PunRPC]
    void RPC_DestroyObject(int TargetCardIndex)
    {
      
        if (PV.IsMine)
        {
           // PhotonNetwork.Destroy(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[TargetCardIndex].transform.GetChild(0).transform.gameObject);
           PhotonNetwork.Destroy(gameObject);

            Debug.Log("DESRT;OrYYY");
        }

       
    }

    public void SetInformation()
    {
        PV.RPC("RPC_SetInformation", RpcTarget.All);
    }

    [PunRPC]
    void RPC_SetInformation()
    {

        GetComponent<CardInformation>().CardNameText.text = GetComponent<CardInformation>().CardName;
        GetComponent<CardInformation>().CardDesText.text = GetComponent<CardInformation>().CardDes;
        GetComponent<CardInformation>().CardHealthText.text = GetComponent<CardInformation>().CardHealth;
        GetComponent<CardInformation>().CardDamageText.text = GetComponent<CardInformation>().CardDamage.ToString();
        GetComponent<CardInformation>().CardManaText.text = GetComponent<CardInformation>().CardMana.ToString();
    }
}
