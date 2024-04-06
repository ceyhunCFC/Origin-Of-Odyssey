using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    GameManager _GameManager;

    private void Start()
    {
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
}
