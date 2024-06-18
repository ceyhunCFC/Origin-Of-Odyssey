using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UI;


public class CardProgress : MonoBehaviourPunCallbacks
{
    

    public GameObject AttackerCard,TargetCard;
    public int TargetCardIndex;

    public bool SecoundTargetCard = false;
    CardInformation AttackerInfo = null;
    CardInformation TargetInfo = null;

    public bool ForMyCard=false;

    private void Update()
    {
        if (AttackerCard == null)
            return;

        Debug.LogError("KART SECİMİ BEKLENİYORRR");

       Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "There's a card in front of it.";


        if (Input.GetMouseButtonDown(0) && SecoundTargetCard==false) // KENDİ SALDIRI KARTIMIZI SEÇTİKTEN SONRA AKTİF OLUR 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("CompetitorCard"))
                {
                    TargetCard = hit.collider.gameObject; // SEÇİLEN RAKİP KART BUDUR

                    
                    TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, hit.collider.gameObject.transform.parent.gameObject);
                    if(TargetCardIndex<7 && TargetCardIndex>=0)
                    {
                        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard");

                        foreach (GameObject target in allTargets)
                        {
                            if (target != TargetCard)
                            {
                                float distance = Vector3.Distance(TargetCard.transform.position, target.transform.position);
                                
                                if (distance <= 0.55f)
                                {
                                    Vector3 directionToTarget = (target.transform.position - TargetCard.transform.position).normalized;

                                    float dotProductForward = Vector3.Dot(TargetCard.transform.up, directionToTarget);

                                    if (dotProductForward > 0.5f)
                                    {
                                        Debug.Log("Önünde kart var");

                                         Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "There's a card in front of it.";
                                        

                                        AttackerCard = null;
                                        TargetCard = null;
                                        TargetCardIndex = -1;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    AttackerCard.GetComponent<CardInformation>().isAttacked = true;
                    if (TargetCard.GetComponent<CardInformation>().HaveShield==true && TargetCard.GetComponent<CardInformation>().FirstTakeDamage==true)
                    {
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        TargetInfo.FirstTakeDamage = false;
                        GetComponent<PlayerController>().RefreshCompotitorCard(TargetCardIndex, TargetInfo.FirstTakeDamage,TargetInfo.CardFreeze);
                        Debug.LogError("Have Shield ");                 // aegis shild varsa saldıramaz

                       Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text= "Have Shield.";


                        AttackerCard = null;
                        TargetCard = null;
                        TargetCardIndex = -1;
                        return;

                    }
                    if(TargetCard.GetComponent<CardInformation>().FirstDamageTaken==true && TargetCard.GetComponent<CardInformation>().CardName== "Pegasus Rider")
                    {
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        TargetInfo.FirstDamageTaken = false;
                        Debug.LogError("Pegasus first damage shield ");                 //pegasus ilk saldırısında hasar almaz
                      
                       Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Pegasus first damage shield.";


                        AttackerCard = null;
                        TargetCard = null;
                        TargetCardIndex = -1;
                        return;
                    }
                    if (TargetCard.GetComponent<CardInformation>().FirstDamageTaken == true && TargetCard.GetComponent<CardInformation>().CardName == "Odyssean Navigator")
                    {
                        Debug.LogError("Odyssean Navigator not attack yet ");                 //Odyssean Navigator saldırmadan saldırılamaz
                        
                       Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Odyssean Navigator not attack yet.";

                        AttackerCard = null;
                        TargetCard = null;
                        TargetCardIndex = -1;
                        return;
                    }
                    
                    StandartDamage(AttackerCard,TargetCard); // BİZİM KARTIMIZ VE SEÇİLEN RAKİP KART GÖNDERİLİR


                }
                else if (hit.collider.gameObject.CompareTag("CompetitorHeroCard"))
                {
                    TargetCard = hit.collider.gameObject; // SEÇİLEN RAKİP KART BUDUR
                    StandartDamage(AttackerCard, TargetCard); // BİZİM KARTIMIZ VE SEÇİLEN RAKİP HERO GÖNDERİLİR



                   
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && SecoundTargetCard == true)
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

                   Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Second Card Selected.";


                    if (AttackerCard.GetComponent<CardInformation>().CardName == "Siren")
                    {
                        Siren(AttackerCard, TargetCard);
                        ForMyCard = false;
                        SecoundTargetCard = false;
                        AttackerCard = null;
                        TargetCard = null;
                        TargetCardIndex = -1;

                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Lightning Bolt")
                    {
                        AttackerInfo = AttackerCard.GetComponent<CardInformation>();
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        LightningBolt(AttackerInfo, TargetInfo);
                        ForMyCard = false;
                        SecoundTargetCard = false;
                        AttackerCard = null;
                        TargetCard = null;
                        TargetCardIndex = -1;
                        Debug.LogError("LİGHTİNG BOLLLLTT");

                       Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text= "LIGHTING BOLLLLTT!";

                    }
                    



                }
                else if(hit.collider.gameObject.CompareTag("UsedCard"))
                {
                    TargetCard = hit.collider.gameObject;
                    TargetCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, hit.collider.gameObject.transform.parent.gameObject);
                    Debug.LogError("İKİNCİ KART SEÇİLDİ");

                   Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text  = "Second Card Selected.";


                    if (AttackerCard.GetComponent<CardInformation>().CardName == "Golden Fleece")
                    {
                        TargetInfo = TargetCard.GetComponent<CardInformation>();

                        GoldenFleece(TargetInfo);

                        Debug.LogError("Golden Fleecee");

                        Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text  = "Golden Fleecee.";

                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Aegis Shield")
                    {
                        AttackerInfo = AttackerCard.GetComponent<CardInformation>();
                        TargetInfo = TargetCard.GetComponent<CardInformation>();
                        TargetInfo.HaveShield = true;

                        RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage,TargetInfo.DivineSelected,TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken);

                        ForMyCard = false;
                        Destroy(AttackerCard);
                        SecoundTargetCard = false;
                        AttackerCard = null;
                        TargetCard = null;
                        TargetCardIndex = -1;

                        Debug.LogError("Aegis Shieldddd");

                          Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Aegis Shieldddd.";

                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Olympian Favor")
                    {
                        AttackerInfo = AttackerCard.GetComponent<CardInformation>();
                        TargetInfo = TargetCard.GetComponent<CardInformation>();

                        TargetInfo.CardHealth= (int.Parse(TargetInfo.CardHealth) + 2).ToString();
                        TargetInfo.CardDamage += 2;
                        TargetInfo.SetInformation();

                        RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected,TargetInfo.FirstTakeDamage, TargetInfo.FirstDamageTaken);

                        ForMyCard = false;
                        Destroy(AttackerCard);
                        SecoundTargetCard = false;
                        AttackerCard = null;
                        TargetCard = null;
                        TargetCardIndex = -1;

                        Debug.LogError("Olympiann");

                       Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Olympiann.";

                    }
                    else if (AttackerCard.GetComponent<CardInformation>().CardName == "Divine Ascention")
                    {
                        AttackerInfo = AttackerCard.GetComponent<CardInformation>();
                        TargetInfo = TargetCard.GetComponent<CardInformation>();

                        TargetInfo.DivineSelected = true;

                        RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected,TargetInfo.FirstTakeDamage,TargetInfo.FirstDamageTaken);

                        ForMyCard = false;
                        Destroy(AttackerCard);
                        SecoundTargetCard = false;
                        AttackerCard = null;
                        TargetCard = null;
                        TargetCardIndex = -1;

                        Debug.LogError("Divine Ascention");

                       Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Divine Ascention.";

                    }

                }
               /* else if (hit.collider.gameObject.CompareTag("CompetitorHeroCard"))
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
                }*/
            }
        }

    }

   

