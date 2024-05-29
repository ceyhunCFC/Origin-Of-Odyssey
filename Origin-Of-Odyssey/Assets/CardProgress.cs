using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CardProgress : MonoBehaviourPunCallbacks
{
    int AttackerCardIndex, TargetCardIndex;

    GameObject AttackerCard,TargetCard;
    PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (AttackerCard == null)
            return;


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("CompetitorCard"))
                {
                    TargetCard = hit.collider.gameObject;
                    TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, hit.collider.gameObject.transform.parent.gameObject);
                    print(TargetCardIndex);
                    StandartDamage(AttackerCard,TargetCard);


                }
            }
        }
    }

    void StandartDamage(GameObject Attacker, GameObject Target)
    {
        CardInformation AttackerInfo = Attacker.GetComponent<CardInformation>();
        CardInformation TargetInfo = Target.GetComponent<CardInformation>();

        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 1).ToString(); // SADECE BİR DAMAGE VURUYOR
        TargetInfo.SetInformation();

        if (int.Parse(TargetInfo.CardHealth) <= 0)
        {
            //  Target.GetComponent<CardController>().DestroyObject(TargetCardIndex);
            GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
         //   Target.GetComponent<CardController>().SetInformation();
        }
    }

  
    public void SetAttackerCard(int AttackerCardIndex)
    {
        print(AttackerCardIndex);
        AttackerCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[AttackerCardIndex].transform.GetChild(0).gameObject;
        print(AttackerCard);
    }
}
