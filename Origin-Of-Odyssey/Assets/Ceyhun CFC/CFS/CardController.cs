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

    public void AddHealCard(int Heal, bool isMaster)
    {

        if (isMaster)
        {

            _GameManager.OtherHeal(Heal);

        }
        else
        {
            _GameManager.MasterHeal(Heal);
        }
    }


}
