using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CardProgress : MonoBehaviourPunCallbacks
{
    

    GameObject AttackerCard,TargetCard,SecoundTargetCard;
    int AttackerCardIndex, TargetCardIndex, SecoundTargetCardIndex;

    bool SirenWorks = false;


    private void Update()
    {
        if (AttackerCard == null)
            return;


        if (Input.GetMouseButtonDown(0) && SirenWorks==false) // KENDİ SALDIRI KARTIMIZI SEÇTİKTEN SONRA AKTİF OLUR 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("CompetitorCard"))
                {
                    TargetCard = hit.collider.gameObject; // SEÇİLEN RAKİP KART BUDUR
                    TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, hit.collider.gameObject.transform.parent.gameObject);
                   
                    StandartDamage(AttackerCard,TargetCard); // BİZİM KARTIMIZ VE SEÇİLEN RAKİP KART GÖNDERİLİR


                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && SirenWorks == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("CompetitorCard"))
                {
                    TargetCard = hit.collider.gameObject;
                    TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, hit.collider.gameObject.transform.parent.gameObject);

                    print("İKİNCİ KART SEÇİLDİ");
                    Siren(AttackerCard, TargetCard);


                }
            }
        }

    }

    void StandartDamage(GameObject Attacker, GameObject Target) // HANGİ HASAR ŞEKLİ UYGULANACAĞI SEÇİLMELİDİR
    {
        CardInformation AttackerInfo = Attacker.GetComponent<CardInformation>();
        CardInformation TargetInfo = Target.GetComponent<CardInformation>();

        //TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - TargetInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR -- Centaur Archer - Minotaur Warrior - Greek Hoplite


        /*if (TargetInfo.CardDamage < 3)
       {
           SirenWorks = true;
           AttackerCard = Target;              -- RAKİBİN 3 DAMAGEDEN KÜÇÜK OLAN BİR MİNYONU SEÇİLİR VE BAŞKA BİR RAKİP MİNYONA HASAR VERİLİR. - Sirens
           print("İLK KART SEÇİLDİ");
       }*/


        /* if (TargetInfo.CardName == "Nemean Lion")
        {
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 1).ToString(); // SADECE BİR DAMAGE VURUYOR -- Nemean Lion 

        }*/


        //DamageCardsAround(1); // VURULAN KARTIN ETRAFINDAKİ KARTLARA DA HASAR VERİYOR. -- Hydra

        /*if (TargetInfo.CardName == "Pegasus Rider" && TargetInfo.FirstTakeDamage==false)
      {
          TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // İLK ALDIĞI HASARI KARTA HASAR VERMEZ -- Pegasus Rider
      }
      else
      {
          TargetInfo.FirstTakeDamage = false;
      }*/


        //FreezeAllEnemyMinions(); // RAKİBİN BÜTÜN MİNYONLARINI DONDURUR -- Gorgon

        //DamageToAlLOtherMinions(); //  BÜTÜN MİNYONLARA İKİ HASAR VERİR. -- Chimera

        //FillWithHoplites(); // ON SIRAYI DOLDURUR -- Athena

        //AddSpell(); // DESTEYE YENİ BİR KART EKLİYOR (+1 SPELL HASARI VERMEKSİ GEREKİYORDU YANLIŞLIKLA DESTEYE BİR KART EKLENME OLARAK YAPILDI) -- Stormcaller





        GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

        if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
        {
          
            GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);
        
        }
    }

  
    public void SetAttackerCard(int AttackerCardIndex)
    {
        print(AttackerCardIndex);
        AttackerCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[AttackerCardIndex].transform.GetChild(0).gameObject;
        print(AttackerCard);
    }

    void DamageCardsAround(int DamageCount)
    {
        // Bütün nesneleri etikete göre bul
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard");
        List<GameObject> nearbyObjects = new List<GameObject>();

        foreach (GameObject target in allTargets)
        {
            // Mesafeyi hesapla
            float distance = Vector3.Distance(transform.position, target.transform.position);

            // Eğer mesafe belirlenen yarıçapın içindeyse, listeye ekle
            if (distance <= 10f)
            {
                nearbyObjects.Add(target);
            }
        }

        // Bulunan nesneleri işleme
        foreach (GameObject obj in nearbyObjects)
        {
            Debug.Log("Nearby object found: " + obj.name);

            obj.GetComponent<CardInformation>().CardHealth = (int.Parse(obj.GetComponent<CardInformation>().CardHealth) - DamageCount).ToString(); // SADECE BİR DAMAGE VURUYOR -- Nemean Lion
            int NearbyCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, obj.transform.parent.gameObject);

            GetComponent<PlayerController>().RefreshUsedCard(NearbyCardIndex, obj.GetComponent<CardInformation>().CardHealth); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

            if (int.Parse(obj.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET
            {

                GetComponent<PlayerController>().DeleteAreaCard(NearbyCardIndex);

            }
        }

       
    }

    void FreezeAllEnemyMinions()
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard"); // RAKİBİN BÜTÜN KARTLARINI AL

       
        foreach (var Card in allTargets)
        {
          
            if (Card.GetComponent<CardInformation>().CardHealth!="") // MİNİYON OLUP OLMADIĞINI ÖĞREN - ŞARTI SAĞLIYORSA MİNYONDUR.
            {
                Card.GetComponent<CardInformation>().CardFreeze = true;

            }
        }
    }

    void DamageToAlLOtherMinions()
    {

        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard"); // RAKİBİN BÜTÜN KARTLARINI AL


        foreach (var Card in allTargets)
        {

            if (Card.GetComponent<CardInformation>().CardHealth != "") // MİNİYON OLUP OLMADIĞINI ÖĞREN - ŞARTI SAĞLIYORSA MİNYONDUR.
            {
                int CurrentCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, Card.transform.parent.gameObject);

                Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) - 2).ToString(); //  İKİ DAMAGE VURUYOR
                GetComponent<PlayerController>().RefreshUsedCard(CurrentCardIndex, Card.GetComponent<CardInformation>().CardHealth); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

                if (int.Parse(Card.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET
                {

                    GetComponent<PlayerController>().DeleteAreaCard(CurrentCardIndex);

                }

                if (TargetCardIndex != CurrentCardIndex) // ŞUANKİ KART İLK SEÇİLEN RAKİP MİNYON KARTI İLE AYNI DEĞİLSE ÇALIŞTIR
                {
                   

                }

            }
        }
    }

    void FillWithHoplites()
    {

        for (int i = 7; i < 14; i++)
        {

            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount==0)
            {
                GetComponent<PlayerController>().CreateHoplitesCard(i);
            }
           
        }

    }

    void AddSpell()
    {
        GetComponent<PlayerController>().CreateSpellCard();

    }

    void Siren(GameObject Attacker, GameObject Target)
    {
        print("HASAR VERİLDİ!");
        CardInformation AttackerInfo = Attacker.GetComponent<CardInformation>();
        CardInformation TargetInfo = Target.GetComponent<CardInformation>();

        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();

        GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

        if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
        {

            GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);

        }

    }

    

}