    void StandartDamage(GameObject Attacker, GameObject Target) // HANGİ HASAR ŞEKLİ UYGULANACAĞI SEÇİLMELİDİR
    {
         AttackerInfo = Attacker.GetComponent<CardInformation>();
        TargetInfo = Target.GetComponent<CardInformation>();

        if(TargetInfo.CardName== "Nemean Lion")
        {
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) -1).ToString();
            RefreshCardDatas();
            AttackerCard = null;
            TargetCard = null;
            SecoundTargetCard = false;
            TargetCardIndex = -1;
            return;
        }

        if (TargetCard.name == "CompetitorHeoCard(Clone)")
        {
            AttackerCard.GetComponent<CardController>().UsedCard(AttackerInfo.CardDamage, GetComponent<PlayerController>().PV.Owner.IsMasterClient); // HERO YA DAMAGE VURMA
            Debug.LogError("HEROYA DAMAGEEEEE");

           Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Damage to the enemy!";


            if (GetComponent<PlayerController>().PV.IsMine)
            {
                GetComponent<PlayerController>().CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);

            }
            if (GetComponent<PlayerController>().PV.IsMine)
            {
                GetComponent<PlayerController>().SetMana(AttackerCard);
            }

            AttackerCard = null;
            TargetCard = null;
            TargetCardIndex = -1;
            SecoundTargetCard = false;
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
                        GetComponent<CardProgress>().SecoundTargetCard = true;
                        AttackerCard = Target;              
                        Debug.LogError("İLK KART SEÇİLDİ");

                       Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "First card selected.";
                        return;

                    }
                    break;

                case "Nemean Lion":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Hydra":
                    //DamageCardsAround(1);
                    DamageAround(1);
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
                    if (GetComponent<PlayerController>().PV.IsMine)
                    {
                        int AttackerCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, AttackerCard.transform.parent.gameObject);
                        AttackerCard.GetComponent<CardInformation>().FirstDamageTaken = false;
                        GetComponent<PlayerController>().RefreshMyCard(AttackerCardIndex, 
                            AttackerCard.GetComponent<CardInformation>().CardHealth,
                            AttackerCard.GetComponent<CardInformation>().HaveShield,
                            AttackerCard.GetComponent<CardInformation>().CardDamage,
                            AttackerCard.GetComponent<CardInformation>().DivineSelected,
                            AttackerCard.GetComponent<CardInformation>().FirstTakeDamage, 
                            AttackerCard.GetComponent<CardInformation>().FirstDamageTaken);
                    }
                    break;

                case "Oracle's Emissary":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;

                case "Lightning Forger":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR 
                    break;
                case "Zeus":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Mongol Messenger":                                                                                  //genghis cards
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Khan’s Envoy":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Mongol Archer":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Steppe Warlord":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Nomadic Scout":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Keshik Cavalry":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Mongol Shaman":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Eagle Hunter":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Yurt Builder":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Mongol Lancer":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Horse Breeder":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Flaming Camel":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Kublai Khan":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "(General) Subutai":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Marco Polo":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
                case "Genghis":
                    TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString();
                    break;
            }
        }
       

     //   TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - AttackerInfo.CardDamage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR -- Centaur Archer - Minotaur Warrior - Greek Hoplite


        /*if (TargetInfo.CardDamage < 3)
       {
           SecoundTargetCard = true;
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

        if(GetComponent<PlayerController>().PV.IsMine)
        {
            GetComponent<PlayerController>().SetMana(AttackerCard);
        }
        RefreshCardDatas();
        AttackerCard = null;
        TargetCard = null;
        SecoundTargetCard = false;
        TargetCardIndex = -1;
        /*if(AttackerCard.GetComponent<CardInformation>().CardName=="Siren")
        {
            if (TargetInfo.CardDamage < 3)
            {
                GetComponent<CardProgress>().SecoundTargetCard = true;
                AttackerCard = Target;
                Debug.LogError("İLK KART SEÇİLDİ");
            }
        } */
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

    public void DamageAround(int damage)
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("CompetitorCard");

        foreach (GameObject target in allTargets)
        {
            if (target != TargetCard)
            {
                float distance = Vector3.Distance(TargetCard.transform.position, target.transform.position);
                if (distance <= 0.4f)
                {
                    Vector3 directionToTarget = (target.transform.position - TargetCard.transform.position).normalized;

                    float dotProductRight = Vector3.Dot(TargetCard.transform.right, directionToTarget);
                    float dotProductLeft = Vector3.Dot(-TargetCard.transform.right, directionToTarget);

                    if (dotProductRight > 0.5f || dotProductLeft > 0.5f )
                    {
                        Debug.Log("Nearobject" + " " +target.GetComponent<CardInformation>().CardName);
                        target.GetComponent<CardInformation>().CardHealth = (int.Parse(target.GetComponent<CardInformation>().CardHealth) - damage).ToString(); 
                        int NearbyCardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, target.transform.parent.gameObject);

                        GetComponent<PlayerController>().RefreshUsedCard(NearbyCardIndex, target.GetComponent<CardInformation>().CardHealth); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

                        if (int.Parse(target.GetComponent<CardInformation>().CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET
                        {

                            GetComponent<PlayerController>().DeleteAreaCard(NearbyCardIndex);

                        }
                    }
                }
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
                int CardIndex = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions, Card.transform.parent.gameObject);
                Card.GetComponent<CardInformation>().CardFreeze = true;
                GetComponent<PlayerController>().RefreshCompotitorCard(CardIndex, Card.GetComponent<CardInformation>().FirstTakeDamage, Card.GetComponent<CardInformation>().CardFreeze);
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
        Debug.LogError("SİREN HASAR VERDİ!");

        Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "The siren did damage.";


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
        int damage = 1;
        if (TargetInfo.CardHealth != "") // BU BİR MİNYON
        {
            if (GetComponent<PlayerController>().PV.IsMine)
            {
                damage += GetComponent<PlayerController>().SpellsExtraDamage;
            }
            TargetInfo.CardHealth = (int.Parse(TargetInfo.CardHealth) - damage).ToString(); // KARTIN DAMAGİNİ VURUR VURUYOR

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
                if(GetComponent<PlayerController>().PV.IsMine)
                {
                    RandomDamage += GetComponent<PlayerController>().SpellsExtraDamage;
                }
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


    void GoldenFleece(CardInformation Target)
    {
        Debug.LogError("Golden Fleece seçildi!");

       Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform).transform.GetChild(0).GetComponent<Text>().text = "Golden Fleece selected.";


        if (Target.CardHealth != "") // BU BİR MİNYON
        {
            Target.CardHealth = (int.Parse(Target.CardHealth) + 5).ToString(); //5 can basıyor

            RefreshMyCardDatas(TargetInfo.HaveShield, TargetInfo.CardDamage, TargetInfo.DivineSelected,TargetInfo.FirstTakeDamage,TargetInfo.FirstDamageTaken);
        }

        ForMyCard = false;
        Destroy(AttackerCard);
        SecoundTargetCard = false;
        AttackerCard = null;
        TargetCard = null;
        TargetCardIndex = -1;
    }




    void RefreshCardDatas()
    {
        GetComponent<PlayerController>().RefreshUsedCard(TargetCardIndex, TargetInfo.CardHealth); // DAMAGE YİYEN KARTIN BİLGİLERİNİ GÜNCELLE

        if (int.Parse(TargetInfo.CardHealth) <= 0) // KART ÖLDÜ MÜ KONTROL ET 
        {

            GetComponent<PlayerController>().DeleteAreaCard(TargetCardIndex);

        }

    }

    void RefreshMyCardDatas(bool haveshield,int damage,bool divineselected,bool firstdamage,bool firstdamagetaken)
    {
        GetComponent<PlayerController>().RefreshMyCard(TargetCardIndex, TargetInfo.CardHealth,haveshield, damage, divineselected,firstdamage,firstdamagetaken); // Can alan kartı güncelle       
    }


}
