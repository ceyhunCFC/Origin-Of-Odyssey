using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CardProgress : MonoBehaviourPunCallbacks
{
    

    public GameObject AttackerCard,TargetCard;
    int AttackerCardIndex, TargetCardIndex;

   public bool SirenWorks = false;


    private void Update()
    {
        if (AttackerCard == null)
            return;

        Debug.LogError("KART SECİMİ BEKLENİYORRR");

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
                else if (hit.collider.gameObject.CompareTag("CompetitorHeroCard"))
                {
                    TargetCard = hit.collider.gameObject; // SEÇİLEN RAKİP KART BUDUR
                    StandartDamage(AttackerCard, TargetCard); // BİZİM KARTIMIZ VE SEÇİLEN RAKİP HERO GÖNDERİLİR



                   
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

                    Debug.LogError("İKİNCİ KART SEÇİLDİ");

                    if (AttackerCard.GetComponent<CardInformation>().CardName== "Siren")
                    {
                        Siren(AttackerCard, TargetCard);

                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Lightning Bolt")
                    {
                        AttackerInfo = AttackerCard.GetComponent<CardInformation>();
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        LightningBolt(AttackerInfo, TargetInfo);

                        Debug.LogError("LİGHTİNG BOLLLLTT");
                    }
                  


                }
                else if (hit.collider.gameObject.CompareTag("CompetitorHeroCard"))
                {
                    TargetCard = hit.collider.gameObject;

                    if (AttackerCard.GetComponent<CardInformation>().CardName == "Siren")
                    {
                        Siren(AttackerCard, TargetCard);

                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Lightning Bolt")
                    {
                        AttackerInfo = AttackerCard.GetComponent<CardInformation>();
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        LightningBolt(AttackerInfo, TargetInfo);

                        Debug.LogError("LİGHTİNG BOLLLLTT");
                    }
                }
            }
        }

    }

    CardInformation AttackerInfo=null;
    CardInformation TargetInfo=null;

    void StandartDamage(GameObject Attacker, GameObject Target) // HANGİ HASAR ŞEKLİ UYGULANACAĞI SEÇİLMELİDİR
    {
         AttackerInfo = Attacker.GetComponent<CardInformation>();
        TargetInfo = Target.GetComponent<CardInformation>();

        if (TargetCard.name == "CompetitorHeoCard(Clone)")
        {
            AttackerCard.GetComponent<CardController>().UsedCard(AttackerInfo.CardDamage, GetComponent<PlayerController>().PV.Owner.IsMasterClient); // HERO YA DAMAGE VURMA
            Debug.LogError("HEROYA DAMAGEEEEE");

            if (GetComponent<PlayerController>().PV.IsMine)
            {
                GetComponent<PlayerController>().CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);

            }

            AttackerCard = null;
            TargetCard = null;
            TargetCardIndex = -1;
            SirenWorks = false;
            return;
        }
        else
        {
            switch (AttackerCard.GetComponent<CardInformation>().CardName)
            {
                case "Centaur Archer":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Minotaur Warrior":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Greek Hoplite":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Siren":
                    if (TargetInfo.CardDamage < 3)
                    {
                        SirenWorks = true;
                        AttackerCard = Target;              
                        Debug.LogError("İLK KART SEÇİLDİ");
                    }
                    break;

                case "Nemean Lion":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Hydra":
                    DamageCardsAround(1);
                    break;

                case "Pegasus Rider":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Gorgon":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Chimera":
                   //BU KART BİR DAHA ÇALIŞMAYACAK
                    break;

                case "Athena":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Heracles":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Stormcaller":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Odyssean Navigator":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Oracle's Emissary":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Lightning Forger":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;
            }
        }
       

     //   TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR -- Centaur Archer - Minotaur Warrior - Greek Hoplite


        /*if (TargetInfo.CardDamage < 3)
       {
           SirenWorks = true;
           AttackerCard = Target;              -- RAKİBİN 3 DAMAGEDEN KÜÇÜK OLAN BİR MİNYONU SEÇİLİR VE BAŞKA BİR RAKİP MİNYONA HASAR VERİLİR. - Sirens
           Debug.LogError("İLK KART SEÇİLDİ");
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

        /*  LightningBolt(AttackerInfo,TargetInfo);  // Bir minyon ya da hero seçecek, ona hasar verecek
          return;*/

       // LightningStorm(); //  rakip minyonlara 2 yada 3 hasar verecek - Lightning Storm


        RefreshCardDatas();
        AttackerCard = null;
        TargetCard = null;
        SirenWorks = false;
        TargetCardIndex = -1;
    }

  
    public void SetAttackerCard(int AttackerCardIndex)
    {
        Debug.LogError(AttackerCardIndex);
        AttackerCard = GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[AttackerCardIndex].transform.GetChild(0).gameObject;
        Debug.LogError(AttackerCard);
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
            Debug.LogError("Nearby object found: " + obj.name);

            obj.GetComponent<CardInformation>().CardHealth = (int.Parse(obj.GetComponent<CardInformation>().CardHealth) - DamageCount).ToString(); // SADECE BİR DAMAGE VURUYOR -- Nemean Lion
            int NearbyCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, obj.transform.parent.gameObject);

            GetComponent<PlayerController>().RefreshUsedCard(NearbyCardIndex, obj.GetComponent<CardInformation>().CardHealth); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

            if (int.Parse(obj.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET
            {

                GetComponent<PlayerController>().DeleteAreaCard(NearbyCardIndex);

            }
        }

       
    }

    public void FreezeAllEnemyMinions()
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

    public void DamageToAlLOtherMinions()
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

    public void FillWithHoplites()
    {

        for (int i = 7; i < 14; i++)
        {

            if (GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions[i].gameObject.transform.childCount==0)
            {
                GetComponent<PlayerController>().CreateHoplitesCard(i);
            }
           
        }

    }

   

    void Siren(GameObject Attacker, GameObject Target)
    {
        Debug.LogError("HASAR VERİLDİ!");
        CardInformation AttackerInfo = Attacker.GetComponent<CardInformation>();
        CardInformation TargetInfo = Target.GetComponent<CardInformation>();

        TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();

        GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

        if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
        {

            GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);

        }

    }

    void LightningBolt(CardInformation AttackerInfo, CardInformation TargetInfo)
    {
        if (TargetInfo.CardHealth != "") // BU BİR MİNYON
        {
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - 1).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR

            RefreshCardDatas();
        }
        else if (TargetCard.name == "CompetitorHeoCard(Clone)") // BU RAKİP HERO
        {
            AttackerCard.GetComponent<CardController>().UsedCard(1, GetComponent<PlayerController>().PV.Owner.IsMasterClient); // HERO YA DAMAGE VURMA

            if (GetComponent<PlayerController>().PV.IsMine)
            {
                GetComponent<PlayerController>().CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);

            }
        }

        Destroy(AttackerCard);

    }

    public void LightningStorm()
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard"); // RAKİBİN BÜTÜN KARTLARINI AL


        foreach (var Card in allTargets)
        {

            if (Card.GetComponent<CardInformation>().CardHealth != "") // MİNİYON OLUP OLMADIĞINI ÖĞREN - ŞARTI SAĞLIYORSA MİNYONDUR.
            {
                int CurrentCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, Card.transform.parent.gameObject);
                int RandomDamage = UnityEngine.Random.Range(2,3);
                Card.GetComponent<CardInformation>().CardHealth = (int.Parse(Card.GetComponent<CardInformation>().CardHealth) - RandomDamage).ToString(); //  İKİ DAMAGE VURUYOR
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









    void RefreshCardDatas()
    {
        GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

        if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
        {

            GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);

        }

    }

}
